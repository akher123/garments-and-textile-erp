using System;
using System.Web.ModelBinding;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.DAL.Repository.AccountingRepository;
using System.Linq;
using SCERP.Model;
using System.Collections.Generic;
using SCERP.Model.AccountingModel;


namespace SCERP.BLL.Manager.AccountingManager
{
    public class JournalVoucherEntryManager : BaseManager, IJournalVoucherEntryManager
    {
        private IJournalVoucherEntryRepository JournalVoucherEntryRepository = null;
        private readonly IGLAccountRepository _glAccountRepository = null;

        public JournalVoucherEntryManager(SCERPDBContext context)
        {
            this.JournalVoucherEntryRepository = new JournalVoucherEntryRepository(context);
        }

        public string SaveMaster(Acc_VoucherMaster voucherMaster)
        {
            if (voucherMaster.Id > 0)
            {               
                JournalVoucherEntryRepository.Edit(voucherMaster);
                JournalVoucherEntryRepository.DeleteDetail(voucherMaster.Id);
                return "Success";
            }
            return JournalVoucherEntryRepository.SaveMaster(voucherMaster);
        }

        public string SaveDetail(Acc_VoucherDetail voucherDetail)
        {              
            return JournalVoucherEntryRepository.SaveDetail(voucherDetail);
        }

        public IQueryable<Acc_VoucherDetail> GetGlVoucherDetail(long Id)
        {
            return JournalVoucherEntryRepository.GetGlVoucherDetail(Id);
        }

        public Acc_VoucherMaster GetAccVoucherMasterById(long Id)
        {
            return JournalVoucherEntryRepository.GetAccVoucherMasterById(Id);
        }

        public string GetAccountNamesById(int Id)
        {
            return JournalVoucherEntryRepository.GetAccountNamesById(Id);
        }

        public string GetFinancialPeriodById(int fpId)
        {
            return JournalVoucherEntryRepository.GetFinancialPeriodById(fpId);
        }

        public List<string> GetAccountName()
        {                                        
            return JournalVoucherEntryRepository.GetAccountNames();
        }

        public List<string> GetAccountNameThirdLayer()
        {
            return JournalVoucherEntryRepository.GetAccountNamesThirdLayer();
        }
        public List<string> GetSubGroupAndControlNames()
        {
            return JournalVoucherEntryRepository.GetSubGroupAndControlNames();
        }
        public List<string> GetControlNames()
        {
            return JournalVoucherEntryRepository.GetControlNames();
        }

        public List<string> GetControlSummaryNames()
        {
            return JournalVoucherEntryRepository.GetControlSummaryNames();
        }

        public string GetPeriodName()
        {
            return JournalVoucherEntryRepository.GetPeriodName();
        }

        public int GetPeriodId()
        {
            return JournalVoucherEntryRepository.GetPeriodId();
        }

        public IQueryable<Acc_CompanySector> GetAllCompanySector()
        {
            return JournalVoucherEntryRepository.GetAllCompanySector();
        }

        public List<Acc_CompanySector> GetAllActiveCompanySectory(Guid? employeeId)
        {
            return JournalVoucherEntryRepository.GetAllActiveCompanySectory(employeeId);
        }
        public IQueryable<Acc_CostCentre> GetAllCostCentres(int sectorId)
        {
            return JournalVoucherEntryRepository.GetAllCostCentres(sectorId);
        }

        public IQueryable<Acc_CostCentreMultiLayer> GetAllCostCentres()
        {
            return JournalVoucherEntryRepository.GetAllCostCentres();
        }


        public string CheckGLHeadValidation(string glHead)
        {
            return JournalVoucherEntryRepository.CheckGLHeadValidation((glHead));
        }

        public long GetVoucherNo(int fiscalPeriodId)
        {
            return JournalVoucherEntryRepository.GetVoucherNo(fiscalPeriodId);
        }

        public int GetAccountId(decimal accountCode)
        {
            return JournalVoucherEntryRepository.GetAccountId(accountCode);
        }

        public long GetLatestVoucherNo()
        {
            return JournalVoucherEntryRepository.GetLatestVoucherNo();
        }

        public IQueryable<Acc_GLAccounts> GetGlAccountsByMasterId(long id)
        {
            return JournalVoucherEntryRepository.GetGlAccountsByMasterId(id);
        }

        public int SaveActiveCompanySector(Guid? employeeId, int? companySectorId)
        {
            return JournalVoucherEntryRepository.SaveActiveCompanySector(employeeId, companySectorId);
        }
    }
}
