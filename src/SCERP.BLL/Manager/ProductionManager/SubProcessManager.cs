using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class SubProcessManager : ISubProcessManager

    {
        private readonly IProcessRepository _processRepository;
        private readonly ISubProcessRepository _subProcessRepository;
        private readonly string _compId;
        public SubProcessManager(IProcessRepository processRepository,ISubProcessRepository subProcessRepository)
        {

            _processRepository = processRepository;
            _subProcessRepository = subProcessRepository;
            _compId = PortalContext.CurrentUser.CompId;
        }


        public List<PLAN_Process> GetProcessList()
        {
            return _processRepository.Filter(x => x.CompId == _compId).ToList();
        }

        public List<PROD_SubProcess> GetSubProcessLsit()
        {
            return _subProcessRepository.Filter(x => x.CompId == _compId).ToList();
        }

        public List<VSubProcess> GetSubProcessByPaging(PROD_SubProcess model, out int totalRecords)
        {

         
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
           
            IQueryable<VSubProcess> subProcesses = _subProcessRepository.GetVSubProcess(x => x.CompId == _compId
                    && ((x.SubProcessName.Contains(model.SearchString) || model.SearchString == null)
                    || (x.SubProcessRefId.Contains(model.SearchString) || model.SearchString == null)));
            totalRecords = subProcesses.Count();
            switch (model.sort)
            {
                case "SubProcessName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            subProcesses = subProcesses
                                 .OrderByDescending(r => r.ProcessRefId).ThenByDescending(r => r.SubProcessName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            subProcesses = subProcesses
                                 .OrderBy(r => r.ProcessRefId).ThenBy(r => r.SubProcessName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "ProcessName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            subProcesses = subProcesses
                                 .OrderByDescending(r => r.ProcessName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            subProcesses = subProcesses
                                 .OrderBy(r => r.ProcessName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "SubProcessRefId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            subProcesses = subProcesses
                                      .OrderByDescending(r => r.ProcessRefId).ThenByDescending(r => r.SubProcessRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            subProcesses = subProcesses

                                     .OrderBy(r => r.ProcessRefId).ThenBy(r => r.SubProcessRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
            
           
                    break;
                default:
                    subProcesses = subProcesses
                        .OrderBy(r => r.ProcessRefId).ThenBy(x => x.SubProcessRefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return subProcesses.ToList();
        }

        public int SaveSubProcess(PROD_SubProcess subprocess)
        {
            subprocess.CompId = _compId;
            subprocess.SubProcessRefId = GetSubProcessNewRefId();
            return _subProcessRepository.Save(subprocess);
        }

        public string GetSubProcessNewRefId()
        {
           
            const int totalWidth = 3;
            var maxRefId = _subProcessRepository.Filter(x => x.CompId == _compId).ToList().Max(x =>x.SubProcessRefId);
            return maxRefId.IncrementOne().PadZero(totalWidth);
        }

        public int EditSubProcess(PROD_SubProcess subprocess)
        {
            var subprocesses = _subProcessRepository.FindOne(x => x.SubProcessId == subprocess.SubProcessId);
            subprocesses.SubProcessRefId = subprocess.SubProcessRefId;
            subprocesses.SubProcessName = subprocess.SubProcessName;
            return _subProcessRepository.Edit(subprocesses);
        }

        public bool CheckExistingSubProcess(PROD_SubProcess model)
        {
            return _subProcessRepository.Exists(x => x.CompId == _compId && x.SubProcessId != model.SubProcessId && x.SubProcessRefId == model.SubProcessRefId && x.SubProcessName == model.SubProcessName);
        }

        public PROD_SubProcess GetSubProcessById(long subprocessId)
        {
            return _subProcessRepository.FindOne(x => x.SubProcessId == subprocessId);
        }

        public int DeleteSubProcess(string subprocessRefId)
        {
            return _subProcessRepository.Delete(x => x.SubProcessRefId == subprocessRefId && x.CompId == _compId);
        }

        public string GetSubProcessNameByRefId(string subProcessRefId)
        {
            return _subProcessRepository.FindOne(x => x.SubProcessRefId == subProcessRefId && x.CompId == _compId).SubProcessName;
        }
    }
}
