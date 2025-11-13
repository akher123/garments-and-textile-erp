using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.Common.Mail;
using SCERP.Model.CommonModel;
using SCERP.Web.Areas.Common.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class GreyDeliveryGatepassController : BaseController
    {
       
        private readonly IGatePassManager _gatePassManager;
        private readonly IEmailSendManager _emailSendManager;
        public GreyDeliveryGatepassController(IGatePassManager gatePassManager, IEmailSendManager emailSendManager)
        {
            _gatePassManager = gatePassManager;
            _emailSendManager = emailSendManager;
        }

        [AjaxAuthorize(Roles = " knitfabdelivery-1,knitfabdelivery-2,knitfabdelivery-3")]
        public ActionResult Index(GatePassViewModel model)
        {
            try
            {
                ModelState.Clear();
                var totalRecords = 0;
                string typeId = GatepassType.KnitFabType; //K for Knititng Grey Fabric Delivery gate pass
                model.GatePasses = _gatePassManager.GetGatePassByPaging(typeId, model.SearchString, PortalContext.CurrentUser.CompId, model.PageIndex, model.sort, model.sortdir, out totalRecords);
                model.TotalRecords = totalRecords;
           
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "knitfabdelivery-2,knitfabdelivery-3")]
        public ActionResult Edit(GatePassViewModel model)
        {
            try
            {
                ModelState.Clear();
                string typeId = GatepassType.KnitFabType; 
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
                    model.GatePass.RefId = _gatePassManager.GetGatePassRefId(PortalContext.CurrentUser.CompId, typeId);
                    model.GatePass.ChallanDate = DateTime.Now;
                    model.GatePass.ChallanNo = model.GatePass.RefId;
                    model.GatePassDetail.Wrapper = "Bag";
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                ErrorResult("Fail To Retrive Data" + exception);
            }
            return View(model);
        }

        public ActionResult AddNewRow([Bind(Include = "GatePassDetail")]GatePassViewModel model)
        {
            ModelState.Clear();
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.GatePassDetails.Add(model.Key, model.GatePassDetail);
            return View("~/Areas/Inventory/Views/GreyDeliveryGatepass/_AddNewRow.cshtml", model);
        }

        [AjaxAuthorize(Roles = "knitfabdelivery-2,knitfabdelivery-3")]
        public ActionResult Save(GatePassViewModel model)
        {
            var index = 0;
            try
            {
                model.GatePass.GatePassDetail = model.GatePassDetails.Select(x => x.Value).ToList();
                model.GatePass.CompId = PortalContext.CurrentUser.CompId;
                model.GatePass.TypeId = GatepassType.KnitFabType;
                    if (model.GatePass.GatePassDetail.Any())
                    {
                        if (model.GatePass.GatePassId > 0)
                        {
                            index = _gatePassManager.EditGatePass(model.GatePass);
                        }
                        else
                        {
                            model.GatePass.RefId = _gatePassManager.GetGatePassRefId(PortalContext.CurrentUser.CompId, model.GatePass.TypeId);
                           index=_gatePassManager.SaveGatePass(model.GatePass);
                        }
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
            return index > 0 ? Reload() : ErrorResult("Fail to Save/Edit Gate Pass !");
        }

        [AjaxAuthorize(Roles = "knitfabdelivery-3")]
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

        [AjaxAuthorize(Roles = "knitfabdelivery-3")]
        public ActionResult ApprovedGatePassById(int gatePassId)
        {
            int approved = _gatePassManager.ApprovedGatePass(gatePassId);
            return approved > 0 ? ErrorResult("Approved Successfully") : ErrorResult("Failed To Approved");
        }
        public ActionResult GatePassReport(int gatePassId)
        {
            DataTable gatePassDataTbl = _gatePassManager.GatePassReport(gatePassId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "KnittingFabricDeliveryGatePassReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("GatePassDSet", gatePassDataTbl) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = .2, MarginRight = .2, MarginBottom = .2};
            var reportExport = ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
            return reportExport;
        }
	}
}