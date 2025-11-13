using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.Accounting.Models.ViewModels
{
    public class DepreciationChartViewModel : Acc_DepreciationChart
    {
        public List<Acc_DepreciationChart> DepreciationCharts { get; set; }
    }
}