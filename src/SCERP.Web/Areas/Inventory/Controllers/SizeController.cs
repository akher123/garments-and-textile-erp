using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Inventory.Models.ViewModels;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class SizeController : BaseInventoryController
    {
        [AjaxAuthorize(Roles = "size-1,size-2,size-3")]
        public ActionResult Index(SizeViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (!model.IsSearch)
                {
                    model.IsSearch = true;
                }
                else
                {
                    model.Title = model.SizeName;
                    var totalRecords = 0;
                    ResponsModel = SizeManager.GetSizeListByPaging(model, out totalRecords);
                    model.TotalRecords = totalRecords;
                    if (!model.IsSearch)
                    {
                        model.IsSearch = true;
                    }
                    else
                    {
                        if (ResponsModel.Status)
                        {
                            model.InventorySizes = (List<Inventory_Size>)ResponsModel.Data;
                        }
                        else
                        {
                            return ErrorResult(ResponsModel.Message);
                        } 
                    }
                   
                }
          
              
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
         
                  return View(model);
        
        }

        [AjaxAuthorize(Roles = "size-2,size-3")]
        public ActionResult Edit(SizeViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.SizeId > 0)
                {
                    Inventory_Size size = SizeManager.GetSizeById(model.SizeId);
                    model.Title = size.Title;
                    model.Description = size.Description;
                    model.IsActive = size.IsActive;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "size-2,size-3")]
        public ActionResult Save(Inventory_Size model)
        {
            try
            {
                ResponsModel = model.SizeId > 0 ? SizeManager.EditSize(model) : SizeManager.SaveSize(model);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (ResponsModel.Status) ? Reload() : ErrorResult(ResponsModel.Message);
        }

        [AjaxAuthorize(Roles = "size-3")]
        public ActionResult Delete(Inventory_Size size)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = SizeManager.DeleteSize(size.SizeId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete Brand");

        }

        public JsonResult CheckSize(Inventory_Size model)
        {
            bool isExist = !SizeManager.IsExistSizeTitle(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
    }
}