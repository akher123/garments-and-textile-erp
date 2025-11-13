using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
   public class VwStockPosition
   {
       public Nullable<int> ItemId { get; set; }
       public string ItemCode { get; set; }
       public Nullable<decimal> OQty { get; set; }
       public Nullable<decimal> OAmt { get; set; }
       public Nullable<decimal> RQty { get; set; }
       public Nullable<decimal> RAmt { get; set; }
       public Nullable<decimal> IQty { get; set; }
       public Nullable<decimal> IAmt { get; set; }
       public Nullable<decimal> OutConsQty { get; set; }
       public string ItemName { get; set; }
       public string GenericName { get; set; }
       public string UnitName { get; set; }
       public string SubGroupName { get; set; }
       public string GroupName { get; set; }
       public string ColorName { get; set; }
       public string SizeName { get; set; }
       public string Brand { get; set; }
       public string BuyerName { get; set; }
    }
}
