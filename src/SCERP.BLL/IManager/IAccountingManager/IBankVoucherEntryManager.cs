using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;


namespace SCERP.BLL.IManager.IAccountingManager
{
    public interface IBankVoucherEntryManager
    {
        string SaveMaster(Acc_VoucherMaster voucherMaster);

        string SaveDetail(Acc_VoucherDetail voucherDetail);

        List<string> GetAccountName();

        List<string> GetBankAccountNames();

        string GetPeriodName();

        IQueryable<Acc_VoucherDetail> GetBankVoucherDetail(long Id);
        Acc_VoucherMaster GetAccVoucherMasterById(long Id);

        string GetBankInHand();

        string GetFinancialPeriodById(int fpId);
        int GetPeriodId();

        IQueryable<Acc_CompanySector> GetAllCompanySector();

        IQueryable<Acc_CostCentre> GetAllCostCentres(int sectorId);

        string CheckGLHeadValidation(string glHead);

        long GetVoucherNo(int fiscalPeriodId, string voucherType);

        int GetAccountId(decimal accountCode);
    }
}
