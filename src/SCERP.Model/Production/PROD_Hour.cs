using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace SCERP.Model.Production
{

    
    public partial class PROD_Hour
    {
        public PROD_Hour()
        {
            this.PROD_SewingOutPutProcess = new HashSet<PROD_SewingOutPutProcess>();
            this.PROD_FinishingProcess = new HashSet<PROD_FinishingProcess>();
        }
    
        public int HourId { get; set; }
        public string HourRefId { get; set; }
        public string HourName { get; set; }
        public string Status { get; set; }
        public string CompId { get; set; }

        public virtual ICollection<PROD_FinishingProcess> PROD_FinishingProcess { get; set; }
        public virtual ICollection<PROD_SewingOutPutProcess> PROD_SewingOutPutProcess { get; set; }
    }
}
