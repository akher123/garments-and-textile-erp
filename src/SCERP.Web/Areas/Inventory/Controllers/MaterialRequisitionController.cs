using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Inventory.Models.ViewModels;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class MaterialRequisitionController : BaseInventoryController
    {
        [AjaxAuthorize(Roles = "materialrequisition-1,materialrequisition-2,materialrequisition-3")]
        public ActionResult Index(MaterialRequisitionViewModel model)
        {
            try
            {
                ModelState.Clear();
                var totalRecords = 0;
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.CompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.BranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.BranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.BranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.BranchUnitDepartmentId);
                model.IsUserIsStorePerson = InventoryAuthorizedPersonManager.CheckUserIsStorePerson((int)InventoryProcessType.StorePurchaseRequisition, (int)StorePurchaseRequisition.Store, PortalContext.CurrentUser.UserId);
                if (!model.IsSearch)
                {
                    model.IsSearch = true;
                }
                else
                {
                    model.VMaterialRequisitions = MaterialRequisitionManager.GetMaterialRequisitionByPaging(model, out totalRecords);
                }
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
     
            return View(model);
        }

        [AjaxAuthorize(Roles = "materialrequisition-2,materialrequisition-3")]
        public ActionResult Edit(MaterialRequisitionViewModel model)
        {
            try
            {
                 ModelState.Clear();
               model.IsUserIsStorePerson= InventoryAuthorizedPersonManager.CheckUserIsStorePerson(
                    (int) InventoryProcessType.StorePurchaseRequisition, (int) StorePurchaseRequisition.Store,
                    PortalContext.CurrentUser.UserId);
               if (model.IsUserIsStorePerson)
               {
                   model.IsSentToStore = true;
                   model.SubmittedTo = PortalContext.CurrentUser.UserId;
               }
                if (model.MaterialRequisitionId > 0)
                {
                    var materialRequisition = MaterialRequisitionManager.GetVMaterialRequisitionById(model.MaterialRequisitionId);
                    model.MaterialRequisitionId = materialRequisition.MaterialRequisitionId;
                    model.BranchUnitDepartmentId = materialRequisition.BranchUnitDepartmentId;
                    model.IsSentToStore = materialRequisition.IsSentToStore;
                    model.BranchId = materialRequisition.BranchId;
                    model.CompanyId = materialRequisition.CompanyId;
                    model.BranchUnitId = materialRequisition.BranchUnitId;
                    model.DepartmentLineId = materialRequisition.DepartmentLineId;
                    model.DepartmentSectionId = materialRequisition.DepartmentSectionId;
                    model.SubmittedTo = materialRequisition.SubmittedTo;
                    model.RequisitionNoteDate = materialRequisition.RequisitionNoteDate;
                    model.RequisitionNoteNo = materialRequisition.RequisitionNoteNo;
                    model.Remarks = materialRequisition.Remarks;
                    model.PreparedBy = materialRequisition.PreparedBy;
                    model.PreparedByRequsition = materialRequisition.PreparedByEmployeeName;
                    model.IsSearch = true;
                    model.MaterialRequisitionDetails =
                        MaterialRequisitionManager.GetMaterialRequisitionDetails(model.MaterialRequisitionId)
                            .ToDictionary(x => Convert.ToString(x.MaterialRequisitionDetailId), x => x);
                    model.InventoryMaterialRequisitionDetails = MaterialRequisitionManager.GetMaterialRequisitionDetails(model.MaterialRequisitionId);
                }
               // model.MaterialRequisitionNo = "MR-00001";
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.CompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.BranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.BranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.BranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.BranchUnitDepartmentId);
                model.AuthorizedPersonList = InventoryAuthorizedPersonManager.GetAuthorizedPersonsByProcessTypeId((int)InventoryProcessType.StorePurchaseRequisition, (int)StorePurchaseRequisition.Store);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
  
            return View(model);
        }

        [AjaxAuthorize(Roles = "materialrequisition-2,materialrequisition-3")]
        public JsonResult Save(MaterialRequisitionViewModel model)
        {
            try
            {
                ResponsModel=new ResponsModel();
                var materialRequisition = new Inventory_MaterialRequisition
                {
                    MaterialRequisitionId = model.MaterialRequisitionId,
                    BranchUnitDepartmentId = model.BranchUnitDepartmentId,
                    DepartmentSectionId = model.DepartmentSectionId,
                    DepartmentLineId = model.DepartmentLineId,
                    PreparedBy =model.PreparedBy,
                    ModifiedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                    SubmittedTo = model.SubmittedTo,
                    IsModifiedByStore = model.IsModifiedByStore.GetValueOrDefault(),
                    IsSentToStore = model.IsSentToStore,
                    SendingDate = DateTime.Now,
                    RequisitionNoteNo = model.RequisitionNoteNo,
                    RequisitionNoteDate = model.RequisitionNoteDate.GetValueOrDefault(),
                    Remarks = model.Remarks,
                    CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Inventory_MaterialRequisitionDetail = model.MaterialRequisitionDetails.Select(x=>x.Value).ToList()
                };
                ResponsModel = model.MaterialRequisitionId > 0 ? MaterialRequisitionManager.EditMaterialRequisition(materialRequisition) : MaterialRequisitionManager.SaveMaterialRequisition(materialRequisition);
            }
            catch (Exception exception)
            {
                ResponsModel=new ResponsModel
                {
                    Message = "Internal Error ! please contact with vendor " 
                };
                Errorlog.WriteLog(exception);
            }
            return (ResponsModel.Status) ? Reload() : ErrorResult(ResponsModel.Message);
        }

        [AjaxAuthorize(Roles = "materialrequisition-2,materialrequisition-3")]
        public ActionResult AddRow(MaterialRequisitionViewModel model)
        {
            ModelState.Clear();
            model.IsSearch = true;
            model.MaterialRequisitionDetails.Add(model.Key,new Inventory_MaterialRequisitionDetail());
            return PartialView("_AddNewRow", model);
        }

        public JsonResult Delete(int? materialRequisitionDetailId)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = MaterialRequisitionManager.DeleteMaterialRequisitionDetail(materialRequisitionDetailId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete");
        }
        public ActionResult GetAllBranchesByCompanyId(int companyId)
        {
            var branches = BranchManager.GetAllPermittedBranchesByCompanyId(companyId);
            return Json(new { Success = true, Branches = branches }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllBranchUnitByBranchId(int branchId)
        {
            var brancheUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(branchId);
            return Json(new { Success = true, BrancheUnits = brancheUnits }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllBranchUnitDepartmentByBranchUnitId(int branchUnitId)
        {
            var branchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(branchUnitId);
            return Json(new { Success = true, BranchUnitDepartments = branchUnitDepartments }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDepartmentSectionAndLineByBranchUnitDepartmentId(int branchUnitDepartmentId)
        {
            var departmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(branchUnitDepartmentId);
            var departmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(branchUnitDepartmentId);
            return Json(new { Success = true, DepartmentSections = departmentSections, DepartmentLines = departmentLines, }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckMaterialRequisitionNoteNo(VMaterialRequisition model)
        {
            bool isExist = !MaterialRequisitionManager.IsExistMaterialRequisitionNoteNo(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
	}
}