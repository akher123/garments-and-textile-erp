using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class OmBrandController : BaseController
    {
        private IOmBrandManager _omBrandManager;
        public OmBrandController(IOmBrandManager omBrandManager)
        {
            this._omBrandManager = omBrandManager;
        }
        [AjaxAuthorize(Roles = "brand-1,brand-2,brand-3")]
        public ActionResult Index(OmBrandViewModel model )
        {
              ModelState.Clear();
            var totalRecords = 0;
            model.Brands = _omBrandManager.GetOmBrandsByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [HttpGet]
        [AjaxAuthorize(Roles = "brand-2,brand-3")]
        public ActionResult Edit(OmBrandViewModel model)
        {
            ModelState.Clear();
            if (model.BrandId > 0)
            {
                var brand = _omBrandManager.GetOmBrandById(model.BrandId);
                model.BrandName = brand.BrandName;
                model.BrandRefId = brand.BrandRefId;
                model.BrandId = brand.BrandId;
            }
            else
            {
                model.BrandRefId = _omBrandManager.GetNewBrandRefId();
            }
            return View(model);
        }
        [HttpPost]
        [AjaxAuthorize(Roles = "brand-2,brand-3")]
        public ActionResult Save(OM_Brand model)
        {

            var index = 0;
            var errorMessage = "";
            try
            {

                var isExist = !_omBrandManager.CheckExistingBrand(model);
                if (isExist)
                {
                    index = model.BrandId > 0 ? _omBrandManager.EditOmBrand(model) : _omBrandManager.SaveOmBrand(model);
                }
                else
                {
                    return ErrorResult(model.BrandName+" Brand name Already Exist !");
                }
               
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            if (!model.IsSearch)
            {
                return index > 0 ? Reload() : ErrorResult("Season save fail " + errorMessage);
            }
            else
            {
                var brandLsit = _omBrandManager.GetBrands();
                return Json(brandLsit, JsonRequestBehavior.AllowGet);
            }
           
        }

       [AjaxAuthorize(Roles = "brand-3")]
        public ActionResult Delete(OM_Brand model)
        {

            var saveIndex = _omBrandManager.DeleteOmBrand(model.BrandRefId);
            if (saveIndex == -1)
            {
                return ErrorResult("Could not possible to delete Style Brand because of it's all ready used in buyer Order");
            }

            return saveIndex > 0 ? Reload() : ErrorResult("Delate Fail");
        }
        [HttpPost]
        public JsonResult CheckExistingBrand(OM_Brand model)
        {
            ModelState.Clear();
            var isExist = !_omBrandManager.CheckExistingBrand(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
	}
}