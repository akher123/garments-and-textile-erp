using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class DownTimeCategoryRepository :Repository<PROD_DownTimeCategory>, IDownTimeCategoryRepository
    {
        public DownTimeCategoryRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
