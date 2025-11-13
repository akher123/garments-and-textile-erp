using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class MaterialIssueViewModel:VMaterialIssue
    {

        public bool IsComeFromIssueRequsition { get; set; }
        public List<VMaterialIssue> VMaterialIssues { get; set; }
        public VMaterialIssueDetail IssueDetail { get; set; }
        public int LastRowIndex { get; set; }
        public Dictionary<string, VMaterialIssueDetail> MaterialIssueDetails { get; set; }
        public string Key { get; set; }
        public List<Inventory_MaterialIssueRequisitionDetail> MaterialIssueRequisitionDetails { get; set; }
        public MaterialIssueViewModel()
        {
       
            VMaterialIssues = new List<VMaterialIssue>();
            DepartmentLines = new List<DepartmentLine>();
            DepartmentSections = new List<DepartmentSection>();
            BranchUnits = new List<object>();
            BranchUnitDepartments = new List<object>();
            MaterialIssueRequisitionDetails=new List<Inventory_MaterialIssueRequisitionDetail>();
            Companies = new List<Company>();
            Branches = new List<Branch>();
            Countries=new List<Country>();
            InventorySizes=new List<Inventory_Size>();
            InventoryBrands=new List<Inventory_Brand>();
            MaterialIssueDetails = new Dictionary<string, VMaterialIssueDetail>();
            Machines=new List<Production_Machine>();
            Currencies=new List<Currency>();
            Batches=new List<Pro_Batch>();
            IssueDetail=new VMaterialIssueDetail();
        }

        public List<Pro_Batch> Batches { get; set; }
        public List<Currency> Currencies { get; set; }
        public List<Production_Machine> Machines { get; set; }
        public List<DepartmentLine> DepartmentLines { get; set; }
        public List<DepartmentSection> DepartmentSections { get; set; }
        public IEnumerable BranchUnitDepartments { get; set; }
        public IEnumerable BranchUnits { get; set; }
        public IEnumerable Branches { get; set; }
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
        public IEnumerable<System.Web.Mvc.SelectListItem> MachineSectionSelectListItem
        {
            get { return new SelectList(Machines, "MachineId", "Name"); }
        }
        public List<Inventory_Brand> InventoryBrands { get; set; }
        public IEnumerable<SelectListItem> BrandSelectListItem
        {
            get
            {
                return new SelectList(InventoryBrands, "BrandId", "Name");
            }
        }

        public List<Country> Countries { get; set; }
        public IEnumerable<SelectListItem> OriginSelectListItem
        {
            get
            {
                return new SelectList(Countries, "Id", "CountryName");
            }
        }

        public List<Inventory_Size> InventorySizes { get; set; }
        public IEnumerable<SelectListItem> SizeSelectListItem
        {
            get
            {
                return new SelectList(InventorySizes, "SizeId", "Title");
            }
        }



        public IEnumerable<SelectListItem> BatcheSelectListItem
        {
            get
            {
                return new SelectList(Batches, "BtRefNo", "BatchNo");
            }
        }

        public string OriginSerarcKey { get; set; }
    }
}