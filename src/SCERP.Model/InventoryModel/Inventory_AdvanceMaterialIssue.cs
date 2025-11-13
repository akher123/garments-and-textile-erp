using System;
using System.Collections.Generic;

namespace SCERP.Model.InventoryModel
{
    public class Inventory_AdvanceMaterialIssue
    {
        public Inventory_AdvanceMaterialIssue()
        {
            this.Inventory_AdvanceMaterialIssueDetail = new HashSet<Inventory_AdvanceMaterialIssueDetail>();
        }

        public long AdvanceMaterialIssueId { get; set; }
        public string CompId { get; set; }
        public string IRefId { get; set; }
        public string SlipNo { get; set; }
        public string IRNoteNo { get; set; }
        public System.DateTime IRNoteDate { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public int StoreId { get; set; }
        public int IType { get; set; }
        public string Remarks { get; set; }
        public System.Guid IssuedBy { get; set; }
        public decimal Amount { get; set; }
        public System.Guid? RefPerson { get; set; }
        public int? PartyId{ get; set; }
        public string ProcessRefId { get; set; }
        public string VehicleNo { get; set; }
        public string DriverName { get; set; }
        public string BuyerRefId { get; set; }
        public bool? LockStatus{ get; set; }
        public string OrderStyleRefId { get; set; }
        public string ProgramRefId { get; set; }
        public Guid? ApprovedBy { get; set; }
        public int? GroupChallanId { get; set; }
      
        public virtual ICollection<Inventory_AdvanceMaterialIssueDetail> Inventory_AdvanceMaterialIssueDetail { get; set; }
    }
}
