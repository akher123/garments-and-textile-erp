using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.DAL.Repository.InventoryRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class MaterialIssueRequisitionManager : IMaterialIssueRequisitionManager
    {

        private readonly IInventoryAuthorizedPersonRepository _authorizedPersonRepository;
        private readonly IMaterialIssueRequisitionRepository _materialIssueRequisitionRepository;
        private readonly IMaterialIssueRequisitionDetailRepository _materialIssueRequisitionDetailRepository;
        public ResponsModel ResponsModel { get; set; }
        public MaterialIssueRequisitionManager(SCERPDBContext context)
        {
            _materialIssueRequisitionDetailRepository = new MaterialIssueRequisitionDetailRepository(context);
            _materialIssueRequisitionRepository = new MaterialIssueRequisitionRepository(context);
            _authorizedPersonRepository = new InventoryAuthorizedPersonRepository(context);
            ResponsModel = new ResponsModel();
        }

        public List<VMaterialIssueRequisition> GetMaterialIssueRequisitionByPaging(VMaterialIssueRequisition model, out int totalRecords)
        {
            totalRecords = 0;
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            bool isUserIsStore = _authorizedPersonRepository.CheckUserIsStorePerson((int)InventoryProcessType.StorePurchaseRequisition, (int)StorePurchaseRequisition.Store, PortalContext.CurrentUser.UserId);
            Expression<Func<VMaterialIssueRequisition, bool>> predicate;
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
                                 && (x.IsSentToStore) && (x.SubmittedTo == PortalContext.CurrentUser.UserId);
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

                                 && (x.IssueReceiveNoteNo == model.IssueReceiveNoteNo || model.IssueReceiveNoteNo == null)
                                 && (x.CreatedBy == PortalContext.CurrentUser.UserId);
            }
            var vMaterialIssueRequisitions = _materialIssueRequisitionRepository.GetMaterialIssueRequisitionByPaging(predicate);
            switch (model.sort)
            {
                case "UnitName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vMaterialIssueRequisitions = vMaterialIssueRequisitions
                                  .OrderByDescending(r => r.UnitName)
                                  .Skip(index * pageSize)
                                  .Take(pageSize);
                            break;
                        default:
                            vMaterialIssueRequisitions = vMaterialIssueRequisitions
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
                            vMaterialIssueRequisitions = vMaterialIssueRequisitions
                             .OrderByDescending(r => r.DepartmentName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssueRequisitions = vMaterialIssueRequisitions
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
                            vMaterialIssueRequisitions = vMaterialIssueRequisitions
                             .OrderByDescending(r => r.PreparedByEmployeeName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssueRequisitions = vMaterialIssueRequisitions
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
                            vMaterialIssueRequisitions = vMaterialIssueRequisitions
                             .OrderByDescending(r => r.SectionName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssueRequisitions = vMaterialIssueRequisitions
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
                            vMaterialIssueRequisitions = vMaterialIssueRequisitions
                             .OrderByDescending(r => r.LineName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssueRequisitions = vMaterialIssueRequisitions
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
                            vMaterialIssueRequisitions = vMaterialIssueRequisitions
                             .OrderByDescending(r => r.IssueReceiveNoteNo)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            vMaterialIssueRequisitions = vMaterialIssueRequisitions
                                 .OrderBy(r => r.IssueReceiveNoteNo)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    vMaterialIssueRequisitions = vMaterialIssueRequisitions
                      .OrderByDescending(r => r.MaterialIssueRequisitionId)
                      .Skip(index * pageSize)
                      .Take(pageSize);
                    break;

            }
            return vMaterialIssueRequisitions.ToList();
        }

        public ResponsModel SaveMaterialIssueRequisition(Inventory_MaterialIssueRequisition materialIssueRequisition)
        {

            var isExistIrnNumber =
                _materialIssueRequisitionRepository.Exists(
                    x => x.IssueReceiveNoteNo == materialIssueRequisition.IssueReceiveNoteNo && x.IsActive);

            if (!isExistIrnNumber)
            {

                int effectedRows = _materialIssueRequisitionRepository.Save(materialIssueRequisition);
                if (effectedRows > 0)
                {
                    ResponsModel.Status = true;
                    ResponsModel.Message = "Saved Successfylly";
                }
                else
                {
                    ResponsModel.Message = "Data Not Saved";
                }

            }
            else
            {
                ResponsModel.Message = "IRN Note NO  already Issued ! Please Issue new one.";
            }
            return ResponsModel;
        }

        public VMaterialIssueRequisition GetVMaterialIssueRequisitionById(int materialIssueRequisitionId)
        {
            return _materialIssueRequisitionRepository.GetVMaterialIssueRequisitionById(materialIssueRequisitionId);
        }

        public List<Inventory_MaterialIssueRequisitionDetail> GetMaterialIssueRequisitionDetails(int materialIssueRequisitionId)
        {
            return
                _materialIssueRequisitionDetailRepository.Filter(
                    x => x.IsActive && x.MaterialIssueRequisitionId == materialIssueRequisitionId).ToList();
        }

        public ResponsModel EditMaterialIssueRequisition(Inventory_MaterialIssueRequisition model)
        {

            var editIndex = 0;
            var materialIssueRequisitionObj =
                _materialIssueRequisitionRepository.FindOne(x => x.MaterialIssueRequisitionId == model.MaterialIssueRequisitionId);
            materialIssueRequisitionObj.BranchUnitDepartmentId = model.BranchUnitDepartmentId;
            materialIssueRequisitionObj.DepartmentSectionId = model.DepartmentSectionId;
            materialIssueRequisitionObj.DepartmentLineId = model.DepartmentLineId;
            materialIssueRequisitionObj.PreparedBy = model.PreparedBy;
            materialIssueRequisitionObj.SubmittedTo = model.SubmittedTo;
            materialIssueRequisitionObj.IsModifiedByStore = model.IsModifiedByStore;
            materialIssueRequisitionObj.SendingDate = model.SendingDate;
            materialIssueRequisitionObj.IssueReceiveNoteNo = model.IssueReceiveNoteNo;
            materialIssueRequisitionObj.IssueReceiveNoteDate = model.IssueReceiveNoteDate;
            materialIssueRequisitionObj.Remarks = model.Remarks;
            materialIssueRequisitionObj.IsSentToStore = model.IsSentToStore;
            if (materialIssueRequisitionObj.IsModifiedByStore)
            {
                ResponsModel.Message = "Material Issue Requisition already Modified  by Store Manager! please create new issue requsition";
            }
            else
            {
                foreach (var requisitionIssueDetail in model.Inventory_MaterialIssueRequisitionDetail)
                {
                    if (requisitionIssueDetail.MaterialIssueRequisitionDetailId > 0)
                    {
                        var materialRequisitionDetail = _materialIssueRequisitionDetailRepository.FindOne(x => x.MaterialIssueRequisitionDetailId == requisitionIssueDetail.MaterialIssueRequisitionDetailId);
                        materialRequisitionDetail.ItemName = requisitionIssueDetail.ItemName;
                        materialRequisitionDetail.DesiredDate = requisitionIssueDetail.DesiredDate;
                        materialRequisitionDetail.Size = requisitionIssueDetail.Size;
                        materialRequisitionDetail.Brand = requisitionIssueDetail.Brand;
                        materialRequisitionDetail.Origin = requisitionIssueDetail.Origin;
                        materialRequisitionDetail.RequiredQuantity = requisitionIssueDetail.RequiredQuantity;
                        materialRequisitionDetail.MeasurementUnit = requisitionIssueDetail.MeasurementUnit;
                        materialRequisitionDetail.DesiredDate = requisitionIssueDetail.DesiredDate;
                        materialRequisitionDetail.FunctionalArea = requisitionIssueDetail.FunctionalArea;
                        materialRequisitionDetail.Machine = requisitionIssueDetail.Machine;
                        materialRequisitionDetail.Remarks = requisitionIssueDetail.Remarks;
                        materialRequisitionDetail.EditedBy = PortalContext.CurrentUser.UserId;

                        materialRequisitionDetail.EditedDate = DateTime.Now;
                        materialRequisitionDetail.IsActive = true;
                        editIndex += _materialIssueRequisitionDetailRepository.Edit(materialRequisitionDetail);
                    }
                    else
                    {
                        requisitionIssueDetail.IsActive = true;
                        requisitionIssueDetail.CreatedBy = PortalContext.CurrentUser.UserId;
                        requisitionIssueDetail.CreatedDate = DateTime.Now;
                        requisitionIssueDetail.MaterialIssueRequisitionId = model.MaterialIssueRequisitionId;
                        editIndex += _materialIssueRequisitionDetailRepository.Save(requisitionIssueDetail);
                    }
                    editIndex += _materialIssueRequisitionRepository.Edit(materialIssueRequisitionObj);
                    ResponsModel.Status = editIndex > 0;
                }


                ResponsModel.Message = "Material Issue Requisition Edit Successfully";
            }


            return ResponsModel;
        }

        public int DeleteMaterialIssueRequisition(int? materialIssueRequisitionId)
        {
            var inventoryMaterialRequisitionDetail = _materialIssueRequisitionRepository.FindOne(
              x => x.MaterialIssueRequisitionId == materialIssueRequisitionId);
            inventoryMaterialRequisitionDetail.EditedBy = PortalContext.CurrentUser.UserId;
            inventoryMaterialRequisitionDetail.EditedDate = DateTime.Now;
            inventoryMaterialRequisitionDetail.IsActive = false;
            var edit = _materialIssueRequisitionRepository.Edit(inventoryMaterialRequisitionDetail);
            return edit;
        }

        public int DeleteMaterialIssueRequisitionDetail(int? materialIssueRequisitionDetailId)
        {
            var inventoryMaterialRequisitionDetail = _materialIssueRequisitionDetailRepository.FindOne(
              x => x.MaterialIssueRequisitionDetailId == materialIssueRequisitionDetailId);
            inventoryMaterialRequisitionDetail.EditedBy = PortalContext.CurrentUser.UserId;
            inventoryMaterialRequisitionDetail.EditedDate = DateTime.Now;
            inventoryMaterialRequisitionDetail.IsActive = false;
            var edit = _materialIssueRequisitionDetailRepository.Edit(inventoryMaterialRequisitionDetail);
            return edit;
        }

        public bool CheckModifiedByStore(int materialIssueRequisitionId)
        {
            return
                _materialIssueRequisitionRepository.Exists(
                    x => x.IsModifiedByStore && x.MaterialIssueRequisitionId == materialIssueRequisitionId);
        }
    }
}
