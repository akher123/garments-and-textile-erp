using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class SewingOutputProcessDetailRepository : Repository<PROD_SewingOutPutProcessDetail>, ISewingOutputProcessDetailRepository
    {
        public SewingOutputProcessDetailRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
