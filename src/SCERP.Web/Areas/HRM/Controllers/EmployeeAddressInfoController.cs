using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Web.Controllers;


namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeAddressInfoController : BaseHrmController
    {

        private Guid _employeeGuidId = Guid.Parse("00000000-0000-0000-0000-000000000000");

        [AjaxAuthorize(Roles = "employeeaddressinfo-1,employeeaddressinfo-2,employeeaddressinfo-3")]
        public ActionResult Index(SCERP.Model.Custom.EmployeeAddressInfoCustomModel model)
        {
            try
            {
                if (Session["EmployeeGuid"] != null)
                    _employeeGuidId = (Guid)Session["EmployeeGuid"];
                else
                {
                    return ErrorMessageResult();
                }
                var employeeAddressInfos = EmployeeAddressInfoManager.GetEmployeeAddressInfoByEmployeeGuidId(_employeeGuidId);
                model = employeeAddressInfos;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeeaddressinfo-2,employeeaddressinfo-3")]
        public ActionResult EditEmployeePresentAddress(Model.Custom.EmployeeAddressInfoCustomModel model)
        {
            try
            {
                ModelState.Clear();

                if (Session["EmployeeGuid"] != null)
                    _employeeGuidId = (Guid)Session["EmployeeGuid"];
                model.PresentCountries = CountryManager.GetAllCountries();

                var employeePresentAddress = EmployeeAddressInfoManager.GetEmployeePresentAddressById(_employeeGuidId, model.EmployeePresentAddressId);

                if (employeePresentAddress != null)
                {
                    model.EmployeePresentAddress.Id = model.EmployeePresentAddressId;
                    model.EmployeePresentAddress.EmployeeId = employeePresentAddress.EmployeeId;
                    model.EmployeePresentAddress.MailingAddress = employeePresentAddress.MailingAddress;
                    model.EmployeePresentAddress.MailingAddressInBengali = employeePresentAddress.MailingAddressInBengali;
                    model.EmployeePresentCountryId = employeePresentAddress.CountryId;
                    model.EmployeePresentDistrictId = employeePresentAddress.DistrictId;
                    model.EmployeePresentPoliceStationId = employeePresentAddress.PoliceStationId;
                    model.EmployeePresentAddress.PostOffice = employeePresentAddress.PostOffice;
                    model.EmployeePresentAddress.PostOfficeInBengali = employeePresentAddress.PostOfficeInBengali;
                    model.EmployeePresentAddress.PostCode = employeePresentAddress.PostCode;
                    model.EmployeePresentAddress.HomePhone = employeePresentAddress.HomePhone;
                    model.EmployeePresentAddress.MobilePhone = employeePresentAddress.MobilePhone;
                    model.EmployeePresentAddress.EmailAddress = employeePresentAddress.EmailAddress;
                    model.EmployeePresentAddress.AlternateEmailAddress = employeePresentAddress.AlternateEmailAddress;
                    model.EmployeePresentAddress.LandlordName = employeePresentAddress.LandlordName;
                    model.EmployeePresentAddress.LandlordNameInBengali = employeePresentAddress.LandlordNameInBengali;
                    model.EmployeePresentAddress.LandlordPhone = employeePresentAddress.LandlordPhone;
                    model.EmployeePresentAddress.EmergencyContactPerson = employeePresentAddress.EmergencyContactPerson;
                    model.EmployeePresentAddress.EmergencyContactPersonInBengali = employeePresentAddress.EmergencyContactPersonInBengali;
                    model.EmployeePresentAddress.ContactPersonRelation = employeePresentAddress.ContactPersonRelation;
                    model.EmployeePresentAddress.ContactPersonRelationInBengali = employeePresentAddress.ContactPersonRelationInBengali;
                    model.EmployeePresentAddress.ContactPersonPhone = employeePresentAddress.ContactPersonPhone;
                }

                model.PresentDistricts = DistrictManager.GetDistrictsByCountry(model.EmployeePresentCountryId);
                model.PresentPoliceStations = PoliceStationManager.GetPoliceStationByDistrict(model.EmployeePresentDistrictId);

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeeaddressinfo-2,employeeaddressinfo-3")]
        public ActionResult EditEmployeePermanentAddress(Model.Custom.EmployeeAddressInfoCustomModel model)
        {
            try
            {
                ModelState.Clear();

                if (Session["EmployeeGuid"] != null)
                    _employeeGuidId = (Guid)Session["EmployeeGuid"];
                model.PermanentCountries = CountryManager.GetAllCountries();

                var employeePermanentAddress = EmployeeAddressInfoManager.GetEmployeePermanentAddressById(_employeeGuidId, model.EmployeePermanentAddressId);

                if (employeePermanentAddress != null)
                {
                    model.EmployeePermanentAddress.Id = model.EmployeePermanentAddressId;
                    model.EmployeePermanentAddress.EmployeeId = employeePermanentAddress.EmployeeId;
                    model.EmployeePermanentAddress.MailingAddress = employeePermanentAddress.MailingAddress;
                    model.EmployeePermanentAddress.MailingAddressInBengali = employeePermanentAddress.MailingAddressInBengali;
                    model.EmployeePermanentCountryId = employeePermanentAddress.CountryId;
                    model.EmployeePermanentDistrictId = employeePermanentAddress.DistrictId;
                    model.EmployeePermanentPoliceStationId = employeePermanentAddress.PoliceStationId;
                    model.EmployeePermanentAddress.PostOffice = employeePermanentAddress.PostOffice;
                    model.EmployeePermanentAddress.PostOfficeInBengali = employeePermanentAddress.PostOfficeInBengali;
                    model.EmployeePermanentAddress.PostCode = employeePermanentAddress.PostCode;
                }

                model.PermanentDistricts = DistrictManager.GetDistrictsByCountry(model.EmployeePermanentCountryId);
                model.PermanentPoliceStations = PoliceStationManager.GetPoliceStationByDistrict(model.EmployeePermanentDistrictId);

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

         [AjaxAuthorize(Roles = "employeeaddressinfo-2,employeeaddressinfo-3")]
        public ActionResult SaveEmployeePresentAddress(EmployeeAddressInfoCustomModel model)
        {
            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];
            model.EmployeePresentAddress.EmployeeId = _employeeGuidId;

            var saveIndex = 0;

            try
            {

                var employeePresentAddress = EmployeeAddressInfoManager.GetEmployeePresentAddressById(_employeeGuidId, model.EmployeePresentAddress.Id) ?? new EmployeePresentAddress();

                employeePresentAddress.EmployeeId = model.EmployeePresentAddress.EmployeeId;
                employeePresentAddress.MailingAddress = model.EmployeePresentAddress.MailingAddress;
                employeePresentAddress.MailingAddressInBengali = model.EmployeePresentAddress.MailingAddressInBengali;
                employeePresentAddress.CountryId = model.EmployeePresentCountryId;
                employeePresentAddress.DistrictId = model.EmployeePresentDistrictId;
                employeePresentAddress.PoliceStationId = model.EmployeePresentPoliceStationId;
                employeePresentAddress.PostOffice = model.EmployeePresentAddress.PostOffice;
                employeePresentAddress.PostOfficeInBengali = model.EmployeePresentAddress.PostOfficeInBengali;
                employeePresentAddress.PostCode = model.EmployeePresentAddress.PostCode;
                employeePresentAddress.HomePhone = model.EmployeePresentAddress.HomePhone;
                employeePresentAddress.MobilePhone = model.EmployeePresentAddress.MobilePhone;
                employeePresentAddress.EmailAddress = model.EmployeePresentAddress.EmailAddress;
                employeePresentAddress.AlternateEmailAddress = model.EmployeePresentAddress.AlternateEmailAddress;
                employeePresentAddress.LandlordName = model.EmployeePresentAddress.LandlordName;
                employeePresentAddress.LandlordNameInBengali = model.EmployeePresentAddress.LandlordNameInBengali;
                employeePresentAddress.LandlordPhone = model.EmployeePresentAddress.LandlordPhone;
                employeePresentAddress.EmergencyContactPerson = model.EmployeePresentAddress.EmergencyContactPerson;
                employeePresentAddress.EmergencyContactPersonInBengali = model.EmployeePresentAddress.EmergencyContactPersonInBengali;
                employeePresentAddress.ContactPersonRelation = model.EmployeePresentAddress.ContactPersonRelation;
                employeePresentAddress.ContactPersonRelationInBengali = model.EmployeePresentAddress.ContactPersonRelationInBengali;
                employeePresentAddress.ContactPersonPhone = model.EmployeePresentAddress.ContactPersonPhone;

                if (model.EmployeePresentAddress.Id > 0)
                {
                    saveIndex = EmployeeAddressInfoManager.EditEmployeePresentAddress(employeePresentAddress);
                }
                else
                {
                    var latestEmployeePresentAddress = EmployeeAddressInfoManager.GetLatestEmployeePresentAddressByEmployeeGuidId(model.EmployeePresentAddress.EmployeeId);

                    if (latestEmployeePresentAddress != null)
                    {
                        EmployeeAddressInfoManager.UpdateEmployeePresentAddress(latestEmployeePresentAddress);
                    }

                    employeePresentAddress.Status = (int)StatusValue.Active;
                    saveIndex = EmployeeAddressInfoManager.SaveEmployeePresentAddress(employeePresentAddress);

                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            if (saveIndex > 0)
            {
                return RedirectToAction("Index");
            }
            return ErrorResult("Failed to save data!");

        }

         [AjaxAuthorize(Roles = "employeeaddressinfo-2,employeeaddressinfo-3")]
        public ActionResult SaveEmployeePermanentAddress(EmployeeAddressInfoCustomModel model)
        {
            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];
            model.EmployeePermanentAddress.EmployeeId = _employeeGuidId;

            var saveIndex = 0;

            try
            {

                var employeePermanentAddress = EmployeeAddressInfoManager.GetEmployeePermanentAddressById(_employeeGuidId, model.EmployeePermanentAddress.Id) ?? new EmployeePermanentAddress();

                employeePermanentAddress.EmployeeId = model.EmployeePermanentAddress.EmployeeId;
                employeePermanentAddress.MailingAddress = model.EmployeePermanentAddress.MailingAddress;
                employeePermanentAddress.MailingAddressInBengali = model.EmployeePermanentAddress.MailingAddressInBengali;
                employeePermanentAddress.CountryId = model.EmployeePermanentCountryId;
                employeePermanentAddress.DistrictId = model.EmployeePermanentDistrictId;
                employeePermanentAddress.PoliceStationId = model.EmployeePermanentPoliceStationId;
                employeePermanentAddress.PostOffice = model.EmployeePermanentAddress.PostOffice;
                employeePermanentAddress.PostOfficeInBengali = model.EmployeePermanentAddress.PostOfficeInBengali;
                employeePermanentAddress.PostCode = model.EmployeePermanentAddress.PostCode;

                if (model.EmployeePermanentAddress.Id > 0)
                {
                    saveIndex = EmployeeAddressInfoManager.EditEmployeePermanentAddress(employeePermanentAddress);
                }
                else
                {
                    var latestEmployeePermanentAddress = EmployeeAddressInfoManager.GetLatestEmployeePermanentAddressByEmployeeGuidId(model.EmployeePermanentAddress.EmployeeId);

                    if (latestEmployeePermanentAddress != null)
                    {
                        EmployeeAddressInfoManager.UpdateEmployeePermanentAddress(latestEmployeePermanentAddress);
                    }

                    employeePermanentAddress.Status = (int)StatusValue.Active;
                    saveIndex = EmployeeAddressInfoManager.SaveEmployeePermanentAddress(employeePermanentAddress);

                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            if (saveIndex > 0)
            {
                return RedirectToAction("Index");
            }
            return ErrorResult("Failed to save data!");

        }

        [AjaxAuthorize(Roles = "employeeaddressinfo-3")]
        public ActionResult DeleteEmployeePresentAddress(int EmployeePresentAddressId)
        {
            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];
            var deleteEmployeePresentAddress = 0;

            try
            {
                var employeePresentAddress = EmployeeAddressInfoManager.GetEmployeePresentAddressById(_employeeGuidId, EmployeePresentAddressId);
                deleteEmployeePresentAddress = EmployeeAddressInfoManager.DeleteEmployeePresentAddress(employeePresentAddress);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return deleteEmployeePresentAddress > 0 ? Reload() : ErrorResult();
        }

        [AjaxAuthorize(Roles = "employeeaddressinfo-3")]
        public ActionResult DeleteEmployeePermanentAddress(int EmployeePermanentAddressId)
        {
            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];

            var deleteEmployeePermanentAddress = 0;

            try
            {
                var employeePermanentAddress = EmployeeAddressInfoManager.GetEmployeePermanentAddressById(_employeeGuidId, EmployeePermanentAddressId);
                deleteEmployeePermanentAddress = EmployeeAddressInfoManager.DeleteEmployeePermanentAddress(employeePermanentAddress);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return deleteEmployeePermanentAddress > 0 ? Reload() : ErrorResult();
        }

        public ActionResult GetAllDistrictsByCountryId(int countryId)
        {
            var districts = DistrictManager.GetDistrictsByCountry(countryId);
            return Json(new { Success = true, Districts = districts }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllPoliceStationsByDistrictId(int districtId)
        {
            var policeStations = PoliceStationManager.GetPoliceStationByDistrict(districtId);
            return Json(new { Success = true, PoliceStations = policeStations }, JsonRequestBehavior.AllowGet);
        }


    }

}
