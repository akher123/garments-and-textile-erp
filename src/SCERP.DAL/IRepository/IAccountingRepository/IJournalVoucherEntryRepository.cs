using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.AccountingModel;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public interface IJournalVoucherEntryRepository : IRepository<Acc_VoucherMaster>
    {       
        string SaveMaster(Acc_VoucherMaster voucherMaster);
        string SaveDetail(Acc_VoucherDetail voucherDetail);
        string DeleteDetail(long Id);
        Acc_VoucherMaster GetAccVoucherMasterById(long Id);
        string GetFinancialPeriodById(int fpId);
        string GetAccountNamesById(int Id);
        List<string> GetAccountNames();
        List<string> GetAccountNamesThirdLayer();
        IQueryable<Acc_VoucherDetail> GetGlVoucherDetail(long Id);
        string GetPeriodName();
        int GetPeriodId();
        IQueryable<Acc_CompanySector> GetAllCompanySector();
        int GetAccountId(decimal AccountCode);
        IQueryable<Acc_CostCentre> GetAllCostCentres(int sectorId);
        IQueryable<Acc_CostCentreMultiLayer> GetAllCostCentres();
        string CheckGLHeadValidation(string glHead);
        long GetVoucherNo(int fiscalPeriodId);
        List<string> GetSubGroupAndControlNames();
        List<string> GetControlNames();
        List<string> GetControlSummaryNames();
        long GetLatestVoucherNo();
        IQueryable<Acc_GLAccounts> GetGlAccountsByMasterId(long id);
        List<Acc_CompanySector> GetAllActiveCompanySectory(Guid? employeeId);
        int SaveActiveCompanySector(Guid? employeeId, int? companySectorId);
    }
}
