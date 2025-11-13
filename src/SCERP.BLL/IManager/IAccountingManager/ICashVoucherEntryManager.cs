using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;


namespace SCERP.BLL.IManager.IAccountingManager
{
    public interface ICashVoucherEntryManager
    {
        string SaveMaster(Acc_VoucherMaster voucherMaster);

        string SaveDetail(Acc_VoucherDetail voucherDetail);

        List<string> GetAccountName();

        IQueryable<Acc_VoucherDetail> GetCashVoucherDetail(long Id);
        Acc_VoucherMaster GetAccVoucherMasterById(long Id);

        string GetFinancialPeriodById(int fpId);
        List<string> GetCashAccountNames();

        string GetPeriodName();

        string GetCashInHand();

        int GetPeriodId();

        IQueryable<Acc_CompanySector> GetAllCompanySector();

        IQueryable<Acc_CostCentre> GetAllCostCentres(int sectorId);

        string CheckGLHeadValidation(string glHead);

        long GetVoucherNo(int fiscalPeriodId, string voucherType);

        int GetAccountId(decimal accountCode);

        string GetClosingBalance(Acc_ClosingBalanceViewModel obj);

        int GetCashInHandGLId();
    }
}
