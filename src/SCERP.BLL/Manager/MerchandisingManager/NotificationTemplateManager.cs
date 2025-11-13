using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository;
using SCERP.DAL.Repository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class NotificationTemplateManager : INotificationTemplateManager
    {
        private readonly IRepository<OM_NotificationTemplate> _notificationTemplateRepository;
        public NotificationTemplateManager(IRepository<OM_NotificationTemplate> notificationTemplateRepository)
        {
            _notificationTemplateRepository = notificationTemplateRepository;

        }

        public List<OM_NotificationTemplate> GetAllNotificatiosByPagin(int pageIndex, string sort, string sortdir,
            DateTime? fromDate, DateTime? toDate, string searchString, out int totalRecords)
        {
            var pageSize = AppConfig.PageSize;
            string compId = PortalContext.CurrentUser.CompId;
            IQueryable<OM_NotificationTemplate> notificationTemplates = _notificationTemplateRepository.GetWithInclude(x => x.CompId == compId
                && (x.OM_TnaActivity.Name.Trim().ToLower().Contains(searchString.Trim().ToLower()) || String.IsNullOrEmpty(searchString))
                || (x.Receiver.Trim().ToLower().Contains(searchString.Trim().ToLower()) || String.IsNullOrEmpty(searchString)), "OM_TnaActivity");
            totalRecords = notificationTemplates.Count();
            switch (sort)
            {

                case "ActivityId":
                    switch (sortdir)
                    {
                        case "DESC":
                            notificationTemplates = notificationTemplates
                             .OrderByDescending(r => r.ActivityId)
                             .Skip(pageIndex * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            notificationTemplates = notificationTemplates
                                 .OrderBy(r => r.ActivityId)
                                 .Skip(pageIndex * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;

            }
            return notificationTemplates.ToList();
        }

        public OM_NotificationTemplate GetNotificationById(int notificationTemplateId)
        {
            var notificationTemplate = _notificationTemplateRepository.FindOne(x => x.NotificationTemplateId == notificationTemplateId);
            return notificationTemplate;
        }

        public int Edit(OM_NotificationTemplate model)
        {
            var notificationTemplate = _notificationTemplateRepository.FindOne(x => x.NotificationTemplateId == model.NotificationTemplateId);
            notificationTemplate.ActivityId = model.ActivityId;
            notificationTemplate.BuyerRefId = model.BuyerRefId;
            notificationTemplate.BeforeDays = model.BeforeDays;
            notificationTemplate.Receiver = model.Receiver;
            notificationTemplate.Remarks = model.Remarks;
            return _notificationTemplateRepository.Edit(notificationTemplate);
        }

        public int Save(OM_NotificationTemplate notificationTemplate)
        {
            return _notificationTemplateRepository.Save(notificationTemplate);
        }


        public int Delete(int? id)
        {

            var notificationTemplate = _notificationTemplateRepository.FindOne(x => x.NotificationTemplateId == id);
            return _notificationTemplateRepository.DeleteOne(notificationTemplate);
        }

        public List<OM_TnaActivity> GetActivityListByBuyerRefId(string buyerRefId)
        {
            string sql =string.Format(@"select ACT.* from OM_BuyerTnaTemplate as BT
                           inner join OM_TnaActivity as ACT on BT.ActivityId=ACT.ActivityId
                            where BT.CompId='{0}' and BT.BuyerRefId='{1}'",PortalContext.CurrentUser.CompId,buyerRefId);
            var  actitiTable = _notificationTemplateRepository.ExecuteQuery<OM_TnaActivity>(sql);
            return actitiTable.ToList();
        }
    }
}
