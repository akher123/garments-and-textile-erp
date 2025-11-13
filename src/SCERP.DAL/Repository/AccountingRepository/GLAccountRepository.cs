using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.AccountingRepository
{
    public class GLAccountRepository : Repository<Acc_GLAccounts>, IGLAccountRepository
    {

        public GLAccountRepository(SCERPDBContext context) : base(context)
        {

        }

        public decimal GetMaxGlControlCode(int newParentCode)
        {
            decimal max = Context.Database.SqlQuery<decimal>("SELECT ISNULL(MAX(AccountCode),0) + 1 AS ControlCode FROM Acc_GLAccounts WHERE ControlCode=" + newParentCode + "").SingleOrDefault();
            return max;
        }     

        public List<Acc_GLAccounts> GetHiddenGlAccounts()
        {
            return Context.Database.SqlQuery<Acc_GLAccounts>("SPGetHiddenGlAccounts").ToList();
        }
    }
}
