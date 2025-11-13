using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
   public class VwMaterialReceiveAgainstPoDetail{

       public long MaterialReceiveAgstPoDetailId { get; set; }
       public long MaterialReceiveAgstPoId { get; set; }
       public int ItemId { get; set; }
       public string ColorRefId { get; set; }
       public string SizeRefId { get; set; }
       public decimal ReceivedQty { get; set; }
       [DisplayFormat(DataFormatString = "{0:0.####}")]
       public decimal ReceivedRate { get; set; }
       public Nullable<decimal> RejectedQty { get; set; }
       public Nullable<decimal> DiscountQty { get; set; }
       public string ColorName { get; set; }
       public string SizeName { get; set; }
       public string ItemName { get; set; }
       public string RefNo { get; set; }
       public string CompId { get; set; }
       public int SupplierId { get; set; }
       public int StoreId { get; set; }
       public string MRRNo { get; set; }
       public System.DateTime MRRDate { get; set; }
       public string VoucherNo { get; set; }
       public string InvoiceNo { get; set; }
       public System.DateTime InvoiceDate { get; set; }
       public string ReceiveRegNo { get; set; }
       public System.DateTime ReceiveRegDate { get; set; }
       public string GateEntryNo { get; set; }
       public System.DateTime GateEntryDate { get; set; }
       public string PoNo { get; set; }
       public string RType { get; set; }
       public Nullable<System.DateTime> PoDate { get; set; }
       public decimal TotalAmount { get; set; }
       public string PayRef { get; set; }
       public Nullable<decimal> TotalDiscount { get; set; }
       public Nullable<decimal> NetAmount { get; set; }
       public string Remarks { get; set; }
       public Nullable<System.Guid> EmployeeId { get; set; }
       public bool GrnStatus { get; set; }
       public string CompanyName { get; set; }
       public string Name { get; set; }
       public string FullAddress { get; set; }
       public string EmployeeName { get; set; }
       public string ItemCode { get; set; }
       public string UnitName { get; set; }
       public string Supplier { get; set; }
       public string GrnRemarks { get; set; }
       public string LcNo { get; set; }
       public string LotNo { get; set; }
       public string Brand { get; set; }
       public string SupplierAddress { get; set; }
       public string GSizeName { get; set; }
       public string GSizeRefId { get; set; }
       public string GizeName{ get; set; }
       public string GColorName { get; set; }
       public decimal? TotalRcvQty { get; set; }
       public decimal? BalanceQty { get; set; }
       public Nullable<System.DateTime> GrnDate { get; set; }
       public Nullable<System.DateTime> QcDate { get; set; }
       public string FColorName { get; set; }
       public String FColorRefId { get; set; }
       public long? PurchaseOrderDetailId { get; set; }
       public string PurchaseOrderRefId { get; set; }
       public string Location { get; set; }
        public string OrderStyleRefId { get; set; }
        public string StyleName { get; set; }
        [NotMapped]
        public decimal? TBookingQty
        {
            get { return TotalRcvQty??0 + BalanceQty??0; }
        }
    }
}
