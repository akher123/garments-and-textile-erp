using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.Payroll.Models.ViewModels
{
    public class SalaryMappingViewModel : SalaryHead
    {
        public List<SalaryHead> SalaryHeads { get; set; }
    }
}
