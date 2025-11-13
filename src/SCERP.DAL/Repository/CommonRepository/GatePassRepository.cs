using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.Model.CommonModel;

namespace SCERP.DAL.Repository.CommonRepository
{
    public class GatePassRepository :Repository<GatePass>, IGatePassRepository
    {
        public GatePassRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
