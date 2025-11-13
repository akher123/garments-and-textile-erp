using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Controllers;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Common.Controllers
{
  
    public class CurrencyCommonController : BaseController
    {
        public ActionResult Index()
        {

            List<Currency> currencies = CurrencyManagerCommon.GetAllCourrency()??new List<Currency>();
            return View(currencies);
        }

        public ActionResult Edit(Currency currency)
        {
            ModelState.Clear();
            if (currency.CurrencyId > 0)
            {
                currency = CurrencyManagerCommon.GetCurrencyById(currency.CurrencyId);
            }
            return View(currency);
        }

        public ActionResult Save(Currency currency)
        {
            var saveIndex = 0;
            saveIndex = currency.CurrencyId > 0 ? CurrencyManagerCommon.EditCurrency(currency) : CurrencyManagerCommon.SaveCurrency(currency);
            return saveIndex > 0 ? Reload() : ErrorMessageResult();

        }

        public ActionResult Delete(int? id)
        {
            var saveIndex = CurrencyManagerCommon.DeleteCurrency(id);
            return saveIndex > 0 ? Reload() : ErrorResult("not delele");
        }
	}
}