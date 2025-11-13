using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.TaskManagementModel
{
    public class TmReportImageInfo : TmSearchModel<TmReportImageInfo>
    {
        public int ReportImageId { get; set; }
        public int SubjectId { get; set; }
        public string ReportName { get; set; }
        public string ReportNo { get; set; }
        public string ReportImageUrl { get; set; }
        public string CompId { get; set; }
        public string Remarks { get; set; }
        public string ProjectReportUrl { get; set; }
    }
}
