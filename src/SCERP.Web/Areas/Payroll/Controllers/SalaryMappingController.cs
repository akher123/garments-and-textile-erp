using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Web.Areas.Accounting.Controllers;
using SCERP.Web.Areas.Payroll.Models.ViewModels;
using SCERP.Web.Controllers;


namespace SCERP.Web.Areas.Payroll.Controllers
{
    public class SalaryMappingController : BaseAccountingController
    {
        public ActionResult Edit(SalaryMappingViewModel model)
        {
            ViewBag.SectorId = new SelectList(JournalVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id",
                "SectorName");

            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");
            model.SalaryHeads = SalaryMappingManager.GetAllSalaryHead() ?? new List<SalaryHead>(); ;

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }

        public ActionResult SaveSalaryValues(List<string> Values)
        {
            var message = SalaryMappingManager.SaveSalaryMapping(Values);
            return Json(new {Success = true, Message = message});
        }

        public ActionResult TagSearch(string term)
        {
            List<string> tags = JournalVoucherEntryManager.GetAccountName();

            return this.Json(tags.Where(t => t.Substring(11, t.Length - 11).StartsWith(term.ToLower())),
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSalaryMapping(int? sectorId, int? costCentreId)
        {
            List<string> lt = SalaryMappingManager.GetSalaryMapping(sectorId, costCentreId);        
            return Json(lt, JsonRequestBehavior.AllowGet);
        }
    }
}
