using System;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Accounting.Models.ViewModels;


namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class CompanySectorController : BaseAccountingController
    {
        public const int PageSize = 10;

        private Guid _employeeGuidId = Guid.Parse("7bca454d-187a-4885-9c65-07e4ef23ee8c");


        [AjaxAuthorize(Roles = "companysector-1,companysector-2,companysector-3")]
        public ActionResult Index(int? page, string sort, CompanySectorViewModel model)
        {
            var startPage = 0;

            if (page.HasValue && page.Value > 0)
            {
                startPage = page.Value - 1;
            }

            model.CompanySector = CompanySectorManager.GetAllCompanySectors(startPage, PageSize, sort);
            model.TotalRecords = model.CompanySector.Count;
            return View(model);
        }

        [AjaxAuthorize(Roles = "companysector-2,companysector-3")]
        public ActionResult Edit(Acc_CompanySector model)
        {
            ModelState.Clear();

            if (model.Id != 0)
            {
                var sectorManager = CompanySectorManager.GetCompanySectorById(model.Id);

                model.SectorCode = sectorManager.SectorCode;
                model.SectorName = sectorManager.SectorName;
                model.SortOrder = sectorManager.SortOrder;
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "companysector-2,companysector-3")]
        public ActionResult Save(CompanySectorViewModel model)
        {
            if (Session["EmployeeGuid"] != null)
            {
                _employeeGuidId = (Guid) Session["EmployeeGuid"];
            }

            var companySector = CompanySectorManager.GetCompanySectorById(model.Id) ?? new Acc_CompanySector();
            
            companySector.SectorName = model.SectorName;
            companySector.SectorCode = model.SectorCode;
            companySector.SortOrder = model.SortOrder;

            if (model.Id > 0)
            {
                companySector.EDT = DateTime.Now;
                companySector.EditedBy = PortalContext.CurrentUser.UserId;               
            }
            else
            {
                companySector.CDT = DateTime.Now;
                companySector.CreatedBy = PortalContext.CurrentUser.UserId;                
            }

            string message = "";

            var i = CompanySectorManager.SaveCompanySector(companySector);

            if (i == 0)
                message = "Database error has occured !";

            else if (i == 2)
                message = "Duplicate Company Sector Code Exists !";

            else if (i == 3)
                message = "Duplicate Company Sector Name Exists !";

            if (i == 1)
            {
                return Reload();
            }
            return CreateJsonResult(new {Success = false, Reload = true, Message = message});
        }

        [AjaxAuthorize(Roles = "companysector-3")]
        public ActionResult Delete(int id)
        {
            var companySector = CompanySectorManager.GetCompanySectorById(id) ?? new Acc_CompanySector();
            CompanySectorManager.DeleteCompanySector(companySector);
            return Reload();
        }
    }
}