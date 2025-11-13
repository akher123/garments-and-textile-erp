using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Linq;
using SCERP.Common;
using System.Data;
using System.Data.SqlClient;
using SCERP.Model.Custom;
using SCERP.Model.HRMModel;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class MaternityLeaveRepository : Repository<HrmMaternityPayment>, IMaternityLeaveRepository
    {
        private SqlConnection _connection;
        private readonly SCERPDBContext _context;

        public MaternityLeaveRepository(SCERPDBContext context)
            : base(context)
        {
            _context = context;
            _connection = (SqlConnection) _context.Database.Connection;
        }

        public List<MaternityLeaveInfo> GetMaternityLeaveInfoByPaging(int startPage, int pageSize, out int totalRecords, HrmMaternityPayment model, SearchFieldModel searchFieldModel)
        {
            if (searchFieldModel.SearchByCompanyId == 0)
                searchFieldModel.SearchByCompanyId = -1;
            var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = searchFieldModel.SearchByCompanyId };

            if (searchFieldModel.SearchByBranchId == 0)
                searchFieldModel.SearchByBranchId = -1;
            var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = searchFieldModel.SearchByBranchId };

            if (searchFieldModel.SearchByBranchId == 0)
                searchFieldModel.SearchByBranchId = -1;
            var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = searchFieldModel.SearchByBranchId };

            if (searchFieldModel.SearchByBranchUnitDepartmentId == 0)
                searchFieldModel.SearchByBranchUnitDepartmentId = -1;
            var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = searchFieldModel.SearchByBranchUnitDepartmentId };

            if (searchFieldModel.SearchByDepartmentSectionId == 0)
                searchFieldModel.SearchByDepartmentSectionId = -1;
            var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = searchFieldModel.SearchByDepartmentSectionId };

            if (searchFieldModel.SearchByDepartmentLineId == 0)
                searchFieldModel.SearchByDepartmentLineId = -1;
            var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = searchFieldModel.SearchByDepartmentLineId };

            if (searchFieldModel.SearchByEmployeeTypeId == 0)
                searchFieldModel.SearchByEmployeeTypeId = -1;
            var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = searchFieldModel.SearchByEmployeeTypeId };

            if (String.IsNullOrEmpty(searchFieldModel.SearchByEmployeeCardId))
                searchFieldModel.SearchByEmployeeCardId = string.Empty;
            var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = searchFieldModel.SearchByEmployeeCardId };


            var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = searchFieldModel.EndDate };

            var toDateParam = new SqlParameter { ParameterName = "ToDate", Value = searchFieldModel.StartDate };

            var userNameParam = new SqlParameter { ParameterName = "UserName", Value = searchFieldModel.SearchByUserName };

            List<MaternityLeaveInfo> maternity = Context.Database.SqlQuery<MaternityLeaveInfo>("SPGetMaternityLeaveInfo @CompanyId,@BranchId,@BranchUnitId,@BranchUnitDepartmentId,@DepartmentSectionId,@DepartmentLineId,@EmployeeCardId,@FromDate,@ToDate,@UserName", companyIdParam, branchIdParam, branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam, employeeCardIdParam, fromDateParam, toDateParam, userNameParam).ToList();

            totalRecords = maternity.Count();

            switch (model.sort)
            {
                case "EmployeeName":
                    switch (model.sortdir)
                    {

                        case "DESC":
                            maternity = maternity.OrderByDescending(
                                x => x.EmployeeName).ThenBy(x => x.FirstPaymentDate)
                                .Skip(startPage * pageSize)
                                .Take(pageSize).ToList();
                            break;
                        default:
                            maternity = maternity.OrderBy(
                                x => x.EmployeeName).ThenBy(x => x.FirstPaymentDate)
                                .Skip(startPage * pageSize)
                                .Take(pageSize).ToList();
                            break;
                    }
                    break;
                case "EmployeeCardId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            maternity = maternity
                                .OrderByDescending(r => r.EmployeeCardId).ThenBy(x => x.FirstPaymentDate)
                                .Skip(startPage * pageSize)
                                .Take(pageSize).ToList();
                            break;
                        default:
                            maternity = maternity
                                .OrderBy(r => r.EmployeeCardId).ThenBy(x => x.FirstPaymentDate)
                                .Skip(startPage * pageSize)
                                .Take(pageSize).ToList();
                            break;
                    }
                    break;
                case "Designation":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            maternity = maternity
                                .OrderByDescending(r => r.Designation).ThenBy(x => x.FirstPaymentDate)
                                .Skip(startPage * pageSize)
                                .Take(pageSize).ToList();
                            break;
                        default:
                            maternity = maternity
                                .OrderBy(r => r.Designation).ThenBy(x => x.FirstPaymentDate)
                                .Skip(startPage * pageSize)
                                .Take(pageSize).ToList();
                            break;
                    }
                    break;

                default:
                    maternity = maternity
                        .OrderBy(r => r.EmployeeCardId).ThenBy(x => x.FirstPaymentDate)
                        .Skip(startPage * pageSize)
                        .Take(pageSize).ToList();
                    break;
            }
            return maternity.ToList();
        }

        public HrmMaternityPayment GetMaternityPaymentById(int maternityPaymentId)
        {
            HrmMaternityPayment maternity = _context.HrmMaternityPayments.FirstOrDefault(p => p.MaternityPaymentId == maternityPaymentId);
            return maternity;
        }

        public Employee GetEmployeeIdByCardId(string employeeCardId)
        {
            Employee employee = _context.Employees.FirstOrDefault(p => p.EmployeeCardId == employeeCardId) ?? new Employee();
            return employee;
        }

        public Employee GetEmployeeCardIdByEmployeeId(Guid employeeId)
        {
            Employee employee = _context.Employees.FirstOrDefault(p => p.EmployeeId == employeeId) ?? new Employee();
            return employee;
        }
    }
}
