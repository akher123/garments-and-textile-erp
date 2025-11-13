using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.IManager.IInventoryManager
{
   public interface IMateraialReceivedDetailManager 
    {
       List<Inventory_MaterialReceivedDetail> GetMaterialReceivedDetailByMaterialReceivedId(long materialReceivedId, string compId);
    }
}
