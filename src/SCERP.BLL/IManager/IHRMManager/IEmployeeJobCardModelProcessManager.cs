using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeJobCardModelProcessManager
    {
        List<EmployeeJobCardModel> GetEmployeeJobCardModelProcessedInfo(int startPage, int pageSize, EmployeeJobCardModel model, SearchFieldModel searchFieldModel);

        List<EmployeesForJobCardModelProcessModel> GetEmployeesForJobCardModelProcess(SearchFieldModel searchFieldModel, EmployeeJobCardModel model);

        int ProcessBulkEmployeeJobCardModel(SearchFieldModel searchFieldModel, EmployeeJobCardModel model);
    }
}
