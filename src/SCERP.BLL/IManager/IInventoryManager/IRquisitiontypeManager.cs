using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface IRequisitiontypeManager
    {
        List<Inventory_RequsitionType> GetRquisitiontypesByPaging(Inventory_RequsitionType model, out int totalRecords);

        Inventory_RequsitionType GetRquisitiontypeById(int requisitionTypeId);
        ResponsModel EditRquisitiontype(Inventory_RequsitionType model);
        ResponsModel SaveRquisitiontype(Inventory_RequsitionType model);

        bool IsExistRquisitiontype(Inventory_RequsitionType model);
        int DeleteRequsitionType(int requisitionTypeId);
 
        List<Inventory_RequsitionType> GetRquisitiontypes();
    }
}
