using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.CommercialModel;
using SCERP.Model.CommonModel;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Commercial.Models.ViewModel
{
    public class PurchaseOrderViewModel : OM_BuyOrdStyle
    {
        public CommPurchaseOrder PurchaseOrder { get; set; }
        public long PurchaseOrderId { get; set; }
        public int SupplierId { get; set; }
        public string BuyerRefId { get; set; }
        public string PiRefId { get; set; }
        public IEnumerable<VwCommPurchaseOrder> PurchaseOrders { get; set; }
        public List<VOMBuyOrdStyle> OmBuyOrdStyles{ get; set; }
        public List<VwFabricOrderDetail> FabricOrderDetails { get; set; }
        public List<Mrc_SupplierCompany> SupplierCompanies { get; set; }
        public List<VwPurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public Dictionary<string, VwPurchaseOrderDetail> Details { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable Styles { get; set; }
        public List<ProFormaInvoice> ProFormaInvoices { get; set; }
        public PurchaseOrderViewModel()
        {
            this.ProFormaInvoices = new List<ProFormaInvoice>();
            Details =new Dictionary<string, VwPurchaseOrderDetail>();
            FabricOrderDetails=new List<VwFabricOrderDetail>();
            OmBuyOrdStyles=new List<VOMBuyOrdStyle>();
            PurchaseOrderDetails=new List<VwPurchaseOrderDetail>();
            PurchaseOrders = new List<VwCommPurchaseOrder>();
            PurchaseOrder=new CommPurchaseOrder();
            SupplierCompanies=new List<Mrc_SupplierCompany>();
            Buyers = new List<OM_Buyer>();
            OrderList = new List<object>();
            Styles = new List<object>();
        }
         
        public IEnumerable<SelectListItem> SupplierSelectListItem
        {
            get
            {
                return new SelectList(SupplierCompanies, "SupplierCompanyId", "CompanyName",PurchaseOrder.SupplierId);
            }
        }
        public IEnumerable<SelectListItem> ApprovalSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { IsApproval = false, Text = "Pending" }, new { IsApproval = true, Text = "Approved" } }, "IsApproval", "Text");
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

        public IEnumerable<SelectListItem> PiNoSelectListItem
        {
            get
            {
                return new SelectList(ProFormaInvoices, "PiRefId", "PiNo");
            }
        }
    }
}