using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.Model.MerchandisingModel
{
    public class PROD_KnittingRollIssueDetail
    {
        public long KnittingRollIssueDetailId { get; set; }
        public int KnittingRollIssueId { get; set; }
        public long KnittingRollId { get; set; }
        [Required]
        public double RollQty { get; set; }
        public virtual PROD_KnittingRoll PROD_KnittingRoll { get; set; }
        public virtual PROD_KnittingRollIssue PROD_KnittingRollIssue { get; set; }
    }
}
