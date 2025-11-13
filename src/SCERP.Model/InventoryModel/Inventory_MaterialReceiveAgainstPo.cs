using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SCERP.Common;
namespace SCERP.Model.InventoryModel
{
   public partial  class Inventory_MaterialReceiveAgainstPo
    {
        public Inventory_MaterialReceiveAgainstPo()
        {
            this.Inventory_MaterialReceiveAgainstPoDetail = new HashSet<Inventory_MaterialReceiveAgainstPoDetail>();
        }

        public long MaterialReceiveAgstPoId { get; set; }
        public string RefNo { get; set; }
        public string CompId { get; set; }
        public int SupplierId { get; set; }
        public int StoreId { get; set; }
       [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string MRRNo { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public System.DateTime MRRDate { get; set; }
        public string VoucherNo { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string InvoiceNo { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
     
        public System.DateTime InvoiceDate { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string ReceiveRegNo { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public System.DateTime ReceiveRegDate { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string GateEntryNo { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public System.DateTime GateEntryDate { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string PoNo { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string RType { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public Nullable<System.DateTime> PoDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PayRef { get; set; }
        public Nullable<decimal> TotalDiscount { get; set; }
        public Nullable<decimal> NetAmount { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.Guid> EmployeeId { get; set; }
        public bool QcStatus { get; set; }
        public bool GrnStatus { get; set; }
       public long? BuyerId { get; set; }
       public string OrderNo { get; set; }
       public string StyleNo { get; set; }
       public DateTime? QcDate { get; set; }
       public DateTime? GrnDate { get; set; }
       public string LcNo { get; set; }
       public string GrnRemarks { get; set; }
       public string QcRemarks { get; set; }
       public string OrderStyleRefId { get; set; }
       public virtual Mrc_SupplierCompany Mrc_SupplierCompany { get; set; }
       public virtual ICollection<Inventory_MaterialReceiveAgainstPoDetail> Inventory_MaterialReceiveAgainstPoDetail { get; set; }
    }
}
