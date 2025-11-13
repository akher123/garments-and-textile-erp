using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class ConsumptionSupplierRepository :Repository<OM_ConsumptionSupplier>, IConsumptionSupplierRepository
    {
        public ConsumptionSupplierRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
