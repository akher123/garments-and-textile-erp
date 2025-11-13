
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class ConsumptionSupplierManager : IConsumptionSupplierManager
    {
        private readonly IConsumptionSupplierRepository _consumptionSupplierRepository;
        public ConsumptionSupplierManager(IConsumptionSupplierRepository consumptionSupplierRepository)
        {
            _consumptionSupplierRepository = consumptionSupplierRepository;
        }

        public double GetAssignedQtyByConsumptionId(long consumptionId)
        {
            return
                _consumptionSupplierRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId&&x.ConsumptionId==consumptionId)
                    .ToList()
                    .Sum(x => x.Quantity);
        }

        public OM_ConsumptionSupplier GetConsumtionSupplierByConsumtionSupplierId(long consumptionId, int supplierId)
        {
            return
                _consumptionSupplierRepository.FindOne(
                    x => x.ConsumptionId == consumptionId && x.SupplierId == supplierId);
        }

        public int SaveConsSupplier(OM_ConsumptionSupplier consumptionSupplier)
        {
            return _consumptionSupplierRepository.Save(consumptionSupplier);
            
        }

        public List<OM_ConsumptionSupplier> GetConsSupplierList(string compId,long consumptionId)
        {
            return _consumptionSupplierRepository.GetWithInclude(x => x.CompId == compId&&x.ConsumptionId==consumptionId, "Mrc_SupplierCompany").ToList();
        }

        public int DeleteConsumptionSupplier(int consumptionSupplierId)
        {
            return _consumptionSupplierRepository.Delete(x => x.ConsumptionSupplierId == consumptionSupplierId);
        }
    }
}
