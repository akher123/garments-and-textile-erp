using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.PayrollModel;

namespace SCERP.Web.Areas.Payroll.Models.ViewModels
{
    public class AdvanceIncomeTaxViewModel : AdvanceIncomeTax
    {
        public AdvanceIncomeTaxViewModel()
        {
            AdvanceIncomeTaxs = new List<AdvanceIncomeTax>();
            AdvanceIncomeTax = new AdvanceIncomeTax();
            
        }

        public List<AdvanceIncomeTax> AdvanceIncomeTaxs { get; set; }
        public AdvanceIncomeTax AdvanceIncomeTax { get; set; }
       
    }
}