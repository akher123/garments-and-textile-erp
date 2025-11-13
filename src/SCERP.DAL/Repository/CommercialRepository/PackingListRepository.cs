using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Model.CommercialModel;

namespace SCERP.DAL.Repository.CommercialRepository
{
    public class PackingListRepository : Repository<CommPackingListDetail>, IPackingListRepository
    {
        public PackingListRepository(SCERPDBContext context)
            : base(context)
        {

        }
    }
}
