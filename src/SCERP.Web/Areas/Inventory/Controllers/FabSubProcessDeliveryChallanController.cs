using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.IMaintenance;
using SCERP.Common;
using SCERP.Model.Maintenance;
using SCERP.Web.Areas.Maintenance.Models;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class FabSubProcessDeliveryChallanController : BaseController
    {
        private readonly IReturnableChallanManager _returnableChallanManager;
        private readonly IReturnableChallanDetailManager _returnableChallanDetailManager;
        public FabSubProcessDeliveryChallanController(IReturnableChallanManager returnableChallanManager, IReturnableChallanDetailManager returnableChallanDetailManager)
        {
            _returnableChallanManager = returnableChallanManager;
            _returnableChallanDetailManager = returnableChallanDetailManager;
        }
        [AjaxAuthorize(Roles = "fabsubprocesschallan-1,fabsubprocesschallan-2,fabsubprocesschallan-3")]
        public ActionResult Index(ReturnableChallanViewModel model)
        {
            ModelState.Clear();
            try
            {
                var totalRecords = 0;

                model.ReturnableChallans = _returnableChallanManager.GetAllReturnableChallanByPaging(model.PageIndex, model.sort, model.sortdir, model.DateFrom, model.DateTo, model.ChallanStatus, model.SearchString, PortalContext.CurrentUser.CompId,ChallanType.Fabric, out totalRecords);
                model.TotalRecords = totalRecords;
              
                model.DateFrom = DateTime.Now;
                model.DateTo = DateTime.Now;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "fabsubprocesschallan-2,fabsubprocesschallan-3")]
        public ActionResult Edit(ReturnableChallanViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.ReturnableChallan.ReturnableChallanId > 0)
                {
                    model.ReturnableChallan = _returnableChallanManager.GetReturnableChallanByReturnableChallanId(model.ReturnableChallan.ReturnableChallanId, PortalContext.CurrentUser.CompId);
                    if (model.ReturnableChallan.IsApproved == true)
                    {
                        return ErrorResult("Can Not Posible To Edit");
                    }
                    List<Maintenance_ReturnableChallanDetail> returnableChallanDetailList = _returnableChallanDetailManager.GetReturnableChallanDetailByReturnableChallanId(model.ReturnableChallan.ReturnableChallanId, PortalContext.CurrentUser.CompId);
                    model.ReturnableChallanDictionary =returnableChallanDetailList.ToDictionary(x =>Convert.ToString(x.ReturnableChallanDetailId), x =>x);
                }
                else
                {
                    const string fabricSpPreefix = "FS";
                    model.ReturnableChallan.ChllanType = ChallanType.Maintenance;
                    model.ReturnableChallan.ChallanDate = DateTime.Now;
                    model.ReturnableChallan.ReturnableChallanRefId = _returnableChallanManager.GetReturnableChallanRefId(ChallanType.Fabric, fabricSpPreefix);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }

            return View(model);
        }
      [AjaxAuthorize(Roles = "fabsubprocesschallan-1,fabsubprocesschallan-2,fabsubprocesschallan-3")]
        public ActionResult AddNewRow([Bind(Include = "ReturnableChallanDetail")]ReturnableChallanViewModel model)
        {

            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.ReturnableChallanDictionary.Add(model.Key, model.ReturnableChallanDetail);
            return PartialView("~/Areas/Inventory/Views/FabSubProcessDeliveryChallan/_AddNewRow.cshtml", model);
        }
      [AjaxAuthorize(Roles = "fabsubprocesschallan-2,fabsubprocesschallan-3")]
        public ActionResult Save(ReturnableChallanViewModel model)
        {
            int savedIndex = 0;
            try
            {
            
                model.ReturnableChallan.Maintenance_ReturnableChallanDetail = model.ReturnableChallanDictionary.Select(x => x.Value).Select(x => new Maintenance_ReturnableChallanDetail()
                {
                    CompId = PortalContext.CurrentUser.CompId,
                    ItemName = x.ItemName,
                    Unit = x.Unit,
                    DeliveryQty = x.DeliveryQty,
                    ReceiveQty = x.ReceiveQty,
                    Remarks = x.Remarks,
                    RollQty = x.RollQty,
                    BatchNo = x.BatchNo,
                    Buyer = x.Buyer,
                    OrderNo = x.OrderNo,
                    StyleNo = x.StyleNo,
                    Color = x.Color
                }).ToList();
                if (model.ReturnableChallan.Maintenance_ReturnableChallanDetail.Count > 0)
                {
                    if (model.ReturnableChallan.ReturnableChallanId > 0)
                    {
                        savedIndex = _returnableChallanManager.EditReturnableChallan(model.ReturnableChallan);

                    }
                    else
                    {
                        const string fabricSpPreefix = "FS";
                        model.ReturnableChallan.ChllanType = ChallanType.Fabric;
                        model.ReturnableChallan.ReturnableChallanRefId = _returnableChallanManager.GetReturnableChallanRefId(ChallanType.Fabric, fabricSpPreefix);
                        savedIndex = _returnableChallanManager.SaveReturnableChallan(model.ReturnableChallan);
                    }
                }
                else
                {
                    return ErrorResult("Add atleast one item.");
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return savedIndex > 0 ? Reload() : ErrorResult("Failed To Save/Edit");
        }
        [AjaxAuthorize(Roles = "fabsubprocesschallan-3")]
        public ActionResult Delete(long returnableChallanId)
        {
            int deleted = 0;
            Maintenance_ReturnableChallan returnableChallan = _returnableChallanManager.GetReturnableChallanByReturnableChallanId(returnableChallanId, PortalContext.CurrentUser.CompId);
            if (returnableChallan.IsApproved == true)
            {
                return ErrorResult("Can Not Posible To Delete");
            }
            else
            {
                deleted = _returnableChallanManager.DeleteReturnableChallan(returnableChallanId, PortalContext.CurrentUser.CompId);
            }
           
            return deleted > 0 ? Reload() : ErrorResult("Failed To Delete Returnable Challan.");
        }

        [AjaxAuthorize(Roles = "fabsubprocessapproval-1,fabsubprocessapproval-2,fabsubprocessapproval-3")]
        public ActionResult ApprovedReturnableChallanList(ReturnableChallanViewModel model)
        {
            var totalRecords = 0;
            model.ReturnableChallans = _returnableChallanManager.GetApprovedReturnableChallanByPaging(model.PageIndex, model.sort, model.sortdir, model.ReturnableChallan.IsApproved ?? false, PortalContext.CurrentUser.CompId, ChallanType.Fabric, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        [AjaxAuthorize(Roles = "fabsubprocessapproval-1,fabsubprocessapproval-2,fabsubprocessapproval-3")]
        public ActionResult ApprovedReturnableChallan(long returnableChallanId)
        {
            int index = 0;
            index = _returnableChallanManager.ApprovedReturnableChallan(returnableChallanId,PortalContext.CurrentUser.CompId);
            return index > 0 ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FabSubProcessDeliveryChallanReport(ReturnableChallanViewModel model)
        {
            ModelState.Clear();
            var reportType = ReportType.PDF;
            List<VwReturnableChallan> returnableChallans = _returnableChallanManager.GetReturnableChallanForReport(model.ReturnableChallan.ReturnableChallanId, PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "FabricSubProcessReturnableChallan.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("FSPRCDSET", returnableChallans) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }
    }
}
	
