using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
    public partial class SkillMatrixDetail
    {
        public int SkillMatrixDetailId { get; set; }
        public int SkillMatrixId { get; set; }
        public Nullable<int> MachineTypeId { get; set; }
        public Nullable<int> ProcessId { get; set; }
        public Nullable<double> ProcessSmv { get; set; }
        public string ProcessGrade { get; set; }
        public Nullable<double> AverageCycle { get; set; }
        public Nullable<double> StandardProcessSmv { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
