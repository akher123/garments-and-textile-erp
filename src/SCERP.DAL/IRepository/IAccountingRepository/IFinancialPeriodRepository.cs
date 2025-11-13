using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public interface IFinancialPeriodRepository : IRepository<Acc_FinancialPeriod>
    {
        Acc_FinancialPeriod GetFinancialPeriodById(int? id);
        List<Acc_FinancialPeriod> GetAllFinancialPeriods(int page, int records, string sort);
        IQueryable<Acc_FinancialPeriod> GetAllFinancialPeriods();
    }
}
