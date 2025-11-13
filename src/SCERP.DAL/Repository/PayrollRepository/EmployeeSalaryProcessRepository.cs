using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.Model;
using SCERP.Common;
using SCERP.Model.Custom;
using System.Configuration;


namespace SCERP.DAL.Repository.PayrollRepository
{
    public class EmployeeSalaryProcessRepository : Repository<EmployeeProcessedSalary>, IEmployeeSalaryProcessRepository
    {
        public EmployeeSalaryProcessRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public List<EmployeeProcessedSalary> GetEmployeeSalaryProcessedInfo(int startPage, int pageSize, EmployeeProcessedSalary model, SearchFieldModel searchFieldModel)
        {
            try
            {
               
                var startPageParam = new SqlParameter { ParameterName = "StartRowIndex", Value = startPage };

                var pageSizeParam = new SqlParameter { ParameterName = "MaxRows", Value = pageSize };


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

                int year = 1900;
                if (Convert.ToInt32(searchFieldModel.SelectedYear) > 0)
                    year = Convert.ToInt32(searchFieldModel.SelectedYear);
                var yearParam = new SqlParameter { ParameterName = "Year", Value = year };

                int month = 1900;
                if (Convert.ToInt32(searchFieldModel.SelectedMonth) > 0)
                    month = Convert.ToInt32(searchFieldModel.SelectedMonth);
                var monthParam = new SqlParameter { ParameterName = "Month", Value = month };

                DateTime? fromDate = new DateTime(1900, 01, 01);
                if (searchFieldModel.StartDate != null)
                    fromDate = searchFieldModel.StartDate;
                var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

                DateTime? toDate = new DateTime(1900, 01, 01);
                if (searchFieldModel.EndDate != null)
                    toDate = searchFieldModel.EndDate;
                var toDateParam = new SqlParameter { ParameterName = "ToDate", Value = toDate };


                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = PortalContext.CurrentUser.Name };



                var employeeProcessedSalaryRecords = Context.Database.SqlQuery<EmployeeProcessedSalary>("SPGetEmployeeSalaryProcessedInfo @StartRowIndex, @MaxRows, @CompanyId, " +
                                                                         "@BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                         "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeCardId, @Year, @Month," +
                                                                         "@FromDate, @ToDate, @UserName", startPageParam, pageSizeParam, companyIdParam, 
                                                                         branchIdParam, branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam,
                                                                         departmentLineIdParam, employeeTypeIdParam, employeeCardIdParam, yearParam, monthParam,
                                                                         fromDateParam, toDateParam, userNameParam).ToList();

                return employeeProcessedSalaryRecords;


            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<EmployeesForSalaryProcessModel> GetEmployeesForSalaryProcess(SearchFieldModel searchFieldModel, EmployeeProcessedSalary model)
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

                DateTime? fromDate = new DateTime(1900, 01, 01);
                if (searchFieldModel.StartDate != null)
                    fromDate = searchFieldModel.StartDate;
                var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

                DateTime? toDate = new DateTime(1900, 01, 01);
                if (searchFieldModel.EndDate != null)
                    toDate = searchFieldModel.EndDate;
                var toDateParam = new SqlParameter { ParameterName = "ToDate", Value = toDate };

                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = PortalContext.CurrentUser.Name };

                var employeesForSalaryProcess = Context.Database.SqlQuery<EmployeesForSalaryProcessModel>("SPGetEmployeesForSalaryProcess  @CompanyId, " +
                                                                         "@BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                         "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, " +
                                                                         "@EmployeeCardId, @FromDate, @ToDate, @UserName", companyIdParam,
                                                                         branchIdParam, branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam,
                                                                         departmentLineIdParam, employeeTypeIdParam, employeeCardIdParam, fromDateParam, toDateParam,
                                                                         userNameParam).ToList();

                return employeesForSalaryProcess;


            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public int ProcessBulkEmployeeSalary(SearchFieldModel searchFieldModel, EmployeeProcessedSalary model)
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
                    SqlCommand cmd = new SqlCommand("dbo.SPProcessBulkEmployeeSalary", conn);
                    cmd.CommandType = CommandType.StoredProcedure;


                    int companyId = -1;
                    if (searchFieldModel.SearchByCompanyId > 0)
                        companyId = searchFieldModel.SearchByCompanyId;
                    SqlParameter companyIdParam = cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    companyIdParam.SqlDbType = SqlDbType.Int;

                    int branchId = -1;
                    if (searchFieldModel.SearchByBranchId > 0)
                        branchId = searchFieldModel.SearchByBranchId;
                    SqlParameter branchIdParam = cmd.Parameters.AddWithValue("@BranchId", branchId);
                    branchIdParam.SqlDbType = SqlDbType.Int;

                    int branchUnitId = -1;
                    if (searchFieldModel.SearchByBranchUnitId > 0)
                        branchUnitId = searchFieldModel.SearchByBranchUnitId;
                    SqlParameter branchUnitIdParam = cmd.Parameters.AddWithValue("@BranchUnitId", branchUnitId);
                    branchUnitIdParam.SqlDbType = SqlDbType.Int;

                    int branchUnitDepartmentId = -1;
                    if (searchFieldModel.SearchByBranchUnitDepartmentId > 0)
                        branchUnitDepartmentId = searchFieldModel.SearchByBranchUnitDepartmentId;
                    SqlParameter branchUnitDepartmentIdParam = cmd.Parameters.AddWithValue("@BranchUnitDepartmentId", branchUnitDepartmentId);
                    branchUnitDepartmentIdParam.SqlDbType = SqlDbType.Int;

                    int departmentSectionId = -1;
                    if (searchFieldModel.SearchByDepartmentSectionId > 0)
                        departmentSectionId = searchFieldModel.SearchByDepartmentSectionId;
                    SqlParameter departmentSectionIdParam = cmd.Parameters.AddWithValue("@DepartmentSectionId", departmentSectionId);
                    departmentSectionIdParam.SqlDbType = SqlDbType.Int;

                    int departmentLineId = -1;
                    if (searchFieldModel.SearchByDepartmentLineId > 0)
                        departmentLineId = searchFieldModel.SearchByDepartmentLineId;
                    SqlParameter departmentLineIdParam = cmd.Parameters.AddWithValue("@DepartmentLineId", departmentLineId);
                    departmentLineIdParam.SqlDbType = SqlDbType.Int;

                    int employeeTypeId = -1;
                    if (searchFieldModel.SearchByEmployeeTypeId > 0)
                        employeeTypeId = searchFieldModel.SearchByEmployeeTypeId;
                    SqlParameter employeeTypeIdParam = cmd.Parameters.AddWithValue("@EmployeeTypeId", employeeTypeId);
                    employeeTypeIdParam.SqlDbType = SqlDbType.Int;

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
