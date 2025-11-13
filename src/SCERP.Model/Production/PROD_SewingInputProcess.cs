using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.Production
{
    
    
    public partial class PROD_SewingInputProcess
    {
        public PROD_SewingInputProcess()
        {
            this.PROD_SewingInputProcessDetail = new HashSet<PROD_SewingInputProcessDetail>();
            this.PROD_SewingOutPutProcess = new HashSet<PROD_SewingOutPutProcess>();
        }
    
        public long SewingInputProcessId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string SewingInputProcessRefId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string BuyerRefId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string OrderNo { get; set; }
        [Required(ErrorMessage = "Required")]
        public string OrderStyleRefId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string ColorRefId { get; set; }
        public int LineId { get; set; }
        public Nullable<System.DateTime> InputDate { get; set; }
        public string CompId { get; set; }
        public System.Guid PreparedBy { get; set; }
        public string Remarks { get; set; }
          [Required(ErrorMessage = "Required")]
        public int? HourId { get; set; }
        public string BatchNo { get; set; }
        public string JobNo { get; set; }
        [Required(ErrorMessage = "Required")]
        public string OrderShipRefId { get; set; }
        public bool? Locked { get; set; }
        public System.Guid? LockedBy { get; set; }
        public virtual ICollection<PROD_SewingInputProcessDetail> PROD_SewingInputProcessDetail { get; set; }
        public virtual ICollection<PROD_SewingOutPutProcess> PROD_SewingOutPutProcess { get; set; }
        public virtual Production_Machine Production_Machine { get; set; }
    }
}
