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
    public class CostCentreController : BaseAccountingController
    {
        public const int PageSize = 10;
        private Guid employeeGuidId = Guid.Parse("7bca454d-187a-4885-9c65-07e4ef23ee8c");

        [AjaxAuthorize(Roles = "costcentre-1,costcentre-2,costcentre-3")]
        public ActionResult Index(int? page, string sort, CostCentreViewModel model)
        {
            var startPage = 0;

            if (page.HasValue && page.Value > 0)
            {
                startPage = page.Value - 1;
            }

            model.CostCentre = CostCentreManager.GetAllCostCentres(startPage, PageSize, sort);
            model.TotalRecords = model.CostCentre.Count;
            return View(model);
        }

        [AjaxAuthorize(Roles = "costcentre-2,costcentre-3")]
        public ActionResult Edit(Acc_CostCentre model)
        {
            ModelState.Clear();

            if (model.Id != 0)
            {
                var sectorManager = CostCentreManager.GetCostCentreById(model.Id);

                model.CostCentreCode = sectorManager.CostCentreCode;
                model.CostCentreName = sectorManager.CostCentreName;
                model.SortOrder = sectorManager.SortOrder;
                model.SectorId = sectorManager.SectorId;
            }

            ViewBag.SectorId = new SelectList(CostCentreManager.GetAllCompanySector(), "Id", "SectorName", model.SectorId);

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "costcentre-2,costcentre-3")]
        public ActionResult Save(CostCentreViewModel model)
        {
            if (Session["EmployeeGuid"] != null)
            {
                employeeGuidId = (Guid)Session["EmployeeGuid"];
            }

            var CostCentre = CostCentreManager.GetCostCentreById(model.Id) ?? new Acc_CostCentre();

            CostCentre.CostCentreName = model.CostCentreName;
            CostCentre.CostCentreCode = model.CostCentreCode;
            CostCentre.SortOrder = model.SortOrder;
            CostCentre.SectorId = model.SectorId;

            if (model.Id > 0)
            {
                CostCentre.EDT = DateTime.Now;
                CostCentre.EditedBy = employeeGuidId;
            }
            else
            {
                CostCentre.CDT = DateTime.Now;
                CostCentre.CreatedBy = employeeGuidId;
            }

            string message = "";

            var i = CostCentreManager.SaveCostCentre(CostCentre);

            if (i == 0)
                message = "Database error has occured !";

            if (i == 2)
                message = "Duplicate Company Sector Name Exists!";

            if (i == 3)
                message = "Duplicate Company Sector Code Exists !";

            if (i == 1)
            {
                return Reload();
            }
            return CreateJsonResult(new { Success = false, Reload = true, Message = message });
        }

        [AjaxAuthorize(Roles = "costcentre-3")]
        public ActionResult Delete(int id)
        {
            var CostCentre = CostCentreManager.GetCostCentreById(id) ?? new Acc_CostCentre();
            CostCentreManager.DeleteCostCentre(CostCentre);
            return Reload();
        }
    }
}