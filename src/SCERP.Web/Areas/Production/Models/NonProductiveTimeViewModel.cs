using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class NonProductiveTimeViewModel : ProSearchModel<NonProductiveTimeViewModel>
    {
        public List<PROD_DownTimeCategory> DownTimeCategories { get; set; }
        public PROD_NonProductiveTime NonProductiveTime { get; set; }
        public List<VwNonProductiveTime> NonProductiveTimes { get; set; }
        public List<Production_Machine> Lines { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        [Required]
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable Styles { get; set; }
        public IEnumerable SizeList { get; set; }
  
        public NonProductiveTimeViewModel()
        {
            DownTimeCategories=new List<PROD_DownTimeCategory>();
            Lines=new List<Production_Machine>();
            Buyers=new List<OM_Buyer>();
            NonProductiveTime=new PROD_NonProductiveTime();
            NonProductiveTimes = new List<VwNonProductiveTime>();
            OrderList = new List<object>();
            Styles = new List<object>();
            SizeList = new List<object>();
    
        }
        public IEnumerable<SelectListItem> MachineSelectListItem
        {
            get
            {
                return new SelectList(Lines, "MachineId", "Name");
            }
        }
        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(Buyers, "BuyerRefId", "BuyerName");
            }
        }
        public IEnumerable<SelectListItem> OrderSelectListItem
        {
            get
            {
                return new SelectList(OrderList, "OrderNo", "RefNo");
            }
        }
        public IEnumerable<SelectListItem> StylesSelectListItem
        {
            get
            {
                return new SelectList(Styles, "OrderStyleRefId", "StyleName");
            }
        }
        public IEnumerable<SelectListItem> DownTimeCategorySelectListItem
        {
            get
            {
                return new SelectList(DownTimeCategories, "DownTimeCategoryId", "CategoryName");
            }
        }
    
    }
}