using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Planning
{
   public class VwAssignedProgram
    {
        public string CompId { get; set; }
        public string OrderStyleRefId { get; set; }
        public string StyleName { get; set; }
        public string OrderNo { get; set; }
        public string RefNo { get; set; }
        public long ProgramId { get; set; }
        public string ProgramRefId { get; set; }
        public string ProcessRefId { get; set; }
        public string ProcessName { get; set; }
        public int BookedQty { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<System.DateTime> PrgDate { get; set; }
        public Nullable<System.DateTime> ExpDate { get; set; }
        public string xStatus { get; set; }

       public int RemainingQty
       {
           get
           {
               return Convert.ToInt32(Quantity ?? 0) - BookedQty;
           }
       }
    }
}
