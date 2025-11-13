using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.DAL.Repository.CommonRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.CommonManager
{
    public class CityManager : ICityManager
    {
        private readonly ICityRepository _cityRepository;
        public CityManager(SCERPDBContext context)
        {
            _cityRepository=new CityRepository(context);
        }

        public List<City> GetCitiesByPaging(City model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var stateList = _cityRepository.All().Include(x => x.Country).Include(x => x.State).Where(x => (x.CityName.Trim().ToLower().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString)));
            totalRecords = stateList.Count();

            if (totalRecords > 0)
            {
                switch (model.sort)
                {
                    case "CityName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                stateList = stateList
                                    .OrderByDescending(r => r.CityName)
                                    .Skip(index * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                stateList = stateList
                                    .OrderBy(r => r.CityName)
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
                            .OrderBy(r => r.StateId)
                            .Skip(index * pageSize)
                            .Take(pageSize);
                        break;
                }
            }
            return stateList.ToList();
        }

        public City GetCityById(int cityId)
        {
            return _cityRepository.FindOne(x => x.CityId == cityId);
        }

        public int SaveCity(City model)
        {
            return _cityRepository.Save(model);
        }

        public int EditCity(City model)
        {
            City city = _cityRepository.FindOne(x => x.CityId == model.CityId);
            city.CityName = model.CityName;
            city.CountryId = model.CountryId;
            city.StateId = model.StateId;
            city.Latitude = model.Latitude;
            city.Longitude = model.Longitude;
            return _cityRepository.Edit(city);
        }

        public int DeleteCity(int cityId)
        {
            return _cityRepository.Delete(x => x.CityId == cityId);
        }

        public IEnumerable GetCityByCountry(int countryId)
        {
            return _cityRepository.Filter(x => x.CountryId == countryId).Select(x => new
            {
                CityId = x.CityId,
                CityName = x.CityName
            });
        }
    }
}
