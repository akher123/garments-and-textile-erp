using System;
using System.ComponentModel.DataAnnotations;
using SCERP.Common;
using SCERP.Model.Production;

namespace SCERP.Model.InventoryModel
{
    public class VMaterialLoanReturn : ProSearchModel<VMaterialLoanReturn>
    {
        public int MaterialIssueId { get; set; }
        public int MaterialIssueRequisitionId { get; set; }
        public string StoreEmployeeName { get; set; }
        public string IssueReceiveNo { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public DateTime? IssueReceiveDate { get; set; }
        [Required(ErrorMessage = @"Required!")]
        public string IssueReceiveNoteNo { get; set; }
        public DateTime? IssueReceiveNoteDate { get; set; }
        [Required(ErrorMessage = @"Required!")]
        public int? SupplierId { get; set; }
        public int IType { get; set; }
        public string LoanRefNo { get; set; }
        public string Supplyer { get; set; }
        public string Remarks { get; set; }
   
    }
}
