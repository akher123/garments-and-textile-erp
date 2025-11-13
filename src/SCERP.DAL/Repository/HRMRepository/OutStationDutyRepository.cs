
using System.Collections.Generic;
using System.Linq;

using System.Data.Entity;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class OutStationDutyRepository : Repository<OutStationDuty>, IOutStationDutyRepository
    {
        public OutStationDutyRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public List<VOutStationDutyDetail> GetAllOutStationDutyDetail(int startPage, int pageSize, OutStationDuty model, SearchFieldModel searchFieldModel,
            out int totalRecords)
        {
            var outStationDutyDetails = GetOutStationDutyBySearchKey(searchFieldModel).AsQueryable();
            totalRecords = outStationDutyDetails.Count();
            
            switch (model.sort)
            {
                case "EmployeeName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            outStationDutyDetails = outStationDutyDetails
                                .OrderByDescending(r => r.EmployeeName)
                                .Skip(startPage * pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            outStationDutyDetails = outStationDutyDetails
                                 .OrderBy(r => r.EmployeeName)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                case "DutyDate":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            outStationDutyDetails = outStationDutyDetails
                                .OrderByDescending(r => r.DutyDate)
                                .Skip(startPage * pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            outStationDutyDetails = outStationDutyDetails
                                 .OrderBy(r => r.DutyDate)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;

                case "EmployeeCardId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            outStationDutyDetails = outStationDutyDetails
                                .OrderByDescending(r => r.EmployeeCardId)
                                .Skip(startPage * pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            outStationDutyDetails = outStationDutyDetails
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
                            outStationDutyDetails = outStationDutyDetails
                             .OrderByDescending(r => r.Designation)
                             .Skip(startPage * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            outStationDutyDetails = outStationDutyDetails
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
                            outStationDutyDetails = outStationDutyDetails
                            .OrderByDescending(r => r.CompanyName)
                            .Skip(startPage * pageSize)
                            .Take(pageSize);
                            break;
                        default:
                            outStationDutyDetails = outStationDutyDetails
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
                            outStationDutyDetails = outStationDutyDetails
                            .OrderByDescending(r => r.BranchName)
                            .Skip(startPage * pageSize)
                            .Take(pageSize);
                            break;
                        default:
                            outStationDutyDetails = outStationDutyDetails
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
                            outStationDutyDetails = outStationDutyDetails
                            .OrderByDescending(r => r.UnitName)
                            .Skip(startPage * pageSize)
                            .Take(pageSize);
                            break;
                        default:
                            outStationDutyDetails = outStationDutyDetails
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
                            outStationDutyDetails = outStationDutyDetails
                            .OrderByDescending(r => r.DepartmentName)
                            .Skip(startPage * pageSize)
                            .Take(pageSize);
                            break;
                        default:
                            outStationDutyDetails = outStationDutyDetails
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
                            outStationDutyDetails = outStationDutyDetails
                            .OrderByDescending(r => r.SectionName)
                            .Skip(startPage * pageSize)
                            .Take(pageSize);
                            break;
                        default:
                            outStationDutyDetails = outStationDutyDetails
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
                            outStationDutyDetails = outStationDutyDetails
                            .OrderByDescending(r => r.LineName)
                            .Skip(startPage * pageSize)
                            .Take(pageSize);
                            break;
                        default:
                            outStationDutyDetails = outStationDutyDetails
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
                            outStationDutyDetails = outStationDutyDetails
                           .OrderByDescending(r => r.EmployeeCardId)
                           .Skip(startPage * pageSize)
                           .Take(pageSize);
                            break;
                        default:
                            outStationDutyDetails = outStationDutyDetails
                        .OrderBy(r => r.EmployeeCardId)
                        .Skip(startPage * pageSize)
                        .Take(pageSize);
                            break;
                    }

                    break;
            }

            return outStationDutyDetails.ToList();
        }

        public OutStationDuty GetOutStationDutyById(int outStationDutyId)
        {
            OutStationDuty outStationDuty = Context.OutStationDutys.Include(x => x.Employee).FirstOrDefault(x => x.OutStationDutyId == outStationDutyId);
            return outStationDuty;
        }

        public List<VOutStationDutyDetail> GetOutStationDutyBySearchKey(SearchFieldModel searchFieldModel)
        {
            IEnumerable<int> companyIdList = PortalContext.CurrentUser.PermissionContext.CompanyList.Select(x => x.CompanyId);
            IEnumerable<int> branchIdList = PortalContext.CurrentUser.PermissionContext.BranchList.Select(x => x.BranchId);
            IEnumerable<int> branchUnitIdList = PortalContext.CurrentUser.PermissionContext.UnitList.Select(x => x.BranchUnitId);
            IEnumerable<int> branchUnitDepartmentIdList = PortalContext.CurrentUser.PermissionContext.DepartmentList.Select(x => x.BranchUnitDepartmentId);
            IEnumerable<int> employeeTypeList = PortalContext.CurrentUser.PermissionContext.EmployeeTypeList.Select(x => x.Id);

            var outStationDutyDetails =
                Context.VOutStationDutyDetails.Where(x => companyIdList.Contains(x.CompanyId) &&
                                                          branchIdList.Contains(x.BranchId) &&
                                                          branchUnitIdList.Contains(x.BranchUnitId) &&
                                                          branchUnitDepartmentIdList.Contains(x.BranchUnitDepartmentId) &&
                                                          employeeTypeList.Contains(x.EmployeeTypeId) &&
                                                          (x.EmployeeCardId == searchFieldModel.SearchByEmployeeCardId ||
                                                          searchFieldModel.SearchByEmployeeCardId == null) &&
                                                          (x.DepartmentSectionId == searchFieldModel.SearchByDepartmentSectionId || searchFieldModel.SearchByDepartmentSectionId == 0) &&
                                                          (x.DepartmentLineId == searchFieldModel.SearchByDepartmentLineId || searchFieldModel.SearchByDepartmentLineId == 0) 
                                                          && ((x.DutyDate >= searchFieldModel.StartDate ||
                                                          searchFieldModel.StartDate == null)
                                                          && (x.DutyDate <= searchFieldModel.EndDate ||
                                                          searchFieldModel.EndDate == null))
                                                          );


            if (searchFieldModel.SearchByCompanyId > 0 || searchFieldModel.SearchByBranchId > 0 || searchFieldModel.SearchByBranchUnitId > 0 || searchFieldModel.SearchByBranchUnitDepartmentId > 0)
            {
                outStationDutyDetails =
                    outStationDutyDetails.Where(x => (x.CompanyId == searchFieldModel.SearchByCompanyId || searchFieldModel.SearchByCompanyId == 0)
                    && (x.BranchId == searchFieldModel.SearchByBranchId || searchFieldModel.SearchByBranchId == 0)
                    && (x.BranchUnitId == searchFieldModel.SearchByBranchUnitId || searchFieldModel.SearchByBranchUnitId == 0)
                    && (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId || searchFieldModel.SearchByBranchUnitDepartmentId == 0));
            }

            return outStationDutyDetails.OrderBy(x => x.EmployeeCardId).ThenByDescending(x => x.DutyDate).ToList();
        }
    }
}
