using SCERP.Model.Production;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class CommCashLCDyesChemical : ProSearchModel<CommCashLCDyesChemical>
    {
        public int CashLcId { get; set; }
        [Required]
        public string CashLcNo { get; set; }
       
        public Nullable<System.DateTime> LcDate { get; set; }
        [Required]
        public string Item { get; set; }
        [Required]
        public Nullable<decimal> Quantity { get; set; }
        public string SupplierName { get; set; }
        public string PortOfDelivery { get; set; }
        public Nullable<System.DateTime> DateOfBL { get; set; }
        public string CountryOfOrigin { get; set; }
        public string PaymentTerms { get; set; }
        public string ShipmentMode { get; set; }
        public string BillOfEntry { get; set; }
        public string BillOfImportCode { get; set; }
        public Nullable<decimal> BillOfImport { get; set; }
        public Nullable<System.DateTime> DateOfBill { get; set; }
        public Nullable<decimal> LcValue { get; set; }
        public Nullable<int> BankRef { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public string Remarks { get; set; }

    }
}
