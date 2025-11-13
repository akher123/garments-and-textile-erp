using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Linq;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IDistrictManager
    {
        List<District> GetAllDistrictsByPaging(int startPage, int pageSize, out int totalRecords, District district);

        List<District> GetAllDistricts();

        District GetDistrictById(int? id);

        int SaveDistrict(District district);

        int EditDistrict(District district);
        
        int DeleteDistrict(District district);

        bool CheckExistingDistrict(District district);

        List<District> GetDistrictBySearchKey(int searchByCountry, string searchByDistrict);

        List<District> GetDistrictsByCountry(int? countryId);
    }
}
