using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class ItemModeRepository :Repository<OM_ItemMode>, IItemModeRepository
    {
        public ItemModeRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
