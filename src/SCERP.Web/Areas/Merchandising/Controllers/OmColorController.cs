using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class OmColorController : BaseController
    {
        private readonly IOmColorManager _omColorManager;
        public OmColorController(IOmColorManager omColorManager)
        {
            this._omColorManager = omColorManager;
        }
        [AjaxAuthorize(Roles = "color-1,color-2,color-3")]
        public ActionResult Index(OmColorViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.SearchString= model.SearchString.RemoveWhiteSpace();
            model.TypeId = ColorType.COLOR;
            model.OmColors = _omColorManager.GetOmColorByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "color-2,color-3")]
        [HttpGet]
        public ActionResult Edit(OmColorViewModel model)
        {

            ModelState.Clear();
            if (model.ColorId > 0)
            {
                var color = _omColorManager.GetOmColorById(model.ColorId);
                model.ColorName = color.ColorName;
                model.ColorRefId = color.ColorRefId;
                model.ColorCode = color.ColorCode;
                model.ColorId = color.ColorId;
                model.TypeId = color.TypeId;
            }
            else
            {
                model.ColorRefId = _omColorManager.GetNewOmColorRefId();
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "color-2,color-3")]
        [HttpPost]
        public ActionResult Save(OM_Color model)
        {

            var index = 0;
            var errorMessage = "";
            try
            {
                model.TypeId = ColorType.COLOR; //Color TypeId
              index = model.ColorId > 0 ? _omColorManager.EditOmColor(model) : _omColorManager.SaveOmColor(model);
            }
            catch (Exception exception)
            {
                errorMessage = "Color save fail " + exception.Message;
                Errorlog.WriteLog(exception);

            }
            return index > 0 ? Reload() : ErrorResult(errorMessage);
        }
        [HttpGet]
        [AjaxAuthorize(Roles = "color-3")]
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
            var colorList = _omColorManager.ColorAutoComplite(searchString, typeId);
            return Json(colorList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CheckExistingColor(OM_Color color)
        {
            color.ColorName = color.ColorName.RemoveWhiteSpace();
            var isExist = !_omColorManager.CheckExistingColor(color);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
    }
}