using System;
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
    public class BatchRollController : BaseController
    {
        private readonly IBatchRollManager _batchRollManager;
        private readonly IBatchManager _batchManager;
        private readonly IBatchDetailManager _batchDetailManager;
      
        private readonly IKnittingRollIssueManager _knittingRollIssue;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyle;
        public BatchRollController(IOmBuyerManager buyerManager, IKnittingRollIssueManager knittingRollIssue, IOmBuyOrdStyleManager buyOrdStyle, IBatchRollManager batchRollManager, IBatchManager batchManager, IBatchDetailManager batchDetailManager)
        {
            _batchRollManager = batchRollManager;
            _batchManager = batchManager;
            _batchDetailManager = batchDetailManager;
            _buyOrdStyle = buyOrdStyle;
            _knittingRollIssue = knittingRollIssue;
            _buyerManager = buyerManager;
        }
        [AjaxAuthorize(Roles = "rollbatch-1,rollbatch-2,rollbatch-3")]
        public ActionResult Index(BatchRollViewModel model)
        {
            var totalRecords = 0;
            ModelState.Clear();
            model.BtType = (int)BatchType.Internal;
            if (model.IsSearch)
            {
                model.Batches = _batchManager.GetBachListByPaging(model, out totalRecords);
            }
            model.IsSearch = true;
            model.TotalRecords = totalRecords;
            model.Buyers = _buyerManager.GetAllBuyers();
            model.OrderList = _buyOrdStyle.GetOrderByBuyer(model.BuyerRefId);
            model.Styles = _buyOrdStyle.GetBuyerOrderStyleByOrderNo(model.OrderNo);
            return View("~/Areas/Production/Views/BatchRoll/Index.cshtml",model);
        }
        [AjaxAuthorize(Roles = "rollbatch-2,rollbatch-3")]
        public ActionResult Edit(BatchRollViewModel model)
        {
            ModelState.Clear();
            model.BatchDetails = _batchDetailManager.GetbatchDetailByBatchId(model.BatchId, PortalContext.CurrentUser.CompId);
            model.VwBatchRoll.BatchId = model.BatchId;
            model.VProBatch = _batchManager.GetBachById(model.BatchId);
    
            model.BatchRolls = _batchRollManager.GetBatchRollByBatchId(model.BatchId)??new List<VwBatchRoll>();
            if (model.BatchRolls.Any())
            {
                model.VwBatchRoll.KnittingRollIssueId = model.BatchRolls.First().KnittingRollIssueId;
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "rollbatch-2,rollbatch-3")]
        public ActionResult KnittingRollIssueDetail(int knittingRollIssueId)
        {
            BatchRollViewModel model = new BatchRollViewModel();
            List<VwKnittingRollIssueDetail> rollIssueDetails = _knittingRollIssue.GetRollIssueDetailsByKnittingRollIssueId(knittingRollIssueId);
            model.BatchRolls = rollIssueDetails.Select( x => new VwBatchRoll()
            {
                KnittingRollIssueId = x.KnittingRollIssueId.GetValueOrDefault(),
                KnittingRollId = x.KnittingRollId,
                RollRefNo = x.RollRefNo,
                CharllRollNo = x.CharllRollNo,
                ItemName = x.ItemName,
                SizeName = x.StyleName,
                FinishSizeName = x.FinishSizeName,
                GSM = x.GSM,
                Quantity = x.Quantity
            }).ToList();

            return View("~/Areas/Production/Views/BatchRoll/_BatchRollList.cshtml", model);
        }
        [AjaxAuthorize(Roles = "rollbatch-2,rollbatch-3")]
        public ActionResult Save(BatchRollViewModel model)
        {
            var batchRollS = new List<PROD_BatchRoll> { new PROD_BatchRoll()
            {
                BatchId = model.VwBatchRoll.BatchId,
                KnittingRollIssueId = model.VwBatchRoll.KnittingRollIssueId,
                KnittingRollId = model.VwBatchRoll.KnittingRollId,
                CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                CreatedDate = DateTime.Now,
                CompId = PortalContext.CurrentUser.CompId,
                Remarks = model.VwBatchRoll.Remarks
            }};

            var saved = _batchRollManager.SaveBatchRoll(batchRollS);
            if (saved > 0)
            {
                model.BatchRolls = _batchRollManager.GetBatchRollByBatchId(model.VwBatchRoll.BatchId);
                return View("~/Areas/Production/Views/BatchRoll/_BatchRollList.cshtml", model);
            }
            else
            {
                return ErrorResult("Saved Failed");
            }
        }

        public ActionResult GetRoll(string searchKey)
        {
            object rolls=  _knittingRollIssue.GetRollBySearchKey(searchKey);
            return Json(rolls, JsonRequestBehavior.AllowGet);
        }
        [AjaxAuthorize(Roles = "rollbatch-3")]
        public ActionResult DeleteRoll(long batchRollId, long batchId)
        {
           BatchRollViewModel model=new BatchRollViewModel();
           int isDelete = _batchRollManager.DeleteRoll(batchRollId);
           if (isDelete > 0)
           {
               model.BatchRolls = _batchRollManager.GetBatchRollByBatchId(batchId);
               return View("~/Areas/Production/Views/BatchRoll/_BatchRollList.cshtml", model);
           }
           else
           {
               return ErrorResult("Saved Failed");
           }
        }
    }
}