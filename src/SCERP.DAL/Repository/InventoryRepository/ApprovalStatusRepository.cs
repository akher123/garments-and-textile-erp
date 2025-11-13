using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.InventoryRepository
{
   public class InventoryApprovalStatusRepository:Repository<Inventory_ApprovalStatus>,IInventoryApprovalStatusRepository
    {
        public InventoryApprovalStatusRepository(SCERPDBContext context) : base(context)
        {
        }

       public IQueryable<Inventory_ApprovalStatus> GetInventoryApprovalStatusByPaging(Inventory_ApprovalStatus model)
       {
           return
             Context.Inventory_ApprovalStatus.Where(
                 x => x.IsActive && (x.StatusName.ToLower().Contains(model.StatusName) || String.IsNullOrEmpty(model.StatusName)));
       }
    }
}
