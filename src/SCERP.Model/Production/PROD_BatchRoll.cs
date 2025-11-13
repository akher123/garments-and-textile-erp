using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Model.Production
{
   public class PROD_BatchRoll
    {
        public long BatchRollId { get; set; }
        public long BatchId { get; set; }
        public long KnittingRollId { get; set; }
        public string Remarks { get; set; }
        public System.Guid CreatedBy { get; set; }
        public Nullable<System.Guid> EditdBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public string CompId { get; set; }
        public int KnittingRollIssueId { get; set; }
        public virtual PROD_KnittingRollIssue PROD_KnittingRollIssue { get; set; }
        public virtual PROD_KnittingRoll PROD_KnittingRoll { get; set; }
        public virtual Pro_Batch Pro_Batch { get; set; }
    }
}
