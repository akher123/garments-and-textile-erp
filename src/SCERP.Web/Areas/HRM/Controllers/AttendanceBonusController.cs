using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf.qrcode;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;


namespace SCERP.Web.Areas.HRM.Controllers
{
    public class AttendanceBonusController : BaseHrmController
    {

        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "attendancebonus-1,attendancebonus-2,attendancebonus-3")]
        public ActionResult Index(AttendanceBonusViewModel model)
        {
            try
            {
                ModelState.Clear();

                var totalRecords = 0;
                var startPage = 0;

                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }

                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeType(model.SearchFieldModel.SearchByEmployeeTypeId);

                //if (model.IsSearch)
                //{
                //    model.IsSearch = false;
                //    return View(model);
                //}
                model.AttendanceBonuses = AttendanceBonusManager.GetAttendanceBonusByPaging(startPage, _pageSize, out totalRecords, model.SearchFieldModel, model);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "attendancebonus-2,attendancebonus-3")]
        public ActionResult Save(AttendanceBonusViewModel model)
        {
            var saveIndex = 0;

            try
            {
                AttendanceBonus attendance = new AttendanceBonus();
                attendance.AttendanceBonusId = model.AttendanceBonusId;              
                attendance.DesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;
                attendance.Amount = model.Amount;
                attendance.FromDate = model.FromDate;
                attendance.ToDate = model.ToDate;
                saveIndex = model.AttendanceBonusId > 0 ? AttendanceBonusManager.EditAttendanceBonus(attendance) : AttendanceBonusManager.SaveAttendanceBonus(attendance);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "attendancebonus-2,attendancebonus-3")]
        public ActionResult Edit(AttendanceBonusViewModel model)
        {
            ModelState.Clear();

            try
            {

                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                if (model.AttendanceBonusId > 0)
                {
                    AttendanceBonus attendanceBonus = AttendanceBonusManager.GetAttendanceBonusById(model.AttendanceBonusId);

                    model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeType(attendanceBonus.EmployeeDesignation.EmployeeTypeId);

                    model.SearchFieldModel.SearchByEmployeeTypeId = attendanceBonus.EmployeeDesignation.EmployeeTypeId;
                    model.SearchFieldModel.SearchByEmployeeDesignationId = attendanceBonus.DesignationId;

                    model.Amount = attendanceBonus.Amount;

                    model.CreatedBy = attendanceBonus.CreatedBy;
                    model.EditedBy = attendanceBonus.EditedBy;
                    model.CreatedDate = attendanceBonus.CreatedDate;
                    model.EditedDate = attendanceBonus.EditedDate;
                    model.FromDate = attendanceBonus.FromDate;
                    model.ToDate = attendanceBonus.ToDate;

                    model.IsActive = attendanceBonus.IsActive;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "attendancebonus-3")]
        public ActionResult Delete(AttendanceBonus model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = AttendanceBonusManager.DeleteAttendanceBonus(model.AttendanceBonusId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        public void GetExcel(int searchByEmployeeTypeId,int? searchByEmployeeDesignationId)
        {
            List<AttendanceBonus> designationsForAttendanceBonuses = AttendanceBonusManager.GetAttendanceBonusBySearchKey(searchByEmployeeTypeId, searchByEmployeeDesignationId);

            const string fileName = "DesignationsForAttendanceBonuses";
            var boundFields = new List<BoundField>
            {
               new BoundField(){HeaderText = @"Designation",DataField = "EmployeeDesignation.Title"},
               new BoundField(){HeaderText = @"Amount",DataField = "Amount"},
               new BoundField(){HeaderText = @"FromDate",DataField = "FromDate"},
               new BoundField(){HeaderText = @"ToDate",DataField = "ToDate"},
            };
            ReportConverter.CustomGridView(boundFields, designationsForAttendanceBonuses, fileName);
        }

        public ActionResult GetEmployeeDesignationByEmployeeTypeId(int employeeTypeId)
        {
            var designations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeType(employeeTypeId);
            return Json(new { Success = true, EmployeeDesignations = designations }, JsonRequestBehavior.AllowGet);
        }
    }
}