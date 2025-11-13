using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCERP.Model.Production
{
    
    
    public partial class PROD_SewingOutPutProcessDetail
    {
        public long SewingOutPutProcessDetailId { get; set; }
        public long SewingOutPutProcessId { get; set; }
        public string SizeRefId { get; set; }
        public int Quantity { get; set; }
        public string CompId { get; set; }
        public int? QcRejectQty { get; set; }
        public virtual PROD_SewingOutPutProcess PROD_SewingOutPutProcess { get; set; }
    }
}
