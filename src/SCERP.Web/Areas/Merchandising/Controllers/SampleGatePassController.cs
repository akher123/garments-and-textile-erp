using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Common.Mail;
using SCERP.Model.CommonModel;
using SCERP.Web.Areas.Common.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class SampleGatePassController : BaseController
    {
        private readonly IGatePassManager _gatePassManager;
        private readonly IEmailSendManager _emailSendManager;
        private readonly ISampleOrderManager _sampleOrderManager;
        public SampleGatePassController(ISampleOrderManager sampleOrderManager,IGatePassManager gatePassManager, IEmailSendManager emailSendManager)
        {
            _gatePassManager = gatePassManager;
            _emailSendManager = emailSendManager;
            _sampleOrderManager = sampleOrderManager;
        }
        public ActionResult Index(GatePassViewModel model)
        {
            try
            {
                ModelState.Clear();
                string typeId = GatepassType.SampleGatePass; //S for sample genereal
                model.GatePasses = _gatePassManager.GetGateSamplePass(typeId, model.SearchString, PortalContext.CurrentUser.CompId);
                model.TotalRecords = model.GatePasses.Count;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }

        public ActionResult Edit(GatePassViewModel model)
        {
            try
            {
                ModelState.Clear();
                string typeId = GatepassType.SampleGatePass; //S for genereal
             
                if (model.GatePass.GatePassId > 0)
                {
                    model.GatePass = _gatePassManager.GetGatePassById(model.GatePass.GatePassId);
                    if (model.GatePass.IsApproved)
                    {
                        return ErrorResult("This Gate Pass not allow to edite");
                    }
                    else
                    {
                        List<GatePassDetail> gatePassDetails = _gatePassManager.GetGatepassDetailById(model.GatePass.GatePassId);
                        model.GatePassDetails = gatePassDetails.ToDictionary(x => Convert.ToString(x.GatePassDetailId), x => x);
                    }

                }
                else
                {
                    var order = _sampleOrderManager.GetSampleOrderByRefId(model.SearchString);
                    model.GatePass.BillNo = model.SearchString;
                    model.GatePass.RefId = _gatePassManager.GetGatePassRefId(PortalContext.CurrentUser.CompId, typeId);
                    model.GatePass.ChallanDate = DateTime.Now;
                    model.GatePass.ChallanNo = model.GatePass.RefId;
                    model.GatePass.OrderName = order.OrderNo;
                    model.GatePass.StyleName = order.OrderNo;
                    model.GatePass.BuyerName = order.OM_Buyer.BuyerName;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                ErrorResult("Fail To Retrive Data" + exception);
            }
            return View(model);
        }

        public ActionResult Save(GatePassViewModel model)
        {
            var index = 0;
            try
            {
            
                model.GatePass.GatePassDetail = model.GatePassDetails.Select(x => x.Value).ToList();
                model.GatePass.CompId = PortalContext.CurrentUser.CompId;
                model.GatePass.TypeId = GatepassType.SampleGatePass;
                model.SearchString = model.GatePass.BillNo;
                if (model.GatePass.GatePassDetail.Any())
                {
                    if (model.GatePass.GatePassId > 0)
                    {
                        index = _gatePassManager.EditGatePass(model.GatePass);
                    }
                    else
                    {
                        model.GatePass.RefId = _gatePassManager.GetGatePassRefId(PortalContext.CurrentUser.CompId, model.GatePass.TypeId);
                        index = _gatePassManager.SaveGatePass(model.GatePass);
                    }
                    model.GatePasses = _gatePassManager.GetGateSamplePass(model.GatePass.TypeId, model.GatePass.BillNo, PortalContext.CurrentUser.CompId);
                    model.TotalRecords = model.GatePasses.Count;
                }
                else
                {
                    return ErrorResult("Please Add  Gate Pass Detail");
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit Gatepass:" + exception);
            }
      
            return PartialView("~/Areas/Merchandising/Views/SampleGatePass/Index.cshtml", model);
        }
        public ActionResult Delete(int gatePassId)
        {
            var index = 0;
            try
            {
                index = _gatePassManager.DeleteGatePass(gatePassId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail to Delete Gate Pass :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Delete Gate Pass !");
        }

        public ActionResult ApprovedGatePass(GatePassViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            string typeId = GatepassType.SampleGatePass;
            model.GatePasses = _gatePassManager.GetGatePassByPaging(typeId, model.SearchString, PortalContext.CurrentUser.CompId, model.PageIndex, model.sort, model.sortdir, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        public ActionResult ApprovedGatePassById(int gatePassId)
        {
            int approved = _gatePassManager.ApprovedGatePass(gatePassId);
            return approved > 0 ? ErrorResult("Approved Successfully") : ErrorResult("Failed To Approved");
        }

        public ActionResult GatePassReport(int gatePassId)
        {
            DataTable gatePassDataTbl = _gatePassManager.GatePassReport(gatePassId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "SampleGatePassReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("GatePassDSet", gatePassDataTbl) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = .2, MarginRight = .2, MarginBottom = .2 };
            var reportExport = ReportExtension.ToWhiteFile(ReportType.PDF, path, reportDataSources, deviceInformation);

            DbEmailModel dbEmail = _emailSendManager.GetDbEmailByTemplateId(EmailTemplateRefId.GENERELGETEPASS, PortalContext.CurrentUser.CompId);
            dbEmail.Subject = "GENERAL GATE PASS";
            dbEmail.Body = "GENEREL GATE PASS CREATD BY USER :" + PortalContext.CurrentUser.Name;
            dbEmail.FileAttachments = HostingEnvironment.MapPath(AppConfig.ExportReportFillPath + "." + ReportType.PDF);
            bool send = _emailSendManager.SendDbEmail(dbEmail);
            return reportExport;
        }

    }
}