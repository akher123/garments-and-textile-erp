using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model.MerchandisingModel;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class BuyerOrderColorSizeController : BaseController
    {
        private readonly IBuyerOrderManager _buyerOrderManager;
        private readonly IBuyOrdStyleColorManager _buyOrdStyleColorManager;
        private readonly IBuyOrdStyleSizeManager _buyOrdStyleSizeManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyleManager;
        private readonly IOmSizeManager _omSizeManager;
        private readonly IOmColorManager _omColorManager;
        public BuyerOrderColorSizeController(IBuyerOrderManager buyerOrderManager, IBuyOrdStyleColorManager buyOrdStyleColorManager, IBuyOrdStyleSizeManager buyOrdStyleSizeManager, IOmBuyOrdStyleManager buyOrdStyleManager, IOmSizeManager omSizeManager, IOmColorManager omColorManager)
        {
            this._buyerOrderManager = buyerOrderManager;
            this._buyOrdStyleColorManager = buyOrdStyleColorManager;
            this._buyOrdStyleSizeManager = buyOrdStyleSizeManager;
            this._buyOrdStyleManager = buyOrdStyleManager;
            this._omSizeManager = omSizeManager;
            this._omColorManager = omColorManager;
        }
        [AjaxAuthorize(Roles = "buyerorder-1,buyerorder-2,buyerorder-3")]
        public ActionResult Index(BuyerOrderColorSizeViewModel model)
        {
            try
            {
                ModelState.Clear();
                model.OrdStyle = _buyOrdStyleManager.GetVBuyOrdStyleByRefId(model.OrderStyleRefId);
                model.BuyerOrder = _buyerOrderManager.GetBuyerOrderByOrderNo(model.OrdStyle.OrderNo);
                model.BuyOrdStyleColors = _buyOrdStyleColorManager.GetBuyOrdStyleColor(model.OrderStyleRefId);
                model.BuyOrdStyleSizes = _buyOrdStyleSizeManager.GetBuyOrdStyleSize(model.OrderStyleRefId);
                model.Color.OrderStyleRefId = model.OrderStyleRefId;
                model.Size.OrderStyleRefId = model.OrderStyleRefId;
                model.ButtonName = "Add";
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult("System Error :" + exception.Message);
            }

            return View(model);
        }

        public ActionResult SizeAutoComplite(string serachString,string typeId)
        {
            var sizeList = _omSizeManager.SizeAutoComplite(serachString,typeId);
            return Json(sizeList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ColorAutoComplite(string serachString, string typeId)
        {
            var sizeList = _omColorManager.ColorAutoComplite(serachString,typeId);
            return Json(sizeList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditSize(BuyerOrderColorSizeViewModel model)
        {
            return View(model);
        }


        [AjaxAuthorize(Roles = "buyerorder-2,buyerorder-3")]
        [HttpPost]
        public ActionResult SaveColor(BuyerOrderColorSizeViewModel model)
        {
            var isNullOrWhiteSpace = String.IsNullOrWhiteSpace(model.Color.ColorRefId);
            if (isNullOrWhiteSpace)
            {
                return ErrorResult("Invalide Size Name");
            }
            else
            {
                var saveIndex = model.Color.OrderStyleColorId > 0 ? _buyOrdStyleSizeManager.EditBuyOrdStyleColor(model.Color) : _buyOrdStyleSizeManager.SaveBuyOrdStyleColor(model.Color);
                if (saveIndex > 0)
                {
                    model.BuyOrdStyleColors = _buyOrdStyleSizeManager.GetBuyOrdStyleColor(model.Color.OrderStyleRefId);
                    model.ButtonName = "Add";
                    return View("~/Areas/Merchandising/Views/BuyerOrderColorSize/EditColor.cshtml", model);
                }
                else
                {
                    return ErrorResult("Save Fail");
                }
            }

        }
        [AjaxAuthorize(Roles = "buyerorder-2,buyerorder-3")]
        [HttpPost]
        public ActionResult SaveSize(BuyerOrderColorSizeViewModel model)
        {
            var isNullOrWhiteSpace = String.IsNullOrWhiteSpace(model.Size.SizeRefId);
            if (isNullOrWhiteSpace)
            {
                return ErrorResult("Invalide Size Name");
            }
            else
            {
                var saveIndex = model.Size.OrderStyleSizeId > 0 ? _buyOrdStyleSizeManager.EditBuyOrdStyleSize(model.Size) : _buyOrdStyleSizeManager.SaveBuyOrdStyleSize(model.Size);
                if (saveIndex > 0)
                {
                    model.BuyOrdStyleSizes = _buyOrdStyleSizeManager.GetBuyOrdStyleSize(model.Size.OrderStyleRefId);
                    model.ButtonName = "Add";
                    return View("~/Areas/Merchandising/Views/BuyerOrderColorSize/EditSize.cshtml", model);
                }
                else
                {
                    return ErrorResult("Save Fail");
                }
            }
        }
        [AjaxAuthorize(Roles = "buyerorder-3")]
        public ActionResult DelteOrderStyleSize(BuyerOrderColorSizeViewModel model)
        {

            var saveIndex = _buyOrdStyleSizeManager.DelteBuyOrdStyleSize(model.Size);
            if (saveIndex > 0)
            {
                model.BuyOrdStyleSizes = _buyOrdStyleSizeManager.GetBuyOrdStyleSize(model.Size.OrderStyleRefId);
            }

            return Json(new { SuccessStatus = true }, JsonRequestBehavior.AllowGet);
        }
        [AjaxAuthorize(Roles = "buyerorder-3")]
        public ActionResult DelteOrderStyleColor(BuyerOrderColorSizeViewModel model)
        {
            var saveIndex = _buyOrdStyleSizeManager.DelteBuyOrdStyleColor(model.Color);
            if (saveIndex > 0)
            {
                model.BuyOrdStyleColors = _buyOrdStyleSizeManager.GetBuyOrdStyleColor(model.Color.OrderStyleRefId);
            }
            else
            {
                return ErrorResult("This Delate Fail!!");
            }
            return Json(new { SuccessStatus = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult CreateColor()
        {
            var colorSiseVw=  new BuyerOrderColorSizeViewModel
            {
                OmColor = {ColorRefId = _omColorManager.GetNewOmColorRefId()}
            };
            return View(colorSiseVw);
        }
        [HttpPost]
        public ActionResult CreateColor(BuyerOrderColorSizeViewModel model)
        {
            int saveIndex = 0;
            try
            {
                model.OmColor.TypeId = ColorType.COLOR;
                 saveIndex = _omColorManager.SaveOmColor(model.OmColor);
            
            }
            catch (Exception exception)
            {

                return ErrorResult(exception.Message);
            }
            return saveIndex > 0 ? Json(saveIndex) : ErrorResult("Save Failed");
        }


        [HttpGet]
        public ActionResult CreateSize()
        {
            var colorSiseVw = new BuyerOrderColorSizeViewModel
            {
                OmSize = { SizeRefId = _omSizeManager.GetNewOmSizeRefId() }
            };
            return View(colorSiseVw);
        }
        [HttpPost]
        public ActionResult CreateSize(BuyerOrderColorSizeViewModel model)
        {
            int saveIndex = 0;
            try
            {
                model.OmSize.TypeId = "01"; // Garment Type 
                model.OmSize.ItemType = "GARMENT"; // Garment Typext 
                saveIndex = _omSizeManager.SaveOmSize(model.OmSize);

            }
            catch (Exception exception)
            {

                return ErrorResult(exception.Message);
            }
            return saveIndex > 0 ? Json(saveIndex) : ErrorResult("Save Failed");
        }

        [AjaxAuthorize(Roles = "buyerorder-3")]
        public ActionResult UpdateStyleColor(long id)
        {
            BuyerOrderColorSizeViewModel model = new BuyerOrderColorSizeViewModel();
            VBuyOrdStyleColor color= _buyOrdStyleColorManager.GetBuyOrdStyleColorById(id);
            model.ColorSearchSetring = color.ColorName;
            model.Color.ColorRefId = color.ColorRefId;
            model.Color.OrderStyleRefId = color.OrderStyleRefId;
            model.Color.OrderStyleColorId = color.OrderStyleColorId;
            model.Color.CompId = color.CompId;
            model.Color.ColorRow = color.ColorRow;
            model.ButtonName = "Edit";
            return PartialView("~/Areas/Merchandising/Views/BuyerOrderColorSize/_UpdateColor.cshtml", model);
        }
        [AjaxAuthorize(Roles = "buyerorder-3")]
        public ActionResult UpdateStyleSize(long id)
        {
            BuyerOrderColorSizeViewModel model = new BuyerOrderColorSizeViewModel();
            VBuyOrdStyleSize size = _buyOrdStyleSizeManager.GetBuyOrdStyleSizeById(id);
            model.SizeSearchSetring = size.SizeName;
            model.Size.SizeRefId = size.SizeRefId;
            model.Size.OrderStyleRefId = size.OrderStyleRefId;
            model.Size.OrderStyleSizeId = size.OrderStyleSizeId;
            model.Size.CompId = size.CompId;
            model.Size.SizeRow = size.SizeRow;
            model.ButtonName = "Edit";
            return PartialView("~/Areas/Merchandising/Views/BuyerOrderColorSize/_UpdateSize.cshtml", model);
        }
    }
}