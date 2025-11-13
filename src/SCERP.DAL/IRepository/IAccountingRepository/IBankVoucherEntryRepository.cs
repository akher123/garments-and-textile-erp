using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public interface IBankVoucherEntryRepository : IRepository<Acc_VoucherMaster>
    {
        string SaveMaster(Acc_VoucherMaster voucherMaster);
        string SaveDetail(Acc_VoucherDetail voucherDetail);
        List<string> GetAccountNames();
        List<string> GetBankAccountNames();
        string GetPeriodName();
        string GetBankInHand();
        int GetPeriodId();
        string DeleteDetail(long Id);
        IQueryable<Acc_VoucherDetail> GetBankVoucherDetail(long Id);
        string GetFinancialPeriodById(int fpId);
        Acc_VoucherMaster GetAccVoucherMasterById(long Id);
        IQueryable<Acc_CompanySector> GetAllCompanySector();
        int GetAccountId(decimal AccountCode);
        IQueryable<Acc_CostCentre> GetAllCostCentres(int sectorId);
        string CheckGLHeadValidation(string glHead);
        long GetVoucherNo(int fiscalPeriodId, string voucherType);
    }
}
