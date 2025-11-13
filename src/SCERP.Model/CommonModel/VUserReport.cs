using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommonModel
{
    public class VUserReport
    {
        public long UserReportId { get; set; }
        public string UserName { get; set; }
        public int CustomSqlQuaryId { get; set; }
        public string SqlQuaryRefId { get; set; }
        public bool IsEnable { get; set; }
        public string ReportName { get; set; }
    

    }
}
