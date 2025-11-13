using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.Repository.MerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class ConsumptionTypeManager : IConsumptionTypeManager
    {
        private readonly IConsumptionTypeRepository _consumptionTypeRepository;
        public ConsumptionTypeManager(IConsumptionTypeRepository consumptionTypeRepository)
        {
            _consumptionTypeRepository = consumptionTypeRepository;
        }

        public List<OM_ConsumptionType> GetConsumptionTypes()
        {
            return _consumptionTypeRepository.Filter(x =>!String.IsNullOrEmpty(x.ConsTypeRefId)).ToList();
        }
    }
}
