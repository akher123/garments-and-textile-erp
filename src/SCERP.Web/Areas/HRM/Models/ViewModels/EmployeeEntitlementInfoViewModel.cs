using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{

    public class EmployeeEntitlementInfoViewModel : EmployeeEntitlement
    {
        public EmployeeEntitlementInfoViewModel()
        {
            EmployeeEntitlements = new List<EmployeeEntitlement>();
        }

        public List<EmployeeEntitlement> EmployeeEntitlements { get; set; }

        public List<Entitlement> Entitlements { get; set; }     
        public List<SelectListItem> EntitlementSelectListItem
        {
            get { return new SelectList(Entitlements, "Id", "Title").ToList(); }

        }

    }

}