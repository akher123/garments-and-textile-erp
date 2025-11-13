using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.DAL.Repository.InventoryRepository;
using SCERP.DAL.Repository.ProductionRepository;
using SCERP.Model;
using SCERP.Model.InventoryModel;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class MaterialIssueManager : IMaterialIssueManager
    {
        private readonly IMaterialIssueRepository _materialIssueRepository;
        private readonly IMaterialIssueRequisitionRepository _materialIssueRequisitionRepository;
        private readonly IMaterialIssueDetailRepository _materialIssueDetailRepository;
        private readonly IInventoryAuthorizedPersonRepository _authorizedPersonRepository;
        private readonly IBatchRepository _batchRepository;
        public ResponsModel ResponsModel { get; set; }
        private readonly IStoreLedgerRepository _storeLedgerRepository;
        public MaterialIssueManager(SCERPDBContext context)
        {
            ResponsModel = new ResponsModel();
            _authorizedPersonRepository = new InventoryAuthorizedPersonRepository(context);
            _materialIssueRepository = new MaterialIssueRepository(context);
            _materialIssueDetailRepository = new MaterialIssueDetailRepository(context);
            _materialIssueRequisitionRepository = new MaterialIssueRequisitionRepository(context);
            _storeLedgerRepository = new StoreLedgerRepository(context);
            _batchRepository = new BatchRepository(context);
        }

        public VMaterialIssue GetVMaterialIssueById(int materialIssueId)
        {
            return _materialIssueRepository.GetVMaterialIssueById(materialIssueId);
        }

        public string GetNewIssueReceiveNo()
        {
            return _materialIssueRepository.GetNewIssueReceiveNo();
        }

        public int EditMaterialIssue(Inventory_MaterialIssue model)
        {
            var delete = 0;
            model.EditedBy = PortalContext.CurrentUser.UserId;
            model.EditedDate = DateTime.Now;
            model.IsActive = true;
            model.Inventory_StoreLedger =
                    model.Inventory_MaterialIssueDetail.Select(x => new Inventory_StoreLedger()
                    {
                        ItemId = x.ItemId,
                        BrandId = x.BrandId,
                        SizeId = x.SizeId,
                        OriginId = x.OriginId,
                        MaterialIssueId = x.MaterialIssueId,
                        CurrencyId = x.CurrencyId,
                        TransactionType = Convert.ToString((int)StoreLedgerTransactionType.Issue),
                        Quantity = x.IssuedQuantity,
                        UnitPrice = x.IssuedItemRate,
                        Amount = x.IssuedQuantity * x.IssuedItemRate,
                        TransactionDate = x.TransactionDate.GetValueOrDefault(),
                        IsActive = true,
                        CreatedBy = PortalContext.CurrentUser.UserId,
                        CreatedDate = DateTime.Now,

                    }).ToList();

            using (var transaction = new TransactionScope())
            {

                delete += _storeLedgerRepository.Delete(x => x.MaterialIssueId == model.MaterialIssueId);
                delete += _materialIssueDetailRepository.Delete(x => x.IsActive && x.MaterialIssueId == model.MaterialIssueId);
                delete += _materialIssueRepository.Delete(x => x.MaterialIssueId == model.MaterialIssueId);
                delete += _materialIssueRequisitionRepository.Delete(x => x.MaterialIssueRequisitionId == model.MaterialIssueRequisitionId);
                model.MaterialIssueRequisitionId = 0;
                delete += _materialIssueRepository.Save(model);
                transaction.Complete();
            }
            return delete;
        }

        private int SendToStoreLadger(Inventory_MaterialIssueDetail issueDetail)
        {
            var storeLedger = new Inventory_StoreLedger()
            {
                ItemId = issueDetail.ItemId,
                BrandId = issueDetail.BrandId,
                SizeId = issueDetail.SizeId,
                OriginId = issueDetail.OriginId,
                MaterialIssueId = issueDetail.MaterialIssueId,
                CurrencyId = issueDetail.CurrencyId,
                TransactionType = Convert.ToString((int)StoreLedgerTransactionType.Issue),
                Quantity = issueDetail.IssuedQuantity,
                UnitPrice = issueDetail.IssuedItemRate,
                Amount = issueDetail.IssuedQuantity * issueDetail.IssuedItemRate,
                TransactionDate = issueDetail.TransactionDate.GetValueOrDefault(),
                IsActive = true,
                CreatedBy = PortalContext.CurrentUser.UserId,
                CreatedDate = DateTime.Now,
            };

            return _storeLedgerRepository.Save(storeLedger); ;
        }

        public int SaveMaterialIssue(Inventory_MaterialIssue materialIssue)
        {
            var saveIndex = 0;
            materialIssue.IssueReceiveNo = _materialIssueRepository.GetNewIssueReceiveNo();
            var isExistIrnNumber =  _materialIssueRepository.Exists(x => x.IssueReceiveNo == materialIssue.IssueReceiveNo && x.IsActive);
            if (!isExistIrnNumber)
            {
                bool isExistInvocie = _materialIssueRequisitionRepository.Exists( x => x.IssueReceiveNoteNo == materialIssue.Inventory_MaterialIssueRequisition.IssueReceiveNoteNo);
                if (!isExistInvocie)
                {
                    using (var transaction = new TransactionScope())
                    {
                        materialIssue.Inventory_StoreLedger =
                        materialIssue.Inventory_MaterialIssueDetail.Select(x => new Inventory_StoreLedger()
                        {
                            ItemId = x.ItemId,
                            BrandId = x.BrandId,
                            SizeId = x.SizeId,
                            OriginId = x.OriginId,
                            MaterialIssueId = x.MaterialIssueId,

                            CurrencyId = x.CurrencyId,
                            TransactionType = Convert.ToString((int)StoreLedgerTransactionType.Issue),
                            Quantity = x.IssuedQuantity,
                            UnitPrice = x.IssuedItemRate,
                            Amount = x.IssuedQuantity * x.IssuedItemRate,
                            TransactionDate = x.TransactionDate.GetValueOrDefault(),
                            IsActive = true,
                            CreatedBy = PortalContext.CurrentUser.UserId,
                            CreatedDate = DateTime.Now,

                        }).ToList();

                        saveIndex += _materialIssueRepository.Save(materialIssue);
                        var oldeMIssueReq = _materialIssueRequisitionRepository.FindOne(x => x.MaterialIssueRequisitionId == materialIssue.MaterialIssueRequisitionId);
                        oldeMIssueReq.IsModifiedByStore = true;
                        saveIndex += _materialIssueRequisitionRepository.Edit(oldeMIssueReq);
                        transaction.Complete();
                    }
                    
                }
                else
                {
                    throw new Exception(materialIssue.Inventory_MaterialIssueRequisition.IssueReceiveNoteNo+" "+ "Invoce/IRN NO Number already Exist");
                }
            
              

            }
            return saveIndex;
        }



        public List<VMaterialIssue> GetMaterialIssuesByPaging(VMaterialIssue model, out int totalRecords)
        {

            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            bool isUserIsStore = _authorizedPersonRepository.CheckUserIsStorePerson((int)InventoryProcessType.StorePurchaseRequisition, (int)StorePurchaseRequisition.Store, PortalContext.CurrentUser.UserId);
            Expression<Func<VMaterialIssue, bool>> predicate;
            if (isUserIsStore)
            {
                predicate = x => (x.BranchId == model.BranchId || model.BranchId == 0)
                                 && (x.CompanyId == model.CompanyId || model.CompanyId == 0)
                                 &&
                                 (x.BranchUnitDepartmentId == model.BranchUnitDepartmentId ||
                                  model.BranchUnitDepartmentId == 0)
                                 && (x.BranchUnitId == model.BranchUnitId || model.BranchUnitId == 0)
                                 &&
                                 (x.DepartmentSectionId == model.DepartmentSectionId ||
                                  model.DepartmentSectionId == null)
                                 && (x.DepartmentLineId == model.DepartmentLineId || model.DepartmentLineId == null)
                                 && (x.IssueReceiveNoteNo == model.IssueReceiveNoteNo || model.IssueReceiveNoteNo == null)
                                       && (x.IssueReceiveNo == model.IssueReceiveNo || model.IssueReceiveNo == null)
                                    && (x.PreparedByStore == PortalContext.CurrentUser.UserId);
            }
            else
            {
                predicate = x => (x.BranchId == model.BranchId || model.BranchId == 0)
                                 && (x.CompanyId == model.CompanyId || model.CompanyId == 0)
                                 &&
                                 (x.BranchUnitDepartmentId == model.BranchUnitDepartmentId ||
                                  model.BranchUnitDepartmentId == 0)
                                 && (x.BranchUnitId == model.BranchUnitId || model.BranchUnitId == 0)
                                 &&
                                 (x.DepartmentSectionId == model.DepartmentSectionId || model.DepartmentSectionId == null)
                                 && (x.DepartmentLineId == model.DepartmentLineId || model.DepartmentLineId == null)
                                   && (x.IssueReceiveNo == model.IssueReceiveNo || model.IssueReceiveNo == null)
                                 && (x.IssueReceiveNoteNo == model.IssueReceiveNoteNo || model.IssueReceiveNoteNo == null)
                                    && (x.PreparedByStore == PortalContext.CurrentUser.UserId);
            }
            var vMaterialIssues = _materialIssueRepository.GetMaterialIssuesByPaging(predicate);
            totalRecords = vMaterialIssues.Count();
            switch (model.sort)
            {

                case "UnitName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialIssues = vMaterialIssues
                                  .OrderByDescending(r => r.UnitName)
                                  .Skip(index * pageSize)
                                  .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
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
                            vMaterialIssues = vMaterialIssues
                             .OrderByDescending(r => r.DepartmentName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
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
                            vMaterialIssues = vMaterialIssues
                             .OrderByDescending(r => r.PreparedByEmployeeName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
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
                            vMaterialIssues = vMaterialIssues
                             .OrderByDescending(r => r.SectionName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
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
                            vMaterialIssues = vMaterialIssues
                             .OrderByDescending(r => r.LineName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
                                 .OrderBy(r => r.LineName)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                case "IssueReceiveNoteNo":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialIssues = vMaterialIssues
                             .OrderByDescending(r => r.IssueReceiveNoteNo)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
                                 .OrderBy(r => r.IssueReceiveNoteNo)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                case "IssueReceiveNoteDate":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialIssues = vMaterialIssues
                             .OrderByDescending(r => r.IssueReceiveNoteDate)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
                                 .OrderBy(r => r.IssueReceiveNoteDate)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;

                case "IssueReceiveNo":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialIssues = vMaterialIssues
                             .OrderByDescending(r => r.IssueReceiveNo)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
                                 .OrderBy(r => r.IssueReceiveNo)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                case "IssueReceiveDate":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialIssues = vMaterialIssues
                             .OrderByDescending(r => r.IssueReceiveDate)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
                                 .OrderBy(r => r.IssueReceiveDate)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    vMaterialIssues = vMaterialIssues
                      .OrderByDescending(r => r.IssueReceiveNo)
                      .Skip(index * pageSize)
                      .Take(pageSize);
                    break;

            }

            return vMaterialIssues.ToList();
        }

        public List<VMaterialIssueDetail> GetMaterialIssueDetails(int materialIssueId)
        {
            return
                _materialIssueDetailRepository.GetMaterialIssueDetails(materialIssueId);
        }

        public int DeleteMaterialIssue(int? materialIssueId)
        {
            int delete = 0;
            using (var transaction = new TransactionScope())
            {
                delete += _materialIssueDetailRepository.Delete(x => x.MaterialIssueId == materialIssueId);
                delete += _storeLedgerRepository.Delete(x => x.MaterialIssueId == materialIssueId && x.TransactionType == "2");
                delete += _materialIssueRepository.Delete(x => x.MaterialIssueId == materialIssueId && x.IType == 1);
                transaction.Complete();
            }
            return delete;
        }

        public int DeleteMaterialIssueDetail(int? materialIssueDetailId)
        {
            var materialIssueDetail = _materialIssueDetailRepository.FindOne(
         x => x.MaterialIssueDetailId == materialIssueDetailId);
            materialIssueDetail.EditedBy = PortalContext.CurrentUser.UserId;
            materialIssueDetail.EditedDate = DateTime.Now;
            materialIssueDetail.IsActive = false;
            var edit = _materialIssueDetailRepository.Edit(materialIssueDetail);
            return edit;
        }

        public List<Pro_Batch> GetAllBatch()
        {
            return _batchRepository.Filter(x => x.IsActive ==true ).OrderByDescending(x=>x.BatchDate).ToList();
        }

        public List<VMaterialIssue> GetMaterialIssues(VMaterialIssue model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            Expression<Func<VMaterialIssue, bool>> predicate;
            predicate = x => (x.IType == model.IType &&
                      ((x.BatchNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                      ||(x.BtRefNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                      || (x.PartyName.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                      || (x.IssueReceiveNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower())))
                     && ((x.IssueReceiveDate >= model.FromDate || model.FromDate == null) && (x.IssueReceiveDate <= model.ToDate || model.ToDate == null)));
            var vMaterialIssues = _materialIssueRepository.GetMaterialIssuesByPaging(predicate);
            totalRecords = vMaterialIssues.Count();
            switch (model.sort)
            {

                case "BatchNo":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialIssues = vMaterialIssues
                                  .OrderByDescending(r => r.BatchNo)
                                  .Skip(index * pageSize)
                                  .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
                                  .OrderBy(r => r.BatchNo)
                                  .Skip(index * pageSize)
                                  .Take(pageSize);
                            break;
                    }
                    break;
                case "PartyName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialIssues = vMaterialIssues
                             .OrderByDescending(r => r.PartyName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
                                 .OrderBy(r => r.PartyName)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;

                case "IssueReceiveNo":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialIssues = vMaterialIssues
                             .OrderByDescending(r => r.IssueReceiveNo)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
                                 .OrderBy(r => r.IssueReceiveNo)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                case "IssueReceiveDate":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialIssues = vMaterialIssues
                             .OrderByDescending(r => r.IssueReceiveDate)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
                                 .OrderBy(r => r.IssueReceiveDate)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    vMaterialIssues = vMaterialIssues
                      .OrderByDescending(r => r.IssueReceiveNo)
                      .Skip(index * pageSize)
                      .Take(pageSize);
                    break;

            }

            return vMaterialIssues.ToList();
        }

        public List<VMaterialIssue> GetGeneralIssueByPaging(VMaterialIssue model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            Expression<Func<VMaterialIssue, bool>> predicate = x => x.IType == 1&&(x.CompanyId == model.CompanyId || model.CompanyId == 0)
                                        && (x.BranchUnitDepartmentId == model.BranchUnitDepartmentId || model.BranchUnitDepartmentId == 0)
                                        && (x.BranchUnitId == model.BranchUnitId || model.BranchUnitId == 0)
                                        && (x.DepartmentSectionId == model.DepartmentSectionId || model.DepartmentSectionId == null)
                                        && (x.DepartmentLineId == model.DepartmentLineId || model.DepartmentLineId == null)
                                        &&((x.IssueReceiveNoteNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                                         ||(x.IssueReceiveNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                                           || (x.DepartmentName.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                                           || (x.SectionName.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                                           || (x.LineName.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                                           || (x.BranchName.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower())))
                                        && ((x.IssueReceiveNoteDate >= model.FromDate || model.FromDate == null) && (x.IssueReceiveNoteDate <= model.ToDate || model.ToDate == null));
            var vMaterialIssues = _materialIssueRepository.GetMaterialIssuesByPaging(predicate);
            totalRecords = vMaterialIssues.Count();
            switch (model.sort)
            {

                case "UnitName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialIssues = vMaterialIssues
                                  .OrderByDescending(r => r.UnitName)
                                  .Skip(index * pageSize)
                                  .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
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
                            vMaterialIssues = vMaterialIssues
                             .OrderByDescending(r => r.DepartmentName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
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
                            vMaterialIssues = vMaterialIssues
                             .OrderByDescending(r => r.PreparedByEmployeeName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
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
                            vMaterialIssues = vMaterialIssues
                             .OrderByDescending(r => r.SectionName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
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
                            vMaterialIssues = vMaterialIssues
                             .OrderByDescending(r => r.LineName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
                                 .OrderBy(r => r.LineName)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                case "IssueReceiveNoteNo":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialIssues = vMaterialIssues
                             .OrderByDescending(r => r.IssueReceiveNoteNo)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
                                 .OrderBy(r => r.IssueReceiveNoteNo)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                case "IssueReceiveNoteDate":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialIssues = vMaterialIssues
                             .OrderByDescending(r => r.IssueReceiveNoteDate)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
                                 .OrderBy(r => r.IssueReceiveNoteDate)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;

                case "IssueReceiveNo":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialIssues = vMaterialIssues
                             .OrderByDescending(r => r.IssueReceiveNo)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
                                 .OrderBy(r => r.IssueReceiveNo)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                case "IssueReceiveDate":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialIssues = vMaterialIssues
                             .OrderByDescending(r => r.IssueReceiveDate)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssues = vMaterialIssues
                                 .OrderBy(r => r.IssueReceiveDate)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    vMaterialIssues = vMaterialIssues
                      .OrderByDescending(r => r.IssueReceiveNo)
                      .Skip(index * pageSize)
                      .Take(pageSize);
                    break;

            }

            return vMaterialIssues.ToList();
        }

        public List<VMaterialLoanReturn> GetMaterialLoanReturns(VMaterialLoanReturn model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            Expression<Func<VMaterialLoanReturn, bool>> predicate;
            predicate = x =>
                      ((x.LoanRefNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                      || (x.Supplyer.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                      || (x.IssueReceiveNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower())))
                     && ((x.IssueReceiveDate >= model.FromDate || model.FromDate == null) && (x.IssueReceiveDate <= model.ToDate || model.ToDate == null));
            var vMaterialLoanReturns = _materialIssueRepository.GetMaterialLoanReturns(predicate);
            totalRecords = vMaterialLoanReturns.Count();
            switch (model.sort)
            {

                case "LoanRefNo":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialLoanReturns = vMaterialLoanReturns
                                  .OrderByDescending(r => r.LoanRefNo)
                                  .Skip(index * pageSize)
                                  .Take(pageSize);
                            break;
                        default:
                            vMaterialLoanReturns = vMaterialLoanReturns
                                  .OrderBy(r => r.LoanRefNo)
                                  .Skip(index * pageSize)
                                  .Take(pageSize);
                            break;
                    }
                    break;
                case "Supplyer":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialLoanReturns = vMaterialLoanReturns
                             .OrderByDescending(r => r.Supplyer)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialLoanReturns = vMaterialLoanReturns
                                 .OrderBy(r => r.Supplyer)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;

                case "IssueReceiveNo":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialLoanReturns = vMaterialLoanReturns
                             .OrderByDescending(r => r.IssueReceiveNo)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialLoanReturns = vMaterialLoanReturns
                                 .OrderBy(r => r.IssueReceiveNo)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                case "IssueReceiveDate":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialLoanReturns = vMaterialLoanReturns
                             .OrderByDescending(r => r.IssueReceiveDate)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialLoanReturns = vMaterialLoanReturns
                                 .OrderBy(r => r.IssueReceiveDate)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    vMaterialLoanReturns = vMaterialLoanReturns
                      .OrderByDescending(r => r.IssueReceiveNo)
                      .Skip(index * pageSize)
                      .Take(pageSize);
                    break;

            }

            return vMaterialLoanReturns.ToList();
        }

        public VMaterialLoanReturn GetVMaterialLoanReturnById(int materialIssueId)
        {
            return
                _materialIssueRepository.GetMaterialLoanReturns(x => x.MaterialIssueId == materialIssueId)
                    .FirstOrDefault();
        }

        public int DeleteBatchWiseIssue(int materialIssueId,int issueType)
        {
            int delete = 0;
            using (var transaction =new TransactionScope())
            {
                delete+=_materialIssueDetailRepository.Delete(x => x.MaterialIssueId == materialIssueId);
                delete+=_storeLedgerRepository.Delete(x => x.MaterialIssueId == materialIssueId && x.TransactionType == "2");
                delete+=  _materialIssueRepository.Delete(x => x.MaterialIssueId == materialIssueId && x.IType == issueType);
                transaction.Complete();
            }
            return delete;
        }

        public List<VLoanGiven> GetMaterialLoanGiven(VLoanGiven model, out int totalRecords)
        {
            IQueryable<VLoanGiven>loanGivens=  _materialIssueRepository.GetMaterialLoanGiven( model, out  totalRecords);
           

            return loanGivens.ToList();
        }

        public VLoanGiven GetVLaonGivenById(int materialIssueId)
        {
            VLoanGiven loanGiven = _materialIssueRepository.GetVLaonGivenById(materialIssueId);
            return loanGiven;
        }

        public DataTable GetChemicalIssueChallan(int materialIssueId)
        {
            return _materialIssueRepository.GetChemicalIssueChallan(materialIssueId); 
        }

        public DataTable GetLoanReturnChallan(int materialIssueId)
        {
            return _materialIssueRepository.GetLoanReturnChallan(materialIssueId); 
        }
    }
}
