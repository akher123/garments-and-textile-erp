using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Accounting.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IAccountingManager;

namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class ControlChangeByParentController : BaseAccountingController
    {
        private readonly IControlAccountManager _controlAccountManager;
        private readonly IVoucherMasterManager voucherMasterManager;
        public ControlChangeByParentController(IVoucherMasterManager voucherMasterManager,IControlAccountManager controlAccountManager)
        {
            _controlAccountManager = controlAccountManager;
            this.voucherMasterManager = voucherMasterManager;
        }
        private readonly Guid? _employeeGuidId = PortalContext.CurrentUser.UserId;

        public ActionResult Index(int? page, string sort, GeneralLedgerViewModel model, int? costCentreId)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);

            if (companyId != null) model.sectorId = (int) companyId;

            ViewBag.sectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", model.sectorId);

            var costCentre = CostCentreManager.GetCostCentreById(costCentreId);
            var costCentrelist = new List<Acc_CostCentre>();
            if (costCentre != null)
                costCentrelist.Add(costCentre);
            ViewBag.CostCentreId = costCentre != null ? new SelectList(costCentrelist, "Id", "CostCentreName", costCentre.Id) : new SelectList(costCentrelist, "Id", "CostCentreId");

            return View(model);
        }

        public ActionResult TransferGlAccount(GeneralLedgerViewModel model)
        {
            int result = 0;

            try
            {
                if (string.IsNullOrEmpty(model.GLHeadName) || string.IsNullOrEmpty(model.GLHeadNameNew))
                    return ErrorResult("Please Enter valid Account Head !");

                var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
                if (companyId != null) model.sectorId = (int)companyId;

                string controlCode = model.GLHeadName.Substring(model.GLHeadName.Length - 7, 7);
                string groupCode = model.GLHeadNameNew.Substring(model.GLHeadNameNew.Length - 5, 5);

                result = _controlAccountManager.ControltoSubGroupChange(groupCode, controlCode);
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return (result > 0) ? Reload() : ErrorResult("Failed to save data");
        }
    }
}