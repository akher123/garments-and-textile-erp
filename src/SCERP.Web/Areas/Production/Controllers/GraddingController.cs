using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class GraddingController : BaseController
    {
        private readonly ICuttingBatchManager _cuttingBatchManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IGraddingManager _graddingManager;
        private readonly IBuyOrdStyleSizeManager _buyOrdStyleSizeManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyleManager;
        private readonly ICuttingSequenceManager _cuttingSequenceManager;
        public GraddingController(ICuttingSequenceManager cuttingSequenceManager,IOmBuyOrdStyleManager buyOrdStyleManager,IBuyOrdStyleSizeManager buyOrdStyleSizeManager,IGraddingManager graddingManager,ICuttingBatchManager cuttingBatchManager,IOmBuyerManager buyerManager)
        {
            _cuttingBatchManager = cuttingBatchManager;
            _buyerManager = buyerManager;
            _graddingManager = graddingManager;
            _buyOrdStyleSizeManager = buyOrdStyleSizeManager;
            _buyOrdStyleManager = buyOrdStyleManager;
            _cuttingSequenceManager = cuttingSequenceManager;
        }
       [AjaxAuthorize(Roles = "gradding-1,gradding-2,gradding-3")]
        public ActionResult Index(GraddingViewModel model)
        {
            ModelState.Clear();
            model.Buyers = _buyerManager.GetAllBuyers();
            if (!model.IsSearch)
            {
                model.IsSearch = true;

                return View(model);
            }
            model.OrderList = _buyOrdStyleManager.GetOrderByBuyer(model.CuttingBatch.BuyerRefId);
            model.StyleList = _buyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.CuttingBatch.OrderNo);
            model.Colors = _buyOrdStyleManager.GetColorsByOrderStyleRefId(model.CuttingBatch.OrderStyleRefId);
            model.Components = _cuttingSequenceManager.GetComponentsByColor(model.CuttingBatch.ColorRefId, model.CuttingBatch.OrderStyleRefId);
            model.CuttingBatches = _cuttingBatchManager.GetCuttingBatchList(model.CuttingBatch.BuyerRefId, model.CuttingBatch.OrderNo, model.CuttingBatch.OrderStyleRefId, model.CuttingBatch.ColorRefId, model.CuttingBatch.ComponentRefId, model.CuttingBatch.CuttingBatchRefId, model.CuttingBatch.CuttingBatchId);
            model.CuttingBatchList = _cuttingBatchManager.GetJobNoByComponentRefId(model.CuttingBatch.ColorRefId, model.CuttingBatch.ComponentRefId, model.CuttingBatch.OrderStyleRefId); 
           return View(model);
        }
            [AjaxAuthorize(Roles = "gradding-2,gradding-3")]
        public ActionResult GraddingList(GraddingViewModel model)
        {
            model.Graddings = _graddingManager.GetGradingListByCuttingBatch(model.CuttingBatchId);
            return PartialView("~/Areas/Production/Views/Gradding/_graddingList.cshtml", model);
        }
              [AjaxAuthorize(Roles = "gradding-2,gradding-3")]
        public ActionResult Edit(GraddingViewModel model)
        {
            model.Gradding.CuttingBatchId = model.CuttingBatchId;
            var cuttingBatch = _cuttingBatchManager.GetCuttingBatchById(model.Gradding.CuttingBatchId);
            model.Sizes = _buyOrdStyleSizeManager.GetBuyOrdStyleSize(cuttingBatch.OrderStyleRefId);
            if (model.Gradding.CuttingGraddingId>0)
            {
                model.Gradding = _graddingManager.GetGraddingById(model.Gradding.CuttingGraddingId);
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "gradding-2,gradding-3")]
        public ActionResult Save(GraddingViewModel model)
        {
            try
            {
                int saveIndex = model.Gradding.CuttingGraddingId > 0 ? _graddingManager.EditGradding(model.Gradding) : _graddingManager.SaveGradding(model.Gradding);
                if (saveIndex > 0)
                {
                    return RedirectToAction("GraddingList", new { CuttingBatchId = model.Gradding.CuttingBatchId });
                }
                else
                {
                    return ErrorResult("Failed To Save Gradding");
                }
            }
            catch (Exception exception)
            {
               
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
          
            
        }
        [AjaxAuthorize(Roles = "gradding-3")]
        public ActionResult Delete(GraddingViewModel model)
        {
            int deletedIndex = 0;
            deletedIndex = _graddingManager.DeleteGradding(model.Gradding.CuttingGraddingId);
            if (deletedIndex>0)
            {
                return RedirectToAction("GraddingList", new { CuttingBatchId = model.CuttingBatchId });
            }
            else
            {
                return ErrorResult("Failed To delte Gradding");
            }
        }
      
    }
}
