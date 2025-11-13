using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.CommonRepository
{
    public class StateRepository :Repository<State>, IStateRepository
    {
        public StateRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
