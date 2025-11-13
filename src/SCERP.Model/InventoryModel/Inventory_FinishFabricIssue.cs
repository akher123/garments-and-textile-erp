using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SCERP.Common;
using SCERP.Model.CommonModel;
namespace SCERP.Model.InventoryModel
{
    public class Inventory_FinishFabricIssue
    {
        public Inventory_FinishFabricIssue()
        {
            this.Inventory_FinishFabricIssueDetail = new HashSet<Inventory_FinishFabricIssueDetail>();
        }
        public long FinishFabIssueId { get; set; }
        public string FinishFabIssureRefId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public long PartyId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string ChallanNo { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public System.DateTime ChallanDate { get; set; }
        public string VehicleType { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public string Remarks { get; set; }
        public System.Guid CreatedBy { get; set; }
        public Guid? EditedBy { get; set; }
        public Guid? ApprovedBy { get; set; }
        public bool? IsApproved { get; set; }
        public Guid? ReceivedBy { get; set; }
        public bool? IsReceived { get; set; }
        public System.DateTime? ReceivedDate { get; set; }
        public string Commerns { get; set; }
        public long? VoucherMasterId { get; set; }
        public string Posted { get; set; }
        public virtual Party Party { get; set; }
     
        public string CompId { get; set; }
        public virtual ICollection<Inventory_FinishFabricIssueDetail> Inventory_FinishFabricIssueDetail { get; set; }
    }
}
