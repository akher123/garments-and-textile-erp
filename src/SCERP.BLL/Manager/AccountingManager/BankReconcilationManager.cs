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
    public class BankReconcilationManager : BaseManager, IBankReconcilationManager
    {
        private IBankReconcilationRepository BankReconcilationRepository = null;

        public BankReconcilationManager(SCERPDBContext context)
        {
            this.BankReconcilationRepository = new BankReconcilationRepository(context); 
        }

        public List<VoucherList> GetAllVoucherList(int page, int records, string sort,int? fpId, DateTime? FromDate, DateTime? ToDate, string AccountName, string SectorId)
        {
            List<VoucherList> allVoucherList = null;
            try
            {
               allVoucherList = BankReconcilationRepository.GetAllVoucherList(page, records, sort, fpId, FromDate, ToDate, AccountName, SectorId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                allVoucherList = null;
            }
            return allVoucherList;
        }

        public IQueryable<Acc_FinancialPeriod> GetFinancialPeriod()
        {
            return BankReconcilationRepository.GetFinancialPeriod();
        }

        public IQueryable<Acc_CompanySector> GetAllCompanySector()
        {
            return BankReconcilationRepository.GetAllCompanySector();
        }
        public Acc_VoucherMaster GetVoucherMasterById(long? id)
        {
            return BankReconcilationRepository.GetVoucherMasterById(id);
        }

        public string CheckPeriodFromToDate(int Id, DateTime fromDate, DateTime toDate)
        {
            return BankReconcilationRepository.CheckPeriodFromToDate(Id, fromDate, toDate);
        }

        public List<DateTime> GetPeriodFromToDate(int Id)
        {
            return BankReconcilationRepository.GetPeriodFromToDate(Id);
        }

        public void DeleteVoucherList(Acc_VoucherMaster voucherMaster)
        {
            voucherMaster.IsActive = false;
            BankReconcilationRepository.Edit(voucherMaster);
        }

        public List<string> GetAccountNames()
        {
            return BankReconcilationRepository.GetAccountNames();
        }

        public string SaveBankReconMaster(string sectorId, string fpId, string accountName, DateTime? fromDate,
            DateTime? toDate)
        {

            return BankReconcilationRepository.SaveBankReconMaster(sectorId, fpId, accountName, fromDate,
                toDate);
        }

        public string SaveBankReconDetail(List<long> detail)
        {
            return BankReconcilationRepository.SaveBankReconDetail(detail);
        }

        public string SaveManualDetail(Acc_BankVoucherManual bankVoucherManual)
        {
            return BankReconcilationRepository.SaveManualDetail(bankVoucherManual);
        }

        public string CheckDuplicate(List<string> voucherId)
        {
            return BankReconcilationRepository.CheckDuplicate(voucherId);
        }
    }
}
