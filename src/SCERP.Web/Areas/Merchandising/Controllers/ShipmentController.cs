using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class ShipmentController : BaseController
    {
        private readonly IBuyerOrderManager _buyerOrderManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyleManager;
        private readonly IPortOfLoadingManager _portOfLoadingManager;
        private readonly IItemModeManager _itemModeManager;
        private readonly IBuyOrdShipDetailManager _buyOrdShipDetailManager;
        private readonly IBuyOrdShipManager _buyOrdShipManager;
        private readonly IBuyOrdStyleColorManager _buyOrdStyleColorManager;
        private readonly IBuyOrdStyleSizeManager _buyOrdStyleSizeManager;
        public ShipmentController(IBuyerOrderManager buyerOrderManager, IOmBuyOrdStyleManager buyOrdStyleManager,
            IPortOfLoadingManager portOfLoadingManager, IItemModeManager itemModeManager,
            IBuyOrdShipDetailManager buyOrdShipDetailManager, IBuyOrdShipManager buyOrdShipManager, IBuyOrdStyleColorManager buyOrdStyleColorManager, IBuyOrdStyleSizeManager buyOrdStyleSizeManager)
        {
            _buyerOrderManager = buyerOrderManager;
            _buyOrdStyleManager = buyOrdStyleManager;
            _portOfLoadingManager = portOfLoadingManager;
            _itemModeManager = itemModeManager;
            _buyOrdShipDetailManager = buyOrdShipDetailManager;
            _buyOrdShipManager = buyOrdShipManager;
            _buyOrdStyleColorManager = buyOrdStyleColorManager;
            _buyOrdStyleSizeManager = buyOrdStyleSizeManager;
        }
        public ActionResult Index(ShipmentViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            const string closed = "O";
            model.BuyerOrders = _buyerOrderManager.GetBuyerOrderPaging(closed,model.PageIndex, model.sort, model.sortdir, model.SearchString, model.FromDate, model.ToDate, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        public ActionResult BuyerOrderStyleLsit(ShipmentViewModel model)
        {
            ModelState.Clear();
            model.BuyOrdStyles = _buyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.OrderNo);
            return PartialView("~/Areas/Merchandising/Views/Shipment/_OrderStyleList.cshtml", model.BuyOrdStyles);
        }

        public ActionResult ShipmentInOrdeList(ShipmentViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.OmBuyOrdShips = _buyOrdShipManager.GetBuyOrdShipPaging(model.OrderStyleRefId, out totalRecords);
            return PartialView("~/Areas/Merchandising/Views/Shipment/_ShipmentInOrder.cshtml", model);
        }

        public ActionResult Edit(ShipmentViewModel model)
        {
            ModelState.Clear();
            model.PrOmPortOfLoadings = _portOfLoadingManager.GetPortOfLoading();
            model.ItemModes = _itemModeManager.GetItemModes();
            model.Countries = CountryManager.GetAllCountries();
            if (model.OrderShipId > 0)
            {
                OM_BuyOrdShip ordShip = _buyOrdShipManager.GetBuyerById(model.OrderShipId);
                model.BuyOrdShip = ordShip;
            }
            return View(model);
        }

        public ActionResult SaveShipmentOfOder(ShipmentViewModel model)
        {
            int saveIndex = _buyOrdShipManager.SaveShipmentOfOder(model.BuyOrdShip);
            return RedirectToAction("ShipmentInOrdeList", new { OrderStyleRefId = model.BuyOrdShip.OrderStyleRefId });
        }
        public ActionResult GetShipAssort(ShipmentViewModel model)
        {

            try
            {
                model.OrdShipDetail.OrderShipRefId = model.OrderShipRefId;
                model.BuyOrdStyleColors = _buyOrdStyleColorManager.GetBuyOrdStyleColor(model.OrderStyleRefId);
                model.BuyOrdStyleSizes = _buyOrdStyleSizeManager.GetBuyOrdStyleSize(model.OrderStyleRefId);
                model.AssortTable = _buyOrdShipManager.GetTotalShipAssort(model.OrderShipRefId, model.OrderStyleRefId);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return PartialView("~/Areas/Merchandising/Views/Shipment/_ShipAssort.cshtml", model);

        }

        public ActionResult UpdateShipDetail(ShipmentViewModel model)
        {
            ModelState.Clear();
            var saveIndex = 0;
            var message = "";
            try
            {
                saveIndex = _buyOrdShipDetailManager.UpdateBuyOrdShipDetail(model.OrdShipDetail, model.OrderStyleRefId);
                model.BuyOrdStyleColors = _buyOrdStyleColorManager.GetBuyOrdStyleColor(model.OrderStyleRefId);
                model.BuyOrdStyleSizes = _buyOrdStyleSizeManager.GetBuyOrdStyleSize(model.OrderStyleRefId);
                model.AssortTable = _buyOrdShipManager.GetTotalShipAssort(model.OrdShipDetail.OrderShipRefId, model.OrderStyleRefId);
            }
            catch (Exception exception)
            {
                message = exception.Message;
                Errorlog.WriteLog(exception);
            }
            return saveIndex > 0
                ? (ActionResult)View("~/Areas/Merchandising/Views/Shipment/_ShipAssort.cshtml", model)
                : ErrorResult("Save Fail, System Error :" + message);
        }
    }
}