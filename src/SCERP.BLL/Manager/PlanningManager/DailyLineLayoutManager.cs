using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model;
using SCERP.Model.Planning;

namespace SCERP.BLL.Manager.PlanningManager
{
    public class DailyLineLayoutManager : IDailyLineLayoutManager
    {
        private readonly IRepository<PLAN_DailyLineLayout> _dailyLineLayoutRepository;
        private readonly IMachineRepository _machineRepository;
        private readonly IProcessorRepository _processorRepository;
        public DailyLineLayoutManager(IRepository<PLAN_DailyLineLayout> dailyLineLayoutRepository, IMachineRepository machineRepository, IProcessorRepository processorRepository)
        {
            _dailyLineLayoutRepository = dailyLineLayoutRepository;
            _machineRepository = machineRepository;
            _processorRepository = processorRepository;
        }

        public List<PLAN_DailyLineLayout> GetDailyLineLayout(string processRefId, DateTime? outputDate, string compId)
        {
            List<string> processorList = _processorRepository.Filter(x => x.ProcessRefId == processRefId).Select(x => x.ProcessorRefId).ToList();
            List<Production_Machine> linesList = _machineRepository.Filter(x => x.IsActive && processorList.Contains(x.ProcessorRefId)).ToList();
            List<PLAN_DailyLineLayout> exitlineLayouts = _dailyLineLayoutRepository.Filter(x => x.CompId == compId && x.OutputDate >= outputDate && x.OutputDate <= outputDate).ToList();
            List<PLAN_DailyLineLayout> lineLayouts = linesList.Select(x => new PLAN_DailyLineLayout()
                        {
                            CompId = compId,
                            Production_Machine = x,
                            LineId = x.MachineId,
                            PlanQty = exitlineLayouts.Any(y => y.LineId == x.MachineId) ? exitlineLayouts.Single(y => y.LineId == x.MachineId).PlanQty : 0,
                            OutputDate = outputDate.GetValueOrDefault(),
                            NumberOfMachine = exitlineLayouts.Any(y => y.LineId == x.MachineId) ? exitlineLayouts.Single(y => y.LineId == x.MachineId).NumberOfMachine : x.NoMachine.GetValueOrDefault(),
                            Remarks = exitlineLayouts.Any(y => y.LineId == x.MachineId) ? exitlineLayouts.Single(y => y.LineId == x.MachineId).Remarks : "",
                        }).ToList();
            return lineLayouts;
        }

        public int SaveDailyLineLayout(List<PLAN_DailyLineLayout> lineLayouts)
        {
            int saved = 0;
           // lineLayouts = lineLayouts.Where(x => x.NumberOfMachine > 0).ToList();
            DateTime outputDate = lineLayouts.First().OutputDate;
            string compId= lineLayouts.First().CompId;
            using (var trancate = new TransactionScope())
            {
                List<PLAN_DailyLineLayout> lineLayoutList = _dailyLineLayoutRepository.Filter(x => x.CompId == compId && x.OutputDate >= outputDate && x.OutputDate <= outputDate).ToList();
                if (lineLayoutList.Any())
                {
                    foreach (PLAN_DailyLineLayout dailyLine in lineLayoutList)
                    {
                        _dailyLineLayoutRepository.DeleteOne(dailyLine);
                    }
                }
                saved = _dailyLineLayoutRepository.SaveList(lineLayouts);
                trancate.Complete();
            }
            return saved;
        }
    }
}
