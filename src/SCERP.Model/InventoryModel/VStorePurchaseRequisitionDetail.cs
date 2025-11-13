using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
    public partial class VStorePurchaseRequisitionDetail
    {
        public int StorePurchaseRequisitionDetailId { get; set; }
        public int StorePurchaseRequisitionId { get; set; }
        public string Description { get; set; }
        public decimal? Quantity { get; set; }
        public decimal PresentRate { get; set; }
        public bool  IsReceived { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DesiredDate { get; set; }
        public string FunctionalArea { get; set; }
        public string StockInHand { get; set; }
        public decimal? SuppliedUptoDate { get; set; }
        public Nullable<decimal> LastUnitPrice { get; set; }
        public string EstimatedYearlyRequirement { get; set; }
        public Nullable<decimal> ModifiedRequiredQuantity { get; set; }
      
        public Nullable<decimal> ApprovedQuantity { get; set; }
        public Nullable<System.DateTime> ApprovalDate { get; set; }
        public string RemarksOfRequisitionApprovalPerson { get; set; }
        public string RemarksOfPurchaseApprovalPerson { get; set; }
        public string Quotation { get; set; }
        public string ApprovedPurchase { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string MeasurementUnit { get; set; }
        public string BrandName { get; set; }
        public string SizeName { get; set; }
        public string Origin { get; set; }
        public string ApprovalStatus { get; set; }
        public Nullable<int> SizeId { get; set; }
        public Nullable<int> BrandId { get; set; }
        public Nullable<int> OriginId { get; set; }
        public int ApprovalStatusId { get; set; }
        public int ItemId { get; set; }
    }
}
