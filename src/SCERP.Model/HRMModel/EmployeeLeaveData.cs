using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model
{
    public class EmployeeLeaveData
    {
        public string Title { get; set; }
        public int? Total { get; set; }
        public int? Allowed { get; set; }
        public int? Available { get; set; }
    }
}
