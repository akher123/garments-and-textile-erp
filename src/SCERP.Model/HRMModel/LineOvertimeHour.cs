using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
   public class LineOvertimeHour
    {
        public long LineOvertimeHourId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public int DepartmentLineId { get; set; }
        public string Line { get; set; }
        public int OvertimePerson { get; set; }
        public decimal OvertimeHour { get; set; }
        public string FirstSign { get; set; }
        public string SecondSign { get; set; }
        public double TTLOtAmount { get; set; }
        public Guid? FirstSignBy { get; set; }
        public Guid? SeconSignBy { get; set; }
        public Guid? PrepairedBy { get; set; }
    }
}
