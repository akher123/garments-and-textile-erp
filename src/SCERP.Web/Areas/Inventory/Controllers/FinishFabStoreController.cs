using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.InventoryModel;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class FinishFabStoreController : BaseController
    {
        private readonly IFinishFabStoreManager _finishFabStoreManager;
        private readonly IDyeingSpChallanManager _dyeingSpChallanManager;
        public FinishFabStoreController(IFinishFabStoreManager finishFabStoreManager, IDyeingSpChallanManager dyeingSpChallanManager)
        {
            _finishFabStoreManager = finishFabStoreManager;
            _dyeingSpChallanManager = dyeingSpChallanManager;
        }

        [AjaxAuthorize(Roles = "finishfabricreceive-1,finishfabricreceive-2,finishfabricreceive-3")]
        public ActionResult Index(FinishFabStoreViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            var compId = PortalContext.CurrentUser.CompId;
            model.FinishFabStores = _finishFabStoreManager.GetFinishFabricLsitByPaging(compId, model.SearchString,
                model.PageIndex, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "finishfabricreceive-2,finishfabricreceive-3")]
        public ActionResult Edit(FinishFabStoreViewModel model)
        {
            ModelState.Clear();
            if (model.FinishFabbricStore.FinishFabStoreId > 0)
            {
                model.FinishFabbricStore =  _finishFabStoreManager.GetFinishFabricById(model.FinishFabbricStore.FinishFabStoreId);
                model.FabDictionary = _finishFabStoreManager.GetDetailChallanBy(model.FinishFabbricStore.DyeingSpChallanId, model.FinishFabbricStore.FinishFabStoreId).ToDictionary(x => Convert.ToString(x.DyeingSpChallanDetailId), x => x);
            }
            else
            {
                model.FinishFabbricStore.FinishFabRefId =
                    _finishFabStoreManager.GetNewRefId(PortalContext.CurrentUser.CompId);
                model.FinishFabbricStore.GateEntryDate = DateTime.Now;
                model.FinishFabbricStore.InvoiceDate = DateTime.Now;
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "finishfabricreceive-2,finishfabricreceive-3")]
        public ActionResult Save(FinishFabStoreViewModel model)
        {
            try
            {
                var saved = 0;
                var compId = PortalContext.CurrentUser.CompId;
                model.FinishFabbricStore.PROD_DyeingSpChallan = null;
                model.FinishFabbricStore.Inventory_FinishFabDetailStore = model.FabDictionary.Select(x => x.Value).Where(x=>x.Qty>0).Select(x => new Inventory_FinishFabDetailStore()
                {
                    FinishFabStoreId =  model.FinishFabbricStore.FinishFabStoreId,
                    BatchId = x.BatchId,
                    BatchDetailId = x.BatchDetailId,
                    RcvQty = x.Qty,
                    CompId = compId,
                    GreyWt = x.GreyWt,
                    RejQty = 0,
                    CcuffQty = x.CcuffQty,
                    Remarks = x.Remarks,
                }).ToList();
                if (model.FinishFabbricStore.Inventory_FinishFabDetailStore.Any())
                {
                    if (model.FinishFabbricStore.FinishFabStoreId > 0)
                    {
                        model.FinishFabbricStore.EditedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();

                        saved = _finishFabStoreManager.EditFinishFabric(model.FinishFabbricStore);
                    }
                    else
                    {
                        model.FinishFabbricStore.FinishFabRefId =
                            _finishFabStoreManager.GetNewRefId(PortalContext.CurrentUser.CompId);
                        model.FinishFabbricStore.CratedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
                        model.FinishFabbricStore.CompId = compId;
                        saved = _finishFabStoreManager.SaveFinishFabric(model.FinishFabbricStore);
                    } 
                }
                else
                {
                    return ErrorResult("Missing Challan Details information!!");
                }
              
                return saved > 0 ? Reload() : ErrorResult("Save Failed");
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult("System Error : " + exception.Message);
            }


        }
        [AjaxAuthorize(Roles = "finishfabricreceive-3")]
        public ActionResult Delete(long finishFabStoreId)
        {
            try
            {
                int deleted = 0;
                deleted = _finishFabStoreManager.DeleteFinishFabric(finishFabStoreId);
                return deleted > 0 ? Reload() : ErrorResult("Delete  Failed");
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("System Error : " + exception.Message);
            }
        }
        [AjaxAuthorize(Roles = "finishfabricreceive-2,finishfabricreceive-3")]
        public ActionResult GetDetailChallan(long dyeingSpChallanId)
        {
            FinishFabStoreViewModel model = new FinishFabStoreViewModel();
            long finishFabStoreId = 0;
            model.FabDictionary =
                _finishFabStoreManager.GetDetailChallanBy(dyeingSpChallanId, finishFabStoreId)
                    .ToDictionary(x => Convert.ToString(x.DyeingSpChallanDetailId), x => x);
            return PartialView("~/Areas/Inventory/Views/FinishFabStore/_DetailChallan.cshtml", model);

        }
        
        public JsonResult DyeingSpChallanAutocomplite(string searchString)
        {
            IEnumerable dyeingSp = _dyeingSpChallanManager.DyeingSpChallanAutocomplite(searchString, PortalContext.CurrentUser.CompId);
            return Json(dyeingSp, JsonRequestBehavior.AllowGet);
        }
    }
}
