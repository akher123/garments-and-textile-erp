using SCERP.DAL.IRepository.IMarketingRepository;
using SCERP.Model.MarketingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.DAL.Repository.MarketingRepository
{
    public class MarketingInstituteRepository : Repository<MarketingInstitute>, IMarketingInstituteRepository
    {

         public MarketingInstituteRepository(SCERPDBContext context)
            : base(context)
        {
        }

        
    }
}
