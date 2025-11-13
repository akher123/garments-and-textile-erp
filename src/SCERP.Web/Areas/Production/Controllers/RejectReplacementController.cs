using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class RejectReplacementController : BaseController
    {
        
        private readonly IOmBuyerManager _buyerManager;
        private readonly IRejectReplacementManager _rejectReplacementManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyleManager;
        private readonly ICuttingSequenceManager _cuttingSequenceManager;
        private readonly ICuttingBatchManager _cuttingBatchManager;
        private readonly IBuyOrdShipManager _buyOrdShipManager;
        public RejectReplacementController(IBuyOrdShipManager buyOrdShipManager,ICuttingBatchManager cuttingBatchManager, ICuttingSequenceManager cuttingSequenceManager, IOmBuyOrdStyleManager buyOrdStyleManager, IRejectReplacementManager rejectReplacementManager, IOmBuyerManager buyerManager)
        {
            _rejectReplacementManager = rejectReplacementManager;
            _buyerManager = buyerManager;
            _buyOrdStyleManager = buyOrdStyleManager;
            _cuttingSequenceManager = cuttingSequenceManager;
            _cuttingBatchManager = cuttingBatchManager;
            _buyOrdShipManager = buyOrdShipManager;
        }
       [AjaxAuthorize(Roles = "rejectionreplacement-1,rejectionreplacement-2,rejectionreplacement-3")]
        public ActionResult Index(RejectReplacementViewModel model)
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
                    model.RejectAdjustmentDictionary = _rejectReplacementManager.GetRejectReplacementByCuttingBatch(model.CuttingBatch.CuttingBatchId);
                    var sizeList = model.RejectAdjustmentDictionary.Where(x => x.Key == "SizeRefId").Select(x => x.Value).First().ToList();
                    var quantityList = model.RejectAdjustmentDictionary.Where(x => x.Key == "Reject").Select(x => x.Value).First().ToList();
                    foreach (var size in sizeList.Select((x, i) => new { Value = x, Index = i }))
                    {
                        int rejectQty = Convert.ToInt32(quantityList[size.Index]);
                        model.RejectAdjustments.Add(size.Value, new PROD_RejectReplacement()
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
                model.CuttingBatches = _cuttingBatchManager.GetJobNoByComponentRefId(model.CuttingBatch.ColorRefId, model.CuttingBatch.ComponentRefId, model.CuttingBatch.OrderStyleRefId,model.CuttingBatch.StyleRefId);
                model.OrderShips = _buyOrdShipManager.GetStyleWiseShipment(model.CuttingBatch.OrderStyleRefId, PortalContext.CurrentUser.CompId);
            }
            else
            {
                model.IsSearch = true;
            }
            model.Buyers = _buyerManager.GetCuttingProcessStyleActiveBuyers() as IEnumerable;
            return View(model);
        }
        [AjaxAuthorize(Roles = "rejectionreplacement-2,rejectionreplacement-3")]
        public ActionResult Save(RejectReplacementViewModel model)
        {
            int saveIndex = 0;
            List<PROD_RejectReplacement> rejectAdjustments = model.RejectAdjustments.Select(x => x.Value).Where(x => x.RejectQty != 0).ToList();
            if (rejectAdjustments.Any())
            {

                saveIndex = _rejectReplacementManager.SaveRejectReplacement(rejectAdjustments);
            }
            else
            {
                return ErrorResult("Rejected Quantity is Missing");
            }
            if (saveIndex > 0)
            {
                ModelState.Clear();
                model.RejectAdjustmentDictionary = _rejectReplacementManager.GetRejectReplacementByCuttingBatch(model.CuttingBatch.CuttingBatchId);
                var sizeList = model.RejectAdjustmentDictionary.Where(x => x.Key == "SizeRefId").Select(x => x.Value).First().ToList();
                model.RejectAdjustments = new Dictionary<string, PROD_RejectReplacement>();
                var quantityList = model.RejectAdjustmentDictionary.Where(x => x.Key == "Reject").Select(x => x.Value).First().ToList();
                foreach (var size in sizeList.Select((x, i) => new { Value = x, Index = i }))
                {
                    int rejectQty = Convert.ToInt32(quantityList[size.Index]);
                    model.RejectAdjustments.Add(size.Value, new PROD_RejectReplacement()
                    {
                        SizeRefId = size.Value,
                        CuttingBatchId = model.CuttingBatch.CuttingBatchId,
                        CompId = PortalContext.CurrentUser.CompId,
                        RejectQty = rejectQty
                    });
                }
                return PartialView("~/Areas/Production/Views/RejectReplacement/_RejectReplacementDtl.cshtml", model);
            }
            else
            {
                return ErrorResult("Failed To Save");
            }

        }
       [AjaxAuthorize(Roles = "rejectionreplacement -3")]
        public ActionResult Delete(RejectAdjustmentViewModel model)
        {
            int deletedIndex = _rejectReplacementManager.DeleteRejectReplacement(model.CuttingBatch.CuttingBatchId);
            return deletedIndex > 0 ? Reload() : ErrorResult("Failed To Delete RejectReplacement");
        }

        public JsonResult GetJobNoByComponentRefId(string colorRefId, string componentRefId, string orderStyleRefId,string orderShipRefId = "")
        {
            var jobNoList = _cuttingBatchManager.GetJobNoByComponentRefId(colorRefId, componentRefId, orderStyleRefId, orderShipRefId);
            return Json(jobNoList, JsonRequestBehavior.AllowGet);
        }

    }
}