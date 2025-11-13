using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
    public class WorkShiftRoster : SearchModel<Company>
    {
        public WorkShiftRoster()
        {
            shiftDetail = new List<WorkShiftRosterDetail>();
        }
        public string UnitName { get; set; }
        public string GroupName { get; set; }    
        public string ShiftName { get; set; }
        public string EmployeeCardId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime? FromDate { get; set; }
        public List<WorkShiftRosterDetail> shiftDetail { get; set; }
    }
}