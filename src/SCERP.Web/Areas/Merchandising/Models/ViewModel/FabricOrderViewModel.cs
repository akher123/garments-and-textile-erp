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
    public class FabricOrderViewModel : ProSearchModel<FabricOrderViewModel>
    {
        public List<VwFabricOrder> FabricOrders { get; set; }
        public OM_FabricOrder FabricOrder { get; set; }
        public List<VwCompConsumptionOrderStyle> OmBuyOrdStyles { get; set; }
        public List<object> StatusList { get;set; }
        public FabricOrderViewModel()
        {
            StatusList = new List<object>
            {
                new {Text = "Pending", Value = "P"},
                new {Text = "Approved", Value = "A"},
                new {Text = "Closed", Value = "C"}
            };
            FabricOrders = new List<VwFabricOrder>();
            FabricOrder=new OM_FabricOrder();
            OmBuyOrdStyles=new List<VwCompConsumptionOrderStyle>();
            Buyers=new List<OM_Buyer>();
            OrderList =new List<OM_BuyerOrder>();
        }
        public List<OM_Buyer> Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
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
        public List<Mrc_SupplierCompany> SuppliersList { get; set; }
        public IEnumerable<SelectListItem> SupplierSelectListItem
        {
            get
            {
                return new SelectList(SuppliersList, "SupplierCompanyId", "CompanyName");
            }
        }
        public IEnumerable<SelectListItem> StatusSeectListItem
        {
            get
            {
                return new SelectList(StatusList, "Value", "Text");
            }
        }
    }
}