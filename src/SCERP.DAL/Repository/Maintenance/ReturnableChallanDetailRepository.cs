using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMaintenance;
using SCERP.Model.Maintenance;

namespace SCERP.DAL.Repository.Maintenance
{
    public class ReturnableChallanDetailRepository : Repository<Maintenance_ReturnableChallanDetail>, IReturnableChallanDetailRepository
   {
        public ReturnableChallanDetailRepository(SCERPDBContext context) : base(context)
        {
        }
   }
}
