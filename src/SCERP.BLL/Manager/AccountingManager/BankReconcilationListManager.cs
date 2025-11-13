using System;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.DAL.Repository.AccountingRepository;
using System.Linq;
using SCERP.Model;
using System.Collections.Generic;

namespace SCERP.BLL.Manager.AccountingManager
{
    public class BankReconcilationListManager : BaseManager, IBankReconcilationListManager
    {
        private IBankReconcilationListRepository BankReconcilationListRepository = null;

        public BankReconcilationListManager(SCERPDBContext context)
        {
            this.BankReconcilationListRepository = new BankReconcilationListRepository(context);
        }

        public List<Acc_BankReconcilationList> GetAllReconcilationList(int page, int records, string sort,
            int? fpId, DateTime? FromDate, DateTime? ToDate, string AccountName, string SectorId)
        {
            return BankReconcilationListRepository.GetAllReconcilationList(page, records, sort,
                fpId, FromDate, ToDate, AccountName, SectorId);
        }

        public Acc_BankReconcilationMaster GetBankReconcilationMasterById(long? id)
        {
            return BankReconcilationListRepository.GetBankReconcilationMasterById(id);
        }

        public void DeleteBankReconcilationMaster(Acc_BankReconcilationMaster BankReconcilationMaster)
        {
            BankReconcilationMaster.IsActive = false;
            BankReconcilationListRepository.Edit(BankReconcilationMaster);
        }
    }
}
