using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public interface IContraVoucherEntryRepository : IRepository<Acc_VoucherMaster>
    {
        string SaveMaster(Acc_VoucherMaster voucherMaster);
        string SaveDetail(Acc_VoucherDetail voucherDetail);
        List<string> GetAccountNames(string accountType);   
        string GetPeriodName();
        int GetPeriodId();
        IQueryable<Acc_VoucherDetail> GetContraVoucherDetail(long Id);
        string GetFinancialPeriodById(int fpId);
        string DeleteDetail(long Id);
        Acc_VoucherMaster GetAccVoucherMasterById(long Id);
        IQueryable<Acc_CompanySector> GetAllCompanySector();
        int GetAccountId(decimal AccountCode);
        IQueryable<Acc_CostCentre> GetAllCostCentres(int sectorId);
        string CheckGLHeadValidation(string glHead);
        long GetVoucherNo(int fiscalPeriodId, string voucherType);
        string GetClosingBalance(Acc_ClosingBalanceViewModel obj);   
    }
}
