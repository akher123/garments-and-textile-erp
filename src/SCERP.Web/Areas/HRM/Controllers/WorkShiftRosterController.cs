using SCERP.Common;
using SCERP.Model;
using SCERP.Model.HRMModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class WorkShiftRosterController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        public ActionResult Index(WorkShiftRoster model)
        {
            ModelState.Clear();

            ViewBag.UnitName = new SelectList(new[]
                                                 {    new {Id = "", Value = "- Select -"}
                                                     , new {Id = "Dyeing", Value = "Dyeing"}
                                                     ,new {Id = "Knitting", Value = "Knitting"}}
                                                     , "Id", "Value", model.UnitName);

            ViewBag.GroupName = new SelectList(new[]
                                                {     new {Id = "", Value = "- Select -"}
                                                     , new {Id = "Group-1", Value = "Group-1 (General)"}
                                                     ,new {Id = "Group-2", Value = "Group-2"}
                                                     ,new {Id = "Group-3", Value = "Group-3"}}
                                                    , "Id", "Value", model.GroupName);


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

            int totalRecords = 0;
            model.shiftDetail = WorkShiftManager.GetWorkShiftRoster(model) ?? new List<WorkShiftRosterDetail>();
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult Edit(WorkShiftRoster model)
        {
            ModelState.Clear();

            ViewBag.UnitName = new SelectList(new[]
                                               {    new {Id = "", Value = "- Select -"}
                                                     , new {Id = "Dyeing", Value = "Dyeing"}
                                                     ,new {Id = "Knitting", Value = "Knitting"}}
                                                   , "Id", "Value", model.UnitName);

            ViewBag.GroupName = new SelectList(new[]
                                                {     new {Id = "", Value = "- Select -"}
                                                     , new {Id = "Group-1", Value = "Group-1"}
                                                     ,new {Id = "Group-2", Value = "Group-2"}
                                                     ,new {Id = "Group-3", Value = "Group-3"}}
                                                    , "Id", "Value", model.GroupName);

            return View(model);
        }

        public ActionResult Save(WorkShiftRoster model)
        {
            var saveIndex = 1;

            try
            {
                string[] lines = model.EmployeeCardId.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                foreach (var t in lines)
                {
                    model.EmployeeCardId = t;
                    WorkShiftManager.SaveWorkShiftRoster(model);
                }
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }


        public ActionResult WorkShiftRosterChangeIndex(WorkShiftRoster model)
        {
            ModelState.Clear();

            ViewBag.ShiftName = new SelectList(new[]
                                                    {
                                                          new {Id = "", Value = "- Select -"}
                                                         ,new {Id = "General", Value = "General"}
                                                         ,new {Id = "Day", Value = "Day"}
                                                         ,new {Id = "Night", Value = "Night"}
                                                    }
                                                    , "Id", "Value", model.GroupName);

            return View();
        }


        public ActionResult WorkShiftRosterChange(WorkShiftRoster model)
        {
            var saveIndex = 0;

            Employee employee = new Employee();

            if (!string.IsNullOrEmpty(model.EmployeeCardId))
                employee = EmployeeManager.GetEmployeeByCardId(model.EmployeeCardId);

            if (employee == null)
                return ErrorResult("Invalid Employee Id !");

            try
            {
                saveIndex = WorkShiftManager.ChangeWorkShiftRoster(model);
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }
    }
}