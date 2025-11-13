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
using SCERP.Web.Controllers;


namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeFamilyInfoController : BaseHrmController
    {

        private Guid _employeeGuidId = Guid.Parse("00000000-0000-0000-0000-000000000000");

        [AjaxAuthorize(Roles = "employeefamilyinfo-1,employeefamilyinfo-2,employeefamilyinfo-3")]
        public ActionResult Index(Models.ViewModels.EmployeeFamilyInfoViewModel model)
        {
            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];
            else
            {
                return ErrorMessageResult();
            }
            var employeeFamilyInfos = EmployeeFamilyInfoManager.GetEmployeeFamilyInfoByEmployeeGuidId(_employeeGuidId);
            model.EmployeeFamilyInfos = employeeFamilyInfos;
            return View(model);
        }

        [AjaxAuthorize(Roles = "employeefamilyinfo-2,employeefamilyinfo-3")]
        public ActionResult Edit(Models.ViewModels.EmployeeFamilyInfoViewModel model)
        {
            ModelState.Clear();

            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];
            model.Genders = GenderManager.GetAllGenders();
            var employeeFamilyInfo = EmployeeFamilyInfoManager.GetEmployeeFamilyInfoById(_employeeGuidId, model.EmployeeFamilyInfoId);

            if (employeeFamilyInfo != null)
            {
                model.EmployeeId = _employeeGuidId;
                model.NameOfChild = employeeFamilyInfo.NameOfChild;
                model.NameOfChildInBengali = employeeFamilyInfo.NameOfChildInBengali;
                model.DateOfBirth = employeeFamilyInfo.DateOfBirth;
                model.GenderId = employeeFamilyInfo.GenderId;
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeefamilyinfo-2,employeefamilyinfo-3")]
        public ActionResult Save(Models.ViewModels.EmployeeFamilyInfoViewModel model)
        {
            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];
            model.EmployeeId = _employeeGuidId;
            var saveIndex = 0;
            try
            {
                var isExist = EmployeeFamilyInfoManager.CheckExistingEmployeeFamilyInfo(model);

                if (isExist)
                {
                    return ErrorResult("Same family information for this person already exist!");
                }
                var employeeFamilyInfo = EmployeeFamilyInfoManager.GetEmployeeFamilyInfoById(model.EmployeeId, model.EmployeeFamilyInfoId) ?? new EmployeeFamilyInfo();
                employeeFamilyInfo.EmployeeId = model.EmployeeId;
                employeeFamilyInfo.NameOfChild = model.NameOfChild;
                employeeFamilyInfo.NameOfChildInBengali = model.NameOfChildInBengali;
                employeeFamilyInfo.DateOfBirth = model.DateOfBirth;
                employeeFamilyInfo.GenderId = model.GenderId;
                saveIndex = model.EmployeeFamilyInfoId > 0 ? EmployeeFamilyInfoManager.EditEmployeeFamilyInfo(employeeFamilyInfo) : EmployeeFamilyInfoManager.SaveEmployeeFamilyInfo(employeeFamilyInfo);
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

        [AjaxAuthorize(Roles = "employeefamilyinfo-3")]
        public ActionResult Delete(int EmployeeFamilyInfoId)
        {
            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];
            var deleteEmployeeFamilyInfo = 0;

            try
            {
                var employeeFamilyInfo = EmployeeFamilyInfoManager.GetEmployeeFamilyInfoById(_employeeGuidId, EmployeeFamilyInfoId);
                deleteEmployeeFamilyInfo = EmployeeFamilyInfoManager.DeleteEmployeeFamilyInfo(employeeFamilyInfo);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return deleteEmployeeFamilyInfo > 0 ? Reload() : ErrorResult();
        }

    }

}
