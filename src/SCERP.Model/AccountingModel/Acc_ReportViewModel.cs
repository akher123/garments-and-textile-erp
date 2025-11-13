using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model
{
    public class Acc_ReportViewModel
    {
        public string SectorCode { get; set; }
        public string FpId { get; set; }
        public string GLId { get; set; }
        public string AccountCode { get; set; }
        public string CostCentreID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string OpStartDate { get; set; }
        public string OpEndDate { get; set; }
        public int TrialBalanceLebel { get; set; }
        public string cotrolcode { get; set; }
        public int currencyId { get; set; }
    }
}
