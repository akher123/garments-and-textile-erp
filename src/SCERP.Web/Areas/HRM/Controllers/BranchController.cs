
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class BranchController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;


        [AjaxAuthorize(Roles = "branch-1,branch-2,branch-3")]
        public ActionResult Index(Model.Custom.BranchViewModel model)
        {
            ModelState.Clear();
            model.Companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
            Branch branch = model;
            branch.Name = model.SearchByBranchName;
            branch.CompanyId = model.SearchByCompany;
            //if (model.IsSearch)
            //{
            //    model.IsSearch = false;
            //    return View(model);
            //}
            var startPage = 0;
            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }

            int totalRecords = 0;
            model.Branches = BranchManager.GetAllBranchesByPaging(startPage, _pageSize, branch, out totalRecords) ?? new List<Branch>();
            model.TotalRecords = totalRecords;

            return View(model);
        }

        [AjaxAuthorize(Roles = "branch-2,branch-3")]
        public ActionResult Edit(Model.Custom.BranchViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.Id > 0)
                {
                    var branch = BranchManager.GetBranchById(model.Id);
                    model.CompanyId = branch.CompanyId;
                    model.Name = branch.Name;
                    model.NameInBengali = branch.NameInBengali;
                    model.FullAddress = branch.FullAddress;
                    model.FullAddressInBengali = branch.FullAddressInBengali;
                    model.DistrictId = branch.DistrictId;
                    model.PoliceStationId = branch.PoliceStationId;
                    model.PostOffice = branch.PostOffice;
                    model.PostOfficeInBengali = branch.PostOfficeInBengali;
                    model.PostCode = branch.PostCode;
                    model.Phone = branch.Phone;
                    model.Fax = branch.Fax;
                    model.Email = branch.Email;
                }
                model.Companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
                model.Districts = DistrictManager.GetAllDistricts();
                model.PoliceStations = PoliceStationManager.GetPoliceStationByDistrict(model.DistrictId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "branch-2,branch-3")]
        public ActionResult Save(Model.Custom.BranchViewModel model)
        {
            var isExist = BranchManager.CheckExistingBranch(model);
            if (isExist)
            {
                return ErrorResult(model.Name + " " + "Branch already exist");
            }
            var branch = BranchManager.GetBranchById(model.Id) ?? new Branch();
            branch.Name = model.Name;
            branch.NameInBengali = model.NameInBengali;
            branch.CompanyId = model.CompanyId;
            branch.FullAddress = model.FullAddress;
            branch.FullAddressInBengali = model.FullAddressInBengali;
            branch.DistrictId = model.DistrictId;
            branch.PoliceStationId = model.PoliceStationId;
            branch.PostOffice = model.PostOffice;
            branch.PostOfficeInBengali = model.PostOfficeInBengali;
            branch.PostCode = model.PostCode;
            branch.Phone = model.Phone;
            branch.Email = model.Email;
            branch.Fax = model.Fax;
            branch.IsActive = true;
            var saveIndex = (model.Id > 0) ? BranchManager.EditBranch(branch) : BranchManager.SaveBranch(branch);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "branch-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var branch = BranchManager.GetBranchById(id) ?? new Branch();
            deleted = BranchManager.DeleteBranch(branch);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        public ActionResult GetAllPoliceStationsByDistrictId(int districtId)
        {
            var policeStations = PoliceStationManager.GetPoliceStationByDistrict(districtId);
            return Json(new { Success = true, PoliceStations = policeStations }, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "branch-1,branch-2,branch-3")]
        public void GetExcel(Model.Custom.BranchViewModel model)
        {
            var branches = BranchManager.GetAllBranchesBySearchKey(model.SearchByBranchName, model.SearchByCompany);
            const string fileName = "BranchList";
            var boundFields = new List<BoundField>
            {     
               new BoundField(){HeaderText = @"Name",DataField = "Name"},
               new BoundField(){HeaderText = @"Company Name",DataField = "Company.Name"},
               new BoundField(){HeaderText = @"Address",DataField = "FullAddress"},
               new BoundField(){HeaderText = @"Police Station",DataField = "PoliceStation.Name"},
               new BoundField(){HeaderText = @"District",DataField = "District.Name"},
               new BoundField(){HeaderText = @"Post Office",DataField = "PostOffice"},
               new BoundField(){HeaderText = @"Post Code",DataField = "PostCode"},
               new BoundField(){HeaderText = @"Phone",DataField = "Phone"},
               new BoundField(){HeaderText = @"Fax",DataField = "Email"},
               new BoundField(){HeaderText = @"Email",DataField = "Fax"},
            };

            ReportConverter.CustomGridView(boundFields, branches, fileName);
        }


        [AjaxAuthorize(Roles = "branch-1,branch-2,branch-3")]
        public ActionResult Print(Model.Custom.BranchViewModel model)
        {
            List<Branch> branches = BranchManager.GetAllBranchesBySearchKey(model.SearchByBranchName, model.SearchByCompany);
            model.Branches = branches;

            return View("_BranchPdfReport", model);
        }
    }

}

