using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.Model;
using SCERP.Model.Custom;
using System.Data.Entity.Core.Objects;
using SCERP.Common;
using SCERP.Model.PayrollModel;


namespace SCERP.DAL.Repository.PayrollRepository
{
    public class PayrollReportRepository : Repository<Employee>, IPayrollReportRepository
    {
        public PayrollReportRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public List<SalarySheetView> GetSalarySheetInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId,
            int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
            int year, int month, DateTime? fromDate, DateTime? toDate, int? employeeCategoryId)
        {

            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter
                {
                    ParameterName = "BranchUnitDepartmentId",
                    Value = branchUnitDepartmentId
                };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentSectionId",
                    Value = departmentSectionId
                };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentLineId",
                    Value = departmentLineId
                };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter("Year", year);

                var monthParam = new SqlParameter("Month", month);

                var fromDateParam = new SqlParameter("FromDate", fromDate);

                var toDateParam = new SqlParameter("Todate", toDate);

                if (employeeCategoryId == 0)
                    employeeCategoryId = -1;
                var employeeCategoryIdParam = new SqlParameter
                {
                    ParameterName = "EmployeeCategoryId",
                    Value = employeeCategoryId
                };

                var userNameParam = new SqlParameter
                {
                    ParameterName = "UserName",
                    Value = PortalContext.CurrentUser.Name
                };


                return
                    Context.Database.SqlQuery<SalarySheetView>(
                        "SPGetSalarySheet @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                        "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeCardId, @Year, @Month, " +
                        "@FromDate, @ToDate, @EmployeeCategoryId, @UserName", companyIdParam, branchIdParam,
                        branchUnitIdParam,
                        branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                        employeeTypeIdParam,
                        employeeCardIdParam, yearParam, monthParam, fromDateParam,
                        toDateParam, employeeCategoryIdParam, userNameParam).ToList();



            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<SalarySheetView> GetSalarySheetGrossDeductionInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId,
            int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
            int year, int month, DateTime? fromDate, DateTime? toDate, int? employeeCategoryId)
        {

            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter
                {
                    ParameterName = "BranchUnitDepartmentId",
                    Value = branchUnitDepartmentId
                };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentSectionId",
                    Value = departmentSectionId
                };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentLineId",
                    Value = departmentLineId
                };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter("Year", year);

                var monthParam = new SqlParameter("Month", month);

                var fromDateParam = new SqlParameter("FromDate", fromDate);

                var toDateParam = new SqlParameter("Todate", toDate);

                if (employeeCategoryId == 0)
                    employeeCategoryId = -1;
                var employeeCategoryIdParam = new SqlParameter
                {
                    ParameterName = "EmployeeCategoryId",
                    Value = employeeCategoryId
                };

                var userNameParam = new SqlParameter
                {
                    ParameterName = "UserName",
                    Value = PortalContext.CurrentUser.Name
                };

                return
                    Context.Database.SqlQuery<SalarySheetView>(
                        "SPGetSalarySheetDeduction @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                        "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeCardId, @Year, @Month, " +
                        "@FromDate, @ToDate, @EmployeeCategoryId, @UserName", companyIdParam, branchIdParam,
                        branchUnitIdParam,
                        branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                        employeeTypeIdParam,
                        employeeCardIdParam, yearParam, monthParam, fromDateParam,
                        toDateParam, employeeCategoryIdParam, userNameParam).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<SalarySheetView> GetSalarySheetBankInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month, DateTime? fromDate, DateTime? toDate, int? employeeCategoryId)
        {
            try
            {
                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter
                {
                    ParameterName = "BranchUnitDepartmentId",
                    Value = branchUnitDepartmentId
                };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentSectionId",
                    Value = departmentSectionId
                };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentLineId",
                    Value = departmentLineId
                };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter("Year", year);

                var monthParam = new SqlParameter("Month", month);

                var fromDateParam = new SqlParameter("FromDate", fromDate);

                var toDateParam = new SqlParameter("Todate", toDate);

                if (employeeCategoryId == 0)
                    employeeCategoryId = -1;
                var employeeCategoryIdParam = new SqlParameter
                {
                    ParameterName = "EmployeeCategoryId",
                    Value = employeeCategoryId
                };

                var userNameParam = new SqlParameter
                {
                    ParameterName = "UserName",
                    Value = PortalContext.CurrentUser.Name
                };

                return Context.Database.SqlQuery<SalarySheetView>(
                    "SPGetSalarySheetBank @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                    "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeCardId, @Year, @Month, " +
                    "@FromDate, @ToDate, @EmployeeCategoryId, @UserName", companyIdParam, branchIdParam,
                    branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                    employeeTypeIdParam, employeeCardIdParam, yearParam, monthParam, fromDateParam, toDateParam, employeeCategoryIdParam, userNameParam).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<EmployeeAllPaymentSheetView> GetEmployeeAllPaymentSheetInfo(int? companyId, int? branchId,
            int? branchUnitId, int? branchUnitDepartmentId,
            int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
            int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {

            try
            {
                if (companyId == 0)
                    companyId = -1;

                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;

                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;

                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;

                var branchUnitDepartmentIdParam = new SqlParameter
                {
                    ParameterName = "BranchUnitDepartmentId",
                    Value = branchUnitDepartmentId
                };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;

                var departmentSectionIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentSectionId",
                    Value = departmentSectionId
                };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentLineId",
                    Value = departmentLineId
                };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;

                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;

                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter("Year", year);

                var monthParam = new SqlParameter("Month", month);

                var fromDateParam = new SqlParameter("FromDate", fromDate);

                var toDateParam = new SqlParameter("Todate", toDate);

                var employeeCategoryIdParam = new SqlParameter
                {
                    ParameterName = "EmployeeCategoryId",
                    Value = employeeCategoryId
                };

                var userNameParam = new SqlParameter
                {
                    ParameterName = "UserName",
                    Value = PortalContext.CurrentUser.Name
                };

                return Context.Database.SqlQuery<EmployeeAllPaymentSheetView>(
                    "SPGetEmployeeAllPayments @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                    "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeCardId, @Year, @Month, " +
                    "@FromDate, @ToDate, @EmployeeCategoryId, @UserName", companyIdParam, branchIdParam,
                    branchUnitIdParam,
                    branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                    employeeTypeIdParam,
                    employeeCardIdParam, yearParam, monthParam, fromDateParam,
                    toDateParam, employeeCategoryIdParam, userNameParam).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<EmployeeAllPaymentSheetView> GetEmployeeAllPaymentSheetInfoGrossDeduction(int? companyId, int? branchId,
            int? branchUnitId, int? branchUnitDepartmentId,
            int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
            int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {

            try
            {
                if (companyId == 0)
                    companyId = -1;

                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;

                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;

                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;

                var branchUnitDepartmentIdParam = new SqlParameter
                {
                    ParameterName = "BranchUnitDepartmentId",
                    Value = branchUnitDepartmentId
                };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;

                var departmentSectionIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentSectionId",
                    Value = departmentSectionId
                };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentLineId",
                    Value = departmentLineId
                };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;

                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;

                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter("Year", year);

                var monthParam = new SqlParameter("Month", month);

                var fromDateParam = new SqlParameter("FromDate", fromDate);

                var toDateParam = new SqlParameter("Todate", toDate);

                var employeeCategoryIdParam = new SqlParameter
                {
                    ParameterName = "EmployeeCategoryId",
                    Value = employeeCategoryId
                };

                var userNameParam = new SqlParameter
                {
                    ParameterName = "UserName",
                    Value = PortalContext.CurrentUser.Name
                };

                return Context.Database.SqlQuery<EmployeeAllPaymentSheetView>(
                    "SPGetEmployeeAllPaymentsGrossDeduction @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                    "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeCardId, @Year, @Month, " +
                    "@FromDate, @ToDate, @EmployeeCategoryId, @UserName", companyIdParam, branchIdParam,
                    branchUnitIdParam,
                    branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                    employeeTypeIdParam,
                    employeeCardIdParam, yearParam, monthParam, fromDateParam,
                    toDateParam, employeeCategoryIdParam, userNameParam).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }


        public List<PaySlipView> GetPaySlipInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId,
            int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
            int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {

            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter
                {
                    ParameterName = "BranchUnitDepartmentId",
                    Value = branchUnitDepartmentId
                };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentSectionId",
                    Value = departmentSectionId
                };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentLineId",
                    Value = departmentLineId
                };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter("Year", year);

                var monthParam = new SqlParameter("Month", month);

                var fromDateParam = new SqlParameter("FromDate", fromDate);

                var toDateParam = new SqlParameter("Todate", toDate);


                var employeeCategoryIdParam = new SqlParameter
                {
                    ParameterName = "EmployeeCategoryId",
                    Value = employeeCategoryId
                };


                var userNameParam = new SqlParameter
                {
                    ParameterName = "UserName",
                    Value = PortalContext.CurrentUser.Name
                };


                return
                    Context.Database.SqlQuery<PaySlipView>(
                        "SPGetPaySlip @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                        "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeCardId, @Year, @Month, " +
                        "@FromDate, @ToDate, @EmployeeCategoryId, @UserName", companyIdParam, branchIdParam,
                        branchUnitIdParam,
                        branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                        employeeTypeIdParam,
                        employeeCardIdParam, yearParam, monthParam, fromDateParam,
                        toDateParam, employeeCategoryIdParam, userNameParam).ToList();



            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<AdvanceSalarySheetView> GetAdvanceSalarySheetInfo(string employeeCardId, int companyId, int branchId,
            int branchUnitId, int? branchUnitDepartmentId, int? sectionId, int? lineId, DateTime fromDate,
            DateTime toDate, int employeeTypeId)
        {

            var listSalarySheetModel = new List<AdvanceSalarySheetView>();

            try
            {
                var employeeCardIdParam = !String.IsNullOrEmpty(employeeCardId)
                    ? new ObjectParameter("employeeCardId", employeeCardId)
                    : new ObjectParameter("employeeCardId", typeof(string));

                var companyIdParam = new ObjectParameter("companyId", companyId);

                var branchIdParam = new ObjectParameter("branchId", branchId);

                var branchUnitIdParam = new ObjectParameter("branchUnitId", branchUnitId);

                var branchUnitDepartmentIdParam = (branchUnitDepartmentId != null && branchUnitDepartmentId > 0)
                    ? new ObjectParameter("branchUnitDepartmentId", branchUnitDepartmentId)
                    : new ObjectParameter("branchUnitDepartmentId", typeof(int));

                var sectionIdParam = (sectionId != null && sectionId > 0)
                    ? new ObjectParameter("sectionId", sectionId)
                    : new ObjectParameter("sectionId", typeof(int));

                var lineIdParam = (lineId != null && lineId > 0)
                    ? new ObjectParameter("lineId", lineId)
                    : new ObjectParameter("lineId", typeof(int));

                var fromDateParam = new ObjectParameter("FromDate", fromDate);

                var toDateParam = new ObjectParameter("Todate", toDate);


                var employeeTypeParam = new ObjectParameter("employeeTypeId", employeeTypeId);


                var spAdvanceSalarySheetInfo = Context.SPGetAdvanceSalarySheet(employeeCardIdParam, companyIdParam,
                    branchIdParam,
                    branchUnitIdParam, branchUnitDepartmentIdParam, sectionIdParam,
                    lineIdParam, fromDateParam, toDateParam, employeeTypeParam);


                foreach (var advanceSalarySheetInfo in spAdvanceSalarySheetInfo)
                {
                    var salarySheetModel = new AdvanceSalarySheetView
                    {
                        EmployeeId = advanceSalarySheetInfo.EmployeeId,
                        EmployeeCardId = advanceSalarySheetInfo.EmployeeCardId,
                        CompanyName = advanceSalarySheetInfo.CompanyName,
                        CompanyAddress = advanceSalarySheetInfo.CompanyAddress,
                        Branch = advanceSalarySheetInfo.Branch,
                        Unit = advanceSalarySheetInfo.Unit,
                        Department = advanceSalarySheetInfo.Department,
                        Section = advanceSalarySheetInfo.Section,
                        Line = advanceSalarySheetInfo.Line,
                        Name = advanceSalarySheetInfo.Name,
                        EmployeeType = advanceSalarySheetInfo.EmployeeType,
                        Designation = advanceSalarySheetInfo.Designation,
                        Grade = advanceSalarySheetInfo.Grade,
                        JoiningDate = advanceSalarySheetInfo.JoiningDate,
                        BasicSalary = advanceSalarySheetInfo.BasicSalary,
                        GrossSalary = advanceSalarySheetInfo.GrossSalary,
                        Amount = advanceSalarySheetInfo.Amount,
                        ReceivedDate = advanceSalarySheetInfo.ReceivedDate,
                        FromDate = advanceSalarySheetInfo.FromDate,
                        ToDate = advanceSalarySheetInfo.ToDate,
                        MonthYear = advanceSalarySheetInfo.MonthYear
                    };

                    listSalarySheetModel.Add(salarySheetModel);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return listSalarySheetModel;
        }

        public List<EmployeeBonusSheetView> GetEmployeeBonusSheetInfo(string employeeCardId, int companyId, int branchId,
            int branchUnitId, int? branchUnitDepartmentId, int? sectionId, int? lineId, int employeeTypeId,
            DateTime effectiveDate)
        {

            var listBonusSheetModel = new List<EmployeeBonusSheetView>();

            try
            {
                var employeeCardIdParam = !String.IsNullOrEmpty(employeeCardId)
                    ? new ObjectParameter("employeeCardId", employeeCardId)
                    : new ObjectParameter("employeeCardId", typeof(string));

                var companyIdParam = new ObjectParameter("companyId", companyId);

                var branchIdParam = new ObjectParameter("branchId", branchId);

                var branchUnitIdParam = new ObjectParameter("branchUnitId", branchUnitId);

                var branchUnitDepartmentIdParam = (branchUnitDepartmentId != null && branchUnitDepartmentId > 0)
                    ? new ObjectParameter("branchUnitDepartmentId", branchUnitDepartmentId)
                    : new ObjectParameter("branchUnitDepartmentId", typeof(int));

                var sectionIdParam = (sectionId != null && sectionId > 0)
                    ? new ObjectParameter("sectionId", sectionId)
                    : new ObjectParameter("sectionId", typeof(int));

                var lineIdParam = (lineId != null && lineId > 0)
                    ? new ObjectParameter("lineId", lineId)
                    : new ObjectParameter("lineId", typeof(int));

                var employeeTypeParam = new ObjectParameter("employeeTypeId", employeeTypeId);

                var effectiveDateParam = new ObjectParameter("effectiveDate", effectiveDate);



                var spEmployeeBonusSheetInfo = Context.SPGetEmployeeBonusSheet(employeeCardIdParam, companyIdParam,
                    branchIdParam,
                    branchUnitIdParam, branchUnitDepartmentIdParam, sectionIdParam,
                    lineIdParam, employeeTypeParam, effectiveDateParam);


                foreach (var employeeBonusSheetInfo in spEmployeeBonusSheetInfo)
                {
                    var salarySheetModel = new EmployeeBonusSheetView
                    {
                        EmployeeId = employeeBonusSheetInfo.EmployeeId,
                        EmployeeCardId = employeeBonusSheetInfo.EmployeeCardId,
                        CompanyName = employeeBonusSheetInfo.CompanyName,
                        CompanyAddress = employeeBonusSheetInfo.CompanyAddress,
                        Branch = employeeBonusSheetInfo.Branch,
                        Unit = employeeBonusSheetInfo.Unit,
                        Department = employeeBonusSheetInfo.Department,
                        Section = employeeBonusSheetInfo.Section,
                        Line = employeeBonusSheetInfo.Line,
                        Name = employeeBonusSheetInfo.Name,
                        EmployeeType = employeeBonusSheetInfo.EmployeeType,
                        Designation = employeeBonusSheetInfo.Designation,
                        Grade = employeeBonusSheetInfo.Grade,
                        JoiningDate = employeeBonusSheetInfo.JoiningDate,
                        BonusDate = employeeBonusSheetInfo.BonusDate,
                        //ServiceLength = employeeBonusSheetInfo.ServiceLength, 
                        ServiceLength =
                            DateDifference(employeeBonusSheetInfo.BonusDate, employeeBonusSheetInfo.JoiningDate),
                        BasicSalary = employeeBonusSheetInfo.BasicSalary,
                        GrossSalary = employeeBonusSheetInfo.GrossSalary,
                        BonusAmount = employeeBonusSheetInfo.BonusAmount
                    };

                    listBonusSheetModel.Add(salarySheetModel);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return listBonusSheetModel;
        }

        public int? DateDifference(string bonusDate, string joiningDate)
        {

            DateTime bonus = Convert.ToDateTime(bonusDate);
            DateTime join = Convert.ToDateTime(joiningDate);


            if (join == Convert.ToDateTime("2017-03-01"))
            {
                return 6;
            }

            if (join == Convert.ToDateTime("2016-09-01"))
            {
                return 12;
            }

            int? diff = 0;
            int? yeardiff = bonus.Year - join.Year;

            if (yeardiff > 0)
                diff = yeardiff * 12;

            diff = diff + bonus.Month - join.Month;

            if (bonus.Day < join.Day - 1)
                diff = diff - 1;

            return diff;
        }

        public List<ExtraOTSheetView> GetExtraOTSheetInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {
            try
            {
                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter
                {
                    ParameterName = "BranchUnitDepartmentId",
                    Value = branchUnitDepartmentId
                };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentSectionId",
                    Value = departmentSectionId
                };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentLineId",
                    Value = departmentLineId
                };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter("Year", year);

                var monthParam = new SqlParameter("Month", month);

                var fromDateParam = new SqlParameter("FromDate", fromDate);

                var toDateParam = new SqlParameter("Todate", toDate);

                var employeeCategoryIdParam = new SqlParameter
                {
                    ParameterName = "EmployeeCategoryId",
                    Value = employeeCategoryId
                };

                var userNameParam = new SqlParameter
                {
                    ParameterName = "UserName",
                    Value = PortalContext.CurrentUser.Name
                };

                return Context.Database.SqlQuery<ExtraOTSheetView>(
                    "SPGetExtraOTSheet @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                    "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeCardId, @Year, @Month, " +
                    "@FromDate, @ToDate, @EmployeeCategoryId, @UserName", companyIdParam, branchIdParam,
                    branchUnitIdParam,
                    branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                    employeeTypeIdParam,
                    employeeCardIdParam, yearParam, monthParam, fromDateParam,
                    toDateParam, employeeCategoryIdParam, userNameParam).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<ExtraOTSheetView> GetExtraOTSheetModelInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {
            try
            {
                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter
                {
                    ParameterName = "BranchUnitDepartmentId",
                    Value = branchUnitDepartmentId
                };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentSectionId",
                    Value = departmentSectionId
                };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentLineId",
                    Value = departmentLineId
                };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter("Year", year);

                var monthParam = new SqlParameter("Month", month);

                var fromDateParam = new SqlParameter("FromDate", fromDate);

                var toDateParam = new SqlParameter("Todate", toDate);

                var employeeCategoryIdParam = new SqlParameter
                {
                    ParameterName = "EmployeeCategoryId",
                    Value = employeeCategoryId
                };

                var userNameParam = new SqlParameter
                {
                    ParameterName = "UserName",
                    Value = PortalContext.CurrentUser.Name
                };

                return Context.Database.SqlQuery<ExtraOTSheetView>(
                    "SPGetExtraOTSheetModel @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                    "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeCardId, @Year, @Month, " +
                    "@FromDate, @ToDate, @EmployeeCategoryId, @UserName", companyIdParam, branchIdParam,
                    branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                    employeeTypeIdParam, employeeCardIdParam, yearParam, monthParam, fromDateParam, toDateParam, employeeCategoryIdParam, userNameParam).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<ExtraOTSheetView> GetExtraOTSheet10PMNoWeekendInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {
            try
            {
                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter
                {
                    ParameterName = "BranchUnitDepartmentId",
                    Value = branchUnitDepartmentId
                };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentSectionId",
                    Value = departmentSectionId
                };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentLineId",
                    Value = departmentLineId
                };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter("Year", year);

                var monthParam = new SqlParameter("Month", month);

                var fromDateParam = new SqlParameter("FromDate", fromDate);

                var toDateParam = new SqlParameter("Todate", toDate);

                var employeeCategoryIdParam = new SqlParameter
                {
                    ParameterName = "EmployeeCategoryId",
                    Value = employeeCategoryId
                };

                var userNameParam = new SqlParameter
                {
                    ParameterName = "UserName",
                    Value = PortalContext.CurrentUser.Name
                };

                return Context.Database.SqlQuery<ExtraOTSheetView>(
                    "SPGetExtraOTSheet_10PM_NoWeekend @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                    "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeCardId, @Year, @Month, " +
                    "@FromDate, @ToDate, @EmployeeCategoryId, @UserName", companyIdParam, branchIdParam,
                    branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                    employeeTypeIdParam, employeeCardIdParam, yearParam, monthParam, fromDateParam, toDateParam, employeeCategoryIdParam, userNameParam).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<ExtraOTSheetView> GetExtraOTSheetAfter10PMWithHolidayInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month, DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {
            try
            {
                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter
                {
                    ParameterName = "BranchUnitDepartmentId",
                    Value = branchUnitDepartmentId
                };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentSectionId",
                    Value = departmentSectionId
                };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentLineId",
                    Value = departmentLineId
                };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter("Year", year);

                var monthParam = new SqlParameter("Month", month);

                var fromDateParam = new SqlParameter("FromDate", fromDate);

                var toDateParam = new SqlParameter("Todate", toDate);

                var employeeCategoryIdParam = new SqlParameter
                {
                    ParameterName = "EmployeeCategoryId",
                    Value = employeeCategoryId
                };

                var userNameParam = new SqlParameter
                {
                    ParameterName = "UserName",
                    Value = PortalContext.CurrentUser.Name
                };

                return Context.Database.SqlQuery<ExtraOTSheetView>(
                    "SPGetExtraOTSheet_10PM_AfterWithHoliday @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                    "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeCardId, @Year, @Month, " +
                    "@FromDate, @ToDate, @EmployeeCategoryId, @UserName", companyIdParam, branchIdParam,
                    branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                    employeeTypeIdParam, employeeCardIdParam, yearParam, monthParam, fromDateParam, toDateParam, employeeCategoryIdParam, userNameParam).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<WeekendOTSheetView> GetWeekendOTSheetInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month,
            DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {

            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter
                {
                    ParameterName = "BranchUnitDepartmentId",
                    Value = branchUnitDepartmentId
                };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentSectionId",
                    Value = departmentSectionId
                };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentLineId",
                    Value = departmentLineId
                };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter("Year", year);

                var monthParam = new SqlParameter("Month", month);

                var fromDateParam = new SqlParameter("FromDate", fromDate);

                var toDateParam = new SqlParameter("Todate", toDate);


                var employeeCategoryIdParam = new SqlParameter
                {
                    ParameterName = "EmployeeCategoryId",
                    Value = employeeCategoryId
                };


                var userNameParam = new SqlParameter
                {
                    ParameterName = "UserName",
                    Value = PortalContext.CurrentUser.Name
                };


                return
                    Context.Database.SqlQuery<WeekendOTSheetView>(
                        "SPGetWeekendOTSheet @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                        "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeCardId, @Year, @Month, " +
                        "@FromDate, @ToDate, @EmployeeCategoryId, @UserName", companyIdParam, branchIdParam,
                        branchUnitIdParam,
                        branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                        employeeTypeIdParam,
                        employeeCardIdParam, yearParam, monthParam, fromDateParam,
                        toDateParam, employeeCategoryIdParam, userNameParam).ToList();



            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

        }

        public List<HolidayOTSheetView> GetHolidayOTSheetInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month,
            DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {

            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter
                {
                    ParameterName = "BranchUnitDepartmentId",
                    Value = branchUnitDepartmentId
                };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentSectionId",
                    Value = departmentSectionId
                };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentLineId",
                    Value = departmentLineId
                };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter("Year", year);

                var monthParam = new SqlParameter("Month", month);

                var fromDateParam = new SqlParameter("FromDate", fromDate);

                var toDateParam = new SqlParameter("Todate", toDate);


                var employeeCategoryIdParam = new SqlParameter
                {
                    ParameterName = "EmployeeCategoryId",
                    Value = employeeCategoryId
                };


                var userNameParam = new SqlParameter
                {
                    ParameterName = "UserName",
                    Value = PortalContext.CurrentUser.Name
                };


                return
                    Context.Database.SqlQuery<HolidayOTSheetView>(
                        "spPayrollGetHolidayOTSheetInfo @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                        "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeCardId, @Year, @Month, " +
                        "@FromDate, @ToDate, @EmployeeCategoryId, @UserName", companyIdParam, branchIdParam,
                        branchUnitIdParam,
                        branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                        employeeTypeIdParam,
                        employeeCardIdParam, yearParam, monthParam, fromDateParam,
                        toDateParam, employeeCategoryIdParam, userNameParam).ToList();



            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

        }

        public List<ExtraOTWeekendOTAndHolidayOTSheetView> GetExtraOTWeekendOTAndHolidayOTSheetInfo(int? companyId,
            int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month,
            DateTime? fromDate, DateTime? toDate, int employeeCategoryId)
        {

            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter
                {
                    ParameterName = "BranchUnitDepartmentId",
                    Value = branchUnitDepartmentId
                };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentSectionId",
                    Value = departmentSectionId
                };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentLineId",
                    Value = departmentLineId
                };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter("Year", year);

                var monthParam = new SqlParameter("Month", month);

                var fromDateParam = new SqlParameter("FromDate", fromDate);

                var toDateParam = new SqlParameter("Todate", toDate);


                var employeeCategoryIdParam = new SqlParameter
                {
                    ParameterName = "EmployeeCategoryId",
                    Value = employeeCategoryId
                };


                var userNameParam = new SqlParameter
                {
                    ParameterName = "UserName",
                    Value = PortalContext.CurrentUser.Name
                };


                return
                    Context.Database.SqlQuery<ExtraOTWeekendOTAndHolidayOTSheetView>(
                        "spPayrollGetExtraOTWeekendOTAndHolidayOTSheet @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                        "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeCardId, @Year, @Month, " +
                        "@FromDate, @ToDate, @EmployeeCategoryId, @UserName", companyIdParam, branchIdParam,
                        branchUnitIdParam,
                        branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                        employeeTypeIdParam,
                        employeeCardIdParam, yearParam, monthParam, fromDateParam,
                        toDateParam, employeeCategoryIdParam, userNameParam).ToList();



            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

        }

        public List<EmployeeSalaryInfoModel> GetEmployeeSalaryReport(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId,
            int? employeeGradeId, int? employeeDesignationId, int? genderId, DateTime? joiningDateBegin,
            DateTime? joiningDateEnd,
            DateTime? quitDateBegin, DateTime? quitDateEnd, string employeeCardId, string employeeName,
            int? activeStatus, string userName, DateTime? upToDate)
        {


            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter
                {
                    ParameterName = "BranchUnitDepartmentId",
                    Value = branchUnitDepartmentId
                };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentSectionId",
                    Value = departmentSectionId
                };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter
                {
                    ParameterName = "DepartmentLineId",
                    Value = departmentLineId
                };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (employeeGradeId == 0)
                    employeeGradeId = -1;
                var employeeGradeIdParam = new SqlParameter { ParameterName = "EmployeeGradeId", Value = employeeGradeId };

                if (employeeDesignationId == 0)
                    employeeDesignationId = -1;
                var employeeDesignationIdParam = new SqlParameter
                {
                    ParameterName = "EmployeeDesignationId",
                    Value = employeeDesignationId
                };


                if (genderId == 0)
                    genderId = -1;
                var genderIdParam = new SqlParameter { ParameterName = "GenderId", Value = genderId };


                if (joiningDateBegin == null)
                    joiningDateBegin = new DateTime(1900, 01, 01);
                var joiningDateBeginParam = new SqlParameter
                {
                    ParameterName = "JoiningDateBegin",
                    Value = joiningDateBegin
                };


                if (joiningDateEnd == null)
                    joiningDateEnd = new DateTime(1900, 01, 01);
                var joiningDateEndParam = new SqlParameter { ParameterName = "JoiningDateEnd", Value = joiningDateEnd };


                if (quitDateBegin == null)
                    quitDateBegin = new DateTime(1900, 01, 01);
                var quitDateBeginParam = new SqlParameter { ParameterName = "QuitDateBegin", Value = quitDateBegin };


                if (quitDateEnd == null)
                    quitDateEnd = new DateTime(1900, 01, 01);
                var quitDateEndParam = new SqlParameter { ParameterName = "QuitDateEnd", Value = quitDateEnd };


                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                if (String.IsNullOrEmpty(employeeName))
                    employeeName = string.Empty;
                var employeeNameParam = new SqlParameter { ParameterName = "EmployeeName", Value = employeeName };


                if (activeStatus == 0)
                    activeStatus = -1;
                var activeStatusParam = new SqlParameter { ParameterName = "ActiveStatus", Value = activeStatus };

                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };

                if (upToDate == null)
                    upToDate = new DateTime(1900, 01, 01);
                var upToDateParam = new SqlParameter { ParameterName = "UpToDate", Value = upToDate };

                return
                    Context.Database.SqlQuery<EmployeeSalaryInfoModel>(
                        "SPGetEmployeeSalaryReport @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                        "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeGradeId, @EmployeeDesignationId, " +
                        "@GenderId, @JoiningDateBegin, @JoiningDateEnd, " +
                        "@QuitDateBegin, @QuitDateEnd," +
                        "@EmployeeCardId, @EmployeeName, @ActiveStatus, @UserName, " +
                        "@UpToDate ", companyIdParam, branchIdParam, branchUnitIdParam,
                        branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                        employeeTypeIdParam,
                        employeeGradeIdParam, employeeDesignationIdParam, genderIdParam,
                        joiningDateBeginParam, joiningDateEndParam, quitDateBeginParam,
                        quitDateEndParam, employeeCardIdParam, employeeNameParam,
                        activeStatusParam, userNameParam, upToDateParam).ToList();


            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<SPSalaryIncrementReport> GetSalaryIncrementReport(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, DateTime? fromDate, DateTime? toDate, string employeeCardId, float? incrementPercent, decimal? incrementAmount, string userName)
        {
            if (companyId == 0)
                companyId = -1;
            var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

            if (branchId == 0)
                branchId = -1;
            var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

            if (branchUnitId == 0)
                branchUnitId = -1;
            var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

            if (branchUnitDepartmentId == 0)
                branchUnitDepartmentId = -1;
            var branchUnitDepartmentIdParam = new SqlParameter
            {
                ParameterName = "BranchUnitDepartmentId",
                Value = branchUnitDepartmentId
            };

            if (departmentSectionId == 0)
                departmentSectionId = -1;
            var departmentSectionIdParam = new SqlParameter
            {
                ParameterName = "DepartmentSectionId",
                Value = departmentSectionId
            };

            if (departmentLineId == 0)
                departmentLineId = -1;
            var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

            if (employeeTypeId == 0)
                employeeTypeId = -1;
            var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

            if (fromDate == null)
                fromDate = new DateTime(1900, 01, 01);
            var fromDateParam = new SqlParameter { ParameterName = "fromDate", Value = fromDate };

            if (toDate == null)
                toDate = new DateTime(1900, 01, 01);
            var toDateParam = new SqlParameter { ParameterName = "toDate", Value = toDate };

            if (String.IsNullOrEmpty(employeeCardId))
                employeeCardId = string.Empty;
            var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

            var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };

            if (incrementPercent == 0)
                incrementPercent = 5;
            var incrementPercentParam = new SqlParameter { ParameterName = "IncrementPercent", Value = incrementPercent };

            if (incrementAmount == 0)
                incrementAmount = 5;
            var incrementAmountParam = new SqlParameter { ParameterName = "OtherIncrement", Value = incrementAmount };

            return Context.Database.SqlQuery<SPSalaryIncrementReport>("SPSalaryIncrementReport @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, @DepartmentSectionId, @DepartmentLineId, @FromDate, @ToDate, @EmployeeTypeId, @EmployeeCardId, @UserName, @IncrementPercent, @OtherIncrement"
                , companyIdParam, branchIdParam, branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam, fromDateParam, toDateParam, employeeTypeIdParam, employeeCardIdParam, userNameParam, incrementPercentParam, incrementAmountParam).ToList();
        }

        public List<SalarySummaryView> GetEmployeeSalarySummary(int? companyId, int? branchId,
            List<int> branchUnitIdList,
            List<int> employeeTypeIdList, int employeeCategoryId, int year,
            int month, DateTime? fromDate, DateTime? toDate)
        {
            var dataTableBranchUnitId = new DataTable();
            dataTableBranchUnitId.Columns.Add("BranchUnitId", typeof(int));
            foreach (var branchUnitId in branchUnitIdList)
            {
                var dataRow = dataTableBranchUnitId.NewRow();
                dataRow[0] = branchUnitId;
                dataTableBranchUnitId.Rows.Add(dataRow);
            }

            var dataTableEmployeeTypeId = new DataTable();
            dataTableEmployeeTypeId.Columns.Add("EmployeeTypeId", typeof(int));
            foreach (var employeeTypeId in employeeTypeIdList)
            {
                var dataRow = dataTableEmployeeTypeId.NewRow();
                dataRow[0] = employeeTypeId;
                dataTableEmployeeTypeId.Rows.Add(dataRow);
            }



            var connectionString = Context.Database.Connection.ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            List<SalarySummaryView> salarySummaryViews = new List<SalarySummaryView>();

            using (conn)
            {
                SqlCommand cmd = new SqlCommand("dbo.spPayrollGetEmployeeSalarySummaryGross", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter companyIdParam = cmd.Parameters.AddWithValue("@CompanyId", companyId);
                companyIdParam.SqlDbType = SqlDbType.Int;

                SqlParameter branchIdParam = cmd.Parameters.AddWithValue("@BranchId", branchId);
                branchIdParam.SqlDbType = SqlDbType.Int;

                SqlParameter branchUnitIdListParam = cmd.Parameters.AddWithValue("@BranchUnitIdList",
                    dataTableBranchUnitId);
                branchUnitIdListParam.SqlDbType = SqlDbType.Structured;

                SqlParameter employeeTypeListParam = cmd.Parameters.AddWithValue("@EmployeeTypeIdList",
                    dataTableEmployeeTypeId);
                employeeTypeListParam.SqlDbType = SqlDbType.Structured;

                SqlParameter employeeCategoryIdParam = cmd.Parameters.AddWithValue("@EmployeeCategoryId",
                    employeeCategoryId);
                employeeCategoryIdParam.SqlDbType = SqlDbType.Int;

                SqlParameter yearParam = cmd.Parameters.AddWithValue("@Year", year);
                yearParam.SqlDbType = SqlDbType.Int;

                SqlParameter monthParam = cmd.Parameters.AddWithValue("@Month", month);
                monthParam.SqlDbType = SqlDbType.Int;

                SqlParameter fromDateParam = cmd.Parameters.AddWithValue("@FromDate", fromDate);
                fromDateParam.SqlDbType = SqlDbType.DateTime;

                SqlParameter toDateParam = cmd.Parameters.AddWithValue("@ToDate", toDate);
                toDateParam.SqlDbType = SqlDbType.DateTime;

                string userName = PortalContext.CurrentUser.Name;
                SqlParameter userNameParam = cmd.Parameters.AddWithValue("@UserName", userName);
                userNameParam.SqlDbType = SqlDbType.NVarChar;

                conn.Open();
                cmd.CommandTimeout = 36000;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);



                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SalarySummaryView salarySummaryView = new SalarySummaryView();

                    salarySummaryView.CompanyName = Convert.ToString(ds.Tables[0].Rows[i]["CompanyName"]);
                    salarySummaryView.CompanyAddress = Convert.ToString(ds.Tables[0].Rows[i]["CompanyAddress"]);
                    salarySummaryView.BranchName = Convert.ToString(ds.Tables[0].Rows[i]["BranchName"]);
                    salarySummaryView.Unit = Convert.ToString(ds.Tables[0].Rows[i]["Unit"]);
                    salarySummaryView.Department = Convert.ToString(ds.Tables[0].Rows[i]["Department"]);
                    salarySummaryView.Section = Convert.ToString(ds.Tables[0].Rows[i]["Section"]);
                    salarySummaryView.Line = Convert.ToString(ds.Tables[0].Rows[i]["Line"]);
                    salarySummaryView.EmployeeType = Convert.ToString(ds.Tables[0].Rows[i]["EmployeeType"]);
                    salarySummaryView.NetAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["NetAmount"]);
                    salarySummaryView.TotalEmployee = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalEmployee"]);
                    salarySummaryView.ExtraOTWeekendOTandHolidayOT =
                        Convert.ToDecimal(ds.Tables[0].Rows[i]["ExtraOTWeekendOTandHolidayOT"]);
                    salarySummaryView.TotalOTHours = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalOTHours"]);
                    salarySummaryView.TotalOTAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalOTAmount"]);
                    salarySummaryView.TotalExtraOTHours = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalExtraOTHours"]);
                    salarySummaryView.TotalExtraOTAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalExtraOTAmount"]);
                    salarySummaryView.TotalWeekendOTHours =
                        Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalWeekendOTHours"]);
                    salarySummaryView.TotalWeekendOTAmount =
                        Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalWeekendOTAmount"]);
                    salarySummaryView.TotalHolidayOTHours =
                        Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalHolidayOTHours"]);
                    salarySummaryView.TotalHolidayOTAmount =
                        Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalHolidayOTAmount"]);
                    salarySummaryView.TotalAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalAmount"]);

                    salarySummaryViews.Add(salarySummaryView);
                }

                conn.Close();
            }

            return salarySummaryViews;
        }

        public List<SalarySummaryView> GetEmployeeSalarySummaryAll(int? companyId, int? branchId,
            List<int> branchUnitIdList,
            List<int> employeeTypeIdList, int year, int month, DateTime? fromDate, DateTime? toDate)
        {
            var dataTableBranchUnitId = new DataTable();
            dataTableBranchUnitId.Columns.Add("BranchUnitId", typeof(int));
            foreach (var branchUnitId in branchUnitIdList)
            {
                var dataRow = dataTableBranchUnitId.NewRow();
                dataRow[0] = branchUnitId;
                dataTableBranchUnitId.Rows.Add(dataRow);
            }

            var dataTableEmployeeTypeId = new DataTable();
            dataTableEmployeeTypeId.Columns.Add("EmployeeTypeId", typeof(int));
            foreach (var employeeTypeId in employeeTypeIdList)
            {
                var dataRow = dataTableEmployeeTypeId.NewRow();
                dataRow[0] = employeeTypeId;
                dataTableEmployeeTypeId.Rows.Add(dataRow);
            }



            var connectionString = Context.Database.Connection.ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            List<SalarySummaryView> salarySummaryViews = new List<SalarySummaryView>();

            using (conn)
            {
                SqlCommand cmd = new SqlCommand("dbo.spPayrollGetEmployeeSalarySummaryAllGross", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter companyIdParam = cmd.Parameters.AddWithValue("@CompanyId", companyId);
                companyIdParam.SqlDbType = SqlDbType.Int;

                SqlParameter branchIdParam = cmd.Parameters.AddWithValue("@BranchId", branchId);
                branchIdParam.SqlDbType = SqlDbType.Int;

                SqlParameter branchUnitIdListParam = cmd.Parameters.AddWithValue("@BranchUnitIdList",
                    dataTableBranchUnitId);
                branchUnitIdListParam.SqlDbType = SqlDbType.Structured;

                SqlParameter employeeTypeListParam = cmd.Parameters.AddWithValue("@EmployeeTypeIdList",
                    dataTableEmployeeTypeId);
                employeeTypeListParam.SqlDbType = SqlDbType.Structured;

                SqlParameter yearParam = cmd.Parameters.AddWithValue("@Year", year);
                yearParam.SqlDbType = SqlDbType.Int;

                SqlParameter monthParam = cmd.Parameters.AddWithValue("@Month", month);
                monthParam.SqlDbType = SqlDbType.Int;

                SqlParameter fromDateParam = cmd.Parameters.AddWithValue("@FromDate", fromDate);
                fromDateParam.SqlDbType = SqlDbType.DateTime;

                SqlParameter toDateParam = cmd.Parameters.AddWithValue("@ToDate", toDate);
                toDateParam.SqlDbType = SqlDbType.DateTime;

                string userName = PortalContext.CurrentUser.Name;
                SqlParameter userNameParam = cmd.Parameters.AddWithValue("@UserName", userName);
                userNameParam.SqlDbType = SqlDbType.NVarChar;

                conn.Open();
                cmd.CommandTimeout = 36000;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);



                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SalarySummaryView salarySummaryView = new SalarySummaryView();

                    salarySummaryView.EmployeeCategory = Convert.ToString(ds.Tables[0].Rows[i]["EmployeeCategory"]);
                    salarySummaryView.NetAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["NetAmount"]);
                    salarySummaryView.TotalEmployee = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalEmployee"]);
                    salarySummaryView.ExtraOTWeekendOTandHolidayOT =
                        Convert.ToDecimal(ds.Tables[0].Rows[i]["ExtraOTWeekendOTandHolidayOT"]);
                    salarySummaryView.TotalOTHours = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalOTHours"]);
                    salarySummaryView.TotalOTAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalOTAmount"]);
                    salarySummaryView.TotalExtraOTHours = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalExtraOTHours"]);
                    salarySummaryView.TotalExtraOTAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalExtraOTAmount"]);
                    salarySummaryView.TotalWeekendOTHours =
                        Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalWeekendOTHours"]);
                    salarySummaryView.TotalWeekendOTAmount =
                        Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalWeekendOTAmount"]);
                    salarySummaryView.TotalHolidayOTHours =
                        Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalHolidayOTHours"]);
                    salarySummaryView.TotalHolidayOTAmount =
                        Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalHolidayOTAmount"]);
                    salarySummaryView.TotalAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalAmount"]);

                    salarySummaryViews.Add(salarySummaryView);
                }

                conn.Close();
            }

            return salarySummaryViews;
        }

        public int GetWeekendDays(DateTime fromDate, DateTime toDate)
        {
            double TotalDays = Math.Ceiling((toDate - fromDate).TotalDays);

            var WeekDays = Context.Weekends.Where(p => p.IsActive == true);

            int count = 0;

            for (int i = 1; i <= TotalDays; i++)
            {
                fromDate = fromDate.AddDays(1);

                foreach (var t in WeekDays)
                {
                    string day = fromDate.ToString("ddd");
                    if (fromDate.ToString("ddd").ToLower() == t.DayName.ToLower())
                        count++;
                }
            }
            return count;
        }

        public List<WeekendBillModel> GetWeekendBill(DateTime fromDate)
        {
            List<WeekendBillModel> weekendBill =
                Context.Database.SqlQuery<WeekendBillModel>("SPGetWeekendBill @FromDate",
                    new SqlParameter("FromDate", fromDate)).ToList();
            return weekendBill.ToList();
        }

        public int ProcessWeekendBill(DateTime fromDate)
        {
            string userName = PortalContext.CurrentUser.Name;

            List<WeekendBillModel> weekendBill =
                Context.Database.SqlQuery<WeekendBillModel>("SPProcessWeekendBill @FromDate, @ToDate, @UserName",
                    new SqlParameter("FromDate", fromDate), new SqlParameter("ToDate", fromDate),
                    new SqlParameter("UserName", userName)).ToList();
            return weekendBill.Count;
        }

        public string GetCompanyNameByCompanyId(string companyId)
        {
            var company = Context.Companies.FirstOrDefault(p => p.CompanyRefId == companyId && p.IsActive == true);

            if (company != null)
                return company.Name;
            else
                return "Not Found !";
        }

        public Company GetCompanyByCompanyId(string companyId)
        {
            Company company = new Company();
            company = Context.Companies.FirstOrDefault(p => p.CompanyRefId == companyId && p.IsActive == true);

            if (company != null)
                return company;
            else
                return null;
        }

        public string CategoryNameById(int categoryId)
        {
            var category = Context.EmployeeCategories.FirstOrDefault(p => p.Id == categoryId && p.IsActive == true);

            if (category != null)
                return category.Title;
            else
                return "not found !";
        }

        public DataTable GetSalaryBankStatement(DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate == null)
                fromDate = new DateTime(1900, 01, 01);

            if (toDate == null)
                toDate = new DateTime(1900, 01, 01);

            SqlConnection connection = (SqlConnection)Context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPGetSalaryBankStatement"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@toDate", SqlDbType.DateTime).Value = toDate;
                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable SalarySummaryMultipleMonth(int? companyId, int? branchId, int? fromYear, int? toYear, int? fromMonth, int? toMonth, DateTime? fromDate, DateTime? toDate)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpEmployeeSalarySummaryMultipleMonth"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchId", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@FromYear", SqlDbType.Int).Value = fromYear;
                cmd.Parameters.Add("@ToYear", SqlDbType.Int).Value = toYear;

                cmd.Parameters.Add("@FromMonth", SqlDbType.Int).Value = fromMonth;
                cmd.Parameters.Add("@ToMonth", SqlDbType.Int).Value = toMonth;

                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = "superadmin";

                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable SalarySummaryTopSheet(int? companyId, int? branchId, int? year, int? month, DateTime? fromDate, DateTime? toDate)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPGetSalarySummaryTopSheet"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchId", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@Year", SqlDbType.Int).Value = year;
                cmd.Parameters.Add("@Month", SqlDbType.Int).Value = month;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;

                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable SalaryXL(int? companyId, int? branchId, int? year, int? month, DateTime? fromDate, DateTime? toDate)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPGetSalaryXL"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchId", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@Year", SqlDbType.Int).Value = year;
                cmd.Parameters.Add("@Month", SqlDbType.Int).Value = month;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;

                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }
    }
}