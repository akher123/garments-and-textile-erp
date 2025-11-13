using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class BankAccountTypeRepository : Repository<BankAccountType>, IBankAccountTypeRepository
    {
        public BankAccountTypeRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public List<BankAccountType> GetBankAccountTypes(int startPage, int pageSize, BankAccountType model, out int totalRecords)
        {
            IQueryable<BankAccountType> bankAccountTypes;
            try
            {
                bankAccountTypes = Context.BankAccountTypes
                    .Where(x => x.IsActive == true && ((x.AccountType.Replace(" ", "")
                        .ToLower().Contains(model.AccountType.Replace(" ", "")
                            .ToLower())) || String.IsNullOrEmpty(model.AccountType)));
                totalRecords = bankAccountTypes.Count();
                switch (model.sortdir)
                {
                    case "DESC":
                        bankAccountTypes = bankAccountTypes.OrderByDescending(r => r.AccountType)
                            .Skip(startPage * pageSize)
                            .Take(pageSize);
                        break;
                    default:
                        bankAccountTypes = bankAccountTypes.OrderBy(r => r.AccountType)
                          .Skip(startPage * pageSize)
                          .Take(pageSize);
                        break;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return bankAccountTypes.ToList();
        }
    }
}
