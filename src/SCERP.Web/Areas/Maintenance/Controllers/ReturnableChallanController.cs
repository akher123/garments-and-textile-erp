using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMaintenance;
using SCERP.Common;
using SCERP.Model.Maintenance;
using SCERP.Web.Areas.Maintenance.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Maintenance.Controllers
{
    public class ReturnableChallanController : BaseController
    {
        private readonly IReturnableChallanManager _returnableChallanManager;
        private readonly IReturnableChallanDetailManager _returnableChallanDetailManager;
        public ReturnableChallanController(IReturnableChallanManager returnableChallanManager, IReturnableChallanDetailManager returnableChallanDetailManager)
        {
            _returnableChallanManager = returnableChallanManager;
            _returnableChallanDetailManager = returnableChallanDetailManager;
        }
        [AjaxAuthorize(Roles = "returnablechallan-1,returnablechallan-2,returnablechallan-3")]
        public ActionResult Index(ReturnableChallanViewModel model)
        {
            ModelState.Clear();
            try
            {
                var totalRecords = 0;

                model.ReturnableChallans = _returnableChallanManager.GetAllReturnableChallanByPaging(model.PageIndex, model.sort, model.sortdir, model.DateFrom, model.DateTo, model.ChallanStatus, model.SearchString, PortalContext.CurrentUser.CompId,ChallanType.Maintenance, out totalRecords);
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
        [AjaxAuthorize(Roles = "returnablechallan-2,returnablechallan-3")]
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
                    const string maintainanceSpPreefix = "PF";
                    model.ReturnableChallan.ChallanDate = DateTime.Now;
                    model.ReturnableChallan.ReturnableChallanRefId = _returnableChallanManager.GetReturnableChallanRefId(ChallanType.Maintenance, maintainanceSpPreefix);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }

            return View(model);
        }
        [AjaxAuthorize(Roles = "returnablechallan-1,returnablechallan-2,returnablechallan-3")]
        public ActionResult AddNewRow([Bind(Include = "ReturnableChallanDetail")]ReturnableChallanViewModel model)
        {

            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.ReturnableChallanDictionary.Add(model.Key, model.ReturnableChallanDetail);
            return PartialView("~/Areas/Maintenance/Views/ReturnableChallan/_AddNewRow.cshtml", model);
        }
        [AjaxAuthorize(Roles = "returnablechallan-2,returnablechallan-3")]
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
                    RollQty = 0,
                    BatchNo = "--",
                    Buyer = "--",
                    OrderNo = "--",
                    StyleNo = "--",
                    Color = "--",
                    Remarks = x.Remarks
                }).ToList();
                if (model.ReturnableChallan.Maintenance_ReturnableChallanDetail.Count > 0)
                {
                    if (model.ReturnableChallan.ReturnableChallanId>0)
                    {
                        savedIndex = _returnableChallanManager.EditReturnableChallan(model.ReturnableChallan);
                      
                    }
                    else
                    {
                        const string maintainanceSpPreefix = "PF";
                        model.ReturnableChallan.ChllanType = ChallanType.Maintenance;
                        model.ReturnableChallan.ReturnableChallanRefId = _returnableChallanManager.GetReturnableChallanRefId(ChallanType.Maintenance, maintainanceSpPreefix);   
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
        [AjaxAuthorize(Roles = "returnablechallan-3")]
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

        public ActionResult ApprovedReturnableChallanList(ReturnableChallanViewModel model)
        {
           
            var totalRecords = 0;
            model.ReturnableChallans = _returnableChallanManager.GetApprovedReturnableChallanByPaging(model.PageIndex, model.sort, model.sortdir, model.ReturnableChallan.IsApproved??false, PortalContext.CurrentUser.CompId,ChallanType.Maintenance, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult ApprovedReturnableChallan(long returnableChallanId)
        {
            int index = 0;
            index = _returnableChallanManager.ApprovedReturnableChallan(returnableChallanId,PortalContext.CurrentUser.CompId);
            return index > 0 ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }

    }
}