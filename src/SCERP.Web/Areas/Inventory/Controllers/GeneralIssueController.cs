using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Inventory.Models.ViewModels;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class GeneralIssueController : BaseInventoryController
    {
        private readonly IMachineManager _machineManager;
        public GeneralIssueController(IMachineManager machineManager)
        {
            _machineManager = machineManager;
        }
        [AjaxAuthorize(Roles = " generalissue-1,generalissue-2,generalissue-3")]
        public ActionResult Index(MaterialIssueViewModel model)
        {
            try
            {
                ModelState.Clear();
                var totalRecords = 0;
                model.BranchId = model.BranchId > 0 ? model.BranchId : 1; //1 for Narayangoanj
                model.CompanyId = model.CompanyId > 0 ? model.CompanyId : 1;// for Plummy Fashions Limited
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.CompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.BranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.BranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.BranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.BranchUnitDepartmentId);
                if (!model.IsSearch)
                {
                    model.IsSearch = true;
                }
                else
                {
                    model.VMaterialIssues = MaterialIssueManager.GetGeneralIssueByPaging(model, out totalRecords);
                }
               
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }
       [AjaxAuthorize(Roles = "generalissue-2,generalissue-3")]
        public ActionResult Edit(MaterialIssueViewModel model)
        {
            ModelState.Clear();
            model.Companies = CompanyManager.GetAllPermittedCompanies();
            model.IssueReceiveNoteDate = DateTime.Now;
            model.BranchId = model.BranchId>0?model.BranchId:1; //1 for Narayangoanj
            model.CompanyId = model.CompanyId > 0 ? model.CompanyId : 1;// for Plummy Fashions Limited
            if (model.MaterialIssueId > 0)
            {
                VMaterialIssue materialIssue = MaterialIssueManager.GetVMaterialIssueById(model.MaterialIssueId);
                model.MaterialIssueRequisitionId = materialIssue.MaterialIssueRequisitionId;
                model.MaterialIssueId = materialIssue.MaterialIssueId;
                model.MachineId = materialIssue.MachineId;
                model.BranchId = materialIssue.BranchId;
                model.CompanyId = materialIssue.CompanyId;
                model.BranchUnitDepartmentId = materialIssue.BranchUnitDepartmentId;
                model.DepartmentLineId = materialIssue.DepartmentLineId;
                model.DepartmentSectionId = materialIssue.DepartmentSectionId;
                model.BranchUnitId = materialIssue.BranchUnitId;
                
                model.IssueReceiveNoteDate = materialIssue.IssueReceiveNoteDate;
                model.IssueReceiveNoteNo = materialIssue.IssueReceiveNoteNo;
                model.IssueReceiveNo = materialIssue.IssueReceiveNo;
                model.IssueReceiveDate = materialIssue.IssueReceiveDate;
                model.PreparedBy = materialIssue.PreparedBy;
                model.PreparedByEmployeeName = materialIssue.PreparedByEmployeeName;
                model.Note = materialIssue.Note;
                model.CompanyName = materialIssue.CompanyName;
                model.BranchName = materialIssue.BranchName;
                model.UnitName = materialIssue.UnitName;
                model.DepartmentName = materialIssue.DepartmentName;
                model.SectionName = materialIssue.SectionName;
                model.LineName = materialIssue.LineName;
                model.MaterialIssueDetails = MaterialIssueManager.GetMaterialIssueDetails(model.MaterialIssueId).ToDictionary(x => Convert.ToString(x.MaterialIssueDetailId), x => x);
            }
            model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.CompanyId);
            model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.BranchId);
            model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.BranchUnitId);
            model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.BranchUnitDepartmentId);
            model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.BranchUnitDepartmentId);
            model.InventorySizes = SizeManager.GetSizeList();
            model.InventoryBrands = BrandManager.GetBrands();
            model.Countries = CountryManager.GetAllCountries();
            model.Machines = _machineManager.GetMachines();
            return View(model);
        }
        [AjaxAuthorize(Roles = "generalissue-2,generalissue-3")]
        public JsonResult Save(MaterialIssueViewModel model)
        {
            var saveIndex = 0;
            try
            {
                var materialIssue = new Inventory_MaterialIssue
                {
                    MaterialIssueRequisitionId = model.MaterialIssueRequisitionId,
                    MaterialIssueId = model.MaterialIssueId,
                    IssueReceiveDate = DateTime.Now,
                    MachineId = model.MachineId,
                    IssueReceiveNo = model.IssueReceiveNo,
                    Note = model.Note,
                    IType = (int)MaterialIssueType.GeneralIssue, // 1 for General Issue
                    PreparedByStore = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                    CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Inventory_MaterialIssueRequisition = new Inventory_MaterialIssueRequisition
                      {
                      MaterialIssueRequisitionId = model.MaterialIssueRequisitionId,
                      BranchUnitDepartmentId = model.BranchUnitDepartmentId,
                      DepartmentSectionId = model.DepartmentSectionId,
                      DepartmentLineId = model.DepartmentLineId,
                      PreparedBy = model.PreparedBy,
                      SubmittedTo = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                      IsModifiedByStore = true,
                      IsSentToStore = true,
                      SendingDate = DateTime.Now,
                      IssueReceiveNoteNo = model.IssueReceiveNoteNo,
                      IssueReceiveNoteDate = model.IssueReceiveNoteDate,
                      CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                      CreatedDate = DateTime.Now,
                      IsActive = true,

                  }

                };
                foreach (var materialIssueDetail in model.MaterialIssueDetails.Values)
                {
                    materialIssue.Inventory_MaterialIssueDetail.Add(new Inventory_MaterialIssueDetail()
                    {
                        MaterialIssueDetailId = materialIssueDetail.MaterialIssueDetailId,
                        MaterialIssueId = model.MaterialIssueId,
                        ItemId = materialIssueDetail.ItemId,
                        SizeId = materialIssueDetail.SizeId,
                        MachineId = materialIssueDetail.MachineId,
                        BrandId = materialIssueDetail.BrandId,
                        OriginId = materialIssueDetail.OriginId,
                        StockInHand = materialIssueDetail.StockInHand.GetValueOrDefault(),
                        RequiredQuantity = materialIssueDetail.RequiredQuantity.GetValueOrDefault(),
                        CurrencyId = 1,
                        Remarks = materialIssueDetail.Remarks,
                        IssuedQuantity = materialIssueDetail.IssuedQuantity.GetValueOrDefault(),
                        IssuedItemRate = materialIssueDetail.IssuedItemRate.GetValueOrDefault(),
                        TransactionDate = model.IssueReceiveNoteDate,
                        IsActive = true,
                        CreatedBy = PortalContext.CurrentUser.UserId,
                        CreatedDate = DateTime.Now,
                    });
                }
                if (model.MaterialIssueDetails.Any())
                {
                    saveIndex = model.MaterialIssueId > 0 ? MaterialIssueManager.EditMaterialIssue(materialIssue) : MaterialIssueManager.SaveMaterialIssue(materialIssue);
                }
                else
                {
                    return ErrorResult("Please add issue items  !!");
                }
               
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult(String.Format("Failed to save data"));
        }

         [AjaxAuthorize(Roles = "generalissue-3")]
        public ActionResult Delete(int materialIssueId)
        {
            var deleteIndex = 0;
            const int generalIssue = 1;
            try
            {

                deleteIndex = MaterialIssueManager.DeleteMaterialIssue(materialIssueId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return deleteIndex > 0 ? Reload() : ErrorResult("Delete Fail");
        }
        public ActionResult AddNewRow(MaterialIssueViewModel model)
        {
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                model.MaterialIssueDetails.Add(model.Key, model.IssueDetail);
                if (model.IssueDetail != null && model.IssueDetail.ItemId == -1)
                {
                    return ErrorResult("Invalid Item Please select correct one");
                }
                if (model.IssueDetail != null && model.IssueDetail.RequiredQuantity == 0)
                {
                    return ErrorResult("RQty is Required !!! ");
                }
                if (model.IssueDetail != null && model.IssueDetail.RequiredQuantity < 0)
                {
                    return ErrorResult("RQty is not allow negative figure !!! ");
                }
                if (model.IssueDetail != null && model.IssueDetail.IssuedQuantity == 0)
                {
                    return ErrorResult("IQty is Required !!! ");
                }
                if (model.IssueDetail != null && model.IssueDetail.IssuedQuantity < 0)
                {
                    return ErrorResult("IQty is not allow negative figure !!! ");
                }
                if (model.IssueDetail != null && model.IssueDetail.IssuedQuantity > model.IssueDetail.RequiredQuantity)
                {
                    return ErrorResult("IQty not greater than RQty!!! ");
                }
                if (model.IssueDetail != null && model.IssueDetail.StockInHand < model.IssueDetail.IssuedQuantity)
                {
                    return ErrorResult("IQty not greater than SinH !!! ");
                }
                if (model.IssueDetail != null && model.IssueDetail.StockInHand - model.IssueDetail.RequiredQuantity<0)
                {
                    return ErrorResult("RQty not greater than SinH !!! ");
                }
            }
            return PartialView("~/Areas/Inventory/Views/GeneralIssue/_AddNewRow.cshtml", model);
        }


    }
}