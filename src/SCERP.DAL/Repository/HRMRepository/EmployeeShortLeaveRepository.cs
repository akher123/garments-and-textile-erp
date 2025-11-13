using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Linq;
using SCERP.Model;
using SCERP.Common;
using System.Data;
using System.Data.SqlClient;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeShortLeaveRepository : Repository<EmployeeShortLeave>, IEmployeeShortLeaveRepository
    {
        private SqlConnection _connection;
        private readonly SCERPDBContext _context;

        public EmployeeShortLeaveRepository(SCERPDBContext context)
            : base(context)
        {
            _context = context;
            _connection = (SqlConnection) _context.Database.Connection;
        }

        public List<VEmployeeShortLeave> GetAllEmployeeShortLeavesByPaging(int startPage, int pageSize, out int totalRecords, ShortLeaveModel employeeShortLeave)
        {

            IQueryable<VEmployeeShortLeave> shortLeaves = null;

            try
            {
                Guid? employeeId = employeeShortLeave.EmployeeId;
                if (employeeId == Guid.Parse("00000000-0000-0000-0000-000000000000")) employeeId = null;

                int? reasonType = employeeShortLeave.ReasonType;
                DateTime? fromDate = employeeShortLeave.fromDate;
                DateTime? toDate = employeeShortLeave.toDate;
                DateTime dt = DateTime.Now;

                IEnumerable<int> companyIdList = PortalContext.CurrentUser.PermissionContext.CompanyList.Select(x => x.CompanyId);
                IEnumerable<int> branchIdList = PortalContext.CurrentUser.PermissionContext.BranchList.Select(x => x.BranchId);
                IEnumerable<int> branchUnitIdList = PortalContext.CurrentUser.PermissionContext.UnitList.Select(x => x.BranchUnitId);
                IEnumerable<int> branchUnitDepartmentIdList = PortalContext.CurrentUser.PermissionContext.DepartmentList.Select(x => x.BranchUnitDepartmentId);
                IEnumerable<int> employeeTypeList = PortalContext.CurrentUser.PermissionContext.EmployeeTypeList.Select(x => x.Id);

                shortLeaves =
                    Context.VEmployeeShortLeave.Where(x => companyIdList.Contains(x.CompanyId) &&
                                                           branchIdList.Contains(x.BranchId) &&
                                                           branchUnitIdList.Contains(x.BranchUnitId) &&
                                                           branchUnitDepartmentIdList.Contains(x.BranchUnitDepartmentId) &&
                                                           employeeTypeList.Contains(x.EmployeeTypeId) &&
                                                           (x.EmployeeId == employeeId || employeeId == null) &&
                                                           (x.ReasonType == reasonType || reasonType == 0) &&
                                                           ((x.Date >= fromDate || fromDate == null) &&
                                                            (x.Date <= toDate || toDate == null))).AsQueryable();

                totalRecords = shortLeaves.Count();

                switch (employeeShortLeave.sort)
                {
                    case "EmployeeName":

                        switch (employeeShortLeave.sortdir)
                        {
                            case "DESC":
                                shortLeaves = shortLeaves
                                    .OrderByDescending(r => r.EmployeeName)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                shortLeaves = shortLeaves
                                    .OrderBy(r => r.EmployeeName)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;

                    default:

                        switch (employeeShortLeave.sortdir)
                        {
                            case "DESC":
                                shortLeaves = shortLeaves
                                    .OrderByDescending(r => r.Id)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                shortLeaves = shortLeaves
                                    .OrderByDescending(r => r.Id)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                }
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.Message);
            }

            return shortLeaves.ToList();
        }

        public District GetDistrictById(int? id)
        {
            return Context.Districts.FirstOrDefault(x => x.Id == id);
        }

        public EmployeeShortLeave GetEmployeeShortLeaveById(int? id)
        {
            return Context.EmployeeShortLeaves.FirstOrDefault(p => p.Id == id);
        }

        public int CheckDuplicateDateTime(EmployeeShortLeave leave)
        {
            var temp = Context.EmployeeShortLeaves.Where(p => p.EmployeeId == leave.EmployeeId && p.Date == leave.Date && ((p.FromTime <= leave.FromTime && p.ToTime >= leave.FromTime) || (p.FromTime <= leave.ToTime && p.ToTime >= leave.ToTime) || (p.FromTime >= leave.FromTime && p.ToTime <= leave.ToTime)));

            if (temp.Any())
                return 2;
            else
                return 1;
        }
    }
}
