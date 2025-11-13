using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.AccountingModel;

namespace SCERP.BLL.IManager.IAccountingManager
{
    public interface IAccCurrencManager
    {
        Acc_Currency GetCurrencyById(int currencyId);
        int SaveCurrency(Acc_Currency model);
        List<Acc_Currency> GetCurrencies();
        int CurrencyEdit(Acc_Currency model);
    }
}
