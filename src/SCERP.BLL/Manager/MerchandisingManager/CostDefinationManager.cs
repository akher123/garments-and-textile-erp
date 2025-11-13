using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.Repository.MerchandisingRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class CostDefinationManager : ICostDefinationManager
    {
        private readonly ICostDefinationRepository _costDefinationRepository;
        private readonly ICostOrdStyleRepository _costOrdStyleRepository;
        private readonly string _compId;
        public CostDefinationManager(ICostDefinationRepository costDefinationRepository, ICostOrdStyleRepository costOrdStyleRepository)
        {
            _costOrdStyleRepository = costOrdStyleRepository;
            _compId = PortalContext.CurrentUser.CompId;
            _costDefinationRepository = costDefinationRepository;
        }

        public List<OM_CostDefination> GetCostDefinationPaging(OM_CostDefination model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var costDefinations = _costDefinationRepository.Filter(x => x.CompId == _compId
                && ((x.CostName.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.CostGroup.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.CostRefId.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString)))).AsNoTracking();
            totalRecords = costDefinations.Count();
            switch (model.sort)
            {
                case "CostName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            costDefinations = costDefinations
                                .OrderByDescending(r => r.CostName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            costDefinations = costDefinations
                                .OrderBy(r => r.CostName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "CostRefId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            costDefinations = costDefinations
                                .OrderByDescending(r => r.CostRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            costDefinations = costDefinations
                                .OrderBy(r => r.CostRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
            
                default:
                    costDefinations = costDefinations
                        .OrderByDescending(r => r.CostDefinationId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return costDefinations.ToList();
        }

        public OM_CostDefination GetCostDefinationById(int costDefinationId)
        {
            return _costDefinationRepository.FindOne(x => x.CostDefinationId == costDefinationId);
        }

        public string GetNewCostRefId()
        {
        
           return _costDefinationRepository.GetNewCostRefId(_compId);
        }

        public int EditCostDefination(OM_CostDefination model)
        {
            var costDefination = _costDefinationRepository.FindOne(x => x.CostDefinationId == model.CostDefinationId && x.CompId == _compId);
            costDefination.CostName = model.CostName;
            costDefination.CostGroup = model.CostGroup;
            costDefination.CostRefId = model.CostRefId;
            return _costDefinationRepository.Edit(costDefination);
        }

        public int SaveCostDefination(OM_CostDefination model)
        {

            model.CompId = _compId;
            return _costDefinationRepository.Save(model);
        }

        public int DeleteCostDefination(string costRefId)
        {
            var isUsesd = _costOrdStyleRepository.Exists(x => x.CostRefId == costRefId && x.CompId == _compId);
            var deleted = 0;
            if (isUsesd)
            {
                deleted = -1;
            }
            else
            {
                deleted = _costDefinationRepository.Delete(x => x.CostRefId == costRefId && x.CompId == _compId);
            }
            return deleted;
        }

        public List<OM_CostDefination> GetCostDefination()
        {
            return _costDefinationRepository.Filter(x =>x.CompId == _compId).ToList();
        }

        public bool CheckExistingCostDefination(OM_CostDefination model)
        {
          return  _costDefinationRepository.Exists(
                x =>
                    x.CompId == _compId && x.CostDefinationId != model.CostDefinationId &&
                    x.CostName.Replace(" ", "").ToLower().Equals(model.CostName.Replace(" ", "").ToLower()));
        }

        public List<OM_CostDefination> GetCostDefinationByCostGroup(string costGroupId, string compId)
        {
            return _costDefinationRepository.Filter(x => x.CostGroup == costGroupId).ToList();
        }
    }
}
