using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Accounting.Models.ViewModels
{
    public class AccountingReportViewModel : ProSearchModel<AccountingReportViewModel>
    {
    
        public int GlId { get; set; }
    }
}