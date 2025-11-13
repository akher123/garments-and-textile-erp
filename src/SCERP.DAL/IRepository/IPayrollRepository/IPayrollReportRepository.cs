using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Model.PayrollModel;
using System.Data;

namespace SCERP.DAL.IRepository.IPayrollRepository
{
    public interface IPayrollReportRepository : IRepository<Employee>
    {
        List<PaySlipView> GetPaySlipInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId,
            int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
            int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId);

        List<SalarySheetView> GetSalarySheetInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId,
            int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
            int year, int month, DateTime? fromDate, DateTime? toDate, int? employeeCategoryId);

        List<SalarySheetView> GetSalarySheetGrossDeductionInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId,
            int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
            int year, int month, DateTime? fromDate, DateTime? toDate, int? employeeCategoryId);

        List<SalarySheetView> GetSalarySheetBankInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month, DateTime? fromDate, DateTime? toDate, int? employeeCategoryId);

        int GetWeekendDays(DateTime fromDate, DateTime toDate);

        List<AdvanceSalarySheetView> GetAdvanceSalarySheetInfo(string employeeCardId, int companyId, int branchId,
            int branchUnitId, int? branchUnitDepartmentId, int? sectionId, int? lineId, DateTime fromDate,
            DateTime toDate, int employeeTypeId);

        List<EmployeeBonusSheetView> GetEmployeeBonusSheetInfo(string employeeCardId, int companyId, int branchId,
            int branchUnitId, int? branchUnitDepartmentId, int? sectionId, int? lineId, int employeeTypeId,
            DateTime effectiveDate);

        List<ExtraOTSheetView> GetExtraOTSheetInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId,
            int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
            int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId);

        List<ExtraOTSheetView> GetExtraOTSheetModelInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId);

        List<ExtraOTSheetView> GetExtraOTSheet10PMNoWeekendInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId);

        List<ExtraOTSheetView> GetExtraOTSheetAfter10PMWithHolidayInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId);

        List<WeekendOTSheetView> GetWeekendOTSheetInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month,
            DateTime? fromDate, DateTime? toDate, int employeeCategoryId);

        List<EmployeeAllPaymentSheetView> GetEmployeeAllPaymentSheetInfo(int? companyId, int? branchId,
            int? branchUnitId, int? branchUnitDepartmentId,
            int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
            int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId);

        List<EmployeeAllPaymentSheetView> GetEmployeeAllPaymentSheetInfoGrossDeduction(int? companyId, int? branchId,
            int? branchUnitId, int? branchUnitDepartmentId,
            int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
            int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId);

        List<HolidayOTSheetView> GetHolidayOTSheetInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month,
            DateTime? fromDate, DateTime? toDate, int employeeCategoryId);

        List<ExtraOTWeekendOTAndHolidayOTSheetView> GetExtraOTWeekendOTAndHolidayOTSheetInfo(int? companyId,
            int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month,
            DateTime? fromDate, DateTime? toDate, int employeeCategoryId);

        List<EmployeeSalaryInfoModel> GetEmployeeSalaryReport(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId,
            int? employeeGradeId, int? employeeDesignationId, int? genderId, DateTime? joiningDateBegin, DateTime? joiningDateEnd,
            DateTime? quitDateBegin, DateTime? quitDateEnd, string employeeCardId, string employeeName,
            int? activeStatus, string userName, DateTime? upToDate);

        List<SalarySummaryView> GetEmployeeSalarySummary(int? companyId, int? branchId,
            List<int> branchUnitIdList, List<int> employeeTypeIdList, int employeeCategoryId, int year,
            int month, DateTime? fromDate, DateTime? toDate);

        List<SalarySummaryView> GetEmployeeSalarySummaryAll(int? companyId, int? branchId, List<int> branchUnitIdList,
            List<int> employeeTypeIdList, int year, int month, DateTime? fromDate, DateTime? toDate);

        List<WeekendBillModel> GetWeekendBill(DateTime fromDate);

        int ProcessWeekendBill(DateTime fromDate);

        string GetCompanyNameByCompanyId(string companyId);

        Company GetCompanyByCompanyId(string companyId);

        string CategoryNameById(int categoryId);

        List<SPSalaryIncrementReport> GetSalaryIncrementReport(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, DateTime? fromDate, DateTime? toDate, string employeeCardId, float? incrementPercent, decimal? incrementAmount, string userName);

        DataTable GetSalaryBankStatement(DateTime? fromDate, DateTime? toDate);

        DataTable SalarySummaryMultipleMonth(int? companyId, int? branchId, int? fromYear, int? toYear, int? fromMonth, int? toMonth, DateTime? fromDate, DateTime? toDate);

        DataTable SalarySummaryTopSheet(int? companyId, int? branchId, int? year, int? month, DateTime? fromDate, DateTime? toDate);

        DataTable SalaryXL(int? companyId, int? branchId, int? year, int? month, DateTime? fromDate, DateTime? toDate);
    }
}
