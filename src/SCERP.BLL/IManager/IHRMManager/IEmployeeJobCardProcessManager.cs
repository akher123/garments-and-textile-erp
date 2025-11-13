using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeJobCardProcessManager
    {
        List<EmployeeJobCard> GetEmployeeJobCardProcessedInfo(int startPage, int pageSize, EmployeeJobCard model, SearchFieldModel searchFieldModel);

        List<EmployeesForJobCardProcessModel> GetEmployeesForJobCardProcess(SearchFieldModel searchFieldModel, EmployeeJobCard model);

        int ProcessBulkEmployeeJobCard(SearchFieldModel searchFieldModel, EmployeeJobCard model);
    }
}
