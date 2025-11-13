using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
    public interface IMaterialReceiveAgainstPoDetailRepository:IRepository<Inventory_MaterialReceiveAgainstPoDetail>
    {
        VwMaterialReceiveAgainstPoDetail GetMaterialReceiveAgainstPoDetail(int itemId, long sourceId, string colorRefId, string compId);
    }
}
