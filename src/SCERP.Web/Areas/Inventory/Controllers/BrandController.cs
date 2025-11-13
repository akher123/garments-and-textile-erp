using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Inventory.Models.ViewModels;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class BrandController : BaseInventoryController
    {

       /// [AjaxAuthorize(Roles = "brand-1,brand-2,brand-3")]
        public ActionResult Index(BrandViewModel model)
        {
            ModelState.Clear();
            try
            {
             var totalRecords = 0;
             model.Name = model.BrandName;
             ResponsModel = BrandManager.GetBrandsByPaging(model, out totalRecords);
             model.TotalRecords = totalRecords;
                if (!model.IsSearch)
                {
                    model.IsSearch = true;
                }
                else
                {
                    if (ResponsModel.Status)
                    {
                        model.InventoryBrands = (List<Inventory_Brand>)ResponsModel.Data;
                    }
                    else
                    {
                        return ErrorResult(ResponsModel.Message);
                    }  
                }
               
         
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "brand-2,brand-3")]
        public ActionResult Edit(BrandViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.BrandId > 0)
                {
                    var brand = BrandManager.GetBrandById(model.BrandId);
                    model.Name = brand.Name;
                    model.Description = brand.Description;
                    model.CreatedBy = brand.CreatedBy;
                    model.EditedBy = brand.EditedBy;
                    model.CreatedDate = brand.CreatedDate;
                    model.EditedDate = brand.EditedDate;
                    model.IsActive = brand.IsActive;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "brand-2,brand-3")]
        public ActionResult Save(Inventory_Brand model)
        {
            try
            {
                ResponsModel = model.BrandId > 0 ? BrandManager.EditBrand(model) : BrandManager.SaveBrand(model);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (ResponsModel.Status) ? Reload() : ErrorResult(ResponsModel.Message);
        }

        [AjaxAuthorize(Roles = "brand-3")]
        public ActionResult Delete(Inventory_Brand brand)
        {
            try
            {
                ResponsModel = BrandManager.DeleteBrand(brand.BrandId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (ResponsModel.Status) ? Reload() : ErrorResult(ResponsModel.Message);
        }

        public JsonResult CheckBrandName(Inventory_Brand model)
        {
            bool isExist = !BrandManager.IsExistBrandName(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
    }
}