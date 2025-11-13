using System;
using System.ComponentModel.DataAnnotations;
using SCERP.Common;

namespace SCERP.Model.InventoryModel
{
   public class VwAdvanceMaterialIssue
    {
        public long AdvanceMaterialIssueId { get; set; }
        public string CompId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
      
        public string IRefId { get; set; }
         [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string SlipNo { get; set; }
         [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string IRNoteNo { get; set; }
         [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public System.DateTime IRNoteDate { get; set; }
         [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string OrderNo { get; set; }
         [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string StyleNo { get; set; }
         [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public int StoreId { get; set; }
        public int IType { get; set; }
        public string Remarks { get; set; }
        public System.Guid IssuedBy { get; set; }
        
        public string Employee { get; set; }
       public decimal Amount { get; set; }
       [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
       public string RefEmployee { get; set; }
       public System.Guid? RefPerson { get; set; }
       public string CompanyName { get; set; }
       public string FullAddress { get; set; }
       [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
       public int? PartyId { get; set; }
           [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
       public string ProcessRefId { get; set; }
              [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
       public string VehicleNo { get; set; }
              [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
       public string DriverName { get; set; }
       public string PartyName { get; set; }

       public string BuyerName { get; set; }
       public string BuyerRefId { get; set; }
       public bool? LockStatus { get; set; }
       public string OrderStyleRefId { get; set; }
       public string ProgramRefId { get; set; }
       public Guid? ApprovedBy { get; set; }
       public int? GroupChallanId { get; set; }
    }
}
