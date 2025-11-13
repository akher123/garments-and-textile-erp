using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class CostSheetMasterViewModel
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
        public CostSheetMasterViewModel()
        {
            CostSheetMaster =new OM_CostSheetMaster();
            CostSheetMasters=new List<OM_CostSheetMaster>();
            Buyers=new List<OM_Buyer>();
            ItemTypes=new List<OM_ItemType>();
            CostSheetDetail=new OM_CostSheetDetail();
            CostSheetTemplateList=new List<OM_CostSheetTemplate>();
            CsTemDictionary=new Dictionary<string, OM_CostSheetTemplate>();
        }

        public Dictionary<string, OM_CostSheetTemplate> CsTemDictionary { get; set; }
        public List<OM_CostSheetTemplate> CostSheetTemplateList { get; set; } 
        public OM_CostSheetDetail CostSheetDetail { get; set; }
        public OM_CostSheetMaster CostSheetMaster { get; set; }
        public List<OM_CostSheetMaster> CostSheetMasters { get; set; }
        public List<OM_Buyer> Buyers { get; set; } 
        public List<OM_ItemType> ItemTypes { get; set; }
        public IEnumerable<SelectListItem> BuyerSelectListItems { get { return new SelectList(Buyers, "BuyerId", "BuyerName");} } 
        public IEnumerable<SelectListItem> ItmeTypeSelectListItems {get {return new SelectList(ItemTypes,"ItemTypeId", "Title");} }
    }
}