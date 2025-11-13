using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SCERP.BLL.IManager.IProductionManager;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class MachingInterruptionManager : IMachingInterruptionManager
    {
        private readonly IMachingInterruptionRepository _machingInterruptionRepository;

        public MachingInterruptionManager(IMachingInterruptionRepository machingInterruptionRepository)
        {
            _machingInterruptionRepository = machingInterruptionRepository;
        }

        public List<SpProdMatchingInterruption> GetMachingInterruptionByDate(DateTime? interrupDate,string processRefId, string compId)
        {
            return _machingInterruptionRepository.GetMachingInterruptionByDate(interrupDate, processRefId, compId);
        }

        public int SaveMachingInterruption(PROD_MachingInterruption model)
        {
           PROD_MachingInterruption oldModel=  _machingInterruptionRepository.FindOne(x=>x.InterrupDate==model.InterrupDate&&x.ProcessRefId==model.ProcessRefId&&x.MachineId==model.MachineId&&x.CompId==model.CompId)??new PROD_MachingInterruption();
            oldModel.ProcessRefId = model.ProcessRefId;
            oldModel.Remarks = model.Remarks;
            oldModel.MachineId = model.MachineId;
            oldModel.InterrupDate = model.InterrupDate;
            oldModel.CreatedBy = model.CreatedBy;
            oldModel.CompId = model.CompId;
            return _machingInterruptionRepository.Save(oldModel);
        }
    }
}
