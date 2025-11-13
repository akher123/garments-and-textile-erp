using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
    public class VwKnittingRollIssueDetail
    {
        public long? KnittingRollIssueDetailId { get; set; }
        public int? KnittingRollIssueId { get; set; }
        public long KnittingRollId { get; set; }
        public double? RollQty { get; set; }
        public string RollRefNo { get; set; }
        public int CountIssue { get; set; }
        public System.DateTime RollDate { get; set; }
     
        public long PartyId { get; set; }
        public long ProgramId { get; set; }
        public string Buyer { get; set; }
        public string OrderNo { get; set; }
        public string StyleName { get; set; }
        public string OrderStyleRefId { get; set; }
 
        public int MachineId { get; set; }
        public string SizeRefId { get; set; }
        public string ColorRefId { get; set; }
        public string FinishSizeRefId { get; set; }
 
        public string GSM { get; set; }
       
        public double Quantity { get; set; }
        public string Rmks { get; set; }
 
        public string CompId { get; set; }

        public string ItemCode { get; set; }
   
        public string ItemName { get; set; }
        public string PartyName { get; set; }
 
        public string ProgramRefId { get; set; }
        public string MachineName { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public string FinishSizeName { get; set; }

        public string CharllRollNo { get; set; }
        public double? RollLength { get; set; }
        public string ComponentName { get; set; }
        public string ComponentRefId { get; set; }

    }
}
