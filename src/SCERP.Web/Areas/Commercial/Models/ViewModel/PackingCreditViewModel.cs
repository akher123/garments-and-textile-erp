using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.CommercialModel;

namespace SCERP.Web.Areas.Commercial.Models.ViewModel
{
    public class PackingCreditViewModel
    {
        public List<CommPackingCredit> PackingCredits { get; set; }
        public CommPackingCredit PackingCredit { get; set; }

        public PackingCreditViewModel()
        {
            PackingCredits=new List<CommPackingCredit>();
            PackingCredit=new CommPackingCredit();
        }
    }
}