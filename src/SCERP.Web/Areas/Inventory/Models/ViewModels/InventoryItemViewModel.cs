using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.Process.TreeView;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Model.InventoryModel;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class InventoryItemViewModel:Inventory_Item
    {

        public List<Inventory_Item> InventoryItems { get; set; }
        public List<Inventory_Group> ChartOfItemTreeViews { get; set; }
        public List<Inventory_SubGroup> InventorySubGroups { get; set; }
        [Required(ErrorMessage = @"Required")]
        public DateTime? UpdateDate { get; set; }

        public string GroupCode { get; set; }
        public List<Inventory_GenericName> GenericNames { get; set; }
        public InventoryItemViewModel()
        {
            ChartOfItemTreeViews = new List<Inventory_Group>();
            InventoryItems=new List<Inventory_Item>();  
            InventorySearchField = new InventorySearchField();
            InventorySubGroups=new List<Inventory_SubGroup>();
            GenericNames=new List<Inventory_GenericName>();
        }

        public string SearchKey { get; set; }


        public InventorySearchField InventorySearchField { get; set; }
        public int GroupId { get; set; }


        public List<MeasurementUnit> MeasurementUnits { get; set; }
        public IEnumerable<SelectListItem> MeasurementUnitSelectListItem
        {
            get
            {
                return new SelectList(MeasurementUnits, "UnitId", "UnitName");
            }
        }
        public IEnumerable<SelectListItem> GroupSelectListItem
        {
            get
            {
                return new SelectList(ChartOfItemTreeViews, "GroupId", "GroupName");
            }
        }
        public IEnumerable<SelectListItem> SubGroupSelectListItem
        {
            get
            {
                return new SelectList(InventorySubGroups, "SubGroupId", "SubGroupName");
            }
        }
        public IEnumerable<SelectListItem> GenericNameSelectListItem
        {
            get
            {
                return new SelectList(GenericNames, "GenericNameId", "Name");
            }
        }
        public IEnumerable<SelectListItem> ItemTypeSelectListItem
        {
            get
            {
                var values = from ItemType e in Enum.GetValues(typeof(ItemType))
                  select new { Id = (int)e, Name = e.ToString() };
                 return new SelectList(values, "Id", "Name");
                
            }
        }

    }
}