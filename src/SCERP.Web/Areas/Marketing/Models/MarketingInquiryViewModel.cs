using SCERP.Model.MarketingModel;
using SCERP.Model.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Marketing.Models
{
    public class MarketingInquiryViewModel : ProSearchModel<MarketingInquiryViewModel>
    {
        public List<MarketingInquiry> MarketingInquiries { get; set; }
        public MarketingInquiry MarketingInquiriy { get; set; }
        public List<MarketingStatus> Status { get; set; }

        public List<MarketingPerson> MarketingPersons { get; set; }
        public List<MarketingInstitute> MarketingInstitute { get; set; }
        public MarketingInquiryViewModel()
        {
            MarketingInquiriy = new MarketingInquiry();
            MarketingInquiries = new List<MarketingInquiry>();
            Status = new List<MarketingStatus>();
            MarketingInstitute = new List<MarketingInstitute>();
            MarketingPersons = new List<MarketingPerson>();
        }

        public List<SelectListItem> MarketingStatus
        {
            get { return new SelectList(Status, "StatusId", "StatusName").ToList(); }

        }

        public List<SelectListItem> MarketingInstituteSelectList
        {
            get { return new SelectList(MarketingInstitute, "InstituteId", "InstituteName").ToList(); }

        }

        public List<SelectListItem> MarketingPersonSelectList
        {
            get { return new SelectList(MarketingPersons, "MarketingPersonId", "Name").ToList(); }

        }
    }
}