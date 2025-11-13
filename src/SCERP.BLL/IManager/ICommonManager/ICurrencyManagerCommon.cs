using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.ICommonManager
{
    public interface ICurrencyManagerCommon
    {
        List<Currency> GetAllCourrency();
        Currency GetCurrencyById(int currencyId);
        int EditCurrency(Currency currency);
        int SaveCurrency(Currency currency);
        int DeleteCurrency(int? id);
    }
}
