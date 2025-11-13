using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.AccountingModel;

namespace SCERP.BLL.IManager.IAccountingManager
{
    public interface IJournalVoucherEntryManager
    {     
        string SaveMaster(Acc_VoucherMaster voucherMaster);

        string SaveDetail(Acc_VoucherDetail voucherDetail);

        Acc_VoucherMaster GetAccVoucherMasterById(long Id);

        string GetFinancialPeriodById(int fpId);

        IQueryable<Acc_VoucherDetail> GetGlVoucherDetail(long Id);

        string GetAccountNamesById(int Id);

        List<string> GetAccountName();
        List<string> GetAccountNameThirdLayer();

        string GetPeriodName();

        int GetPeriodId();

        IQueryable<Acc_CompanySector> GetAllCompanySector();

        IQueryable<Acc_CostCentre> GetAllCostCentres(int sectorId);

        IQueryable<Acc_CostCentreMultiLayer> GetAllCostCentres();

        string CheckGLHeadValidation(string glHead);

        long GetVoucherNo(int fiscalPeriodId);

        int GetAccountId(decimal accountCode);

        List<string> GetSubGroupAndControlNames();

        List<string> GetControlNames();

        List<string> GetControlSummaryNames();

        long GetLatestVoucherNo();

        IQueryable<Acc_GLAccounts> GetGlAccountsByMasterId(long id);

        List<Acc_CompanySector> GetAllActiveCompanySectory(Guid? employeeId);

        int SaveActiveCompanySector(Guid? employeeId, int? companySectorId);
    }
}
