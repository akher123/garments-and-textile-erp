using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
    public class EmployeeLeaveHistoryIndividual
    {
        public string EmployeeCardId { get; set; }
        public string LeaveTypeTitle { get; set; }
        public int Year { get; set; }
        public int TotalConsumed { get; set; }
        public int TotalAllowed { get; set; }
        public int TotalAvailable { get; set; }
    }
}