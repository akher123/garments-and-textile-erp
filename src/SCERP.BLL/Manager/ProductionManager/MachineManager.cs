using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class MachineManager : IMachineManager
    {
     
        private readonly IMachineRepository _machineRepository;
        private readonly IProcessorRepository _processorRepository;
        private readonly string _compId;
        public MachineManager(IMachineRepository machineRepository, IProcessorRepository processorRepository)
        {
            _compId = PortalContext.CurrentUser.CompId;
            _machineRepository = machineRepository;
            _processorRepository = processorRepository;
        }

        public List<Production_Machine> GetMachines(string processRefId)
        {
            var machines = from m in _machineRepository.Filter(x => x.IsActive)
                           join processor in _processorRepository.Filter(x => x.ProcessRefId == processRefId && x.CompId == _compId) on m.ProcessorRefId equals processor.ProcessorRefId
                select m;
            return machines.ToList();
        }


        public List<VMachine> GetMachineListByPaging(Production_Machine model, out int totalRecords)
        {
          
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;

            var vMachines = _machineRepository.GetVMachineList(x => x.IsActive && x.CompId == _compId && (x.ProcessorRefId == model.ProcessorRefId || String.IsNullOrEmpty(model.ProcessorRefId)) && (x.ProcessRefId == model.ProcessRefId || String.IsNullOrEmpty(model.ProcessRefId)) && (x.Name.Trim().ToLower().Contains(model.Name.Trim().ToLower()) || String.IsNullOrEmpty(model.Name.Trim())));
            totalRecords = vMachines.Count();

            if (totalRecords > 0)
            {
                switch (model.sort)
                {
                    case "Name":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                vMachines = vMachines
                                    .OrderByDescending(r => r.Name).ThenBy(x=>x.ProcessorRefId)
                                    .Skip(index * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                vMachines = vMachines
                                    .OrderBy(r => r.Name).ThenBy(x => x.ProcessorRefId)
                                    .Skip(index * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    default:
                        vMachines = vMachines
                            .OrderBy(r => r.MachineId).ThenBy(x => x.ProcessorRefId)
                            .Skip(index * pageSize)
                            .Take(pageSize);
                        break;
                }
            }
            return vMachines.ToList();
        }

        public Production_Machine GetMachineById(int machineId)
        {
            return _machineRepository.FindOne(x => x.IsActive && x.MachineId == machineId&&x.CompId==_compId);
        }

        public int EditMachine(Production_Machine model)
        {
            
            var machine = _machineRepository.FindOne(x => x.IsActive && x.MachineId == model.MachineId);
        
            machine.Name = model.Name;
            machine.EfficiencyPer = model.EfficiencyPer;
            machine.IdelPer = model.IdelPer;
            machine.RatedCapacity = GetRatedCapacity(model);
            machine.NoMachine = model.NoMachine;
            machine.ProcessorRefId = model.ProcessorRefId;
            machine.Description = model.Description;
            machine.EditedBy = PortalContext.CurrentUser.UserId;
            machine.EditedDate = DateTime.Now;
            return _machineRepository.Edit(machine);
        }

        public int SaveMachine(Production_Machine model)
        {
        
            model.CompId = _compId;
            model.CreatedBy = PortalContext.CurrentUser.UserId;
            model.CreatedDate = DateTime.Now;
            model.IsActive = true;
            model.RatedCapacity = GetRatedCapacity(model);
            return _machineRepository.Save(model);
        }

        public int DeleteColor(int machineId)
        {
            var machine = _machineRepository.FindOne(x => x.IsActive && x.MachineId == machineId);
            machine.EditedBy = PortalContext.CurrentUser.UserId;
            machine.EditedDate = DateTime.Now;
            machine.IsActive = false;
            return _machineRepository.Edit(machine);
        }

        public string GetNewMachineRefId()
        {
            return _machineRepository.GetNewMachineRefId(_compId);
        }

        public List<Production_Machine> AutocompliteMechineByProcessor(string processorRefId)
        {
            return
                _machineRepository.Filter(x => x.ProcessorRefId == processorRefId && x.CompId == _compId).ToList();
        }

        public List<Production_Machine> GetLines()
        {
          var linerProcessors = new[] { "003", "004", "007" }; //Sewing Unit1=03,Sewing Unit2=04,Sewing Unit2=07,
          return _machineRepository.Filter(x => x.CompId == _compId && x.IsActive==true && linerProcessors.Contains(x.ProcessorRefId)).Distinct().ToList();
        }

        public List<Production_Machine> GetMachines()
        {
            return _machineRepository.Filter(x => x.IsActive&&x.ProcessorRefId=="002").OrderBy(y => y.Name).ToList();
        }


        public decimal? GetRatedCapacity(Production_Machine machine)
        { 
              const int measurementMin = 480; // For 8 Hours
              return machine.NoMachine * measurementMin * machine.EfficiencyPer * .01M * (100 - machine.IdelPer) * .01M;
        }
    }
}
