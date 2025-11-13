using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IAccountingManager
{
    public interface IVoucherListManager
    {
        List<VoucherList> GetAllVoucherList(int page, int records, string sort,
            int? fpId, DateTime? FromDate, DateTime? ToDate, string VoucherType, string VoucherNo);

        Acc_VoucherMaster GetVoucherMasterById(long? id);

        IQueryable<Acc_FinancialPeriod> GetFinancialPeriod();

        string CheckPeriodFromToDate(int Id, DateTime fromDate, DateTime toDate);

        List<DateTime> GetPeriodFromToDate(int Id);

        void DeleteVoucherList(Acc_VoucherMaster voucherMaster);
    }
}
