using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeBankInfoManager
    {

        List<EmployeeBankInfo> GetEmployeeBankInfoByEmployeeId(Guid employeeId);

        EmployeeBankInfo GetEmployeeBankInfoById(int id);

        List<BankAccountType> GetAllBankAccountTypes();

        int SaveEmployeeBankInfo(EmployeeBankInfo employeeBankInfo);

        int EditEmployeeBankInfo(EmployeeBankInfo employeeBankInfo);

        int DeleteEmployeeBankInfo(EmployeeBankInfo employeeBankInfo);

        BankAccountType GeEmployeeBankAccountTypeById(int? id);

        EmployeeBankInfo GetEmployeeBankInfoById(Guid? employeeId, int? id);

        EmployeeBankInfo GetLatestEmployeeBankInfoByEmployeeGuidId(Guid employeeId);

        int UpdateEmployeeBankInfoDate(EmployeeBankInfo employeeBankInfo);

    }
}
