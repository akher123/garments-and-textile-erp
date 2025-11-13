using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class InventoryItemController : BaseInventoryController
    {
        private readonly IGenericNameManager _genericNameManager;
        
        public InventoryItemController(IGenericNameManager genericNameManager)
        {
            _genericNameManager = genericNameManager;
            
        }

        [AjaxAuthorize(Roles = "inventoryitem-1,inventoryitem-2,inventoryitem-3")]
        public ActionResult Index(InventoryItemViewModel model)
        {
            ModelState.Clear();
            model.ChartOfItemTreeViews = InventoryItemManager.GetChartOfItemTreeView(model.SearchKey).ToList();
            return View(model);
        }

        [AjaxAuthorize(Roles = "inventoryitem-1,inventoryitem-2,inventoryitem-3")]
        public ActionResult GetMaxGroupCode(int? branchId)
        {
            var groupCode = InventoryGroupManager.GetMaxGroupCode();
            return Json(groupCode, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "inventoryitem-2,inventoryitem-3")]
        public ActionResult Save(Inventory_Item model)
        {
            model.ItemType = model.ItemType ?? Convert.ToInt32(ItemType.Compliance);
            ResponsModel = model.ItemId > 0 ? InventoryItemManager.EditInventoryItem(model) : InventoryItemManager.SaveInventoryItem(model);
            return ResponsModel.Status ? Reload() : ErrorResult(ResponsModel.Message);
        }
        public ActionResult Edit(InventoryItemViewModel model)
        {
            ModelState.Clear();
            if (model.ItemId > 0)
            {
                var inventoryItem = InventoryItemManager.GetItemById(model.ItemId);
                model.ItemId = inventoryItem.ItemId;
                model.MeasurementUinitId = inventoryItem.MeasurementUinitId;
                model.ReorderLevel = inventoryItem.ReorderLevel;
                model.ItemName = inventoryItem.ItemName;
                model.ItemCode = inventoryItem.ItemCode;
                model.SubGroupId = inventoryItem.SubGroupId;
                model.ItemType = inventoryItem.ItemType;
                model.GenericNameId = inventoryItem.GenericNameId;
                model.Inventory_SubGroup = inventoryItem.Inventory_SubGroup;

            }
            else
            {

                model.ItemCode = InventoryItemManager.GetItemCode(model.SubGroupId, 1);
            }
            model.GenericNames = _genericNameManager.GetAllGenericNames();
            List<MeasurementUnit> measurementUnits = MeasurementUnitManager.GetMeasurementUnits();
            model.MeasurementUnits = measurementUnits;
            return View(model);
        }

        public ActionResult EditGroup(Inventory_Group model)
        {
            ModelState.Clear();
            if (model.GroupId > 0)
            {
                model = InventoryItemManager.GetGroupById(model.GroupId);
            }
            else
            {
                model.GroupCode = InventoryGroupManager.GetMaxGroupCode();
            }

            return View(model);
        }

        public ActionResult SaveGroup(Inventory_Group group)
        {
            ResponsModel = group.GroupId > 0 ? InventoryGroupManager.EditGroup(group) : InventoryGroupManager.SaveGroup(group);
            return ResponsModel.Status ? Reload() : ErrorResult(ResponsModel.Message);
        }
        public ActionResult EditSubGroup(Inventory_SubGroup moGroup)
        {
            ModelState.Clear();
            if (moGroup.SubGroupId > 0)
            {
                moGroup = InventorySubGroupManager.GetSubGroupById(moGroup.SubGroupId);
            }
            else
            {
                moGroup.SubGroupCode = InventoryItemManager.GetMaxSubGroupCode(moGroup.GroupId);
            }

            return View(moGroup);
        }

        public ActionResult SaveSubGroup(Inventory_SubGroup subGroup)
        {
            ResponsModel = subGroup.SubGroupId > 0 ? InventorySubGroupManager.EditSubGroup(subGroup) : InventorySubGroupManager.SaveSubGroup(subGroup);
            return ResponsModel.Status ? Reload() : ErrorResult(ResponsModel.Message);
        }
        public JsonResult GetItemCode(int? subGroupId, int? branchId)
        {
            var itemCode = InventoryItemManager.GetItemCode(subGroupId, branchId);
            return Json(new { ItemCode = itemCode }, JsonRequestBehavior.AllowGet);
        }



        [AjaxAuthorize(Roles = "inventoryitem-2,inventoryitem-3")]
        public ActionResult AutocompliteGroup(string groupName)
        {
            var groups = InventoryGroupManager.AutocompliteGroup(groupName);
            return Json(groups, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "inventoryitem-2,inventoryitem-3")]
        public ActionResult AutocompliteSubGroup(string subGroupName, int branchId, int groupId)
        {
            List<Inventory_SubGroup> subGroups = InventorySubGroupManager.AutocompliteSubGroup(subGroupName, groupId, branchId);
            return Json(subGroups, JsonRequestBehavior.AllowGet);
        }


        [AjaxAuthorize(Roles = "inventoryitem-2,inventoryitem-3")]
        public ActionResult AutocompliteItem(string itemName, int subGroupId)
        {
            List<Inventory_Item> items = InventoryItemManager.AutocompliteItem(itemName, subGroupId);
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult CreatePartialItem(InventoryItemViewModel model)
        {
            ModelState.Clear();
            model.ChartOfItemTreeViews = InventoryGroupManager.GetGroups();
            model.MeasurementUnits = MeasurementUnitManager.GetMeasurementUnits();
            return View(model);
        }

        [HttpGet]
        public ActionResult CreatePartial(InventoryItemViewModel model)
        {
            ModelState.Clear();
            model.ChartOfItemTreeViews = InventoryGroupManager.GetGroups(model.GroupCode);
            model.MeasurementUnits = MeasurementUnitManager.GetMeasurementUnits();
            return View("CreatePartialItem",model);
        }
        [HttpPost]
        public ActionResult SavePartialItem(Inventory_Item model)
        {
            model.ItemType = Convert.ToInt32(ItemType.Compliance);
            model.ReorderLevel = 0;
            model.CompId = PortalContext.CurrentUser.CompId;
            model.ItemCode = InventoryItemManager.GetItemCode(model.SubGroupId, 1);
            ResponsModel = model.ItemId > 0 ? InventoryItemManager.EditInventoryItem(model) : InventoryItemManager.SaveInventoryItem(model);
            return ResponsModel.Status ? Json(ResponsModel.Status, JsonRequestBehavior.AllowGet) : ErrorResult(ResponsModel.Message);
        }
        [HttpGet]
        public JsonResult GetSubGroupByGroupId(int groupId)
        {
            var subGroups = InventorySubGroupManager.GetSubGroupByGroupId(groupId);
            return Json(subGroups, JsonRequestBehavior.AllowGet);
        }

      
        public ActionResult RateUpdate(InventoryItemViewModel model)
        {
            int updated = 0;
            ModelState.Clear();
            try
            {
                if (!model.IsSearch)
                {
                    model.IsSearch = true;
                    model.UpdateDate = DateTime.Now;
                    return View(model);
                }
                else
                {
                    updated = InventoryItemManager.RateUpdate(model.UpdateDate, model.ItemId);
                }
            }
            catch (Exception exception)
            {

                return ErrorResult(exception.Message);
            }
            return updated > 0 ? ErrorResult(model.ItemName + " Iteme Rate Update Succesfylly") : ErrorResult(model.ItemName + " Iteme Rate does not Update");


        }
        [HttpPost]
        public ActionResult MonthlyRateUpdate(InventoryItemViewModel model)
        {
            int updated = 0;
            try
            {
             updated = InventoryItemManager.MonthlyRateUpdate(model.UpdateDate);
            }
            catch (Exception exception)
            {

                return ErrorResult(exception.Message);
            }
            return updated > 0 ? ErrorResult("Update Succesfylly") : ErrorResult(" Not Updated");
        }
    }
}