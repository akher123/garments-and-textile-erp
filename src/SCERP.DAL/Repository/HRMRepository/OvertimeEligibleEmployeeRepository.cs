using System.Runtime.Remoting.Contexts;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class OvertimeEligibleEmployeeRepository : Repository<OvertimeEligibleEmployee>, IOvertimeEligibleEmployeeRepository
    {
        public OvertimeEligibleEmployeeRepository(SCERPDBContext context)
            : base(context)
        {

        }
        public List<VOvertimeEligibleEmployeeDetail> GetOvertimeEligibleEmployeeByPaging(int startPage, int pageSize,
            out int totalRecords,
            OvertimeEligibleEmployee model, SearchFieldModel searchFieldModel)
        {
            IQueryable<VOvertimeEligibleEmployeeDetail> overtimeEligibleEmployeeDetails;
            
            try
            {
                var fromDate = searchFieldModel.StartDate;
                var toDate = searchFieldModel.EndDate;
                if (searchFieldModel.StartDate.HasValue && searchFieldModel.EndDate.HasValue)
                {
                    fromDate = fromDate.GetValueOrDefault();
                    toDate = toDate.GetValueOrDefault();
                }
                IEnumerable<int> companyIdList = PortalContext.CurrentUser.PermissionContext.CompanyList.Select(x => x.CompanyId);
                IEnumerable<int> branchIdList = PortalContext.CurrentUser.PermissionContext.BranchList.Select(x => x.BranchId);
                IEnumerable<int> branchUnitIdList = PortalContext.CurrentUser.PermissionContext.UnitList.Select(x => x.BranchUnitId);
                IEnumerable<int> branchUnitDepartmentIdList = PortalContext.CurrentUser.PermissionContext.DepartmentList.Select(x => x.BranchUnitDepartmentId);
                IEnumerable<int> employeeTypeList = PortalContext.CurrentUser.PermissionContext.EmployeeTypeList.Select(x => x.Id);
                overtimeEligibleEmployeeDetails =
                                      Context.VOvertimeEligibleEmployeeDetails.Where(x => companyIdList.Contains(x.CompanyId) &&
                                      branchIdList.Contains(x.BranchId) &&
                                      branchUnitIdList.Contains(x.BranchUnitId) &&
                                      branchUnitDepartmentIdList.Contains(x.BranchUnitDepartmentId) &&
                                      employeeTypeList.Contains(x.EmployeeTypeId) &&
                                      (x.EmployeeCardId == searchFieldModel.SearchByEmployeeCardId || searchFieldModel.SearchByEmployeeCardId == null) &&
                                      (x.DepartmentSectionId == searchFieldModel.SearchByDepartmentSectionId || searchFieldModel.SearchByDepartmentSectionId == 0) &&
                                      (x.DepartmentLineId == searchFieldModel.SearchByDepartmentLineId || searchFieldModel.SearchByDepartmentLineId == 0) &&
                                      ((x.OvertimeDate >= fromDate || fromDate == null) && 
                                      (x.OvertimeDate <= toDate || toDate == null)));

                if (searchFieldModel.SearchByCompanyId > 0 || searchFieldModel.SearchByBranchId > 0 || searchFieldModel.SearchByBranchUnitId > 0 || searchFieldModel.SearchByBranchUnitDepartmentId > 0)
                {
                    overtimeEligibleEmployeeDetails =
                        overtimeEligibleEmployeeDetails.Where(x => (x.CompanyId == searchFieldModel.SearchByCompanyId || searchFieldModel.SearchByCompanyId == 0)
                        && (x.BranchId == searchFieldModel.SearchByBranchId || searchFieldModel.SearchByBranchId == 0)
                        && (x.BranchUnitId == searchFieldModel.SearchByBranchUnitId || searchFieldModel.SearchByBranchUnitId == 0)
                        && (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId || searchFieldModel.SearchByBranchUnitDepartmentId == 0));
                }

                totalRecords = overtimeEligibleEmployeeDetails.Count();

                switch (model.sort)
                {
                    case "CompanyName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                    .OrderByDescending(r => r.CompanyName).ThenBy(r=>r.OvertimeDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                     .OrderBy(r => r.CompanyName).ThenBy(r=>r.OvertimeDate)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    case "OvertimeHour":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                    .OrderByDescending(r => r.OvertimeHour).ThenBy(r => r.OvertimeDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                     .OrderBy(r => r.OvertimeHour).ThenBy(r => r.OvertimeDate)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;

                    case "BranchName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                      .OrderByDescending(r => r.BranchName).ThenBy(r => r.OvertimeDate)
                                      .Skip(startPage * pageSize)
                                      .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                     .OrderBy(r => r.BranchName).ThenBy(r => r.OvertimeDate)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    case "UnitName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                      .OrderByDescending(r => r.UnitName).ThenBy(r => r.OvertimeDate)
                                      .Skip(startPage * pageSize)
                                      .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                      .OrderBy(r => r.UnitName).ThenBy(r => r.OvertimeDate)
                                      .Skip(startPage * pageSize)
                                      .Take(pageSize);
                                break;
                        }
                        break;
                    case "DepartmentName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                 .OrderByDescending(r => r.DepartmentName).ThenBy(r => r.OvertimeDate)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                     .OrderBy(r => r.DepartmentName).ThenBy(r => r.OvertimeDate)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    case "EmployeeName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                 .OrderByDescending(r => r.EmployeeName).ThenBy(r => r.OvertimeDate)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                     .OrderBy(r => r.EmployeeName).ThenBy(r => r.OvertimeDate)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    case "SectionName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                 .OrderByDescending(r => r.SectionName).ThenBy(r => r.OvertimeDate)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                     .OrderBy(r => r.EmployeeCardId).ThenBy(r => r.OvertimeDate)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    case "LneName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                 .OrderByDescending(r => r.LineName).ThenBy(r => r.OvertimeDate)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                     .OrderBy(r => r.LineName).ThenBy(r => r.OvertimeDate)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    case "OvertimeDate":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                 .OrderByDescending(r => r.OvertimeDate)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                     .OrderBy(r => r.OvertimeDate)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    default:
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                  .OrderByDescending(r => r.EmployeeCardId).ThenBy(r => r.OvertimeDate)
                                  .Skip(startPage * pageSize)
                                  .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                 .OrderBy(r => r.EmployeeCardId).ThenBy(r => r.OvertimeDate)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                        }

                        break;
                }
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return overtimeEligibleEmployeeDetails.ToList();
        }

        public List<VOvertimeEligibleEmployeeDetail> GetOvertimeIneligibleEmployeeByPaging(int startPage, int pageSize,
            out int totalRecords,
            OvertimeEligibleEmployee model, SearchFieldModel searchFieldModel)
        {
            IQueryable<VOvertimeEligibleEmployeeDetail> overtimeEligibleEmployeeDetails;

            try
            {
                var fromDate = searchFieldModel.StartDate;
                var toDate = searchFieldModel.EndDate;
                if (searchFieldModel.StartDate.HasValue && searchFieldModel.EndDate.HasValue)
                {
                    fromDate = fromDate.GetValueOrDefault();
                    toDate = toDate.GetValueOrDefault();
                }
                IEnumerable<int> companyIdList = PortalContext.CurrentUser.PermissionContext.CompanyList.Select(x => x.CompanyId);
                IEnumerable<int> branchIdList = PortalContext.CurrentUser.PermissionContext.BranchList.Select(x => x.BranchId);
                IEnumerable<int> branchUnitIdList = PortalContext.CurrentUser.PermissionContext.UnitList.Select(x => x.BranchUnitId);
                IEnumerable<int> branchUnitDepartmentIdList = PortalContext.CurrentUser.PermissionContext.DepartmentList.Select(x => x.BranchUnitDepartmentId);
                IEnumerable<int> employeeTypeList = PortalContext.CurrentUser.PermissionContext.EmployeeTypeList.Select(x => x.Id);
                overtimeEligibleEmployeeDetails =
                                      Context.VOvertimeEligibleEmployeeDetails.Where(x => companyIdList.Contains(x.CompanyId) &&
                                      branchIdList.Contains(x.BranchId) &&
                                      branchUnitIdList.Contains(x.BranchUnitId) &&
                                      branchUnitDepartmentIdList.Contains(x.BranchUnitDepartmentId) &&
                                      employeeTypeList.Contains(x.EmployeeTypeId) &&
                                      (x.EmployeeCardId == searchFieldModel.SearchByEmployeeCardId || searchFieldModel.SearchByEmployeeCardId == null) &&
                                      (x.DepartmentSectionId == searchFieldModel.SearchByDepartmentSectionId || searchFieldModel.SearchByDepartmentSectionId == 0) &&
                                      (x.DepartmentLineId == searchFieldModel.SearchByDepartmentLineId || searchFieldModel.SearchByDepartmentLineId == 0) &&
                                      ((x.OvertimeDate >= fromDate || fromDate == null) &&
                                      (x.OvertimeDate <= toDate || toDate == null)));

                if (searchFieldModel.SearchByCompanyId > 0 || searchFieldModel.SearchByBranchId > 0 || searchFieldModel.SearchByBranchUnitId > 0 || searchFieldModel.SearchByBranchUnitDepartmentId > 0)
                {
                    overtimeEligibleEmployeeDetails =
                        overtimeEligibleEmployeeDetails.Where(x => (x.CompanyId == searchFieldModel.SearchByCompanyId || searchFieldModel.SearchByCompanyId == 0)
                        && (x.BranchId == searchFieldModel.SearchByBranchId || searchFieldModel.SearchByBranchId == 0)
                        && (x.BranchUnitId == searchFieldModel.SearchByBranchUnitId || searchFieldModel.SearchByBranchUnitId == 0)
                        && (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId || searchFieldModel.SearchByBranchUnitDepartmentId == 0));
                }

                totalRecords = overtimeEligibleEmployeeDetails.Count();

                switch (model.sort)
                {
                    case "CompanyName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                    .OrderByDescending(r => r.CompanyName).ThenBy(r => r.OvertimeDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                     .OrderBy(r => r.CompanyName).ThenBy(r => r.OvertimeDate)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    case "OvertimeHour":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                    .OrderByDescending(r => r.OvertimeHour).ThenBy(r => r.OvertimeDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                     .OrderBy(r => r.OvertimeHour).ThenBy(r => r.OvertimeDate)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;

                    case "BranchName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                      .OrderByDescending(r => r.BranchName).ThenBy(r => r.OvertimeDate)
                                      .Skip(startPage * pageSize)
                                      .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                     .OrderBy(r => r.BranchName).ThenBy(r => r.OvertimeDate)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    case "UnitName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                      .OrderByDescending(r => r.UnitName).ThenBy(r => r.OvertimeDate)
                                      .Skip(startPage * pageSize)
                                      .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                      .OrderBy(r => r.UnitName).ThenBy(r => r.OvertimeDate)
                                      .Skip(startPage * pageSize)
                                      .Take(pageSize);
                                break;
                        }
                        break;
                    case "DepartmentName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                 .OrderByDescending(r => r.DepartmentName).ThenBy(r => r.OvertimeDate)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                     .OrderBy(r => r.DepartmentName).ThenBy(r => r.OvertimeDate)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    case "EmployeeName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                 .OrderByDescending(r => r.EmployeeName).ThenBy(r => r.OvertimeDate)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                     .OrderBy(r => r.EmployeeName).ThenBy(r => r.OvertimeDate)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    case "SectionName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                 .OrderByDescending(r => r.SectionName).ThenBy(r => r.OvertimeDate)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                     .OrderBy(r => r.EmployeeCardId).ThenBy(r => r.OvertimeDate)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    case "LneName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                 .OrderByDescending(r => r.LineName).ThenBy(r => r.OvertimeDate)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                     .OrderBy(r => r.LineName).ThenBy(r => r.OvertimeDate)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    case "OvertimeDate":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                 .OrderByDescending(r => r.OvertimeDate)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                     .OrderBy(r => r.OvertimeDate)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    default:
                        switch (model.sortdir)
                        {
                            case "DESC":
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                  .OrderByDescending(r => r.EmployeeCardId).ThenBy(r => r.OvertimeDate)
                                  .Skip(startPage * pageSize)
                                  .Take(pageSize);
                                break;
                            default:
                                overtimeEligibleEmployeeDetails = overtimeEligibleEmployeeDetails
                                 .OrderBy(r => r.EmployeeCardId).ThenBy(r => r.OvertimeDate)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                        }

                        break;
                }
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return overtimeEligibleEmployeeDetails.ToList();
        }

        public List<OvertimeEligibleEmployee> GetOvertimeEligibleEmployeeListBySearchKey(
            OvertimeEligibleEmployee overtimeEligibleEmployeepar)
        {
            return Context.OvertimeEligibleEmployees.Where(x=>x.EmployeeId==overtimeEligibleEmployeepar.EmployeeId && x.OvertimeDate==overtimeEligibleEmployeepar.OvertimeDate).ToList();
            
        }

        public int SaveOvertimeEligibleEmployee(List<OvertimeEligibleEmployee> overtimeEligibleEmployees)
        {
            var saveIndex = 0;
            try
            {
                Context.OvertimeEligibleEmployees.AddRange(overtimeEligibleEmployees);
                saveIndex = Context.SaveChanges();
            }
            catch (Exception exception)
            {

                throw;
            }
            return saveIndex;
        }

        public List<VOvertimeEligibleEmployeeDetail> GetOvertimeEligibleEmployeeBySearchKey(SearchFieldModel searchFieldModel)
        {

            IQueryable<VOvertimeEligibleEmployeeDetail> overtimeEligibleEmployeeDetails;
            try
            {

                var fromDate = searchFieldModel.StartDate;
                var toDate = searchFieldModel.EndDate;
                if (searchFieldModel.StartDate.HasValue && searchFieldModel.EndDate.HasValue)
                {
                    fromDate = fromDate.GetValueOrDefault();
                    toDate = toDate.GetValueOrDefault();
                }
                IEnumerable<int> companyIdList = PortalContext.CurrentUser.PermissionContext.CompanyList.Select(x => x.CompanyId);
                IEnumerable<int> branchIdList = PortalContext.CurrentUser.PermissionContext.BranchList.Select(x => x.BranchId);
                IEnumerable<int> branchUnitIdList = PortalContext.CurrentUser.PermissionContext.UnitList.Select(x => x.BranchUnitId);
                IEnumerable<int> branchUnitDepartmentIdList = PortalContext.CurrentUser.PermissionContext.DepartmentList.Select(x => x.BranchUnitDepartmentId);
                IEnumerable<int> employeeTypeList = PortalContext.CurrentUser.PermissionContext.EmployeeTypeList.Select(x => x.Id);
                overtimeEligibleEmployeeDetails =
                                      Context.VOvertimeEligibleEmployeeDetails.Where(x => companyIdList.Contains(x.CompanyId) &&
                                      branchIdList.Contains(x.BranchId) &&
                                      branchUnitIdList.Contains(x.BranchUnitId) &&
                                      branchUnitDepartmentIdList.Contains(x.BranchUnitDepartmentId) &&
                                      employeeTypeList.Contains(x.EmployeeTypeId) &&
                                      (x.EmployeeCardId == searchFieldModel.SearchByEmployeeCardId || searchFieldModel.SearchByEmployeeCardId == null) &&
                                      (x.DepartmentSectionId == searchFieldModel.SearchByDepartmentSectionId || searchFieldModel.SearchByDepartmentSectionId == 0) &&
                                      (x.DepartmentLineId == searchFieldModel.SearchByDepartmentLineId || searchFieldModel.SearchByDepartmentLineId == 0) &&
                                      ((x.OvertimeDate >= fromDate || fromDate == null) &&
                                      (x.OvertimeDate <= toDate || toDate == null)));

                if (searchFieldModel.SearchByCompanyId > 0 || searchFieldModel.SearchByBranchId > 0 || searchFieldModel.SearchByBranchUnitId > 0 || searchFieldModel.SearchByBranchUnitDepartmentId > 0)
                {
                    overtimeEligibleEmployeeDetails =
                        overtimeEligibleEmployeeDetails.Where(x => (x.CompanyId == searchFieldModel.SearchByCompanyId || searchFieldModel.SearchByCompanyId == 0)
                        && (x.BranchId == searchFieldModel.SearchByBranchId || searchFieldModel.SearchByBranchId == 0)
                        && (x.BranchUnitId == searchFieldModel.SearchByBranchUnitId || searchFieldModel.SearchByBranchUnitId == 0)
                        && (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId || searchFieldModel.SearchByBranchUnitDepartmentId == 0));
                }

            }
            catch (Exception)
            {

                throw;
            }

            return overtimeEligibleEmployeeDetails.ToList();
        }

        public List<VEmployeeCompanyInfoDetail> GetEmployes(int startPage, int pageSize, out int totalRecords,
            OvertimeEligibleEmployee model, SearchFieldModel searchFieldModel)
        {

            IQueryable<VEmployeeCompanyInfoDetail> employeeCompanyInfoDetails;
            try
            {
                IEnumerable<int> companyIdList = PortalContext.CurrentUser.PermissionContext.CompanyList.Select(x => x.CompanyId);
                IEnumerable<int> branchIdList = PortalContext.CurrentUser.PermissionContext.BranchList.Select(x => x.BranchId);
                IEnumerable<int> branchUnitIdList = PortalContext.CurrentUser.PermissionContext.UnitList.Select(x => x.BranchUnitId);
                IEnumerable<int> branchUnitDepartmentIdList = PortalContext.CurrentUser.PermissionContext.DepartmentList.Select(x => x.BranchUnitDepartmentId);
                IEnumerable<int> employeeTypeList = PortalContext.CurrentUser.PermissionContext.EmployeeTypeList.Select(x => x.Id);


                employeeCompanyInfoDetails =
                                      Context.VEmployeeCompanyInfoDetails.Where(x => companyIdList.Contains(x.CompanyId) &&
                                      branchIdList.Contains(x.BranchId) &&
                                      branchUnitIdList.Contains(x.BranchUnitId) &&
                                      branchUnitDepartmentIdList.Contains(x.BranchUnitDepartmentId) &&
                                      employeeTypeList.Contains(x.EmployeeTypeId) &&
                                      (x.EmployeeCardId == searchFieldModel.SearchByEmployeeCardId || searchFieldModel.SearchByEmployeeCardId == null) &&
                                      (x.DepartmentSectionId == searchFieldModel.SearchByDepartmentSectionId || searchFieldModel.SearchByDepartmentSectionId == 0) &&
                                      (x.DepartmentLineId == searchFieldModel.SearchByDepartmentLineId || searchFieldModel.SearchByDepartmentLineId == 0)
                                      );

                if (searchFieldModel.SearchByCompanyId > 0 || searchFieldModel.SearchByBranchId > 0 || searchFieldModel.SearchByBranchUnitId > 0 || searchFieldModel.SearchByBranchUnitDepartmentId > 0)
                {
                    employeeCompanyInfoDetails =
                        employeeCompanyInfoDetails.Where(x => (x.CompanyId == searchFieldModel.SearchByCompanyId || searchFieldModel.SearchByCompanyId == 0)
                        && (x.BranchId == searchFieldModel.SearchByBranchId || searchFieldModel.SearchByBranchId == 0)
                        && (x.BranchUnitId == searchFieldModel.SearchByBranchUnitId || searchFieldModel.SearchByBranchUnitId == 0)
                        && (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId || searchFieldModel.SearchByBranchUnitDepartmentId == 0));
                }

                totalRecords = employeeCompanyInfoDetails.Count();
                switch (model.sort)
                {
                    case "EmployeeName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.EmployeeName);
                                    //.Skip(startPage * pageSize)
                                    //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.EmployeeName);
                                     //.Skip(startPage * pageSize)
                                     //.Take(pageSize);
                                break;
                        }
                        break;
                    case "EmployeeCardId":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.EmployeeCardId);
                                    //.Skip(startPage * pageSize)
                                    //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.EmployeeCardId);
                                    //.Skip(startPage * pageSize)
                                    //.Take(pageSize);
                                break;
                        }
                        break;
                    case "Designation":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                 .OrderByDescending(r => r.Designation);
                                 //.Skip(startPage * pageSize)
                                 //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.Designation);
                                 //.Skip(startPage * pageSize)
                                 //.Take(pageSize);
                                break;
                        }
                        break;
                    case "CompanyName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.CompanyName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.CompanyName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                        }
                        break;
                    case "BranchName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.BranchName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.BranchName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                        }
                        break;
                    case "UnitName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.UnitName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.UnitName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                        }
                        break;
                    case "DepartmentName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.DepartmentName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.DepartmentName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                        }
                        break;
                    case "SectionName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.SectionName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.SectionName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                        }
                        break;
                    case "LineName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.LineName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.LineName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                        }
                        break;

                    default:
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.EmployeeCardId);
                               //.Skip(startPage * pageSize)
                               //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.EmployeeCardId);
                            //.Skip(startPage * pageSize)
                            //.Take(pageSize);
                                break;
                        }

                        break;
                }

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return employeeCompanyInfoDetails.ToList();
        }

        public List<VEmployeeCompanyInfoDetail> GetEligibleEmployees(int startPage, int pageSize, out int totalRecords,
            OvertimeEligibleEmployee model, SearchFieldModel searchFieldModel)
        {

            IQueryable<VEmployeeCompanyInfoDetail> employeeCompanyInfoDetails;
            try
            {
                IEnumerable<int> companyIdList = PortalContext.CurrentUser.PermissionContext.CompanyList.Select(x => x.CompanyId);
                IEnumerable<int> branchIdList = PortalContext.CurrentUser.PermissionContext.BranchList.Select(x => x.BranchId);
                IEnumerable<int> branchUnitIdList = PortalContext.CurrentUser.PermissionContext.UnitList.Select(x => x.BranchUnitId);
                IEnumerable<int> branchUnitDepartmentIdList = PortalContext.CurrentUser.PermissionContext.DepartmentList.Select(x => x.BranchUnitDepartmentId);
                IEnumerable<int> employeeTypeList = PortalContext.CurrentUser.PermissionContext.EmployeeTypeList.Select(x => x.Id);


                employeeCompanyInfoDetails =
                                      Context.VEmployeeCompanyInfoDetails.Where(x => companyIdList.Contains(x.CompanyId) &&
                                      branchIdList.Contains(x.BranchId) &&
                                      branchUnitIdList.Contains(x.BranchUnitId) &&
                                      branchUnitDepartmentIdList.Contains(x.BranchUnitDepartmentId) &&
                                      employeeTypeList.Contains(x.EmployeeTypeId) &&
                                      (x.EmployeeCardId == searchFieldModel.SearchByEmployeeCardId || searchFieldModel.SearchByEmployeeCardId == null) &&
                                      (x.DepartmentSectionId == searchFieldModel.SearchByDepartmentSectionId || searchFieldModel.SearchByDepartmentSectionId == 0) &&
                                      (x.DepartmentLineId == searchFieldModel.SearchByDepartmentLineId || searchFieldModel.SearchByDepartmentLineId == 0)
                                      );

                if (searchFieldModel.SearchByCompanyId > 0 || searchFieldModel.SearchByBranchId > 0 || searchFieldModel.SearchByBranchUnitId > 0 || searchFieldModel.SearchByBranchUnitDepartmentId > 0)
                {
                    employeeCompanyInfoDetails =
                        employeeCompanyInfoDetails.Where(x => (x.CompanyId == searchFieldModel.SearchByCompanyId || searchFieldModel.SearchByCompanyId == 0)
                        && (x.BranchId == searchFieldModel.SearchByBranchId || searchFieldModel.SearchByBranchId == 0)
                        && (x.BranchUnitId == searchFieldModel.SearchByBranchUnitId || searchFieldModel.SearchByBranchUnitId == 0)
                        && (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId || searchFieldModel.SearchByBranchUnitDepartmentId == 0));
                }

                totalRecords = employeeCompanyInfoDetails.Count();
                switch (model.sort)
                {
                    case "EmployeeName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.EmployeeName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.EmployeeName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                        }
                        break;
                    case "EmployeeCardId":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.EmployeeCardId);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.EmployeeCardId);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                        }
                        break;
                    case "Designation":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                 .OrderByDescending(r => r.Designation);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.Designation);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                        }
                        break;
                    case "CompanyName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.CompanyName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.CompanyName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                        }
                        break;
                    case "BranchName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.BranchName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.BranchName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                        }
                        break;
                    case "UnitName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.UnitName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.UnitName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                        }
                        break;
                    case "DepartmentName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.DepartmentName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.DepartmentName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                        }
                        break;
                    case "SectionName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.SectionName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.SectionName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                        }
                        break;
                    case "LineName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.LineName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.LineName);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                        }
                        break;

                    default:
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.EmployeeCardId);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.EmployeeCardId);
                                //.Skip(startPage * pageSize)
                                //.Take(pageSize);
                                break;
                        }

                        break;
                }

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return employeeCompanyInfoDetails.ToList();
        }

        public int DeleteOvertimeEligibleEmployee(OvertimeEligibleEmployee overtimeEligibleEmployee)
        {
            var deleteIndex = 0;
            try
            {
                overtimeEligibleEmployee.EditedDate = DateTime.Now;
                overtimeEligibleEmployee.EditedBy = PortalContext.CurrentUser.UserId;
                overtimeEligibleEmployee.IsActive = false;
                deleteIndex = Edit(overtimeEligibleEmployee);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return deleteIndex;
        }
    }
}
