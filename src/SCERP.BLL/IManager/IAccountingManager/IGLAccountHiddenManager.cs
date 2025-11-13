using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.AccountingModel;

namespace SCERP.BLL.IManager.IAccountingManager
{
    public interface IGLAccountHiddenManager
    {
        List<Acc_GLAccounts_Hidden> GetAllGLAccountHiddens(int startPage, int pageSize, out int totalRecordsHidden);

        List<Acc_GLAccounts_Hidden> GetAllGLAccountVisibles(int startPage, int pageSize, out int totalRecordsVisible);

        Acc_GLAccounts_Hidden GetGLAccountHiddenById(int? id);

        string SaveGLAccountHidden(Acc_GLAccounts_Hidden GLAccountHidden);

        void DeleteGLAccountHidden(Acc_GLAccounts_Hidden GLAccountHidden);

        void MakeGLAccountHidden(Acc_GLAccounts_Hidden GLAccountHidden);

        void MakeGLAccountVisible(Acc_GLAccounts_Hidden GLAccountHidden);

        string SaveStatus(bool status);

        int GetStatus();
    }
}
