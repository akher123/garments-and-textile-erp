using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.ICommonManager
{
    public interface ICityManager
    {
        List<City> GetCitiesByPaging(City model, out int totalRecords);
        City GetCityById(int cityId);
        int SaveCity(City model);
        int EditCity(City model);
        int DeleteCity(int cityId);
        IEnumerable GetCityByCountry(int countryId);
    }
}
