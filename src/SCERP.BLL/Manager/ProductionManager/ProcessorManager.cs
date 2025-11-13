using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.DAL.Repository.Planning;
using SCERP.DAL.Repository.ProductionRepository;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class ProcessorManager : IProcessorManager
    {
        private readonly IProcessorRepository _processorRepository;
        private readonly IProcessRepository _processRepository;
        private readonly string _compId;
        public ProcessorManager(IProcessorRepository processorRepository,IProcessRepository processRepository)
        {
            _processRepository = processRepository;
            _compId = PortalContext.CurrentUser.CompId;
            _processorRepository = processorRepository;
        }
        public List<PROD_Processor> GetProcessorLsit()
        {
            return _processorRepository.Filter(x => x.CompId == _compId).ToList();
        }

        public List<VProcessor> GetProcessorByPaging(PROD_Processor model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            IQueryable<VProcessor> processors = _processorRepository.GetVProcessor(x => x.CompId == _compId && (x.ProcessRefId == model.ProcessRefId || model.ProcessRefId == null)
                    && ((x.ProcessorName.Contains(model.SearchString) || model.SearchString == null)
                    
                       || (x.ProcessRefId.Contains(model.SearchString) || model.SearchString == null)
                    || (x.ProcessName.Contains(model.SearchString) || model.SearchString == null)));

            totalRecords = processors.Count();
            switch (model.sort)
            {
                case "ProcessorName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            processors = processors
                                 .OrderByDescending(r => r.ProcessRefId).ThenByDescending(r => r.ProcessorName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            processors = processors
                                 .OrderBy(r => r.ProcessRefId).ThenBy(r => r.ProcessorName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "ProcessorRefId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            processors = processors
                                      .OrderByDescending(r => r.ProcessRefId).ThenByDescending(r => r.ProcessorRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            processors = processors

                                     .OrderBy(r => r.ProcessRefId).ThenBy(r => r.ProcessorRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "ProcessName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            processors = processors
                                .OrderByDescending(r => r.ProcessRefId).ThenByDescending(r => r.ProcessName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            processors = processors
                                .OrderBy(r => r.ProcessRefId).ThenBy(r => r.ProcessName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    processors = processors
                        .OrderBy(r => r.ProcessRefId).ThenBy(x => x.ProcessorRefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return processors.ToList();

        }

        public List<PLAN_Process> GetProcessList()
        {
            return _processRepository.Filter(x => x.CompId == _compId).ToList();
        }

        public int SaveProcessor(PROD_Processor processor)
        {
            processor.CompId = _compId;
            return _processorRepository.Save(processor);
        }

        public string GetProcessorNewRefId()
        {
            return _processorRepository.GetProcessorNewRefId(_compId);
        }

        public int EditProcessor(PROD_Processor model)
        {
            var processor = _processorRepository.FindOne(x => x.ProcessorId == model.ProcessorId);
            processor.ProcessRefId = model.ProcessRefId;
            processor.ProcessorName = model.ProcessorName;
            return _processorRepository.Edit(processor);
        }

        public bool CheckExistingProcessor(PROD_Processor model)
        {
            return _processorRepository.Exists(x => x.CompId == _compId && x.ProcessorId != model.ProcessorId && x.ProcessRefId == model.ProcessRefId && x.ProcessorName == model.ProcessorName);
        }

        public PROD_Processor GetProcessorById(int processorId)
        {
            return _processorRepository.FindOne(x => x.ProcessorId == processorId);
        }

        public int DeleteProcessor(string processorRefId)
        {
            return _processorRepository.Delete(x => x.ProcessorRefId == processorRefId && x.CompId == _compId);
        }

        public List<PROD_Processor> GetProcessorByProcessRefId(string processRefId, string compId)
        {
            return _processorRepository.Filter(x => x.CompId == compId && x.ProcessRefId == processRefId).ToList();
        }
    }
}
