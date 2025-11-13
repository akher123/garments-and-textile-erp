using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeWorkGroupRepository : IRepository<EmployeeWorkGroup>
    {
        List<EmployeeWorkGroup> GetAllAssignedEmployeeWorkGroup(SearchFieldModel searchFieldModel);
        List<VEmployeeCompanyInfoDetail> GetAllUnAssignedEmployeeWorkGroup(int startPage, int pageSize, out int totalRecords,SearchFieldModel searchFieldModel, EmployeeWorkGroup model);


        List<VEmployeeWorkGroupDetail> GetAllEmployeeWorkGroupByPaging(int startPage, int pageSize, out int totalRecords, EmployeeWorkGroup model, SearchFieldModel searchFieldModel);
        int SaveEmployeeWorkGroups(List<EmployeeWorkGroup> employeeWorkGroups);
        List<VEmployeeWorkGroupDetail> GetEmployeeWorkGroupDetailBySearchKey(SearchFieldModel searchField);
    }
}
