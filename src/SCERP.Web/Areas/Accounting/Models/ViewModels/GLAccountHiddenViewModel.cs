using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;
using SCERP.Model.AccountingModel;

namespace SCERP.Web.Areas.Accounting.Models.ViewModels
{
    public class GLAccountHiddenViewModel: Acc_GLAccounts_Hidden
    {
        public List<Acc_GLAccounts_Hidden> GLAccountHidden { get; set; }
        public List<Acc_GLAccounts_Hidden> GLAccountVisible { get; set; }
        public int totalRecordsVisible { get; set; }
        public int totalRecordsHidden { get; set; }        
    }
}