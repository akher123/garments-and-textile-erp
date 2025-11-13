using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.Model.MerchandisingModel
{
    public class PROD_KnittingRollIssue
    {
        public PROD_KnittingRollIssue()
        {
            this.PROD_KnittingRollIssueDetail = new HashSet<PROD_KnittingRollIssueDetail>();
            this.PROD_BatchRoll=new HashSet<PROD_BatchRoll>();
        }

        public int KnittingRollIssueId { get; set; }
        public string IssueRefNo { get; set; }
        [Required]
        public string BuyerRefId { get; set; }
        [Required]
        public string OrderNo { get; set; }
        [Required]
        public string OrderStyleRefId { get; set; }
        [Required]
        public System.DateTime IssueDate { get; set; }
        [Required]
        public string BatchNo { get; set; }
        public double Qty { get; set; }
        public string CompId { get; set; }
        public string Remarks { get; set; }
        public bool? IsRecived { get; set; }
        [Required]
        public int ChallanType { get; set; }
        public long? VoucherMasterId { get; set; }
        public string Posted { get; set; }
        public Nullable<System.Guid> ReceivedBy { get; set; }

        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.Guid> Editedby { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public string ProgramRefId { get; set; }
          public virtual ICollection<PROD_BatchRoll> PROD_BatchRoll { get; set; }
        public virtual ICollection<PROD_KnittingRollIssueDetail> PROD_KnittingRollIssueDetail { get; set; }
     
     
    }
}
