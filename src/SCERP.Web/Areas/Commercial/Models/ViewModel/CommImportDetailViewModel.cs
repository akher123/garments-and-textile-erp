using SCERP.Model.CommercialModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCERP.Web.Areas.Commercial.Models.ViewModel
{
    public class CommImportDetailViewModel
    {
        public List<CommImportDetails> commImportDetails { get; set; }
        public CommImportDetails CommImportDetail { get; set; }

        public CommImportDetailViewModel()
        {
            commImportDetails = new List<CommImportDetails>();
            CommImportDetail = new CommImportDetails();
        }

    }
}