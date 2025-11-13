
using System.Collections.Generic;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IBankAccountTypeRepository : IRepository<BankAccountType>
    {
        List<BankAccountType> GetBankAccountTypes(int startPage, int pageSize, BankAccountType model, out int totalRecords);
    }
}
