using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class NotificationTemplateController : BaseController
    {

        private readonly INotificationTemplateManager _notificationTemplateManager;
        private readonly IOmBuyerManager _omBuyerManager;
        public NotificationTemplateController(INotificationTemplateManager notificationTemplateManager, IOmBuyerManager omBuyerManager) 
        {
            _notificationTemplateManager = notificationTemplateManager;
            _omBuyerManager = omBuyerManager;
        }
        public ActionResult Index(NotificationTemplateViewModel model)
        {
            ModelState.Clear();
            int totalRecords = 0;
            model.NotificationTemplates = _notificationTemplateManager.GetAllNotificatiosByPagin(model.PageIndex, model.sort, model.sortdir, model.FromDate, model.ToDate, model.SearchString, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult Edit(NotificationTemplateViewModel model)
        {
            ModelState.Clear();
            if (model.NotificationTemplate.NotificationTemplateId > 0)
            {
                model.NotificationTemplate = _notificationTemplateManager.GetNotificationById(model.NotificationTemplate.NotificationTemplateId);
                model.Activities = _notificationTemplateManager.GetActivityListByBuyerRefId(model.NotificationTemplate.BuyerRefId);
            }
            model.OmBuyers = _omBuyerManager.GetAllBuyers();
            return View(model);
        }

       
        public ActionResult Save(NotificationTemplateViewModel model)
        {
            int savedIndex = 0;
            try
            {
                model.NotificationTemplate.CompId = PortalContext.CurrentUser.CompId;
                savedIndex = model.NotificationTemplate.NotificationTemplateId>0 ? _notificationTemplateManager.Edit(model.NotificationTemplate) : _notificationTemplateManager.Save(model.NotificationTemplate);
              
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return savedIndex > 0 ? Reload() : ErrorResult("Save Failed");
        }

        public ActionResult Delete(int id)
        {
            var deleleteIndex = _notificationTemplateManager.Delete(id);
            return deleleteIndex > 0 ? Reload() : ErrorResult("Delete Failed");
        }

        public JsonResult GetActivityListByBuyerRefId(string buyerRefId)
        {
            var activities = _notificationTemplateManager.GetActivityListByBuyerRefId(buyerRefId);
            return Json(activities, JsonRequestBehavior.AllowGet);
        }
	}
}