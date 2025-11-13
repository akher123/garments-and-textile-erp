using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCERP.Model.Production
{
    public partial class VwSewingOutput
    {
       public long SewingOutPutProcessId { get; set; }
        public string CompId { get; set; }
        public string OrderStyleRefId { get; set; }
        public string ColorRefId { get; set; }
        public int OutputQuantity { get; set; }
        public string SizeRefId { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public Nullable<int> OrderQty { get; set; }
        public int TotalInput { get; set; }
        public int TotalOutput { get; set; }
        public int ManPower { get; set; }
        public int? QcRejectQty { get; set; }
        public string OrderShipRefId { get; set; }
    }
}
