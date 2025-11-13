using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class PoliceStationManager : BaseManager, IPoliceStationManager
    {

        private readonly IPoliceStationRepository _policeStationRepository = null;

        public PoliceStationManager(SCERPDBContext context)
        {
            this._policeStationRepository = new PoliceStationRepository(context);
        }

        public List<PoliceStation> GetAllPoliceStationsByPaging(int startPage, int pageSize, PoliceStation policeStation, out int totalRecords)
        {
            List<PoliceStation> policeStations = null;
            try
            {
                policeStations = _policeStationRepository.GetAllPoliceStationsByPaging(startPage, pageSize, out totalRecords, policeStation).ToList();
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.InnerException.Message);
            }

            return policeStations;
        }

        public List<PoliceStation> GetAllPoliceStationsBySearchKey(string searchByPoliceStationName, int searchByDistrict,
            int searchByCountry)
        {
            List<PoliceStation> policeStations = null;

            try
            {
                policeStations = _policeStationRepository.GetAllPoliceStationsBySearchKey(searchByPoliceStationName, searchByDistrict, searchByCountry);
            }
            catch (Exception exception)
            {
               throw new Exception(exception.InnerException.Message);
            }

            return policeStations;
        }

        public PoliceStation GetPoliceStationById(int policeStationId)
        {
            PoliceStation policeStation = null;
            try
            {
                policeStation =
                    _policeStationRepository.GetPoliceStationById(policeStationId);

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return policeStation;
        }

        public List<PoliceStation> GetAllPoliceStations()
        {
            var policeStationList = new List<PoliceStation>();

            try
            {
                policeStationList = _policeStationRepository.GetAllPoliceStations();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }

            return policeStationList;
        }

        public List<PoliceStation> GetPoliceStationByDistrict(int? distictId)
        {
            var policeStations = new List<PoliceStation>();
            try
            {
                policeStations = _policeStationRepository.GetPoliceStationsByDistrict(distictId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }
            return policeStations;
        }

        public int SavePoliceStation(PoliceStation policeStation)
        {
            var savedpoliceStation = 0;
            try
            {
                policeStation.CreatedDate = DateTime.Now;
                policeStation.CreatedBy = PortalContext.CurrentUser.UserId;
                policeStation.IsActive = true;

                savedpoliceStation = _policeStationRepository.Save(policeStation);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }

            return savedpoliceStation;
        }

        public int EditPoliceStation(PoliceStation policeStation)
        {
            var editedPoliceStation = 0;
            try
            {
                policeStation.EditedDate = DateTime.Now;
                policeStation.EditedBy = PortalContext.CurrentUser.UserId;
                editedPoliceStation = _policeStationRepository.Edit(policeStation);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }

            return editedPoliceStation;
        }

        public int DeletePoliceStation(PoliceStation policeStation)
        {
            var deletedPoliceStation = 0;
            try
            {
                policeStation.EditedDate = DateTime.Now;
                policeStation.EditedBy = PortalContext.CurrentUser.UserId;
                policeStation.IsActive = false;
                deletedPoliceStation = _policeStationRepository.Edit(policeStation);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }

            return deletedPoliceStation;
        }


        public bool CheckExistingPoliceStation(PoliceStation policeStation)
        {
            bool isExist = false;
            try
            {
                isExist =
                    _policeStationRepository.Exists(
                        x =>
                            x.IsActive == true &&
                            x.Id != policeStation.Id &&
                            x.CountryId == policeStation.Country.Id &&
                            x.DistrictId == policeStation.District.Id &&
                            x.Name.Replace(" ", "").ToLower().Equals(policeStation.Name.Replace(" ", "").ToLower()));
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }
            return isExist;
        }
    }
}

