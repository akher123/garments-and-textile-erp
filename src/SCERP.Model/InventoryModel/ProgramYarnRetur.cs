using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
   public  class ProgramYarnRetur
    {
        public long ProgramId { get; set; }
        public string CompId { get; set; }
        public string PrgramRefId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public string LotRefId { get; set; }
        public string LotName { get; set; }
        public decimal PgQty { get; set; }
        public decimal IRate { get; set; }
    }
}
