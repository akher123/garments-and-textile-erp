using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class AgentManager : IAgentManager
    {
        private readonly IAgentRepository _agentRepository;
        private readonly string _compId;
        private readonly IBuyerOrderRepository _buyerOrderRepository;
        public AgentManager(IBuyerOrderRepository buyerOrderRepository, IAgentRepository agentRepository)
        {
            _buyerOrderRepository = buyerOrderRepository;
            _agentRepository = agentRepository;
            _compId = PortalContext.CurrentUser.CompId;
        }

        public List<OM_Agent> GetAgents()
        {
            return _agentRepository.Filter(x => x.AType == "B" && x.CompId == _compId).OrderBy(x => x.AgentName).ToList();
        }

        public List<OM_Agent> GetShepAgents()
        {
            return _agentRepository.Filter(x => x.AType == "S" && x.CompId == _compId).OrderBy(x => x.AgentName).ToList();
        }

        public List<OM_Agent> GetAgentByPaging(OM_Agent model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var categories = _agentRepository.Filter(x => x.CompId == _compId
                && ((x.AgentName.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.AgentRefId.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))));
            totalRecords = categories.Count();
            switch (model.sort)
            {
                case "AgentName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            categories = categories
                                .OrderByDescending(r => r.AgentName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            categories = categories
                                .OrderBy(r => r.AgentName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "AgentRefId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            categories = categories
                                .OrderByDescending(r => r.AgentRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            categories = categories
                                .OrderBy(r => r.AgentRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    categories = categories
                        .OrderBy(r => r.AgentRefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return categories.ToList();
        }

        public OM_Agent GetAgentById(int agentId)
        {
            return _agentRepository.FindOne(x => x.AgentId == agentId && x.CompId == _compId);
        }

        public string GetNewAgentRefId()
        {
            return _agentRepository.GetNewAgentRefId(_compId);
        }

        public int EditAgent(OM_Agent model)
        {
            var agent = _agentRepository.FindOne(x => x.AgentId == model.AgentId && x.CompId == _compId);
            agent.AgentName = model.AgentName;
            agent.Address1 = model.Address1;
            agent.Address2 = model.Address2;
            agent.Address3 = model.Address3;
            agent.Address3 = model.Address3;
            agent.AType = model.AType;
            agent.CountryId = model.CountryId;
            agent.CityId = model.CityId;
            agent.Phone = model.Phone;
            agent.Fax = model.Fax;
            agent.EMail = model.EMail;
            return _agentRepository.Edit(agent);
        }

        public int SaveAgent(OM_Agent model)
        {

            bool isExist = _agentRepository.Exists(
             x =>
                 x.CompId == _compId &&
                 x.AgentName.Trim().Replace(" ", "").ToLower() == model.AgentName.Trim().Replace(" ", "").ToLower());
            if (!isExist)
            {
                model.CompId = _compId;
                model.AgentRefId = GetNewAgentRefId();
                return _agentRepository.Save(model);
            }
            else
            {
                throw new Exception(model.AgentName + " Buyer Name Already Exist !");
            }
       
        }

        public int DeleteAgent(string agentRefId)
        {
            var isUsesd = _buyerOrderRepository.Exists(x => x.AgentRefId == agentRefId && x.CompId == _compId);
            var deleted = 0;
            if (isUsesd)
            {
                deleted = -1;
            }
            else
            {
                deleted = _agentRepository.Delete(x => x.AgentRefId == agentRefId && x.CompId == _compId);
            }
            return deleted;
        }

        public bool CheckExistingAgent(OM_Agent model)
        {
         return   _agentRepository.Exists(
                x =>
                    x.CompId == _compId && x.AgentId != model.AgentId &&
                    x.AgentName.Replace(" ", "").ToLower().Equals(model.AgentName.Replace(" ", "").ToLower()));
        }
    }
}
