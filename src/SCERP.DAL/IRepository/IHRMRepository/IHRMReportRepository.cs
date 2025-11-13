using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Model.HRMModel;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IHRMReportRepository : IRepository<Employee>
    {
        List<JobCardInfoModel> GetJobCardInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, string employeeCardId, int Year, int Month, DateTime? fromDate,
            DateTime? toDate, string userName);

        List<JobCardInfoModel> GetJobCardInfo10PM(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int Year, int Month, DateTime? fromDate, DateTime? toDate, string userName);

        List<JobCardInfoModel> GetJobCardInfo10PMNoWeekend(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int Year, int Month, DateTime? fromDate, DateTime? toDate, string userName);

        List<JobCardInfoModel> GetJobCardOriginalNoWeekend(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int Year, int Month, DateTime? fromDate, DateTime? toDate, string userName);

        List<ShortLeaveSummaryModel> GetShortLeaveSummary(
            int branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, string employeeCardId, DateTime startDate, DateTime endDate);

        List<ShortLeaveDetailModel> GetShortLeaveDetail(int? companyId, int? branchId,
            int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, Guid employeeId, DateTime fromDate, DateTime toDate, int? reasonType);


        List<AttendanceModel> GetEmployeeAttendanceInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId,
            int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
            int? branchUnitWorkShiftId, DateTime? fromDate, DateTime? toDate, string attendanceStatus,
            int? totalContinuousAbsentDays, bool otEnabled, bool extraOTEnabled, bool weekendOTEnabled);

        List<AllEmployeeJobCardView> GetAllEmployeeJobCardInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month,
            DateTime? fromDate, DateTime? toDate, string attendanceStatus, int employeeActiveStatusId, int employeeCategoryId, bool otEnabled, bool extraOTEnabled, bool weekendOTEnabled);

        List<AttendanceSummaryModel> GetAttendanceSummaryInfo(int companyId, int branchId, int branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? branchUnitWorkShiftId,
            DateTime? transactionDate);

        List<AttendanceSummaryByDesignationModel> GetAttendanceSummaryByDesignationInfo(int companyId, int branchId,
            int branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? branchUnitWorkShiftId,
            DateTime? transactionDate);

        List<EmployeeAllInfoModel> GetEmployeeAllInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId,
            int? bloodGroupId, int? genderId, int? religionId, int? maritalStateId, DateTime? joiningDateBegin,
            DateTime? joiningDateEnd, DateTime? confirmationDateBegin, DateTime? confirmationDateEnd,
            DateTime? quitDateBegin, DateTime? quitDateEnd, int? birthDayMonth, DateTime? mariageAnniversaryDateBegin,
            DateTime? mariageAnniversaryDateEnd, int? permanentCountryId, int? permanentDistrictId,
            int? educationLevelId, string employeeCardId,
            string employeeName, string mobileNo, int? activeStatus, string userName, DateTime? fromDate);

        List<EmployeeAllInfoNewModel> GetEmployeeAllInfoNew(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId,
            int? bloodGroupId, int? genderId, int? religionId, int? maritalStateId, DateTime? joiningDateBegin,
            DateTime? joiningDateEnd, DateTime? confirmationDateBegin, DateTime? confirmationDateEnd,
            DateTime? quitDateBegin, DateTime? quitDateEnd, int? birthDayMonth, DateTime? mariageAnniversaryDateBegin,
            DateTime? mariageAnniversaryDateEnd, int? permanentCountryId, int? permanentDistrictId,
            int? educationLevelId, string employeeCardId,
            string employeeName, string mobileNo, int? activeStatus, string userName, DateTime? fromDate);

        List<JobCardInfoModel> GetJobCardModelInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, string employeeCardId, int Year, int Month, DateTime? fromDate,
            DateTime? toDate, string userName);

        List<AttendanceModel> GetEmployeeAttendanceModelInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId,
            int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
            int? branchUnitWorkShiftId, DateTime? fromDate, DateTime? toDate, string attendanceStatus,
            int? totalContinuousAbsentDays, bool otEnabled);

        List<EmployeeLeaveHistoryModel> GetEmployeeLeaveHistoryInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId,
            int? genderId, DateTime? joiningDateBegin, DateTime? joiningDateEnd, DateTime? quitDateBegin, DateTime? quitDateEnd,
            string employeeCardId, string employeeName, int? leaveTypeId, int? activeStatus, int? year, string userName, DateTime? fromDate);

        List<ManpowerSummaryModel> GetManpowerSummaryInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? employeeDesignationId,
            int? genderId, DateTime? joiningDateBegin, DateTime? joiningDateEnd,
            DateTime? confirmationDateBegin, DateTime? confirmationDateEnd,
            DateTime? quitDateBegin, DateTime? quitDateEnd, string userName);

        List<EmployeeLeaveDetailModel> GetEmployeeLeaveDetailInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId,
            int? genderId, DateTime? joiningDateBegin, DateTime? joiningDateEnd,
            DateTime? quitDateBegin, DateTime? quitDateEnd, string employeeCardId, string employeeName,
            int? leaveTypeId, DateTime? consumedDateBegin, DateTime? consumedDateEnd, int? activeStatus,
            DateTime? fromDate, string userName);

        List<AttendanceSearchByTimeModel> GetAttendanceSearchByTime(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string userName, DateTime? fromDate, TimeSpan fromTime, TimeSpan toTime);

        List<SPCommCMInfo> GetCMInfo(DateTime? fromDate, DateTime? toDate);

        List<SPGetEmployeesForBonus> GetEmployeeBonusInfo();

        List<SpHrmCuttingSectionAbsent> GetCuttingAbsentInfo(DateTime? fromDate);

        DataTable GetMaternityInfo(string employeeCardId, DateTime? date);

        string GetCompanyNameByCompanyId(string companyId);

        List<HrmDailyOTReport> GetDailyOtReport(DateTime? fromDate);

        List<HrmOTSummaryReport> GetMonthlyOtSummaryReport(DateTime? fromDate, DateTime? toDate);

        List<ManpowerSummaryModel> GetManpowerSummarySkillInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? employeeDesignationId,
            int? genderId, DateTime? joiningDateBegin, DateTime? joiningDateEnd,
            DateTime? confirmationDateBegin, DateTime? confirmationDateEnd,
            DateTime? quitDateBegin, DateTime? quitDateEnd, string userName);

        string CategoryNameById(int categoryId);

        DataTable GetEmployeeEarnLeave(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate, int? activeStatus);

        DataTable GetEmployeeMonthwiseAttendence(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate);

        DataTable GetEmployeeDailyAbsent(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate);

        DataTable GetEmployeeDailyAttendance(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate);

        DataTable GetEmployeeDailyAttendanceButPreviousDayAbsent(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate);

        DataTable GetEmployeeDailyAttendanceByDesignation(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate);

        DataTable GetEmployeeWorkingHoursDetails(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate);

        JobCardEditView GetJobCardEditInfo(string jobCardName, string employeeCardId, DateTime? fromDate, DateTime? toDate);

        int EditJobCard(JobCardEditView model);

        EmployeeInOutEditView GetEmployeeInOutInfo(string inOutName, string employeeCardId, DateTime? date);

        int EditEmployeeInOut(EmployeeInOutEditView model);

        DataTable GetSkillMatrixPoint(string employeeCardId);

        DataTable GetSkillMatrixPointSecondPart(string employeeCardId);

        DataTable GetSkillMatrixAll(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId);

        DataTable GetManpowerApprovedEmployee(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, DateTime? effectiveDate, DateTime? fromDate);

        DataTable EmployeeNewJoinAndQuitSummary(int? companyId, int? branchId, int? fromYear, int? toYear, int? fromMonth, int? toMonth, DateTime? fromDate, DateTime? toDate);

        DataTable GetAdvanceOTAmount(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, DateTime? date);

        DataTable GetEmployeeDailyAbsentRootCause(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate);

        DataTable GetTiffinBill(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, string employeeCardId, string userName, DateTime? date, bool all, bool management, bool middleManagement, bool teamMemberA, bool teamMemberB);

        DataTable GetTiffinBillDyeing(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, string employeeCardId, string userName, DateTime? date, bool all, bool management, bool middleManagement, bool teamMemberA, bool teamMemberB);

        DataTable GetJobCardSummary(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate);

        List<JobCardInfoModel> GetJobCardInfoNoPenalty(int? companyId, int? branchId, int? branchUnitId,
           int? branchUnitDepartmentId, int? departmentSectionId,
           int? departmentLineId, int? employeeTypeId, string employeeCardId, int Year, int Month, DateTime? fromDate,
           DateTime? toDate, string userName);

        DataTable GetAgeAndFitnessCertificateInfo(Guid employeeId, string userName, DateTime prepareDate);

        DataTable GetJobApplicationInfo(Guid employeeId, string userName, DateTime prepareDate);

        DataTable GetJobVerificationInfo(Guid employeeId, Guid? userId, DateTime prepareDate);

        DataTable GetLeaveApplicationWorkerInfo(Guid employeeId, string userName, DateTime prepareDate);

        List<EmployeeLeaveData> GetEmployeeLeaveData(string employeeCardId, int year);

        DataTable GetEmployeeLeaveSummary(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? employeeTypeId, string employeeCardId, DateTime? fromDate, DateTime? toDate);

        DataTable LeaveApplicationStaff(Guid employeeId, string userName, DateTime prepareDate);

        DataTable GetNominationFormInfo(Guid employeeId, string userName, DateTime prepareDate);

        DataTable GetManPowerBudget(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate);

        DataTable GetDailyOTDetailWithAmount(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate);

        DataTable GetEmployeeEarnLeaveQuit(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate);

        DataTable GetFemaleConsentLetterInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, string employeeCardId, DateTime? disagreeDate, DateTime? effectiveDate);

        List<JobCardInfoModel> GetJobCardModelKnittingDyeingInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId,
         int? departmentLineId, int? employeeTypeId, string employeeCardId, int Year, int Month, DateTime? fromDate,
         DateTime? toDate, string userName);

        DataTable GetEmployeeLeaveRegister(string employeeCardId, DateTime prepareDate);

        DataTable GetDyeingShiftCount(DateTime fromDate, DateTime toDate);

        DataTable GetLeaveButPresent(DateTime fromDate, DateTime toDate);

        DataTable GetAbsentOnJoiningDate(DateTime fromDate, DateTime toDate);
    }
}