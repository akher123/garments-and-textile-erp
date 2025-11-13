using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class CostSheetTemplateViewModel
    {
        public string sort { get; set; }
        public string sortdir { get; set; }
        public int TotalRecords { get; set; }
        public int? page { get; set; }
        public int PageIndex
        {
            get
            {
                int index = 0;
                //  int pageSize = AppConfig.PageSize;
                if (page.HasValue && page.Value > 0)
                {
                    index = page.Value - 1;
                }
                return index;
            }
        }
        public CostSheetTemplateViewModel()
        {
            CostSheetTemplate=new OM_CostSheetTemplate();
            CostSheetTemplates=new List<OM_CostSheetTemplate>();
            TempGroups=new List<OM_TempGroup>();
            ItemTypes=new List<OM_ItemType>();
        }
        public OM_CostSheetTemplate CostSheetTemplate { get; set; }
        public List<OM_CostSheetTemplate> CostSheetTemplates { get; set; }
        public List<OM_TempGroup> TempGroups { get; set; } 
        public List<OM_ItemType> ItemTypes { get; set; } 

        public IEnumerable<SelectListItem> TempGroupSelectListItem
        {
            get
            {
                return new SelectList(TempGroups, "TempGroupId", "TempGroupName");
            }
        }
        public IEnumerable<SelectListItem> ItemTypeSelectListItem { get { return new SelectList(ItemTypes, "ItemTypeId", "Title");} } 
    }
}