using System.Collections.Generic;
using SCERP.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.Custom
{
    public class SearchFieldModel
    {

        public string SearchByEmployeeCardId { get; set; }

        [Display(Name = @"From date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = @"To date")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = @"Required")]
        public int SearchByCompanyId { get; set; }

        [Required(ErrorMessage = @"Required")]
        public int SearchByBranchId { get; set; }

        [Required(ErrorMessage = @"Required")]
        public int SearchByDepartmentId { get; set; }

        [Required(ErrorMessage = @"Required")]
        public int SearchBySectionId { get; set; }

        [Required(ErrorMessage = @"Required")]
        public int SearchByWorkGroupId { get; set; }


        public int SearchByWorkShiftId { get; set; }

        [Required(ErrorMessage = @"Required")]
        public int SearchByUnitId { get; set; }

        [Required(ErrorMessage = @"Required")]

        public int SearchByLineId { get; set; }


        public int SearchByEmployeeTypeId { get; set; }

        [Required(ErrorMessage = @"Required")]
        public int SearchByLeaveTypeId { get; set; }


        [Required(ErrorMessage = @"Required")]
        public int SearchByBranchUnitId { get; set; }

        [Required(ErrorMessage = @"Required")]
        public int SearchByUnitDepartmentId { get; set; }

        [Required(ErrorMessage = @"Required")]
        public int SearchByBranchUnitDepartmentId { get; set; }

        public int SearchByBranchUnitWorkShiftId { get; set; }

        [Required(ErrorMessage = @"Required")]
        public int SearchByDepartmentSectionId { get; set; }

        [Required(ErrorMessage = @"Required")]
        public int SearchByDepartmentLineId { get; set; }

        public string SearchByWorkGroupName { get; set; }

        public string SearchByEmployeeName { get; set; }

        public string SearchByEmployeeMobileNo { get; set; }

        public int SearchByEmployeeBloodGroupId { get; set; }

        public int SearchByEmployeeGenderId { get; set; }

        public int SearchByEmployeeGradeId { get; set; }

        public int SearchByEmployeeDesignationId { get; set; }

        public int SearchByEmployeeReligionId { get; set; }

        public int SearchByEmployeeMaritalStateId { get; set; }

        [Required(ErrorMessage = @"Required")]
        public int? ReasonTypes { get; set; }

        [Required(ErrorMessage = @"Required")]
        public int SearchLanguageId { get; set; }

        public Guid SearchByEmployeeId { get; set; }

        public string SearchByUserName { get; set; }

        public int SearchByEmployeeStatus { get; set; }

        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public int NoCard { get; set; }

        public int PrintFormatId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? JoiningDateBegin { get; set; }

        [DataType(DataType.Date)]
        public DateTime? JoiningDateEnd { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ConfirmationDateBegin { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ConfirmationDateEnd { get; set; }

        [DataType(DataType.Date)]
        public DateTime? QuitDateBegin { get; set; }

        [DataType(DataType.Date)]
        public DateTime? QuitDateEnd { get; set; }

        public string BirthDayMonth { get; set; }

        [DataType(DataType.Date)]
        public DateTime? MariageAnniversaryDateBegin { get; set; }

        [DataType(DataType.Date)]
        public DateTime? MariageAnniversaryDateEnd { get; set; }

        public int SearchByEmployeePermanentCountryId { get; set; }

        public int SearchByEmployeePermanentDistrictId { get; set; }

        public int SearchByEmployeeEducationLevelId { get; set; }

        public List<Guid> EmployeeIdList { get; set; }

        [Required(ErrorMessage = "Required")]
        public string SelectedMonth { get; set; }


        [Required(ErrorMessage = "Required")]
        public string SelectedYear { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? UpToDate { get; set; }

        [Required(ErrorMessage = @"Required!")]
        public int TargetDepartmentLineId { get; set; }

        public int ExistingJobTypeId { get; set; }

        [Required(ErrorMessage = @"Required!")]
        public int TargetJobTypeId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ConsumedDateBegin { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ConsumedDateEnd { get; set; }

        [Required(ErrorMessage = @"Required!")]
        [DataType(DataType.Date)]
        public DateTime? EffectiveFromDate { get; set; }

        public string ProcessType { get; set; }

        public float? IncrementPercent { get; set; }

        public decimal? IncrementAmount { get; set; }

        public DateTime? DisagreeDate { get; set; }

    }
}
