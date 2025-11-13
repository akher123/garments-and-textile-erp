using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using System.Linq;

namespace SCERP.BLL.Manager.HRMManager
{
    public class CountryManager : BaseManager, ICountryManager
    {
        private readonly ICountryRepository _countryRepository = null;

        public CountryManager(SCERPDBContext context)
        {
            _countryRepository = new CountryRepository(context);
        }

        public List<Country> GetAllCountriesByPaging(int startPage, int pageSize, out int totalRecords, Country country)
        {
            var countries = new List<Country>();
            try
            {
                countries = _countryRepository.GetAllCountriesByPaging(startPage, pageSize, out totalRecords, country).ToList();

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                totalRecords = 0;
            }

            return countries;
        }

        public List<Country> GetAllCountries()
        {
            List<Country> countries = null;

            try
            {
                countries = _countryRepository.Filter(x => x.IsActive).OrderBy(x => x.CountryName).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return countries;
        }

        public Country GetCountryById(int? id)
        {
            Country country = null;
            try
            {
                country = _countryRepository.GetCountryById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return country;
        }

        public bool CheckExistingCountry(Country country)
        {
            bool isExist = false;
            try
            {
                isExist =
                    _countryRepository.Exists(
                        x =>
                            x.IsActive == true &&
                            x.Id != country.Id &&
                            x.CountryName.Replace(" ", "").ToLower().Equals(country.CountryName.Replace(" ", "").ToLower()));
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return isExist;
        }

        public int SaveCountry(Country country)
        {
            var savedCountry = 0;
            try
            {
                country.CreatedDate = DateTime.Now;
                country.CreatedBy = PortalContext.CurrentUser.UserId;
                country.IsActive = true;
                savedCountry = _countryRepository.Save(country);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return savedCountry;
        }

        public int EditCountry(Country country)
        {
            var editedCountry = 0;
            try
            {
                country.EditedDate = DateTime.Now;
                country.EditedBy = PortalContext.CurrentUser.UserId;
                editedCountry = _countryRepository.Edit(country);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return editedCountry;
        }

        public int DeleteCountry(Country country)
        {
            var deletedCountry = 0;
            try
            {
                country.EditedDate = DateTime.Now;
                country.EditedBy = PortalContext.CurrentUser.UserId;
                country.IsActive = false;
                deletedCountry = _countryRepository.Edit(country);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }          
            return deletedCountry;
        }

        public List<Country> GetCountryBySearchKey(string searchKey)
        {
          
              return  _countryRepository .Filter(x => x.IsActive&& ((x.CountryName.Replace(" ", "") .ToLower().Contains(searchKey.Replace(" ", "")
                            .ToLower())) || String.IsNullOrEmpty(searchKey))).OrderBy(x=>x.CountryName).ToList();
        }

    }
}
