using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface IFabricReturnManager
    {
        List<Inventory_FabricReturn> GetFabricReturnByProgramId(long programId);
        Inventory_FabricReturn GetFabricReturnById(long fabricReturnId);

        int EditFabricReturn(Inventory_FabricReturn fabricReturn);
        int SaveFabricReturn(Inventory_FabricReturn fabricReturn);
        int DeleteFabricById(long fabricReturnId);
    }
}
