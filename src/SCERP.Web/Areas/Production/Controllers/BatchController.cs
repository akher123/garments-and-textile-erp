using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class BatchController : BaseController
    {
        private readonly IBatchManager _batchManager;
        private readonly IColorManager _colorManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IComponentManager _componentManager;
        private readonly IMachineManager _machineManager;
        private readonly IOmColorManager _omColorManager;
        private readonly IBatchDetailManager _batchDetailManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyleManager;
        private readonly IGroupSubProcessManager _groupSubProcessManager;
        private IKnittingRollIssueManager _KnittingRollIssueManager;
        private readonly IDyeingJobOrderManager _dyeingJobOrderManager;
        private readonly IPartyManager partyManager;
        public BatchController(IPartyManager partyManager,IDyeingJobOrderManager dyeingJobOrderManager,IMachineManager machineManager,IBatchManager batchManager, IColorManager colorManager, IOmBuyerManager buyerManager, IComponentManager componentManager, IOmColorManager omColorManager, IBatchDetailManager batchDetailManager, IOmBuyOrdStyleManager buyOrdStyleManager, IGroupSubProcessManager groupSubProcessManager, IKnittingRollIssueManager knittingRollIssueManager)
        {
            this._batchManager = batchManager;
            this._colorManager = colorManager;
            _machineManager = machineManager;
            _componentManager = componentManager;
            _buyerManager = buyerManager;
            _omColorManager = omColorManager;
            _batchDetailManager = batchDetailManager;
            _buyOrdStyleManager = buyOrdStyleManager;
            _groupSubProcessManager = groupSubProcessManager;
            _KnittingRollIssueManager = knittingRollIssueManager;
            _dyeingJobOrderManager = dyeingJobOrderManager;
            this.partyManager = partyManager;
        }

        [AjaxAuthorize(Roles = "dyeingbatch-1,dyeingbatch-2,dyeingbatch-3")]
        public ActionResult Index(BatchViewModel model)
        {
            var totalRecords = 0;
            if (model.IsSearch)
            {
               
                ModelState.Clear();
                model.TotalRecords = totalRecords; 
            }
            else
            {
                model.IsSearch = true;
                model.FromDate=DateTime.Now.AddDays(-1);
                model.ToDate = DateTime.Now.AddDays(1);
            }
            model.VProBatches = _batchManager.GetBachListByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "dyeingbatch-2,dyeingbatch-3")]
        public ActionResult Edit(BatchViewModel model)  
        {
            const string partyType = "P";
            ModelState.Clear();
            model.Parties = partyManager.GetParties(partyType);
            model.Machines = _machineManager.GetMachines(ProcessCode.DYEING);
        
            if (model.Batch.BatchId > 0)
            {
                var batch = _batchManager.GetBachById(model.Batch.BatchId);
                model.Batch.BatchNo = batch.BatchNo;
                model.Batch.BtRefNo = batch.BtRefNo;
                model.Batch.BtType =Convert.ToInt32(batch.BtType);
                model.Batch.BatchId = batch.BatchId;
                model.Batch.BatchQty = batch.BatchQty;
                //model.Batch.ColorId = batch.ColorId;
                model.Batch.PartyId = batch.PartyId;
                model.Batch.MachineId = batch.MachineId;
                model.Batch.BatchDate = batch.BatchDate;
                model.Batch.ItemId = batch.ItemId;
                // model.Batch.ItemName = batch.ItemName;
                model.Batch.Gsm = batch.Gsm;
                model.GColorName = batch.GColorName;
                model.Batch.GSizeRefId = batch.GSizeRefId;
                //  model.Batch.GSizeName = batch.GSizeName;
                model.Batch.FColorRefId = batch.FColorRefId;
                //  model.Batch.FColorName = batch.FColorName;
                model.Batch.FSizeRefId = batch.FSizeRefId;
                //  model.Batch.FSizeName = batch.FSizeName;
                model.Batch.ShadePerc = batch.ShadePerc;
                //model.Batch.CostRate = batch.CostRate;
               // model.Batch.BillRate = batch.BillRate;
                model.Batch.OrderStyleRefId = batch.OrderStyleRefId;
                // model.Batch.StyleName = batch.StyleName;
                // model.Batch.ColorName = batch.ColorName;
                model.Batch.ApprovedLdNo = batch.ApprovedLdNo;
                model.Batch.JobRefId = batch.JobRefId;
                model.Batch.Remarks = batch.Remarks;
                model.BuyerRefId = batch.BuyerRefId;
                model.OrderNo = batch.OrderNo;
                model.OrderStyleRefId = batch.OrderStyleRefId;
                model.Batch.GrColorRefId = batch.GrColorRefId;
                model.OrderList = _buyOrdStyleManager.GetOrderByBuyer(model.BuyerRefId);
                model.StyleList = _buyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.OrderNo);
                model.ColorList = _buyOrdStyleManager.GetColorsByOrderStyleRefId(model.OrderStyleRefId);
                List<VwProdBatchDetail> batchDetailList = _batchDetailManager.GetbatchDetailByBatchId(model.Batch.BatchId, PortalContext.CurrentUser.CompId);
                model.BatchDetailDictionary = batchDetailList.ToDictionary(x => Convert.ToString(x.BatchDetailId), x => x);

            }
            else
            {
                model.Batch.BatchDate =DateTime.Now;
                model.Batch.BtType = (int)BatchType.Internal;
                const string prefix = "BT";
                model.Batch.BtRefNo = _batchManager.GetBachNewRefNo(prefix);

            }
            model.GroupList = _groupSubProcessManager.GetAllGroupSubProcess(PortalContext.CurrentUser.CompId);
            model.BuyerList = _buyerManager.GetAllBuyers();

            model.JobOrders = _dyeingJobOrderManager.GetDyeingJobOrderByPartyId(model.PartyId);
            return View(model);
        }
        public ActionResult AddNewRow([Bind(Include = "BatchDetail")]BatchViewModel model)
        {
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.BatchDetailDictionary.Add(model.Key, model.BatchDetail);
            return PartialView("~/Areas/Production/Views/Batch/_AddNewRow.cshtml", model);
        }

        public ActionResult OrderInfo(int BatchType)
        {
            BatchViewModel model=new BatchViewModel();
            model.Batch.BtType = BatchType;
            model.BuyerList = _buyerManager.GetAllBuyers();
            return PartialView("~/Areas/Production/Views/Batch/_OrderInfo.cshtml", model);
        }
        [AjaxAuthorize(Roles = "dyeingbatch-2,dyeingbatch-3")]
        public ActionResult Save(BatchViewModel model)
        {
            var index = 0;
            var errorMessage = "";
            try
            {
                    bool exist=  _batchManager.IsBatchExist(model.Batch);
                    if (!exist)
                    {
                        model.Batch.ColorId = 5;
                        model.Batch.BuyerRefId = model.BuyerRefId;
                        model.Batch.OrderNo = model.OrderNo;
                        model.Batch.OrderStyleRefId = model.OrderStyleRefId;
                        
                        model.Batch.PROD_BatchDetail =
                            model.BatchDetailDictionary.Select(x => x.Value).Select(x => new PROD_BatchDetail()
                            {
                                CompId=PortalContext.CurrentUser.CompId,
                                ItemId=x.ItemId,
                                ComponentRefId = x.ComponentRefId,
                                MdiaSizeRefId=x.MdiaSizeRefId,
                                FdiaSizeRefId = x.FdiaSizeRefId,
                                GSM=x.GSM,
                                Quantity = x.Quantity,
                                StLength = x.StLength,
                                RollQty = x.RollQty,
                                FLength = x.FLength,
                                Remarks = x.Remarks
                            }).ToList();
                        model.Batch.BatchQty =Convert.ToDecimal(model.Batch.PROD_BatchDetail.Sum(x => x.Quantity));
                        if (model.Batch.PROD_BatchDetail.Any())
                        {
                            index = model.Batch.BatchId > 0 ? _batchManager.EditBatch(model.Batch) : _batchManager.SaveBatch(model.Batch);
                        }
                        else
                        {
                            return ErrorResult("Add atlist one fabric item");
                        }
                        
                    }
                    else
                    {
                        return ErrorResult("Batch No :"+model.Batch.BatchNo+" "+"Already Exsit ! Please Entry another one");
                    }
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to save Batch ! " + errorMessage);
        }
        [AjaxAuthorize(Roles = "dyeingbatch-3")]
        public ActionResult Delete(long batchId)
        {
            int deleted = 0;
                deleted = _batchManager.DeleteBatch(batchId, PortalContext.CurrentUser.CompId);
            return deleted > 0 ? Reload() : ErrorResult("Failed To Delete Batch.");
        }
        public ActionResult UpdateBatchStatus(BatchViewModel model)
        {
            ModelState.Clear();
            var batch = _batchManager.GetBachById(model.Batch.BatchId);
            model.BatchId = batch.BatchId;
            model.BtRefNo = batch.BtRefNo;
            model.BatchStatus = batch.BatchStatus;
            model.LoadingDateTime = batch.LoadingDateTime;
            model.MachineName = batch.MachineName;
            model.PartyName = batch.PartyName;
            model.BillRate = batch.BillRate;
            model.ShadePerc = batch.ShadePerc;
            if (model.LoadingDateTime != null)
            {
                model.LoadingTime = Convert.ToDateTime(batch.LoadingDateTime).ToShortTimeString();
            }
            else
            {
                model.LoadingDateTime = DateTime.Now;
                model.LoadingTime = Convert.ToDateTime(model.LoadingDateTime).ToShortTimeString();

            }
            model.UnLoadingDateTime = batch.UnLoadingDateTime;
            if (model.Batch.UnLoadingDateTime != null)
            {
                model.UnLoadingTime = Convert.ToDateTime(batch.UnLoadingDateTime).ToShortTimeString();
            }
            List<VwProdBatchDetail> batchDetailList = _batchDetailManager.GetbatchDetailByBatchId(model.Batch.BatchId, PortalContext.CurrentUser.CompId);
            model.BatchDetailDictionary = batchDetailList.ToDictionary(x => Convert.ToString(x.BatchDetailId), x => x);
            return View(model);
        }
        [HttpPost]
        public ActionResult Update(BatchViewModel model)
        {
            int index = 0;
            if (model.LoadingDateTime != null)
            {
               model.Batch.LoadingDateTime = model.LoadingDateTime.GetValueOrDefault().ToMargeDateAndTime(model.LoadingTime); 
            }
            if (model.UnLoadingDateTime != null)
            {
                model.Batch.UnLoadingDateTime = model.UnLoadingDateTime.GetValueOrDefault().ToMargeDateAndTime(model.UnLoadingTime); 
            }
            model.Batch.BatchId = model.BatchId;
            model.Batch.ShadePerc = model.ShadePerc;
            model.Batch.BillRate = model.BillRate;
            model.Batch.BatchStatus = model.BatchStatus;
            model.Batch.PROD_BatchDetail = model.BatchDetailDictionary.Select(x => x.Value).Select(x => new PROD_BatchDetail()
                           {
                               CompId = PortalContext.CurrentUser.CompId,
                               BatchDetailId = x.BatchDetailId,
                               Rate = x.Rate
                           }).ToList();
            index = _batchManager.UpdateBatchStatus(model.Batch);
            return index > 0 ? Reload() : ErrorResult("Failed To Edit Batch.");
        }
        public ActionResult GeBachInfo(string btRefNo)
        {
            var batch = _batchManager.GetBachByRefNo(btRefNo);
            return Json(batch, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BatchInActive(BatchViewModel model)
        {
            var totalRecords = 0;
            ModelState.Clear();
            model.Batches = _batchManager.GetBachList(model.SearchString,model.BtType,model.BatchStatus,model.PageIndex, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult DailyDyeingBatch(BatchViewModel model)
        {
            var totalRecords = 0;
            
            ModelState.Clear();
            model.BatchDate = model.BatchDate ?? DateTime.Now.Date;
            model.FromDate = model.BatchDate;
            model.ToDate = model.BatchDate;
            model.VProBatches = _batchManager.GetBachListByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        public ActionResult SaveInActiveBatch(int batchId)
        {
            int updateIndex= _batchManager.SaveInActiveBatch(batchId);
            return ErrorResult(updateIndex>0 ? "Done" : "In Active Failed");
        }
        public JsonResult AutoCompliteColor(string searchString)
        {
            var colorList = _colorManager.AutoCompliteColor(searchString);
            return Json(colorList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AutoCompliteComponent(string searchString)
        {
            var componentList = _componentManager.AutoCompliteComponent(searchString);
            return Json(componentList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoCompleteColor(string searchString)
        {
            var colorList= _omColorManager.AutoCompleteColor(searchString);
            return Json(colorList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BatchStatus(BatchViewModel model)
        {
            model.FromDate=DateTime.Now; 
            model.ToDate=DateTime.Now;
            model.Parties = partyManager.GetParties("P");
            model.Machines = _machineManager.GetMachines(ProcessCode.DYEING);
            return View(model);
        }


        public ActionResult RollChallanReceived(KnittingRollIssueViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.KnittingRollIssues = _KnittingRollIssueManager.GetKnittingRollIssueByPaging(model.PageIndex, model.KnittingRollIssue.OrderStyleRefId, model.sortdir,model.SearchString, PortalContext.CurrentUser.CompId, out totalRecords);
            model.TotalRecords = totalRecords;
            model.Buyers = _buyerManager.GetAllBuyers();
            return View(model);
        }

        public ActionResult IsReceivedRollChallan(int knittingRollIssueId)
        {
            int index = 0;
            index = _KnittingRollIssueManager.IsReceivedRollChallan(knittingRollIssueId, PortalContext.CurrentUser.CompId);
            return index > 0 ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDyeingJobOrderByPartyId(long partyId)
        {
            var jobOrders = _dyeingJobOrderManager.GetDyeingJobOrderByPartyId(partyId);
            return Json(jobOrders, JsonRequestBehavior.AllowGet);
        }
    }
}