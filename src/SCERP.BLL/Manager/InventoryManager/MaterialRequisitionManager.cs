using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.DAL.Repository.InventoryRepository;
using SCERP.Model;
namespace SCERP.BLL.Manager.InventoryManager
{
    public class MaterialRequisitionManager : IMaterialRequisitionManager
    {

        private readonly IMaterialRequisitionRepository _materialRequisitionRepository;
        private readonly IMaterialRequisitionDetailRepository _materialRequisitionDetailRepository;
        public ResponsModel ResponsModel { get; set; }
        private readonly IInventoryAuthorizedPersonRepository _authorizedPersonRepository;
        public MaterialRequisitionManager(DAL.SCERPDBContext context)
        {
            this._materialRequisitionRepository = new MaterialRequisitionRepository(context);
            _materialRequisitionDetailRepository = new MaterialRequisitionDetailRepository(context);
            ResponsModel = new ResponsModel();

            _authorizedPersonRepository = new InventoryAuthorizedPersonRepository(context);
        }

        public List<VMaterialRequisition> GetMaterialRequisitionByPaging(VMaterialRequisition model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            bool isUserIsStore = _authorizedPersonRepository.CheckUserIsStorePerson((int)InventoryProcessType.StorePurchaseRequisition, (int)StorePurchaseRequisition.Store, PortalContext.CurrentUser.UserId);
            Expression<Func<VMaterialRequisition, bool>> predicate;
            if (isUserIsStore)
            {
                predicate = x => (x.BranchId == model.BranchId || model.BranchId == 0)
                     && (x.IsModifiedByStore == model.IsModifiedByStore || model.IsModifiedByStore == null)
                                 && (x.CompanyId == model.CompanyId || model.CompanyId == 0)
                                 &&
                                 (x.BranchUnitDepartmentId == model.BranchUnitDepartmentId ||
                                  model.BranchUnitDepartmentId == 0)
                                 && (x.BranchUnitId == model.BranchUnitId || model.BranchUnitId == 0)
                                 &&
                                 (x.DepartmentSectionId == model.DepartmentSectionId ||
                                  model.DepartmentSectionId == null)
                                 && (x.DepartmentLineId == model.DepartmentLineId || model.DepartmentLineId == null)
                                 && (x.RequisitionNoteNo == model.RequisitionNoteNo || model.RequisitionNoteNo == null)
                                 && (x.IsSentToStore) && (x.SubmittedTo == PortalContext.CurrentUser.UserId);
            }
            else
            {

                predicate = x => (x.BranchId == model.BranchId || model.BranchId == 0)
                                 && (x.CompanyId == model.CompanyId || model.CompanyId == 0)
                                      && (x.IsModifiedByStore == model.IsModifiedByStore || model.IsModifiedByStore == null)
                                 &&
                                 (x.BranchUnitDepartmentId == model.BranchUnitDepartmentId ||
                                  model.BranchUnitDepartmentId == 0)
                                 && (x.BranchUnitId == model.BranchUnitId || model.BranchUnitId == 0)
                                 &&
                                 (x.DepartmentSectionId == model.DepartmentSectionId || model.DepartmentSectionId == null)
                                 && (x.DepartmentLineId == model.DepartmentLineId || model.DepartmentLineId == null)

                                 && (x.RequisitionNoteNo == model.RequisitionNoteNo || model.RequisitionNoteNo == null)
                                 && (x.CreatedBy == PortalContext.CurrentUser.UserId);
            }

            var vMaterialRequisitions =
                _materialRequisitionRepository.GetMaterialRequisitions(predicate);
            totalRecords = vMaterialRequisitions.Count();

            switch (model.sort)
            {
                case "UnitName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialRequisitions = vMaterialRequisitions
                                  .OrderByDescending(r => r.UnitName)
                                  .Skip(index * pageSize)
                                  .Take(pageSize);
                            break;
                        default:
                            vMaterialRequisitions = vMaterialRequisitions
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
                            vMaterialRequisitions = vMaterialRequisitions
                             .OrderByDescending(r => r.DepartmentName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialRequisitions = vMaterialRequisitions
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
                            vMaterialRequisitions = vMaterialRequisitions
                             .OrderByDescending(r => r.PreparedByEmployeeName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialRequisitions = vMaterialRequisitions
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
                            vMaterialRequisitions = vMaterialRequisitions
                             .OrderByDescending(r => r.SectionName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialRequisitions = vMaterialRequisitions
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
                            vMaterialRequisitions = vMaterialRequisitions
                             .OrderByDescending(r => r.LineName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialRequisitions = vMaterialRequisitions
                                 .OrderBy(r => r.LineName)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    vMaterialRequisitions = vMaterialRequisitions
                      .OrderBy(r => r.RequisitionNoteDate)
                      .Skip(index * pageSize)
                      .Take(pageSize);
                    break;

            }
            return vMaterialRequisitions.ToList();
        }

        public ResponsModel SaveMaterialRequisition(Inventory_MaterialRequisition materialRequisition)
        {
            var isExistRNONumber =
               _materialRequisitionRepository.Exists(
                   x => x.RequisitionNoteNo == materialRequisition.RequisitionNoteNo && x.IsActive);
            var isUserIsStore = _authorizedPersonRepository.CheckUserIsStorePerson((int)InventoryProcessType.StorePurchaseRequisition, (int)StorePurchaseRequisition.Store, PortalContext.CurrentUser.UserId);
            if (isUserIsStore)
            {
                materialRequisition.IsSentToStore = true;

            }
            if (!isExistRNONumber)
            {
                var effectedRows = _materialRequisitionRepository.Save(materialRequisition);
                if (effectedRows > 0)
                {
                    ResponsModel.Status = true;
                    ResponsModel.Message = "Saved Successfylly";
                }
                else
                {
                    ResponsModel.Status = false;
                    ResponsModel.Message = "Data Not seved";
                }

            }
            else
            {
                ResponsModel.Message = "Requsition Note Number  already Used ! Please use new one.";
            }
            return ResponsModel;

        }

        public VMaterialRequisition GetVMaterialRequisitionById(int materialRequisitionId)
        {
            return _materialRequisitionRepository.GetVMaterialRequisitionById(materialRequisitionId);
        }

        public List<Inventory_MaterialRequisitionDetail> GetMaterialRequisitionDetails(int materialRequisitionId)
        {
            return _materialRequisitionRepository.GetMaterialRequisitionDetails(materialRequisitionId);
        }

        public ResponsModel EditMaterialRequisition(Inventory_MaterialRequisition model)
        {
            var editIndex = 0;
            var materialRequisition =
                _materialRequisitionRepository.FindOne(x => x.MaterialRequisitionId == model.MaterialRequisitionId);
            var isUserIsStore = _authorizedPersonRepository.CheckUserIsStorePerson((int)InventoryProcessType.StorePurchaseRequisition, (int)StorePurchaseRequisition.Store, PortalContext.CurrentUser.UserId);
            if (isUserIsStore)
            {
                model.IsSentToStore = true;

            }
            if (materialRequisition.IsModifiedByStore)
            {
                ResponsModel.Message = "Material  Requisition already modified by store! please create new requsition";
            }
            else
            {
                materialRequisition.BranchUnitDepartmentId = model.BranchUnitDepartmentId;
                materialRequisition.DepartmentSectionId = model.DepartmentSectionId;
                materialRequisition.DepartmentLineId = model.DepartmentLineId;
                materialRequisition.PreparedBy = model.PreparedBy;
                materialRequisition.ModifiedBy = materialRequisition.SubmittedTo; //  modifiedBy=SubmittedTo whene edit 
                materialRequisition.SubmittedTo = model.SubmittedTo;
                materialRequisition.IsModifiedByStore = model.IsModifiedByStore;
                materialRequisition.IsSentToStore = model.IsSentToStore;
                materialRequisition.SendingDate = DateTime.Now;
                materialRequisition.RequisitionNoteNo = model.RequisitionNoteNo;
                materialRequisition.RequisitionNoteDate = model.RequisitionNoteDate;
                materialRequisition.Remarks = model.Remarks;
                foreach (var requisitionDetail in model.Inventory_MaterialRequisitionDetail)
                {
                    if (requisitionDetail.MaterialRequisitionDetailId > 0)
                    {
                        var materialRequisitionDetail =
                            _materialRequisitionDetailRepository.FindOne(
                                x => x.MaterialRequisitionDetailId == requisitionDetail.MaterialRequisitionDetailId);
                        materialRequisitionDetail.ItemName = requisitionDetail.ItemName;
                        materialRequisitionDetail.DesiredDate = requisitionDetail.DesiredDate;
                        materialRequisitionDetail.Size = requisitionDetail.Size;
                        materialRequisitionDetail.Brand = requisitionDetail.Brand;
                        materialRequisitionDetail.Origin = requisitionDetail.Origin;
                        materialRequisitionDetail.RequiredQuantity = requisitionDetail.RequiredQuantity;
                        materialRequisitionDetail.MeasurementUnit = requisitionDetail.MeasurementUnit;
                        materialRequisitionDetail.DesiredDate = requisitionDetail.DesiredDate;
                        materialRequisitionDetail.FunctionalArea = requisitionDetail.FunctionalArea;
                        materialRequisitionDetail.ApprovedQuantity = requisitionDetail.ApprovedQuantity;
                        materialRequisitionDetail.Remarks = requisitionDetail.Remarks;
                        materialRequisitionDetail.EditedBy = PortalContext.CurrentUser.UserId;
                        materialRequisitionDetail.EditedDate = DateTime.Now;

                        editIndex += _materialRequisitionDetailRepository.Edit(materialRequisitionDetail);
                    }
                    else
                    {
                        requisitionDetail.IsActive = true;
                        requisitionDetail.CreatedBy = PortalContext.CurrentUser.UserId;
                        requisitionDetail.CreatedDate = DateTime.Now;
                        requisitionDetail.MaterialRequisitionId = model.MaterialRequisitionId;
                        editIndex += _materialRequisitionDetailRepository.Save(requisitionDetail);
                    }


                }
                editIndex = +_materialRequisitionRepository.Edit(materialRequisition);
                ResponsModel.Status = editIndex > 0;
                ResponsModel.Message = "Edid Successfully";


            }

            return ResponsModel;
        }

        public bool IsExistMaterialRequisitionNoteNo(VMaterialRequisition model)
        {
            return _materialRequisitionRepository.IsExistMaterialRequisitionNoteNo(model);

        }

        public bool CheckModifiedByStore(int materialRequisitionId)
        {
            return
                _materialRequisitionRepository.Exists(
                    x => x.IsModifiedByStore && x.MaterialRequisitionId == materialRequisitionId);
        }

        public int DeleteMaterialRequisitionDetail(int? materialRequisitionDetailId)
        {
            var inventoryMaterialRequisitionDetail = _materialRequisitionDetailRepository.FindOne(
                x => x.MaterialRequisitionDetailId == materialRequisitionDetailId);
            inventoryMaterialRequisitionDetail.EditedBy = PortalContext.CurrentUser.UserId;
            inventoryMaterialRequisitionDetail.EditedDate = DateTime.Now;
            inventoryMaterialRequisitionDetail.IsActive = false;
            var edit = _materialRequisitionDetailRepository.Edit(inventoryMaterialRequisitionDetail);
            return edit;
        }
    }
}
