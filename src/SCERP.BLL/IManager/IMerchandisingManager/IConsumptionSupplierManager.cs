using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface IConsumptionSupplierManager
    {
       double GetAssignedQtyByConsumptionId(long consumptionId);
       OM_ConsumptionSupplier GetConsumtionSupplierByConsumtionSupplierId(long consumptionId, int supplierId);
       int SaveConsSupplier(OM_ConsumptionSupplier consumptionSupplier);
       List<OM_ConsumptionSupplier> GetConsSupplierList(string compId,long consumptionId);
       int DeleteConsumptionSupplier(int consumptionSupplierId);
    }
}
