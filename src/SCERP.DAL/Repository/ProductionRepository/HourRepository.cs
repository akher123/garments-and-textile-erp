using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class HourRepository : Repository<PROD_Hour>, IHourRepository
    {
        public HourRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
