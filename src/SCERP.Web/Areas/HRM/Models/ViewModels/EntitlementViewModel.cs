using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class EntitlementViewModel:Entitlement
    {
        public EntitlementViewModel()
        {
            Entitlements=new List<Entitlement>();
            IsSearch = true;
        }
        public List<Entitlement> Entitlements { get; set; }
        public string SearchKey { get; set; }

    }
}