using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model.CommonModel;

namespace SCERP.Web.Areas.Common.Models.ViewModel
{
    public class ReportParameterViewModel:SqlReportParameter
    {
        public ReportParameterViewModel()
        {
            Dropdowns=new List<Dropdown>();
        }
        public List<Dropdown> Dropdowns { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> ActiveStatusSelectedItem
        {
            get { return new SelectList(Dropdowns,"Id","Value"); }

        }
    }
}