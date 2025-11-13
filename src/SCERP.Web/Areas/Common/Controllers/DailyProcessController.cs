using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Common.Controllers
{
    public class DailyProcessController : BaseController
    {
        private readonly IDailyProcessManager _dailyProcessManager;

        public DailyProcessController(IDailyProcessManager dailyProcessManager)
        {
            _dailyProcessManager = dailyProcessManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save()
        {
            int saved=_dailyProcessManager.SendSmsTnaNotificationAlert();
            return ErrorResult(saved>0 ? "TNA Notification Message Send Successfully " : "Saved");
        }
	}
}