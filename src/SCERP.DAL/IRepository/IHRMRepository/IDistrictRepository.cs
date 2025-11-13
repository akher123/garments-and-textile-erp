using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IDistrictRepository : IRepository<District>
    {
        District GetDistrictById(int? id);
        List<District> GetAllDistricts();
        List<District> GetAllDistrictsByPaging(int startPage, int pageSize, out int totalRecords, District district);
        List<District> GetDistrictBySearchKey(int searchByCountry, string searchByDistrict);
        List<District> GetDistrictsByCountry(int? countryId);
    }
}
