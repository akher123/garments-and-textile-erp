using System.Collections.Generic;

namespace SCERP.Model.MerchandisingModel
{
    public class OM_TnaActivity
    {
        public OM_TnaActivity()
        {
            OM_NotificationTemplate=new HashSet<OM_NotificationTemplate>();
        }
        public int ActivityId { get; set; }
        public int SlNo { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Responsible { get; set; }
        public int? MaskId { get; set; }
        public string XStatus { get; set; }
        public virtual ICollection<OM_NotificationTemplate> OM_NotificationTemplate { get; set; } 
    }
}
