using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeInOutProcessRepository : IRepository<EmployeeInOut>
    {

        List<EmployeeInOut> GetEmployeeInOutProcessedInfo(int startPage, int pageSize,  EmployeeInOut model, SearchFieldModel searchFieldModel);

        List<EmployeesForInOutProcessModel> GetEmployeeForInOutProcess(SearchFieldModel searchFieldModel,
            EmployeeInOut model);

        int ProcessBulkEmployeeInOut(SearchFieldModel searchFieldModel, EmployeeInOut model);
    }
}