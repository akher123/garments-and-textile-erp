using System;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
//using iTextSharp.text.pdf.qrcode;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Model;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class AttendanceBonusSettingController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        //[AjaxAuthorize(Roles = "AttendanceBonusSetting-1,AttendanceBonusSetting-2,AttendanceBonusSetting-3")]
        public ActionResult Index(AttendanceBonusSettingViewModel model)
        {
            ModelState.Clear();

            AttendanceBonusSetting attendanceBonusSetting = model;
            attendanceBonusSetting.FromDate = model.SearchByFromDate;
            attendanceBonusSetting.ToDate = model.SearchByToDate;

            if (model.IsSearch)
            {
                model.IsSearch = false;
                return View(model);
            }

            var startPage = 0;
            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }

            var totalRecords = 0;
            model.AttendanceBonusSettings = AttendanceBonusSettingManager.GetAllAttendanceBonusSettingsByPaging(startPage, _pageSize, out totalRecords, attendanceBonusSetting) ?? new List<AttendanceBonusSetting>();
            model.TotalRecords = totalRecords;

            return View(model);
        }

        //[AjaxAuthorize(Roles = "AttendanceBonusSetting-2,AttendanceBonusSetting-3")]
        public ActionResult Edit(AttendanceBonusSettingViewModel model)
        {
            ModelState.Clear();

            try
            {
                if (model.AttendanceBonusSettingId > 0)
                {
                    var attendanceBonusSetting = AttendanceBonusSettingManager.GetAttendanceBonusSettingById(model.AttendanceBonusSettingId);
                    model.AbsentDays = attendanceBonusSetting.AbsentDays;
                    model.LateDays = attendanceBonusSetting.LateDays;
                    model.LeavewithoutApplication = attendanceBonusSetting.LeavewithoutApplication;
                    model.LeavewithApplication = attendanceBonusSetting.LeavewithApplication;
                    model.FromDate = attendanceBonusSetting.FromDate;
                    model.ToDate = attendanceBonusSetting.ToDate;

                    ViewBag.Title = "Edit AttendanceBonus Setting";
                }
                else
                {
                    ViewBag.Title = "Add AttendanceBonus Setting";
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        //[AjaxAuthorize(Roles = "AttendanceBonusSetting-2,AttendanceBonusSetting-3")]
        public ActionResult Save(AttendanceBonusSettingViewModel model)
        {
            var attendanceBonusSetting = AttendanceBonusSettingManager.GetAttendanceBonusSettingById(model.AttendanceBonusSettingId) ?? new AttendanceBonusSetting();
            attendanceBonusSetting.AbsentDays = model.AbsentDays;
            attendanceBonusSetting.LateDays = model.LateDays;
            attendanceBonusSetting.LeavewithoutApplication = model.LeavewithoutApplication;
            attendanceBonusSetting.LeavewithApplication = model.LeavewithApplication;
            attendanceBonusSetting.FromDate = model.FromDate;
            attendanceBonusSetting.FromDate = model.FromDate;

            var saveIndex = (model.AttendanceBonusSettingId > 0) ? AttendanceBonusSettingManager.EditAttendanceBonusSetting(attendanceBonusSetting) : AttendanceBonusSettingManager.SaveAttendanceBonusSetting(attendanceBonusSetting);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        //[AjaxAuthorize(Roles = "AttendanceBonusSetting-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var attendanceBonusSetting = AttendanceBonusSettingManager.GetAttendanceBonusSettingById(id) ?? new AttendanceBonusSetting();
            deleted = AttendanceBonusSettingManager.DeleteAttendanceBonusSetting(attendanceBonusSetting);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }
    }
}
