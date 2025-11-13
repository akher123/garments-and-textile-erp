using System;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.UserManagement.Models.ViewModels;

namespace SCERP.Web.Areas.UserManagement.Controllers
{

    public class ModuleFeatureController : BaseUserManagementController
    {

        [AjaxAuthorize(Roles = "modulefeature-1,modulefeature-2,modulefeature-3")]
        public ActionResult Index(NewModuleFeatureViewModel model)
        {
            ModelState.Clear();
            try
            {
                var totalRecords = 0;
                ModelState.Clear();
                model.Features = ModuleFeatureManager.GetModuleFeaturesByPaging(model, out totalRecords);
                model.TotalRecords = totalRecords;
                model.Modules = ModuleManager.GetModules();
                model.ModuleFeatures = ModuleFeatureManager.GetFeaturesByModule(model.ModuleId);
                model.ParentFeatureNames = ModuleFeatureManager.GetAllParentFearureName();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "modulefeature-2,modulefeature-3")]
        public ActionResult Edit(NewModuleFeatureViewModel model)
        {
            ModelState.Clear();

            try
            {
                if (model.Id > 0)
                {
                   
                    var moduleFeature = ModuleFeatureManager.GetModuleFeatureById(model.Id);
                    model.Id = moduleFeature.Id;
                    model.ModuleId = moduleFeature.ModuleId;
                    model.FeatureName = moduleFeature.FeatureName;
                    model.OrderId =moduleFeature.OrderId;
                    model.NavURL = moduleFeature.NavURL;
                    model.ShowInMenu = moduleFeature.ShowInMenu;
                    model.ParentFeatureId = moduleFeature.ParentFeatureId;
                    model.ModuleFeatures = ModuleFeatureManager.GetFeaturesByModule(moduleFeature.ModuleId);
                }
                else
                {
                    model.OrderId = 0;
                }
                model.Modules = ModuleManager.GetModules();
                
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "modulefeature-3")]
        public ActionResult SaveModuleFeature(ModuleFeature model)
        {

            try
            {
              
                var saved = 0;
                var existingModuleFeature = ModuleFeatureManager.CheckExistingModuleFeature(model.Id, model.ModuleId, model.FeatureName);
                if (!existingModuleFeature)
                {
                    if (ModelState.IsValid)
                    {
                        saved = model.Id > 0
                            ? ModuleFeatureManager.EditModuleFeature(model)
                            : ModuleFeatureManager.SaveModuleFeature(model);
                    }
                }
                else
                {
                    return ErrorResult("Module feature already exists");
                }
                ModelState.Clear();
                return saved > 0 ? Reload() : ErrorMessageResult();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
        
        }


        [AjaxAuthorize(Roles = "modulefeature-3")]
        public ActionResult Delete(ModuleFeature model)
        {
            var deletedIndex = ModuleFeatureManager.DeleteModuleFeature(model.Id);
            return deletedIndex > 0 ? Reload() : ErrorResult("Failed to delete Module Feature");
        }

        public JsonResult GetFeaturesByModule(int moduleId)
        {
            var moduleFeatures = ModuleFeatureManager.GetFeaturesByModule(moduleId);
            return Json(moduleFeatures, JsonRequestBehavior.AllowGet); 
        }
        public JsonResult GetMaxOrderIdByParentFeatureId(int parentFeatureId)
        {
            var orderId = ModuleFeatureManager.GetMaxOrderIdByParentFeatureId(parentFeatureId);  
            return Json(orderId, JsonRequestBehavior.AllowGet);
        }
    }
}