using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.InventoryModel;
using SCERP.Web.Areas.Inventory.Models.ViewModels;

namespace SCERP.Web.Areas.Inventory.Controllers
{
 
    public class StorePurchaseController : BaseInventoryController
    {
       [AjaxAuthorize(Roles = "purchaserequisition-1,purchaserequisition-2,purchaserequisition-3")]
        public ActionResult Index(StorePurchaseViewModel model)
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
            model.InventoryPurchaseTypes = PurchaseTypeManager.GetPurchaseTypes();
            model.InventoryRequsitionTypes = RequisitiontypeManager.GetRquisitiontypes();
            if (!model.IsSearch)
            {
                model.IsSearch = true;
            }
            else
            {
                model.VStorePurchaseRequisitions = StorePurchaseRequisitionManager.GetStorePurchaseByPaging(model, out totalRecords);
            }
            model.TotalRecords = totalRecords;
            return View(model);
        }
         [AjaxAuthorize(Roles = "purchaserequisition-2,purchaserequisition-3")]
        public ActionResult Edit(StorePurchaseViewModel model)
        {
            ModelState.Clear();
            model.BranchId = model.BranchId > 0 ? model.BranchId : 1; //1 for Narayangoanj
            model.CompanyId = model.CompanyId > 0 ? model.CompanyId : 1;// for Plummy Fashions Limited
            model.Countries = CountryManager.GetAllCountries();
            if (model.StorePurchaseRequisitionId > 0)
            {
                var storePurchaseRequisition = StorePurchaseRequisitionManager.GetVStorePurchaseRequisitionById(model.StorePurchaseRequisitionId);
                var matreq = MaterialRequisitionManager.GetVMaterialRequisitionById(storePurchaseRequisition.MaterialRequisitionId)??new VMaterialRequisition();
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
                model.BranchUnitId = storePurchaseRequisition.BranchUnitId;
                model.BranchUnitDepartmentId = storePurchaseRequisition.BranchUnitDepartmentId;
                model.DepartmentLineId = storePurchaseRequisition.DepartmentLineId;
                model.DepartmentSectionId = storePurchaseRequisition.DepartmentSectionId;
                model.PreparedBy = matreq.PreparedBy;
                model.PreparedByEmployeeName = matreq.PreparedByEmployeeName;
                model.IsSearch = true;
                model.MaterialRequisitionId = storePurchaseRequisition.MaterialRequisitionId;

                model.StorePurchaseRequisitionDetails = StorePurchaseRequisitionManager.GetStorePurchaseRequisitionDetails(model.StorePurchaseRequisitionId).ToDictionary(x => Convert.ToString(x.StorePurchaseRequisitionDetailId), x => new VStorePurchaseRequisitionDetail()
                {
                    ItemId = x.ItemId,
                    ItemName = x.Inventory_Item == null ? "" : x.Inventory_Item.ItemName,
                    StorePurchaseRequisitionId = x.StorePurchaseRequisitionId,
                    SizeId = x.SizeId,
                    SizeName = x.Inventory_Size == null ? "" : x.Inventory_Size.Title,
                    BrandName = x.Inventory_Brand == null ? "" : x.Inventory_Brand.Name,
                 
                    BrandId = x.BrandId,
                    Origin = x.Country == null ? "" : x.Country.CountryName,
                    OriginId = x.OriginId,
                    Quantity = x.Quantity,
                    DesiredDate = x.DesiredDate,
                    FunctionalArea = x.FunctionalArea,
                    SuppliedUptoDate = (decimal?) Convert.ToDouble(x.SuppliedUptoDate),
                    StockInHand = x.StockInHand,
                    LastUnitPrice = x.LastUnitPrice,
                    EstimatedYearlyRequirement = x.EstimatedYearlyRequirement,
                    Description = x.Description,
                    ModifiedRequiredQuantity = x.ModifiedRequiredQuantity,
                    ApprovedQuantity = x.ApprovedQuantity,
                    ApprovalDate = x.ApprovalDate,
                    ApprovalStatusId = 1,
                    RemarksOfRequisitionApprovalPerson = x.RemarksOfRequisitionApprovalPerson,
                    Quotation = x.Quotation,
                    PresentRate = x.PresentRate,
                    ApprovedPurchase = x.ApprovedPurchase,
                    RemarksOfPurchaseApprovalPerson = x.RemarksOfPurchaseApprovalPerson,

                });


            }
            else
            {
                model.RequisitionNo = StorePurchaseRequisitionManager.GetNewRequisitionNo();
            }
            model.Companies = CompanyManager.GetAllPermittedCompanies();
            model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.CompanyId);
            model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.BranchId);
            model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.BranchUnitId);
            model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.BranchUnitDepartmentId);
            model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.BranchUnitDepartmentId);
            model.InventoryPurchaseTypes = PurchaseTypeManager.GetPurchaseTypes();
            model.InventoryRequsitionTypes = RequisitiontypeManager.GetRquisitiontypes();
            model.InventorySizes = SizeManager.GetSizeList();
            model.InventoryBrands = BrandManager.GetBrands();
            return View(model);
        }
       [AjaxAuthorize(Roles = "purchaserequisition-2,purchaserequisition-3")]
        public ActionResult Save(StorePurchaseViewModel model)
        {
            try
            {
                var storePurchaseRequisition = new Inventory_StorePurchaseRequisition
                {
                    StorePurchaseRequisitionId = model.StorePurchaseRequisitionId,
                    MaterialRequisitionId = model.MaterialRequisitionId,
                    ApprovalStatusId = 1,
                    RequisitionDate = DateTime.Now,
                    RequisitionTypeId = model.RequisitionTypeId,
                    PurchaseTypeId = model.PurchaseTypeId,
                    ProcessId = (int)InventoryProcessType.StorePurchaseRequisition,
                    PreparedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                    
                    RequisitionNo = model.RequisitionNo,
                    Remarks = model.Remarks,
                    CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Inventory_MaterialRequisition = new Inventory_MaterialRequisition
                    {
                        MaterialRequisitionId = model.MaterialRequisitionId,
                        BranchUnitDepartmentId = model.BranchUnitDepartmentId,
                        DepartmentSectionId = model.DepartmentSectionId,
                        DepartmentLineId = model.DepartmentLineId,
                        PreparedBy = model.PreparedBy,
                        ModifiedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                        SubmittedTo = model.SubmittedTo,
                        IsModifiedByStore = true,
                        IsSentToStore = true,
                        SendingDate = DateTime.Now,
                        RequisitionNoteNo = model.RequisitionNoteNo,
                        RequisitionNoteDate = model.RequisitionNoteDate.GetValueOrDefault(),
                        Remarks = model.Remarks,
                        CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                    },
                    Inventory_StorePurchaseRequisitionDetail =
                        model.StorePurchaseRequisitionDetails.Select(x => new Inventory_StorePurchaseRequisitionDetail()
                        {
                            ItemId = x.Value.ItemId,
                            StorePurchaseRequisitionId = x.Value.StorePurchaseRequisitionId,
                            SizeId = x.Value.SizeId,
                            BrandId = x.Value.BrandId,
                            OriginId = x.Value.OriginId,
                            IsReceived = false,
                            Quantity = x.Value.Quantity.GetValueOrDefault(),
                            DesiredDate = x.Value.DesiredDate,
                            FunctionalArea = x.Value.FunctionalArea,
                            SuppliedUptoDate = x.Value.SuppliedUptoDate.ToString(),
                            StockInHand = x.Value.StockInHand,
                            LastUnitPrice = x.Value.LastUnitPrice,
                            EstimatedYearlyRequirement = x.Value.EstimatedYearlyRequirement,
                            Description = x.Value.Description,
                            ModifiedRequiredQuantity = x.Value.ModifiedRequiredQuantity,
                            ApprovedQuantity = x.Value.ApprovedQuantity,
                            ApprovalDate = x.Value.ApprovalDate,
                            ApprovalStatusId = 1,
                            RemarksOfRequisitionApprovalPerson = x.Value.RemarksOfRequisitionApprovalPerson,
                            Quotation = x.Value.Quotation,
                            PresentRate = x.Value.PresentRate,
                            ApprovedPurchase = x.Value.ApprovedPurchase,
                            RemarksOfPurchaseApprovalPerson = x.Value.RemarksOfPurchaseApprovalPerson,
                            CreatedBy = PortalContext.CurrentUser.UserId,
                            CreatedDate = DateTime.Now,
                            IsActive = true,

                        })
                            .ToList()
                };
                int index = 0;
                if (storePurchaseRequisition.Inventory_StorePurchaseRequisitionDetail.Any())
                {
                   index = StorePurchaseRequisitionManager.SaveStorePurchase(storePurchaseRequisition); 
                }
                else
                {
                    return ErrorResult("Please add required items  !!");
                }
                
                return index > 0 ? Reload() : ErrorResult("Fail to save");

            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult("System Error " + exception.Message);
            }



        }
        public ActionResult AddNewRow(StorePurchaseViewModel model)
        {
            ModelState.Clear();
            model.VStorePrDetail.FunctionalArea=String.IsNullOrEmpty(model.VStorePrDetail.FunctionalArea) ? "--" : model.VStorePrDetail.FunctionalArea;
            model.VStorePrDetail.BrandName = (model.VStorePrDetail.BrandId > 0) ?
                BrandManager.GetBrandById(model.VStorePrDetail.BrandId.GetValueOrDefault()).Name : "";
            model.VStorePrDetail.SizeName = (model.VStorePrDetail.SizeId > 0) ?
           SizeManager.GetSizeById(model.VStorePrDetail.SizeId.GetValueOrDefault()).Title : "";
            model.VStorePrDetail.Origin = (model.VStorePrDetail.OriginId > 0) ?
            CountryManager.GetCountryById(model.VStorePrDetail.OriginId.GetValueOrDefault()).CountryName : "";
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.StorePurchaseRequisitionDetails.Add(model.Key, model.VStorePrDetail);
            return PartialView("~/Areas/Inventory/Views/StorePurchase/_AddNewRow.cshtml", model);
        }

        public JsonResult ExistReqNotNo(VMaterialRequisition model)
        {
            bool isExist = !MaterialRequisitionManager.IsExistMaterialRequisitionNoteNo(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
    }
}