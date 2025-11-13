using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.Model.Planning;

namespace SCERP.DAL.Repository.Planning
{
    public class WorkingDayRepository :Repository<PLAN_WorkingDay>, IWorkingDayRepository
    {
        public WorkingDayRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
