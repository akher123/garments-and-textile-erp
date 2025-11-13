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
using System.Text.RegularExpressions;

namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class GlHeadGroupChangeByParentController : BaseAccountingController
    {
        private IControlAccountManager _controlAccountManager;
        private readonly IVoucherMasterManager voucherMasterManager;
        public GlHeadGroupChangeByParentController(IVoucherMasterManager voucherMasterManager,IControlAccountManager controlAccountManager)
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

                string glCode = model.GLHeadName.Substring(model.GLHeadName.Length - 10, 10);
                string controlCode = model.GLHeadNameNew.Substring(model.GLHeadNameNew.Length - 7, 7);
                string subGroupCode = model.GLHeadNameNew.Substring(model.GLHeadNameNew.Length - 5, 5);

                if (Regex.IsMatch(controlCode, @"^\d+$"))
                {
                    result = _controlAccountManager.GLtoControlChange(glCode, controlCode);
                }
                else if(Regex.IsMatch(subGroupCode, @"^\d+$"))
                {
                    result = _controlAccountManager.GLtoSubGroupChange(glCode, subGroupCode);
                }               
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return (result > 0) ? Reload() : ErrorResult("Failed to save data");
        }
    }
}