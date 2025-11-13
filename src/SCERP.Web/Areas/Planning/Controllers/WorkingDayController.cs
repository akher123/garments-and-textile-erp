using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.Web.Areas.Planning.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Planning.Controllers
{
    public class WorkingDayController : BaseController
    {
        private readonly IWorkingDayManager _workingDayManager;
        public WorkingDayController(IWorkingDayManager workingDayManager)
        {
            _workingDayManager = workingDayManager;
        }
        public ActionResult Index(WorkingDayViewModel model)
        {
            ModelState.Clear();
            int  totalRecords;
            int index = model.PageIndex;
            model.WorkingDays = _workingDayManager.GetWorkingDay( model.WorkingDayStatus,model.FromDate, model.ToDate, index,  out  totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult Save(WorkingDayViewModel model)
        {
            try
            {
                int saveIndex = 0;
                if (model.WorkingDay.WorkingDayId > 0)
                {
                    model.WorkingDay.DayStatus = (model.HolidayStatus
                        ? (int)WorkingDayStatus.Holiday
                        : (int)WorkingDayStatus.WorkingDay);
                    saveIndex = _workingDayManager.EditWorkingDays(model.WorkingDay);
                }
                else
                {
                    saveIndex = _workingDayManager.CreateWorkingDays(model.CureentYar);
                }
                return saveIndex > 0 ? Reload() : ErrorResult("Saved Failed !");
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }

           
        }

        public ActionResult Edit(WorkingDayViewModel model)
        {
            if (model.WorkingDay.WorkingDayId>0)
            {
           
                model.WorkingDay = _workingDayManager.GetWorkingDayById(model.WorkingDay.WorkingDayId);
                model.HolidayStatus = (model.WorkingDay.DayStatus == (int)WorkingDayStatus.Holiday);
                return View(model);
            }
            model.Yars = Enumerable.Range(DateTime.Now.Year, 5).ToList();
            return View("Create",model);
        }
	}
}