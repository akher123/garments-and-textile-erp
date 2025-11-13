using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  SCERP.Model;

namespace SCERP.Web.Areas.Accounting.Models.ViewModels
{
    public class CompanySectorViewModel : Acc_CompanySector
    {
        public List<Acc_CompanySector> CompanySector { get; set; }
    }
}