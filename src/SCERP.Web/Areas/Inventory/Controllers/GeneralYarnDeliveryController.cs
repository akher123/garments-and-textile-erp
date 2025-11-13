using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Humanizer;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.Model.CommonModel;
using SCERP.Web.Areas.Common.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;
using Spell;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class GeneralYarnDeliveryController : BaseController
    {

        private readonly IGatePassManager _gatePassManager;

        public GeneralYarnDeliveryController(IGatePassManager gatePassManager)
        {
            _gatePassManager = gatePassManager;

        }
        [AjaxAuthorize(Roles = "generalyarndelivery-1,generalyarndelivery-2,generalyarndelivery-3")]
        public ActionResult Index(GatePassViewModel model)
        {
            try
            {
                ModelState.Clear();
                var totalRecords = 0;
                string typeId = model.TypeId?? GatepassType.YarnType;
                model.TypeId = typeId;
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
        [AjaxAuthorize(Roles = "generalyarndelivery-2,generalyarndelivery-3")]
        public ActionResult Edit(GatePassViewModel model)
        {
            try
            {
                ModelState.Clear();
             
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

                    model.GatePass.TypeId = GatepassType.YarnType;
                    model.GatePass.RefId = _gatePassManager.GetGatePassRefId(PortalContext.CurrentUser.CompId, GatepassType.YarnType);
                    model.GatePass.ChallanDate = DateTime.Now;
                    model.GatePass.ChallanNo = model.GatePass.RefId;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                ErrorResult("Fail To Retrive Data" + exception);
            }
            return View(model);
        }

        public ActionResult AddNewRow([Bind(Include = "GatePassDetail")] GatePassViewModel model)
        {
            ModelState.Clear();
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.GatePassDetails.Add(model.Key, model.GatePassDetail);
            return View("~/Areas/Inventory/Views/GeneralYarnDelivery/_AddNewRow.cshtml", model);
        }

        [AjaxAuthorize(Roles = "generalyarndelivery-2,generalyarndelivery-3")]
        public ActionResult Save(GatePassViewModel model)
        {
            var index = 0;
            try
            {
                model.GatePass.GatePassDetail = model.GatePassDetails.Select(x => x.Value).ToList();
                model.GatePass.CompId = PortalContext.CurrentUser.CompId;
             
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

        [AjaxAuthorize(Roles = "generalyarndelivery-3")]
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

        [AjaxAuthorize(Roles = "generalyarndelivery-3")]
        public ActionResult ApprovedGeneralYarnChallanById(int gatePassId)
        {
            int approved = _gatePassManager.ApprovedGatePass(gatePassId);
            return approved > 0 ? ErrorResult("Approved Successfully") : ErrorResult("Failed To Approved");
        }
        public ActionResult GeneralYarnDeliveryChallan(int gatePassId)
        {
            DataTable gatePassDataTbl = _gatePassManager.GatePassReport(gatePassId);

            double totalQty = gatePassDataTbl.AsEnumerable().Sum(x => x.Field<double>("Quantity"));
            //  string inWord = SpellAmount.InWrods(Convert.ToDecimal(totalQty)).NumberToWord();
            string inWord = totalQty.NumWordsWrapper().ToUpper();
            var reportParameters = new List<ReportParameter>() { new ReportParameter("InWord", inWord) };
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "GeneralYarnDeliveryChallan.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("GeneralYarnDeliveryChallanDSet", gatePassDataTbl) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = .2, MarginRight = .2, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, reportParameters);
        }


        [AjaxAuthorize(Roles = "generalyarndelivery-2,generalyarndelivery-3")]
        public ActionResult GeneralYarnReceiveWnd(GatePassViewModel model)
        {
            try
            {
                ModelState.Clear();
          
                model.GatePass = _gatePassManager.GetGatePassById(model.GatePass.GatePassId);
                if (model.GatePass.GatePassId > 0)
                {
                    string typeId = GatepassType.YarnType_Receive;
                    if (model.GatePass.TypeId == GatepassType.YarnType)
                    {
                        List<GatePassDetail> gatePassDetails = _gatePassManager.GetGatepassDetailById(model.GatePass.GatePassId).Select(x =>
                        {
                            x.ParentId = x.GatePassDetailId;
                            x.Remarks = String.Empty;
                            return x;
                        }).ToList();
                        model.GatePassDetails = gatePassDetails.ToDictionary(x => Convert.ToString(x.GatePassDetailId), x => x);
                        model.GatePass.RefId = _gatePassManager.GetGatePassRefId(PortalContext.CurrentUser.CompId, typeId);
                        model.GatePass.ChallanDate = null;
                        model.GatePass.GatePassId = 0;
                        model.GatePass.ChallanNo = String.Empty;
                        model.GatePass.Remarks = String.Empty;
                    }
                    else if (model.GatePass.TypeId == GatepassType.YarnType_Receive)
                    {
                        List<GatePassDetail> gatePassDetails = _gatePassManager.GetGatepassDetailById(model.GatePass.GatePassId).ToList();
                        model.GatePassDetails = gatePassDetails.ToDictionary(x => Convert.ToString(x.GatePassDetailId), x => x);
                    }
                }
               
                   
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                ErrorResult("Fail To Retrive Data" + exception);
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "generalyarndelivery-2,generalyarndelivery-3")]
        public ActionResult SaveGeneralYarnReceive(GatePassViewModel model)
        {
            var index = 0;
            try
            {
                model.GatePass.GatePassDetail = model.GatePassDetails.Select(x => x.Value).ToList();
                model.GatePass.CompId = PortalContext.CurrentUser.CompId;
                model.GatePass.TypeId = GatepassType.YarnType_Receive;
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

    }
}