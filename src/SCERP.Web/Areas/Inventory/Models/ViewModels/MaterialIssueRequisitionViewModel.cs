
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class MaterialIssueRequisitionViewModel : VMaterialIssueRequisition
    {
 
        public List<VMaterialIssueRequisition> VMaterialIssueRequisitions { get; set; }
        public Dictionary<string, Inventory_MaterialIssueRequisitionDetail> MaterialIssueRequisitionDetails { get; set; }

        public string PreparedByIssueRequsition { get; set; }
        public bool IsUserIsStorePerson { get; set; }
        public string Key { get; set; }
  
        public List<DepartmentLine> DepartmentLines { get; set; }
        public List<DepartmentSection> DepartmentSections { get; set; }
        public IEnumerable BranchUnitDepartments { get; set; }
        public IEnumerable BranchUnits { get; set; }
        public IEnumerable Branches { get; set; }
        public List<Inventory_AuthorizedPerson> AuthorizedPersonList { get; set; }
        public MaterialIssueRequisitionViewModel()
        {
            DepartmentLines=new List<DepartmentLine>();
            DepartmentSections=new List<DepartmentSection>();
            BranchUnitDepartments = new List<BranchUnitDepartment>();
            BranchUnits = new List<BranchUnit>();
            Branches = new List<Branch>();
            MaterialIssueRequisitionDetails=new Dictionary<string, Inventory_MaterialIssueRequisitionDetail>();
            VMaterialIssueRequisitions=new List<VMaterialIssueRequisition>();
            AuthorizedPersonList = new List<Inventory_AuthorizedPerson>();
        }

        public IEnumerable<SelectListItem> BranchSelectListItem
        {
            get { return new SelectList(Branches, "BranchId", "BranchName").ToList(); }
        }
        public IEnumerable Companies { get; set; }
        public IEnumerable<SelectListItem> CompanySelectListItem
        {
            get { return new SelectList(Companies, "CompanyId", "CompanyName").ToList(); }

        }

        public IEnumerable<System.Web.Mvc.SelectListItem> BranchUnitSelectListItem
        {
            get { return new SelectList(BranchUnits, "BranchUnitId", "UnitName"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> BranchUnitDepartmentSelectListItem
        {
            get { return new SelectList(BranchUnitDepartments, "BranchUnitDepartmentId", "DepartmentName"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentLineSelectListItem
        {
            get { return new SelectList(DepartmentLines, "ValueMember", "DisplayMember"); }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentSectionSelectListItem
        {
            get { return new SelectList(DepartmentSections, "ValueMember", "DisplayMember"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> AuthorizedPersonSelectListItem
        {
            get { return new SelectList(AuthorizedPersonList, "EmployeeId", "Employee.Name"); }

        }

    }
}