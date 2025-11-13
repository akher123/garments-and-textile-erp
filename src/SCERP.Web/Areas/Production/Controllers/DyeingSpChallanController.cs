using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class DyeingSpChallanController : BaseController
    {
        private readonly IDyeingSpChallanManager _dyeingSpChallanManager;
        private readonly IDyeingSpChallanDetailManager _dyeingSpChallanDetailManager;
        private readonly IBatchManager _batchManager;
        private readonly IGroupSubProcessManager _groupSubProcessManager;
        private readonly IPartyManager partyManager;
        public DyeingSpChallanController(IPartyManager partyManager,IDyeingSpChallanManager dyeingSpChallanManager, IBatchManager batchManager, IGroupSubProcessManager groupSubProcessManager, IDyeingSpChallanDetailManager dyeingSpChallanDetailManager)
        {
            _dyeingSpChallanManager = dyeingSpChallanManager;
            _batchManager = batchManager;
            _groupSubProcessManager = groupSubProcessManager;
            _dyeingSpChallanDetailManager = dyeingSpChallanDetailManager;
            this.partyManager = partyManager;
        }
       [AjaxAuthorize(Roles = "dyeingsubprocess-1,dyeingsubprocess-2,dyeingsubprocess-3")]
        public ActionResult Index(DyeingSpChallanViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.DyeingSpChallans=_dyeingSpChallanManager.GetAllDyeingSpChallanByPaging(model.PageIndex, model.sort, model.sortdir, model.FromDate, model.ToDate, model.SearchString, PortalContext.CurrentUser.CompId, out totalRecords);
           model.TotalRecords = totalRecords;
           return View(model);
        }
           [AjaxAuthorize(Roles = "dyeingsubprocess-2,dyeingsubprocess-3")]
        public ActionResult Edit(DyeingSpChallanViewModel model)
        {
            ModelState.Clear();
           
            if (model.DyeingSpChallan.DyeingSpChallanId>0)
            {
                model.DyeingSpChallan = _dyeingSpChallanManager.GetDyeingSpChallanByDyeingSpChallanId(model.DyeingSpChallan.DyeingSpChallanId,PortalContext.CurrentUser.CompId);
                List<VwProdDyeingSpChallanDetail>  vwDyeingSpChallanDetail = _dyeingSpChallanDetailManager.GetDyeingSpChallanDetailByDyeingSpChallanId(model.DyeingSpChallan.DyeingSpChallanId);
                model.DyeingSpChallanDetailDictionary = vwDyeingSpChallanDetail.ToDictionary(x => Convert.ToString(x.DyeingSpChallanDetailId), x => x);
                PROD_DyeingSpChallanDetail dyeingSpChallanDetail =_dyeingSpChallanDetailManager.GetAnDyeingSpChallanDetailById(model.DyeingSpChallan.DyeingSpChallanId);
                model.Items= _batchManager.GetBachByBatchId(dyeingSpChallanDetail.BatchId, PortalContext.CurrentUser.CompId);
                
            }
            else
            {
                model.DyeingSpChallan.DyeingSpChallanRefId = _dyeingSpChallanManager.GetDyeingSpChallanRefId();
                model.DyeingSpChallan.ChallanDate = DateTime.Now;
                model.DyeingSpChallan.ExpDate = DateTime.Now;
            }
            const string partyType = "D";
            model.Parties = partyManager.GetParties(partyType);
            model.Batches = _batchManager.GetAllBatch(PortalContext.CurrentUser.CompId);
            model.GroupList = _groupSubProcessManager.GetAllGroupSubProcess(PortalContext.CurrentUser.CompId);
            return View(model);
        }
        public ActionResult AddNewRow([Bind(Include = "VDyeingSpChallanDetail")]DyeingSpChallanViewModel model)
        {
            ModelState.Clear();
            if (model.VDyeingSpChallanDetail.BatchDetailId <= 0 && model.VDyeingSpChallanDetail.BatchId <= 0)
            {
                return ErrorResult("Invalied Batch and Item ! Please Select a valid batch and item");
            }
            if (model.VDyeingSpChallanDetail.SpGroupId<=0)
            {
                return ErrorResult("Invalied Process name ! Please Select a valid Process");
            }

            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.DyeingSpChallanDetailDictionary.Add(model.Key, model.VDyeingSpChallanDetail);
            return PartialView("~/Areas/Production/Views/DyeingSpChallan/_AddNewRow.cshtml", model);
        }
       [AjaxAuthorize(Roles = "dyeingsubprocess-2,dyeingsubprocess-3")]
        public ActionResult Save(DyeingSpChallanViewModel model)
        {
            var index = 0;
            var errorMessage = "";
            try
            {
                var compId = PortalContext.CurrentUser.CompId;
                    model.DyeingSpChallan.PROD_DyeingSpChallanDetail =
                        model.DyeingSpChallanDetailDictionary.Select(x => x.Value).Select(x => new PROD_DyeingSpChallanDetail()
                        {
                            CompId = compId,
                            BatchId = x.BatchId,
                            BatchDetailId = x.BatchDetailId,
                            SpGroupId = x.SpGroupId,
                            GreyWeight = x.GreyWeight,
                            FinishWeight = x.FinishWeight,
                            Rate = x.Rate,
                            CcuffQty = x.CcuffQty,
                            Remarks = x.Remarks
                        }).ToList();
                    if (model.DyeingSpChallan.PROD_DyeingSpChallanDetail.Any())
                    {
                        if (model.DyeingSpChallan.DyeingSpChallanId>0)
                        {
                            index = _dyeingSpChallanManager.EditDyeingSpChallan(model.DyeingSpChallan);
                        }
                        else
                        {
                            model.DyeingSpChallan.DyeingSpChallanRefId = _dyeingSpChallanManager.GetDyeingSpChallanRefId();
                            model.DyeingSpChallan.CreatedBy = PortalContext.CurrentUser.UserId;
                            model.DyeingSpChallan.CompId = compId;
                            model.DyeingSpChallan.Party = null;
                            model.DyeingSpChallan.Inventory_FinishFabStore = null;
                            index = _dyeingSpChallanManager.SaveDyeingSpChallan(model.DyeingSpChallan);
                           
                        }
    
                    }
                    else
                    {
                        return ErrorResult("Add atlist one Dyeing Subprocess.");
                    }

             }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            model.DyeingSpChallan =
                new PROD_DyeingSpChallan() {PROD_DyeingSpChallanDetail = new List<PROD_DyeingSpChallanDetail>()};
            return index > 0 ? RedirectToAction("Index") : (ActionResult)ErrorResult("Failed to save Dyeing Subprocess ! " + errorMessage);
        }
        [AjaxAuthorize(Roles = "dyeingsubprocess-3")]
        public ActionResult Delete(long dyeingSpChallanId)
        {
            int deleted = 0;
            deleted = _dyeingSpChallanManager.DeleteDyeingSpChallan(dyeingSpChallanId, PortalContext.CurrentUser.CompId);
            return deleted > 0 ? Reload() : ErrorResult("Failed To Delete Dyeing Subprocess.");
        }
       
        public ActionResult GetBatchById(long batchId)
        { 
            var batch = _batchManager.GetBachByBatchId(batchId,PortalContext.CurrentUser.CompId);
            return Json(batch, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBatchItemQtyByBatchDetailId(long batchDetailId)
        {
            IEnumerable<dynamic> batchItemQty = _dyeingSpChallanManager.GetBatchItemQtyByBatchDetailId(batchDetailId, PortalContext.CurrentUser.CompId);
            var s = JsonConvert.SerializeObject(batchItemQty, Newtonsoft.Json.Formatting.Indented);
            return Json(s, JsonRequestBehavior.AllowGet);
        }
        
        [AjaxAuthorize(Roles = "approvedsubprocess-1,approvedsubprocess-2,approvedsubprocess-3")]
        public ActionResult ApprovedSp(DyeingSpChallanViewModel model)
        {
            ModelState.Clear();
            model.DyeingSpChallans = _dyeingSpChallanManager.GetAllDyeingSpChallanList(PortalContext.CurrentUser.CompId, model.IsActive);
            return View(model);
        }
        [AjaxAuthorize(Roles = "approvedsubprocess-2,approvedsubprocess-3")]
        public ActionResult UpdateApprovedSp(long dyeingSpChallanId)
        {
            int update = _dyeingSpChallanManager.UpdateApprovedSp(dyeingSpChallanId);
            return Json(update > 0, JsonRequestBehavior.AllowGet);
        }
    }
}