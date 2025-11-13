using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeInOutModelProcessRepository : IRepository<EmployeeInOutModel>
    {

        List<EmployeeInOutModel> GetEmployeeInOutModelProcessedInfo(int startPage, int pageSize, EmployeeInOutModel model, SearchFieldModel searchFieldModel);

        List<EmployeesForInOutModelProcessModel> GetEmployeesForInOutModelProcess(SearchFieldModel searchFieldModel,EmployeeInOutModel model);

        int ProcessBulkEmployeeInOutModel(SearchFieldModel searchFieldModel, EmployeeInOutModel model);
    }
}