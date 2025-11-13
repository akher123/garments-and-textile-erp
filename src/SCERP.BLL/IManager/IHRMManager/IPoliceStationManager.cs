
using System.Collections.Generic;
using System.Linq;
using SCERP.Model;


namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IPoliceStationManager
    {

        List<PoliceStation> GetAllPoliceStationsByPaging(int startPage, int pageSize, PoliceStation policeStation,
            out int totalRecords);

        List<PoliceStation> GetAllPoliceStations();

        List<PoliceStation> GetPoliceStationByDistrict(int? districtId);

        int SavePoliceStation(PoliceStation policeStation);

        int EditPoliceStation(PoliceStation policeStation);

        int DeletePoliceStation(PoliceStation policeStation);

        List<PoliceStation> GetAllPoliceStationsBySearchKey(string searchByPoliceStationName, int searchByDistrict,
            int searchByCountry);

        bool CheckExistingPoliceStation(PoliceStation policeStation);

        PoliceStation GetPoliceStationById(int policeStationId);
    }
}
