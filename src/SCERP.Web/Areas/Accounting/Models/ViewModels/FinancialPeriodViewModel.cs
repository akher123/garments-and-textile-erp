using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.Accounting.Models.ViewModels
{
    public class FinancialPeriodViewModel : Acc_FinancialPeriod
    {
        public List<Acc_FinancialPeriod> FinancialPeriod { get; set; }
    }
}