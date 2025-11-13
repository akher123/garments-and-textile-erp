using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.DAL.Repository.CommonRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.CommonManager
{
    public class StateManager : IStateManager
    {
        private readonly IStateRepository _stateRepository;
        public StateManager(SCERPDBContext context)
        {
            _stateRepository=new StateRepository(context);
        }

        public List<State> GetStatesByPaging(State model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var stateList = _stateRepository.All().Include(x=>x.Country).Where(x => (x.StateName.Trim().ToLower().Contains(model.StateName.Trim().ToLower()) || String.IsNullOrEmpty(model.StateName.Trim())));
            totalRecords = stateList.Count();
            if (totalRecords > 0)
            {
                switch (model.sort)
                {
                    case "StateName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                stateList = stateList
                                    .OrderByDescending(r => r.StateName)
                                    .Skip(index * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                stateList = stateList
                                    .OrderBy(r => r.StateName)
                                    .Skip(index * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    case "CountryName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                stateList = stateList
                                    .OrderByDescending(r => r.Country.CountryName)
                                    .Skip(index * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                stateList = stateList
                                    .OrderBy(r => r.Country.CountryName)
                                    .Skip(index * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    default:
                        stateList = stateList
                            .OrderBy(r => r.StateName)
                            .Skip(index * pageSize)
                            .Take(pageSize);
                        break;
                }
            }
            return stateList.ToList();
        }

        public State GetStateById(int stateId)
        {
          return  _stateRepository.FindOne(x => x.StateId == stateId);
        }

        public int EditState(State model)
        {
            var state = _stateRepository.FindOne(x => x.StateId == model.StateId);
            state.StateName = model.StateName;
            state.CountryId = model.CountryId;
            state.Latitude = model.Latitude;
            state.Longitude = model.Longitude;
            return _stateRepository.Edit(state);

        }

        public int SaveState(State model)
        {
            return _stateRepository.Save(model);
        }

        public int DeleteState(int stateId)
        {
            return _stateRepository.Delete(x=>x.StateId==stateId);
        }

        public List<State> GetStates()
        {
            return _stateRepository.All().ToList();
        }

        public object GetStateByCountry(int countryId)
        {
            return _stateRepository.Filter(x => x.CountryId == countryId).Select(x=>new
            {
                StateName=x.StateName,
                StateId=x.StateId
            }).ToList();
        }
    }
}
