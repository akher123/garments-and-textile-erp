using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.Production;
using SCERP.Web.Areas.MIS.Models.ViewModel;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class JobCardController : BaseController
    {
        private readonly ICuttingBatchManager _cuttingBatchManager;
        private readonly ILayCuttingManager _layCuttingManager;
        private readonly IRollCuttingManager _rollCuttingManager;
        private readonly ICuttingSequenceManager _cuttingSequenceManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyleManager;
        private readonly IPartCuttingManager _partCuttingManager;
        private readonly ICuttingProcessStyleActiveManager _cuttingProcessStyleActive;
        private readonly IMachineManager _machineManager;
        private IBuyOrdShipManager _buyOrdShipManager;
        private readonly IFinishFabricIssueManager _finishFabricIssueManager;
        public JobCardController(IFinishFabricIssueManager finishFabricIssueManager,IMachineManager machineManager, ICuttingProcessStyleActiveManager cuttingProcessStyleActive, ICuttingSequenceManager cuttingSequenceManager, IPartCuttingManager partCuttingManager, ICuttingBatchManager cuttingBatchManager, IOmBuyOrdStyleManager buyOrdStyleManager, IComponentManager componentManager, IOmBuyerManager buyerManager, ILayCuttingManager layCuttingManager, IRollCuttingManager rollCuttingManager, IBuyOrdShipManager buyOrdShipManager)
        {
            _cuttingSequenceManager = cuttingSequenceManager;
            _partCuttingManager = partCuttingManager;
            _cuttingBatchManager = cuttingBatchManager;
            _cuttingProcessStyleActive = cuttingProcessStyleActive;
            _buyerManager = buyerManager;
            _layCuttingManager = layCuttingManager;
            _rollCuttingManager = rollCuttingManager;
            _buyOrdShipManager = buyOrdShipManager;
            _buyOrdStyleManager = buyOrdStyleManager;
            _machineManager = machineManager;
            _finishFabricIssueManager = finishFabricIssueManager;
        }

        [AjaxAuthorize(Roles = "cuttingjobcard-1,cuttingjobcard-2,cuttingjobcard-3")]
        public ActionResult Index(CuttingBatchViewModel model)
        {

            ModelState.Clear();
            try
            {
                model.CuttingBatch = _cuttingBatchManager.GetCuttingBatchByCuttingBatchId(model.CuttingBatch.CuttingBatchRefId) ?? new PROD_CuttingBatch();
                if (model.CuttingBatch.CuttingBatchId > 0 && !String.IsNullOrEmpty(model.CuttingBatch.CuttingBatchRefId))
                {
                    model.PartCutting = _partCuttingManager.GetPartCuttingByCuttingBatchId(model.CuttingBatch.CuttingBatchId);
                    model.OrderList = _buyOrdStyleManager.GetOrderByBuyer(model.CuttingBatch.BuyerRefId);
                    model.StyleList = _buyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.CuttingBatch.OrderNo);
                    model.Colors = _buyOrdStyleManager.GetColorsByOrderStyleRefId(model.CuttingBatch.OrderStyleRefId);
                    model.VwRollCuttingDictionary = _rollCuttingManager.GetRollCuttingByCuttingBatchRefId(model.CuttingBatch.CuttingBatchRefId).ToDictionary(x => Convert.ToString(x.RollCuttingId), x => x);
                    model.RollCutting.ColorRefId = model.VwRollCuttingDictionary.First().Value.ColorRefId;
                    model.RatioDictionary = _layCuttingManager.GetLayCuttingByCuttingBatchId(model.CuttingBatch.CuttingBatchId).ToDictionary(x => x.SizeRefId, x => x);
                    model.SpCuttingJobCards = _cuttingBatchManager.GetCuttingJobCards(model.CuttingBatch.OrderStyleRefId, model.RollCutting.ColorRefId, model.CuttingBatch.ComponentRefId,model.CuttingBatch.StyleRefId);
                    model.CuttingBatchList = _cuttingBatchManager.GetAllCuttingBatchList(model.CuttingBatch.BuyerRefId, model.CuttingBatch.OrderNo, model.CuttingBatch.OrderStyleRefId, model.RollCutting.ColorRefId, model.PartCutting.ComponentRefId,model.CuttingBatch.StyleRefId);
                    model.TotalRecords = model.CuttingBatchList.Count;
                    model.Components = _cuttingSequenceManager.GetComponentsByColor(model.CuttingBatch.ColorRefId, model.CuttingBatch.OrderStyleRefId);
                    model.OrderShips = _buyOrdShipManager.GetStyleWiseShipment(model.CuttingBatch.OrderStyleRefId, PortalContext.CurrentUser.CompId);
              
                }
                else
                {
                    model.CuttingBatch.CuttingDate = DateTime.Now;
                    model.CuttingBatch.CuttingBatchRefId = _cuttingBatchManager.GetNewCuttingBatchRefId();
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            model.Buyers = _buyerManager.GetCuttingProcessStyleActiveBuyers() as IEnumerable;
            model.Machines = _machineManager.GetMachines(ProcessCode.CUTTING);
            return View(model);
        }

        [AjaxAuthorize(Roles = "cuttingjobcard-2,cuttingjobcard-3")]
        public ActionResult Save(CuttingBatchViewModel model)
        {
            int save = 0;
            try
            {
                model.CuttingBatch.FIT = "NO FIT";
                model.CuttingBatch.CuttingStatus = "W";
                List<PROD_RollCutting> rollCuttings = model.VwRollCuttingDictionary.Select(x => x.Value).Select(x => new PROD_RollCutting()
                {
                    CompId = PortalContext.CurrentUser.CompId,
                    CuttingBatchRefId = model.CuttingBatch.CuttingBatchRefId,
                    CuttingBatchId = model.CuttingBatch.CuttingBatchId,
                    RollNo = x.RollNo,
                    ColorRefId = x.ColorRefId,
                    BatchNo = x.BatchNo,
                    Quantity = x.Quantity,
                    RollSart = x.RollSart,
                    RollEnd = x.RollEnd,
                    Combo = x.Combo,
                    RollName = x.RollName,

                }).ToList();
                model.CuttingBatch.ApprovalStatus = "P";
                model.CuttingBatch.StyleRefId = model.CuttingBatch.StyleRefId;
                model.CuttingBatch.Rmks = model.CuttingBatch.Rmks ?? "-";
                model.CuttingBatch.ColorRefId = model.RollCutting.ColorRefId;
                model.CuttingBatch.ComponentRefId = model.PartCutting.ComponentRefId;
                model.PartCutting.CompId = PortalContext.CurrentUser.CompId;
                model.PartCutting.CuttingBatchId = model.CuttingBatch.CuttingBatchId;
                model.PartCutting.CuttingBatchRefId = model.CuttingBatch.CuttingBatchRefId;
                model.PartCutting.PartSL = "01";
                PROD_PartCutting partCutting = model.PartCutting;
                var layCuttings = new List<PROD_LayCutting>();
                if (model.CuttingBatch.MachineId == null)
                {
                    return ErrorResult("Select Any Table");
                }
                if (model.CuttingBatch.MarkerEffPct == null)
                {
                    return ErrorResult("Missing Markert Eff % !");
                }
                if (model.CuttingBatch.ConsPerDzn == null)
                {
                    return ErrorResult("Missing Fabric Consumption Per Dzn!");
                }
                foreach (PROD_LayCutting prodRollCutting in model.RatioDictionary.Select(x => x.Value))
                {
                    if (prodRollCutting.Ratio == null)
                    {
                        return ErrorResult("Ratio Missing");
                    }
                    prodRollCutting.CuttingBatchId = model.CuttingBatch.CuttingBatchId;

                    layCuttings.Add(prodRollCutting);
                }
                PROD_CuttingBatch cuttingBatch = model.CuttingBatch;
                cuttingBatch.PROD_RollCutting = rollCuttings;
                cuttingBatch.PROD_LayCutting = layCuttings;
                cuttingBatch.PROD_PartCutting.Add(partCutting);

                if (rollCuttings.Any())
                {
                    if (model.CuttingBatch.CuttingBatchId > 0)
                    {
                        save = _cuttingBatchManager.EditCuttingBatch(cuttingBatch);
                    }
                    else
                    {
                        model.CuttingBatch.CuttingBatchRefId = _cuttingBatchManager.GetNewCuttingBatchRefId();
                        save = _cuttingBatchManager.SaveCuttingBatch(cuttingBatch);

                    }

                    if (save > 0)
                    {
                        var cuttBatch =
                            _cuttingBatchManager.GetCuttingBatchByCuttingBatchId(model.CuttingBatch.CuttingBatchRefId);
                        _cuttingBatchManager.GenerateBundleChat(cuttBatch.CuttingBatchId);
                    }
                }
                else
                {
                    return ErrorResult("Add at least one batch");
                }

            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

            return save > 0 ? Reload() : ErrorResult("Save Failed");

        }
        [AjaxAuthorize(Roles = "cuttingjobcard-3")]
        public ActionResult Delete(CuttingBatchViewModel model)
        {
            int index = 0;
            try
            {
                model.CuttingBatch =
                    _cuttingBatchManager.GetCuttingBatchByCuttingBatchId(model.CuttingBatch.CuttingBatchRefId);
                index = _cuttingBatchManager.DeleteCuttingBatchByCuttingBatchId(model.CuttingBatch.CuttingBatchId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return index > 0 ? Reload() : ErrorResult("Delete Failed !");


        }

        public ActionResult DailyJobCard(CuttingBatchViewModel model)
        {
            ModelState.Clear();
            model.IsSearch = true;
            var totalRecords = 0;
            var totalBody = 0;
            model.CuttingBatch.CuttingDate = model.CuttingBatch.CuttingDate ?? DateTime.Now.Date;
            model.CuttingBatchList = _cuttingBatchManager.GetAllCuttingBatchListByPaging(model.CuttingBatch.CuttingDate, model.CuttingBatch.MachineId, model.SearchString, model.PageIndex, out totalRecords, out totalBody);
            model.TotalRecords = totalRecords;
            model.TotalBody = totalBody;
            model.RowNo = model.PageIndex * 20;
            model.Machines = _machineManager.GetMachines(ProcessCode.CUTTING);
            return View(model);
        }
       

        [AjaxAuthorize(Roles = "cuttingjobcard-2,cuttingjobcard-3")]
        public ActionResult GetCuttingJobDtl([Bind(Include = "CuttingBatch,RollCutting,PartCutting")]CuttingBatchViewModel model)
        {
            ModelState.Clear();
            model.CuttingBatch.CuttingDate = DateTime.Now;
            model.SpCuttingJobCards = _cuttingBatchManager.GetCuttingJobCards(model.CuttingBatch.OrderStyleRefId, model.RollCutting.ColorRefId, model.PartCutting.ComponentRefId,model.CuttingBatch.StyleRefId);
            var sizeList = model.SpCuttingJobCards.Where(x => x.Key == "Ratio").Select(x => x.Value).First().ToList();
            int index = 1;
            foreach (var sizeRefId in sizeList)
            {
                var sl = Convert.ToString(index).PadZero(2);
                model.RatioDictionary.Add(sizeRefId, new PROD_LayCutting() { SizeRefId = sizeRefId, CuttingBatchRefId = model.CuttingBatch.CuttingBatchRefId, CompId = PortalContext.CurrentUser.CompId, LaySl = sl });
                index++;
            }
            model.CuttingBatchList = _cuttingBatchManager.GetAllCuttingBatchList(model.CuttingBatch.BuyerRefId, model.CuttingBatch.OrderNo, model.CuttingBatch.OrderStyleRefId, model.RollCutting.ColorRefId, model.PartCutting.ComponentRefId,model.CuttingBatch.StyleRefId);
            model.TotalRecords = model.CuttingBatchList.Count;
            model.Machines = _machineManager.GetMachines(ProcessCode.CUTTING);
            return PartialView("~/Areas/Production/Views/JobCard/_CuttingJobDtl.cshtml", model);
        }
        [AjaxAuthorize(Roles = "cuttingjobcard-2,cuttingjobcard-3")]
        public ActionResult AddNewRoll([Bind(Include = "VwRollCuttingDictionary,RollCutting")]CuttingBatchViewModel model)
        {
            ModelState.Clear();
            string key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            if (String.IsNullOrEmpty(model.RollCutting.BatchNo))
            {
            
                return ErrorResult("Please Enter Batch No");
            }
          //  bool isExist = _finishFabricIssueManager.IsExistReceivedBatch(model.RollCutting.BatchNo);
            //if (!isExist)
            //{
            //    return ErrorResult("ব্যাচ নম্বর সঠিক নয় ! সঠিক  ব্যাচ নম্বর এন্ট্রি দেয়ার জন্য বলা হইতেছে ! ফ্যাব্রিক চালান রিসিভ করে কাজ করতে হবে !");
            //}
                if (String.IsNullOrEmpty(model.RollCutting.RollName))
            {
                return ErrorResult("Please Enter Roll");
            }
            if (model.RollCutting.Quantity == null)
            {
                return ErrorResult("Please Enter Ply");
            }

            model.RollCutting.RollSart = model.VwRollCuttingDictionary.Sum(x => x.Value.Quantity) + 1;
            model.RollCutting.RollEnd = model.RollCutting.RollSart + model.RollCutting.Quantity - 1;
            model.VwRollCuttingDictionary.Add(key, model.RollCutting);
            var rollcuttings = new Dictionary<string, VwRollCutting>();
            int index = 1;
            int startNumber = 1;
            int endNumber = 0;
            foreach (var rollCutting in model.VwRollCuttingDictionary)
            {
                endNumber = endNumber + rollCutting.Value.Quantity.GetValueOrDefault();
                var roll = Convert.ToString(index).PadZero(2);
                rollCutting.Value.RollNo = roll;
                rollCutting.Value.RollSart = startNumber;
                rollCutting.Value.RollEnd = endNumber;
                rollcuttings.Add(rollCutting.Key, rollCutting.Value);
                startNumber = startNumber + rollCutting.Value.Quantity.GetValueOrDefault();
                index++;
            }

            model.VwRollCuttingDictionary = rollcuttings;
            return PartialView("~/Areas/Production/Views/JobCard/_RollCutting.cshtml", model);
        }
        [AjaxAuthorize(Roles = "cuttingjobcard-2,cuttingjobcard-3")]
        public ActionResult DeleteRollCutting([Bind(Include = "VwRollCuttingDictionary")]CuttingBatchViewModel model)
        {
            ModelState.Clear();
            var rollCuttingLsit = new Dictionary<string, VwRollCutting>();
            int index = 1;
            int startNumber = 1;
            int endNumber = 0;
            foreach (var rollCutting in model.VwRollCuttingDictionary)
            {
                endNumber = endNumber + rollCutting.Value.Quantity.GetValueOrDefault();
                var roll = Convert.ToString(index).PadZero(2);
                rollCutting.Value.RollNo = roll;
                rollCutting.Value.RollSart = startNumber;
                rollCutting.Value.RollEnd = endNumber;
                rollCuttingLsit.Add(rollCutting.Key, rollCutting.Value);
                startNumber = startNumber + rollCutting.Value.Quantity.GetValueOrDefault();
                index++;
            }
            model.VwRollCuttingDictionary = rollCuttingLsit;
            return PartialView("~/Areas/Production/Views/JobCard/_RollCutting.cshtml", model);
        }
        [AjaxAuthorize(Roles = "cuttingapproval-1,cuttingapproval-2,cuttingapproval-3")]
        public ActionResult CuttingApproval(CuttingBatchViewModel model)
        {
            ModelState.Clear();
            if (model.IsSearch)
            {
                model.OrderList = _buyOrdStyleManager.GetOrderByBuyer(model.CuttingBatch.BuyerRefId);
                model.StyleList = _buyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.CuttingBatch.OrderNo);
                model.Colors = _buyOrdStyleManager.GetColorsByOrderStyleRefId(model.CuttingBatch.OrderStyleRefId);
                model.Components = _cuttingSequenceManager.GetComponentsByColor(model.CuttingBatch.ColorRefId, model.CuttingBatch.OrderStyleRefId);
            }
            else
            {
                model.IsSearch = true;
            }
            model.VwCuttingApprovalList = _cuttingBatchManager.GetCuttingApproval(model.CuttingBatch.BuyerRefId, model.CuttingBatch.OrderNo, model.CuttingBatch.OrderStyleRefId, model.CuttingBatch.ColorRefId, model.CuttingBatch.ComponentRefId, model.ApprovalStatus);
            model.Buyers = _buyerManager.GetAllBuyers();
            return View(model);
        }
        [AjaxAuthorize(Roles = "cuttingapproval-2,cuttingapproval-3")]
        public ActionResult SaveApprovalStatus(long cuttingBatchId)
        {
            int updateIndex = _cuttingBatchManager.SaveApprovalStatus(cuttingBatchId);
            bool approvalStatust = false;
            approvalStatust = updateIndex > 0;
            return Json(approvalStatust);
        }
        public ActionResult MonthlyCuttintStatus(CuttingBatchViewModel model)
        {
            ModelState.Clear();
            model.FromDate = DateTime.Now;
            model.ToDate = DateTime.Now;
            return View(model);
        }
        public ActionResult GetOrderByBuyer(string buyerRefId)
        {
          //  IEnumerable orderList = _buyOrdStyleManager.GetOrderByBuyer(buyerRefId);
           IEnumerable orderList = _cuttingProcessStyleActive.GetOrderByBuyer(buyerRefId);
            return Json(orderList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStyleByOrderNo(string orderNo)
        {
          // IEnumerable styleList = _buyOrdStyleManager.GetStyleByOrderNo(orderNo);
         IEnumerable styleList = _cuttingProcessStyleActive.GetStyleByOrderNo(orderNo);
            return Json(styleList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetColorsByOrderStyleRefId(string orderStyleRefId)
        {
            IEnumerable styleList = _buyOrdStyleManager.GetColorsByOrderStyleRefId(orderStyleRefId);
            return Json(styleList, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetComponentByColor(string colorRefId, string orderStyleRefId)
        {
            var components = _cuttingSequenceManager.GetComponentsByColor(colorRefId, orderStyleRefId);
            return Json(components, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DailyMonthWiseCutting(ProductionReportViewModel model)
        {
            model.DataTable = _cuttingBatchManager.GetDailyMonthWiseCutting(model.YearId,PortalContext.CurrentUser.CompId);
            return View(model);
        }

        public ActionResult GetStyleWiseShipment(string orderStyleRefId)
        {
            var shipments = _buyOrdShipManager.GetStyleWiseShipment(orderStyleRefId, PortalContext.CurrentUser.CompId);
            return Json(shipments, JsonRequestBehavior.AllowGet);
        }


    }
}