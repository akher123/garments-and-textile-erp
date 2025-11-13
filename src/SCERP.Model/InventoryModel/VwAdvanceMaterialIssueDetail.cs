
using System;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.InventoryModel
{
    public class VwAdvanceMaterialIssueDetail
    {
        public long AdvanceMaterialIssueDetailId { get; set; }
        public long AdvanceMaterialIssueId { get; set; }
        public string ColorRefId { get; set; }
        public string CompId { get; set; }
        public string GSizeRefId { get; set; }
        public string GSizeName { get; set; }
        public decimal IssueQty { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00000}", ApplyFormatInEditMode = true)]
        public decimal IssueRate { get; set; }
        public int ItemId { get; set; }
        public string FColorRefId { get; set; }
        public string FColorName { get; set; }
        public string GColorRefId { get; set; }
        public string GColorName { get; set; }
        public string Brand { get; set; }
        public Nullable<decimal> QtyInBag { get; set; }
        public string Wrapper { get; set; }
        public string SizeRefId { get; set; }
        public string BuyerName { get; set; }
        public int StoreId { get; set; }
        public string IRefId { get; set; }
        public System.DateTime IRNoteDate { get; set; }
        public string IRNoteNo { get; set; }
        public System.Guid IssuedBy { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public string SlipNo { get; set; }
        public string Remarks { get; set; }
        public int IType { get; set; }
        public string Employee { get; set; }
        public string RefEmployee { get; set; }
        public string Designation { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string ItemCode { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string CompanyName { get; set; }
        public string FullAddress { get; set; }
        public string PartyAddress { get; set; }
        public string ParyName { get; set; }
        public string VehecleNo { get; set; }
        public string DriverName { get; set; }
        public int CurrencyId { get; set; }
        public string Attention { get; set; }
        public string CurrencyName { get; set; }
        public string ApprovedByName { get; set; }
        public long? PurchaseOrderDetailId { get; set; }
        public decimal? TotalRcvQty { get; set; }
        public decimal? StockInHand { get; set; }
        

    }
}
