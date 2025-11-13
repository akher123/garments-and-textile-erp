using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.PayrollRepository
{
    public class ExcludedEmployeeFromSalaryProcessRepository : Repository<PayrollExcludedEmployeeFromSalaryProcess>, IExcludedEmployeeFromSalaryProcessRepository
    {
        public ExcludedEmployeeFromSalaryProcessRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public List<PayrollExcludedEmployeeFromSalaryProcess> GetExcludedEmployeeFromSalaryProcessInfo(int startPage, int pageSize, PayrollExcludedEmployeeFromSalaryProcess model, SearchFieldModel searchFieldModel, out int totalRecords)
        {
            var employeeInfos = new List<PayrollExcludedEmployeeFromSalaryProcess>();

            try
            {

                var companyIdParam = (searchFieldModel.SearchByCompanyId > 0) ?
                    new ObjectParameter("CompanyID", searchFieldModel.SearchByCompanyId) :
                    new ObjectParameter("CompanyID", typeof(int));

                var branchIdParam = (searchFieldModel.SearchByBranchId > 0) ?
                    new ObjectParameter("BranchID", searchFieldModel.SearchByBranchId) :
                    new ObjectParameter("BranchID", typeof(int));

                var branchUnitIdParam = (searchFieldModel.SearchByBranchUnitId > 0) ?
                    new ObjectParameter("BranchUnitID", searchFieldModel.SearchByBranchUnitId) :
                    new ObjectParameter("BranchUnitID", typeof(int));

                var branchUnitDepartmentIdParam = (searchFieldModel.SearchByBranchUnitDepartmentId > 0) ?
                    new ObjectParameter("BranchUnitDepartmentID", searchFieldModel.SearchByBranchUnitDepartmentId) :
                    new ObjectParameter("BranchUnitDepartmentID", typeof(int));

                var departmentSectionIdParam = (searchFieldModel.SearchByDepartmentSectionId > 0) ?
                    new ObjectParameter("DepartmentSectionId", searchFieldModel.SearchByDepartmentSectionId) :
                    new ObjectParameter("DepartmentSectionId", typeof(int));

                var departmentLineIdParam = (searchFieldModel.SearchByDepartmentLineId > 0) ?
                    new ObjectParameter("DepartmentLineId", searchFieldModel.SearchByDepartmentLineId) :
                    new ObjectParameter("DepartmentLineId", typeof(int));

                var employeeTypeIdParam = (searchFieldModel.SearchByEmployeeTypeId > 0) ?
                    new ObjectParameter("EmployeeTypeID", searchFieldModel.SearchByEmployeeTypeId) :
                    new ObjectParameter("EmployeeTypeID", typeof(int));

                var employeeCardIdParam = !String.IsNullOrEmpty(searchFieldModel.SearchByEmployeeCardId) ?
                       new ObjectParameter("EmployeeCardId", searchFieldModel.SearchByEmployeeCardId) :
                       new ObjectParameter("EmployeeCardId", typeof(string));

                var yearParam = (Convert.ToInt32(searchFieldModel.SelectedYear) > 0) ?
                    new ObjectParameter("Year", Convert.ToInt32(searchFieldModel.SelectedYear)) :
                    new ObjectParameter("Year", typeof(int));

                var monthParam = (Convert.ToInt32(searchFieldModel.SelectedMonth) > 0) ?
                   new ObjectParameter("Month", Convert.ToInt32(searchFieldModel.SelectedMonth)) :
                   new ObjectParameter("Month", typeof(int));

                var fromDateParam = (searchFieldModel.StartDate.HasValue) ?
                   new ObjectParameter("FromDate", searchFieldModel.StartDate) :
                   new ObjectParameter("FromDate", typeof(DateTime));

                var toDateParam = (searchFieldModel.EndDate.HasValue) ?
                  new ObjectParameter("ToDate", searchFieldModel.EndDate) :
                  new ObjectParameter("ToDate", typeof(DateTime));


                var userNameParam = !String.IsNullOrEmpty(PortalContext.CurrentUser.Name) ?
                   new ObjectParameter("UserName", PortalContext.CurrentUser.Name) :
                   new ObjectParameter("UserName", typeof(string));

                var startRowIndexParam = (startPage >= 0) ?
                     new ObjectParameter("StartRowIndex", startPage) :
                     new ObjectParameter("StartRowIndex", typeof(int));

                var maxRowsParam = (pageSize > 0) ?
                    new ObjectParameter("MaxRows", pageSize) :
                    new ObjectParameter("MaxRows", typeof(int));

                var sortFieldParam = new ObjectParameter("SortField", GetSortField(model.sort));

                var sortDirectionParam = new ObjectParameter("SortDiriection", Common.CustomPaging.GetSortDirection(model.sortdir));

                var employees = Context.spPayrollGetExcludedEmployeeFromSalaryProcessInfo(companyIdParam, branchIdParam,
                                                         branchUnitIdParam, branchUnitDepartmentIdParam,
                                                         departmentSectionIdParam, departmentLineIdParam,
                                                         employeeTypeIdParam, employeeCardIdParam, yearParam, monthParam,
                                                         fromDateParam, toDateParam,
                                                         userNameParam, startRowIndexParam, maxRowsParam, sortFieldParam, sortDirectionParam, out totalRecords);


                foreach (var employeeInfoResult in employees)
                {
                    var employeeInfoModel = new ExludedEmployeeFromSalaryProcessInfoCustomModel
                    {
                        ExcludedEmployeeFromSalaryProcessId = employeeInfoResult.ExcludedEmployeeFromSalaryProcessId,
                        EmployeeCardId = employeeInfoResult.EmployeeCardId,
                        EmployeeId = employeeInfoResult.EmployeeId,
                        Name = employeeInfoResult.Name,
                        JoiningDate = employeeInfoResult.JoiningDate,
                        QuitDate = employeeInfoResult.QuitDate,
                        Company = employeeInfoResult.Company,
                        Branch = employeeInfoResult.Branch,
                        Unit = employeeInfoResult.Unit,
                        Department = employeeInfoResult.Department,
                        Section = employeeInfoResult.Section,
                        Line = employeeInfoResult.Line,
                        EmployeeType = employeeInfoResult.EmployeeType,
                        Designation = employeeInfoResult.Designation,
                        Year = employeeInfoResult.Year,
                        Month = employeeInfoResult.Month,
                        FromDate = employeeInfoResult.FromDate,
                        ToDate = employeeInfoResult.ToDate,
                        Remarks = employeeInfoResult.Remarks
                    };
                    employeeInfos.Add(employeeInfoModel);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return employeeInfos;
        }

        public PayrollExcludedEmployeeFromSalaryProcess GetExcludedEmployeeFromSalaryProcessById(int excludedEmployeeFromSalaryProcessId)
        {
            return Context.PayrollExcludedEmployeeFromSalaryProcess.FirstOrDefault(x => x.ExcludedEmployeeFromSalaryProcessId == excludedEmployeeFromSalaryProcessId && x.IsActive);
        }

        private int? GetSortField(string sortExpression)
        {

            switch (sortExpression)
            {
                case "Employee.EmployeeCardId":
                    return 1;
                    break;
                case "Employee.Name":
                    return 2;
                    break;
                case "Employee.Gender.Title":
                    return 3;
                    break;
                case "Company.Name":
                    return 4;
                    break;
                case "Branch.Name":
                    return 5;
                    break;
                case "Unit.Name":
                    return 6;
                    break;
                case "Department.Name":
                    return 7;
                    break;
                case "DepartmentSection.Section.Name":
                    return 8;
                    break;
                case "DepartmentLine.Line.Name":
                    return 9;
                    break;
                case "EmployeeDesignation.Title":
                    return 10;
                    break;
                case "Employee.Status":
                    return 11;
                    break;

                default:
                    return 1;
                    break;

            }
        }

        public int SaveExcludedEmployeeFromSalaryProcess(PayrollExcludedEmployeeFromSalaryProcess payrollExcludedEmployeeFromSalaryProcess)
        {
            return base.Save(payrollExcludedEmployeeFromSalaryProcess);
        }

        public int EditExcludedEmployeeFromSalaryProcess(PayrollExcludedEmployeeFromSalaryProcess payrollExcludedEmployeeFromSalaryProcess)
        {
            return base.Edit(payrollExcludedEmployeeFromSalaryProcess);
        }

        public List<ExludedEmployeeFromSalaryProcessInfoCustomModel> GetEmployeesForExcludingFromSalaryProcess(SearchFieldModel searchFieldModel, ExludedEmployeeFromSalaryProcessInfoCustomModel model)
        {
            try
            {
                int companyId = -1;
                if (searchFieldModel.SearchByCompanyId > 0)
                    companyId = searchFieldModel.SearchByCompanyId;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                int branchId = -1;
                if (searchFieldModel.SearchByBranchId > 0)
                    branchId = searchFieldModel.SearchByBranchId;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                int branchUnitId = -1;
                if (searchFieldModel.SearchByBranchUnitId > 0)
                    branchUnitId = searchFieldModel.SearchByBranchUnitId;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                int branchUnitDepartmentId = -1;
                if (searchFieldModel.SearchByBranchUnitDepartmentId > 0)
                    branchUnitDepartmentId = searchFieldModel.SearchByBranchUnitDepartmentId;
                var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

                int departmentSectionId = -1;
                if (searchFieldModel.SearchByDepartmentSectionId > 0)
                    departmentSectionId = searchFieldModel.SearchByDepartmentSectionId;
                var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

                int departmentLineId = -1;
                if (searchFieldModel.SearchByDepartmentLineId > 0)
                    departmentLineId = searchFieldModel.SearchByDepartmentLineId;
                var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

                int employeeTypeId = -1;
                if (searchFieldModel.SearchByEmployeeTypeId > 0)
                    employeeTypeId = searchFieldModel.SearchByEmployeeTypeId;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                string employeeCardId = string.Empty;
                if (!String.IsNullOrEmpty(searchFieldModel.SearchByEmployeeCardId))
                    employeeCardId = searchFieldModel.SearchByEmployeeCardId;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                DateTime? joiningDateBegin = null;
                if (searchFieldModel.JoiningDateBegin == null)
                    joiningDateBegin = new DateTime(1900, 01, 01);
                else
                {
                    joiningDateBegin = searchFieldModel.JoiningDateBegin;
                }
                var joiningDateBeginParam = new SqlParameter { ParameterName = "JoiningDateBegin", Value = joiningDateBegin };

                DateTime? joiningDateEnd = null;
                if (searchFieldModel.JoiningDateEnd == null)
                    joiningDateEnd = new DateTime(1900, 01, 01);
                else
                {
                    joiningDateEnd = searchFieldModel.JoiningDateEnd;
                }
                var joiningDateEndParam = new SqlParameter { ParameterName = "JoiningDateEnd", Value = joiningDateEnd };

                DateTime? quitDateBegin = null;
                if (searchFieldModel.QuitDateBegin == null)
                    quitDateBegin = new DateTime(1900, 01, 01);
                else
                {
                    quitDateBegin = searchFieldModel.QuitDateBegin; 
                }
                var quitDateBeginParam = new SqlParameter { ParameterName = "QuitDateBegin", Value = quitDateBegin };

                DateTime? quitDateEnd = null;
                if (searchFieldModel.QuitDateEnd == null)
                    quitDateEnd = new DateTime(1900, 01, 01);
                else
                {
                    quitDateEnd = searchFieldModel.QuitDateEnd;
                }
                var quitDateEndParam = new SqlParameter { ParameterName = "QuitDateEnd", Value = quitDateEnd };

                DateTime? fromDate = new DateTime(1900, 01, 01);
                if (searchFieldModel.StartDate != null)
                    fromDate = searchFieldModel.StartDate;
                var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

                DateTime? toDate = new DateTime(1900, 01, 01);
                if (searchFieldModel.EndDate != null)
                    toDate = searchFieldModel.EndDate;
                var toDateParam = new SqlParameter { ParameterName = "ToDate", Value = toDate };

                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = PortalContext.CurrentUser.Name };

                var employeesForSalaryProcess = Context.Database.SqlQuery<ExludedEmployeeFromSalaryProcessInfoCustomModel>("spPayrollGetEmployeesForExcludingEmployeeFromSalaryProcess  " +
                                                                         "@CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                         "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, " +
                                                                         "@EmployeeCardId, @JoiningDateBegin, @JoiningDateEnd, @QuitDateBegin, " +
                                                                         "@QuitDateEnd, @FromDate, @ToDate, @UserName", companyIdParam,
                                                                         branchIdParam, branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam,
                                                                         departmentLineIdParam, employeeTypeIdParam, employeeCardIdParam, joiningDateBeginParam, 
                                                                         joiningDateEndParam, quitDateBeginParam, quitDateEndParam, fromDateParam, toDateParam,
                                                                         userNameParam).ToList();

                return employeesForSalaryProcess;


            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public int ProcessBulkEmployeesForExcludingFromSalaryProcess(SearchFieldModel searchFieldModel, ExludedEmployeeFromSalaryProcessInfoCustomModel model)
        {

            var processedResult = 0;

            var dataTableEmployeeCard = new DataTable();
            dataTableEmployeeCard.Columns.Add("EmployeeId", typeof(Guid));
            foreach (var employeeCardId in searchFieldModel.EmployeeIdList)
            {
                var dataRow = dataTableEmployeeCard.NewRow();
                dataRow[0] = employeeCardId;
                dataTableEmployeeCard.Rows.Add(dataRow);
            }

            try
            {
                var connectionString = Context.Database.Connection.ConnectionString;
                SqlConnection conn = new SqlConnection(connectionString);

                using (conn)
                {
                    SqlCommand cmd = new SqlCommand("dbo.spPayrollProcessBulkEmployeesForExcludingFromSalaryProcess", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter employeeIdCardListParam = cmd.Parameters.AddWithValue("@EmployeeList", dataTableEmployeeCard);
                    employeeIdCardListParam.SqlDbType = SqlDbType.Structured;


                    int year = 1900;
                    if (Convert.ToInt32(searchFieldModel.SelectedYear) > 0)
                        year = Convert.ToInt32(searchFieldModel.SelectedYear);
                    SqlParameter yearParam = cmd.Parameters.AddWithValue("@Year", year);
                    yearParam.SqlDbType = SqlDbType.Int;

                    int month = 1;
                    if (Convert.ToInt32(searchFieldModel.SelectedMonth) > 0)
                        month = Convert.ToInt32(searchFieldModel.SelectedMonth);
                    SqlParameter monthParam = cmd.Parameters.AddWithValue("@Month", month);
                    monthParam.SqlDbType = SqlDbType.Int;

                    DateTime? fromDate = new DateTime(1900, 01, 01);
                    if (searchFieldModel.StartDate != null)
                        fromDate = searchFieldModel.StartDate;
                    SqlParameter fromDateParam = cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    fromDateParam.SqlDbType = SqlDbType.DateTime;

                    DateTime? toDate = new DateTime(1900, 01, 01);
                    if (searchFieldModel.EndDate != null)
                        toDate = searchFieldModel.EndDate;
                    SqlParameter toDateParam = cmd.Parameters.AddWithValue("@ToDate", toDate);
                    toDateParam.SqlDbType = SqlDbType.DateTime;

                    SqlParameter userNameParam = cmd.Parameters.AddWithValue("@UserName", PortalContext.CurrentUser.Name);
                    userNameParam.SqlDbType = SqlDbType.NVarChar;

                    conn.Open();
                    cmd.CommandTimeout = 36000;
                    processedResult = Convert.ToInt16(cmd.ExecuteScalar());
                    conn.Close();
                }
            }
            catch (Exception exception)
            {
                processedResult = 0;
            }
            return processedResult;
        }
    }
}
