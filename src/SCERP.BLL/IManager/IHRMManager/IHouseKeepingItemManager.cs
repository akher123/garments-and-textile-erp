using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IHouseKeepingItemManager 
    {
        List<HouseKeepingItem> GetHouseKeepingItems
            (int pageIndex, string sort, string sortdir, string searchString, out int totalRecords);

        HouseKeepingItem GetHouseKeepingItemById(int houseKeepingItemId);
        string GetNewRefId();
        int EditHouseKeepingItem(HouseKeepingItem houseKeepingItem);
        int SaveHouseKeepingItem(HouseKeepingItem houseKeepingItem);
        int DeleteHouseKeepingItem(HouseKeepingItem hk);

        List<HouseKeepingItem> GetHouseKeepingItems(string compId);
    }
}
