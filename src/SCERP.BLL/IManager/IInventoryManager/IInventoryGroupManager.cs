using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface IInventoryGroupManager 
    {
        string GetMaxGroupCode();
        Inventory_Group GetGroupById(int groupId);
        ResponsModel EditGroup(Inventory_Group @group);
        List<Inventory_Group> AutocompliteGroup( string groupName);
        List<Inventory_Group> GetGroups();
        ResponsModel SaveGroup(Inventory_Group @group);

        List<Inventory_Group> GetGroups(string groupCode);
    }
}
