using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public Company GetCompanyById(int? id)
        {
            return Context.Companies.Find(id);
        }

        public List<Company> GetAllCompaniesByPaging(int startPage, int pageSize, out int totalRecords, Company company)
        {
            IQueryable<Company> companies = null;

            try
            {
                string searchByCompanyName = company.Name;
                companies = Context.Companies.Include(x => x.District).Include(x => x.PoliceStation).Where(
                    x =>
                        x.IsActive == true &&
                        ((x.Name.Replace(" ", "")
                            .ToLower()
                            .Contains(searchByCompanyName.Replace(" ", "").ToLower())) ||
                         String.IsNullOrEmpty(searchByCompanyName)));

                totalRecords = companies.Count();

                switch (company.sortdir)
                {
                    case "DESC":
                        companies = companies
                            .OrderByDescending(r => r.Name)
                            .Skip(startPage*pageSize)
                            .Take(pageSize);
                        break;
                    default:
                        companies = companies
                           .OrderBy(r => r.Name)
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

            return companies.ToList();
        }

        public List<Company> GetAllCompaniesBySearchKey(string searchByCompanyName)
        {
            List<Company> companies = null;

            try
            {
                companies = Context.Companies.Where(
                    x =>
                        x.IsActive == true &&
                        ((x.Name.Replace(" ", "")
                            .ToLower()
                            .Contains(searchByCompanyName.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(searchByCompanyName))).Include(x => x.District).Include(x => x.PoliceStation).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return companies;
        }
    }


}

