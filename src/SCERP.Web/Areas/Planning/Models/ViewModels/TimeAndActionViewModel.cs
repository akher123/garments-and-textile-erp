using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Planning.Models.ViewModels
{
    public class TimeAndActionViewModel:  ProSearchModel<TimeAndActionViewModel>
    {
        public dynamic Tnas { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable Styles { get; set; }
        public string BuyerRefId { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public string CopyOrderStyleRefId { get; set; }
        public string CopyOrderNo { get; set; }
        public string ActivitySearchKey { get; set; }
        public int RowNumber { get; set; }
        public bool IsMerchandiser { get; set; }
        public string IndicationKey { get; set; }
        public string IsLocked { get; set; }
        public List<VOM_BuyOrdStyle> OrdStyles { get; set; }
        public List<object> Columns { get; set; }
        public List<string> Heads { get; set; }

     

        public TimeAndActionViewModel()
        {
            this.Heads = new List<string>();
            this.Columns = new List<object>();
            Buyers = new List<OM_Buyer>();
            OrderList = new List<object>();
            Styles = new List<object>();
            OrdStyles = new List<VOM_BuyOrdStyle>();
        }
  
        public IEnumerable<SelectListItem> StatusSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { IsLocked = "U", Value = "Panding" }, new { IsLocked = "L", Value = "Approved" } }, "IsLocked", "Value");
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

        public IEnumerable<SelectListItem> InidicatorsSelectListItem
        {
            get
            {
                return new SelectList(new[] {new { Value = "D", Text = "-Select-" }, new { Value = "Y", Text = "Yellow" }, new { Value = "A", Text = "Amber" }, new { Value = "R", Text = "Red" } , new { Value = "D", Text = "Double Red"  }}, "Value", "Text");
            }
        }
    }
}