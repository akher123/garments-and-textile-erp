using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class HolidaySetupController : BaseHrmController
    {
     
        [AjaxAuthorize(Roles = "holiday-1,holiday-2,holiday-3")]
        public ActionResult Index()
        {
            var weekends = HolidaySetupManager.GetAllWeekends();
            var dayNames = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            ViewBag.dayNames = dayNames;
            return View(weekends);
        }

        [AjaxAuthorize(Roles = "holiday-1,holiday-2,holiday-3")]
        public ActionResult UpdateWeekends(List<int> weekends)
        {

            var weekend = HolidaySetupManager.UpdateWeekends(weekends);
            if (weekend <= 0) return ErrorResult();
            var allWeekends = HolidaySetupManager.GetAllActiveWeekends();
            return Json(allWeekends, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "holiday-1,holiday-2,holiday-3")]
        public ActionResult GetWeekends()
        {
            var allWeekends = HolidaySetupManager.GetAllActiveWeekends();
            return Json(allWeekends, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "holiday-2,holiday-3")]
        public ActionResult Save(HolidaysSetup holidaysSetup)
        {

            var holidaysSetupObj = HolidaySetupManager.GetHolidayById(holidaysSetup.Id) ?? new HolidaysSetup();
            holidaysSetupObj.StartDate = holidaysSetup.StartDate;
            holidaysSetupObj.EndDate = holidaysSetup.EndDate;
            holidaysSetupObj.Title = holidaysSetup.Title;
            holidaysSetupObj.Description = holidaysSetup.Description;
            var monthPositon = holidaysSetup.StartDate.Month - 1;
            if (holidaysSetup.Id != 0)
            {
                var holiday = HolidaySetupManager.EditHoliday(holidaysSetupObj);
                return holiday > 0 ? Json(monthPositon, JsonRequestBehavior.AllowGet) : ErrorResult();
            }
            else
            {
                var holiday = HolidaySetupManager.SaveHolidaye(holidaysSetupObj);
                return holiday > 0 ? Json(monthPositon, JsonRequestBehavior.AllowGet) : ErrorResult();
            }
        }

        [AjaxAuthorize(Roles = "holiday-1,holiday-2,holiday-3")]
        public JsonResult GetAllHolidays()
        {
            var holidayes = HolidaySetupManager.GetAllHolidays().Select(x => new { Id = x.Id, title = x.Title, start = x.StartDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), end = x.EndDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") }).ToList();

            return Json(holidayes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult HolidayDetails(int id)
        {
            var holidaysSetup = HolidaySetupManager.GetHolidayDetails(id);
            return Json(holidaysSetup, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "holiday-2,holiday-3")]
        public ActionResult CreateHoliday(HolidaysSetup holiday)
        {
            ModelState.Clear();
            var holidaysSetup = holiday.Id > 0 ? HolidaySetupManager.GetHolidayById(holiday.Id) : holiday;
            return View(holidaysSetup);
        }

        [AjaxAuthorize(Roles = "holiday-3")]
        public ActionResult DeleteHoliday(int id)
        {
            var delete = HolidaySetupManager.DeleteHoliday(id);
            return delete == 0 ? Reload() : ErrorResult();
        }
    }
}