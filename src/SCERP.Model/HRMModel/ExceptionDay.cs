using System;
using System.Collections.Generic;
namespace SCERP.Model.HRMModel
{


    public partial class ExceptionDay : HrmSearchModel<ExceptionDay>
    {
      
        public int ExceptionDayId { get; set; }
        public int BranchUnitId { get; set; }
        public System.DateTime ExceptionDate { get; set; }
        public bool IsExceptionForWeekend { get; set; }
        public bool IsExceptionForHoliday { get; set; }
        public bool IsExceptionForGeneralDay { get; set; }
        public bool IsDeclaredAsWeekend { get; set; }
        public bool IsDeclaredAsHoliday { get; set; }
        public bool IsDeclaredAsGeneralDay { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
        public BranchUnit BranchUnit { get; set; }
    }
}
