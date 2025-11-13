using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public interface IGLAccountRepository:IRepository<Acc_GLAccounts>
    {
        decimal GetMaxGlControlCode(int newParentCode);
        List<Acc_GLAccounts> GetHiddenGlAccounts();
    }
}
