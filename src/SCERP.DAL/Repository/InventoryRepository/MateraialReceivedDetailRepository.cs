using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.InventoryRepository
{
   public class MateraialReceivedDetailRepository:Repository<Inventory_MaterialReceivedDetail>, IMateraialReceivedDetailRepository
    {
       public MateraialReceivedDetailRepository(SCERPDBContext context) : base(context)
       {
       }
    }
}
