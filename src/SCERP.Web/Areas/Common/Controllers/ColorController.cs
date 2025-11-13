using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.Manager.CommonManager;
using SCERP.Common;
using SCERP.Model.CommonModel;
using SCERP.Web.Areas.Common.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Common.Controllers
{
    public class ColorController : BaseController
    {
        private IColorManager ColorManager;
        public ColorController(ColorManager colorManager)
        {
            this.ColorManager = colorManager;
        }
        [AjaxAuthorize(Roles = "color-1,color-2,color-3")]
        public ActionResult Index(ColorViewModel model)
        {
            var totalRecords = 0;
            ModelState.Clear();
            model.Colors= ColorManager.GetColorstByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
           [AjaxAuthorize(Roles = "color-2,color-3")]
        public ActionResult Edit(ColorViewModel model)
        {
            ModelState.Clear();
            if (model.ColorId>0)
            {
                Color color = ColorManager.GetColorById(model.ColorId);
                model.ColorId = color.ColorId;
                model.ColorRef = color.ColorRef;
                model.ColorName = color.ColorName;
                model.ColorCode = color.ColorCode;
            }
            else
            {
                model.ColorRef = ColorManager.GetNewColorRef();
            }
            return View(model);
        }
          [AjaxAuthorize(Roles = "color-2,color-3")]
        public ActionResult Save(Color model)
        {
            var index = 0;
            index = model.ColorId>0 ? ColorManager.EditColor(model) : ColorManager.SaveColor(model);
            return index>0 ? Reload() : ErrorResult("Color save fail");
        }
          [AjaxAuthorize(Roles = "color-3")]
        public ActionResult Delete(Color color)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = ColorManager.DeleteColor(color.ColorId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return deleteIndex > 0 ? Reload() : ErrorResult("Delete Fail");
        }
   
    }
}