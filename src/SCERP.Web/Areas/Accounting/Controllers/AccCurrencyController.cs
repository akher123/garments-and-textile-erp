using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model.AccountingModel;

namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class AccCurrencyController : BaseAccountingController
    {
        public ActionResult Index()
        {
            ModelState.Clear();
            var currencies = AccCurrencManager.GetCurrencies();
            return View(currencies);
        }

        public ActionResult Edit(Acc_Currency model)
        {
            ModelState.Clear();
            model.ActiveStatus = true;
            if (model.CurrencyId > 0)
            {
                var currency = AccCurrencManager.GetCurrencyById(model.CurrencyId) ?? new Acc_Currency();
                model.FirstCurName = currency.FirstCurName;
                model.FirstCurValue = currency.FirstCurValue;
                model.FirstCurSymbol = currency.FirstCurSymbol;
                model.SecendCurName = currency.SecendCurName;
                model.SecendCurSymbol = currency.SecendCurSymbol;
                model.SecendCurValue = currency.SecendCurValue;
                model.ThirdCurName = currency.ThirdCurName;
                model.ThirdCurValue = currency.ThirdCurValue;
                model.ThirdCurSymbol = currency.ThirdCurSymbol;
                model.CurDate = currency.CurDate;
                model.CurrencyId = currency.CurrencyId;
                model.ActiveStatus = currency.ActiveStatus;
            }

            return View(model);
        }


        public ActionResult Save(Acc_Currency model)
        {
            var status = 0;
            try
            {
                status = model.CurrencyId > 0 ? AccCurrencManager.CurrencyEdit(model) : AccCurrencManager.SaveCurrency(model);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return status > 0 ? Reload() : ErrorResult("Fail to Save Currency");
        }
    }
}