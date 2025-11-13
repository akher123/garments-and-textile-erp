using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface IItemTypeManager
    {
       OM_ItemType GetItemTypeById(int itemTypeId, string compId);
       List<OM_ItemType> GetAllItemByPagingType(string title, string compId); 
       bool IsItemTypeExist(OM_ItemType model); 
       int EditItemType(OM_ItemType model);
       int SaveItemType(OM_ItemType model);
       int DeleteItemType(int itemTypeId);
       List<OM_ItemType> GetAllItemType(string compId);
    }
}
