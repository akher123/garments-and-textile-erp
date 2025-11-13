using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class BuyOrdShipController : BaseController
    {

        private readonly IBuyOrdShipManager _buyOrdShipManager;
        private readonly IBuyOrdStyleColorManager _buyOrdStyleColorManager;
        private readonly IBuyOrdStyleSizeManager _buyOrdStyleSizeManager;
        private readonly IOmBuyOrdStyleManager _omBuyOrdStyleManager;
        private readonly IPortOfLoadingManager _portOfLoadingManager;
        private readonly IItemModeManager _itemModeManager;
        private readonly IBuyOrdShipDetailManager _buyOrdShipDetailManager;
        public BuyOrdShipController(IBuyOrdShipManager buyOrdShipManager,
            IBuyOrdStyleColorManager buyOrdStyleColorManager,
            IBuyOrdStyleSizeManager buyOrdStyleSizeManager, IOmBuyOrdStyleManager omBuyOrdStyleManager,
            IPortOfLoadingManager portOfLoadingManager, IItemModeManager itemModeManager, IBuyOrdShipDetailManager buyOrdShipDetailManager)
        {
            this._buyOrdShipManager = buyOrdShipManager;
            this._buyOrdStyleColorManager = buyOrdStyleColorManager;
            this._buyOrdStyleSizeManager = buyOrdStyleSizeManager;
            this._omBuyOrdStyleManager = omBuyOrdStyleManager;
            this._portOfLoadingManager = portOfLoadingManager;
            this._itemModeManager = itemModeManager;
            this._buyOrdShipDetailManager = buyOrdShipDetailManager;
        }
        [AjaxAuthorize(Roles = "buyerorder-1,buyerorder-2,buyerorder-3")]
        public ActionResult Index(BuyOrdShipViewModel model)
        {
            try
            {
                if (!String.IsNullOrEmpty(model.OrderStyleRefId))
                {
                    ModelState.Clear();
                    var totalRecords = 0;
                    model.OrdStyle = _omBuyOrdStyleManager.GetVBuyOrdStyleByRefId(model.OrderStyleRefId);
                    model.OrderStyleRefId = model.OrderStyleRefId.Replace(" ", String.Empty);
                    model.OmBuyOrdShips = _buyOrdShipManager.GetBuyOrdShipPaging(model.OrderStyleRefId, out totalRecords);
                    var omBuyOrdShip = model.OmBuyOrdShips.FirstOrDefault();
                    if (omBuyOrdShip != null)
                        model.OrdShipDetail.OrderShipRefId = omBuyOrdShip.OrderShipRefId;
                    model.TotalRecords = totalRecords;
                    model.AssortTable = _buyOrdShipManager.UpdateTempAssort(model.OrderStyleRefId);
                    model.BuyOrdStyleColors = _buyOrdStyleColorManager.GetBuyOrdStyleColor(model.OrderStyleRefId);
                    model.BuyOrdStyleSizes = _buyOrdStyleSizeManager.GetBuyOrdStyleSize(model.OrderStyleRefId);

                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);

            }
            return View("~/Areas/Merchandising/Views/BuyOrdShip/Index.cshtml", model);

        }
        [HttpGet]
        [AjaxAuthorize(Roles = "buyerorder-2,buyerorder-3")]
        public ActionResult Edit(BuyOrdShipViewModel model)
        {
            ModelState.Clear();
            model.PrOmPortOfLoadings = _portOfLoadingManager.GetPortOfLoading();
            model.ItemModes = _itemModeManager.GetItemModes();
            model.Countries = CountryManager.GetAllCountries();
            if (model.OrderShipId > 0)
            {
                OM_BuyOrdShip ordShip = _buyOrdShipManager.GetBuyerById(model.OrderShipId);
                model.OrderShipId = ordShip.OrderShipId;
                model.OrderNo = ordShip.OrderNo;
                model.OrderShipRefId = ordShip.OrderShipRefId;
                model.OrderStyleRefId = ordShip.OrderStyleRefId;
                model.LotNo = ordShip.LotNo;
                model.ShipDate = ordShip.ShipDate;
                model.CountryId = ordShip.CountryId;
                model.PortOfLoadingRefId = ordShip.PortOfLoadingRefId;
                model.IModeRefId = ordShip.IModeRefId;
                model.Quantity = ordShip.Quantity;
                model.ETD = ordShip.ETD;
                model.Remarks = ordShip.Remarks;
                model.DespatchQty = ordShip.DespatchQty;
            }
            else
            {
                var ordStyle = _omBuyOrdStyleManager.GetBuyOrdStyleByRefId(model.OrderStyleRefId);
                model.OrderNo = ordStyle.OrderNo;
                model.OrderStyleRefId = model.OrderStyleRefId;
                model.OrderShipRefId = _buyOrdShipManager.GetNewOrderShipRefId();
                model.LotNo = _buyOrdShipManager.GetNewLotNo(model.OrderStyleRefId);
            }
            return View(model);
        }
        [HttpPost]
        [AjaxAuthorize(Roles = "buyerorder-2,buyerorder-3")]
        public ActionResult Save(BuyOrdShipViewModel model)
        {
            var index = 0;
            var errorMessage = "";
            try
            {
                var ordShip = new OM_BuyOrdShip
                {
                    OrderShipId = model.OrderShipId,
                    OrderNo = model.OrderNo,
                    OrderShipRefId = model.OrderShipRefId,
                    OrderStyleRefId = model.OrderStyleRefId,
                    LotNo = model.LotNo,
                    ShipDate = model.ShipDate,
                    CountryId = model.CountryId,
                    IModeRefId = model.IModeRefId,
                    PortOfLoadingRefId = model.PortOfLoadingRefId,
                    Quantity = model.Quantity,
                    ProductionQty = model.Quantity,
                    ETD = model.ETD,
                    Remarks = model.Remarks,
                    DespatchQty = model.DespatchQty.GetValueOrDefault()
                };
                bool qtyCheck = _buyOrdShipManager.CheckShipGreaterQty(model.OrderStyleRefId, model.Quantity.GetValueOrDefault());

                if (qtyCheck)
                {
                    index = model.OrderShipId > 0 ? _buyOrdShipManager.EditBuyOrdShip(ordShip) : _buyOrdShipManager.SaveBuyOrdShip(ordShip); 
                }
                else
                {
                    return ErrorResult("Total Shipment Qty is Greater Than  Style Qty");
                }
             
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            if (index > 0)
            {
                return RedirectToAction("Index", new { OrderStyleRefId = model.OrderStyleRefId });
            }
            return ErrorResult("Failed to save data!" + errorMessage);
        }
        [AjaxAuthorize(Roles = "buyerorder-3")]
        public ActionResult Delete(OM_BuyOrdShip model)
        {

           try
           {
               var saveIndex = _buyOrdShipManager.DeleteBuyOrdShip(model.OrderShipRefId);
               if (saveIndex == -1)
               {
                   return ErrorResult("Remove Assorted Qty First !");
               }

               if (saveIndex > 0)
               {
                   return RedirectToAction("Index", new { OrderStyleRefId = model.OrderStyleRefId });
               }
               else
               {
                   return ErrorResult("Delete Failed!");
               }
           }
           catch (Exception exception)
           {
               
              Errorlog.WriteLog(exception);
              return ErrorResult(exception.Message);
           }
         
         
        }
      
        
        [HttpGet]
        public ActionResult GetShipAssort(BuyOrdShipViewModel model)
        {

            try
            {
                model.OrdShipDetail.OrderShipRefId = model.OrderShipRefId;
                model.BuyOrdStyleColors = _buyOrdStyleColorManager.GetBuyOrdStyleColor(model.OrderStyleRefId);
                model.BuyOrdStyleSizes = _buyOrdStyleSizeManager.GetBuyOrdStyleSize(model.OrderStyleRefId);
                model.AssortTable = _buyOrdShipManager.GetShipAssort(model.OrderShipRefId, model.OrderStyleRefId);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return PartialView("~/Areas/Merchandising/Views/BuyOrdShip/ShipAssort.cshtml", model);

        }
        [HttpPost]
        [AjaxAuthorize(Roles = "buyerorder-2,buyerorder-3")]
        public ActionResult SaveShipDetail(BuyOrdShipViewModel model)
        {
            ModelState.Clear();
            var saveIndex = 0;
            var message = "";
            try
            {

                saveIndex = _buyOrdShipDetailManager.SaveBuyOrdShipDetail(model.OrdShipDetail, model.OrderStyleRefId);
                model.BuyOrdStyleColors = _buyOrdStyleColorManager.GetBuyOrdStyleColor(model.OrderStyleRefId);
                model.BuyOrdStyleSizes = _buyOrdStyleSizeManager.GetBuyOrdStyleSize(model.OrderStyleRefId);
                model.AssortTable = _buyOrdShipManager.GetShipAssort(model.OrdShipDetail.OrderShipRefId, model.OrderStyleRefId);
            }
            catch (Exception exception)
            {
                message = exception.Message;
                Errorlog.WriteLog(exception);
            }
            return saveIndex > 0
                ? (ActionResult)View("~/Areas/Merchandising/Views/BuyOrdShip/ShipAssort.cshtml", model)
                : ErrorResult("Save Fail, System Error :" + message);
        }
    }
}