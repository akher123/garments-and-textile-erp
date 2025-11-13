using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Planning;

namespace SCERP.DAL.IRepository.IPlanningRepository
{
    public interface IProcessSequenceRepository:IRepository<PLAN_ProcessSequence>
    {
        List<VProcessSequence> GetProcessSequence(string compId, string orderStyleRefId);
    }
}
