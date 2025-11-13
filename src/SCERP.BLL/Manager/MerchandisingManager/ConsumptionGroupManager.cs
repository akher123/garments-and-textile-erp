using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.Repository.MerchandisingRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class ConsumptionGroupManager : IConsumptionGroupManager
    {
        private readonly IConsumptionGroupRepository _consumptionGroupRepository;
        public ConsumptionGroupManager(IConsumptionGroupRepository consumptionGroupRepository)
        {
            _consumptionGroupRepository = consumptionGroupRepository;
        }

        public List<OM_ConsumptionGroup> GetConsumptionGroups()
        {
            return _consumptionGroupRepository.All().OrderByDescending(x=>x.ConsGroupId).ToList();
        }
    }
}
