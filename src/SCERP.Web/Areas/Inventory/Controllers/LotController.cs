using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class LotController : BaseController
    {
        private readonly IOmColorManager _omColorManager;
        private readonly IBrandManager _brandManager;
        public LotController(IOmColorManager omColorManager, IBrandManager brandManager)
        {
             this._omColorManager = omColorManager;
             _brandManager = brandManager;
        }
        [AjaxAuthorize(Roles = "lot-1,lot-2,lot-3")]
        public ActionResult Index(LotViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.SearchString= model.SearchString.RemoveWhiteSpace();
            model.TypeId = ColorType.LOT; //YarnLot TypeId
            model.OmColors = _omColorManager.GetLotByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "lot-2,lot-3")]
        [HttpGet]
        public ActionResult Edit(LotViewModel model)
        {

            ModelState.Clear();
            model.TypeId = ColorType.LOT;
            if (model.ColorId > 0)
            {
                var color = _omColorManager.GetOmColorById(model.ColorId);
                model.ColorName = color.ColorName;
                model.ColorRefId = color.ColorRefId;
                model.BrandId = color.ColorCode;
                model.ColorId = color.ColorId;
                model.TypeId = color.TypeId;
            }
            else
            {
                model.ColorRefId = _omColorManager.GetNewOmColorRefId();
            }
            model.Brands = _brandManager.GetBrands();
            return View(model);
        }
        [AjaxAuthorize(Roles = "lot-2,lot-3")]
        [HttpPost]
        public ActionResult Save(LotViewModel model)
        {

            var index = 0;
            var errorMessage = "";
            try
            {
                OM_Color color=new OM_Color();
                color.TypeId = ColorType.LOT;
                color.ColorCode = model.BrandId;
                color.ColorName = model.ColorName;
                color.ColorRefId = model.ColorRefId;
                color.ColorId = model.ColorId;
                index = model.ColorId > 0 ? _omColorManager.EditLot(color) : _omColorManager.SaveLot(color);
            }
            catch (Exception exception)
            {
                errorMessage = "Lot save fail " + exception.Message;
                Errorlog.WriteLog(exception);

            }
            return index > 0 ? Reload() : ErrorResult(errorMessage);
        }
        [HttpGet]
        [AjaxAuthorize(Roles = "lot-3")]
        public ActionResult Delete(OM_Color model)
        {

            var saveIndex = _omColorManager.DeleteOmColor(model.ColorRefId);
            if (saveIndex == -1)
            {
                return ErrorResult("Could not possible to delete Color because of it's all ready used in buyer Order style");
            }

            return saveIndex > 0 ? Reload() : ErrorResult("Delate Fail");
        }
        [HttpGet]
        public JsonResult ColorAutoComplite(string searchString,string typeId)
        {
            var colorList = _omColorManager.ColorAutoComplite(searchString,typeId);
            return Json(colorList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CheckExistingColor(OM_Color color)
        {
            color.ColorName = color.ColorName.RemoveWhiteSpace();
            var isExist = !_omColorManager.CheckExistingColor(color);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LotAutocomplite(string serachString, string typeId)
        {
            var lots = _omColorManager.LotAutocomplite(serachString, typeId);
            return Json(lots, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLotDetails(string lotId)
        {
            dynamic lots = _omColorManager.GetLotDetails(lotId);
            return Json(JsonConvert.SerializeObject(lots), JsonRequestBehavior.AllowGet);
        }
    }

}