using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using SCERP.BLL.Manager.HRMManager;
using SCERP.BLL.Manager.UserManagementManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.UserManagement.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.UserManagement.Controllers
{
    public class ModulesController : BaseUserManagementController
    {

        private readonly int _pageSize = AppConfig.PageSize;


        [AjaxAuthorize(Roles = "module-3")]
        public ActionResult Index(ModuleViewModel model)
        {
            int totalRecords = 0;
            model.Modules = ModuleManager.GetModulesByPaging(model.PageIndex,model.sort,model.sortdir,model.SearchString, out totalRecords) ?? new List<Module>();
            model.TotalRecords = totalRecords;
            return View(model);

        }


        [AjaxAuthorize(Roles = "module-3")]
        public ActionResult Edit(Module model)
        {
            ModelState.Clear();

            try
            {
                if (model != null && model.Id > 0)
                {
                    var module = ModuleManager.GetModuleById(model.Id);
                    model.ModuleName = module.ModuleName;
                    model.Description = module.Description;
                    ViewBag.Title = "Edit Module";
                }
                else
                {
                    ViewBag.Title = "Add Module";
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }


        [AjaxAuthorize(Roles = "module-3")]
        public ActionResult Save(Module model)
        {
            var module = ModuleManager.GetModuleById(model.Id) ?? new Module();
            module.ModuleName = model.ModuleName;
            module.Description = model.Description;
            var saved = 0;

            if (module.Id > 0)
            {
                var existingModule = ModuleManager.CheckExistingModule(model.Id, model.ModuleName);
                if (existingModule != null)
                    return Json(new { Success = false, Message = "Module already exists", Error = true }, JsonRequestBehavior.AllowGet);
                saved = ModuleManager.EditModule(module);
            }
            else
            {
                var existingModule = ModuleManager.GetModuleByName(model.ModuleName);
                if (existingModule != null)
                    return Json(new { Success = false, Message = "Module already exists", Error = true }, JsonRequestBehavior.AllowGet);
                saved = ModuleManager.SaveModule(module);
            }

            return saved > 0 ? Reload() : ErrorMessageResult();
        }


        [AjaxAuthorize(Roles = "module-3")]
        public ActionResult Delete(int Id)
        {
            var deleted = 0;
            var module = ModuleManager.GetModuleById(Id) ?? new Module();
            deleted = ModuleManager.DeleteModule(module);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

    }
}