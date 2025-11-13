using System.IO;
using System.Web.UI.WebControls;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Web.Helpers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class AuthorizedPersonController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "authorizedperson-1,authorizedperson-2,authorizedperson-3")]
        public ActionResult Index(AuthorizedPersonViewModel model)
        {
            try
            {
                ModelState.Clear();
                var authorizationTypes = AuthorizedPersonManager.GetAllAuthorizedType();
                model.AuthorizationTypes = authorizationTypes;
                model.AuthorizationTypeId = model.SearchByAuthorizationTypeId;
                //if (!model.IsSearch)
                //{
                //    model.IsSearch = true;
                //    return View(model);
                //}

                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                var totalRecords = 0;
                model.AuthorizedPersons = AuthorizedPersonManager.GetAllAuthorizedPersonsByPaging(startPage, _pageSize, model, model.SearchByAuthorizedPerson, out totalRecords) ?? new List<AuthorizedPerson>();
                model.TotalRecords = totalRecords;

            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "authorizedperson-2,authorizedperson-3")]
        public ActionResult Edit(AuthorizedPersonViewModel model)
        {
            ModelState.Clear();
            try
            {
                var authorizationTypes = AuthorizedPersonManager.GetAllAuthorizedType();
                model.AuthorizationTypes = authorizationTypes;
                if (model.Id > 0)
                {
                    var authorizedPerson = AuthorizedPersonManager.GetAuthorizedPersonById(model.Id);
                    model.AuthorizationTypeId = authorizedPerson.AuthorizationTypeId;
                    model.EmployeeCardId = authorizedPerson.Employee.EmployeeCardId;
                    model.EmployeeCompanyInfo = EmployeeDailyAttendanceManager.GetEmployeeByEmployeeCardId(authorizedPerson.Employee.EmployeeCardId);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "authorizedperson-2,authorizedperson-3")]
        public JsonResult Save(AuthorizedPersonViewModel model)
        {
            ModelState.Clear();
            var isExist = AuthorizedPersonManager.CheckExistingAuthorizedPerson(model);
            if (isExist)
            {
                return ErrorResult("Authorized Person already exist");
            }

            var saveIndex = (model.Id > 0) ? AuthorizedPersonManager.EditAuthorizedPerson(model) : AuthorizedPersonManager.SaveAuthorizedPerson(model);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "authorizedPerson-3")]
        public JsonResult Delete(int id)
        {
            var deleted = 0;
            deleted = AuthorizedPersonManager.DeleteAuthorizedPerson(id);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        public ActionResult GetEmployeeDetailByEmployeeCadId(AuthorizedPersonViewModel model)
        {

            var checkEmployeeCardId =
                EmployeeManager.CheckExistingEmployeeCardNumber(new Employee() { EmployeeCardId = model.EmployeeCardId });
            if (!checkEmployeeCardId)
            {
                return Json(new { Message = "Invalid ID or Access Denied!", ValidStatus = false }, JsonRequestBehavior.AllowGet);
            }

            Employee employee = EmployeeManager.GetEmployeeByCardId(model.EmployeeCardId);
            User user = UserManager.GetUserByEmployeeId(employee.EmployeeId);

            if (user == null)
            {
                return Json(new { Message = "This employee is not a system user!", ValidStatus = false }, JsonRequestBehavior.AllowGet);
            }

            var employeeDetails = EmployeeDailyAttendanceManager.GetEmployeeByEmployeeCardId(model.EmployeeCardId);
            return Json(new { EmployeeDetailView = RenderViewToString("_EmployeeDetails", employeeDetails), Success = true }, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "authorizedperson-1,authorizedperson-2,authorizedperson-3")]
        public void GetExcel(AuthorizedPersonViewModel model)
        {
            try
            {
                model.AuthorizedPersons = AuthorizedPersonManager.GetAllAuthorizedPersonBySearchKey(model.AuthorizationTypeId, model.SearchByAuthorizedPerson);
                const string fileName = "uthorizedPerson";
                var boundFields = new List<BoundField>
                 {
                   new BoundField(){HeaderText = @"Authorized Person",DataField = "Employee.Name"},
                        new BoundField(){HeaderText = @"Authorized Person ID",DataField = "Employee.EmployeeCardId"},
                    new BoundField(){HeaderText = @"AuthorizationType",DataField = "AuthorizationType.TypeName"},
                 };
                ReportConverter.CustomGridView(boundFields, model.AuthorizedPersons, fileName);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

        }

        [AjaxAuthorize(Roles = "authorizedperson-1,authorizedperson-2,authorizedperson-3")]
        public ActionResult Print(AuthorizedPersonViewModel model)
        {
            try
            {
                model.AuthorizedPersons = AuthorizedPersonManager.GetAllAuthorizedPersonBySearchKey(model.AuthorizationTypeId, model.SearchByAuthorizedPerson);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return PartialView("_AuthorizedPersonReport", model);
        }
    }

}
