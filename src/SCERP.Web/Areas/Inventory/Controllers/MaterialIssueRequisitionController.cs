using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Inventory.Models.ViewModels;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class MaterialIssueRequisitionController : BaseInventoryController
    {
        //
        // GET: /Inventory/MaterialIssueRequisition/
        public ActionResult Index(MaterialIssueRequisitionViewModel model)
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
                    model.VMaterialIssueRequisitions = MaterialIssueRequisitionManager.GetMaterialIssueRequisitionByPaging(model, out totalRecords);
                }
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }
        public ActionResult Edit(MaterialIssueRequisitionViewModel model)
        {
            try
            {
                ModelState.Clear();

                if (model.MaterialIssueRequisitionId > 0)
                {
                    var materialIssueRequisition = MaterialIssueRequisitionManager.GetVMaterialIssueRequisitionById(model.MaterialIssueRequisitionId);
                    model.MaterialIssueRequisitionId = materialIssueRequisition.MaterialIssueRequisitionId;
                    model.BranchUnitDepartmentId = materialIssueRequisition.BranchUnitDepartmentId;
                    model.IsSentToStore = materialIssueRequisition.IsSentToStore;
                    model.BranchId = materialIssueRequisition.BranchId;
                    model.CompanyId = materialIssueRequisition.CompanyId;
                    model.BranchUnitId = materialIssueRequisition.BranchUnitId;
                    model.DepartmentLineId = materialIssueRequisition.DepartmentLineId;
                    model.DepartmentSectionId = materialIssueRequisition.DepartmentSectionId;
                    model.SubmittedTo = materialIssueRequisition.SubmittedTo;
                    model.IssueReceiveNoteDate = materialIssueRequisition.IssueReceiveNoteDate;
                    model.IssueReceiveNoteNo = materialIssueRequisition.IssueReceiveNoteNo;
                    model.Remarks = materialIssueRequisition.Remarks;
                    model.PreparedBy = materialIssueRequisition.PreparedBy;
                    model.PreparedByIssueRequsition = materialIssueRequisition.PreparedByEmployeeName;
                    model.IsSearch = true;
                    model.MaterialIssueRequisitionDetails = MaterialIssueRequisitionManager.GetMaterialIssueRequisitionDetails(model.MaterialIssueRequisitionId).ToDictionary(x=>Convert.ToString(x.MaterialIssueRequisitionDetailId),x=>x);
                }
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

        public JsonResult Save(MaterialIssueRequisitionViewModel model)
        {

            model.IsUserIsStorePerson = InventoryAuthorizedPersonManager.CheckUserIsStorePerson((int)InventoryProcessType.StorePurchaseRequisition, (int)StorePurchaseRequisition.Store, PortalContext.CurrentUser.UserId);
            try
            {
                var materialIssueRequisition = new Inventory_MaterialIssueRequisition
                {
                    MaterialIssueRequisitionId = model.MaterialIssueRequisitionId,
                    BranchUnitDepartmentId = model.BranchUnitDepartmentId,
                    DepartmentSectionId = model.DepartmentSectionId,
                    DepartmentLineId = model.DepartmentLineId,
                    PreparedBy = model.PreparedBy,
                    SubmittedTo = model.SubmittedTo,
                    IsModifiedByStore = model.IsModifiedByStore,
                    IsSentToStore =model.IsUserIsStorePerson || model.IsSentToStore,
                    SendingDate =model.IsSentToStore? DateTime.Now:(DateTime?) null,
                    IssueReceiveNoteNo = model.IssueReceiveNoteNo,
                    IssueReceiveNoteDate = model.IssueReceiveNoteDate,
                    Remarks = model.Remarks,
                    CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Inventory_MaterialIssueRequisitionDetail = model.MaterialIssueRequisitionDetails.Select(x=>x.Value).ToList()
                };
                ResponsModel = model.MaterialIssueRequisitionId > 0 ? MaterialIssueRequisitionManager.EditMaterialIssueRequisition(materialIssueRequisition) : MaterialIssueRequisitionManager.SaveMaterialIssueRequisition(materialIssueRequisition);
            }
            catch (Exception exception)
            {
                ResponsModel = new ResponsModel
                {
                    Message = "Internal Error ! please contact with vendor ."
                };
                Errorlog.WriteLog(exception);
               
            }
            return (ResponsModel.Status) ? Reload() : ErrorResult(ResponsModel.Message);
        }

   
        public ActionResult AddRow(MaterialIssueRequisitionViewModel model)
        {
            ModelState.Clear();
            model.MaterialIssueRequisitionDetails.Add(model.Key,new Inventory_MaterialIssueRequisitionDetail()); 
            return PartialView("_AddNewRow", model);
        }
        public JsonResult Delete(int? materialIssueRequisitionDetailId)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = MaterialIssueRequisitionManager.DeleteMaterialIssueRequisition(materialIssueRequisitionDetailId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete");
        }
        public JsonResult DeleteMaterialIssueRequisitionDetail(int? materialIssueRequisitionDetailId)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = MaterialIssueRequisitionManager.DeleteMaterialIssueRequisitionDetail(materialIssueRequisitionDetailId);
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

        public JsonResult AutocompliteGetEmployeeInfo(string employeeCardId, int? branchUnitDepartmentId)
   {
            var employeeDetail = EmployeeCompanyInfoManager.AutocompliteGetEmployeeInfo(employeeCardId, branchUnitDepartmentId);
            return Json(employeeDetail, JsonRequestBehavior.AllowGet);
        }

    }
}