using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface IItemModeManager
    {
       List<OM_ItemMode> GetItemModes();
       List<OM_ItemMode> GetItemModePaging(OM_ItemMode model, out int totalRecords);
       OM_ItemMode GetItemModeById(long styleId);

       int EditItemMode(OM_ItemMode model);
       int SaveItemMode(OM_ItemMode model);

       int DeleteItemMode(string p);
    }
}
