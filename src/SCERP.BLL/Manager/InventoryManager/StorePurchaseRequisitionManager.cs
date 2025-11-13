using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.DAL.Repository.InventoryRepository;
using SCERP.Model;
namespace SCERP.BLL.Manager.InventoryManager
{
    public class StorePurchaseRequisitionManager : IStorePurchaseRequisitionManager
    {
        private readonly IStorePurchaseRequisitionRepository _storePurchaseRequisitionRepository;
        private readonly InventoryAuthorizedPersonRepository _inventoryAuthorizedPersonRepository;
        private readonly IStorePurchaseRequisitionDetailRepository _storePurchaseRequisitionDetailRepository;
        private readonly IMaterialRequisitionRepository _materialRequisitionRepository;
        public StorePurchaseRequisitionManager(SCERPDBContext context)
        {
            _storePurchaseRequisitionRepository = new StorePurchaseRequisitionRepository(context);
            _storePurchaseRequisitionDetailRepository = new StorePurchaseRequisitionDetailRepository(context);
            _inventoryAuthorizedPersonRepository = new InventoryAuthorizedPersonRepository(context);
            _materialRequisitionRepository = new MaterialRequisitionRepository(context);
        }

        public List<Inventory_StorePurchaseRequisitionDetail> GetStorePurchaseRequisitionDetails(int storePurchaseRequisitionId)
        {
            var storePurchaseRequisitionDetails =
                _storePurchaseRequisitionRepository.GetStorePurchaseRequisitionDetails(storePurchaseRequisitionId);
            return storePurchaseRequisitionDetails.ToList();
        }

        public VStorePurchaseRequisition GetVStorePurchaseRequisitionById(int storePurchaseRequisitionId)
        {
            return _storePurchaseRequisitionRepository.GetVStorePurchaseRequisitionById(storePurchaseRequisitionId);
        }

        public Inventory_StorePurchaseRequisition GetStorePurchaseRequisitionById(int storePurchaseRequisitionId)
        {
            return
                _storePurchaseRequisitionRepository.FindOne(
                    x => x.StorePurchaseRequisitionId == storePurchaseRequisitionId);
        }

        public int EditStorePurchaseRequisition(Inventory_StorePurchaseRequisition storePurchaseRequisition, List<Inventory_StorePurchaseRequisitionDetail> storePurchaseRequisitionDetails)
        {
            var saveIndex = 0;
            try
            {
                using (var transaction = new TransactionScope())
                {


                    if (storePurchaseRequisitionDetails.Any())
                    {
                        foreach (var sprd in storePurchaseRequisitionDetails)
                        {
                            if (sprd.StorePurchaseRequisitionDetailId > 0)
                            {
                                var oldSprqd = _storePurchaseRequisitionDetailRepository.FindOne(
                                      x => x.StorePurchaseRequisitionDetailId == sprd.StorePurchaseRequisitionDetailId);
                                oldSprqd.ItemId = sprd.ItemId;
                                oldSprqd.StorePurchaseRequisitionId = sprd.StorePurchaseRequisitionId;
                                oldSprqd.Description = sprd.Description;
                                oldSprqd.SizeId = sprd.SizeId;
                                oldSprqd.BrandId = sprd.BrandId;
                                oldSprqd.OriginId = sprd.OriginId;
                                oldSprqd.Quantity = sprd.Quantity;

                                oldSprqd.DesiredDate = sprd.DesiredDate;
                                oldSprqd.FunctionalArea = sprd.FunctionalArea;
                                oldSprqd.SuppliedUptoDate = sprd.SuppliedUptoDate;
                                oldSprqd.StockInHand = sprd.StockInHand;
                                oldSprqd.LastUnitPrice = sprd.LastUnitPrice;
                                oldSprqd.EstimatedYearlyRequirement = sprd.EstimatedYearlyRequirement;
                                oldSprqd.EstimatedYearlyRequirement = sprd.EstimatedYearlyRequirement;
                                oldSprqd.ModifiedRequiredQuantity = sprd.ModifiedRequiredQuantity;
                                oldSprqd.ApprovedQuantity = sprd.ApprovedQuantity;
                                oldSprqd.ApprovalDate = sprd.ApprovalDate;
                                oldSprqd.ApprovalStatusId = sprd.ApprovalStatusId;
                                oldSprqd.RemarksOfRequisitionApprovalPerson = sprd.RemarksOfRequisitionApprovalPerson;
                                oldSprqd.Quotation = sprd.Quotation;
                                oldSprqd.PresentRate = sprd.PresentRate;
                                oldSprqd.ApprovedPurchase = sprd.ApprovedPurchase;
                                oldSprqd.RemarksOfPurchaseApprovalPerson = sprd.RemarksOfPurchaseApprovalPerson;
                                oldSprqd.EditedBy = PortalContext.CurrentUser.UserId;
                                oldSprqd.EditedDate = DateTime.Now;
                                saveIndex += _storePurchaseRequisitionDetailRepository.Edit(oldSprqd);
                            }
                            else
                            {
                                sprd.StorePurchaseRequisitionId = storePurchaseRequisition.StorePurchaseRequisitionId;
                                sprd.CreatedBy = PortalContext.CurrentUser.UserId;
                                sprd.CreatedDate = DateTime.Now;
                                sprd.IsActive = true;
                                sprd.Inventory_Item = null;
                                saveIndex += _storePurchaseRequisitionDetailRepository.Save(sprd);
                            }

                        }
                    }
                    saveIndex += _storePurchaseRequisitionRepository.Edit(storePurchaseRequisition);
                    transaction.Complete();
                }
            }
            catch (Exception exception)
            {

                throw exception;
            }
            return saveIndex;
        }

        public List<VStorePurchaseRequisition> GetStorePurchaseRequisitionsByPaging(VStorePurchaseRequisition model, out int totalRecords)
        {
            var userId = PortalContext.CurrentUser.UserId;
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var companyIdList = PortalContext.CurrentUser.PermissionContext.CompanyList.Select(x => x.CompanyId);
            var branchIdList = PortalContext.CurrentUser.PermissionContext.BranchList.Select(x => x.BranchId);
            var branchUnitIdList = PortalContext.CurrentUser.PermissionContext.UnitList.Select(x => x.BranchUnitId);
            var branchUnitDepartmentIdList = PortalContext.CurrentUser.PermissionContext.DepartmentList.Select(x => x.BranchUnitDepartmentId);
            //   var employeeTypeList = PortalContext.CurrentUser.PermissionContext.EmployeeTypeList.Select(x => x.Id); 
            Expression<Func<VStorePurchaseRequisition, bool>> predicate;
            if (model.BranchId > 0 || model.CompanyId > 0 ||
           model.BranchUnitId > 0 || model.BranchUnitDepartmentId > 0)
            {
                predicate = x => (x.BranchId == model.BranchId || model.BranchId == 0)
                         && (x.CompanyId == model.CompanyId || model.CompanyId == 0)
                         && (x.BranchUnitDepartmentId == model.BranchUnitDepartmentId || model.BranchUnitDepartmentId == 0)
                         && (x.BranchUnitId == model.BranchUnitId || model.BranchUnitId == 0)
                         && (x.DepartmentSectionId == model.DepartmentSectionId || model.DepartmentSectionId == null)
                         && (x.DepartmentLineId == model.DepartmentLineId || model.DepartmentLineId == null)
                         && (x.RequisitionNo == model.RequisitionNo || model.RequisitionNo == null)
                         && (x.RequisitionNoteNo == model.RequisitionNoteNo || model.RequisitionNoteNo == null)
                         && (x.ApprovalStatusId == model.ApprovalStatusId || model.ApprovalStatusId == 0);
            }
            else
            {
                predicate = x => companyIdList.Contains(x.CompanyId)
                                 && branchIdList.Contains(x.BranchId)
                                 && branchUnitIdList.Contains(x.BranchUnitId)
                                 && branchUnitDepartmentIdList.Contains(x.BranchUnitDepartmentId)
                                 && (x.DepartmentSectionId == model.DepartmentSectionId || model.DepartmentSectionId == null)
                                 && (x.DepartmentLineId == model.DepartmentLineId || model.DepartmentLineId == null)
                                 && (x.RequisitionNo == model.RequisitionNo || model.RequisitionNo == null)
                                 && (x.RequisitionNoteNo == model.RequisitionNoteNo || model.RequisitionNoteNo == null)
                                 && (x.ApprovalStatusId == model.ApprovalStatusId || model.ApprovalStatusId == 0);
            }
            var process = _inventoryAuthorizedPersonRepository.FindOne(x => x.ProcessTypeId == (int)InventoryProcessType.StorePurchaseRequisition && x.EmployeeId == userId && x.IsActive) ?? new Inventory_AuthorizedPerson();
            var storePurchaseRequisitions = _storePurchaseRequisitionRepository.GetStorePurchaseRequisitions(predicate);
            List<VStorePurchaseRequisition> vStorePurchaseRequisitions = storePurchaseRequisitions.ToList();
            switch (process.ProcessId)
            {
                case (int)StorePurchaseRequisition.Purchases:
                    storePurchaseRequisitions = storePurchaseRequisitions.Where(x => x.SubmittedTo == userId);
                    break;
                case (int)StorePurchaseRequisition.Approval:
                    storePurchaseRequisitions = storePurchaseRequisitions.Where(x => x.SubmittedTo == userId);
                    break;
                default:
                    storePurchaseRequisitions = storePurchaseRequisitions.Where(x => x.PreparedBy == userId);
                    break;
            }
            switch (model.sort)
            {

                case "UnitName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            storePurchaseRequisitions = storePurchaseRequisitions
                                  .OrderByDescending(r => r.UnitName)
                                  .Skip(index * pageSize)
                                  .Take(pageSize);
                            break;
                        default:
                            storePurchaseRequisitions = storePurchaseRequisitions
                                  .OrderBy(r => r.UnitName)
                                  .Skip(index * pageSize)
                                  .Take(pageSize);
                            break;
                    }
                    break;
                case "DepartmentName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            storePurchaseRequisitions = storePurchaseRequisitions
                             .OrderByDescending(r => r.DepartmentName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            storePurchaseRequisitions = storePurchaseRequisitions
                                 .OrderBy(r => r.DepartmentName)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                case "EmployeeName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            storePurchaseRequisitions = storePurchaseRequisitions
                             .OrderByDescending(r => r.PreparedByEmployeeName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            storePurchaseRequisitions = storePurchaseRequisitions
                                 .OrderBy(r => r.PreparedByEmployeeName)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                case "SectionName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            storePurchaseRequisitions = storePurchaseRequisitions
                             .OrderByDescending(r => r.SectionName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            storePurchaseRequisitions = storePurchaseRequisitions
                                 .OrderBy(r => r.SectionName)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                case "LneName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            storePurchaseRequisitions = storePurchaseRequisitions
                             .OrderByDescending(r => r.LineName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            storePurchaseRequisitions = storePurchaseRequisitions
                                 .OrderBy(r => r.LineName)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;

                default:

                    storePurchaseRequisitions = storePurchaseRequisitions
                     .OrderByDescending(r => r.RequisitionNo)
                     .Skip(index * pageSize)
                     .Take(pageSize);
                    break;

            }


            totalRecords = storePurchaseRequisitions.Count();
            return storePurchaseRequisitions.ToList();
        }

        public int DeletePurchaseRequisition(int storePurchaseRequisitionId)
        {
            int deleteIndex = 0;
            try
            {
                var storePurchaseRequisition =
             _storePurchaseRequisitionRepository.FindOne(
                 x => x.StorePurchaseRequisitionId == storePurchaseRequisitionId);
                storePurchaseRequisition.IsActive = false;
                storePurchaseRequisition.EditedDate = DateTime.Now;
                storePurchaseRequisition.EditedBy = PortalContext.CurrentUser.UserId;
                deleteIndex = _storePurchaseRequisitionRepository.Edit(storePurchaseRequisition);
            }
            catch (Exception exception)
            {

                throw exception;
            }

            return deleteIndex;
        }

        public string GetNewRequisitionNo()
        {
            return _storePurchaseRequisitionRepository.GetNewRequisitionNo();
        }

        public int SaveStorePurchaseRequisition(Inventory_StorePurchaseRequisition model)
        {
            var saveIndex = 0;
            model.RequisitionNo = GetNewRequisitionNo();
            try
            {
                using (var transaction = new TransactionScope())
                {
                    model.Inventory_StorePurchaseRequisitionDetail = model.Inventory_StorePurchaseRequisitionDetail.Select(
                         x => new Inventory_StorePurchaseRequisitionDetail()
                         {
                             ItemId = x.ItemId,
                             StorePurchaseRequisitionId = x.StorePurchaseRequisitionId,
                             SizeId = x.SizeId,
                             BrandId = x.BrandId,
                             OriginId = x.OriginId,
                             Quantity = x.Quantity,

                             DesiredDate = x.DesiredDate,
                             FunctionalArea = x.FunctionalArea,
                             SuppliedUptoDate = x.SuppliedUptoDate,
                             StockInHand = x.StockInHand,
                             LastUnitPrice = x.LastUnitPrice,
                             EstimatedYearlyRequirement = x.EstimatedYearlyRequirement,
                             Description = x.Description,
                             ModifiedRequiredQuantity = x.ModifiedRequiredQuantity,
                             ApprovedQuantity = x.ApprovedQuantity,
                             ApprovalDate = x.ApprovalDate,
                             ApprovalStatusId = x.ApprovalStatusId,

                             RemarksOfRequisitionApprovalPerson = x.RemarksOfRequisitionApprovalPerson,
                             Quotation = x.Quotation,
                             PresentRate = x.PresentRate,
                             ApprovedPurchase = x.ApprovedPurchase,
                             RemarksOfPurchaseApprovalPerson = x.RemarksOfPurchaseApprovalPerson,
                             CreatedBy = PortalContext.CurrentUser.UserId,
                             CreatedDate = DateTime.Now,
                             IsActive = true,
                         }).ToList();

                    saveIndex += _storePurchaseRequisitionRepository.Save(model);
                    var materialRequisition = _materialRequisitionRepository.FindOne(x => x.MaterialRequisitionId == model.MaterialRequisitionId);
                    materialRequisition.IsModifiedByStore = true;
                    saveIndex += _materialRequisitionRepository.Edit(materialRequisition);
                    transaction.Complete();
                }
            }
            catch (Exception exception)
            {

                throw;
            }


            return saveIndex;
        }

        public int DeleteDeletePurchaseRequisitionDetail(int? storePurchaseRequisitionDetailId)
        {
            var deleteIndex = 0;
            try
            {
                var storePurchaseRequisitionDetail =
             _storePurchaseRequisitionDetailRepository.FindOne(
                 x => x.StorePurchaseRequisitionDetailId == storePurchaseRequisitionDetailId);
                storePurchaseRequisitionDetail.IsActive = false;
                storePurchaseRequisitionDetail.EditedDate = DateTime.Now;
                storePurchaseRequisitionDetail.EditedBy = PortalContext.CurrentUser.UserId;
                deleteIndex = _storePurchaseRequisitionDetailRepository.Edit(storePurchaseRequisitionDetail);
            }
            catch (Exception exception)
            {

                throw exception;
            }

            return deleteIndex;
        }

        public List<Inventory_StorePurchaseRequisition> GetStorePurchaseRequisitions(string searchString)
        {
            return _storePurchaseRequisitionRepository.Filter(x => x.IsActive && x.RequisitionNo.Trim().ToLower().Contains(searchString.Trim().ToLower())).ToList();
        }

        public Dictionary<string, VItemReceiveDetail> GetVStorePurchaseRequisitionDetails(int storePurchaseRequisitionId)
        {
            var vStorePurchaseRequisitionDetails = _storePurchaseRequisitionRepository.GetVStorePurchaseRequisitionDetails(storePurchaseRequisitionId);

            return vStorePurchaseRequisitionDetails.ToDictionary(x => Convert.ToString(x.StorePurchaseRequisitionDetailId), x => new VItemReceiveDetail()
                {
                    ItemId = x.ItemId,
                    ItemName = x.ItemName,
                    SizeId = x.SizeId,
                    SizeName = x.SizeName,
                    BrandId = x.BrandId,
                    BrandName = x.BrandName,
                    OriginId = x.OriginId,
                    OriginName = x.Origin,
                    SuppliedQuantity = (int)x.ApprovedQuantity,
                    ReceivedQuantity = (int)x.ApprovedQuantity,
                    StorePurchaseRequisitionDetailId = x.StorePurchaseRequisitionDetailId
                });


        }

        public int SaveStorePurchase(Inventory_StorePurchaseRequisition model)
        {

            var editIndex = 0;
            var delete = 0;
            model.EditedBy = PortalContext.CurrentUser.UserId;
            model.EditedDate = DateTime.Now;
            model.IsActive = true;
            if (model.StorePurchaseRequisitionId == 0)
            {
                model.RequisitionNo = GetNewRequisitionNo();
            }
            using (var transaction = new TransactionScope())
            {

                delete += _storePurchaseRequisitionDetailRepository.Delete(x => x.IsActive && x.StorePurchaseRequisitionId == model.StorePurchaseRequisitionId);
                delete += _storePurchaseRequisitionRepository.Delete(x => x.StorePurchaseRequisitionId == model.StorePurchaseRequisitionId);
                delete += _materialRequisitionRepository.Delete(x => x.MaterialRequisitionId == model.MaterialRequisitionId);
                editIndex += _storePurchaseRequisitionRepository.Save(model);
                transaction.Complete();
            }
            return editIndex;
        }

        public List<VStorePurchaseRequisition> GetStorePurchaseByPaging(VStorePurchaseRequisition model, out int totalRecords)
        {

            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;

            Expression<Func<VStorePurchaseRequisition, bool>> predicate = x =>
                (x.CompanyId == model.CompanyId || model.CompanyId == 0)
                && (x.BranchUnitDepartmentId == model.BranchUnitDepartmentId || model.BranchUnitDepartmentId == 0)
                && (x.BranchUnitId == model.BranchUnitId || model.BranchUnitId == 0)
                && (x.DepartmentSectionId == model.DepartmentSectionId || model.DepartmentSectionId == null)
                     && (x.DepartmentLineId == model.DepartmentLineId || model.DepartmentLineId == null)
                && (x.PurchaseTypeId == model.PurchaseTypeId || model.PurchaseTypeId == 0)
                && (x.RequisitionTypeId == model.RequisitionTypeId || model.RequisitionTypeId == 0)
                &&((x.RequisitionNoteNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                   || (x.RequisitionNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower())))
                && ((x.RequisitionNoteDate >= model.FromDate || model.FromDate == null) && (x.RequisitionNoteDate <= model.ToDate || model.ToDate == null));
               var storePurchaseRequisitions = _storePurchaseRequisitionRepository.GetStorePurchaseRequisitions(predicate);
              totalRecords= storePurchaseRequisitions.Count();
            switch (model.sort)
            {

                case "UnitName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            storePurchaseRequisitions = storePurchaseRequisitions
                                  .OrderByDescending(r => r.UnitName)
                                  .Skip(index * pageSize)
                                  .Take(pageSize);
                            break;
                        default:
                            storePurchaseRequisitions = storePurchaseRequisitions
                                  .OrderBy(r => r.UnitName)
                                  .Skip(index * pageSize)
                                  .Take(pageSize);
                            break;
                    }
                    break;
                case "DepartmentName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            storePurchaseRequisitions = storePurchaseRequisitions
                             .OrderByDescending(r => r.DepartmentName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            storePurchaseRequisitions = storePurchaseRequisitions
                                 .OrderBy(r => r.DepartmentName)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                case "EmployeeName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            storePurchaseRequisitions = storePurchaseRequisitions
                             .OrderByDescending(r => r.PreparedByEmployeeName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            storePurchaseRequisitions = storePurchaseRequisitions
                                 .OrderBy(r => r.PreparedByEmployeeName)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                case "SectionName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            storePurchaseRequisitions = storePurchaseRequisitions
                             .OrderByDescending(r => r.SectionName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            storePurchaseRequisitions = storePurchaseRequisitions
                                 .OrderBy(r => r.SectionName)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                case "LneName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            storePurchaseRequisitions = storePurchaseRequisitions
                             .OrderByDescending(r => r.LineName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            storePurchaseRequisitions = storePurchaseRequisitions
                                 .OrderBy(r => r.LineName)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;

                default:

                    storePurchaseRequisitions = storePurchaseRequisitions
                     .OrderByDescending(r => r.RequisitionNo).ThenByDescending(x=>x.RequisitionNoteDate)
                     .Skip(index * pageSize)
                     .Take(pageSize);
                    break;

            }
    
            return storePurchaseRequisitions.ToList();
        }
    }
}

#region

//const int store = (int)SCERP.Common.AuthorizationType.RequisitionPreparation;
//     AuthorizedPerson authorizedPerson = _authorizedPersonRepository.GetAuthorizedPersonByEmployeeId(userId, processKeyId);
//     IQueryable<VStorePurchaseRequisition> storePurchaseRequisitions = _storePurchaseRequisitionRepository.GetStorePurchaseRequisitionsByPaging(model);
//     totalRecords = storePurchaseRequisitions.Count();
//     switch (authorizedPerson.AuthorizationTypeId)
//     {
//         case store:
//           storePurchaseRequisitions=  storePurchaseRequisitions.Where(x => x.AuthorizationId == authorizedPerson.AuthorizationTypeId && x.SubmittedTo == userId);
//             break;
//         default:
//            storePurchaseRequisitions= storePurchaseRequisitions.Where(x=>x.PreparedBy==userId);

//             break;
//     }

#endregion