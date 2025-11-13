using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public  interface IPermitedChartOfAccountRepository:IRepository<Acc_PermitedChartOfAccount>
    {
        List<Acc_PermitedChartOfAccount> GetPermitedChartOfAccount(int companySectorId);
        bool CheckExistVoucherByGLId(int controlLevel, int id, ref string message);
        bool CheckExistingName(Acc_GLAccounts glAccount);
        bool CheckExistingName(Acc_ControlAccounts controlAccounts);
        int GetGLAccountIdByCode(string accountCode);
        int GetControlIdByCode(string controlCode);
    }
}
