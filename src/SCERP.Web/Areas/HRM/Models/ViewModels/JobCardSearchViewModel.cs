using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class JobCardSearchViewModel
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string EmployeeCardId { get; set; }
    }
}