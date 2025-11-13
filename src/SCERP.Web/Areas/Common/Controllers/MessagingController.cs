using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.Model.CommonModel;
using SCERP.Web.Controllers;
using SCERP.Web.Hub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Common.Controllers
{
    public class MessagingController:BaseController 
    {
        private readonly IMessagingManager messagingManager;
        public MessagingController(IMessagingManager messagingManager)
        {
            this.messagingManager = messagingManager;
        }
        public ActionResult SendChatMessage(Messaging messaging)
        {
            messaging.SendTime = DateTime.Now;
            messaging.SenderId = PortalContext.CurrentUser.UserId;
            messaging.IsViewed = 0;
            int sent= messagingManager.SaveChatMessage(messaging);
            ChatHub.NotificationMessage();
            return Json(sent, JsonRequestBehavior.AllowGet);
        }

  
        public ActionResult GetChatMessage()
        {
            List<Dropdown> LoginUsers = messagingManager.GetLoginusers(PortalContext.CurrentUser.CompId);
            LoginUsers = LoginUsers.Select(x =>
            {
                x.Value = Url.Content(x.Value);
                return x;
            }).ToList();
            ViewBag.LoginUsers = LoginUsers;
            List<MessagingNotification> notifications = messagingManager.GetChatMessage(PortalContext.CurrentUser.UserId);
             notifications = notifications.Select(x =>
             {
                 x.PhotographPath = Url.Content(x.PhotographPath);
                 return x;
             }).ToList();
            return PartialView("~/Views/Shared/_ChatNotification.cshtml", notifications);
        }

    }
} 