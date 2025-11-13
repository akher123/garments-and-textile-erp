using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class StorePurchaseRequisitionController : BaseInventoryController
    {
        [AjaxAuthorize(Roles = "requisitionpreparation-1,requisitionpreparation-2,requisitionpreparation-3,requisitionapproval-1,requisitionapproval-2,requisitionapproval-3,purchase-1,purchase-2,purchase-3,purchaseapproval-1,purchaseapproval-2,purchaseapproval-3")]
        public ActionResult Index(StorePurchaseRequisitionViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.Companies = CompanyManager.GetAllPermittedCompanies();
            model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.CompanyId);
            model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.BranchId);
            model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.BranchUnitId);
            model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.BranchUnitDepartmentId);
            model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.BranchUnitDepartmentId);
            model.InventoryPurchaseTypes = PurchaseTypeManager.GetPurchaseTypes();
            model.InventoryRequsitionTypes = RequisitiontypeManager.GetRquisitiontypes();
            model.InventoryApprovalStatuses = InventoryApprovalStatusManager.GetInventoryApprovalStatus();
            model.AuthonticatedProcessId = InventoryAuthorizedPersonManager.FindSoterRequisiotionProcessId((int)InventoryProcessType.StorePurchaseRequisition, PortalContext.CurrentUser.UserId);
            if (!model.IsSearch)
            {
                model.IsSearch = true;

            }
            else
            {

                model.VStorePurchaseRequisitions = StorePurchaseRequisitionManager.GetStorePurchaseRequisitionsByPaging(model, out totalRecords);
            }
            model.TotalRecords = totalRecords;
            return View(model);
        }

        [AjaxAuthorize(Roles = "requisitionpreparation-2,requisitionpreparation-3,requisitionapproval-2,requisitionapproval-3,purchase-2,purchase-3,purchaseapproval-2,purchaseapproval-3")]
        public ActionResult Edit(StorePurchaseRequisitionViewModel model)
        {
            if (model.StorePurchaseRequisitionId > 0)
            {
                var storePurchaseRequisition = StorePurchaseRequisitionManager.GetVStorePurchaseRequisitionById(model.StorePurchaseRequisitionId);
                model.StorePurchaseRequisitionId = storePurchaseRequisition.StorePurchaseRequisitionId;
                model.PurchaseTypeTitle = storePurchaseRequisition.PurchaseTypeTitle;
                model.RequsitionTypeTitle = storePurchaseRequisition.RequsitionTypeTitle;
                model.RequisitionNoteDate = storePurchaseRequisition.RequisitionNoteDate;
                model.RequisitionNoteNo = storePurchaseRequisition.RequisitionNoteNo;
                model.RequisitionNo = storePurchaseRequisition.RequisitionNo;
                model.RequisitionDate = storePurchaseRequisition.RequisitionDate;
                model.RequisitionTypeId = storePurchaseRequisition.RequisitionTypeId;
                model.ApprovalStausName = storePurchaseRequisition.ApprovalStausName;
                model.ApprovalDate = storePurchaseRequisition.ApprovalDate;
                model.PurchaseTypeId = storePurchaseRequisition.PurchaseTypeId;
                model.SubmittedTo = storePurchaseRequisition.SubmittedTo;
                model.ApprovalStatusId = storePurchaseRequisition.ApprovalStatusId;
                model.ProcessId = storePurchaseRequisition.ProcessId;
                model.Remarks = storePurchaseRequisition.Remarks;
                model.BranchId = storePurchaseRequisition.BranchId;
                model.IsSearch = true;
                model.AuthorizedPersonList = InventoryAuthorizedPersonManager.GetAuthorizedPersons((int)InventoryProcessType.StorePurchaseRequisition, model.ProcessId);
                model.InventoryStorePurchaseRequisitionDetails = StorePurchaseRequisitionManager.GetStorePurchaseRequisitionDetails(model.StorePurchaseRequisitionId).ToDictionary(x => Convert.ToString(x.StorePurchaseRequisitionDetailId), x => x);
                model.InventoryMaterialRequisitionDetails =
                     MaterialRequisitionManager.GetMaterialRequisitionDetails(storePurchaseRequisition.MaterialRequisitionId);

            }
            else
            {
                var isModifiedByStore = MaterialRequisitionManager.CheckModifiedByStore(model.MaterialRequisitionId);
                if (isModifiedByStore)
                {
                    return ErrorResult("This material requisition is modified !!");
                }
                else
                {
                    var materialRequisition = MaterialRequisitionManager.GetVMaterialRequisitionById(model.MaterialRequisitionId);
                    model.MaterialRequisitionId = materialRequisition.MaterialRequisitionId;
                    model.RequisitionNoteDate = materialRequisition.RequisitionNoteDate;
                    model.RequisitionNoteNo = materialRequisition.RequisitionNoteNo;
                    model.InventoryMaterialRequisitionDetails = MaterialRequisitionManager.GetMaterialRequisitionDetails(materialRequisition.MaterialRequisitionId);
                    model.RequisitionNo = StorePurchaseRequisitionManager.GetNewRequisitionNo();
                    model.BranchId = materialRequisition.BranchId;
                    model.RequisitionDate = DateTime.Now;
                }

            }
            model.AuthonticatedProcessId = InventoryAuthorizedPersonManager.FindSoterRequisiotionProcessId((int)InventoryProcessType.StorePurchaseRequisition, PortalContext.CurrentUser.UserId);
            model.InventorySizes = SizeManager.GetSizeList();
            model.InventoryBrands = BrandManager.GetBrands();
            model.Countries = CountryManager.GetAllCountries();
            model.MeasurementUnits = MeasurementUnitManager.GetMeasurementUnits();
            model.InventoryApprovalStatuses = InventoryApprovalStatusManager.GetInventoryApprovalStatus();
            model.InventoryProcessLsit = InventoryProcessUtility.GetPocessByProcessType((int)InventoryProcessType.StorePurchaseRequisition);
            model.InventoryPurchaseTypes = PurchaseTypeManager.GetPurchaseTypes();
            model.InventoryRequsitionTypes = RequisitiontypeManager.GetRquisitiontypes();
            model.InventoryApprovalStatuses = InventoryApprovalStatusManager.GetInventoryApprovalStatus();
            model.InventoryApprovalStatuses = InventoryApprovalStatusManager.GetInventoryApprovalStatus();
            return View(model);
        }

        [AjaxAuthorize(Roles = "requisitionpreparation-2,requisitionpreparation-3,requisitionapproval-2,requisitionapproval-3,purchase-2,purchase-3,purchaseapproval-2,purchaseapproval-3")]
        public ActionResult Save(StorePurchaseRequisitionViewModel model)
        {
            var saveIndex = 0;
            try
            {
                if (model.StorePurchaseRequisitionId > 0)
                {
                    var storePurchaseRequisition = StorePurchaseRequisitionManager.GetStorePurchaseRequisitionById(model.StorePurchaseRequisitionId);
                    storePurchaseRequisition.PurchaseTypeId = model.PurchaseTypeId;
                    storePurchaseRequisition.RequisitionTypeId = model.RequisitionTypeId;
                    storePurchaseRequisition.ApprovalStatusId = model.ApprovalStatusId;
                    storePurchaseRequisition.ProcessId = model.ProcessId;
                    storePurchaseRequisition.Remarks = model.Remarks;
                    storePurchaseRequisition.ApprovalDate = model.ApprovalDate;
                    storePurchaseRequisition.ModifiedBy = storePurchaseRequisition.SubmittedTo;
                    storePurchaseRequisition.SubmittedTo = model.SubmittedTo;
                    storePurchaseRequisition.IsActive = true;
                    storePurchaseRequisition.EditedDate = DateTime.Now;
                    storePurchaseRequisition.EditedBy = PortalContext.CurrentUser.UserId;
                    saveIndex = StorePurchaseRequisitionManager.EditStorePurchaseRequisition(storePurchaseRequisition, model.InventoryStorePurchaseRequisitionDetails.Select(x => x.Value).ToList());
                }
                else
                {
                    var storePurchaseRequisition = new Inventory_StorePurchaseRequisition
                    {
                        PurchaseTypeId = model.PurchaseTypeId,
                        MaterialRequisitionId = model.MaterialRequisitionId,
                        RequisitionTypeId = model.RequisitionTypeId,
                        ApprovalStatusId = model.ApprovalStatusId,
                        ProcessId = model.ProcessId,
                        RequisitionNo = model.RequisitionNo,
                        RequisitionDate = model.RequisitionDate.GetValueOrDefault(),
                        Remarks = model.Remarks,
                        ApprovalDate = model.ApprovalDate,
                        SubmittedTo = model.SubmittedTo,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        CreatedBy = PortalContext.CurrentUser.UserId,
                        PreparedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                        ModifiedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                        Inventory_StorePurchaseRequisitionDetail = model.InventoryStorePurchaseRequisitionDetails.Select(x => x.Value).ToList()
                    };
                    saveIndex = StorePurchaseRequisitionManager.SaveStorePurchaseRequisition(storePurchaseRequisition);
                }
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return saveIndex > 0 ? Reload() : ErrorResult("Fail to save ");
        }

        [AjaxAuthorize(Roles = "requisitionpreparation-2,requisitionpreparation-3,requisitionapproval-2,requisitionapproval-3,purchase-2,purchase-3,purchaseapproval-2,purchaseapproval-3")]
        public ActionResult AddRow(StorePurchaseRequisitionViewModel model)
        {
            model.NumberOfHeaderRows = 2;
            model.InventorySizes = SizeManager.GetSizeList();
            model.InventoryBrands = BrandManager.GetBrands();
            model.Countries = CountryManager.GetAllCountries();
            model.MeasurementUnits = MeasurementUnitManager.GetMeasurementUnits();
            model.InventoryApprovalStatuses = InventoryApprovalStatusManager.GetInventoryApprovalStatus();
            model.IsSearch = true;
            model.InventoryStorePurchaseRequisitionDetails.Add(model.Key, new Inventory_StorePurchaseRequisitionDetail());
            return PartialView("_AddNewRow", model);
        }

        [AjaxAuthorize(Roles = "requisitionpreparation-2,requisitionpreparation-3,requisitionapproval-2,requisitionapproval-3,purchase-2,purchase-3")]
        public ActionResult Delete(Inventory_StorePurchaseRequisition requisition)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = StorePurchaseRequisitionManager.DeletePurchaseRequisition(requisition.StorePurchaseRequisitionId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete");

        }

        [AjaxAuthorize(Roles = "requisitionpreparation-2,requisitionpreparation-3,requisitionapproval-2,requisitionapproval-3,purchase-2,purchase-3,purchaseapproval-2,purchaseapproval-3")]
        public ActionResult DeletePurchaseRequisitionDetail(int? storePurchaseRequisitionDetailId)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = StorePurchaseRequisitionManager.DeleteDeletePurchaseRequisitionDetail(storePurchaseRequisitionDetailId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete");

        }
        public ActionResult Report(int storePurchaseRequisitionId)
        {
            string reportName = "StorePurchaseRequisition";
            var reportParams = new List<ReportParameter> { new ReportParameter("StorePurchaseRequisitionId", storePurchaseRequisitionId.ToString()),
                 new ReportParameter("CompId", PortalContext.CurrentUser.CompId), new ReportParameter("HostingServerAddress", AppConfig.HostingServerAddress)  };
            return ReportExtension.ToSsrsFile(ReportType.PDF, reportName, reportParams);
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

        public ActionResult GetAllAuthorizedPersonsByProcessId(int authorizationId)
        {
            var processId = authorizationId;
            var authorizedPersons = InventoryAuthorizedPersonManager.GetAuthorizedPersons((int)InventoryProcessType.StorePurchaseRequisition, processId);
            return Json(new { Success = true, AuthorizedPersons = authorizedPersons }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PresentStockInfo(int? itemId,int?sizeId,int?brandId,int?originId)
        {
            var presentStock = StoreLedgerManager.PresentStockInfo(itemId, sizeId, brandId, originId);
            return Json(presentStock, JsonRequestBehavior.AllowGet);
        }
    }
}