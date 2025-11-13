using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CRMModel
{
    public class SPCRMDocumentationReport
    {
        public int Id { get; set; }
        public string RefNo { get; set; }
        public string ReportName { get; set; }
        public string Description { get; set; }
        public string Literature { get; set; }
        public Nullable<int> ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string PhotographPath { get; set; }
        public Nullable<System.Guid> ResponsiblePerson { get; set; }
        public string LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
