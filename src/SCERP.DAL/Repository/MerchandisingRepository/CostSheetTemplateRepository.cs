using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
   public class CostSheetTemplateRepository:Repository<OM_CostSheetTemplate>, ICostSheetTemplateRepository
    {
       public CostSheetTemplateRepository(SCERPDBContext context) : base(context)
       {
       }
    }
}
