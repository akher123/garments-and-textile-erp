using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class HouseKeepingItemController : BaseController
    {
        private readonly IHouseKeepingItemManager _houseKeepingItemManager;
        public HouseKeepingItemController(IHouseKeepingItemManager houseKeepingItemManager)
        {
            _houseKeepingItemManager = houseKeepingItemManager;
        }
        [AjaxAuthorize(Roles = "housekeepingitem-1,housekeepingitem-2,housekeepingitem-3")]
        public ActionResult Index(HouseKeepingItemViewModel model)
        {
            ModelState.Clear();
            int totalRecords = 0;
            model.HouseKeepingItems = _houseKeepingItemManager.GetHouseKeepingItems(model.PageIndex, model.sort, model.sortdir, model.SearchString, out totalRecords);
            return View(model);
        }
        [AjaxAuthorize(Roles = "housekeepingitem-2,housekeepingitem-3")]
        public ActionResult Edit(HouseKeepingItemViewModel model)
        {
            ModelState.Clear();
            if (model.HouseKeepingItem.HouseKeepingItemId > 0)
            {
                model.HouseKeepingItem = _houseKeepingItemManager.GetHouseKeepingItemById(model.HouseKeepingItem.HouseKeepingItemId);
            }
            else
            {
                model.HouseKeepingItem.HkItemRefId = _houseKeepingItemManager.GetNewRefId();
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "housekeepingitem-2,housekeepingitem-3")]
        public ActionResult Save(HouseKeepingItemViewModel model)
        {
            try
            {
                model.HouseKeepingItem.CompId = PortalContext.CurrentUser.CompId;
                var saved = model.HouseKeepingItem.HouseKeepingItemId > 0 ? _houseKeepingItemManager.EditHouseKeepingItem(model.HouseKeepingItem) : _houseKeepingItemManager.SaveHouseKeepingItem(model.HouseKeepingItem);
                return saved > 0 ? Reload() : ErrorResult("House Keeping Good Information not save successfully!!");

            }
            catch (Exception e)
            {
                Errorlog.WriteLog(e);
                return ErrorResult(e.Message);
            }

        }
        [AjaxAuthorize(Roles = "housekeepingitem-3")]
        public ActionResult Delete(int houseKeepingItemId)
        {
            var deleted = 0;
            var hk = _houseKeepingItemManager.GetHouseKeepingItemById(houseKeepingItemId);
            deleted = _houseKeepingItemManager.DeleteHouseKeepingItem(hk);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }
    }
}