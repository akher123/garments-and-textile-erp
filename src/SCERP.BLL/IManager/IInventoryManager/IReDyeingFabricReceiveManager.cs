using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface IReDyeingFabricReceiveManager
    {
        List<Inventory_ReDyeingFabricReceive> GetReDyeingFabReceivesByPaging
            (string compId, string modelSearchString, int modelPageIndex, out int totalRecords);

        string GetNewRefNo(string currentUserCompId);
        int EditRedyeingFabricReceive(Inventory_ReDyeingFabricReceive model);
        int SaveFinishFabricIssue(Inventory_ReDyeingFabricReceive model);
        Inventory_ReDyeingFabricReceive GetReDyeingFabricReceiveById(long reDyeingFabricReceiveId);
        List<VwReDyeingFabricReceiveDetail> GetVwReDyeingFabricReceiveDetailById(long reDyeingFabricReceiveId);
        int DeleteRedyeingFabricReceive(long reDyeingFabricReceiveId);
    }
}
