using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class ItemModeController : BaseController
    {
        private IItemModeManager _itemModeManager;

        public ItemModeController(IItemModeManager itemModeManager)
        {
            this._itemModeManager = itemModeManager;
        }
        public ActionResult Index(ItemModeViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.ItemModes = _itemModeManager.GetItemModePaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
     
        }
        public ActionResult Edit(ItemModeViewModel model)
        {
            ModelState.Clear();
            if (model.ItemModeId > 0)
            {
                var itemMode = _itemModeManager.GetItemModeById(model.ItemModeId);
                model.ItemModeId = itemMode.ItemModeId;
                model.IModeRefId = itemMode.IModeRefId;
                model.IModeName = itemMode.IModeName;
   
            }
         
            return View(model);
        }
        public ActionResult Save(OM_ItemMode model)
        {

            var index = 0;
            var errorMessage = "";
            try
            {
                index = model.ItemModeId > 0 ? _itemModeManager.EditItemMode(model) : _itemModeManager.SaveItemMode(model);
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            return index > 0 ? Reload() : ErrorResult("Style save fail " + errorMessage);

        }

        public ActionResult Delete(OM_Style model)
        {

            var deleteIndex = _itemModeManager.DeleteItemMode(model.StylerefId);
            if (deleteIndex == -1)
            {
                return ErrorResult("Could not possible to delete Style because of it's all ready used ");
            }

            return deleteIndex > 0 ? Reload() : ErrorResult("Delate Fail");
        }

	}
}