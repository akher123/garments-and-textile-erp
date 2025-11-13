using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Contexts;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Linq;


namespace SCERP.DAL.Repository.HRMRepository
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public Country GetCountryById(int? id)
        {
            return Context.Countries.FirstOrDefault(x => x.Id == id);
        }


        public List<Country> GetAllCountriesByPaging(int startPage, int pageSize, out int totalRecords, Country country)
        {
            IQueryable<Country> countries;

            try
            {
                string searchKey = country.CountryName;
                countries = Context.Countries.Where(
                    x =>
                        x.IsActive == true &&
                        ((x.CountryName.Replace(" ", "")
                            .ToLower()
                            .Contains(searchKey.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(searchKey)));
                totalRecords = countries.Count();

                switch (country.sort)
                {
                    case "CountryCode":
                        switch (country.sortdir)
                        {
                            case "DESC":
                                countries = countries
                                    .OrderByDescending(r => r.CountryCode)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                countries = countries
                                   .OrderBy(r => r.CountryCode)
                                   .Skip(startPage * pageSize)
                                   .Take(pageSize);
                                break;
                        }
                        break;

                    default:
                        countries = countries = countries
                            .OrderBy(r => r.CountryName)
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

            return countries.ToList();
        }

        public List<Country> GetAllCountries()
        {
            return Context.Countries.Where(x => x.IsActive).OrderBy(x => x.CountryName).ToList();
        }
    }
}
