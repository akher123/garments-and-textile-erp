using SCERP.Model.MarketingModel;
using SCERP.Model.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Marketing.Models
{
    public class MarketingInstituteViewModel : ProSearchModel<MarketingInstituteViewModel>
    {
        public List<MarketingInstitute> MarketingInstitutes { get; set; }
        public MarketingInstitute MarketingInstitute { get; set; }
        public List<MarketingStatus> Status = new List<MarketingStatus>();
        public object MarketingPersons { get; set; }

        public MarketingInstituteViewModel()
        {
            MarketingInstitute = new MarketingInstitute();
            MarketingInstitutes = new List<MarketingInstitute>();
            Status = new List<MarketingStatus>();
            MarketingPersons = new object();
        }

        public List<SelectListItem> MarketingStatus
        {
            get { return new SelectList(Status, "StatusId", "StatusName").ToList(); }

        }
    }
}