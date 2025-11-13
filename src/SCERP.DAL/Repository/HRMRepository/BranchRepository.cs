using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class BranchRepository : Repository<Branch>, IBranchRepository
    {
        public BranchRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public Branch GetBranchById(int? id)
        {
            return Context.Branches.Find(id);
        }
        public List<Branch> GetAllBranchesByPaging(int startPage, int pageSize, out int totalRecords, Branch branch)
        {
            List<Branch> branches;
            try
            {
                var searchByCompany = branch.CompanyId;
                var searchByBranchName = branch.Name;
                var query = Context.Branches.Where(
                    x =>
                        x.IsActive == true &&
                        ((x.Name.Replace(" ", "")
                            .ToLower()
                            .Contains(searchByBranchName.Replace(" ", "").ToLower())) ||
                         String.IsNullOrEmpty(searchByBranchName))
                        && (x.CompanyId == searchByCompany || searchByCompany == 0))
                    .Include(x=>x.District).Include(x=>x.PoliceStation).Include(x=>x.Company);

                totalRecords = query.Count();

                switch (branch.sort)
                {
                    case "Company.Name":

                        switch (branch.sortdir)
                        {
                            case "DESC":
                                branches = query
                                  .OrderByDescending(r => r.Company.Name)
                                  .Skip(startPage * pageSize)
                                  .Take(pageSize)
                                  .ToList();
                                break;
                            default:
                                branches = query
                                      .OrderBy(r => r.Company.Name)
                                      .Skip(startPage * pageSize)
                                      .Take(pageSize)
                                      .ToList();
                                break;
                        }
                        break;

                    default:
                        switch (branch.sortdir)
                        {
                            case "DESC":
                                branches = query
                                  .OrderByDescending(r => r.Name)
                                  .Skip(startPage * pageSize)
                                  .Take(pageSize)
                                  .ToList();
                                break;
                            default:
                                branches = query
                                      .OrderBy(r => r.Name)
                                      .Skip(startPage * pageSize)
                                      .Take(pageSize)
                                      .ToList();
                                break;
                        }
                        break;
                }
            }
            catch (Exception exception)
            {
               
                throw new Exception(exception.Message);
            }

            return branches;
        }


        public List<Branch> GetAllBranchesBySearchKey(string searchByBranchName, int searchByCompanyName)
        {
            var branches = new List<Branch>();

            try
            {
                branches = Context.Branches.Where(
                    x =>
                        x.IsActive == true &&
                        ((x.Name.Replace(" ", "")
                            .ToLower()
                            .Contains(searchByBranchName.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(searchByBranchName))
                        && (x.CompanyId == searchByCompanyName || searchByCompanyName == 0)).Include("District").Include("PoliceStation").Include("Company").ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return branches;
        }

        public List<Branch> GetAllBranchesByCompanyId(int companyId)
        {
            List<Branch> branches;
            try
            {
                branches = Context.Branches.Where(x => x.CompanyId == companyId && x.IsActive).OrderBy(r => r.Name).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return branches;
        }

    }
}
