using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
    public partial class SkillMatrixPointTable
    {
        public int PointId { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public Nullable<double> ExperienceYear { get; set; }
        public Nullable<double> ExperiencePoint { get; set; }
        public Nullable<double> ExperienceWPoint { get; set; }
        public Nullable<double> MultiMCTtl { get; set; }
        public Nullable<double> MultiMCPoint { get; set; }
        public Nullable<double> MultiMCWPoint { get; set; }
        public Nullable<double> MultiProcessTtl { get; set; }
        public Nullable<double> MultiProcessPoint { get; set; }
        public Nullable<double> MultiProcessWPoint { get; set; }
        public Nullable<double> ProcessGradePoint { get; set; }
        public Nullable<double> ProcessGradeWPoint { get; set; }
        public string AttitudeGrade { get; set; }
        public Nullable<double> AttitudePoint { get; set; }
        public Nullable<double> AttitudeWPoint { get; set; }
        public Nullable<double> PerformancePercentage { get; set; }
        public Nullable<double> PerformancePoint { get; set; }
        public Nullable<double> PerformanceWPoint { get; set; }
        public Nullable<double> TotalPoints { get; set; }
        public string Grading { get; set; }
        public Nullable<decimal> ProcessedWages { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
