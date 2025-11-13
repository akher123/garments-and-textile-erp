using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface ISizeManager
    {
        Inventory_Size GetSizeById(int sizeId);
        ResponsModel EditSize(Inventory_Size model);
        ResponsModel SaveSize(Inventory_Size model);
        int DeleteSize(int sizeId);
        bool IsExistSizeTitle(Inventory_Size model);
        ResponsModel GetSizeListByPaging(Inventory_Size model, out int totalRecords);
        List<Inventory_Size> GetSizeList();
    }
}
