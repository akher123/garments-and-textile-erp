using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.Manager.AccountingManager;
using SCERP.Common;
using SCERP.Web.Areas.Accounting.Models.ViewModels;

namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class ActiveCompanySectoryController : BaseAccountingController
    {

        private readonly Guid? _employeeGuidId = PortalContext.CurrentUser.UserId;

        public ActionResult Index(VoucherEntryViewModel model)
        {
            var temp = JournalVoucherEntryManager.GetAllActiveCompanySectory(_employeeGuidId).ToList();
            model.CompanySectors = temp;
            return View(model);
        }

        public JsonResult Save(int? companySectorId)
        {
            var result = JournalVoucherEntryManager.SaveActiveCompanySector(_employeeGuidId, companySectorId);

            if (result > 0)
                return ErrorResult("Data save successfully !");
            else
                return ErrorResult("Error has occured !");
        }
    }
}