using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
    public class EmployeeAttendanceIndividual
    {
        public int Year { get; set; }
        public string EmployeeCardId { get; set; }
        public int Present { get; set; }
        public int Late { get; set; }
        public int Absent { get; set; }
        public int Leave { get; set; }
    }
}
