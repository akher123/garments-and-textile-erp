using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class YarnConsumptionViewModel:VYarnConsumption
    {

        public List<VwFabricOrderDetail> OmBuyOrdStyles { get; set; }
        public List<VConsumption> Consumptions { get; set; }
        public VConsumption VConsumption { get; set; }
        public List<VCompConsumptionDetail> VCompConsumptionDetails { get; set; }
        public List<VYarnConsumption> VYarnConsumptions { get; set; }
        public string OrderNo { get; set; }
        public string BuyerRefId { get; set; }
        public decimal TQty { get; set; }
        public string ButtonName { get; set; }
        public VOMBuyOrdStyle OrdStyle { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable BuyerOrderStyles { get; set; }
        public YarnConsumptionViewModel()
        {
            Buyers = new List<OM_Buyer>();
            VCompConsumptionDetails = new List<VCompConsumptionDetail>();
            VYarnConsumptions=new List<VYarnConsumption>();
            Consumptions=new List<VConsumption>();
            Buyers = new List<OM_Buyer>();
            OrdStyle=new VOMBuyOrdStyle();
            OmBuyOrdStyles = new List<VwFabricOrderDetail>();
            OrderList = new List<object>();
            BuyerOrderStyles = new List<object>();

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
                return new SelectList(BuyerOrderStyles, "OrderStyleRefId", "StyleName");
            }
        }
    }

}