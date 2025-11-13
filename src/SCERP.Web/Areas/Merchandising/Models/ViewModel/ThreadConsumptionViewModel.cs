using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class ThreadConsumptionViewModel : ProSearchModel<ThreadConsumptionViewModel>
    {
        public string Key { get; set; }
        public List<VwThreadConsumption> Consumptions { get; set; }
        public Dictionary<string,OM_ThreadConsumptionDetail> ConsumptionDetails { get; set; }
        public OM_ThreadConsumption Consumption { get; set; }
        public OM_ThreadConsumptionDetail ConsumptionDetail { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable Styles { get; set; }
        public IEnumerable SizeList { get; set; }
        public ThreadConsumptionViewModel()
        {
            ConsumptionDetail=new OM_ThreadConsumptionDetail();
            ConsumptionDetails=new Dictionary<string, OM_ThreadConsumptionDetail>();
            Consumptions=new List<VwThreadConsumption>();
            Consumption=new OM_ThreadConsumption();
            Buyers = new List<OM_Buyer>();
            OrderList = new List<object>();
            Styles = new List<object>();
            SizeList = new List<object>();
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

        public IEnumerable<SelectListItem> SizeSelectListItem
        {
            get
            {
                return new SelectList(SizeList, "SizeRefId", "SizeName");
            }
        }
    }
}