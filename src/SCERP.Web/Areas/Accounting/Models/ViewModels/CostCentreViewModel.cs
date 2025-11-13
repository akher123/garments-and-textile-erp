using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.Accounting.Models.ViewModels
{
    public class CostCentreViewModel : Acc_CostCentre
    {
        public List<Acc_CostCentre> CostCentre { get; set; }
    }
}