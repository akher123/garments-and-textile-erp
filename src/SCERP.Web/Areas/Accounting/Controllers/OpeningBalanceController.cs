using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.BLL.Manager.AccountingManager;
using SCERP.Common;
using SCERP.Model;

using SCERP.Web.Areas.Accounting.Models.ViewModels;
using System.IO;
using SCERP.Web.Controllers;


namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class OpeningBalanceController : BaseAccountingController
    {
        public ActionResult Index(OpeningBalanceViewModel model)
        {
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", 1);

            return View(model);
        }

        public ActionResult Save(OpeningBalanceViewModel model)
        {
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", 1);

            int lenght = model.AccountHead.Length;
            string accountCode = model.AccountHead.Substring(lenght - 10, 10);

            var result = OpeningBalaceManager.Save(model, accountCode);

            if (result > 0)
                return ErrorResult("Data saved successfully !");

            else            
                return ErrorResult("Data Can not be saved !");            
        }

        public ActionResult TagSearch(string term)
        {
            List<string> tags = JournalVoucherEntryManager.GetAccountName();
            return this.Json(tags.Where(t => t.Substring(0, t.Length - 11).ToLower().Contains(term.ToLower())).Skip(0).Take(15), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAccountName(int id)
        {
            string value = OpeningBalaceManager.GetAccountNamesById(id);
            return Json(new {data = value, Success = true});
        }
    }
}
