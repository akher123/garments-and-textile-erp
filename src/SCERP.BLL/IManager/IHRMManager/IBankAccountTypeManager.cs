using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IBankAccountTypeManager
    {
        List<BankAccountType> GetBankAccountTypes(int startPage, int pageSize, BankAccountType model, out int totalRecords);
        BankAccountType GetBankAccountTypeById(int bankAccountTypeId);
        bool IsExistBankAccountType(BankAccountType model);
        int EditBankAccountType(BankAccountType model);
        int SaveBankAccountType(BankAccountType model);
        int DeleteBankAccountType(int bankAccountTypeId);
    }
}
