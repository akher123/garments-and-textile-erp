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
    public class ContraVoucherEntryManager : BaseManager, IContraVoucherEntryManager
    {
        private IContraVoucherEntryRepository ContraVoucherEntryRepository = null;

        public ContraVoucherEntryManager(SCERPDBContext context)
        {
            this.ContraVoucherEntryRepository = new ContraVoucherEntryRepository(context);
        }

        public string SaveMaster(Acc_VoucherMaster voucherMaster)
        {
            if (voucherMaster.Id > 0)
            {
                ContraVoucherEntryRepository.Edit(voucherMaster);
                ContraVoucherEntryRepository.DeleteDetail(voucherMaster.Id);
                return "Success";
            }
            return ContraVoucherEntryRepository.SaveMaster(voucherMaster);
        }

        public string SaveDetail(Acc_VoucherDetail voucherDetail)
        {
            return ContraVoucherEntryRepository.SaveDetail(voucherDetail);
        }

        public List<string> GetAccountNames(string accountType)
        {
            return ContraVoucherEntryRepository.GetAccountNames(accountType);
        }

        public string GetPeriodName()
        {
            return ContraVoucherEntryRepository.GetPeriodName();
        }

        public Acc_VoucherMaster GetAccVoucherMasterById(long Id)
        {
            return ContraVoucherEntryRepository.GetAccVoucherMasterById(Id);
        }

        public int GetPeriodId()
        {
            return ContraVoucherEntryRepository.GetPeriodId();
        }

        public IQueryable<Acc_VoucherDetail> GetContraVoucherDetail(long Id)
        {
            return ContraVoucherEntryRepository.GetContraVoucherDetail(Id);
        }

        public IQueryable<Acc_CompanySector> GetAllCompanySector()
        {
            return ContraVoucherEntryRepository.GetAllCompanySector();
        }

        public string GetFinancialPeriodById(int fpId)
        {
            return ContraVoucherEntryRepository.GetFinancialPeriodById(fpId);
        }

        public IQueryable<Acc_CostCentre> GetAllCostCentres(int sectorId)
        {
            return ContraVoucherEntryRepository.GetAllCostCentres(sectorId);
        }

        public string CheckGLHeadValidation(string glHead)
        {
            return ContraVoucherEntryRepository.CheckGLHeadValidation((glHead));
        }

        public long GetVoucherNo(int fiscalPeriodId, string voucherType)
        {
            return ContraVoucherEntryRepository.GetVoucherNo(fiscalPeriodId, voucherType);
        }

        public int GetAccountId(decimal accountCode)
        {
            return ContraVoucherEntryRepository.GetAccountId(accountCode);
        }

        public string GetClosingBalance(Acc_ClosingBalanceViewModel obj)
        {
            return ContraVoucherEntryRepository.GetClosingBalance(obj);
        }

        //public bool CheckVoucherLimit(string VoucherType, string Amount)
        //{
        //    return ContraVoucherEntryRepository.CheckVoucherLimit(VoucherType, Amount);
        //}
    }
}
