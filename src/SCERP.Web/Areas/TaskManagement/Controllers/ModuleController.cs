using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.ITaskManagementManager;
using SCERP.Common;
using SCERP.Model.TaskManagementModel;
using SCERP.Web.Areas.TaskManagement.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.TaskManagement.Controllers
{
    public class ModuleController : BaseController
    {
        
        private readonly ITmModuleManager _tmModuleManager;

        public ModuleController(ITmModuleManager tmModuleManager)
        {
            _tmModuleManager = tmModuleManager;
        }
        public ActionResult Index(TmModuleViewModel model)
        {
            try
            {
                var totalRecords = 0;
                model.Modules = _tmModuleManager.GetAllModuleByPaging(model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return View(model);
        }

        public ActionResult Save(TmModuleViewModel model)
        {
            var index = 0;
            try
            {
                    bool exist = _tmModuleManager.IsModuleNameExist(model);
                    if (!exist)
                    {
                        if (model.ModuleId > 0)
                        {
                            index = _tmModuleManager.EditModule(model);
                        }
                        else
                        {
                            var moduleName = new TmModule { ModuleName = model.ModuleName };
                            index = _tmModuleManager.SaveModule(moduleName);
                        }
                    }
                    else
                    {
                        return ErrorResult("Module Name :" + model.ModuleName + " " + "Already Exist ! Please Entry Another One");
                    }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Save Module Name !");
        }

        public ActionResult Edit(TmModuleViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.ModuleId > 0)
                {
                    TmModule module = _tmModuleManager.GetModuleByModuleId(model.ModuleId);
                    model.ModuleName = module.ModuleName;
                }
            }
            catch (Exception exception)
            {
               Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult Delete(int moduleId)
        {
            var index = 0;
            try
            {
                 index = _tmModuleManager.DeleleModule(moduleId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Delete Module");
        }
	}
}