using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Linq;


namespace SCERP.DAL.Repository.HRMRepository
{
    public class DistrictRepository : Repository<District>, IDistrictRepository
    {
        public DistrictRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public District GetDistrictById(int? id)
        {
            return Context.Districts.Include(x=>x.Country).FirstOrDefault(x => x.Id == id);
        }

        public List<District> GetAllDistrictsByPaging(int startPage, int pageSize, out int totalRecords, District district)
        {
            IQueryable<District> districts;

            try
            {
                var searchByCountry = district.CountryId;
                var searchByDistrict = district.Name;

                districts = Context.Districts.Include(x => x.Country).Where(
                    x => x.IsActive == true &&
                         ((x.Name.Replace(" ", "")
                             .ToLower()
                             .Contains(searchByDistrict.Replace(" ", "").ToLower())) ||
                          String.IsNullOrEmpty(searchByDistrict)) &&
                         ((x.Country.Id == searchByCountry || searchByCountry == 0)));

                totalRecords = districts.Count();

                switch (district.sort)
                {
                    case "Country.CountryName":

                        switch (district.sortdir)
                        {
                            case "DESC":
                                districts = districts
                                    .OrderByDescending(r => r.Country.CountryName).ThenBy(x=>x.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                
                                break;
                            default:
                                districts = districts
                                    .OrderBy(r => r.Country.CountryName).ThenBy(x => x.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                   
                                break;
                        }

                        break;
                    case "Name":

                        switch (district.sortdir)
                        {
                            case "DESC":
                                districts = districts
                                    .OrderByDescending(r => r.Name).ThenBy(x => x.Country.CountryName)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                districts = districts
                                    .OrderBy(r => r.Name).ThenBy(x => x.Country.CountryName)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }

                        break;
                   
                    default:
                        districts = districts
                                  .OrderBy(r => r.Country.CountryName).ThenBy(x => x.Name)
                                  .Skip(startPage * pageSize)
                                  .Take(pageSize);
                        break;
                 
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return districts.ToList();
        }

        public List<District> GetAllDistricts()
        {
            return Context.Districts.Where(x => x.IsActive == true).OrderBy(y => y.Name).ToList();
        }

        public List<District> GetDistrictBySearchKey(int searchByCountry, string searchByDistrict)
        {
            List<District> districts = null;

            try
            {
                districts = Context.Districts.Where(
                    x =>
                        x.IsActive == true &&
                        ((x.Name.Replace(" ", "")
                            .ToLower()
                            .Contains(searchByDistrict.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(searchByDistrict))
                        && (x.CountryId == searchByCountry || searchByCountry == 0)).Include(x => x.Country).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return districts;
        }

        public List<District> GetDistrictsByCountry(int? countryId)
        {
            var districts = new List<District>();
            try
            {
                districts = Context.Districts.Where(x => x.CountryId == countryId && x.IsActive == true).OrderBy(r => r.Name).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }
            return districts;
        }
    }
}
