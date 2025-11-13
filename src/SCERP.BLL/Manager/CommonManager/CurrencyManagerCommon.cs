using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.DAL.Repository.CommonRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.CommonManager
{
    public class CurrencyManagerCommon : ICurrencyManagerCommon
    {
        private ICurrencyRepositoryCommon _currencyRepositoryCommon = null;
        public CurrencyManagerCommon(SCERPDBContext context)
        {
            _currencyRepositoryCommon = new CurrencyRepositoryCommon(context);
        }

        public List<Currency> GetAllCourrency()
        {
            var currencyList = new List<Currency>();
            try
            {
                currencyList = _currencyRepositoryCommon.All().Where(x => x.IsActive == true).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);

            }
            return currencyList;
        }

        public Currency GetCurrencyById(int currencyId)
        {

            var currency = new Currency();
            try
            {
                currency = _currencyRepositoryCommon.FindOne(x => x.CurrencyId == currencyId && x.IsActive);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return currency;
        }

        public int EditCurrency(Currency currency)
        {
            var editIndex = 0;
            try
            {
                var currencyObj = _currencyRepositoryCommon.FindOne(x => x.CurrencyId == currency.CurrencyId && x.IsActive == true);
                currencyObj.Name = currency.Name;
                currencyObj.Name = currency.NameInBengali;
                currencyObj.CurrencyCode = currency.CurrencyCode;
                currencyObj.EditedDate = DateTime.Now;
                currencyObj.CreatedDate = currency.CreatedDate;

                editIndex = _currencyRepositoryCommon.Edit(currencyObj);

            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return editIndex;
        }

        public int SaveCurrency(Currency currency)
        {
            var saveIndex = 0;
            try
            {
                currency.CreatedDate = DateTime.Now;
                currency.IsActive = true;
                saveIndex = _currencyRepositoryCommon.Save(currency);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return saveIndex;
        }

        public int DeleteCurrency(int? id)
        {
            var deleteIndex = 0;
            try
            {
                var currencyObj = _currencyRepositoryCommon.FindOne(x => x.CurrencyId == id && x.IsActive == true);
                currencyObj.IsActive = false;
                deleteIndex = _currencyRepositoryCommon.Edit(currencyObj);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return deleteIndex;
        }
    }
}
