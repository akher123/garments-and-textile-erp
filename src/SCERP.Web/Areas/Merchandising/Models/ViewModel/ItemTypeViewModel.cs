using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class ItemTypeViewModel
    {
        public string CompId { get; set; }
        public int TotalRecords { get; set; }
        public ItemTypeViewModel()
        {
            ItemType=new OM_ItemType();
            ItemTypes=new List<OM_ItemType>();
        }
        public OM_ItemType ItemType { get; set; }
        public List<OM_ItemType> ItemTypes { get; set; } 
    }
}