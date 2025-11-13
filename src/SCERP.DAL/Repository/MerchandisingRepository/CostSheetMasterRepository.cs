using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
   public class CostSheetMasterRepository:Repository<OM_CostSheetMaster>, ICostSheetMasterRepository
    {
       public CostSheetMasterRepository(SCERPDBContext context) : base(context)
       {
       }
    }
}
