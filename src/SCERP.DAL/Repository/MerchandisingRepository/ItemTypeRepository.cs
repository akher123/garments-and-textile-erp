using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
   public class ItemTypeRepository:Repository<OM_ItemType>, IItemTypeRepository
    {
       public ItemTypeRepository(SCERPDBContext context) : base(context)
       {
       }
    }
}
