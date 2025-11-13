using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.Manager.InventoryManager
{
   public class MateraialReceivedDetailManager: IMateraialReceivedDetailManager
   {
       private readonly IMateraialReceivedDetailRepository _materaialReceivedDetailRepository;

       public MateraialReceivedDetailManager(IMateraialReceivedDetailRepository materaialReceivedDetailRepository)
       {
           _materaialReceivedDetailRepository = materaialReceivedDetailRepository;
       }

       public List<Inventory_MaterialReceivedDetail> GetMaterialReceivedDetailByMaterialReceivedId(long materialReceivedId, string compId)
       {
            return _materaialReceivedDetailRepository.Filter(x => x.CompId == compId && x.MaterialReceivedId == materialReceivedId).ToList();
        }
   }
}
