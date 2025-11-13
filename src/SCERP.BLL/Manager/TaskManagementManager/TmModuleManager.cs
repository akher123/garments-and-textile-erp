using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.ITaskManagementManager;
using SCERP.Common;
using SCERP.DAL.IRepository.ITaskManagementRepository;
using SCERP.Model.TaskManagementModel;

namespace SCERP.BLL.Manager.TaskManagementManager
{
    public class TmModuleManager : ITmModuleManager
    {
        private readonly ITmModuleRepository _tmModuleRepository;
        public TmModuleManager(ITmModuleRepository tmModuleRepository)
        {
            _tmModuleRepository = tmModuleRepository;
        }
        public List<TmModule> GetAllModule()
        {
            return _tmModuleRepository.All().OrderBy(y=>y.ModuleName).ToList();
        }

        public List<TmModule> GetAllModuleByPaging(TmModule model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var moduleList =_tmModuleRepository.Filter(  x => (x.ModuleName.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString)));
            totalRecords = moduleList.Count();
            switch (model.sort)
            {
                case "ModuleName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            moduleList = moduleList
                                 .OrderByDescending(r => r.ModuleName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            moduleList = moduleList
                                 .OrderBy(r => r.ModuleName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    moduleList = moduleList
                        .OrderByDescending(r => r.ModuleId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return moduleList.ToList();
        }

        public int EditModule(TmModule model)
        {
            var module = _tmModuleRepository.FindOne(x => x.ModuleId == model.ModuleId);
            module.ModuleName = model.ModuleName;
            return _tmModuleRepository.Edit(module);
        }

        public int SaveModule(TmModule model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
            return _tmModuleRepository.Save(model);
        }

        public TmModule GetModuleByModuleId(int moduleId)
        {
            return _tmModuleRepository.FindOne(x => x.ModuleId == moduleId);
        }
        public int DeleleModule(int moduleId)
        {
            return _tmModuleRepository.Delete(x => x.ModuleId == moduleId);
        }

        public bool IsModuleNameExist(TmModule model)
        {
            return _tmModuleRepository.Exists(x => x.ModuleName == model.ModuleName.Trim());
        }
    }
}
