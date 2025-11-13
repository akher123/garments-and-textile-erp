using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface ICountryRepository:IRepository<Country>
    {
        Country GetCountryById(int? id);

        List<Country> GetAllCountries();

        List<Country> GetAllCountriesByPaging(int startPage, int pageSize, out int totalRecords, Country country);

    }
}
