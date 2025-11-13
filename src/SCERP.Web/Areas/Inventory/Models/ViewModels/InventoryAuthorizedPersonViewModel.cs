using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class InventoryAuthorizedPersonViewModel:Inventory_AuthorizedPerson
    {
        public VEmployeeCompanyInfoDetail EmployeeCompanyInfo { get; set; }
        public List<InventoryProcess> InventoryProcessLsit { get; set; }
        public IEnumerable InventoryProcessTypeList { get; set; }
        public List<Inventory_AuthorizedPerson> InventoryAuthorizedPersons { get; set; } 
        public InventoryAuthorizedPersonViewModel()
        {
            InventoryProcessLsit = new List<InventoryProcess>();
            InventoryProcessTypeList = new List<object>();
            InventoryAuthorizedPersons=new List<Inventory_AuthorizedPerson>();
            EmployeeCompanyInfo=new VEmployeeCompanyInfoDetail();
        }
        public IEnumerable<SelectListItem> InventoryProcessSelectListItem
        {
            get { return new SelectList(InventoryProcessLsit, "ProcessId", "ProcessName").ToList(); }

        }
        public IEnumerable<SelectListItem> InventoryProcessTypeSelectListItem
        {
            get { return new SelectList(InventoryProcessTypeList, "ProcessTypeId", "ProcessTypeName").ToList(); }

        }
    }
}