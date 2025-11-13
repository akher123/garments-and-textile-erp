using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
   public class VInvItem
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public int SubGroupId { get; set; }
        public string SubGroupName { get; set; }
        public string SubGroupCode { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public Nullable<decimal> ReorderLevel { get; set; }
        public string UnitName { get; set; }
        public Nullable<int> UnitId { get; set; }
    }
}
