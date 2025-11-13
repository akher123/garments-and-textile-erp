using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.Model.Planning;

namespace SCERP.DAL.Repository.Planning
{
    public class ProcessSequenceDefaultRepository :Repository<PLAN_ProcessSequenceDefault>, IProcessSequenceDefaultRepository
    {
        public ProcessSequenceDefaultRepository(SCERPDBContext context) : base(context)
        {

        }
    }
}
