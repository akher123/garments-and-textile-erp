using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface IGenericNameManager
    {
        List<Inventory_GenericName> GetGenericNameByPaging
            (int pageIndex, string sort, string sortdir, string searchString, out int totalRecords);

        Inventory_GenericName GetGenericNameById(int genericNameId);
        int EditGenericName(Inventory_GenericName genericName);
        int SaveGenericName(Inventory_GenericName genericName);
        int DeleteGenericName(int genericNameId);
        List<Inventory_GenericName> GetAllGenericNames();
    }
}
