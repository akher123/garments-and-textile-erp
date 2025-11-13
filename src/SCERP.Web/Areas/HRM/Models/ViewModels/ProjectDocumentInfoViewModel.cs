using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.CRMModel;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class ProjectDocumentInfoViewModel : CRMDocumentationReport
    {
        public ProjectDocumentInfoViewModel()
        {
            DocumentationReport = new List<CRMDocumentationReport>();
            IsSearch = true;
        }

        public List<CRMDocumentationReport> DocumentationReport { get; set; }
     
        public string SearchKey
        {
            get;
            set;
        }
    }
}