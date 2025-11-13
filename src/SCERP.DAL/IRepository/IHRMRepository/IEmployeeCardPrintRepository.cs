using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;


namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeCardPrintRepository : IRepository<Employee>
    {

        List<EmployeeCardInfo> GetCardBackInfo(int companyId, int language, int noofCard);

        List<EmployeeCardPrintModel> GetEmployeeIDCardInfoInEnglishByPaging(int startPage, int pageSize, out int totalRecords,
            Employee model, SearchFieldModel searchFieldModel);

        List<EmployeeCardPrintModel> GetEmployeeIDCardInfoInBengaliByPaging(int startPage, int pageSize, out int totalRecords,
            Employee model, SearchFieldModel searchFieldModel);

        IQueryable<VEmployeeIDCardInfoInEnglish> GetEmployeeIDCardInfoInEnglishBySearchKey(
            SearchFieldModel searchFieldModel);

        IQueryable<VEmployeeIDCardInfoInBengali> GetEmployeeIDCardInfoInBengaliBySearchKey(
            SearchFieldModel searchFieldModel);

        List<EmployeeCardPrintModel> GetEmployeeIDCardInfoInEnglish(List<Guid> employeeIdList,
            SearchFieldModel searchFieldModel);

        List<EmployeeCardPrintModel> GetEmployeeIDCardInfoInBengali(List<Guid> employeeIdList,
            SearchFieldModel searchFieldModel);

    }
}
