using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Runtime.Remoting.Contexts;
using SCERP.DAL.IRepository.IHRMRepository;
//using SCERP.Model;
using System.Linq;
using SCERP.Model;
using SCERP.Common;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class PoliceStationRepository : Repository<PoliceStation>, IPoliceStationRepository
    {
        public PoliceStationRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public PoliceStation GetPoliceStationById(int policeStationId)
        {
            PoliceStation policeStation = null;
            try
            {
                policeStation =
                    Context.PoliceStations.Where(x => x.IsActive && x.Id == policeStationId)
                        .Include(x => x.Country)
                        .Include(x => x.District)
                        .FirstOrDefault();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return policeStation;
        }

        public override IQueryable<PoliceStation> All()
        {
            return Context.PoliceStations.Where(x => x.IsActive == true).OrderBy(r => r.Name);
        }

        public List<PoliceStation> GetAllPoliceStationsBySearchKey(string searchByPoliceStationName, int searchByDistrict,
            int searchByCountry)
        {
            List<PoliceStation> policeStations = null;

            try
            {
                policeStations = Context.PoliceStations.Where(
                    x =>
                        x.IsActive == true &&
                        ((x.Name.Replace(" ", "")
                            .ToLower()
                            .Contains(searchByPoliceStationName.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(searchByPoliceStationName))
                        && (x.DistrictId == searchByDistrict || searchByDistrict == 0)
                        && (x.CountryId == searchByCountry || searchByCountry == 0)).Include(x => x.District).Include(x => x.Country).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }
            return policeStations;
        }

        public List<PoliceStation> GetAllPoliceStationsByPaging(int startPage, int pageSize, out int totalRecords, PoliceStation policeStation)
        {
            IQueryable<PoliceStation> policeStations = null;

            try
            {
                var searchByPoliceStationName = policeStation.Name;
                var searchByDistrict = policeStation.DistrictId;
                var searchByCountry = policeStation.CountryId;

                policeStations = Context.PoliceStations.Include(x => x.Country)
                    .Include(x => x.District).Where(
                        x =>x.IsActive &&((x.Name.Replace(" ", "").ToLower().Contains(searchByPoliceStationName.Replace(" ", "").ToLower())) ||
                             String.IsNullOrEmpty(searchByPoliceStationName)) && (x.DistrictId == searchByDistrict || searchByDistrict == 0)
                            && (x.CountryId == searchByCountry || searchByCountry == 0));

                totalRecords = policeStations.Count();
                switch (policeStation.sort)
                {
                    case "District.Name":
                        switch (policeStation.sortdir)
                        {
                            case "DESC":
                                policeStations = policeStations
                                    .OrderByDescending(r => r.District.Name).ThenBy(x=>x.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                policeStations = policeStations
                                    .OrderBy(r => r.District.Name).ThenBy(x => x.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;

                    case "Country.CountryName":


                        switch (policeStation.sortdir)
                        {
                            case "DESC":
                                policeStations = policeStations
                                    .OrderByDescending(r => r.Country.CountryName)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                policeStations = policeStations
                                    .OrderBy(r => r.Country.CountryName)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    default:
                        policeStations = policeStations
                                      .OrderBy(r => r.Country.CountryName).ThenBy(x => x.District.Name).ThenBy(x => x.Name)
                                      .Skip(startPage * pageSize)
                                      .Take(pageSize);
                        break;
                }
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.Message);
            }

            return policeStations.ToList();
        }

        public List<PoliceStation> GetAllPoliceStations()
        {
            List<PoliceStation> policeStationList;
            try
            {
                 policeStationList = Context.PoliceStations.Where(x => x.IsActive == true).OrderBy(r => r.Name).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }
            return policeStationList;
        }

        public List<PoliceStation> GetPoliceStationsByDistrict(int? districtId)
        {
            List<PoliceStation> policeStationList = null;
            try
            {
                 policeStationList = Context.PoliceStations.Where(x => x.DistrictId == districtId && x.IsActive == true).OrderBy(r => r.Name).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }
            return policeStationList;
        }

    }
}
