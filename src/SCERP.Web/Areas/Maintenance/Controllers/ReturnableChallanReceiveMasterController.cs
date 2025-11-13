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
    public class ReturnableChallanReceiveMasterController : BaseController
    {
        private readonly IReturnableChallanReceiveMasterManager _challanReceiveMasterManager;
        private readonly IReturnableChallanManager _returnableChallanManager;
        private readonly IReturnableChallanReceiveManager _returnableChallanReceiveManager;

        public ReturnableChallanReceiveMasterController(IReturnableChallanReceiveMasterManager challanReceiveMasterManager, IReturnableChallanManager returnableChallanManager, IReturnableChallanReceiveManager returnableChallanReceiveManager)
        {
            _challanReceiveMasterManager = challanReceiveMasterManager;
            _returnableChallanManager = returnableChallanManager;
            _returnableChallanReceiveManager = returnableChallanReceiveManager;
        }
        [AjaxAuthorize(Roles = "returnablechallanreceive-1,returnablechallanreceive-2,returnablechallanreceive-3")]
        public ActionResult Index(ChallanReceiveMasterViewModel model)
        {
            ModelState.Clear();
            try
            {
                var totalRecords = 0;
                model.ChallanReceiveMasterList = _challanReceiveMasterManager.GetChallanReceiveMasterByPaging(model.PageIndex, model.sort, model.sortdir, model.SearchString, PortalContext.CurrentUser.CompId,ChallanType.Maintenance, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "returnablechallanreceive-2,returnablechallanreceive-3")] 
        public ActionResult FindReturnableChallan(ChallanReceiveMasterViewModel model)
        {
            if (model.ReturnableChallanId > 0)
            {
                model.ReturnableChallan = _returnableChallanManager.GetReturnableChallanByReturnableChallanId(model.ReturnableChallanId, PortalContext.CurrentUser.CompId);
                List<VwReturnableChallanReceive> returnableChallanReceives = _returnableChallanReceiveManager.GetAllReturnableChallanReceiveByReturnableChallanId(model.ReturnableChallan.ReturnableChallanId, PortalContext.CurrentUser.CompId);
                model.ChallanReceiveMaster.ReturnableChallanId = model.ReturnableChallanId;
                model.ChallanReceiveMaster.ReceiveDate = DateTime.Now;
                model.RChallanReceiveDictionary = returnableChallanReceives.ToDictionary(x => Convert.ToString(x.ReturnableChallanDetailId), x => x);
                return Json(new { returnableChallanReceive = RenderViewToString("_EditReturnableChallanReceive", model), Success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return ErrorResult("Not Found");
            }
        }
        [AjaxAuthorize(Roles = "returnablechallanreceive-2,returnablechallanreceive-3")] 
        public ActionResult Edit(ChallanReceiveMasterViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.ChallanReceiveMaster.ReturnableChallanReceiveMasterId > 0 )
                {
                    model.ChallanReceiveMaster = _challanReceiveMasterManager.GetChallanReceiveMasterByReturnableChallanReceiveMasterId(model.ChallanReceiveMaster.ReturnableChallanReceiveMasterId, PortalContext.CurrentUser.CompId);
                    model.ReturnableChallan = _returnableChallanManager.GetReturnableChallanByReturnableChallanId(model.ChallanReceiveMaster.ReturnableChallanId.GetValueOrDefault(), PortalContext.CurrentUser.CompId);
                    model.SearchString = model.ReturnableChallan.ReturnableChallanRefId;
                    List<VwChallanReceiveMaster> challanReceiveMasters = _returnableChallanReceiveManager.GetReturnableChallanReceiveByReturnableChallanReceiveMasterId(model.ChallanReceiveMaster.ReturnableChallanReceiveMasterId, PortalContext.CurrentUser.CompId);
                    List<VwReturnableChallanReceive> rvwReturnableChallanReceives= challanReceiveMasters.Select(x => new VwReturnableChallanReceive
                    {
                        ReturnableChallanReceiveId = x.ReturnableChallanReceiveId,
                        ReturnableChallanDetailId = x.ReturnableChallanDetailId,
                        ItemName = x.ItemName,
                        DeliveryQty = x.DeliveryQty, 
                        TotalReceiveQty = x.TotalReceiveQty, 
                        TotalRejectQty = x.TotalRejectQty,
                        RemainingQty = x.RemainingQty,
                        ReceiveQty = x.ReceiveQty,
                        RejectQty = x.RejectQty,
                        Amount = x.Amount
                    }).ToList();

                    model.RChallanReceiveDictionary = rvwReturnableChallanReceives.ToDictionary(x => Convert.ToString(x.ReturnableChallanReceiveId), x => x);
                    return View(model);
                }
                else
                {
                    model.ChallanReceiveMaster.ReceiveDate = DateTime.Now;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "returnablechallanreceive-2,returnablechallanreceive-3")] 
        public ActionResult Save(ChallanReceiveMasterViewModel model)
        {
            int index = 0;
            model.ChallanReceiveMaster.Maintenance_ReturnableChallanReceive = model.RChallanReceiveDictionary.Select(x => x.Value).Select(x => new Maintenance_ReturnableChallanReceive()
            {
                ReturnableChallanReceiveId=x.ReturnableChallanReceiveId,
                ReturnableChallanDetailId = x.ReturnableChallanDetailId,
                ReceiveDate=model.ChallanReceiveMaster.ReceiveDate,
                ReceiveQty = x.ReceiveQty,
                CompId = PortalContext.CurrentUser.CompId,
                RejectQty = x.RejectQty,
                ChallanNo = model.ChallanReceiveMaster.ChallanNo,
                Amount = x.Amount??0
            }).ToList();
            index = model.ChallanReceiveMaster.ReturnableChallanReceiveMasterId > 0 ? _challanReceiveMasterManager.EditChallanReceiveMaster(model.ChallanReceiveMaster) : _challanReceiveMasterManager.SaveChallanReceiveMaster(model.ChallanReceiveMaster);
            return index > 0 ? Reload() : ErrorResult("Failed To Save/Edit");
        }
        [AjaxAuthorize(Roles = "returnablechallanreceive-3")] 
        public ActionResult Delete(long returnableChallanReceiveMasterId)
        {
            int index = 0;
            index = _challanReceiveMasterManager.DeleteReturnableChallanReceiveMaster(returnableChallanReceiveMasterId,PortalContext.CurrentUser.CompId);
            return index > 0 ? Reload() : ErrorResult("Fail to Delete ");
        }
        public JsonResult GetRefNoBySearchkey(string searchCharacter) 
        {
             var returnableChallans = _returnableChallanManager.GetRefNoBySearchCharacter(searchCharacter,ChallanType.Maintenance);
            return Json(returnableChallans, JsonRequestBehavior.AllowGet);
        }
	}
}