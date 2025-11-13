using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Model.CRMModel;
using SCERP.Model;

namespace SCERP.Web.Areas.CRM.Models.ViewModels
{
    public class ProjectDocumentInfoViewModel : CRMDocumentationReport
    {
        public ProjectDocumentInfoViewModel()
        {
            DocumentationReport = new List<CRMDocumentationReport>();
            IsSearch = true;
        }

        public List<CRMDocumentationReport> DocumentationReport { get; set; }

        public List<CRMCollaborator> ResponsibePersons { get; set; }

        public List<SelectListItem> ResonsiblePersonSelectListItem
        {
            get { return new SelectList(ResponsibePersons, "CollaboratorId", "CollaboratorDisplayName").ToList(); }
        }

        public List<CRMCollaborator> LastUpdatdBy { get; set; }

        public List<SelectListItem> LastUpdatedBySelectListItem
        {
            get { return new SelectList(LastUpdatdBy, "CollaboratorId", "CollaboratorDisplayName").ToList(); }
        }

        public List<Module> Modules { get; set; }

        public List<SelectListItem> ModuleSelectListItem
        {
            get { return new SelectList(Modules, "Id", "ModuleName").ToList(); }
        }

        public string SearchKey
        {
            get;
            set;
        }
    }
}