using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Inventory.Models.ViewModels;


namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class MaterialIssueController : BaseInventoryController
    {
        private readonly IMachineManager _machineManager;
        public MaterialIssueController(IMachineManager machineManager)
        {
            _machineManager = _machineManager;
        }
        public ActionResult Index(MaterialIssueViewModel model)
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
                if (!model.IsSearch)
                {
                    model.IsSearch = true;
                }
                else
                {
                    model.VMaterialIssues = MaterialIssueManager.GetMaterialIssuesByPaging(model, out totalRecords);
                }
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }
        public ActionResult Edit(MaterialIssueViewModel model)
        {
            try
            {
                ModelState.Clear();
                if (model.MaterialIssueId > 0)
                {
                    VMaterialIssue materialIssue = MaterialIssueManager.GetVMaterialIssueById(model.MaterialIssueId);
                    model.MaterialIssueRequisitionId = materialIssue.MaterialIssueRequisitionId;
                    model.MaterialIssueId = materialIssue.MaterialIssueId;
                    model.BranchId = materialIssue.BranchId;
                    model.IssueReceiveNoteDate = materialIssue.IssueReceiveNoteDate;
                    model.IssueReceiveNoteNo = materialIssue.IssueReceiveNoteNo;
                    model.IssueReceiveNo = materialIssue.IssueReceiveNo;
                    model.IssueReceiveDate = materialIssue.IssueReceiveDate;
                    model.Note = materialIssue.Note;
                    model.CompanyName = materialIssue.CompanyName;
                    model.BranchName = materialIssue.BranchName;
                    model.UnitName = materialIssue.UnitName;
                    model.DepartmentName = materialIssue.DepartmentName;
                    model.SectionName = materialIssue.SectionName;
                    model.LineName = materialIssue.LineName;
                    model.MaterialIssueDetails = MaterialIssueManager.GetMaterialIssueDetails(model.MaterialIssueId).ToDictionary(x=>Convert.ToString(x.MaterialIssueDetailId),x=>x);
                }
                else
                {
                    var isModifiedByStore = MaterialIssueRequisitionManager.CheckModifiedByStore(model.MaterialIssueRequisitionId);
                    if (!isModifiedByStore)
                    {
                        var materialIssue = MaterialIssueRequisitionManager.GetVMaterialIssueRequisitionById(model.MaterialIssueRequisitionId);
                        model.MaterialIssueRequisitionId = materialIssue.MaterialIssueRequisitionId;
                        model.BranchId = materialIssue.BranchId;
                        model.IssueReceiveNo = MaterialIssueManager.GetNewIssueReceiveNo();
                        model.IssueReceiveNoteDate = materialIssue.IssueReceiveNoteDate;
                        model.IssueReceiveNoteNo = materialIssue.IssueReceiveNoteNo;
                        model.IssueReceiveDate =DateTime.Now;
                        model.CompanyName = materialIssue.CompanyName;
                        model.BranchName = materialIssue.BranchName;
                        model.UnitName = materialIssue.UnitName;
                        model.DepartmentName = materialIssue.DepartmentName;
                        model.SectionName = materialIssue.SectionName;
                        model.LineName = materialIssue.LineName;
                      
                    }
                    else
                    {
                        return ErrorResult("This Material Issue  Requisition is modified !!");
                    }
                }
                model.IsSearch = true;
                model.Currencies = CurrencyManagerCommon.GetAllCourrency();

                model.Machines = _machineManager.GetMachines();
                model.InventorySizes = SizeManager.GetSizeList();
                model.InventoryBrands = BrandManager.GetBrands();
                model.Countries = CountryManager.GetAllCountries();
                model.MaterialIssueRequisitionDetails = MaterialIssueRequisitionManager.GetMaterialIssueRequisitionDetails(model.MaterialIssueRequisitionId);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return View("~/Areas/Inventory/Views/MaterialIssue/Edit.cshtml", model);
        }


     
        public JsonResult StockStatys(int? itemId)
        {
            var stockStatys = StoreLedgerManager.StockStatys(itemId);
            return Json(stockStatys, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SizeWizeStockInHand(int? itemId, int? sizeId)
        {
            var stockInfo = StoreLedgerManager.SizeWizeStockInHand(itemId, sizeId);
            return Json(stockInfo, JsonRequestBehavior.AllowGet);
          
        }

        public JsonResult BrandWizeStockInHand(int? itemId, int? sizeId ,int?brandId)
        {
            var stockInfo = StoreLedgerManager.BrandWizeStockInHand(itemId, sizeId,brandId);
            return Json(stockInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult OriginWizeStockInHand(int? itemId, int? sizeId, int? brandId,int?originId)
        {
            var stockInfo = StoreLedgerManager.OriginWizeStockInHand(itemId, sizeId, brandId, originId);
            return Json(stockInfo, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Save(MaterialIssueViewModel model)
        {
            var saveIndex = 0;
            try
            {
                var materialIssue = new Inventory_MaterialIssue
                {
                    MaterialIssueRequisitionId = model.MaterialIssueRequisitionId,
                    MaterialIssueId = model.MaterialIssueId,
                    IssueReceiveDate = model.IssueReceiveDate,
                    IssueReceiveNo = model.IssueReceiveNo,
                    Note = model.Note,
                    PreparedByStore = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                    CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };
                foreach (var materialIssueDetail in model.MaterialIssueDetails.Values)
                {
                    materialIssue.Inventory_MaterialIssueDetail.Add(new Inventory_MaterialIssueDetail()
                    {
                         MaterialIssueDetailId = materialIssueDetail.MaterialIssueDetailId,
                         MaterialIssueId = materialIssueDetail.MaterialIssueId,
                         ItemId = materialIssueDetail.ItemId,
                         SizeId = materialIssueDetail.SizeId,
                         BrandId = materialIssueDetail.BrandId,
                         OriginId = materialIssueDetail.OriginId,
                         StockInHand = materialIssueDetail.StockInHand.GetValueOrDefault(),
                         RequiredQuantity = materialIssueDetail.RequiredQuantity.GetValueOrDefault(),
                         MachineId = materialIssueDetail.MachineId,
                         CurrencyId = materialIssueDetail.CurrencyId,
                         Remarks = materialIssueDetail.Remarks,
                         IssuedQuantity = materialIssueDetail.IssuedQuantity.GetValueOrDefault(),
                         IssuedItemRate = materialIssueDetail.IssuedItemRate.GetValueOrDefault(),
                         TransactionDate = materialIssueDetail.TransactionDate,
                         IsActive = true,
                         CreatedBy = PortalContext.CurrentUser.UserId,
                         CreatedDate = DateTime.Now,
                    });
                }
                saveIndex = model.MaterialIssueId > 0 ? MaterialIssueManager.EditMaterialIssue(materialIssue) : MaterialIssueManager.SaveMaterialIssue(materialIssue);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult(String.Format("Failed to save data"));
        }
        public JsonResult Delete(int? materialIssueId)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = MaterialIssueManager.DeleteMaterialIssue(materialIssueId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete");
        }
        public JsonResult DeleteMaterialIssueDetail(int? materialIssueDetailId)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = MaterialIssueManager.DeleteMaterialIssueDetail(materialIssueDetailId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete");
        }

        public ActionResult AutocompliteItemByBranch(string itemName)
        {
            var itemList = InventoryItemManager.AutocompliteItemByBranch(itemName);
            return Json(itemList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddRow(MaterialIssueViewModel model)
        {
            ModelState.Clear();
            model.Currencies = CurrencyManagerCommon.GetAllCourrency();

            model.Machines = _machineManager.GetMachines();
            model.InventorySizes = SizeManager.GetSizeList();
            model.InventoryBrands = BrandManager.GetBrands();
            model.Countries = CountryManager.GetAllCountries();
            model.IsSearch = true;
            model.MaterialIssueDetails.Add(model.Key, new VMaterialIssueDetail());
            return PartialView("_AddNewRow", model);
        }



    }
}