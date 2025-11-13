using System;
using System.Collections.Generic;

using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class FabConsumptionRepository :Repository<OM_FabConsumption>, IFabConsumptionRepository
    {
        public FabConsumptionRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
