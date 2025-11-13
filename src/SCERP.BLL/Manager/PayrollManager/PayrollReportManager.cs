using System;
using System.Collections.Generic;
using SCERP.BLL.IManager.IPayrollManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.DAL.Repository.PayrollRepository;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Model.PayrollModel;
using System.Data;

namespace SCERP.BLL.Manager.PayrollManager
{
    public class PayrollReportManager : BaseManager, IPayrollReportManager
    {
        protected readonly IPayrollReportRepository PayrollReportRepository = null;
        protected readonly IHRMReportRepository HrmReportRepository = null;

        public PayrollReportManager(SCERPDBContext context)
        {
            PayrollReportRepository = new PayrollReportRepository(context);
            HrmReportRepository = new HRMReportRepository(context);
        }

        public List<PaySlipView> GetPaySlipInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId,
                                                             int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
                                                             int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {
            return PayrollReportRepository.GetPaySlipInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId);
        }

        public List<SalarySheetView> GetSalarySheetInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId,
                                                              int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
                                                              int year, int month, DateTime? fromDate, DateTime? toDate, int? employeeCategoryId)
        {
            return PayrollReportRepository.GetSalarySheetInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId);
        }

        public List<SalarySheetView> GetSalarySheetGrossDeductionInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId,
            int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
            int year, int month, DateTime? fromDate, DateTime? toDate, int? employeeCategoryId)
        {
            return PayrollReportRepository.GetSalarySheetGrossDeductionInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId);
        }

        public List<SalarySheetView> GetSalarySheetBankInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month, DateTime? fromDate, DateTime? toDate, int? employeeCategoryId)
        {
            List<SalarySheetView> bankSalary = new List<SalarySheetView>();

            bankSalary = PayrollReportRepository.GetSalarySheetBankInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId);

            foreach (var t in bankSalary)
            {
                if (t.FoodAllowance > 0)
                    t.EntertainmentAllowance = t.FoodAllowance;
            }

            return bankSalary;
        }

        public List<EmployeeAllPaymentSheetView> GetEmployeeAllPaymentSheetInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId,
                                                      int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
                                                      int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {
            return PayrollReportRepository.GetEmployeeAllPaymentSheetInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId);
        }

        public List<EmployeeAllPaymentSheetView> GetEmployeeAllPaymentSheetInfoGrossDeduction(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId,
                                                     int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
                                                     int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {
            return PayrollReportRepository.GetEmployeeAllPaymentSheetInfoGrossDeduction(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId);
        }

        public List<AdvanceSalarySheetView> GetAdvanceSalarySheetInfo(string employeeCardId, int companyId, int branchId,
            int branchUnitId, int? branchUnitDepartmentId, int? sectionId, int? lineId, DateTime fromDate,
            DateTime toDate, int employeeTypeId)
        {
            return PayrollReportRepository.GetAdvanceSalarySheetInfo(employeeCardId, companyId, branchId, branchUnitId, branchUnitDepartmentId, sectionId, lineId, fromDate, toDate, employeeTypeId);
        }

        public List<EmployeeBonusSheetView> GetEmployeeBonusSheetInfo(string employeeCardId, int companyId, int branchId, int branchUnitId, int? branchUnitDepartmentId, int? sectionId, int? lineId, int employeeTypeId, DateTime effectiveDate)
        {
            return PayrollReportRepository.GetEmployeeBonusSheetInfo(employeeCardId, companyId, branchId, branchUnitId, branchUnitDepartmentId, sectionId, lineId, employeeTypeId, effectiveDate);
        }


        public List<ExtraOTSheetView> GetExtraOTSheetInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId,
                                                              int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
                                                              int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {
            return PayrollReportRepository.GetExtraOTSheetInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId);
        }

        public List<ExtraOTSheetView> GetExtraOTSheetModelInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {
            return PayrollReportRepository.GetExtraOTSheetModelInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId);
        }

        public List<ExtraOTSheetView> GetExtraOTSheet10PMNoWeekendInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {
            return PayrollReportRepository.GetExtraOTSheet10PMNoWeekendInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId);
        }

        public List<ExtraOTSheetView> GetExtraOTSheetAfter10PMWithHolidayInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {
            return PayrollReportRepository.GetExtraOTSheetAfter10PMWithHolidayInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId);
        }

        public List<WeekendOTSheetView> GetWeekendOTSheetInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId,
                                                              int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
                                                              int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {
            return PayrollReportRepository.GetWeekendOTSheetInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                                                                 departmentLineId, employeeTypeId, employeeCardId, year, month,
                                                                 fromDate, toDate, employeeCategoryId);
        }

        public List<HolidayOTSheetView> GetHolidayOTSheetInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId,
                                                             int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
                                                             int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {
            return PayrollReportRepository.GetHolidayOTSheetInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                                                                 departmentLineId, employeeTypeId, employeeCardId, year, month,
                                                                 fromDate, toDate, employeeCategoryId);
        }

        public List<ExtraOTWeekendOTAndHolidayOTSheetView> GetExtraOTWeekendOTAndHolidayOTSheetInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId,
                                                    int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
                                                    int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {
            return PayrollReportRepository.GetExtraOTWeekendOTAndHolidayOTSheetInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                                                                 departmentLineId, employeeTypeId, employeeCardId, year, month,
                                                                 fromDate, toDate, employeeCategoryId);
        }

        public List<EmployeeSalaryInfoModel> GetEmployeeSalaryReport(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId,
            int? employeeGradeId, int? employeeDesignationId, int? genderId, DateTime? joiningDateBegin,
            DateTime? joiningDateEnd,
            DateTime? quitDateBegin, DateTime? quitDateEnd, string employeeCardId, string employeeName,
            int? activeStatus, string userName, DateTime? upToDate)
        {
            return PayrollReportRepository.GetEmployeeSalaryReport(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId,
                employeeGradeId, employeeDesignationId, genderId, joiningDateBegin, joiningDateEnd, quitDateBegin, quitDateEnd,
                 employeeCardId, employeeName, activeStatus, userName, upToDate);
        }

        public List<SalarySummaryView> GetEmployeeSalarySummary(int? companyId, int? branchId,
            List<int> branchUnitIdList,
            List<int> employeeTypeIdList, int employeeCategoryId, int year,
            int month, DateTime? fromDate, DateTime? toDate)
        {
            return PayrollReportRepository.GetEmployeeSalarySummary(companyId, branchId,
                branchUnitIdList, employeeTypeIdList, employeeCategoryId, year, month, fromDate, toDate);
        }

        public List<SalarySummaryView> GetEmployeeSalarySummaryAll(int? companyId, int? branchId,
            List<int> branchUnitIdList,
            List<int> employeeTypeIdList, int year, int month, DateTime? fromDate, DateTime? toDate)
        {
            return PayrollReportRepository.GetEmployeeSalarySummaryAll(companyId, branchId, branchUnitIdList,
                employeeTypeIdList, year, month, fromDate, toDate);
        }

        public int GetWeekendDays(DateTime fromDate, DateTime toDate)
        {
            return PayrollReportRepository.GetWeekendDays(fromDate, toDate);
        }

        public List<WeekendBillModel> GetWeekendBill(DateTime fromDate)
        {
            return PayrollReportRepository.GetWeekendBill(fromDate);
        }

        public int ProcessWeekendBill(DateTime fromDate)
        {
            return PayrollReportRepository.ProcessWeekendBill(fromDate);
        }

        public string GetCompanyNameByCompanyId(string companyId)
        {
            return PayrollReportRepository.GetCompanyNameByCompanyId(companyId);
        }

        public Company GetCompanyByCompanyId(string companyId)
        {
            return PayrollReportRepository.GetCompanyByCompanyId(companyId);
        }

        public string CategoryNameById(int categoryId)
        {
            return HrmReportRepository.CategoryNameById(categoryId);
        }

        public List<SPSalaryIncrementReport> GetSalaryIncrementReport(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, DateTime? fromDate, DateTime? toDate, string employeeCardId, float? incrementPercent, decimal? incrementAmount, string userName)
        {
            return PayrollReportRepository.GetSalaryIncrementReport(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, fromDate, toDate, employeeCardId, incrementPercent, incrementAmount, userName);
        }

        public DataTable GetSalaryBankStatement(DateTime? fromDate, DateTime? toDate)
        {
            return PayrollReportRepository.GetSalaryBankStatement(fromDate, toDate);
        }

        public DataTable SalarySummaryMultipleMonth(int? companyId, int? branchId, int? fromYear, int? toYear, int? fromMonth, int? toMonth, DateTime? fromDate, DateTime? toDate)
        {
            return PayrollReportRepository.SalarySummaryMultipleMonth(companyId, branchId, fromYear, toYear, fromMonth, toMonth, fromDate, toDate);
        }

        public DataTable SalarySummaryTopSheet(int? companyId, int? branchId, int? year, int? month, DateTime? fromDate, DateTime? toDate)
        {
            return PayrollReportRepository.SalarySummaryTopSheet(companyId, branchId, year, month, fromDate, toDate);
        }

        public DataTable SalaryXL(int? companyId, int? branchId, int? year, int? month, DateTime? fromDate, DateTime? toDate)
        {
            return PayrollReportRepository.SalaryXL(companyId, branchId, year, month, fromDate, toDate);
        }
    }
}
