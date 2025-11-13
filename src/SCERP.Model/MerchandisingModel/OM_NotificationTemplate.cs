using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MerchandisingModel
{
    public class OM_NotificationTemplate
    {
        public int NotificationTemplateId { get; set; }
        public string CompId { get; set; }
        public string BuyerRefId { get; set; }
        public int ActivityId { get; set; }
        public double BeforeDays { get; set; }
        public string Receiver { get; set; }
   
        public  string Remarks { get; set; }
        public virtual OM_TnaActivity OM_TnaActivity { get; set; }

    }
}
