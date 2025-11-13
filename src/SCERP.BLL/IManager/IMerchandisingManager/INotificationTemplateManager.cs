using System;
using System.Collections;
using System.Collections.Generic;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
     public interface INotificationTemplateManager
     {
         List<OM_NotificationTemplate> GetAllNotificatiosByPagin(int pageIndex, string sort, string sortdir, DateTime? fromDate, DateTime? toDate, string searchString, out int totalRecords);
         OM_NotificationTemplate GetNotificationById(int notificationTemplateId);
        int Edit(OM_NotificationTemplate model);
        int Save(OM_NotificationTemplate model);
        int Delete(int? id);

        List<OM_TnaActivity> GetActivityListByBuyerRefId(string buyerRefId);
     }
}
