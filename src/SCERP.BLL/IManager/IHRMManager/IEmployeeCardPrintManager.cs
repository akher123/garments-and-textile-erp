using SCERP.Model;
using SCERP.Model.Custom;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeCardPrintManager
    {
      
        List<EmployeeCardPrintModel> GetEmployeeIDCardInfoByPaging(int startPage, int pageSize, out int totalRecords,
            Employee model, SearchFieldModel searchFieldModel);

        List<EmployeeCardPrintModel> GetEmployeeIDCardInfo(List<Guid> employeeIdList, SearchFieldModel searchFieldModel);

        List<EmployeeCardInfo> GetCardBackInfo(int companyId, int language, int noofCard);
    }
}
