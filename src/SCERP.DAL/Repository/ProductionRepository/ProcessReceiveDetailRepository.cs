using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class ProcessReceiveDetailRepository : Repository<PROD_ProcessReceiveDetail>, IProcessReceiveDetailRepository
    {
        public ProcessReceiveDetailRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
