using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class ComponentManager : IComponentManager
    {
        private readonly IComponentRepository _componentRepository;
        private readonly string _compId;
        private readonly ICompConsumptionRepository _compConsumptionRepository;
        public ComponentManager(ICompConsumptionRepository compConsumptionRepositor,IComponentRepository componentRepository)
        {
            _compConsumptionRepository = compConsumptionRepositor;
            _compId = PortalContext.CurrentUser.CompId;
            _componentRepository = componentRepository;
        }

        public List<OM_Component> GetComponentByPaging(OM_Component model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var componentLsit = _componentRepository.Filter(x => x.CompId == _compId
                && ((x.ComponentName.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.ComponentRefId.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
               || (x.Pannel.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                ));
            totalRecords = componentLsit.Count();
            switch (model.sort)
            {
                case "ComponentName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            componentLsit = componentLsit
                                .OrderByDescending(r => r.ComponentName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            componentLsit = componentLsit
                                .OrderBy(r => r.ComponentName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "ComponentRefId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            componentLsit = componentLsit
                                .OrderByDescending(r => r.ComponentRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            componentLsit = componentLsit
                                .OrderBy(r => r.ComponentRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    componentLsit = componentLsit
                        .OrderBy(r => r.ComponentRefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return componentLsit.ToList();
        }

        public OM_Component GetComponentById(long componentId)
        {
            return _componentRepository.FindOne(x => x.ComponentId == componentId && x.CompId == _compId);
        }

        public string GetComponentRefId()
        {
            return _componentRepository.GetComponentRefId(_compId);
        }
        public int EditComponent(OM_Component model)
        {
            var component = _componentRepository.FindOne(x => x.ComponentId == model.ComponentId && x.CompId == _compId);
            component.ComponentName = model.ComponentName;
            component.Pannel = model.Pannel;
            component.CompType = model.CompType;
            return _componentRepository.Edit(component);
        }
        public int SaveComponent(OM_Component model)
        {
            model.CompId = _compId;
            return _componentRepository.Save(model);
        }
        public int DeleteComponent(string componentRefId)
        {
            var isUsesd = _compConsumptionRepository.Exists(x => x.ComponentRefId == componentRefId && x.CompId == _compId);
            var deleted = 0;
            if (isUsesd)
            {
                deleted = -1;
            }
            else
            {
                deleted = _componentRepository.Delete(x => x.ComponentRefId == componentRefId && x.CompId == _compId);
            }
            return deleted;
        }

        public List<OM_Component> GetComponents()
        {
            return _componentRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId).OrderBy(x=>x.ComponentName).ToList();
          
        }

        public bool CheckExistingComponent(OM_Component model)
        {
          return  _componentRepository.Exists(
                x => x.CompId == _compId && x.ComponentId != model.ComponentId && x.ComponentName == model.ComponentName);
        }

        public object AutoCompliteComponent(string searchString)
        {
            return _componentRepository.Filter(x => x.ComponentName.Trim().Replace(" ", String.Empty).StartsWith(searchString.Replace(" ", String.Empty))).Take(10).OrderBy(x => x.ComponentName);
    }
    }
}
