using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MerchandisingModel
{
    public class OM_ActivityResponsible
    {
        public int ActivityResponsibleId { get; set; }
        public int ActivityId { get; set; }
        public string CompId { get; set; }
        public string RespobsibleName { get; set; }
        public string RespobsiblePhone { get; set; }
    }
}
