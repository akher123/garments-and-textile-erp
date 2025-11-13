using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMaintenance;
using SCERP.Common;
using SCERP.Model.Maintenance;
using SCERP.Web.Areas.Maintenance.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Maintenance.Controllers
{
    public class ReturnableChallanReceiveController : BaseController
    {
        private readonly IReturnableChallanReceiveManager _returnableChallanReceiveManager;
        private readonly IReturnableChallanManager _returnableChallanManager;
        private readonly IReturnableChallanDetailManager _returnableChallanDetailManager;

        public ReturnableChallanReceiveController(IReturnableChallanReceiveManager returnableChallanReceiveManager, IReturnableChallanManager returnableChallanManager, IReturnableChallanDetailManager returnableChallanDetailManager)
        {
            _returnableChallanReceiveManager = returnableChallanReceiveManager;
            _returnableChallanManager = returnableChallanManager;
            _returnableChallanDetailManager = returnableChallanDetailManager;
        }
        [AjaxAuthorize(Roles = "returnablechallanreceive-1,returnablechallanreceive-2,returnablechallanreceive-3")]
        public ActionResult Index(ReturnableChallanReceiveViewModel model)
        {
             ModelState.Clear();
            if (model.IsSearch)
            {
                if (model.SearchString !=null)
                {
                    model.ReturnableChallan = _returnableChallanManager.GetReturnableChallanByReturnableChallanId(model.ReturnableChallan.ReturnableChallanId, PortalContext.CurrentUser.CompId);
                    List<VwReturnableChallanReceive> returnableChallanReceives = _returnableChallanReceiveManager.GetAllReturnableChallanReceiveByReturnableChallanId(model.ReturnableChallan.ReturnableChallanId, PortalContext.CurrentUser.CompId);
                    model.VwReturnableChallanReceives = returnableChallanReceives;
                    model.ReturnableChallanReceive.ReceiveDate = DateTime.Now; 
                }
                else
                {
                    return ErrorResult("Enter Ref Id.");
                }
            }
            else
            {
                model.IsSearch = true;
            }
            return View(model);
        }

       [AjaxAuthorize(Roles = "returnablechallanreceive-2,returnablechallanreceive-3")] 
        public ActionResult Save(ReturnableChallanReceiveViewModel model)
        {
            ModelState.Clear();
            int index = 0;
            if (model.ReturnableChallanReceive.ReturnableChallanDetailId==0)
            {
                return ErrorResult("Select Description.");   
            }
            Maintenance_ReturnableChallanDetail detail = _returnableChallanDetailManager.GetDetailByReturnableChallanDetailId(model.ReturnableChallanReceive.ReturnableChallanDetailId,PortalContext.CurrentUser.CompId);
            Maintenance_ReturnableChallanReceive receive = _returnableChallanReceiveManager.GetChallanReceiveByReturnableChallanReceiveId(model.ReturnableChallanReceive.ReturnableChallanReceiveId);
            double receiveQty = 0;
            if (receive !=null)
            {
                receiveQty = receive.ReceiveQty;
            }
            if (detail.DeliveryQty - (detail.ReceiveQty + (model.ReturnableChallanReceive.ReceiveQty - receiveQty)) >= 0)
            {
                index = model.ReturnableChallanReceive.ReturnableChallanReceiveId > 0 ? _returnableChallanReceiveManager.EditReturnableChallanRecieve(model.ReturnableChallanReceive) : _returnableChallanReceiveManager.SaveReturnableChallanReceive(model.ReturnableChallanReceive);
            }
            else
            {
                return ErrorResult("Receive Quantity can not greater than  Delivery Quantiry.");
            }
            if (index > 0)
            {
                Maintenance_ReturnableChallanReceive challanReceive = _returnableChallanReceiveManager.GetChallanReceiveByReturnableChallanReceiveId(model.ReturnableChallanReceive.ReturnableChallanReceiveId);
                model.ReturnableChallanReceive.ReturnableChallanDetailId = challanReceive.ReturnableChallanDetailId;
                model.ReturnableChallanReceive.ReceiveDate = challanReceive.ReceiveDate;
                model.ReturnableChallanReceive.ReceiveQty = challanReceive.ReceiveQty;
                model.ReturnableChallanReceive.ReturnableChallanReceiveId = 0;
                model.ReturnableChallan = _returnableChallanManager.GetReturnableChallanByReturnableChallanId(model.ReturnableChallan.ReturnableChallanId, PortalContext.CurrentUser.CompId);
                List<VwReturnableChallanReceive> returnableChallanReceives = _returnableChallanReceiveManager.GetAllReturnableChallanReceiveByReturnableChallanId(model.ReturnableChallan.ReturnableChallanId, PortalContext.CurrentUser.CompId);
                model.VwReturnableChallanReceives = returnableChallanReceives;
                model.VwReceiveDetails = _returnableChallanReceiveManager.GetChallanReceiveByDetailId(challanReceive.ReturnableChallanDetailId, PortalContext.CurrentUser.CompId);
                model.ReturnableChallanDetail = _returnableChallanDetailManager.GetDetailByReturnableChallanDetailId(model.ReturnableChallanReceive.ReturnableChallanDetailId, PortalContext.CurrentUser.CompId);
                return PartialView("~/Areas/Maintenance/Views/ReturnableChallanReceive/_FindChallanReceive.cshtml", model);
            }
            else
            {
                return ErrorResult("Failed To Save/Edit"); 
            }
        }

        [AjaxAuthorize(Roles = "returnablechallanreceive-1,returnablechallanreceive-2,returnablechallanreceive-3")] 
        public JsonResult GetRefNoBySearchCharacter(string searchCharacter)
        {
            var returnableChallans = _returnableChallanManager.GetRefNoBySearchCharacter(searchCharacter,ChallanType.Maintenance);
            return Json(returnableChallans, JsonRequestBehavior.AllowGet);
        }
        [AjaxAuthorize(Roles = "returnablechallanreceive-2,returnablechallanreceive-3")] 
        public ActionResult FindChallanReceive(ReturnableChallanReceiveViewModel model)
        {
            ModelState.Clear();
            model.ReturnableChallan = _returnableChallanManager.GetReturnableChallanByReturnableChallanId(model.ReturnableChallan.ReturnableChallanId, PortalContext.CurrentUser.CompId);
            List<VwReturnableChallanReceive> returnableChallanReceives = _returnableChallanReceiveManager.GetAllReturnableChallanReceiveByReturnableChallanId(model.ReturnableChallan.ReturnableChallanId, PortalContext.CurrentUser.CompId);
            model.VwReturnableChallanReceives = returnableChallanReceives;

            model.VwReceiveDetails = _returnableChallanReceiveManager.GetChallanReceiveByDetailId(model.ReturnableChallanReceive.ReturnableChallanDetailId, PortalContext.CurrentUser.CompId);
            model.ReturnableChallanDetail =_returnableChallanDetailManager.GetDetailByReturnableChallanDetailId(model.ReturnableChallanReceive.ReturnableChallanDetailId, PortalContext.CurrentUser.CompId);
            model.ReturnableChallanReceive.ReceiveDate = DateTime.Now;

            return PartialView("~/Areas/Maintenance/Views/ReturnableChallanReceive/_FindChallanReceive.cshtml", model);
        }
        [AjaxAuthorize(Roles = "returnablechallanreceive-2,returnablechallanreceive-3")] 
        public ActionResult Edit(ReturnableChallanReceiveViewModel model)
        {
            ModelState.Clear();
            model.ReturnableChallan = _returnableChallanManager.GetReturnableChallanByReturnableChallanId(model.ReturnableChallan.ReturnableChallanId, PortalContext.CurrentUser.CompId);
            List<VwReturnableChallanReceive> returnableChallanReceives = _returnableChallanReceiveManager.GetAllReturnableChallanReceiveByReturnableChallanId(model.ReturnableChallan.ReturnableChallanId, PortalContext.CurrentUser.CompId);
            model.VwReturnableChallanReceives = returnableChallanReceives;

            Maintenance_ReturnableChallanReceive challanReceive = _returnableChallanReceiveManager.GetChallanReceiveByReturnableChallanReceiveId(model.ReturnableChallanReceive.ReturnableChallanReceiveId);
            model.ReturnableChallanReceive.ReturnableChallanDetailId = challanReceive.ReturnableChallanDetailId;
            model.ReturnableChallanReceive.ChallanNo = challanReceive.ChallanNo;
            model.ReturnableChallanReceive.ReceiveDate = challanReceive.ReceiveDate;
            model.ReturnableChallanReceive.ReceiveQty = challanReceive.ReceiveQty;
            model.ReturnableChallanReceive.RejectQty = challanReceive.RejectQty;
            model.ReturnableChallanReceive.Amount = challanReceive.Amount;

            model.VwReceiveDetails = _returnableChallanReceiveManager.GetChallanReceiveByDetailId(challanReceive.ReturnableChallanDetailId, PortalContext.CurrentUser.CompId);
            model.ReturnableChallanDetail = _returnableChallanDetailManager.GetDetailByReturnableChallanDetailId(model.ReturnableChallanReceive.ReturnableChallanDetailId, PortalContext.CurrentUser.CompId);
            return PartialView("~/Areas/Maintenance/Views/ReturnableChallanReceive/_FindChallanReceive.cshtml", model);
        }
        [AjaxAuthorize(Roles = "returnablechallanreceive-3")] 
        public ActionResult Delete(ReturnableChallanReceiveViewModel model)
        {
            ModelState.Clear();
            int index = _returnableChallanReceiveManager.DeleteReturnableChallanReceive(model.ReturnableChallanReceive);
            if (index > 0)
            {
                ModelState.Clear();
                model.VwReceiveDetails = _returnableChallanReceiveManager.GetChallanReceiveByDetailId(model.ReturnableChallanReceive.ReturnableChallanDetailId, PortalContext.CurrentUser.CompId);
                model.ReturnableChallanReceive.ReceiveDate = DateTime.Now;
                model.ReturnableChallanReceive.ReceiveQty = 0;
                model.ReturnableChallanReceive.ReturnableChallanReceiveId = 0;
                model.ReturnableChallan = _returnableChallanManager.GetReturnableChallanByReturnableChallanId(model.ReturnableChallan.ReturnableChallanId, PortalContext.CurrentUser.CompId);
                List<VwReturnableChallanReceive> returnableChallanReceives = _returnableChallanReceiveManager.GetAllReturnableChallanReceiveByReturnableChallanId(model.ReturnableChallan.ReturnableChallanId, PortalContext.CurrentUser.CompId);
                model.VwReturnableChallanReceives = returnableChallanReceives;
                model.ReturnableChallanDetail = _returnableChallanDetailManager.GetDetailByReturnableChallanDetailId(model.ReturnableChallanReceive.ReturnableChallanDetailId, PortalContext.CurrentUser.CompId);
                return PartialView("~/Areas/Maintenance/Views/ReturnableChallanReceive/_FindChallanReceive.cshtml", model);
            }
            else
            {
                return ErrorResult("Failed To Delete.");
            }
        }
	}
}