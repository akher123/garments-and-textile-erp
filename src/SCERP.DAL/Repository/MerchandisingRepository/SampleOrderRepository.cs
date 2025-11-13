using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
   public class SampleOrderRepository:Repository<OM_SampleOrder>,ISampleOrderRepository
    {
        public SampleOrderRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
