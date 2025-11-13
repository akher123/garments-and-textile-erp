using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface IGroupChallanManager
    {
        List<Inventory_GroupChallan> GetAllGroupChallanByPaging
                (int pageIndex, string sort, string sortdir, out int totalRecords, string searchString);

        Inventory_GroupChallan GetGroupChallanById(int groupChallanId, string compId);
        string GetNewRefId();
        int EditGroupChallan(Inventory_GroupChallan groupChallan);
        int DeleteGroupChallan(int groupChallanId);
        int SaveGroupChallan(Inventory_GroupChallan groupChallan);
    }
}
