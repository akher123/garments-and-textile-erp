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
    public class OmBuyOrdStyleController : BaseController
    {
        private readonly IOmBuyOrdStyleManager _omBuyOrdStyleManager;
        private readonly IBuyerOrderManager _buyerOrderManager;
        private readonly IBuyOrdShipManager _buyOrdShipManager;
        private readonly IOmStyleManager _omStyleManager;
        private readonly IOmBrandManager _omBrandManager;
        private readonly IOmCategoryManager _omCategoryManager;
        private readonly ISeasonManager _seasonManager;
        public OmBuyOrdStyleController(IOmBuyOrdStyleManager omBuyOrdStyleManager, IBuyerOrderManager buyerOrderManager, IBuyOrdShipManager buyOrdShipManager, IOmStyleManager omStyleManager, IOmBrandManager omBrandManager, IOmCategoryManager omCategoryManager, ISeasonManager seasonManager)
        {
            this._omBuyOrdStyleManager = omBuyOrdStyleManager;
            this._buyerOrderManager = buyerOrderManager;
            this._buyOrdShipManager = buyOrdShipManager;
            this._omStyleManager = omStyleManager;
            this._omBrandManager = omBrandManager;
            this._omCategoryManager = omCategoryManager;
            this._seasonManager = seasonManager;
        }


        [AjaxAuthorize(Roles = "buyerorder-1,buyerorder-2,buyerorder-3")]
        public ActionResult Index(OmBuyOrdStyleViewModel model)
        {
            try
            {
                ModelState.Clear();
                model.OmBuyOrdStyles = _omBuyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.OrderNo);
                model.BuyerOrder = _buyerOrderManager.GetBuyerOrderByOrderNo(model.OrderNo) ?? new VBuyerOrder();
                var vomBuyOrdStyle = model.OmBuyOrdStyles.FirstOrDefault();
                if (vomBuyOrdStyle != null)
                    model.AssortTable = _buyOrdShipManager.UpdateTempAssort(vomBuyOrdStyle.OrderStyleRefId);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult("System Error :" + exception.Message);
            }


            return View("~/Areas/Merchandising/Views/OmBuyOrdStyle/Index.cshtml", model);
        }
        [HttpGet]
        [AjaxAuthorize(Roles = "buyerorder-2,buyerorder-3")]
        public ActionResult Edit(OmBuyOrdStyleViewModel model)
        {
            ModelState.Clear();
            model.OmStyles = _omStyleManager.GetAllStyles();
            model.OmBrands = _omBrandManager.GetBrands();
            model.OmCategories = _omCategoryManager.GetCategories();
            if (model.OrderStyleId>0)
            {
                var buyerOrderStyle = _omBuyOrdStyleManager.GetBuyOrdStyleById(model.OrderStyleId);
                model.OrderStyleId = buyerOrderStyle.OrderStyleId;
                model.OrderStyleRefId = buyerOrderStyle.OrderStyleRefId;
                model.OrderNo = buyerOrderStyle.OrderNo;
                model.SeasonRefId = buyerOrderStyle.SeasonRefId;
                model.StyleentDate = buyerOrderStyle.StyleentDate;
                model.StyleRefId = buyerOrderStyle.StyleRefId;
                model.BuyerArt = buyerOrderStyle.BuyerArt;
                model.BrandRefId = buyerOrderStyle.BrandRefId;
                model.Quantity = buyerOrderStyle.Quantity;
                model.CatIRefId = buyerOrderStyle.CatIRefId;
                model.Rate = buyerOrderStyle.Rate;
                model.Discount = buyerOrderStyle.Discount;
                model.Amt = buyerOrderStyle.Amt;
                model.Rmks = buyerOrderStyle.Rmks;
                model.PI = buyerOrderStyle.PI;
                model.LCSTID = buyerOrderStyle.LCSTID;
                model.ImagePath = buyerOrderStyle.ImagePath;
                var season = _seasonManager.GetSeasonsBySesonRefId(buyerOrderStyle.SeasonRefId);
                model.OmSeasons.Add(season);
            }
            else
            {
                var season = _seasonManager.GetSeasonsBySesonRefId(model.SeasonRefId);
                if (season != null)
                {
                    model.OmSeasons.Add(season);
                }
                model.StyleentDate = DateTime.Now;
                model.OrderStyleRefId = _omBuyOrdStyleManager.GetNewStyleRefNo();
            }

            return View(model);
        }
        [HttpPost]
        [AjaxAuthorize(Roles = "buyerorder-2,buyerorder-3")]
        public ActionResult Save(OM_BuyOrdStyle model)
        {
            var index = 0;
            var errorMessage = "";
            try
            {
                //bool qtyCheck = _omBuyOrdStyleManager.CheckGreaterQty(model.OrderNo, model.Quantity.GetValueOrDefault());
                //if (qtyCheck)
                //{
                //    index = model.OrderStyleId > 0 ? _omBuyOrdStyleManager.EditBuyOrdStyle(model) : _omBuyOrdStyleManager.SaveBuyOrdStyle(model);
                //}
                //else
                //{
                //    return ErrorResult("Total Style Qty is Greater Than from Order Qty");
                //}

                index = model.OrderStyleId > 0 ? _omBuyOrdStyleManager.EditBuyOrdStyle(model) : _omBuyOrdStyleManager.SaveBuyOrdStyle(model);

            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            if (index > 0)
            {
                return RedirectToAction("Index", new { OrderNo = model.OrderNo });
            }
            return ErrorResult("Failed to save data!" + errorMessage);
        }
        [AjaxAuthorize(Roles = "buyerorder-3")]
        public ActionResult Delete(OM_BuyOrdStyle model)
        {
            try
            {
                var deleteIndex = _omBuyOrdStyleManager.DeleteBuyerOrderStyle(model.OrderStyleRefId);
                if (deleteIndex > 0)
                {
                    return RedirectToAction("Index", new { OrderNo = model.OrderNo });
                }
                else if (deleteIndex == -1)
                {
                    return ErrorResult("This Style is Already Used in Shipment");
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
        public ActionResult UpdateShipQty(OmBuyOrdStyleViewModel model)
        {
            model.VomBuyOrdStyle = _omBuyOrdStyleManager.GetVBuyOrdStyleByRefId(model.OrderStyleRefId);
            model.despatchQty = model.VomBuyOrdStyle.despatchQty;
            model.ActiveStatus = model.VomBuyOrdStyle.ActiveStatus;
            model.OrderNo = model.VomBuyOrdStyle.OrderNo;
            model.OrderStyleId = model.VomBuyOrdStyle.OrderStyleId;
            model.OrderStyleRefId = model.VomBuyOrdStyle.OrderStyleRefId;
            return View(model);
        }
        [HttpPost]
        public ActionResult SaveShipQty([Bind(Include = "OrderNo,OrderStyleId,OrderStyleRefId,ActiveStatus,despatchQty")]OM_BuyOrdStyle model)
        {

            int updateQty = _omBuyOrdStyleManager.SaveShipQty(model);
            if (updateQty > 0)
            {
                return RedirectToAction("Index", new { OrderNo = model.OrderNo });
            }
            return ErrorResult("Failed to save data!");

        }
        [HttpGet]
        public JsonResult StyleAutocomplite(string searchString)
        {
            var vOrdStyleList = _omBuyOrdStyleManager.StyleAutocomplite(searchString);
            return Json(vOrdStyleList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAssortedColorSize(OmBuyOrdStyleViewModel model)
        {
            model.AssortTable = _buyOrdShipManager.UpdateTempAssort(model.OrderStyleRefId);
            return View("~/Areas/Merchandising/Views/OmBuyOrdStyle/_AssortedColorSize.cshtml", model);
        }


    }
}
