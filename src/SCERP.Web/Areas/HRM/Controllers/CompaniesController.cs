using System;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;
namespace SCERP.Web.Areas.HRM.Controllers
{
    public class CompaniesController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "company-1,company-2,company-3")]
        public ActionResult Index(CompanyViewModel model)
        {
            ModelState.Clear();

            Company company = model;
            company.Name = model.SearchByCompanyName;

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
            model.Companies = CompanyManager.GetAllCompaniesByPaging(startPage, _pageSize, out totalRecords, company) ?? new List<Company>();
            model.TotalRecords = totalRecords;
            return View(model);
        }

        [AjaxAuthorize(Roles = "company-2,company-3")]
        public ActionResult Edit(Company model)
        {
            ModelState.Clear();

            try
            {
                if (model.Id > 0)
                {
                    var company = CompanyManager.GetCompanyById(model.Id);
                    model.Name = company.Name;
                    model.NameInBengali = company.NameInBengali;
                    model.FullAddress = company.FullAddress;
                    model.FullAddressInBengali = company.FullAddressInBengali;
                    model.DistrictId = company.DistrictId;
                    model.PoliceStationId = company.PoliceStationId;
                    model.PostOffice = company.PostOffice;
                    model.PostOfficeInBengali = company.PostOfficeInBengali;
                    model.PostCode = company.PostCode;
                    model.Phone = company.Phone;
                    model.Fax = company.Fax;
                    model.Email = company.Email;
                    model.Website = company.Website;
                    model.TinNo = company.TinNo;
                    model.VatRegNo = company.VatRegNo;
                    model.ContactPerson = company.ContactPerson;
                    model.ContactPersonDesignation = company.ContactPersonDesignation;
                    model.ContactPersonMobile = company.ContactPersonMobile;
                    model.ContactPersonEmail = company.ContactPersonEmail;
                    model.DomainName = company.DomainName;
                    model.ImagePath = company.ImagePath;
                    model.CompanyRefId = company.CompanyRefId;
                    ViewBag.DistrictItemList = new SelectList(DistrictManager.GetAllDistricts(), "Id", "Name",
                        company.DistrictId);
                    ViewBag.PoliceStationItemList =
                        new SelectList(
                            PoliceStationManager.GetPoliceStationByDistrict(company.DistrictId) ??
                            new List<PoliceStation>(), "Id", "Name", company.PoliceStationId);
                    ViewBag.Title = "Edit";
                }
                else
                {
                    model.CompanyRefId = CompanyManager.GetNewCompanyRefId();
                    ViewBag.DistrictItemList = new SelectList(DistrictManager.GetAllDistricts(), "Id", "Name");
                    ViewBag.PoliceStationItemList = new SelectList(new List<PoliceStation>(), "Id", "Name");
                    ViewBag.Title = "Add";
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "company-2,company-3")]
        public ActionResult Save(CompanyViewModel model)
        {
            var isExist = CompanyManager.CheckExistingCompany(model);

            if (isExist)
            {
                return ErrorResult(model.Name + " " + "Company already exist");
            }

            var company = CompanyManager.GetCompanyById(model.Id) ?? new Company();
            company.Name = model.Name;
            company.NameInBengali = model.NameInBengali;
            company.FullAddress = model.FullAddress;
            company.FullAddressInBengali = model.FullAddressInBengali;
            company.DistrictId = model.DistrictId;
            company.PoliceStationId = model.PoliceStationId;
            company.PostOffice = model.PostOffice;
            company.PostOfficeInBengali = model.PostOfficeInBengali;
            company.PostCode = model.PostCode;
            company.Phone = model.Phone;
            company.Fax = model.Fax;
            company.Email = model.Email;
            company.Website = model.Website;
            company.TinNo = model.TinNo;
            company.VatRegNo = model.VatRegNo;
            company.ContactPerson = model.ContactPerson;
            company.ContactPersonDesignation = model.ContactPersonDesignation;
            company.ContactPersonMobile = model.ContactPersonMobile;
            company.ContactPersonEmail = model.ContactPersonEmail;
            company.IsActive = true;
            company.ImagePath = model.ImagePath;
            company.DomainName = model.DomainName;
            int saveIndex = 0;
            if (model.Id > 0)
            {
                saveIndex = CompanyManager.EditCompany(company);
            }
            else
            {

                saveIndex = CompanyManager.SaveCompany(company);
            }

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save Company Information");
        }

        [AjaxAuthorize(Roles = "company-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var company = CompanyManager.GetCompanyById(id) ?? new Company();
            deleted = CompanyManager.DeleteCompany(company);
            return deleted > 0 ? Reload() : ErrorResult("Failed to delete data");
        }

        [AjaxAuthorize(Roles = "company-1,company-2,company-3")]
        public void GetExcel(CompanyViewModel model)
        {
            List<Company> companies = CompanyManager.GetAllCompaniesBySearchKey(model.SearchByCompanyName);
            model.Companies = companies;

            const string fileName = "CompanyList";
            var boundFields = new List<BoundField>
            {     
               new BoundField(){HeaderText = @"Company Name",DataField = "Name"},
               new BoundField(){HeaderText = @"District Name",DataField = "District.Name"},
               new BoundField(){HeaderText = @"PoliceStation Name",DataField = "PoliceStation.Name"},
               new BoundField(){HeaderText = @"Address",DataField = "FullAddress"},
               new BoundField(){HeaderText = @"PostOffice",DataField = "PostOffice"},
               new BoundField(){HeaderText = @"PostCode",DataField = "PostCode"},
               new BoundField(){HeaderText = @"Phone",DataField = "Phone"},
               new BoundField(){HeaderText = @"Email",DataField = "Email"},
               new BoundField(){HeaderText = @"Fax",DataField = "Fax"},
               new BoundField(){HeaderText = @"Website",DataField = "Website"},
               new BoundField(){HeaderText = @"TinNo",DataField = "TinNo"},
               new BoundField(){HeaderText = @"VatRegNo",DataField = "VatRegNo"},
               new BoundField(){HeaderText = @"Contact Person",DataField = "ContactPerson"},
               new BoundField(){HeaderText = @"Contact Person Designation",DataField = "ContactPersonDesignation"},
               new BoundField(){HeaderText = @"Contact Person Mobile",DataField = "ContactPersonMobile"},
               new BoundField(){HeaderText = @"Contact Person Emaile",DataField = "ContactPersonEmail"},         
            };

            ReportConverter.CustomGridView(boundFields, model.Companies, fileName);
        }

        [AjaxAuthorize(Roles = "company-1,company-2,company-3")]
        public ActionResult Print(CompanyViewModel model)
        {
            List<Company> companies = CompanyManager.GetAllCompaniesBySearchKey(model.SearchByCompanyName);
            model.Companies = companies;
            return View("_CompanyPdfReportViewer", model);
        }
    }
}