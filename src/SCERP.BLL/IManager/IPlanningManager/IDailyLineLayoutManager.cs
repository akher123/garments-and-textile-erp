using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository;
using SCERP.Model.Planning;

namespace SCERP.BLL.IManager.IPlanningManager
{
    public interface IDailyLineLayoutManager
    {
        List<PLAN_DailyLineLayout> GetDailyLineLayout(string processorRefId, DateTime? outputDate, string compId);
        int SaveDailyLineLayout(List<PLAN_DailyLineLayout> lineLayouts);
    }
}
