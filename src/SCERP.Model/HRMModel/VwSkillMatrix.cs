using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model
{

    public partial class VwSkillMatrix
    {
        public int SkillMatrixDetailId { get; set; }
        public int SkillMatrixId { get; set; }
        public int SkillMatrixProcessId { get; set; }
        public int ProcessPercentage { get; set; }
        public int SkillMatrixGradeId { get; set; }
        public string CompId { get; set; }
        public bool IsActive { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string GradeName { get; set; }
        public string ProcessName { get; set; }
    }
}
