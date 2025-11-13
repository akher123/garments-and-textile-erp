using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.DAL.Repository.Planning;
using SCERP.Model.Planning;

namespace SCERP.BLL.Manager.PlanningManager
{
    public class ProcessManager : IProcessManager
    {
        private readonly IProcessRepository _processRepository;
        private readonly string _compId ;
        public ProcessManager(IProcessRepository processRepository)
        {
            _compId = PortalContext.CurrentUser.CompId;
            _processRepository = processRepository;
        }
        public List<PLAN_Process> GetProcessByPaging(PLAN_Process model, out int totalRecords)
        {
         
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var processList = _processRepository.Filter(x =>x.CompId==_compId&&
              ((x.ProcessName.Trim().Replace(" ", string.Empty).ToLower().Contains(model.ProcessName.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.ProcessName))
                || (x.ProcessRefId.Trim().Replace(" ", string.Empty).ToLower().Contains(model.ProcessRefId.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.ProcessName))));
            totalRecords = processList.Count();
            switch (model.sort)
            {
                case "ProcessName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            processList = processList
                                .OrderByDescending(r => r.ProcessName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            processList = processList
                                .OrderBy(r => r.ProcessName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "ProcessRefId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            processList = processList
                                .OrderByDescending(r => r.ProcessRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            processList = processList
                                .OrderBy(r => r.ProcessRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    processList = processList
                        .OrderBy(r => r.ProcessRefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return processList.ToList();
        }

        public PLAN_Process GetProcessById(int processId)
        {
           return _processRepository.FindOne(x => x.ProcessId == processId);
        }

        public string GetNewProcessRefId()
        {
            return _processRepository.GetProcessById(_compId);
        }

        public int EditProcess(PLAN_Process model)
        {
            var process = _processRepository.FindOne(x => x.CompId == _compId && x.ProcessId == model.ProcessId);
            process.EditedBy = PortalContext.CurrentUser.UserId;
            process.CreatedDate = DateTime.Now;
            process.ProcessCode = model.ProcessCode;
            process.ProcessName = model.ProcessName;
            process.BufferDay = model.BufferDay;
           return _processRepository.Edit(process);

        }

        public int SaveProcess(PLAN_Process model)
        {

            model.CreatedBy = PortalContext.CurrentUser.UserId;
            model.CreatedDate = DateTime.Now;
            model.IsActive = true;
            model.CompId = _compId;
            return _processRepository.Save(model);
        }

        public bool CheckExistingProcess(PLAN_Process model)
        {
          return  _processRepository.Exists(
                x =>
                    x.CompId == _compId && x.ProcessId != model.ProcessId &&
                    x.ProcessName.Replace(" ", "").ToLower().Equals(model.ProcessName.Replace(" ", "").ToLower()));
        }

        public int DeleteProcess(string processRefId)
        {
           return _processRepository.Delete(x => x.CompId == _compId && x.ProcessRefId == processRefId);
        }

        public List<PLAN_Process> GetProcess()
        {
            return _processRepository.Filter(x => x.CompId == _compId).ToList();
        }
    }
}
