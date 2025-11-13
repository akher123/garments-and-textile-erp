using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class WorkShiftController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "workshift-1,workshift-2,workshift-3")]
        public ActionResult Index(WorkShiftViewModel model)
        {
            ModelState.Clear();
            var startPage = 0;
            var totalRecords = 0;
            model.Name = model.SearchKey;
            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }
            //if (model.IsSearch)
            //{
            //    model.IsSearch = false;
            //    return View(model);
            //}
            model.WorkShifts = WorkShiftManager.GetAllWorkShiftsByPaging(startPage, _pageSize, model, out totalRecords) ?? new List<WorkShift>();
            model.TotalRecords = totalRecords;
            return View(model);
        }

        [AjaxAuthorize(Roles = "workshift-2,workshift-3")]
        public ActionResult Edit(WorkShiftViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.WorkShiftId > 0)
                {
                    var workShiftObj = WorkShiftManager.GetWorkShiftById(model.WorkShiftId);
                    model.InTime = DateTime.Today.Add(workShiftObj.StartTime).ToString("hh:mm tt");
                    model.OutTime = DateTime.Today.Add(workShiftObj.EndTime).ToString("hh:mm tt");
                    model.Name = workShiftObj.Name;
                    model.NameInBengali = workShiftObj.NameInBengali;
                    model.NameDetail = workShiftObj.NameDetail;
                    model.NameDetailInBengali = workShiftObj.NameDetailInBengali;
                    model.InBufferTime = workShiftObj.InBufferTime;
                    model.MaxBeforeTime = workShiftObj.MaxBeforeTime;
                    model.MaxAfterTime = workShiftObj.MaxAfterTime;
                    model.ExceededMaxAfterTime = workShiftObj.ExceededMaxAfterTime;
                    model.Description = workShiftObj.Description;
                    model.CreatedDate = workShiftObj.CreatedDate;
                    model.CreatedBy = workShiftObj.CreatedBy;
                    model.EditedBy = workShiftObj.EditedBy;
                    model.IsActive = workShiftObj.IsActive;

                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "workshift-2,workshift-3")]
        public ActionResult Save(WorkShiftViewModel model)
        {
            var saveIndex = 0;
            try
            {
                var isExist = WorkShiftManager.IsWorkShiftExist(model);


                switch (isExist)
                {
                    case false:
                        var workShiftObj = WorkShiftManager.GetWorkShiftById(model.WorkShiftId) ?? new WorkShift();
                        workShiftObj.Name = model.Name;
                        workShiftObj.NameInBengali = model.NameInBengali;
                        workShiftObj.NameDetail = model.NameDetail;
                        workShiftObj.NameDetailInBengali = model.NameDetailInBengali;
                        workShiftObj.StartTime = DateTime.ParseExact(model.InTime, "h:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                        workShiftObj.EndTime = DateTime.ParseExact(model.OutTime, "h:mm tt", CultureInfo.InvariantCulture).TimeOfDay.Add(new TimeSpan(0,0,0,59));

                        workShiftObj.InBufferTime = model.InBufferTime;
                        workShiftObj.MaxBeforeTime = model.MaxBeforeTime;
                        workShiftObj.MaxAfterTime = model.MaxAfterTime;
                        workShiftObj.ExceededMaxAfterTime = model.ExceededMaxAfterTime;
                        workShiftObj.Description = model.Description;
                        workShiftObj.CreatedDate = model.CreatedDate;
                        workShiftObj.EditedDate = model.EditedDate;
                        workShiftObj.CreatedBy = model.CreatedBy;
                        workShiftObj.EditedBy = model.EditedBy;
                        saveIndex = model.WorkShiftId > 0 ? WorkShiftManager.EditWorkShift(workShiftObj) : WorkShiftManager.SaveWorkShift(workShiftObj);
                        break;
                    case true:
                        return ErrorResult(model.Name + " " + "WorkShift already exist");

                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "workshift-3")]
        public ActionResult Delete(WorkShiftViewModel model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = WorkShiftManager.DeleteWorkShift(model.WorkShiftId);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");
        }


        [AjaxAuthorize(Roles = "workshift-1,workshift-2,workshift-3")]
        public void GetExcel(WorkShiftViewModel model)
        {
            try
            {
                model.WorkShifts = WorkShiftManager.GetAllWorkShiftsBySearchKey(model.SearchKey);
                const string fileName = "WorkShift";
                var boundFields = new List<BoundField>
            {
                new BoundField() {HeaderText = @"WorkShift Name", DataField = "Name"},
                new BoundField() {HeaderText = @"WorkShift Name (Detail)", DataField = "Name"},
                new BoundField() {HeaderText = @"Start Time", DataField = "StartTime"},
                new BoundField() {HeaderText = @"End Time", DataField = "EndTime"},                  
                new BoundField() {HeaderText = @"Buffer In Time", DataField = "InBufferTime"},
                new BoundField() {HeaderText = @"Buffer Out Time", DataField = "OutBufferTime"}, 
                new BoundField() {HeaderText = @"Description", DataField = "Description"},
            };
                ReportConverter.CustomGridView(boundFields, model.WorkShifts, fileName);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

        }

        [AjaxAuthorize(Roles = "workshift-1,workshift-2,workshift-3")]
        public ActionResult Print(WorkShiftViewModel model)
        {
            try
            {
                model.WorkShifts = WorkShiftManager.GetAllWorkShiftsBySearchKey(model.SearchKey);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return PartialView("_WorkShiftPrintPreview", model);

        }

    }
}
