using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public interface IOpeningBalanceRepository : IRepository<Acc_OpeningClosing>
    {
        Acc_OpeningClosing GetOpeningBalaceById(long? id);

        List<Acc_OpeningClosing> GetAllOpeningBalaces(int page, int records, string sort, ref List<decimal> totalAmount,
            int? fpId, int? sectorId, string glId);
        IQueryable<Acc_OpeningClosing> GetAllOpeningBalaces();
        IQueryable<Acc_FinancialPeriod> GetFinancialPeriod();
        IQueryable<Acc_CompanySector> GetSector();
        IQueryable<Acc_GLAccounts> GetGLAccounts();
        List<string> GetAccountNames();
        string GetAccountNamesById(int Id);
        int Save(Acc_OpeningClosing open, string accountHead);
    }
}
