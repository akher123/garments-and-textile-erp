using System;
using System.Linq;
using System.Linq.Expressions;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model.HRMModel;
using SCERP.Model.Custom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SCERP.Model;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class PenaltyRepository : Repository<HrmPenalty>, IPenaltyRepository
    {
        public PenaltyRepository(SCERPDBContext context) : base(context)
        {
        }

        public IQueryable<VwPenaltyEmployee> GetVwPenaltyEmployee(Expression<Func<VwPenaltyEmployee, bool>> predicates)
        {
            return Context.VwPenaltyEmployee.Where(predicates);
        }

        public List<SPGetAbsentOtPenaltyEmployee> GetAbsentOtPenaltyEmployee(SearchFieldModel searchFieldModel)
        {

            if (searchFieldModel.SearchByCompanyId == 0)
                searchFieldModel.SearchByCompanyId = -1;
            var companyIdParam = new SqlParameter {ParameterName = "CompanyId", Value = searchFieldModel.SearchByCompanyId};

            if (searchFieldModel.SearchByBranchId == 0)
                searchFieldModel.SearchByBranchId = -1;
            var branchIdParam = new SqlParameter {ParameterName = "BranchId", Value = searchFieldModel.SearchByBranchId};

            if (searchFieldModel.SearchByBranchUnitId == 0)
                searchFieldModel.SearchByBranchUnitId = -1;
            var branchUnitIdParam = new SqlParameter {ParameterName = "BranchUnitId", Value = searchFieldModel.SearchByBranchUnitId};

            if (searchFieldModel.SearchByBranchUnitDepartmentId == 0)
                searchFieldModel.SearchByBranchUnitDepartmentId = -1;
            var branchUnitDepartmentIdParam = new SqlParameter {ParameterName = "BranchUnitDepartmentId", Value = searchFieldModel.SearchByBranchUnitDepartmentId};

            if (searchFieldModel.SearchByDepartmentSectionId == 0)
                searchFieldModel.SearchByDepartmentSectionId = -1;
            var departmentSectionIdParam = new SqlParameter {ParameterName = "DepartmentSectionId", Value = searchFieldModel.SearchByDepartmentSectionId};

            if (searchFieldModel.SearchByDepartmentLineId == 0)
                searchFieldModel.SearchByDepartmentLineId = -1;
            var departmentLineIdParam = new SqlParameter {ParameterName = "DepartmentLineId", Value = searchFieldModel.SearchByDepartmentLineId};

            if (searchFieldModel.StartDate == null)
                searchFieldModel.StartDate = new DateTime(1900, 01, 01);
            var fromDate = new SqlParameter {ParameterName = "FromDate", Value = searchFieldModel.StartDate};

            var toDate = new SqlParameter { ParameterName = "ToDate", Value = searchFieldModel.StartDate };

            if (String.IsNullOrEmpty(searchFieldModel.SearchByEmployeeCardId))
                searchFieldModel.SearchByEmployeeCardId = String.Empty;
            var employeeCardIdParam = new SqlParameter {ParameterName = "EmployeeCardId", Value = searchFieldModel.SearchByEmployeeCardId};

            var userNameParam = new SqlParameter {ParameterName = "UserName", Value = "SuperAdmin"};

            return Context.Database.SqlQuery<SPGetAbsentOtPenaltyEmployee>("SPGetAbsentOtPenaltyEmployee @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, @DepartmentSectionId, @DepartmentLineId, @FromDate,@ToDate, @EmployeeCardId, @UserName",
                companyIdParam, branchIdParam, branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam, fromDate, toDate, employeeCardIdParam, userNameParam).ToList();

        }

        public int SavePenaltyEmployee(List<HrmAbsentOTPenalty> penaltyEmployees, DateTime fromDate)
        {
            var saveIndex = 0;
            var processedResult = 0;
            var dataTableEmployee = new DataTable();

            dataTableEmployee.Columns.Add("EmployeeId", typeof (Guid));

            foreach (var employee in penaltyEmployees)
            {
                var dataRow = dataTableEmployee.NewRow();
                dataRow[0] = employee.EmployeeId;
                dataTableEmployee.Rows.Add(dataRow);
            }

            var connectionString = Context.Database.Connection.ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            using (conn)
            {
                SqlCommand cmd = new SqlCommand("dbo.SPAbsentOtPenaltySave", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter fromDateParam = cmd.Parameters.AddWithValue("@FromDate", fromDate);
                fromDateParam.SqlDbType = SqlDbType.DateTime;

                SqlParameter employeeIdCardListParam = cmd.Parameters.AddWithValue("@EmployeeList", dataTableEmployee);
                employeeIdCardListParam.SqlDbType = SqlDbType.Structured;

                conn.Open();
                cmd.CommandTimeout = 36000;
                processedResult = Convert.ToInt16(cmd.ExecuteScalar());
                conn.Close();

                if (processedResult == 0)
                    saveIndex = 1;
            }
            return saveIndex;
        }
    }
}
