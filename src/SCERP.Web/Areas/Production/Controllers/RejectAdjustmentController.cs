using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class RejectAdjustmentController : BaseController
    {
        private readonly IOmBuyerManager _buyerManager;
        private readonly IRejectAdjustmentManager _rejectAdjustmentManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyleManager;
        private readonly ICuttingSequenceManager _cuttingSequenceManager;
        private readonly ICuttingBatchManager _cuttingBatchManager;
        public RejectAdjustmentController(ICuttingBatchManager cuttingBatchManager, ICuttingSequenceManager cuttingSequenceManager, IOmBuyOrdStyleManager buyOrdStyleManager, IRejectAdjustmentManager rejectAdjustmentManager, IOmBuyerManager buyerManager)
        {
            _rejectAdjustmentManager = rejectAdjustmentManager;
            _buyerManager = buyerManager;
            _buyOrdStyleManager = buyOrdStyleManager;
            _cuttingSequenceManager = cuttingSequenceManager;
            _cuttingBatchManager = cuttingBatchManager;
        }
        [AjaxAuthorize(Roles = "rejectadjustment-1,rejectadjustment-2,rejectadjustment-3")]
        public ActionResult Index(RejectAdjustmentViewModel model)
        {
            ModelState.Clear();
            if (model.IsSearch)
            {
                if (!String.IsNullOrEmpty(model.CuttingBatch.CuttingBatchRefId))
                {
                    var cuttingBatch =
                        _cuttingBatchManager.GetCuttingBatchByCuttingBatchId(model.CuttingBatch.CuttingBatchRefId);
                    if (cuttingBatch != null)
                    {
                        model.CuttingBatch.CuttingBatchId = cuttingBatch.CuttingBatchId;
                    }
                    else
                    {
                        return ErrorResult("Invalid Ref No.");
                    }
                }
                if (model.CuttingBatch.CuttingBatchId>0)
                {
                    model.RejectAdjustmentDictionary = _rejectAdjustmentManager.GetRejectAdjustmentByCuttingBatch(model.CuttingBatch.CuttingBatchId);
                    var sizeList = model.RejectAdjustmentDictionary.Where(x => x.Key == "SizeRefId").Select(x => x.Value).First().ToList();
                    var quantityList = model.RejectAdjustmentDictionary.Where(x => x.Key == "Reject").Select(x => x.Value).First().ToList();
                    foreach (var size in sizeList.Select((x, i) => new { Value = x, Index = i }))
                    {
                        int rejectQty = Convert.ToInt32(quantityList[size.Index]);
                        model.RejectAdjustments.Add(size.Value, new PROD_RejectAdjustment()
                        {
                            SizeRefId = size.Value,
                            CuttingBatchId = model.CuttingBatch.CuttingBatchId,
                            CompId = PortalContext.CurrentUser.CompId,
                            RejectQty = rejectQty
                        });
                    }
                }
                else
                {
                    return ErrorResult("Search by Ref No or Job no.");
                }
                model.OrderList = _buyOrdStyleManager.GetOrderByBuyer(model.CuttingBatch.BuyerRefId);
                model.Styles = _buyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.CuttingBatch.OrderNo);
                model.Colors = _buyOrdStyleManager.GetColorsByOrderStyleRefId(model.CuttingBatch.OrderStyleRefId);
                model.Components = _cuttingSequenceManager.GetComponentsByColor(model.CuttingBatch.ColorRefId, model.CuttingBatch.OrderStyleRefId);
                model.CuttingBatches = _cuttingBatchManager.GetJobNoByComponentRefId(model.CuttingBatch.ColorRefId, model.CuttingBatch.ComponentRefId, model.CuttingBatch.OrderStyleRefId);
            }
            else
            {
                model.IsSearch = true;
            }
            model.Buyers = _buyerManager.GetCuttingProcessStyleActiveBuyers() as IEnumerable;
            return View(model);
        }
        [AjaxAuthorize(Roles = "rejectadjustment-2,rejectadjustment-3")]
        public ActionResult Save(RejectAdjustmentViewModel model)
        {
            int saveIndex = 0;
            List<PROD_RejectAdjustment> rejectAdjustments = model.RejectAdjustments.Select(x => x.Value).Where(x => x.RejectQty != 0).ToList();
            if (rejectAdjustments.Any())
            {

                saveIndex = _rejectAdjustmentManager.SaveRejectAdjustment(rejectAdjustments);
            }
            else
            {
                return ErrorResult("Rejected Quantity is Missing");
            }
            if (saveIndex > 0)
            {
                ModelState.Clear();
                model.RejectAdjustmentDictionary = _rejectAdjustmentManager.GetRejectAdjustmentByCuttingBatch(model.CuttingBatch.CuttingBatchId);
                var sizeList = model.RejectAdjustmentDictionary.Where(x => x.Key == "SizeRefId").Select(x => x.Value).First().ToList();
                model.RejectAdjustments = new Dictionary<string, PROD_RejectAdjustment>();
                var quantityList = model.RejectAdjustmentDictionary.Where(x => x.Key == "Reject").Select(x => x.Value).First().ToList();
                foreach (var size in sizeList.Select((x, i) => new { Value = x, Index = i }))
                {
                    int rejectQty = Convert.ToInt32(quantityList[size.Index]);
                    model.RejectAdjustments.Add(size.Value, new PROD_RejectAdjustment()
                    {
                        SizeRefId = size.Value,
                        CuttingBatchId = model.CuttingBatch.CuttingBatchId,
                        CompId = PortalContext.CurrentUser.CompId,
                        RejectQty = rejectQty
                    });
                }
                return PartialView("~/Areas/Production/Views/RejectAdjustment/_RejectAdjustmentDtl.cshtml", model);
            }
            else
            {
                return ErrorResult("Failed To Save");
            }

        }

        public ActionResult Delete(RejectAdjustmentViewModel model)
        {
            int deletedIndex = _rejectAdjustmentManager.DeleteRejectAjsustment(model.CuttingBatch.CuttingBatchId);
            return deletedIndex > 0 ? Reload() : ErrorResult("Failed To Delete RejectAdustment");
        }

        public JsonResult GetJobNoByComponentRefId(string colorRefId, string componentRefId, string orderStyleRefId)
        {
            var jobNoList = _cuttingBatchManager.GetJobNoByComponentRefId(colorRefId, componentRefId, orderStyleRefId);
            return Json(jobNoList, JsonRequestBehavior.AllowGet);
        }

    }
}