using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.Model.Planning;
using SCERP.Web.Areas.Planning.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Planning.Controllers
{
    public class DailyLineLayoutController : BaseController
    {
        private readonly IDailyLineLayoutManager _dailyLineLayoutManager;

        public DailyLineLayoutController(IDailyLineLayoutManager dailyLineLayoutManager)
        {
            _dailyLineLayoutManager = dailyLineLayoutManager;
        }
        public ActionResult Index(DailyLineLayoutViewModel model)
        {
            ModelState.Clear();
            model.FromDate = model.FromDate ?? DateTime.Now.Date;
            model.OutputDate = model.FromDate;
            List<PLAN_DailyLineLayout> dailyLineLayouts = _dailyLineLayoutManager.GetDailyLineLayout(
            ProcessCode.SEWING, model.FromDate, PortalContext.CurrentUser.CompId);
            model.DailyLineLayouts = dailyLineLayouts.ToDictionary(x =>Convert.ToString(x.LineId), x => x);
            return View(model);
        }

        public ActionResult Save(DailyLineLayoutViewModel model)
        {
            List<PLAN_DailyLineLayout> planDailyLineLayouts = model.DailyLineLayouts.Select(x =>
            {
                x.Value.OutputDate = model.OutputDate.GetValueOrDefault();
                x.Value.CompId = PortalContext.CurrentUser.CompId;
                return x.Value;
            }).ToList();
            int saved = _dailyLineLayoutManager.SaveDailyLineLayout(planDailyLineLayouts);
            if (saved > 0)
            {
                return ErrorResult("Saved Successfully");
            }
            else
            {
                return ErrorResult("Fail to save line wise machine plan");
            }
        }
	}
}