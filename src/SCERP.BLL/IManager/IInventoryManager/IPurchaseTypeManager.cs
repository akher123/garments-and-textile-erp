using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface IPurchaseTypeManager
    {
        List<Inventory_PurchaseType> GetPurchaseTypesByPaging(Inventory_PurchaseType model, out int totalRecords);
        Inventory_PurchaseType GetPurchaseTypeById(int purchaseTypeId);
        int EditPurchaseType(Inventory_PurchaseType model);
        int SavePurchaseType(Inventory_PurchaseType model);
        int DeletePurchaseType(int purchaseTypeId);
        bool IsExistPurchaseType(Inventory_PurchaseType model);
        List<Inventory_PurchaseType> GetPurchaseTypes();
    }
}
