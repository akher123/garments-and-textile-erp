using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IAccountingManager
{
    public interface IControlAccountManager
    {
        List<Acc_ControlAccounts> GetAllControlAccounts();

        List<Acc_GLAccounts> GetAllGLAccounts(string searchKey);

        List<Acc_GLAccounts> GetGLAccountsByControlCode(string controlCode);

        int GetMaxControlCode(int parentCode);

        int SaveControlAccount(Acc_ControlAccounts accControlAccount);

        int SaveControlAccountTransfer(Acc_ControlAccounts accControlAccount);

        decimal GetMaxGlControlCode(int newParentCode);

        Acc_ControlAccounts GetControlAccountsById(int id);

        Acc_GLAccounts GetGLAccountsById(int id);

        int EditControlAccount(Acc_ControlAccounts accControlAccount);

        int SaveglAccount(Acc_GLAccounts glAccount);

        int EditglAccount(Acc_GLAccounts glAccount);

        int DeleteControlAccount(int id);

        int DeleteGLAccount(int id);

        List<Acc_CompanySector> GetAllCompanySector();

        int SavePermitedChartOfAccounts(List<Acc_PermitedChartOfAccount> paOfAccounts);

        List<Acc_PermitedChartOfAccount> GetPermitedChartOfAccount(int companySectorId);

        bool CheckExistVoucherByGLId(int controlLevel, int id, ref string message);

        bool CheckExistingName(Acc_GLAccounts glAccount);

        bool CheckExistingName(Acc_ControlAccounts controlAccounts);

        int GetGLAccountIdByCode(string accountCode);

        int GetControlIdByCode(string controlCode);

        string GetControlNameByCode(string controlcode);

        int ControltoSubGroupChange(string SubGroupCode, string ControlCode);

        int GLtoControlChange(string GLCode, string ControlCode);

        int GLtoSubGroupChange(string GLCode, string SubGroup);

    }
}
