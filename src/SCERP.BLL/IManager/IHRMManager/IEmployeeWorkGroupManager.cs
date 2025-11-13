using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
   public interface IEmployeeWorkGroupManager
    {
       int SaveEmployeeWorkGroups(List<EmployeeWorkGroup> employeeWorkGroups);
  
       int EditEmployeeWorkGroups(List<EmployeeWorkGroup> employeeWorkGroups);

       List<VEmployeeCompanyInfoDetail> GetAllUnAssignedEmployeeWorkGroup(int startPage, int pageSize,  out int totalRecords,SearchFieldModel searchFieldModel,
           EmployeeWorkGroup model);
       List<EmployeeWorkGroup> GetAllAssignedEmployeeWorkGroup(SearchFieldModel searchFieldModel);
       int DeleteEmployeeWorkGroup(int? id);




       List<VEmployeeWorkGroupDetail> GetAllEmployeeWorkGroupByPaging(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel, EmployeeWorkGroup model);
       List<VEmployeeWorkGroupDetail> GetEmployeeWorkGroupDetailBySearchKey(SearchFieldModel searchField);
    }
}
