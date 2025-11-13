using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Planning
{
   public class VwProgram
    {
       public long PartyId { get; set; }
        public long ProgramId { get; set; }
        public string Buyer { get; set; }
        public string BuyerRefId { get; set; }
        public string MerchandiserId { get; set; }
        public string Merchandiser { get; set; }
        public string OrderNo { get; set; }
        public string RefNo { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string OrderStyleRefId { get; set; }
        public string ProgramRefId { get; set; }
        public Nullable<System.DateTime> PrgDate { get; set; }
        public Nullable<System.DateTime> ExpDate { get; set; }
        public string ProcessName { get; set; }
        public string ProcessorName { get; set; }
        public string ProcessRefId { get; set; }
        public string xStatus { get; set; }
        public string Attention { get; set; }
        public string CGRID { get; set; }
        public string CID { get; set; }
        public string ProgramType { get; set; }
        public string CompId { get; set; }
        public string StyleName { get; set; }
        public decimal ReqYarnQty { get; set; }
        public decimal ReqFabQty { get; set; }
        public decimal YarnDeliveryQty { get; set; }
        public decimal? YarnReurnQty { get; set; }
        public double? RcvQty { get; set; }
        public bool IsApproved { get; set; }
        public string ApproverName { get; set; }
        public string PrepareName { get; set; }
        public string Unit { get { return "Pcs"; } }
       public string PartyName { get; set; }
       public string ProcessorRefId { get; set; }
       public string Rmks { get; set; }
       public string PartyArddress { get; set; }
       public bool? IsLock { get; set; }
       public int? QtyInPcs { get; set; }
      
    }
}
