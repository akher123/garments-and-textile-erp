using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.PayrollRepository
{
    public class SalaryMappingRepository : Repository<Acc_SalaryMapping>, ISalaryMappingRepository
    {
        public SalaryMappingRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public List<SalaryHead> GetAllSalaryHead()
        {
            return Context.SalaryHeads.Where(p => p.IsActive == true).OrderBy(p => p.Id).ToList();
        }

        public int GetAccountId(decimal AccountCode)
        {
            var AccountId = (from p in Context.Acc_GLAccounts
                             where p.IsActive == true && p.AccountCode == AccountCode
                             select p.Id).FirstOrDefault();

            return AccountId;
        }

        public List<SalaryHead> GetSalaryHead()
        {
            return Context.SalaryHeads.Where(p => p.IsActive == true).ToList();
        }

        public string GetAccountNamesById(int Id)
        {
            var temp = Context.Acc_GLAccounts.FirstOrDefault(p => p.Id == Id && p.IsActive == true);
            return (temp.AccountCode + "-" + temp.AccountName).ToLower();
        }
    }
}
