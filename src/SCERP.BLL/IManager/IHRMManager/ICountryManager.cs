using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Linq;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface ICountryManager
    {
        List<Country> GetAllCountriesByPaging(int startPage, int pageSize, out int totalRecords, Country country);

        List<Country> GetAllCountries();

        Country GetCountryById(int? id);

        int SaveCountry(Country country);

        int EditCountry(Country country);

        int DeleteCountry(Country country);

        bool CheckExistingCountry(Country country);

        List<Country> GetCountryBySearchKey(string searchKey);
    }
}
