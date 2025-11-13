using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class NotificationTemplateViewModel : ProSearchModel<NotificationTemplateViewModel>
    {
        public OM_NotificationTemplate NotificationTemplate { get; set; }
        public List<OM_NotificationTemplate> NotificationTemplates { get; set; }
        public List<OM_Buyer> OmBuyers { get; set; }
        public List<OM_TnaActivity> Activities { get; set; }
        public NotificationTemplateViewModel()
        {
            NotificationTemplate = new OM_NotificationTemplate();
            NotificationTemplates = new List<OM_NotificationTemplate>();
            Activities = new List<OM_TnaActivity>();
        }

        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(OmBuyers, "BuyerRefId", "BuyerName");
            }
        }
        public IEnumerable<SelectListItem> ActivitySelectListItem
        {
            get
            {
                return new SelectList(Activities, "ActivityId", "Name");
            }
        }
    }
}