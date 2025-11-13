using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.Repository.InventoryRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class InventoryApprovalStatusManager : IInventoryApprovalStatusManager
    {
        private readonly InventoryApprovalStatusRepository _inventoryApprovalStatusRepository;
        public InventoryApprovalStatusManager(SCERPDBContext context)
        {
            _inventoryApprovalStatusRepository=new InventoryApprovalStatusRepository(context);
        }

        public List<Inventory_ApprovalStatus> GetInventoryApprovalStatusByPaging(Inventory_ApprovalStatus model, out int totalRecords)
        {
            int index = model.PageIndex;
            int pageSize = AppConfig.PageSize;
            IQueryable<Inventory_ApprovalStatus> approvalStatuses = _inventoryApprovalStatusRepository.GetInventoryApprovalStatusByPaging(model);
            totalRecords = approvalStatuses.Count();
            switch (model.sort)
            {
                case "StatusName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            approvalStatuses = approvalStatuses
                                .OrderByDescending(r => r.StatusName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            approvalStatuses = approvalStatuses
                                .OrderBy(r => r.StatusName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    approvalStatuses = approvalStatuses
                               .OrderByDescending(r => r.StatusName)
                               .Skip(index * pageSize)
                               .Take(pageSize);
                    break;
            }
            return approvalStatuses.ToList();
        }

        public Inventory_ApprovalStatus GetInventoryApprovalStatusById(int approvalStatusId)
        {
            return _inventoryApprovalStatusRepository.FindOne(x => x.IsActive && x.ApprovalStatusId == approvalStatusId);
        }

        public int EditInventoryApprovalStatus(Inventory_ApprovalStatus model)
        {
            var editIndex = 0;
            try
            {
                var inventoryApprovalStatus = _inventoryApprovalStatusRepository.FindOne(x => x.ApprovalStatusId == model.ApprovalStatusId);
                inventoryApprovalStatus.StatusName = model.StatusName;
                inventoryApprovalStatus.Description = model.Description;
                inventoryApprovalStatus.EditedBy = PortalContext.CurrentUser.UserId;
                inventoryApprovalStatus.EditedDate = DateTime.Now;
                inventoryApprovalStatus.IsActive = true;
                editIndex = _inventoryApprovalStatusRepository.Edit(inventoryApprovalStatus);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return editIndex;
        }

        public int SaveInventoryApprovalStatus(Inventory_ApprovalStatus model)
        {
            var saveIndex = 0;
            try
            {
                model.CreatedBy = PortalContext.CurrentUser.UserId;
                model.CreatedDate = DateTime.Now;
                model.IsActive = true;
                saveIndex = _inventoryApprovalStatusRepository.Save(model);
            }
            catch (Exception exception)
            {

                throw exception;
            }
            return saveIndex;
        }

        public int DeleteInventoryApprovalStatus(int approvalStatusId)
        {
            var deleteIndex = 0;
            try
            {
                var approvalStaus = _inventoryApprovalStatusRepository.FindOne(x => x.ApprovalStatusId == approvalStatusId);
                approvalStaus.EditedBy = PortalContext.CurrentUser.UserId;
                approvalStaus.EditedDate = DateTime.Now;
                approvalStaus.IsActive = false;
                deleteIndex = _inventoryApprovalStatusRepository.Edit(approvalStaus);
            }
            catch (Exception exception)
            {

                throw exception;
            }
            return deleteIndex;
        }

  

        public bool IsExistApprovalStatus(Inventory_ApprovalStatus model)
        {
            return _inventoryApprovalStatusRepository.Exists(x => x.StatusName.ToLower().Equals(model.StatusName.ToLower()) && x.ApprovalStatusId != model.ApprovalStatusId);
        }

        public List<Inventory_ApprovalStatus> GetInventoryApprovalStatus()
        {
            return _inventoryApprovalStatusRepository.Filter(x => x.IsActive).ToList();
        }
    }
}
