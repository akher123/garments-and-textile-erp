using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class CutBangkViewModel : ProSearchModel<CutBangkViewModel>
    {
        public CutBangkViewModel()
        {
            StyleList = new List<object>();
            OrderList = new List<object>();
            Buyers = new List<OM_Buyer>();
            CutBanks=new List<VwCutBank>();
        }
        public IEnumerable StyleList { get; set; }
        public IEnumerable OrderList { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public string BuyerRefId { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public List<VwCutBank> CutBanks { get; set; } 
        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(Buyers, "BuyerRefId", "BuyerName");
            }
        }
        public IEnumerable<SelectListItem> StylesSelectListItem
        {
            get
            {
                return new SelectList(StyleList, "OrderStyleRefId", "StyleNo");
            }
        }

        public IEnumerable<SelectListItem> OrderSelectListItem
        {
            get
            {
                return new SelectList(OrderList, "OrderNo", "RefNo");
            }
        }

    }
}