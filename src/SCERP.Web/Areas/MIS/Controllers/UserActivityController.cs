using System.Web.Mvc;
using SCERP.BLL.IManager.IMisManager;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.Web.Areas.MIS.Models.ViewModel;

namespace SCERP.Web.Areas.MIS.Controllers
{
    public class UserActivityController : Controller
    {
        private readonly IUserActivityManager _userActivityManager;
        private IUserManager _userManager;
        public UserActivityController(IUserActivityManager userActivityManager, IUserManager userManager)
        {
            _userActivityManager = userActivityManager;
            _userManager = userManager;
        }

        public ActionResult Index()
        {
         
            var activities = _userActivityManager.GetUserActivityList();
            UserActivityViewModel model = new UserActivityViewModel(activities);
            return View(model);
        }
	}
}