using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
    public class SpHrmCuttingSectionAbsent
    {
        public string EmployeeCardId { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Type { get; set; }
        public string Designation { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public string Present { get; set; }
        public string MobilePhone { get; set; }
        public string Remarks { get; set; }
    }
}
