using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Model;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class LeaveTypesController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "leavetype-1,leavetype-2,leavetype-3")]
        public ActionResult Index(int? page, string sort, LeaveTypeViewModel model)
        {
            try
            {
                var startPage = 0;
                var totalRecords = 0;
                model.Title = model.SearchKey;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                //if (model.IsSearch)
                //{
                //    model.IsSearch = false;
                //    return View(model);
                //}
                model.LeaveTypes = LeaveTypeManager.GetAllLeaveTypes(startPage, _pageSize, out totalRecords, model) ?? new List<LeaveType>();
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "leavetype-2,leavetype-3")]
        public ActionResult Edit(LeaveTypeViewModel model)
        {

            try
            {
                ModelState.Clear();
                if (model.Id > 0)
                {
                    var leaveType = LeaveTypeManager.GetLeaveTypeById(model.Id);
                    model.Title = leaveType.Title;
                    model.TitleInBengali = leaveType.TitleInBengali;
                    model.Description = leaveType.Description;
                }
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "leavetype-2,leavetype-3")]
        public ActionResult Save(LeaveType model)
        {
            var saveIndex = 0;
            try
            {
                var isExist = LeaveTypeManager.IsLeaveTypeExist(model);
                switch (isExist)
                {
                    case false:
                        var leaveType = LeaveTypeManager.GetLeaveTypeById(model.Id) ?? new LeaveType();
                        leaveType.Title = model.Title;
                        leaveType.TitleInBengali = model.TitleInBengali;
                        leaveType.Description = model.Description;
                        saveIndex = model.Id != 0
                            ? LeaveTypeManager.EditLeaveType(leaveType)
                            : LeaveTypeManager.SaveLeaveType(leaveType);
                        break;
                    case true:
                        return ErrorResult(model.Title + ",Leave type already exist");

                }
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");

        }

        [AjaxAuthorize(Roles = "leavetype-3")]
        public ActionResult Delete(int id)
        {
            var deleteIndex = 0;
            try
            {
                var leaveType = LeaveTypeManager.GetLeaveTypeById(id);
                deleteIndex = LeaveTypeManager.DeleteLeaveType(leaveType);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete!");

        }

        [AjaxAuthorize(Roles = "leavetype-1,leavetype-2,leavetype-3")]
        public void GetExcel(LeaveTypeViewModel model)
        {

            try
            {
                model.Title = model.SearchKey;
                model.LeaveTypes = LeaveTypeManager.GetLeaveTypeBySearchKey(model);
                const string fileName = "LeaveType";
                var boundFields = new List<BoundField>
            {
                new BoundField(){HeaderText = @"Leave Type",DataField = "Title"},
                   new BoundField(){HeaderText = @"Description",DataField = "Description"},
          
            };
                ReportConverter.CustomGridView(boundFields, model.LeaveTypes, fileName);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

        }


        [AjaxAuthorize(Roles = "leavetype-1,leavetype-2,leavetype-3")]
        public ActionResult Print(LeaveTypeViewModel model)
        {
            try
            {
                model.Title = model.SearchKey;
                model.LeaveTypes = LeaveTypeManager.GetLeaveTypeBySearchKey(model);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return PartialView("_LeaveTypePrintPreview", model);
        }
    }
}
