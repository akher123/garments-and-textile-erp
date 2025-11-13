using System;
using System.Runtime.Remoting.Messaging;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.DAL.Repository.AccountingRepository;
using System.Linq;
using SCERP.Model;
using System.Collections.Generic;


namespace SCERP.BLL.Manager.AccountingManager
{
    public class CashVoucherEntryManager : BaseManager, ICashVoucherEntryManager
    {
        private ICashVoucherEntryRepository CashVoucherEntryRepository = null;

        public CashVoucherEntryManager(SCERPDBContext context)
        {
            this.CashVoucherEntryRepository = new CashVoucherEntryRepository(context);
        }

        public string SaveMaster(Acc_VoucherMaster voucherMaster)
        {
            if (voucherMaster.Id > 0)
            {
                CashVoucherEntryRepository.Edit(voucherMaster);
                CashVoucherEntryRepository.DeleteDetail(voucherMaster.Id);
                return "Success";
            }
            return CashVoucherEntryRepository.SaveMaster(voucherMaster);        
        }

        public string SaveDetail(Acc_VoucherDetail voucherDetail)
        {
            return CashVoucherEntryRepository.SaveDetail(voucherDetail);
        }
      
        public List<string> GetAccountName()
        {
            return CashVoucherEntryRepository.GetAccountNames();
        }

        public List<string> GetCashAccountNames()
        {
            return CashVoucherEntryRepository.GetCashAccountNames();
        }

        public string GetCashInHand()
        {
            return CashVoucherEntryRepository.GetCashInHand();
        }
        public string GetPeriodName()
        {
            return CashVoucherEntryRepository.GetPeriodName();
        }

        public int GetPeriodId()
        {
            return CashVoucherEntryRepository.GetPeriodId();
        }

        public IQueryable<Acc_CompanySector> GetAllCompanySector()
        {
            return CashVoucherEntryRepository.GetAllCompanySector();
        }

        public string GetFinancialPeriodById(int fpId)
        {
            return CashVoucherEntryRepository.GetFinancialPeriodById(fpId);
        }

        public Acc_VoucherMaster GetAccVoucherMasterById(long Id)
        {
            return CashVoucherEntryRepository.GetAccVoucherMasterById(Id);
        }

        public IQueryable<Acc_CostCentre> GetAllCostCentres(int sectorId)
        {
            return CashVoucherEntryRepository.GetAllCostCentres(sectorId);
        }

        public IQueryable<Acc_VoucherDetail> GetCashVoucherDetail(long Id)
        {
            return CashVoucherEntryRepository.GetCashVoucherDetail(Id);
        }

        public string CheckGLHeadValidation(string glHead)
        {
            return CashVoucherEntryRepository.CheckGLHeadValidation((glHead));
        }

        public long GetVoucherNo(int fiscalPeriodId, string voucherType)
        {
            return CashVoucherEntryRepository.GetVoucherNo(fiscalPeriodId, voucherType);
        }

        public int GetAccountId(decimal accountCode)
        {
            return CashVoucherEntryRepository.GetAccountId(accountCode);
        }

        public string GetClosingBalance(Acc_ClosingBalanceViewModel obj)
        {
            return CashVoucherEntryRepository.GetClosingBalance(obj);
        }

        public int GetCashInHandGLId()
        {
            return CashVoucherEntryRepository.GetCashInHandGLId();
        }
    }
}
