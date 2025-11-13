using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
    public partial class SkillMatrix
    {
        public int SkillMatrixId { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public Nullable<double> Performance { get; set; }
        public Nullable<double> Attitude { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
