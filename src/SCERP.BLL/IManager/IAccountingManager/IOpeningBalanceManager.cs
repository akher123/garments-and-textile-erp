using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IAccountingManager
{
    public interface IOpeningBalaceManager
    {
        List<Acc_OpeningClosing> GetAllOpeningBalaces(int page, int records, string sort, ref List<decimal> totalAmount,
            int? FpId, int? SectorId, string GlId);

        Acc_OpeningClosing GetOpeningBalaceById(long? id);

        int SaveOpeningBalance(Acc_OpeningClosing openingBalance);

        void DeleteOpeningBalace(Acc_OpeningClosing openingBalace);

        IQueryable<Acc_FinancialPeriod> GetFinancialPeriod();

        IQueryable<Acc_CompanySector> GetSector();

        IQueryable<Acc_GLAccounts> GetGLAccounts();

        List<string> GetAccountName();

        string GetAccountNamesById(int Id);

        int Save(Acc_OpeningClosing open, string accountHead);
    }
}
