using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
   public class CostSheetDetailRepository:Repository<OM_CostSheetDetail>, ICostSheetDetailRepository
    {
       public CostSheetDetailRepository(SCERPDBContext context) : base(context)
       {
       }

     
    }
}
