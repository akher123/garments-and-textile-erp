using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IAccountingManager
{
    public interface IFinancialPeriodManager
    {
        List<Acc_FinancialPeriod> GetAllFinancialPeriods(int page, int records, string sort);

        Acc_FinancialPeriod GetFinancialPeriodById(int? id);

        int SaveFinancialPeriod(Acc_FinancialPeriod aFinancialPeriod);

        void DeleteFinancialPeriod(Acc_FinancialPeriod FinancialPeriod);
    }
}
