using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
   public interface IStyleShipmentDetail:IRepository<Inventory_StyleShipmentDetail>
    {
       List<VwInventoryStyleShipment> GetShipmentStyleRefIds(long styleShipmentId);
    }
}
