using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.Model.CommonModel;

namespace SCERP.DAL.Repository.CommonRepository
{
    public class PartyReposity : Repository<Party>, IPartyReposity
   {
        public PartyReposity(SCERPDBContext context) : base(context)
        {

        }

       
    }
}
