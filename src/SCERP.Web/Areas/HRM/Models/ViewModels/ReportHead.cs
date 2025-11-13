using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class ReportHead
    {
        public string Head { get; set; }
        public string DisplayName { get; set; }
        public bool IsSelected { get; set; }
    }
}