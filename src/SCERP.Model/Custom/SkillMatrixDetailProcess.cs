using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
   public class SkillMatrixDetailProcess
    {
        public int SkillMatrixDetailId { get; set; }
        public int SkillMatrixId { get; set; }
        public Nullable<int> MachineTypeId { get; set; }
        public Nullable<int> ProcessId { get; set; }
        public string ProcessName { get; set; }
        public Nullable<double> ProcessSmv { get; set; }
        public string ProcessGrade { get; set; }
        public Nullable<double> AverageCycle { get; set; }
        public Nullable<double> StandardProcessSmv { get; set; }
    }
}
