using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IMisManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.MerchandisingModel;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Areas.MIS.Models.ViewModel;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace SCERP.Web.Areas.MIS.Controllers
{
    public class OrderStatusController : Controller
    {
        private readonly IPurchaseOrderManager _purchaseOrder;
        private readonly IYarnConsumptionManager yarnConsumptionManager;
        private readonly IMaterialReceivedManager _materialReceivedManager;
        private readonly IAdvanceMaterialIssueManager advanceMaterialIssueManager;
        private readonly IKnittingRollManager knittingRollManager;
        private readonly IBatchManager batchManager;
        private readonly IDyeingSpChallanManager dyeingSpChallanManager;
        private readonly IFinishFabStoreManager finishFabStoreManager;
        private readonly ICuttingBatchManager cuttingBatchManager;
        private readonly ICutBankManager cutBankManager;
        private readonly ISewingOutPutProcessManager _sewingOutPutProcessManager;
        private readonly IFinishingProcessManager finishingProcessManager;
        private readonly IProcessDeliveryManager ProcessDeliveryManager;
        private readonly IMisDashboardManager _misDashboardManager;
        private readonly IBuyOrdShipManager _buyOrdShipManager;
        private readonly ICompConsumptionDetailManager compConsumptionDetailManager;
        public OrderStatusController(ICompConsumptionDetailManager compConsumptionDetailManager,IBuyOrdShipManager buyOrdShipManager,IMisDashboardManager misDashboardManager,IProcessDeliveryManager ProcessDeliveryManager,IFinishingProcessManager finishingProcessManager,ISewingOutPutProcessManager sewingOutPutProcessManager,ICutBankManager cutBankManager,IFinishFabStoreManager finishFabStoreManager,ICuttingBatchManager cuttingBatchManager,IDyeingSpChallanManager dyeingSpChallanManager,IBatchManager batchManager,IKnittingRollManager knittingRollManager,IAdvanceMaterialIssueManager advanceMaterialIssueManager,IMaterialReceivedManager materialReceivedManager,IYarnConsumptionManager yarnConsumptionManager,IPurchaseOrderManager purchaseOrder)
        {
            _purchaseOrder = purchaseOrder;
            this.yarnConsumptionManager = yarnConsumptionManager;
            _materialReceivedManager = materialReceivedManager;
            this.advanceMaterialIssueManager = advanceMaterialIssueManager;
            this.knittingRollManager = knittingRollManager;
            this.batchManager = batchManager;
            this.dyeingSpChallanManager = dyeingSpChallanManager;
            this.cuttingBatchManager = cuttingBatchManager;
            this.finishFabStoreManager = finishFabStoreManager;
            this._sewingOutPutProcessManager = sewingOutPutProcessManager;
            this.finishingProcessManager = finishingProcessManager;
            this.ProcessDeliveryManager = ProcessDeliveryManager;
            this.cutBankManager = cutBankManager;
            _misDashboardManager = misDashboardManager;
            _buyOrdShipManager = buyOrdShipManager;
            this.compConsumptionDetailManager = compConsumptionDetailManager;
        }
        public ActionResult Index()
        { 
            DataTable db= _misDashboardManager.GetBuyerWiseOrderStyleDtable();
            return View(db);
        }
        public PartialViewResult ShipmentBrackdownStatus(string orderStyleRefId)
        {
            Dictionary<VwBuyOrdShip, DataTable> shipmts= _buyOrdShipManager.GetBuyOrdShipByeStyle(orderStyleRefId);
            return PartialView("~/Areas/MIS/Views/OrderStatus/_ShipmentBrackdownStatus.cshtml", shipmts);

        }
        public PartialViewResult AccessorisStatus(ReceiveAgainstPoViewModel model)
        {
            ModelState.Clear();
            model.Dictionary = _purchaseOrder.GetAllPurchaseOrderDetailsByStyleRefId(model.OrderStyleRefId);
            return PartialView("~/Areas/MIS/Views/OrderStatus/_AccessoriesStatus.cshtml", model);

        }
        public PartialViewResult YarnStatus(string orderStyleRefId)
        {
            YarnStatusViewModel model = new YarnStatusViewModel();
            model.YBookingDtl= _purchaseOrder.GetYarBookingSummaryByStyle(orderStyleRefId);
            model.YRcvDtl = _materialReceivedManager.GetReceivedYarnByStyle(orderStyleRefId);
            model.YConDtl = yarnConsumptionManager.GetYarnConsSummaryByOrderSyleRefId(orderStyleRefId);
            model.VCompConsumptionDetails = compConsumptionDetailManager.GetComConsumptionsFabric(orderStyleRefId);
            return PartialView("~/Areas/MIS/Views/OrderStatus/_YarnStatus.cshtml", model);
        }
        public PartialViewResult FabricStatus(string orderStyleRefId)
        {
            FabricStatusViewModel model = new FabricStatusViewModel();
            model.YDeliveryDtl = advanceMaterialIssueManager.GetDeliverdYarnByStyle(orderStyleRefId);
            model.RollRcvDtl = knittingRollManager.GetKnittingRollsSummaryByOrderStyleRefId(orderStyleRefId);
            model.DyeingDtl = batchManager.GetBachQtyByStyle(orderStyleRefId);
            model.FinishingDtl = dyeingSpChallanManager.GetDyeingSpChallanByStyle(orderStyleRefId);
            return PartialView("~/Areas/MIS/Views/OrderStatus/_FabricStatus.cshtml", model);
        }
        public PartialViewResult CuttingStatus(string orderStyleRefId)
        {
            CuttingStatusViewModel model = new CuttingStatusViewModel();
            model.FabRcvDtl= finishFabStoreManager.GetFinishFabricDeliveryByStyle(orderStyleRefId);
            model.PivotDictionary= cutBankManager.GetPivotDictionaryByStyle(orderStyleRefId);
            return PartialView("~/Areas/MIS/Views/OrderStatus/_CuttingStatus.cshtml",model);
        }
        public PartialViewResult SewingStatus(string orderStyleRefId)
        {
       
            var keyValuePairs = _sewingOutPutProcessManager.GetSewingDictionaryByStyle(orderStyleRefId);
            return PartialView("~/Areas/MIS/Views/OrderStatus/_SewingStatus.cshtml", keyValuePairs);
        }
        public PartialViewResult BodyRejceReplacementStatus(string orderStyleRefId)
        {
            ViewBag.OrderStyleRefId = orderStyleRefId;
            return PartialView("~/Areas/MIS/Views/OrderStatus/_BodyRejceReplacementStatus.cshtml");
        }
        public PartialViewResult FinishingSatus(string orderStyleRefId)
        {
           var keyValuePairs = finishingProcessManager.GetFinishingDictionaryByStyle(orderStyleRefId);
            return PartialView("~/Areas/MIS/Views/OrderStatus/_FinishingSatus.cshtml", keyValuePairs);
        }
        public PartialViewResult PrintingSatus(string orderStyleRefId)
        {
            var keyValuePairs = ProcessDeliveryManager.GetProcessStatusByStyle(orderStyleRefId, ProcessCode.PRINTING);
            return PartialView("~/Areas/MIS/Views/OrderStatus/_PrintStatus.cshtml", keyValuePairs);
        }
        public PartialViewResult EmbrSatus(string orderStyleRefId)
        {
            var keyValuePairs = ProcessDeliveryManager.GetProcessStatusByStyle(orderStyleRefId, ProcessCode.EMBROIDARY);
            return PartialView("~/Areas/MIS/Views/OrderStatus/_EmbrStatus.cshtml", keyValuePairs);
        }

        public PartialViewResult RuningOrderDetails(string orderStyleRefId)
        {
            DataTable db = _misDashboardManager.GetBuyerWiseOrderStyleDtable();
            return PartialView("~/Areas/MIS/Views/OrderStatus/_RuningOrderDetails.cshtml", db);
        }

        public PartialViewResult ShipmentRatio(string orderStyleRefId)
        {
            DataTable db = _misDashboardManager.GetShipmentRatio(orderStyleRefId);
            return PartialView("~/Areas/MIS/Views/OrderStatus/_ShipmentRatio.cshtml", db);
        }


    }
}