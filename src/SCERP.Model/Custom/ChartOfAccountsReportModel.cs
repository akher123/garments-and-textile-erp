using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class ChartOfAccountsReportModel
    {
        public decimal MainCode { get; set; }
        public string MainHead { get; set; }
        public decimal GroupCode { get; set; }
        public string GroupName { get; set; }
        public decimal SubGroupCode { get; set; }
        public string SubGroupName { get; set; }
        public decimal ControlCode { get; set; }
        public string ControlName { get; set; }
        public decimal GLCode { get; set; }
        public string GLName { get; set; }
    }
}
