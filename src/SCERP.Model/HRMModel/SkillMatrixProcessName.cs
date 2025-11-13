using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
    public partial class SkillMatrixProcessName
    {
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string ProcessDescription { get; set; }
        public Nullable<double> StandardProcessSmv { get; set; }
        public Nullable<int> MachineTypeId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
