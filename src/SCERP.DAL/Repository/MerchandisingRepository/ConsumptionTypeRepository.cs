using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class ConsumptionTypeRepository :Repository<OM_ConsumptionType>, IConsumptionTypeRepository
    {
        public ConsumptionTypeRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
