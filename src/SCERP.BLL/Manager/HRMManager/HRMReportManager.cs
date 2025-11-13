using System;
using System.Collections.Generic;
using System.Data;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model.Custom;
using SCERP.Model.HRMModel;
using SCERP.BLL.Process;
using System.Text;
using SCERP.Common;
using System.Web.Hosting;
using SCERP.Model;

namespace SCERP.BLL.Manager.HRMManager
{
    public class HRMReportManager : BaseManager, IHRMReportManager
    {
        protected readonly IHRMReportRepository HrmReportRepository = null;

        public HRMReportManager(SCERPDBContext context)
        {
            HrmReportRepository = new HRMReportRepository(context);
        }

        public List<JobCardInfoModel> GetJobCardInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month, DateTime? fromDate,
            DateTime? toDate, string userName)
        {
            return HrmReportRepository.GetJobCardInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, userName);
        }

        public List<JobCardInfoModel> GetJobCardInfo10PM(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int Year, int Month, DateTime? fromDate, DateTime? toDate, string userName)
        {
            return HrmReportRepository.GetJobCardInfo10PM(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, Year, Month, fromDate, toDate, userName);
        }

        public List<JobCardInfoModel> GetJobCardInfo10PMNoWeekend(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int Year, int Month, DateTime? fromDate, DateTime? toDate, string userName)
        {
            return HrmReportRepository.GetJobCardInfo10PMNoWeekend(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, Year, Month, fromDate, toDate, userName);
        }

        public List<JobCardInfoModel> GetJobCardOriginalNoWeekend(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int Year, int Month, DateTime? fromDate, DateTime? toDate, string userName)
        {
            return HrmReportRepository.GetJobCardOriginalNoWeekend(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, Year, Month, fromDate, toDate, userName);
        }

        public List<ShortLeaveSummaryModel> GetShortLeaveSummary(int branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, string employeeCardId, DateTime startDate, DateTime endDate)
        {
            return HrmReportRepository.GetShortLeaveSummary(branchUnitDepartmentId, departmentSectionId,
                departmentLineId, employeeCardId, startDate, endDate);
        }



        public List<ShortLeaveDetailModel> GetShortLeaveDetail(int? companyId, int? branchId,
            int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, Guid employeeId, DateTime fromDate, DateTime toDate, int? reasonType)
        {
            return HrmReportRepository.GetShortLeaveDetail(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                departmentLineId, employeeId, fromDate, toDate, reasonType);
        }


        public List<AttendanceModel> GetEmployeeAttendanceInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId,
            int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
            int? branchUnitWorkShiftId, DateTime? fromDate, DateTime? toDate, string attendanceStatus,
            int? totalContinuousAbsentDays, bool otEnabled, bool extraOTEnabled, bool weekendOTEnabled)
        {
            return HrmReportRepository.GetEmployeeAttendanceInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId,
                departmentSectionId, departmentLineId, employeeTypeId, employeeCardId,
                branchUnitWorkShiftId, fromDate, toDate, attendanceStatus,
                totalContinuousAbsentDays, otEnabled, extraOTEnabled, weekendOTEnabled);
        }

        public List<AllEmployeeJobCardView> GetAllEmployeeJobCardInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month, DateTime? fromDate, DateTime? toDate, string attendanceStatus, int employeeActiveStatusId, int employeeCategoryId, bool otEnabled, bool extraOTEnabled, bool weekendOTEnabled)
        {
            return HrmReportRepository.GetAllEmployeeJobCardInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId,
                employeeTypeId, employeeCardId, year, month, fromDate, toDate, attendanceStatus, employeeActiveStatusId, employeeCategoryId, otEnabled, extraOTEnabled, weekendOTEnabled);
        }


        public List<AttendanceSummaryModel> GetAttendanceSummaryInfo(int companyId, int branchId, int branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? branchUnitWorkShiftId, DateTime? transactionDate)
        {
            return HrmReportRepository.GetAttendanceSummaryInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, branchUnitWorkShiftId, transactionDate);
        }

        public List<AttendanceSummaryByDesignationModel> GetAttendanceSummaryByDesignationInfo(int companyId, int branchId, int branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? branchUnitWorkShiftId, DateTime? transactionDate)
        {
            return HrmReportRepository.GetAttendanceSummaryByDesignationInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, branchUnitWorkShiftId, transactionDate);
        }

        public List<EmployeeAllInfoModel> GetEmployeeAllInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId,
            int? bloodGroupId, int? genderId, int? religionId, int? maritalStateId, DateTime? joiningDateBegin,
            DateTime? joiningDateEnd, DateTime? confirmationDateBegin, DateTime? confirmationDateEnd,
            DateTime? quitDateBegin, DateTime? quitDateEnd, int? birthDayMonth, DateTime? mariageAnniversaryDateBegin,
            DateTime? mariageAnniversaryDateEnd, int? permanentCountryId, int? permanentDistrictId, int? educationLevelId, string employeeCardId,
            string employeeName, string mobileNo, int? activeStatus, string userName, DateTime? fromDate)
        {
            return HrmReportRepository.GetEmployeeAllInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId,
                employeeGradeId, employeeDesignationId, bloodGroupId, genderId, religionId, maritalStateId, joiningDateBegin, joiningDateEnd, confirmationDateBegin,
                confirmationDateEnd, quitDateBegin, quitDateEnd, birthDayMonth, mariageAnniversaryDateBegin, mariageAnniversaryDateEnd, permanentCountryId,
                permanentDistrictId, educationLevelId, employeeCardId, employeeName, mobileNo, activeStatus, userName, fromDate);
        }

        public List<EmployeeAllInfoNewModel> GetEmployeeAllInfoNew(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId,
            int? bloodGroupId, int? genderId, int? religionId, int? maritalStateId, DateTime? joiningDateBegin,
            DateTime? joiningDateEnd, DateTime? confirmationDateBegin, DateTime? confirmationDateEnd,
            DateTime? quitDateBegin, DateTime? quitDateEnd, int? birthDayMonth, DateTime? mariageAnniversaryDateBegin,
            DateTime? mariageAnniversaryDateEnd, int? permanentCountryId, int? permanentDistrictId, int? educationLevelId, string employeeCardId,
            string employeeName, string mobileNo, int? activeStatus, string userName, DateTime? fromDate)
        {
            return HrmReportRepository.GetEmployeeAllInfoNew(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId,
                employeeGradeId, employeeDesignationId, bloodGroupId, genderId, religionId, maritalStateId, joiningDateBegin, joiningDateEnd, confirmationDateBegin,
                confirmationDateEnd, quitDateBegin, quitDateEnd, birthDayMonth, mariageAnniversaryDateBegin, mariageAnniversaryDateEnd, permanentCountryId,
                permanentDistrictId, educationLevelId, employeeCardId, employeeName, mobileNo, activeStatus, userName, fromDate);
        }

        public List<JobCardInfoModel> GetJobCardModelInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month, DateTime? fromDate,
            DateTime? toDate, string userName)
        {
            return HrmReportRepository.GetJobCardModelInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, userName);
        }

        public List<AttendanceModel> GetEmployeeAttendanceModelInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId,
            int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
            int? branchUnitWorkShiftId, DateTime? fromDate, DateTime? toDate, string attendanceStatus,
            int? totalContinuousAbsentDays, bool otEnabled)
        {
            return HrmReportRepository.GetEmployeeAttendanceModelInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId,
                departmentSectionId, departmentLineId, employeeTypeId, employeeCardId,
                branchUnitWorkShiftId, fromDate, toDate, attendanceStatus,
                totalContinuousAbsentDays, otEnabled);
        }

        public List<EmployeeLeaveHistoryModel> GetEmployeeLeaveHistoryInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId,
            int? genderId, DateTime? joiningDateBegin,
            DateTime? joiningDateEnd, DateTime? quitDateBegin, DateTime? quitDateEnd, string employeeCardId, string employeeName,
            int? leaveTypeId, int? activeStatus, int? year, string userName, DateTime? fromDate)
        {
            return HrmReportRepository.GetEmployeeLeaveHistoryInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId,
                genderId, joiningDateBegin, joiningDateEnd,
                quitDateBegin, quitDateEnd, employeeCardId, employeeName, leaveTypeId, activeStatus, year,
                userName, fromDate);
        }

        public List<ManpowerSummaryModel> GetManpowerSummaryInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? employeeDesignationId,
            int? genderId, DateTime? joiningDateBegin, DateTime? joiningDateEnd,
            DateTime? confirmationDateBegin, DateTime? confirmationDateEnd,
            DateTime? quitDateBegin, DateTime? quitDateEnd, string userName)
        {
            return HrmReportRepository.GetManpowerSummaryInfo(companyId, branchId, branchUnitId,
                branchUnitDepartmentId, departmentSectionId,
                departmentLineId, employeeTypeId, employeeDesignationId,
                genderId, joiningDateBegin, joiningDateEnd,
                confirmationDateBegin, confirmationDateEnd,
                quitDateBegin, quitDateEnd, userName);
        }

        public List<EmployeeLeaveDetailModel> GetEmployeeLeaveDetailInfo(int? companyId, int? branchId,
            int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId,
            int? genderId, DateTime? joiningDateBegin, DateTime? joiningDateEnd,
            DateTime? quitDateBegin, DateTime? quitDateEnd, string employeeCardId, string employeeName,
            int? leaveTypeId, DateTime? consumedDateBegin, DateTime? consumedDateEnd, int? activeStatus,
            DateTime? fromDate, string userName)
        {
            return HrmReportRepository.GetEmployeeLeaveDetailInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId,
                genderId, joiningDateBegin, joiningDateEnd, quitDateBegin, quitDateEnd, employeeCardId,
                employeeName, leaveTypeId, consumedDateBegin, consumedDateEnd, activeStatus, fromDate,
                userName);
        }

        public List<AttendanceSearchByTimeModel> GetAttendanceSearchByTime(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string userName, DateTime? fromDate, TimeSpan fromTime, TimeSpan toTime)
        {
            return HrmReportRepository.GetAttendanceSearchByTime(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, userName, fromDate, fromTime, toTime);
        }
        public List<SPCommCMInfo> GetCMInfo(DateTime? fromDate, DateTime? toDate)
        {
            return HrmReportRepository.GetCMInfo(fromDate, toDate);
        }

        public List<SPGetEmployeesForBonus> GetEmployeeBonusInfo()
        {
            return HrmReportRepository.GetEmployeeBonusInfo();
        }

        public List<SpHrmCuttingSectionAbsent> GetCuttingAbsentInfo(DateTime? fromDate)
        {
            return HrmReportRepository.GetCuttingAbsentInfo(fromDate);
        }

        public List<HrmDailyOTReport> GetDailyOtReport(DateTime? fromDate)
        {
            return HrmReportRepository.GetDailyOtReport(fromDate);
        }

        public List<HrmOTSummaryReport> GetMonthlyOtSummaryReport(DateTime? fromDate, DateTime? toDate)
        {
            return HrmReportRepository.GetMonthlyOtSummaryReport(fromDate, toDate);
        }

        public DataTable GetMaternityInfo(string employeeCardId, DateTime? date)
        {
            DataTable dt = HrmReportRepository.GetMaternityInfo(employeeCardId, date);
            return HrmReportRepository.GetMaternityInfo(employeeCardId, date);
        }

        public string GetCompanyNameByCompanyId(string companyId)
        {
            return HrmReportRepository.GetCompanyNameByCompanyId(companyId);
        }

        public List<ManpowerSummaryModel> GetManpowerSummarySkillInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? employeeDesignationId,
            int? genderId, DateTime? joiningDateBegin, DateTime? joiningDateEnd,
            DateTime? confirmationDateBegin, DateTime? confirmationDateEnd,
            DateTime? quitDateBegin, DateTime? quitDateEnd, string userName)
        {
            return HrmReportRepository.GetManpowerSummarySkillInfo(companyId, branchId, branchUnitId,
                branchUnitDepartmentId, departmentSectionId,
                departmentLineId, employeeTypeId, employeeDesignationId,
                genderId, joiningDateBegin, joiningDateEnd,
                confirmationDateBegin, confirmationDateEnd,
                quitDateBegin, quitDateEnd, userName);
        }

        public string CategoryNameById(int categoryId)
        {
            return HrmReportRepository.CategoryNameById(categoryId);
        }

        public DataTable GetEmployeeEarnLeave(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate, int? activeStatus)
        {
            return HrmReportRepository.GetEmployeeEarnLeave(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId, userName, fromDate, toDate, activeStatus);
        }

        public DataTable GetEmployeeMonthwiseAttendence(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {
            return HrmReportRepository.GetEmployeeMonthwiseAttendence(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId, userName, fromDate, toDate);
        }

        public DataTable GetEmployeeDailyAbsent(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {
            return HrmReportRepository.GetEmployeeDailyAbsent(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId, userName, fromDate, toDate);
        }
        public DataTable GetEmployeeDailyAttendance(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {
            return HrmReportRepository.GetEmployeeDailyAttendance(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId, userName, fromDate, toDate);
        }

        public DataTable GetEmployeeDailyAttendanceButPreviousDayAbsent(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {
            return HrmReportRepository.GetEmployeeDailyAttendanceButPreviousDayAbsent(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId, userName, fromDate, toDate);
        }
        public DataTable GetEmployeeDailyAttendanceByDesignation(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {
            return HrmReportRepository.GetEmployeeDailyAttendanceByDesignation(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId, userName, fromDate, toDate);
        }

        public DataTable GetEmployeeWorkingHoursDetails(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {
            return HrmReportRepository.GetEmployeeWorkingHoursDetails(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, userName, fromDate, toDate);
        }

        public JobCardEditView GetJobCardEditInfo(string jobCardName, string employeeCardId, DateTime? fromDate, DateTime? toDate)
        {
            return HrmReportRepository.GetJobCardEditInfo(jobCardName, employeeCardId, fromDate, toDate);
        }

        public int EditJobCard(JobCardEditView model)
        {
            return HrmReportRepository.EditJobCard(model);
        }

        public EmployeeInOutEditView GetEmployeeInOutInfo(string inOutName, string employeeCardId, DateTime? date)
        {
            return HrmReportRepository.GetEmployeeInOutInfo(inOutName, employeeCardId, date);
        }

        public int EditEmployeeInOut(EmployeeInOutEditView model)
        {
            return HrmReportRepository.EditEmployeeInOut(model);
        }

        public DataTable GetSkillMatrixPoint(string employeeCardId)
        {
            return HrmReportRepository.GetSkillMatrixPoint(employeeCardId);
        }

        public DataTable GetSkillMatrixPointSecondPart(string employeeCardId)
        {
            return HrmReportRepository.GetSkillMatrixPointSecondPart(employeeCardId);
        }
        public DataTable GetSkillMatrixAll(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId)
        {
            return HrmReportRepository.GetSkillMatrixAll(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId);
        }
        public DataTable GetManpowerApprovedEmployee(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, DateTime? effectiveDate, DateTime? fromDate)
        {
            return HrmReportRepository.GetManpowerApprovedEmployee(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, effectiveDate, fromDate);
        }
        public DataTable EmployeeNewJoinAndQuitSummary(int? companyId, int? branchId, int? fromYear, int? toYear, int? fromMonth, int? toMonth, DateTime? fromDate, DateTime? toDate)
        {
            return HrmReportRepository.EmployeeNewJoinAndQuitSummary(companyId, branchId, fromYear, toYear, fromMonth, toMonth, fromDate, toDate);
        }
        public DataTable GetAdvanceOTAmount(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, DateTime? date)
        {
            return HrmReportRepository.GetAdvanceOTAmount(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, date);
        }
        public DataTable GetEmployeeDailyAbsentRootCause(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {
            return HrmReportRepository.GetEmployeeDailyAbsentRootCause(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId, userName, fromDate, toDate);
        }
        public DataTable GetTiffinBill(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, string employeeCardId, string userName, DateTime? date, bool all, bool management, bool middleManagement, bool teamMemberA, bool teamMemberB)
        {
            return HrmReportRepository.GetTiffinBill(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeCardId, userName, date, all, management, middleManagement, teamMemberA, teamMemberB);
        }
        public DataTable GetTiffinBillDyeing(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, string employeeCardId, string userName, DateTime? date, bool all, bool management, bool middleManagement, bool teamMemberA, bool teamMemberB)
        {
            return HrmReportRepository.GetTiffinBillDyeing(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeCardId, userName, date, all, management, middleManagement, teamMemberA, teamMemberB);
        }
        public DataTable GetJobCardSummary(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {
            return HrmReportRepository.GetJobCardSummary(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId, userName, fromDate, toDate);
        }
        public List<JobCardInfoModel> GetJobCardInfoNoPenalty(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId,
          int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month, DateTime? fromDate,
          DateTime? toDate, string userName)
        {
            return HrmReportRepository.GetJobCardInfoNoPenalty(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, userName);
        }
        public string GetAgeAndFitnessCertificateInfo(Guid employeeId, string userName, DateTime prepareDate)
        {
            DataRow emp;
            DataTable table = HrmReportRepository.GetAgeAndFitnessCertificateInfo(employeeId, userName, prepareDate);

            if (table == null) return null;

            if (table.Rows.Count > 0)
                emp = table.Rows[0];
            else
                return null;

            string DateToday = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime joinDate = emp.Field<DateTime>("JoinDateCalculation");
            DateTime confirmationDate = emp.Field<DateTime>("ConfirmationDate");
            string ProbationEndDate = confirmationDate.AddDays(-1).ToString("dd/MM/yyyy");

            var appointmentInfo = AppointmentLetter.Create(HostingEnvironment.MapPath("~/Content/AgeAndFitnessCertificate.xml"));

            var appointmentLetterBuilder = new StringBuilder();
            appointmentLetterBuilder.AppendLine(appointmentInfo.EmployeeSpecificInfo);

            if (emp != null)
            {
                appointmentLetterBuilder.Replace("{DATE_TODAY}", BanglaConversion.ConvertToBanglaNumber(DateToday));
                appointmentLetterBuilder.Replace("{PREPARE_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("PrepareDate")));
                appointmentLetterBuilder.Replace("{EMP_NAME}", emp.Field<string>("NameInBengali"));
                appointmentLetterBuilder.Replace("{JOINING_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{EMP_DEPARTMENT}", emp.Field<string>("Department"));
                appointmentLetterBuilder.Replace("{EMP_FATHER_NAME}", emp.Field<string>("FathersNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_MOTHER_NAME}", emp.Field<string>("MothersNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_DESIGNATION}", emp.Field<string>("Designation"));
                appointmentLetterBuilder.Replace("{EMP_CARDNO}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("EmployeeCardId")));
                appointmentLetterBuilder.Replace("{EMP_GRADE}", emp.Field<string>("Grade"));
                appointmentLetterBuilder.Replace("{EMP_SPOUSE_NAME}", emp.Field<string>("SpousesNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_PRE_ADDRESS}", emp.Field<string>("PreMailingAddress"));
                appointmentLetterBuilder.Replace("{EMP_PRE_POST_OFFICE}", emp.Field<string>("PrePostOffice"));
                appointmentLetterBuilder.Replace("{EMP_PRE_POLICE_STATION}", emp.Field<string>("PrePolice"));
                appointmentLetterBuilder.Replace("{EMP_PRE_DISTRICT}", emp.Field<string>("PreDist"));
                appointmentLetterBuilder.Replace("{EMP_PER_ADDRESS}", emp.Field<string>("PerMailingAddress"));
                appointmentLetterBuilder.Replace("{EMP_PER_POST_OFFICE}", emp.Field<string>("PerPostOffice"));
                appointmentLetterBuilder.Replace("{EMP_PER_POLICE_STATION}", emp.Field<string>("PerPolice"));
                appointmentLetterBuilder.Replace("{EMP_PER_DISTRICT}", emp.Field<string>("PerDist"));
                appointmentLetterBuilder.Replace("{PROBATION_END_DATE}", BanglaConversion.ConvertToBanglaNumber(ProbationEndDate));
                appointmentLetterBuilder.Replace("{GROSS_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("GrossSalary").ToString()));
                appointmentLetterBuilder.Replace("{BASIC_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("BasicSalary").ToString()));
                appointmentLetterBuilder.Replace("{HOUSE_RENT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("HouseRent").ToString()));
                appointmentLetterBuilder.Replace("{MEDICAL_ALLOWANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("MedicalAllowance").ToString()));
                appointmentLetterBuilder.Replace("{FOOD_ALLOWANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("FoodAllowance").ToString()));
                appointmentLetterBuilder.Replace("{TRANSPORT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("Conveyance").ToString()));
                appointmentLetterBuilder.Replace("{WeekEnd}", BanglaConversion.ConvertToBanglaNumber(emp.Field<int>("Weekend").ToString()));
                appointmentLetterBuilder.Replace("{OVERTIME_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("OverTimeRate").ToString()));
                appointmentLetterBuilder.Replace("{EMPLOYEE_OT_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("EmployeeOTRate").ToString()));
                appointmentLetterBuilder.Replace("{AMOUNT_IN_WORDS}", emp.Field<string>("AmountInWords"));
                appointmentLetterBuilder.Replace("{EMPLOYEE_SKILL_TYPE}", emp.Field<string>("SkillType"));
                appointmentLetterBuilder.Replace("{EMPLOYEE_PHOTOGRAPH_PATH}", emp.Field<string>("PhotographPath"));
                appointmentLetterBuilder.Replace("{APPLICATION_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{EMP_SECTION}", emp.Field<string>("Section"));
                appointmentLetterBuilder.Replace("{MARITAL_STATE}", emp.Field<string>("MaritalState"));
                appointmentLetterBuilder.Replace("{BIRTH_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("DateOfBirth")));
                appointmentLetterBuilder.Replace("{AGE}", emp.Field<string>("Age"));
                appointmentLetterBuilder.Replace("{AGE_BANGLA}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("Age")));
                appointmentLetterBuilder.Replace("{GENDER}", emp.Field<string>("Gender"));

                if (emp.Field<string>("EmployeeType") == "Team Member-A")
                {
                    appointmentLetterBuilder.Replace("{MONTH}", "০৩ (তিন)");
                }
                else
                {
                    appointmentLetterBuilder.Replace("{MONTH}", "০৬ (ছয়)");
                }
            }
            return appointmentLetterBuilder.Replace("\n", "").Replace("\r", "").ToString().Trim();
        }

        public string GetJobApplicationInfo(Guid employeeId, string userName, DateTime prepareDate)
        {
            DataRow emp;
            DataTable table = HrmReportRepository.GetJobApplicationInfo(employeeId, userName, prepareDate);

            if (table == null) return null;

            if (table.Rows.Count > 0)
                emp = table.Rows[0];
            else
                return null;

            string DateToday = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime joinDate = emp.Field<DateTime>("JoinDateCalculation");
            DateTime confirmationDate = emp.Field<DateTime>("ConfirmationDate");
            string ProbationEndDate = confirmationDate.AddDays(-1).ToString("dd/MM/yyyy");

            var appointmentInfo = AppointmentLetter.Create(HostingEnvironment.MapPath("~/Content/JobApplication.xml"));

            var appointmentLetterBuilder = new StringBuilder();
            appointmentLetterBuilder.AppendLine(appointmentInfo.EmployeeSpecificInfo);

            if (emp != null)
            {
                appointmentLetterBuilder.Replace("{DATE_TODAY}", BanglaConversion.ConvertToBanglaNumber(DateToday));
                appointmentLetterBuilder.Replace("{PREPARE_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("PrepareDate")));
                appointmentLetterBuilder.Replace("{EMP_NAME}", emp.Field<string>("NameInBengali"));
                appointmentLetterBuilder.Replace("{JOINING_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{EMP_DEPARTMENT}", emp.Field<string>("Department"));
                appointmentLetterBuilder.Replace("{EMP_FATHER_NAME}", emp.Field<string>("FathersNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_MOTHER_NAME}", emp.Field<string>("MothersNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_DESIGNATION}", emp.Field<string>("Designation"));
                appointmentLetterBuilder.Replace("{EMP_CARDNO}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("EmployeeCardId")));
                appointmentLetterBuilder.Replace("{EMP_GRADE}", emp.Field<string>("Grade"));
                appointmentLetterBuilder.Replace("{EMP_SPOUSE_NAME}", emp.Field<string>("SpousesNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_PRE_ADDRESS}", emp.Field<string>("PreMailingAddress"));
                appointmentLetterBuilder.Replace("{EMP_PRE_POST_OFFICE}", emp.Field<string>("PrePostOffice"));
                appointmentLetterBuilder.Replace("{EMP_PRE_POLICE_STATION}", emp.Field<string>("PrePolice"));
                appointmentLetterBuilder.Replace("{EMP_PRE_DISTRICT}", emp.Field<string>("PreDist"));
                appointmentLetterBuilder.Replace("{EMP_PER_ADDRESS}", emp.Field<string>("PerMailingAddress"));
                appointmentLetterBuilder.Replace("{EMP_PER_POST_OFFICE}", emp.Field<string>("PerPostOffice"));
                appointmentLetterBuilder.Replace("{EMP_PER_POLICE_STATION}", emp.Field<string>("PerPolice"));
                appointmentLetterBuilder.Replace("{EMP_PER_DISTRICT}", emp.Field<string>("PerDist"));
                appointmentLetterBuilder.Replace("{PROBATION_END_DATE}", BanglaConversion.ConvertToBanglaNumber(ProbationEndDate));
                appointmentLetterBuilder.Replace("{GROSS_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("GrossSalary").ToString()));
                appointmentLetterBuilder.Replace("{BASIC_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("BasicSalary").ToString()));
                appointmentLetterBuilder.Replace("{HOUSE_RENT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("HouseRent").ToString()));
                appointmentLetterBuilder.Replace("{MEDICAL_ALLOWANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("MedicalAllowance").ToString()));
                appointmentLetterBuilder.Replace("{FOOD_ALLOWANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("FoodAllowance").ToString()));
                appointmentLetterBuilder.Replace("{TRANSPORT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("Conveyance").ToString()));
                appointmentLetterBuilder.Replace("{WeekEnd}", BanglaConversion.ConvertToBanglaNumber(emp.Field<int>("Weekend").ToString()));
                appointmentLetterBuilder.Replace("{OVERTIME_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("OverTimeRate").ToString()));
                appointmentLetterBuilder.Replace("{EMPLOYEE_OT_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("EmployeeOTRate").ToString()));
                appointmentLetterBuilder.Replace("{AMOUNT_IN_WORDS}", emp.Field<string>("AmountInWords"));
                appointmentLetterBuilder.Replace("{EMPLOYEE_SKILL_TYPE}", emp.Field<string>("SkillType"));
                appointmentLetterBuilder.Replace("{EMPLOYEE_PHOTOGRAPH_PATH}", emp.Field<string>("PhotographPath"));
                appointmentLetterBuilder.Replace("{APPLICATION_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{EMP_SECTION}", emp.Field<string>("Section"));
                appointmentLetterBuilder.Replace("{MARITAL_STATE}", emp.Field<string>("MaritalState"));
                appointmentLetterBuilder.Replace("{BIRTH_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("DateOfBirth")));
                appointmentLetterBuilder.Replace("{NATIONALITY}", emp.Field<string>("NationalityInBengali"));
                appointmentLetterBuilder.Replace("{RELIGION}", emp.Field<string>("ReligionName"));
                appointmentLetterBuilder.Replace("{MARRIAGE_ANNIVERSARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("MarriageAnniversaryDate")));
                appointmentLetterBuilder.Replace("{NATIONALID}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("NationalIdNo")));
                appointmentLetterBuilder.Replace("{EDUCATION_LEVEL}", emp.Field<string>("EducationLevel"));
                appointmentLetterBuilder.Replace("{MOBILEPHONE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("MobilePhone")));
                appointmentLetterBuilder.Replace("{HOMEPHONE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("HomePhone")));
                appointmentLetterBuilder.Replace("{EMERGENCY_PERSON}", emp.Field<string>("EmergencyContactPersonInBengali"));
                appointmentLetterBuilder.Replace("{EMERGENCY_PERSON_RELATION}", emp.Field<string>("ContactPersonRelationInBengali"));
                appointmentLetterBuilder.Replace("{EMERGENCY_PERSON_PHONE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("ContactPersonPhone")));


                if (emp.Field<string>("EmployeeType") == "Team Member-A")
                {
                    appointmentLetterBuilder.Replace("{MONTH}", "০৩ (তিন)");
                }
                else
                {
                    appointmentLetterBuilder.Replace("{MONTH}", "০৬ (ছয়)");
                }
            }
            return appointmentLetterBuilder.Replace("\n", "").Replace("\r", "").ToString().Trim();
        }

        public string GetJobVerificationInfo(Guid employeeId, Guid? userId, DateTime prepareDate)
        {
            DataRow emp;
            DataTable table = HrmReportRepository.GetJobVerificationInfo(employeeId, userId, prepareDate);

            if (table == null) return null;

            if (table.Rows.Count > 0)
                emp = table.Rows[0];
            else
                return null;

            string DateToday = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime joinDate = emp.Field<DateTime>("JoinDateCalculation");
            DateTime confirmationDate = emp.Field<DateTime>("ConfirmationDate");
            string ProbationEndDate = confirmationDate.AddDays(-1).ToString("dd/MM/yyyy");

            var appointmentInfo = AppointmentLetter.Create(HostingEnvironment.MapPath("~/Content/JobVerification.xml"));

            var appointmentLetterBuilder = new StringBuilder();
            appointmentLetterBuilder.AppendLine(appointmentInfo.EmployeeSpecificInfo);

            if (emp != null)
            {
                appointmentLetterBuilder.Replace("{DATE_TODAY}", BanglaConversion.ConvertToBanglaNumber(DateToday));
                appointmentLetterBuilder.Replace("{PREPARE_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("PrepareDate")));
                appointmentLetterBuilder.Replace("{EMP_NAME}", emp.Field<string>("NameInBengali"));
                appointmentLetterBuilder.Replace("{JOINING_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{EMP_DEPARTMENT}", emp.Field<string>("Department"));
                appointmentLetterBuilder.Replace("{EMP_FATHER_NAME}", emp.Field<string>("FathersNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_MOTHER_NAME}", emp.Field<string>("MothersNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_DESIGNATION}", emp.Field<string>("Designation"));
                appointmentLetterBuilder.Replace("{EMP_CARDNO}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("EmployeeCardId")));
                appointmentLetterBuilder.Replace("{EMP_GRADE}", emp.Field<string>("Grade"));
                appointmentLetterBuilder.Replace("{EMP_SPOUSE_NAME}", emp.Field<string>("SpousesNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_PRE_ADDRESS}", emp.Field<string>("PreMailingAddress"));
                appointmentLetterBuilder.Replace("{EMP_PRE_POST_OFFICE}", emp.Field<string>("PrePostOffice"));
                appointmentLetterBuilder.Replace("{EMP_PRE_POLICE_STATION}", emp.Field<string>("PrePolice"));
                appointmentLetterBuilder.Replace("{EMP_PRE_DISTRICT}", emp.Field<string>("PreDist"));
                appointmentLetterBuilder.Replace("{EMP_PER_ADDRESS}", emp.Field<string>("PerMailingAddress"));
                appointmentLetterBuilder.Replace("{EMP_PER_POST_OFFICE}", emp.Field<string>("PerPostOffice"));
                appointmentLetterBuilder.Replace("{EMP_PER_POLICE_STATION}", emp.Field<string>("PerPolice"));
                appointmentLetterBuilder.Replace("{EMP_PER_DISTRICT}", emp.Field<string>("PerDist"));
                appointmentLetterBuilder.Replace("{PROBATION_END_DATE}", BanglaConversion.ConvertToBanglaNumber(ProbationEndDate));
                appointmentLetterBuilder.Replace("{GROSS_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("GrossSalary").ToString()));
                appointmentLetterBuilder.Replace("{BASIC_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("BasicSalary").ToString()));
                appointmentLetterBuilder.Replace("{HOUSE_RENT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("HouseRent").ToString()));
                appointmentLetterBuilder.Replace("{MEDICAL_ALLOWANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("MedicalAllowance").ToString()));
                appointmentLetterBuilder.Replace("{FOOD_ALLOWANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("FoodAllowance").ToString()));
                appointmentLetterBuilder.Replace("{TRANSPORT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("Conveyance").ToString()));
                appointmentLetterBuilder.Replace("{WeekEnd}", BanglaConversion.ConvertToBanglaNumber(emp.Field<int>("Weekend").ToString()));
                appointmentLetterBuilder.Replace("{OVERTIME_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("OverTimeRate").ToString()));
                appointmentLetterBuilder.Replace("{EMPLOYEE_OT_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("EmployeeOTRate").ToString()));
                appointmentLetterBuilder.Replace("{AMOUNT_IN_WORDS}", emp.Field<string>("AmountInWords"));
                appointmentLetterBuilder.Replace("{EMPLOYEE_SKILL_TYPE}", emp.Field<string>("SkillType"));
                appointmentLetterBuilder.Replace("{EMPLOYEE_PHOTOGRAPH_PATH}", emp.Field<string>("PhotographPath"));
                appointmentLetterBuilder.Replace("{APPLICATION_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{EMP_SECTION}", emp.Field<string>("Section"));
                appointmentLetterBuilder.Replace("{MARITAL_STATE}", emp.Field<string>("MaritalState"));
                appointmentLetterBuilder.Replace("{BIRTH_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("DateOfBirth")));
                appointmentLetterBuilder.Replace("{NATIONALITY}", emp.Field<string>("NationalityInBengali"));
                appointmentLetterBuilder.Replace("{RELIGION}", emp.Field<string>("ReligionName"));
                appointmentLetterBuilder.Replace("{MARRIAGE_ANNIVERSARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("MarriageAnniversaryDate")));
                appointmentLetterBuilder.Replace("{NATIONALID}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("NationalIdNo")));
                appointmentLetterBuilder.Replace("{EDUCATION_LEVEL}", emp.Field<string>("EducationLevel"));
                appointmentLetterBuilder.Replace("{MOBILEPHONE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("MobilePhone")));
                appointmentLetterBuilder.Replace("{HOMEPHONE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("HomePhone")));
                appointmentLetterBuilder.Replace("{EMERGENCY_PERSON}", emp.Field<string>("EmergencyContactPersonInBengali"));
                appointmentLetterBuilder.Replace("{EMERGENCY_PERSON_RELATION}", emp.Field<string>("ContactPersonRelationInBengali"));
                appointmentLetterBuilder.Replace("{EMERGENCY_PERSON_PHONE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("ContactPersonPhone")));

                appointmentLetterBuilder.Replace("{VERIFIER_NAME}", emp.Field<string>("VerifierNameInBangla"));
                appointmentLetterBuilder.Replace("{VERIFIER_CARD}", emp.Field<string>("VerifierCardId"));
                appointmentLetterBuilder.Replace("{VERIFIER_DESIGNATION}", emp.Field<string>("VerifierDesignation"));


                if (emp.Field<string>("EmployeeType") == "Team Member-A")
                {
                    appointmentLetterBuilder.Replace("{MONTH}", "০৩ (তিন)");
                }
                else
                {
                    appointmentLetterBuilder.Replace("{MONTH}", "০৬ (ছয়)");
                }
            }
            return appointmentLetterBuilder.Replace("\n", "").Replace("\r", "").ToString().Trim();
        }

        public string GetLeaveApplicationWorkerInfo(Guid employeeId, string employeeCardId, string userName, DateTime prepareDate)
        {
            DataRow emp;
            DataTable table = HrmReportRepository.GetLeaveApplicationWorkerInfo(employeeId, userName, prepareDate);

            List<EmployeeLeaveData> leaveDate = HrmReportRepository.GetEmployeeLeaveData(employeeCardId, prepareDate.Year);

            if (table == null)
                return null;

            if (table.Rows.Count > 0)
                emp = table.Rows[0];
            else
                return null;

            string DateToday = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime joinDate = emp.Field<DateTime>("JoinDateCalculation");
            DateTime confirmationDate = emp.Field<DateTime>("ConfirmationDate");
            string ProbationEndDate = confirmationDate.AddDays(-1).ToString("dd/MM/yyyy");

            var appointmentInfo = AppointmentLetter.Create(HostingEnvironment.MapPath("~/Content/LeaveApplicationWorker.xml"));

            var appointmentLetterBuilder = new StringBuilder();

            appointmentLetterBuilder.AppendLine(appointmentInfo.EmployeeSpecificInfo);

            if (emp != null)
            {
                appointmentLetterBuilder.Replace("{DATE_TODAY}", BanglaConversion.ConvertToBanglaNumber(DateToday));
                appointmentLetterBuilder.Replace("{PREPARE_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("PrepareDate")));
                appointmentLetterBuilder.Replace("{EMP_NAME}", emp.Field<string>("NameInBengali"));
                appointmentLetterBuilder.Replace("{JOINING_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{EMP_DEPARTMENT}", emp.Field<string>("Department"));
                appointmentLetterBuilder.Replace("{EMP_FATHER_NAME}", emp.Field<string>("FathersNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_MOTHER_NAME}", emp.Field<string>("MothersNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_DESIGNATION}", emp.Field<string>("Designation"));
                appointmentLetterBuilder.Replace("{EMP_CARDNO}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("EmployeeCardId")));
                appointmentLetterBuilder.Replace("{EMP_GRADE}", emp.Field<string>("Grade"));
                appointmentLetterBuilder.Replace("{EMP_SPOUSE_NAME}", emp.Field<string>("SpousesNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_PRE_ADDRESS}", emp.Field<string>("PreMailingAddress"));
                appointmentLetterBuilder.Replace("{EMP_PRE_POST_OFFICE}", emp.Field<string>("PrePostOffice"));
                appointmentLetterBuilder.Replace("{EMP_PRE_POLICE_STATION}", emp.Field<string>("PrePolice"));
                appointmentLetterBuilder.Replace("{EMP_PRE_DISTRICT}", emp.Field<string>("PreDist"));
                appointmentLetterBuilder.Replace("{EMP_PER_ADDRESS}", emp.Field<string>("PerMailingAddress"));
                appointmentLetterBuilder.Replace("{EMP_PER_POST_OFFICE}", emp.Field<string>("PerPostOffice"));
                appointmentLetterBuilder.Replace("{EMP_PER_POLICE_STATION}", emp.Field<string>("PerPolice"));
                appointmentLetterBuilder.Replace("{EMP_PER_DISTRICT}", emp.Field<string>("PerDist"));
                appointmentLetterBuilder.Replace("{PROBATION_END_DATE}", BanglaConversion.ConvertToBanglaNumber(ProbationEndDate));
                appointmentLetterBuilder.Replace("{GROSS_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("GrossSalary").ToString()));
                appointmentLetterBuilder.Replace("{BASIC_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("BasicSalary").ToString()));
                appointmentLetterBuilder.Replace("{HOUSE_RENT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("HouseRent").ToString()));
                appointmentLetterBuilder.Replace("{MEDICAL_ALLOWANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("MedicalAllowance").ToString()));
                appointmentLetterBuilder.Replace("{FOOD_ALLOWANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("FoodAllowance").ToString()));
                appointmentLetterBuilder.Replace("{TRANSPORT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("Conveyance").ToString()));
                appointmentLetterBuilder.Replace("{WeekEnd}", BanglaConversion.ConvertToBanglaNumber(emp.Field<int>("Weekend").ToString()));
                appointmentLetterBuilder.Replace("{OVERTIME_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("OverTimeRate").ToString()));
                appointmentLetterBuilder.Replace("{EMPLOYEE_OT_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("EmployeeOTRate").ToString()));
                appointmentLetterBuilder.Replace("{AMOUNT_IN_WORDS}", emp.Field<string>("AmountInWords"));
                appointmentLetterBuilder.Replace("{EMPLOYEE_SKILL_TYPE}", emp.Field<string>("SkillType"));
                appointmentLetterBuilder.Replace("{EMPLOYEE_PHOTOGRAPH_PATH}", emp.Field<string>("PhotographPath"));
                appointmentLetterBuilder.Replace("{APPLICATION_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{EMP_SECTION}", emp.Field<string>("Section"));
                appointmentLetterBuilder.Replace("{MARITAL_STATE}", emp.Field<string>("MaritalState"));
                appointmentLetterBuilder.Replace("{BIRTH_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("DateOfBirth")));
                appointmentLetterBuilder.Replace("{NATIONALITY}", emp.Field<string>("NationalityInBengali"));
                appointmentLetterBuilder.Replace("{RELIGION}", emp.Field<string>("ReligionName"));
                appointmentLetterBuilder.Replace("{MARRIAGE_ANNIVERSARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("MarriageAnniversaryDate")));
                appointmentLetterBuilder.Replace("{NATIONALID}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("NationalIdNo")));
                appointmentLetterBuilder.Replace("{EDUCATION_LEVEL}", emp.Field<string>("EducationLevel"));
                appointmentLetterBuilder.Replace("{MOBILEPHONE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("MobilePhone")));
                appointmentLetterBuilder.Replace("{HOMEPHONE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("HomePhone")));
                appointmentLetterBuilder.Replace("{EMERGENCY_PERSON}", emp.Field<string>("EmergencyContactPersonInBengali"));
                appointmentLetterBuilder.Replace("{EMERGENCY_PERSON_RELATION}", emp.Field<string>("ContactPersonRelationInBengali"));
                appointmentLetterBuilder.Replace("{EMERGENCY_PERSON_PHONE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("ContactPersonPhone")));
                appointmentLetterBuilder.Replace("{YEAR}", BanglaConversion.ConvertToBanglaNumber(prepareDate.Year.ToString()));
            }

            if (leaveDate.Count > 0)
            {
                foreach (var t in leaveDate)
                {
                    if (t.Title == "Casual Leave")
                    {
                        appointmentLetterBuilder.Replace("{CASUAL_LEAVE_ENJOY}", BanglaConversion.ConvertToBanglaNumber(t.Total.ToString()));
                        appointmentLetterBuilder.Replace("{CASUAL_LEAVE_LEFT}", BanglaConversion.ConvertToBanglaNumber(t.Available.ToString()));
                    }
                    else if (t.Title == "Sick Leave")
                    {
                        appointmentLetterBuilder.Replace("{SICK_LEAVE_ENJOY}", BanglaConversion.ConvertToBanglaNumber(t.Total.ToString()));
                        appointmentLetterBuilder.Replace("{SICK_LEAVE_LEFT}", BanglaConversion.ConvertToBanglaNumber(t.Available.ToString()));
                    }
                    else if (t.Title == "Earn Leave")
                    {
                        appointmentLetterBuilder.Replace("{EARN_LEAVE}", BanglaConversion.ConvertToBanglaNumber(t.Available.ToString()));
                        appointmentLetterBuilder.Replace("{EARN_LEAVE_ENJOY}", BanglaConversion.ConvertToBanglaNumber(t.Total.ToString()));
                        appointmentLetterBuilder.Replace("{EARN_LEAVE_LEFT}", BanglaConversion.ConvertToBanglaNumber(t.Available.ToString()));
                    }
                    else if (t.Title == "Maternity Leave")
                    {
                        appointmentLetterBuilder.Replace("{MATERNITY_LEAVE}", BanglaConversion.ConvertToBanglaNumber(t.Available.ToString()) + " দিন");
                        appointmentLetterBuilder.Replace("{MATERNITY_LEAVE_ENJOY}", BanglaConversion.ConvertToBanglaNumber(t.Total.ToString()) + " দিন");
                        appointmentLetterBuilder.Replace("{MATERNITY_LEAVE_LEFT}", BanglaConversion.ConvertToBanglaNumber(t.Available.ToString()) + " দিন");
                    }
                }
            }

            if (emp.Field<string>("Gender").ToString() == "Male")
            {
                appointmentLetterBuilder.Replace("{MATERNITY_LEAVE}", "প্রযোজ্য নয়");
                appointmentLetterBuilder.Replace("{MATERNITY_LEAVE_ENJOY}", "প্রযোজ্য নয়");
                appointmentLetterBuilder.Replace("{MATERNITY_LEAVE_LEFT}", "প্রযোজ্য নয়");
            }

            return appointmentLetterBuilder.Replace("\n", "").Replace("\r", "").ToString().Trim();
        }

        public DataTable GetEmployeeLeaveSummary(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? employeeTypeId, string employeeCardId, DateTime? fromDate, DateTime? toDate)
        {
            return HrmReportRepository.GetEmployeeLeaveSummary(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, employeeTypeId, employeeCardId, fromDate, toDate);
        }

        public string GetLeaveApplicationStaffInfo(Guid employeeId, string employeeCardId, string userName, DateTime prepareDate)
        {
            DataRow emp;
            DataTable table = HrmReportRepository.LeaveApplicationStaff(employeeId, userName, prepareDate);

            List<EmployeeLeaveData> leaveDate = HrmReportRepository.GetEmployeeLeaveData(employeeCardId, prepareDate.Year);

            if (table == null)
                return null;

            if (table.Rows.Count > 0)
                emp = table.Rows[0];
            else
                return null;

            string DateToday = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime joinDate = emp.Field<DateTime>("JoinDateCalculation");
            DateTime confirmationDate = emp.Field<DateTime>("ConfirmationDate");
            string ProbationEndDate = confirmationDate.AddDays(-1).ToString("dd/MM/yyyy");

            var appointmentInfo = AppointmentLetter.Create(HostingEnvironment.MapPath("~/Content/LeaveApplicationStaff.xml"));

            var appointmentLetterBuilder = new StringBuilder();

            appointmentLetterBuilder.AppendLine(appointmentInfo.EmployeeSpecificInfo);

            if (emp != null)
            {
                appointmentLetterBuilder.Replace("{DATE_TODAY}", DateToday);
                appointmentLetterBuilder.Replace("{PREPARE_DATE}", emp.Field<string>("PrepareDate"));
                appointmentLetterBuilder.Replace("{EMP_NAME}", emp.Field<string>("Name"));
                appointmentLetterBuilder.Replace("{JOINING_DATE}", emp.Field<string>("JoiningDate"));
                appointmentLetterBuilder.Replace("{EMP_DEPARTMENT}", emp.Field<string>("Department"));
                appointmentLetterBuilder.Replace("{EMP_FATHER_NAME}", emp.Field<string>("FathersName"));
                appointmentLetterBuilder.Replace("{EMP_MOTHER_NAME}", emp.Field<string>("MothersName"));
                appointmentLetterBuilder.Replace("{EMP_DESIGNATION}", emp.Field<string>("Designation"));
                appointmentLetterBuilder.Replace("{EMP_CARDNO}", emp.Field<string>("EmployeeCardId"));
                appointmentLetterBuilder.Replace("{EMP_GRADE}", emp.Field<string>("Grade"));
                appointmentLetterBuilder.Replace("{EMP_SPOUSE_NAME}", emp.Field<string>("SpousesName"));
                appointmentLetterBuilder.Replace("{EMP_PRE_ADDRESS}", emp.Field<string>("PreMailingAddress"));
                appointmentLetterBuilder.Replace("{EMP_PRE_POST_OFFICE}", emp.Field<string>("PrePostOffice"));
                appointmentLetterBuilder.Replace("{EMP_PRE_POLICE_STATION}", emp.Field<string>("PrePolice"));
                appointmentLetterBuilder.Replace("{EMP_PRE_DISTRICT}", emp.Field<string>("PreDist"));
                appointmentLetterBuilder.Replace("{EMP_PER_ADDRESS}", emp.Field<string>("PerMailingAddress"));
                appointmentLetterBuilder.Replace("{EMP_PER_POST_OFFICE}", emp.Field<string>("PerPostOffice"));
                appointmentLetterBuilder.Replace("{EMP_PER_POLICE_STATION}", emp.Field<string>("PerPolice"));
                appointmentLetterBuilder.Replace("{EMP_PER_DISTRICT}", emp.Field<string>("PerDist"));
                appointmentLetterBuilder.Replace("{PROBATION_END_DATE}", ProbationEndDate);
                appointmentLetterBuilder.Replace("{GROSS_SALARY}", emp.Field<decimal?>("GrossSalary").ToString());
                appointmentLetterBuilder.Replace("{BASIC_SALARY}", emp.Field<decimal?>("BasicSalary").ToString());
                appointmentLetterBuilder.Replace("{HOUSE_RENT}", emp.Field<decimal?>("HouseRent").ToString());
                appointmentLetterBuilder.Replace("{MEDICAL_ALLOWANCE}", emp.Field<decimal?>("MedicalAllowance").ToString());
                appointmentLetterBuilder.Replace("{FOOD_ALLOWANCE}", emp.Field<decimal?>("FoodAllowance").ToString());
                appointmentLetterBuilder.Replace("{TRANSPORT}", emp.Field<decimal?>("Conveyance").ToString());
                appointmentLetterBuilder.Replace("{WeekEnd}", emp.Field<int>("Weekend").ToString());
                appointmentLetterBuilder.Replace("{OVERTIME_RATE}", emp.Field<decimal?>("OverTimeRate").ToString());
                appointmentLetterBuilder.Replace("{EMPLOYEE_OT_RATE}", emp.Field<decimal?>("EmployeeOTRate").ToString());
                appointmentLetterBuilder.Replace("{AMOUNT_IN_WORDS}", emp.Field<string>("AmountInWords"));
                appointmentLetterBuilder.Replace("{EMPLOYEE_SKILL_TYPE}", emp.Field<string>("SkillType"));
                appointmentLetterBuilder.Replace("{EMPLOYEE_PHOTOGRAPH_PATH}", emp.Field<string>("PhotographPath"));
                appointmentLetterBuilder.Replace("{APPLICATION_DATE}", emp.Field<string>("JoiningDate"));
                appointmentLetterBuilder.Replace("{EMP_SECTION}", emp.Field<string>("Section"));
                appointmentLetterBuilder.Replace("{MARITAL_STATE}", emp.Field<string>("MaritalState"));
                appointmentLetterBuilder.Replace("{BIRTH_DATE}", emp.Field<string>("DateOfBirth"));
                appointmentLetterBuilder.Replace("{NATIONALITY}", emp.Field<string>("Nationality"));
                appointmentLetterBuilder.Replace("{RELIGION}", emp.Field<string>("ReligionName"));
                appointmentLetterBuilder.Replace("{MARRIAGE_ANNIVERSARY}", emp.Field<string>("MarriageAnniversaryDate"));
                appointmentLetterBuilder.Replace("{NATIONALID}", emp.Field<string>("NationalIdNo"));
                appointmentLetterBuilder.Replace("{EDUCATION_LEVEL}", emp.Field<string>("EducationLevel"));
                appointmentLetterBuilder.Replace("{MOBILEPHONE}", emp.Field<string>("MobilePhone"));
                appointmentLetterBuilder.Replace("{HOMEPHONE}", emp.Field<string>("HomePhone"));
                appointmentLetterBuilder.Replace("{EMERGENCY_PERSON}", emp.Field<string>("EmergencyContactPerson"));
                appointmentLetterBuilder.Replace("{EMERGENCY_PERSON_RELATION}", emp.Field<string>("ContactPersonRelation"));
                appointmentLetterBuilder.Replace("{EMERGENCY_PERSON_PHONE}", emp.Field<string>("ContactPersonPhone"));
                appointmentLetterBuilder.Replace("{YEAR}", prepareDate.Year.ToString());
            }

            if (leaveDate.Count > 0)
            {
                foreach (var t in leaveDate)
                {
                    if (t.Title == "Casual Leave")
                    {
                        appointmentLetterBuilder.Replace("{CASUAL_LEAVE_ENJOY}", t.Total.ToString());
                        appointmentLetterBuilder.Replace("{CASUAL_LEAVE_LEFT}", t.Available.ToString());
                    }
                    else if (t.Title == "Sick Leave")
                    {
                        appointmentLetterBuilder.Replace("{SICK_LEAVE_ENJOY}", t.Total.ToString());
                        appointmentLetterBuilder.Replace("{SICK_LEAVE_LEFT}", t.Available.ToString());
                    }
                    else if (t.Title == "Earn Leave")
                    {
                        appointmentLetterBuilder.Replace("{EARN_LEAVE}", t.Available.ToString());
                        appointmentLetterBuilder.Replace("{EARN_LEAVE_ENJOY}", t.Total.ToString());
                        appointmentLetterBuilder.Replace("{EARN_LEAVE_LEFT}", t.Available.ToString());
                    }
                    else if (t.Title == "Maternity Leave")
                    {
                        appointmentLetterBuilder.Replace("{MATERNITY_LEAVE}", t.Available.ToString() + " Day");
                        appointmentLetterBuilder.Replace("{MATERNITY_LEAVE_ENJOY}", t.Total.ToString() + " Day");
                        appointmentLetterBuilder.Replace("{MATERNITY_LEAVE_LEFT}", t.Available.ToString() + " Day");
                    }
                }
            }

            if (emp.Field<string>("Gender").ToString() == "Male")
            {
                appointmentLetterBuilder.Replace("{MATERNITY_LEAVE}", "Not Applicable");
                appointmentLetterBuilder.Replace("{MATERNITY_LEAVE_ENJOY}", "Not Applicable");
                appointmentLetterBuilder.Replace("{MATERNITY_LEAVE_LEFT}", "Not Applicable");
            }

            return appointmentLetterBuilder.Replace("\n", "").Replace("\r", "").ToString().Trim();
        }

        public string GetNominationFormInfo(Guid employeeId, string userName, DateTime prepareDate)
        {
            DataRow emp;
            DataTable table = HrmReportRepository.GetNominationFormInfo(employeeId, userName, prepareDate);

            if (table == null) return null;

            if (table.Rows.Count > 0)
                emp = table.Rows[0];
            else
                return null;

            string DateToday = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime joinDate = emp.Field<DateTime>("JoinDateCalculation");
            DateTime confirmationDate = emp.Field<DateTime>("ConfirmationDate");
            string ProbationEndDate = confirmationDate.AddDays(-1).ToString("dd/MM/yyyy");

            var appointmentInfo = AppointmentLetter.Create(HostingEnvironment.MapPath("~/Content/HRM/NominationForm.xml"));

            var appointmentLetterBuilder = new StringBuilder();
            appointmentLetterBuilder.AppendLine(appointmentInfo.EmployeeSpecificInfo);

            if (emp != null)
            {
                appointmentLetterBuilder.Replace("{DATE_TODAY}", BanglaConversion.ConvertToBanglaNumber(DateToday));
                appointmentLetterBuilder.Replace("{PREPARE_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("PrepareDate")));
                appointmentLetterBuilder.Replace("{EMP_NAME}", emp.Field<string>("NameInBengali"));
                appointmentLetterBuilder.Replace("{JOINING_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{EMP_DEPARTMENT}", emp.Field<string>("Department"));
                appointmentLetterBuilder.Replace("{EMP_FATHER_NAME}", emp.Field<string>("FathersNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_MOTHER_NAME}", emp.Field<string>("MothersNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_DESIGNATION}", emp.Field<string>("Designation"));
                appointmentLetterBuilder.Replace("{EMP_CARDNO}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("EmployeeCardId")));
                appointmentLetterBuilder.Replace("{EMP_GRADE}", emp.Field<string>("Grade"));
                appointmentLetterBuilder.Replace("{EMP_SPOUSE_NAME}", emp.Field<string>("SpousesNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_PRE_ADDRESS}", emp.Field<string>("PreMailingAddress"));
                appointmentLetterBuilder.Replace("{EMP_PRE_POST_OFFICE}", emp.Field<string>("PrePostOffice"));
                appointmentLetterBuilder.Replace("{EMP_PRE_POLICE_STATION}", emp.Field<string>("PrePolice"));
                appointmentLetterBuilder.Replace("{EMP_PRE_DISTRICT}", emp.Field<string>("PreDist"));
                appointmentLetterBuilder.Replace("{EMP_PER_ADDRESS}", emp.Field<string>("PerMailingAddress"));
                appointmentLetterBuilder.Replace("{EMP_PER_POST_OFFICE}", emp.Field<string>("PerPostOffice"));
                appointmentLetterBuilder.Replace("{EMP_PER_POLICE_STATION}", emp.Field<string>("PerPolice"));
                appointmentLetterBuilder.Replace("{EMP_PER_DISTRICT}", emp.Field<string>("PerDist"));
                appointmentLetterBuilder.Replace("{PROBATION_END_DATE}", BanglaConversion.ConvertToBanglaNumber(ProbationEndDate));
                appointmentLetterBuilder.Replace("{GROSS_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("GrossSalary").ToString()));
                appointmentLetterBuilder.Replace("{BASIC_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("BasicSalary").ToString()));
                appointmentLetterBuilder.Replace("{HOUSE_RENT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("HouseRent").ToString()));
                appointmentLetterBuilder.Replace("{MEDICAL_ALLOWANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("MedicalAllowance").ToString()));
                appointmentLetterBuilder.Replace("{FOOD_ALLOWANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("FoodAllowance").ToString()));
                appointmentLetterBuilder.Replace("{TRANSPORT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("Conveyance").ToString()));
                appointmentLetterBuilder.Replace("{WeekEnd}", BanglaConversion.ConvertToBanglaNumber(emp.Field<int>("Weekend").ToString()));
                appointmentLetterBuilder.Replace("{OVERTIME_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("OverTimeRate").ToString()));
                appointmentLetterBuilder.Replace("{EMPLOYEE_OT_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("EmployeeOTRate").ToString()));
                appointmentLetterBuilder.Replace("{AMOUNT_IN_WORDS}", emp.Field<string>("AmountInWords"));
                appointmentLetterBuilder.Replace("{EMPLOYEE_SKILL_TYPE}", emp.Field<string>("SkillType"));
                appointmentLetterBuilder.Replace("{EMPLOYEE_PHOTOGRAPH_PATH}", emp.Field<string>("PhotographPath"));
                appointmentLetterBuilder.Replace("{APPLICATION_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{EMP_SECTION}", emp.Field<string>("Section"));
                appointmentLetterBuilder.Replace("{MARITAL_STATE}", emp.Field<string>("MaritalState"));
                appointmentLetterBuilder.Replace("{BIRTH_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("DateOfBirth")));
                appointmentLetterBuilder.Replace("{AGE}", emp.Field<string>("Age"));
                appointmentLetterBuilder.Replace("{AGE_BANGLA}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("Age")));
                appointmentLetterBuilder.Replace("{GENDER}", emp.Field<string>("Gender"));

                if (emp.Field<string>("EmployeeType") == "Team Member-A")
                {
                    appointmentLetterBuilder.Replace("{MONTH}", "০৩ (তিন)");
                }
                else
                {
                    appointmentLetterBuilder.Replace("{MONTH}", "০৬ (ছয়)");
                }
            }
            return appointmentLetterBuilder.Replace("\n", "").Replace("\r", "").ToString().Trim();
        }

        public DataTable GetManPowerBudget(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {
            return HrmReportRepository.GetManPowerBudget(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId, userName, fromDate, toDate);
        }

        public DataTable GetDailyOTDetailWithAmount(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {
            return HrmReportRepository.GetDailyOTDetailWithAmount(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId, userName, fromDate, toDate);
        }

        public DataTable GetEmployeeEarnLeaveQuit(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {
            return HrmReportRepository.GetEmployeeEarnLeaveQuit(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId, userName, fromDate, toDate);
        }

        public DataTable GetFemaleConsentLetterInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, string employeeCardId, DateTime? disagreeDate, DateTime? effectiveDate)
        {
            DataTable dataTable = new DataTable();
            DataTable dataTableResult = new DataTable();

            dataTableResult.Columns.Add("SerialNo", typeof(string));
            dataTableResult.Columns.Add("CompanyNameInBengali", typeof(string));
            dataTableResult.Columns.Add("FullAddressInBengali", typeof(string));
            dataTableResult.Columns.Add("EmployeeCardId", typeof(string));
            dataTableResult.Columns.Add("NameInBengali", typeof(string));
            dataTableResult.Columns.Add("DesignationInBengali", typeof(string));
            dataTableResult.Columns.Add("JoiningDate", typeof(string));
            dataTableResult.Columns.Add("DisagreeDate", typeof(string));

            dataTable = HrmReportRepository.GetFemaleConsentLetterInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, employeeCardId, disagreeDate, effectiveDate);

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                string SerialNo = BanglaConversion.ConvertToBanglaNumber(dataTable.Rows[i].Field<string>("SerialNo"));
                string CompanyNameInBengali = dataTable.Rows[i].Field<string>("CompanyNameInBengali");
                string FullAddressInBengali = dataTable.Rows[i].Field<string>("FullAddressInBengali");
                string EmployeeCardId = BanglaConversion.ConvertToBanglaNumber(dataTable.Rows[i].Field<string>("EmployeeCardId"));
                string NameInBengali = dataTable.Rows[i].Field<string>("NameInBengali");
                string DesignationInBengali = dataTable.Rows[i].Field<string>("DesignationInBengali");
                string JoiningDate = BanglaConversion.ConvertToBanglaNumber(dataTable.Rows[i].Field<string>("JoiningDate"));
                string DisagreeDate = BanglaConversion.ConvertToBanglaNumber(dataTable.Rows[i].Field<string>("DisagreeDate"));

                dataTableResult.Rows.Add(SerialNo, CompanyNameInBengali, FullAddressInBengali, EmployeeCardId, NameInBengali, DesignationInBengali, JoiningDate, DisagreeDate);
            }
            return dataTableResult;
        }

        public List<JobCardInfoModel> GetJobCardModelKnittingDyeingInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId,
          int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month, DateTime? fromDate,
          DateTime? toDate, string userName)
        {
            return HrmReportRepository.GetJobCardModelKnittingDyeingInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, userName);
        }

        public DataTable GetEmployeeLeaveRegister(string employeeCardId, DateTime prepareDate)
        {
            return HrmReportRepository.GetEmployeeLeaveRegister(employeeCardId, prepareDate);
        }

        public DataTable GetDyeingShiftCount(DateTime fromDate, DateTime toDate)
        {
            return HrmReportRepository.GetDyeingShiftCount(fromDate, toDate);
        }
        public DataTable GetLeaveButPresent(DateTime fromDate, DateTime toDate)
        {
            return HrmReportRepository.GetLeaveButPresent(fromDate, toDate);
        }

        public DataTable GetAbsentOnJoiningDate(DateTime fromDate, DateTime toDate)
        {
            return HrmReportRepository.GetAbsentOnJoiningDate(fromDate, toDate);
        }
    }
}