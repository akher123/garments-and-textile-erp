using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MerchandisingModel
{
    public class VwThreadConsumption
    {
        public int ThreadConsumptionId { get; set; }
        public string BuyerRefId { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public string SizeRefId { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string Remarks { get; set; }
        public string CompId { get; set; }
        public string BuyerName { get; set; }
        public string OrderName { get; set; }
        public string StyleName { get; set; }
        public string SizeName { get; set; }
        public string ItemName { get; set; }

        public System.Guid CreatedBy { get; set; }
        public Nullable<System.Guid> ApprovedBy { get; set; }
        public bool IsApproved { get; set; }
    }
}
