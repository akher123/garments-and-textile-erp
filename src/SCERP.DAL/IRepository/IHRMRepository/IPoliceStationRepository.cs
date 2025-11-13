using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IPoliceStationRepository : IRepository<PoliceStation>
    {
        List<PoliceStation> GetAllPoliceStationsByPaging(int startPage, int pageSize, out int totalRecords,
            PoliceStation policeStation);
        List<PoliceStation> GetAllPoliceStations();
        List<PoliceStation> GetPoliceStationsByDistrict(int? districtId);

        List<PoliceStation> GetAllPoliceStationsBySearchKey(string searchByPoliceStationName, int searchByDistrict,
            int searchByCountry);

        PoliceStation GetPoliceStationById(int policeStationId);
    }
}
