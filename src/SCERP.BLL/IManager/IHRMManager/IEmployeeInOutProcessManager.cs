using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeInOutModelProcessManager
    {
        List<EmployeeInOutModel> GetEmployeeInOutModelProcessedInfo(int startPage, int pageSize, EmployeeInOutModel model, SearchFieldModel searchFieldModel);

        List<EmployeesForInOutModelProcessModel> GetEmployeeForInOutModelProcess(SearchFieldModel searchFieldModel,
            EmployeeInOutModel model);

        int ProcessBulkEmployeeInOutModel(SearchFieldModel searchFieldModel, EmployeeInOutModel model);
    }
}
