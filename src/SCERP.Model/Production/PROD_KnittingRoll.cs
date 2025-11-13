using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommonModel;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Model.Production
{
   public class PROD_KnittingRoll
    {
       public PROD_KnittingRoll()
       {
           PROD_BatchRoll=new HashSet<PROD_BatchRoll>();
           PROD_KnittingRollIssueDetail = new HashSet<PROD_KnittingRollIssueDetail>();
       }
        public long KnittingRollId { get; set; }
        public string RollRefNo { get; set; }
        public System.DateTime RollDate { get; set; }
        public long PartyId { get; set; }
        public long ProgramId { get; set; }
        public int MachineId { get; set; }
        public string SizeRefId { get; set; }
        public string ColorRefId { get; set; }
        public string FinishSizeRefId { get; set; }
        public string GSM { get; set; }
        public string CharllRollNo { get; set; }
        public double? RollLength { get; set; }
        public double Quantity { get; set; }
        public string Rmks { get; set; }
        public string CompId { get; set; }
        public string ItemCode { get; set; }
        public string StLength { get; set; }
        public string ComponentRefId { get; set; }
        public double? RejQuantity { get; set; }
        public bool? IsRejected { get; set; }
        public virtual PLAN_Program PLAN_Program { get; set; }
        public virtual Party Party { get; set; }
        public virtual Production_Machine Production_Machine { get; set; }
        public ICollection<PROD_BatchRoll> PROD_BatchRoll { get; set; }
        public ICollection<PROD_KnittingRollIssueDetail> PROD_KnittingRollIssueDetail { get; set; }
       
    }
}
