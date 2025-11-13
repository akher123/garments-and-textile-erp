using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class ComponentController : BaseController
    {
        private readonly IComponentManager _componentManager;

        public ComponentController(IComponentManager componentManager)
        {
            this._componentManager = componentManager;
        }
        [AjaxAuthorize(Roles = "component-1,component-2,component-3")]
        public ActionResult Index(ComponentViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.Components = _componentManager.GetComponentByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "component-2,component-3")]
        [HttpGet]
        public ActionResult Edit(ComponentViewModel model)
        {
            ModelState.Clear();
            if (model.ComponentId > 0)
            {
                var component = _componentManager.GetComponentById(model.ComponentId);
                model.ComponentName = component.ComponentName;
                model.ComponentRefId = component.ComponentRefId;
                model.Pannel = component.Pannel;
                model.ComponentId = component.ComponentId;
                model.CompType = component.CompType;
            }
            else
            {
                model.ComponentRefId = _componentManager.GetComponentRefId();
            }
            return View(model);
        }
        [HttpPost]
        [AjaxAuthorize(Roles = "component-2,component-3")]
 
        public ActionResult Save(OM_Component model)
        {
            var index = 0;
            var errorMessage = "";
            try
            {
                var isExist = _componentManager.CheckExistingComponent(model);
                if (!isExist)
                {
                    index = model.ComponentId > 0 ? _componentManager.EditComponent(model) : _componentManager.SaveComponent(model);
                }
                else
                {
                    return ErrorResult("Style Component Already Exist!");
                }
               
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            return index > 0 ? Reload() : ErrorResult("Color save fail " + errorMessage);
        }
        [AjaxAuthorize(Roles = "component-3")]
        public ActionResult Delete(OM_Component model)
        {

            var saveIndex = _componentManager.DeleteComponent(model.ComponentRefId);
            if (saveIndex == -1)
            {
                return ErrorResult("Could not possible to delete Color because of it's all ready used in buyer Order style");
            }

            return saveIndex > 0 ? Reload() : ErrorResult("Delate Fail");
        }
    
    }
}