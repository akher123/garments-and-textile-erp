using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
    public class AccessoriesReceiveBalance
    {
        public long AdvanceMaterialIssueDetailId { get; set; }
        public long AdvanceMaterialIssueId { get; set; }
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public decimal Rate { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string ItemName { get; set; }
        public string GSizeName { get; set; }
        public string GSizeRefId { get; set; }
        public string GizeName { get; set; }
      
        public decimal? TotalRcvQty { get; set; }
        public decimal ToalIssueQty { get; set; }
        public string FColorName { get; set; }
        public String FColorRefId { get; set; }
        public long? PurchaseOrderDetailId { get; set; }
    }
}
