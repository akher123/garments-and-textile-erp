using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeWorkGroupRepository : Repository<EmployeeWorkGroup>, IEmployeeWorkGroupRepository
    {
        public EmployeeWorkGroupRepository(SCERPDBContext context)
            : base(context)
        {
        }
        public List<EmployeeWorkGroup> GetAllAssignedEmployeeWorkGroup(SearchFieldModel searchFieldModel)
        {
            IQueryable<EmployeeWorkGroup> employeeWorkGroups;
            employeeWorkGroups = Context.EmployeeWorkGroups
           .Include(x => x.Employee)
           .Include(x => x.WorkGroup.BranchUnit.Branch.Company)
           .Include(x => x.WorkGroup.BranchUnit.Unit)
            .Where(
                   x =>
                       x.IsActive && x.Status &&
                       (x.Employee.EmployeeCardId == searchFieldModel.SearchByEmployeeCardId ||
                        searchFieldModel.SearchByEmployeeCardId == null) &&
                       (x.WorkGroup.BranchUnit.Branch.CompanyId == searchFieldModel.SearchByCompanyId)
                       &&
                       (x.WorkGroup.BranchUnit.BranchId == searchFieldModel.SearchByBranchId)
                       &&
                       (x.WorkGroup.BranchUnitId == searchFieldModel.SearchByBranchUnitId) &&
                          (x.WorkGroupId == searchFieldModel.SearchByWorkGroupId || searchFieldModel.SearchByWorkGroupId == 0));
            return employeeWorkGroups.ToList();
        }



        public List<VEmployeeCompanyInfoDetail> GetAllUnAssignedEmployeeWorkGroup(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel, EmployeeWorkGroup model)
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
                                      (x.DepartmentLineId == searchFieldModel.SearchByDepartmentLineId || searchFieldModel.SearchByDepartmentLineId == 0) &&
                                      (!Context.EmployeeWorkGroups.Any(
                                      p => p.EmployeeId == x.EmployeeId && p.IsActive && p.Status))
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
                                    .OrderByDescending(r => r.EmployeeName)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                     .OrderBy(r => r.EmployeeName)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    case "EmployeeCardId":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.EmployeeCardId)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                .OrderBy(r => r.EmployeeCardId)
                                .Skip(startPage * pageSize)
                                .Take(pageSize);
                                break;
                        }
                        break;
                    case "Designation":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                 .OrderByDescending(r => r.Designation)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                 .OrderBy(r => r.Designation)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                        }
                        break;
                    case "CompanyName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                .OrderByDescending(r => r.CompanyName)
                                .Skip(startPage * pageSize)
                                .Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                .OrderBy(r => r.CompanyName)
                                .Skip(startPage * pageSize)
                                .Take(pageSize);
                                break;
                        }
                        break;
                    case "BranchName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                .OrderByDescending(r => r.BranchName)
                                .Skip(startPage * pageSize)
                                .Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                .OrderBy(r => r.BranchName)
                                .Skip(startPage * pageSize)
                                .Take(pageSize);
                                break;
                        }
                        break;
                    case "UnitName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                .OrderByDescending(r => r.UnitName)
                                .Skip(startPage * pageSize)
                                .Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                .OrderBy(r => r.UnitName)
                                .Skip(startPage * pageSize)
                                .Take(pageSize);
                                break;
                        }
                        break;
                    case "DepartmentName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                .OrderByDescending(r => r.DepartmentName)
                                .Skip(startPage * pageSize)
                                .Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                .OrderBy(r => r.DepartmentName)
                                .Skip(startPage * pageSize)
                                .Take(pageSize);
                                break;
                        }
                        break;
                    case "SectionName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                .OrderByDescending(r => r.SectionName)
                                .Skip(startPage * pageSize)
                                .Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                .OrderBy(r => r.SectionName)
                                .Skip(startPage * pageSize)
                                .Take(pageSize);
                                break;
                        }
                        break;
                    case "LineName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                .OrderByDescending(r => r.LineName)
                                .Skip(startPage * pageSize)
                                .Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                .OrderBy(r => r.LineName)
                                .Skip(startPage * pageSize)
                                .Take(pageSize);
                                break;
                        }
                        break;

                    default:
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                               .OrderByDescending(r => r.EmployeeCardId)
                               .Skip(startPage * pageSize)
                               .Take(pageSize);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                            .OrderBy(r => r.EmployeeCardId)
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
            return employeeCompanyInfoDetails.ToList();
        }

        public List<VEmployeeWorkGroupDetail> GetAllEmployeeWorkGroupByPaging(int startPage, int pageSize, out int totalRecords, EmployeeWorkGroup model,
            SearchFieldModel searchFieldModel)
        {
            IQueryable<VEmployeeWorkGroupDetail> employeeWorkGroupDetails;

            try
            {
               
                IEnumerable<int> companyIdList = PortalContext.CurrentUser.PermissionContext.CompanyList.Select(x => x.CompanyId);
                IEnumerable<int> branchIdList = PortalContext.CurrentUser.PermissionContext.BranchList.Select(x => x.BranchId);
                IEnumerable<int> branchUnitIdList = PortalContext.CurrentUser.PermissionContext.UnitList.Select(x => x.BranchUnitId);
                IEnumerable<int> branchUnitDepartmentIdList = PortalContext.CurrentUser.PermissionContext.DepartmentList.Select(x => x.BranchUnitDepartmentId);
                IEnumerable<int> employeeTypeList = PortalContext.CurrentUser.PermissionContext.EmployeeTypeList.Select(x => x.Id);


                employeeWorkGroupDetails =
                                      Context.VEmployeeWorkGroupDetails.Where(x => companyIdList.Contains(x.CompanyId) &&
                                      branchIdList.Contains(x.BranchId) &&
                                      branchUnitIdList.Contains(x.BranchUnitId) &&
                                      branchUnitDepartmentIdList.Contains(x.BranchUnitDepartmentId) &&
                                      employeeTypeList.Contains(x.EmployeeTypeId) &&
                                      (x.EmployeeCardId == searchFieldModel.SearchByEmployeeCardId || searchFieldModel.SearchByEmployeeCardId == null) &&
                                      (x.DepartmentSectionId == searchFieldModel.SearchByDepartmentSectionId || searchFieldModel.SearchByDepartmentSectionId == 0) &&
                                      (x.DepartmentLineId == searchFieldModel.SearchByDepartmentLineId || searchFieldModel.SearchByDepartmentLineId == 0) &&
                                      (x.WorkGroupId == searchFieldModel.SearchByWorkGroupId || searchFieldModel.SearchByWorkGroupId == 0)
                                      );

                if (searchFieldModel.SearchByCompanyId > 0 || searchFieldModel.SearchByBranchId > 0 || searchFieldModel.SearchByBranchUnitId > 0 || searchFieldModel.SearchByBranchUnitDepartmentId > 0)
                {
                    employeeWorkGroupDetails =
                        employeeWorkGroupDetails.Where(x => (x.CompanyId == searchFieldModel.SearchByCompanyId || searchFieldModel.SearchByCompanyId == 0)
                        && (x.BranchId == searchFieldModel.SearchByBranchId || searchFieldModel.SearchByBranchId == 0)
                        && (x.BranchUnitId == searchFieldModel.SearchByBranchUnitId || searchFieldModel.SearchByBranchUnitId == 0)
                        && (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId || searchFieldModel.SearchByBranchUnitDepartmentId == 0));
                }

                totalRecords = employeeWorkGroupDetails.Count();

                switch (model.sort)
                {
                    case "CompanyName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeWorkGroupDetails = employeeWorkGroupDetails
                                    .OrderByDescending(r => r.CompanyName);
                                break;
                            default:
                                employeeWorkGroupDetails = employeeWorkGroupDetails
                                    .OrderBy(r => r.CompanyName);
                                break;
                        }
                        break;
                    case "BranchName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeWorkGroupDetails = employeeWorkGroupDetails
                                    .OrderByDescending(r => r.BranchName);
                                break;
                            default:
                                employeeWorkGroupDetails = employeeWorkGroupDetails
                                    .OrderBy(r => r.BranchName);
                                break;
                        }
                        break;
                    case "UnitName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeWorkGroupDetails = employeeWorkGroupDetails
                                    .OrderByDescending(r => r.UnitName);
                                break;
                            default:
                                employeeWorkGroupDetails = employeeWorkGroupDetails
                                    .OrderBy(r => r.UnitName);
                                break;
                        }
                        break;
                    case "DepartmentName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeWorkGroupDetails = employeeWorkGroupDetails
                                    .OrderByDescending(r => r.DepartmentName);
                                break;
                            default:
                                employeeWorkGroupDetails = employeeWorkGroupDetails
                                    .OrderBy(r => r.DepartmentName);
                                break;
                        }
                        break;
                    case "EmployeeName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeWorkGroupDetails = employeeWorkGroupDetails
                                    .OrderByDescending(r => r.EmployeeName);
                                break;
                            default:
                                employeeWorkGroupDetails = employeeWorkGroupDetails
                                    .OrderBy(r => r.EmployeeName);
                                break;
                        }
                        break;
                    case "SectionName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeWorkGroupDetails = employeeWorkGroupDetails
                                    .OrderByDescending(r => r.SectionName);
                                break;
                            default:
                                employeeWorkGroupDetails = employeeWorkGroupDetails
                                    .OrderBy(r => r.EmployeeCardId);
                                break;
                        }
                        break;
                    case "LineName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeWorkGroupDetails = employeeWorkGroupDetails
                                    .OrderByDescending(r => r.LineName);
                                break;
                            default:
                                employeeWorkGroupDetails = employeeWorkGroupDetails
                                    .OrderBy(r => r.LineName);
                                break;
                        }
                        break;
                    case "AssignedDate":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeWorkGroupDetails = employeeWorkGroupDetails
                                    .OrderByDescending(r => r.AssignedDate);
                                break;
                            default:
                                employeeWorkGroupDetails = employeeWorkGroupDetails
                                    .OrderBy(r => r.AssignedDate);
                                break;
                        }
                        break;
                    default:
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeWorkGroupDetails = employeeWorkGroupDetails
                                    .OrderByDescending(r => r.EmployeeCardId);
                                break;
                            default:
                                employeeWorkGroupDetails = employeeWorkGroupDetails
                                    .OrderBy(r => r.EmployeeCardId);
                                break;
                        }

                        break;
                }
            }
            catch (Exception exception)
            {
               throw new Exception(exception.Message);
            }
            return employeeWorkGroupDetails.ToList();
        }

        public int SaveEmployeeWorkGroups(List<EmployeeWorkGroup> employeeWorkGroups)
        {
            var saveChanges = 0;
            try
            {
                Context.EmployeeWorkGroups.AddRange(employeeWorkGroups);
                saveChanges = Context.SaveChanges();
            }
            catch (Exception exception)
            {
                throw;

            }
            return saveChanges;
        }

        public List<VEmployeeWorkGroupDetail> GetEmployeeWorkGroupDetailBySearchKey(SearchFieldModel searchFieldModel)
        {

            IQueryable<VEmployeeWorkGroupDetail> employeeWorkGroupDetails;
            try
            {
                employeeWorkGroupDetails = Context.VEmployeeWorkGroupDetails
                .Where(
                        x =>

                            (x.EmployeeCardId == searchFieldModel.SearchByEmployeeCardId || searchFieldModel.SearchByEmployeeCardId == null) &&
                            (x.CompanyId == searchFieldModel.SearchByCompanyId || searchFieldModel.SearchByCompanyId == 0)
                            &&
                            (x.BranchId == searchFieldModel.SearchByBranchId || searchFieldModel.SearchByBranchId == 0)
                            &&
                            (x.BranchUnitId == searchFieldModel.SearchByBranchUnitId || searchFieldModel.SearchByBranchUnitId == 0)
                            &&
                              (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId || searchFieldModel.SearchByBranchUnitDepartmentId == 0)
                            &&
                          (x.DepartmentSectionId == searchFieldModel.SearchByDepartmentSectionId || searchFieldModel.SearchByDepartmentSectionId == 0)
                            &&
                               (x.DepartmentLineId == searchFieldModel.SearchByDepartmentLineId || searchFieldModel.SearchByDepartmentLineId == 0)
                            &&
                            (x.WorkGroupId == searchFieldModel.SearchByWorkGroupId || searchFieldModel.SearchByWorkGroupId == 0)).OrderBy(x=>x.EmployeeCardId);
         
          
            }
            catch (Exception exception)
            {

                throw;
            }
            return employeeWorkGroupDetails.ToList();
        }
    }
}
