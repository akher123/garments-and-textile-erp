using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SCERP.Model;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
   public interface IStorePurchaseRequisitionRepository:IRepository<Inventory_StorePurchaseRequisition>
   {
       IQueryable<VStorePurchaseRequisition> GetStorePurchaseRequisitions(Expression<Func<VStorePurchaseRequisition, bool>> predicate);
       VStorePurchaseRequisition GetVStorePurchaseRequisitionById(int storePurchaseRequisitionId);
       IQueryable<Inventory_StorePurchaseRequisitionDetail> GetStorePurchaseRequisitionDetails(int storePurchaseRequisitionId);
       string GetNewRequisitionNo();
       List<VStorePurchaseRequisitionDetail> GetVStorePurchaseRequisitionDetails(int storePurchaseRequisitionId);
   }
}
