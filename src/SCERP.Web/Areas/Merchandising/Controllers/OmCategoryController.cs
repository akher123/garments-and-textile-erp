using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class OmCategoryController : BaseController
    {
        private IOmCategoryManager _omCategoryManager;

        public OmCategoryController(IOmCategoryManager omCategoryManager)
        {
            this._omCategoryManager = omCategoryManager;
        }
        [AjaxAuthorize(Roles = "category-1,category-2,category-3")]
        public ActionResult Index(OmCategoryViewModel model)
        {

            ModelState.Clear();
            var totalRecords = 0;
            model.Categories = _omCategoryManager.GetCategoriesByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "category-2,category-3")]
        public ActionResult Edit(OmCategoryViewModel model)
        {
            ModelState.Clear();
            if (model.CatergoryId > 0)
            {
                var category = _omCategoryManager.GetCategoryById(model.CatergoryId);
                model.CatName = category.CatName;
                model.CatRefId = category.CatRefId;
                model.CatergoryId = category.CatergoryId;

            }
            else
            {
                model.CatRefId = _omCategoryManager.GetNewCategoryRefId();
            }
            return View(model);
        }
        [HttpPost]
        [AjaxAuthorize(Roles = "category-2,category-3")]
        public ActionResult Save(OM_Category model)
        {

            var index = 0;
            var errorMessage = "";
            try
            {
                index = model.CatergoryId > 0 ? _omCategoryManager.EditCatergory(model) : _omCategoryManager.SaveCatergory(model);
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            return index > 0 ? Reload() : ErrorResult("Category save fail " + errorMessage);
        }
        [AjaxAuthorize(Roles = "category-3")]
        public ActionResult Delete(OM_Category model)
        {
            var saveIndex = _omCategoryManager.DeleteCategory(model.CatRefId);
            if (saveIndex == -1)
            {
                return ErrorResult("Could not possible to delete Category because of it's all ready used in buyer Order");
            }

            return saveIndex > 0 ? Reload() : ErrorResult("Delate Fail");
        }

        [HttpPost]
        public JsonResult CheckExistingCategory(OM_Category model)
        {
            var isExist = !_omCategoryManager.CheckExistingCategory(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }

    }
}