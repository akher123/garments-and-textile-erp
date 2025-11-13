using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IInventoryManager
{
   public interface IInventoryApprovalStatusManager
    {
       List<Inventory_ApprovalStatus> GetInventoryApprovalStatusByPaging(Inventory_ApprovalStatus model, out int totalRecords);
       Inventory_ApprovalStatus GetInventoryApprovalStatusById(int approvalStatusId);
       int EditInventoryApprovalStatus(Inventory_ApprovalStatus model);
       int SaveInventoryApprovalStatus(Inventory_ApprovalStatus model);
       int DeleteInventoryApprovalStatus(int approvalStatusId);
       bool IsExistApprovalStatus(Inventory_ApprovalStatus model);
       List<Inventory_ApprovalStatus> GetInventoryApprovalStatus();
    }
}
