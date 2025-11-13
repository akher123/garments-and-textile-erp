using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IAccountingManager
{
    public interface IBankReconcilationListManager
    {
        List<Acc_BankReconcilationList> GetAllReconcilationList(int page, int records, string sort,
            int? fpId, DateTime? FromDate, DateTime? ToDate, string AccountName, string SectorId);

        Acc_BankReconcilationMaster GetBankReconcilationMasterById(long? id);
        void DeleteBankReconcilationMaster(Acc_BankReconcilationMaster BankReconcilationMaster);
    }
}
