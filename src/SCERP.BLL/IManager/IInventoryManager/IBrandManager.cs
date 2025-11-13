using System.Collections.Generic;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface IBrandManager
    {
        ResponsModel GetBrandsByPaging(Inventory_Brand model, out int totalRecords);
        Inventory_Brand GetBrandById(int brandId);
        ResponsModel EditBrand(Inventory_Brand model);
        ResponsModel SaveBrand(Inventory_Brand model);
        ResponsModel DeleteBrand(int brandId);
        bool IsExistBrandName(Inventory_Brand model);

        List<Inventory_Brand> GetBrands();
    }
}
