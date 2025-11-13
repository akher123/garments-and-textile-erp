using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public interface IVoucherListRepository : IRepository<Acc_VoucherMaster>
    {
        List<VoucherList> GetAllVoucherList(int page, int records, string sort,
            int? fpId, DateTime? FromDate, DateTime? ToDate, string VoucherType, string VoucherNo);
        Acc_VoucherMaster GetVoucherMasterById(long? id);
        IQueryable<Acc_FinancialPeriod> GetFinancialPeriod();
        string CheckPeriodFromToDate(int Id, DateTime fromDate, DateTime toDate);
        List<DateTime> GetPeriodFromToDate(int Id);
    }
}
