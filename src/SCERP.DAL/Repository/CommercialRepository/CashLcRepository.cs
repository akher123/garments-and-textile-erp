using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Model.CommercialModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.DAL.Repository.CommercialRepository
{
    public class CashLcRepository : Repository<CommCashLc>, ICashLcRepository
    {
        public CashLcRepository(SCERPDBContext context) : base(context)
        {
            
        }
    }
}
