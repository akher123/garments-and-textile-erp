using System.Linq;
using SCERP.Model;
namespace SCERP.DAL.IRepository.IInventoryRepository
{
   public interface ISizeRepository:IRepository<Inventory_Size>
   {
       IQueryable<Inventory_Size> GetSizeListByPaging(Inventory_Size model);
   }
}
