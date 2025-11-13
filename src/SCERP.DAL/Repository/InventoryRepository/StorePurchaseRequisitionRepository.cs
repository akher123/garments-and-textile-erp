using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class StorePurchaseRequisitionRepository :Repository<Inventory_StorePurchaseRequisition>, IStorePurchaseRequisitionRepository
    {
        public StorePurchaseRequisitionRepository(SCERPDBContext context) : base(context)
        {

        }
        public IQueryable<VStorePurchaseRequisition> GetStorePurchaseRequisitions(Expression<Func<VStorePurchaseRequisition, bool>> predicate)
        {
            return Context.VStorePurchaseRequisitions.Where(predicate);
        }

        public VStorePurchaseRequisition GetVStorePurchaseRequisitionById(int storePurchaseRequisitionId)
        {
            return
                Context.VStorePurchaseRequisitions.FirstOrDefault(
                    x => x.StorePurchaseRequisitionId == storePurchaseRequisitionId);
        }

        public IQueryable<Inventory_StorePurchaseRequisitionDetail> GetStorePurchaseRequisitionDetails(int storePurchaseRequisitionId)
        {
            return
                Context.Inventory_StorePurchaseRequisitionDetail.Include(x => x.Inventory_Item)
                    .Include(x => x.Inventory_Brand)
                    .Include(x => x.Inventory_Size)
                       
                    .Include(x => x.Country)
                   
                    .Include(x => x.Inventory_ApprovalStatus)
                    .Where(x => x.IsActive && x.StorePurchaseRequisitionId == storePurchaseRequisitionId);
        }

      
        public string GetNewRequisitionNo()
        {
            var reqNo = Context.Database.SqlQuery<string>(
                    "Select  substring(MAX(RequisitionNo),4,8 )from Inventory_StorePurchaseRequisition")
                    .SingleOrDefault() ?? "0";
            var maxNumericValue = Convert.ToInt32(reqNo);
            var irNo = "SPR" + GetRefNumber(maxNumericValue, 5);
            return irNo;
        }

        private string GetRefNumber(int maxNumericValue, int length)
        {
            var refNumber = Convert.ToString(maxNumericValue + 1);
            while (refNumber.Length != length)
            {
                refNumber = "0" + refNumber;
            }
            return refNumber;
        }
        public List<VStorePurchaseRequisitionDetail> GetVStorePurchaseRequisitionDetails(int storePurchaseRequisitionId)
        {
          return  Context.VStorePurchaseRequisitionDetails.Where(
                x => x.StorePurchaseRequisitionId == storePurchaseRequisitionId).ToList();
        }
    }
}
