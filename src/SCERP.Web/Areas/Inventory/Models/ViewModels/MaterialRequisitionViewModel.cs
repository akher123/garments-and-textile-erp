using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class MaterialRequisitionViewModel : VMaterialRequisition
    {
        public List<VMaterialRequisition> VMaterialRequisitions { get; set; }
        public string Key { get; set; }
 
        public string PreparedByRequsition { get; set; }
        public List<Inventory_MaterialRequisitionDetail> InventoryMaterialRequisitionDetails { get; set; }

        public Dictionary<string, Inventory_MaterialRequisitionDetail> MaterialRequisitionDetails { get; set; }
        public MaterialRequisitionViewModel()
        {
            MaterialRequisitionDetails=new Dictionary<string, Inventory_MaterialRequisitionDetail>();
            VMaterialRequisitions=new List<VMaterialRequisition>();
            DepartmentLines = new List<DepartmentLine>();
            DepartmentSections = new List<DepartmentSection>();
            BranchUnits = new List<object>();
            BranchUnitDepartments = new List<object>();
            Companies = new List<Company>();
            Branches = new List<Branch>();
             AuthorizedPersonList = new List<Inventory_AuthorizedPerson>();
            InventoryMaterialRequisitionDetails=new List<Inventory_MaterialRequisitionDetail>();
        }
        public List<DepartmentLine> DepartmentLines { get; set; }
        public List<DepartmentSection> DepartmentSections { get; set; }
        public IEnumerable BranchUnitDepartments { get; set; }
        public IEnumerable BranchUnits { get; set; }
        public IEnumerable Branches { get; set; }
        public IEnumerable<SelectListItem> BranchSelectListItem
        {
            get { return new SelectList(Branches, "BranchId", "BranchName").ToList(); }
        }
        public IEnumerable<SelectListItem> ModifiedByStoreSelectListItem
        {
            get { return new SelectList(new[] { new { IsModifiedByStore = true, Status = "Converted" }, new { IsModifiedByStore = false, Status = "Convert" } }, "IsModifiedByStore", "Status"); }

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
        public List<Inventory_AuthorizedPerson> AuthorizedPersonList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AuthorizedPersonSelectListItem
        {
            get { return new SelectList(AuthorizedPersonList, "EmployeeId", "Employee.Name"); }
        }
        public bool IsUserIsStorePerson { get; set; }
    }
}