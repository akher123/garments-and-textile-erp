using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Inventory.Controllers
{ 
    public class GenericNameController : BaseController
    {
        private readonly IGenericNameManager _genericNameManager;
        public GenericNameController(IGenericNameManager genericNameManager)
        {
            _genericNameManager = genericNameManager;
        }
             [AjaxAuthorize(Roles = "genericname-1,genericname-2,genericname-3")]
        public ActionResult Index(GenericNameViewModel model)
        {
            ModelState.Clear();
            int totalRecords = 0;
            model.GenericNames = _genericNameManager.GetGenericNameByPaging(model.PageIndex, model.sort, model.sortdir, model.SearchString, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
            
        }
        [HttpGet]
        [AjaxAuthorize(Roles = "genericname-2,genericname-3")]
        public ActionResult Edit(GenericNameViewModel model)
        {
            ModelState.Clear();
            if (model.GenericName.GenericNameId>0)
            {
                model.GenericName = _genericNameManager.GetGenericNameById(model.GenericName.GenericNameId);
            }
            return View(model);
        }
       [HttpPost]
       [AjaxAuthorize(Roles = "genericname-2,genericname-3")]
        public ActionResult Save(GenericNameViewModel model)
        {
            ModelState.Clear();
            int saveIndex = 0;
            try
            {
                saveIndex = model.GenericName.GenericNameId > 0 ? _genericNameManager.EditGenericName(model.GenericName) : _genericNameManager.SaveGenericName(model.GenericName);
            }
            catch (Exception exception)
            {
                
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return saveIndex>0 ? Reload() : ErrorResult("Save Failed ");
         
        }
        [AjaxAuthorize(Roles = "genericname-3")]
        public ActionResult Delete([Required]int genericNameId)
        {
            int deleted = 0;
            try
            {
                deleted = _genericNameManager.DeleteGenericName(genericNameId);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return deleted > 0 ? Reload() : ErrorResult("Delete Failed ");
        }
	}
}