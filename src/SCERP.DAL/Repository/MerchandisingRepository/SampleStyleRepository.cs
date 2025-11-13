using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
   public class SampleStyleRepository:Repository<OM_SampleStyle>,ISampleStyleRepository
    {
        public SampleStyleRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
