using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public interface IBankReconcilationRepository : IRepository<Acc_VoucherMaster>
    {
        List<VoucherList> GetAllVoucherList(int page, int records, string sort,
           int? fpId, DateTime? FromDate, DateTime? ToDate, string AccountName, string SectorId);
        Acc_VoucherMaster GetVoucherMasterById(long? id);
        IQueryable<Acc_FinancialPeriod> GetFinancialPeriod();
        string CheckPeriodFromToDate(int Id, DateTime fromDate, DateTime toDate);
        List<DateTime> GetPeriodFromToDate(int Id);
        List<string> GetAccountNames();
        IQueryable<Acc_CompanySector> GetAllCompanySector();
        string SaveBankReconMaster(string sectorId, string fpId, string accountName, DateTime? fromDate,
            DateTime? toDate);
        string SaveBankReconDetail(List<long> detail);
        string SaveManualDetail(Acc_BankVoucherManual bankVoucherManual);
        string CheckDuplicate(List<string> voucherId);
    }
}
