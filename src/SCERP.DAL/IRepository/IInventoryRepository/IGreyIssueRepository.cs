using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
   public interface IGreyIssueRepository:IRepository<Inventory_GreyIssue>
   {
       List<KnittingOrderDelivery> GetKnittingOrderDelivery(int programId,long greyIssueId);
       int DeleteGreyIssue(long greyIssueId);
       DataTable GetGeryIssuePartyChallan(long greyIssueId);
   }
}
