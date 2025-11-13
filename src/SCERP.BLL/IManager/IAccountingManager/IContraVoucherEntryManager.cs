using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;


namespace SCERP.BLL.IManager.IAccountingManager
{
    public interface IContraVoucherEntryManager
    {
        string SaveMaster(Acc_VoucherMaster voucherMaster);

        string SaveDetail(Acc_VoucherDetail voucherDetail);

        List<string> GetAccountNames(string accountType);
    
        string GetPeriodName();

        IQueryable<Acc_VoucherDetail> GetContraVoucherDetail(long Id);

        int GetPeriodId();

        string GetFinancialPeriodById(int fpId);
        Acc_VoucherMaster GetAccVoucherMasterById(long Id);
        IQueryable<Acc_CompanySector> GetAllCompanySector();

        IQueryable<Acc_CostCentre> GetAllCostCentres(int sectorId);

        string CheckGLHeadValidation(string glHead);

        long GetVoucherNo(int fiscalPeriodId, string voucherType);

        int GetAccountId(decimal accountCode);

        string GetClosingBalance(Acc_ClosingBalanceViewModel obj);      
    }
}
