using System;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.DAL.Repository.AccountingRepository;
using System.Linq;
using SCERP.Model;
using System.Collections.Generic;


namespace SCERP.BLL.Manager.AccountingManager
{
    public class BankVoucherEntryManager : BaseManager, IBankVoucherEntryManager
    {
        private IBankVoucherEntryRepository BankVoucherEntryRepository = null;

        public BankVoucherEntryManager(SCERPDBContext context)
        {
            this.BankVoucherEntryRepository = new BankVoucherEntryRepository(context);
        }

        public string SaveMaster(Acc_VoucherMaster voucherMaster)
        {
            if (voucherMaster.Id > 0)
            {
                BankVoucherEntryRepository.Edit(voucherMaster);
                BankVoucherEntryRepository.DeleteDetail(voucherMaster.Id);
                return "Success";
            }
            return BankVoucherEntryRepository.SaveMaster(voucherMaster);
        }

        public string SaveDetail(Acc_VoucherDetail voucherDetail)
        {
            return BankVoucherEntryRepository.SaveDetail(voucherDetail);
        }

        public IQueryable<Acc_VoucherDetail> GetBankVoucherDetail(long Id)
        {
            return BankVoucherEntryRepository.GetBankVoucherDetail(Id);
        }
        public List<string> GetAccountName()
        {
            return BankVoucherEntryRepository.GetAccountNames();
        }

        public string GetFinancialPeriodById(int fpId)
        {
            return BankVoucherEntryRepository.GetFinancialPeriodById(fpId);
        }

        public Acc_VoucherMaster GetAccVoucherMasterById(long Id)
        {
            return BankVoucherEntryRepository.GetAccVoucherMasterById(Id);
        }
        public List<string> GetBankAccountNames()
        {
            return BankVoucherEntryRepository.GetBankAccountNames();
        }

        public string GetBankInHand()
        {
            return BankVoucherEntryRepository.GetBankInHand();
        }
        public string GetPeriodName()
        {
            return BankVoucherEntryRepository.GetPeriodName();
        }

        public int GetPeriodId()
        {
            return BankVoucherEntryRepository.GetPeriodId();
        }

        public IQueryable<Acc_CompanySector> GetAllCompanySector()
        {
            return BankVoucherEntryRepository.GetAllCompanySector();
        }

        public IQueryable<Acc_CostCentre> GetAllCostCentres(int sectorId)
        {
            return BankVoucherEntryRepository.GetAllCostCentres(sectorId);
        }

        public string CheckGLHeadValidation(string glHead)
        {
            return BankVoucherEntryRepository.CheckGLHeadValidation((glHead));
        }

        public long GetVoucherNo(int fiscalPeriodId, string voucherType)
        {
            return BankVoucherEntryRepository.GetVoucherNo(fiscalPeriodId, voucherType);
        }

        public int GetAccountId(decimal accountCode)
        {
            return BankVoucherEntryRepository.GetAccountId(accountCode);
        }

    }
}
