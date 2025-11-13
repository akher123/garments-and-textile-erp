using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
    public class VwMaterialReceiveAgainstPo
    {
        public long MaterialReceiveAgstPoId { get; set; }
        public string RefNo { get; set; }
        public string CompId { get; set; }
        public string LotNo { get; set; }
        public Nullable<long> BuyerId { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
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
        public bool QcStatus { get; set; }
        public bool GrnStatus { get; set; }
        public Nullable<System.DateTime> QcDate { get; set; }
        public Nullable<System.DateTime> GrnDate { get; set; }
        public string CompanyName { get; set; }
        public string FullAddress { get; set; }
        public string Supplier { get; set; }
        public string LcNo { get; set; }
        public string GrnRemarks { get; set; }
        public string QcRemarks { get; set; }
        public string OrderStyleRefId { get; set; }
    }
}
