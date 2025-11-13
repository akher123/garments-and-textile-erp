using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface IInventorySubGroupManager
    {
        Inventory_SubGroup GetSubGroupById(int subGroupId);
        ResponsModel EditSubGroup(Inventory_SubGroup subGroup);
        ResponsModel SaveSubGroup(Inventory_SubGroup subGroup);

        List<Inventory_SubGroup> AutocompliteSubGroup
            (string subGroupName, int groupId, int branchId);

        List<Inventory_SubGroup> GetSubGroupByBranchId(int? groupId);
        List<Inventory_SubGroup> GetSubGroupByGroupId(int groupId);
    }
}
