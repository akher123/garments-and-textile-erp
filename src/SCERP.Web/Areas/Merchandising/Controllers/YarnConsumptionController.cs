using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class YarnConsumptionController : BaseController
    {
        private IOmBuyOrdStyleManager omBuyOrdStyleManager;
        private IFabricOrderManager _fabricOrderManager;
        private IYarnConsumptionManager yarnConsumptionManager;
        private IConsumptionManager consumptionManager;
        private IOmBuyOrdStyleManager _buyOrdStyleManager;
        private ICompConsumptionDetailManager compConsumptionDetailManager;
        private readonly IOmBuyerManager _buyerManager;
        public YarnConsumptionController(IOmBuyerManager buyerManager, IFabricOrderManager fabricOrderManager, IOmBuyOrdStyleManager buyOrdStyleManager,IOmBuyOrdStyleManager omBuyOrdStyleManager, IYarnConsumptionManager yarnConsumptionManager, IConsumptionManager consumptionManager, ICompConsumptionDetailManager compConsumptionDetailManager, IInventoryItemManager itemManager)
        {
            _fabricOrderManager = fabricOrderManager;
            this.omBuyOrdStyleManager = omBuyOrdStyleManager;
            this.yarnConsumptionManager = yarnConsumptionManager;
            this.consumptionManager = consumptionManager;
            this.compConsumptionDetailManager = compConsumptionDetailManager;
            _buyerManager = buyerManager;
            this._buyOrdStyleManager = buyOrdStyleManager;

        }
        public ActionResult Index(YarnConsumptionViewModel model)
        {
            ModelState.Clear();
            model.VConsumption=consumptionManager.GetFabricNameByConsRefId(model.ConsRefId);
            model.VYarnConsumptions = yarnConsumptionManager.GetVYarnConsumptions(model.ConsRefId,model.GrColorRefId);
            model.YCRef = yarnConsumptionManager.GetNewYCRef();
            model.ButtonName = "Save";
            return View("~/Areas/Merchandising/Views/YarnConsumption/Index.cshtml", model);
        }
        public ActionResult FabricList(YarnConsumptionViewModel model)
        {
            model.OrdStyle = _buyOrdStyleManager.GetVBuyOrdStyleByRefId(model.OrderStyleRefId);
            //model.Consumptions = consumptionManager.GetConsumptionsFabric(model.OrderStyleRefId);
            model.VCompConsumptionDetails = compConsumptionDetailManager.GetComConsumptionsFabric(model.OrderStyleRefId);
            return View(model);
        }
        public ActionResult Save(OM_YarnConsumption model, decimal tQty)
        {
            var saveIndex = 0;
            try
            {
                saveIndex = model.YarnConsumptionId>0 ?  yarnConsumptionManager.EditYarnConsumption(model, tQty):yarnConsumptionManager.SaveYarnConsumption(model, tQty) ;
            }
            catch (Exception exception)
            {
               return ErrorResult(exception.Message);
            }
            if (saveIndex <= 0) return ErrorResult("Save fail !");
            return RedirectToAction("Index", new { ConsRefId=model.ConsRefId, GrColorRefId = model.GrColorRefId, TQty = tQty });
        }

        public ActionResult Edit(YarnConsumptionViewModel model, decimal tQty)
        {
            ModelState.Clear();
            var yarnConsumption=  yarnConsumptionManager.GetYarnConsumptionById(model.YarnConsumptionId);
            model.VConsumption = consumptionManager.GetFabricNameByConsRefId(yarnConsumption.ConsRefId);
            model.YarnConsumptionId = yarnConsumption.YarnConsumptionId;
            model.GrColorRefId = yarnConsumption.GrColorRefId;
            model.ItemCode = yarnConsumption.ItemCode;
            model.ItemName = yarnConsumption.ItemName;
            model.GColorName = yarnConsumption.GColorName;
            model.KColorName = yarnConsumption.KColorName;
            model.KColorRefId = yarnConsumption.KColorRefId;
            model.KQty = yarnConsumption.KQty;
            model.PLoss = yarnConsumption.PLoss;
            model.YCRef = yarnConsumption.YCRef;
            model.CPercent = yarnConsumption.CPercent;
            model.KSizeName = yarnConsumption.KSizeName;
            model.KSizeRefId = yarnConsumption.KSizeRefId;
            model.ConsRefId = yarnConsumption.ConsRefId;
            model.ButtonName = "Upddate";
      
            return PartialView("~/Areas/Merchandising/Views/YarnConsumption/_Edit.cshtml", model);
        }
        public ActionResult Delete(YarnConsumptionViewModel model)
        {
            var deleteIndex = yarnConsumptionManager.DeleteYarnCons(model.YarnConsumptionId);
            if (deleteIndex <= 0) return ErrorResult("Delete fail !");
            return RedirectToAction("Index", new { model.ConsRefId, model.GrColorRefId });
        }
        public ActionResult YarnConsumptionProcess(YarnConsumptionViewModel model)
        {
            return View(model);
        }
        public ActionResult OrderStyleList(YarnConsumptionViewModel model)
        {
            ModelState.Clear();
            model.Buyers = _buyerManager.GetAllBuyers();
            model.OrderList = omBuyOrdStyleManager.GetOrderByBuyer(model.BuyerRefId);
            model.BuyerOrderStyles = omBuyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.OrderNo);
            int totalRecords;
            model.OmBuyOrdStyles = _fabricOrderManager.GetVwFabricOrders(model.PageIndex, model.BuyerRefId,model.OrderNo,model.OrderStyleRefId, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

	}
}