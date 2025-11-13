using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class StorePurchaseRequisitionViewModel : VStorePurchaseRequisition
    {
        public bool IsComeFromRequsition { get; set; }
        public int NumberOfHeaderRows { get; set; }
        public List<VStorePurchaseRequisition> VStorePurchaseRequisitions { get; set; }
        public int AuthonticatedProcessId { get; set; }
        public int LastRowIndex { get; set; }
        public List<InventoryProcess> InventoryProcessLsit { get; set; }
        public Dictionary<string, Inventory_StorePurchaseRequisitionDetail> InventoryStorePurchaseRequisitionDetails { get; set; }
        public List<Inventory_MaterialRequisitionDetail> InventoryMaterialRequisitionDetails { get; set; }
        public List<DepartmentLine> DepartmentLines { get; set; }
        public List<DepartmentSection> DepartmentSections { get; set; }
        public IEnumerable BranchUnitDepartments { get; set; }
        public IEnumerable BranchUnits { get; set; }
        public StorePurchaseRequisitionViewModel()
        {
            InventoryMaterialRequisitionDetails=new List<Inventory_MaterialRequisitionDetail>();
            VStorePurchaseRequisitions=new List<VStorePurchaseRequisition>();
            InventoryStorePurchaseRequisitionDetails=new Dictionary<string, Inventory_StorePurchaseRequisitionDetail>();
            DepartmentLines=new List<DepartmentLine>();
            DepartmentSections=new List<DepartmentSection>();
            BranchUnits = new List<object>();
            BranchUnitDepartments = new List<object>();
            Companies = new List<Company>();
            Branches = new List<Branch>();
            InventoryPurchaseTypes = new List<Inventory_PurchaseType>();
            InventoryRequsitionTypes=new List<Inventory_RequsitionType>();
            InventoryBrands=new List<Inventory_Brand>();
            Countries=new List<Country>();
            InventorySizes=new List<Inventory_Size>();
            MeasurementUnits=new List<MeasurementUnit>();
            InventoryApprovalStatuses=new List<Inventory_ApprovalStatus>();
           // AuthorizationList=new List<AuthorizationType>();
            AuthorizedPersonList = new List<Inventory_AuthorizedPerson>();
            InventoryProcessLsit = new List<InventoryProcess>();
         
        }
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

        public List<Inventory_PurchaseType> InventoryPurchaseTypes { get; set; }
        public IEnumerable<SelectListItem> PurchaseTypeSelectListItem
        {
            get { return new SelectList(InventoryPurchaseTypes, "PurchaseTypeId", "Title"); }
        }
        public List<Inventory_RequsitionType> InventoryRequsitionTypes { get; set; }
        public IEnumerable<SelectListItem> RequisitionTypeSelectListItem
        {
            get
            {   
                return new SelectList(InventoryRequsitionTypes, "RequisitionTypeId", "Title");
            }
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

        public List<MeasurementUnit> MeasurementUnits { get; set; }
        public IEnumerable<SelectListItem> MeasurementUnitSelectListItem
        {
            get
            {
                return new SelectList(MeasurementUnits, "UnitId", "UnitName");
            }
        }

        public List<Inventory_ApprovalStatus> InventoryApprovalStatuses { get; set; }
        public IEnumerable<SelectListItem> ApprovalStatuSelectListItem
        {
            get
            {
                return new SelectList(InventoryApprovalStatuses, "ApprovalStatusId", "StatusName");
            }
        }
        public IEnumerable<SelectListItem> InventoryProcessSelectListItem
        {
            get { return new SelectList(InventoryProcessLsit, "ProcessId", "ProcessName").ToList(); }

        }


        public IEnumerable AuthorizedPersonList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AuthorizedPersonSelectListItem
        {
            get { return new SelectList(AuthorizedPersonList, "EmployeeId", "Name",SubmittedTo); }

        }

        public string Key { get; set; }
    }
}