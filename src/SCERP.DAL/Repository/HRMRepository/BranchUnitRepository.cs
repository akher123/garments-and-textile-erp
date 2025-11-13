using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class BranchUnitRepository : Repository<BranchUnit>, IBranchUnitRepository
    {
        public BranchUnitRepository(SCERPDBContext context)
            : base(context)
        {
        }
        public List<BranchUnit> GetAllBranchUnit(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel,BranchUnit model)
        {
            IQueryable<BranchUnit> branchUnits = null;
            totalRecords = 0;

            try
            {
                branchUnits = Context.BranchUnits.Include(x => x.Branch).Include(x => x.Branch.Company)
                    .Include(x => x.Unit)
                    .Where(x => x.IsActive
                    && (x.BranchId == searchFieldModel.SearchByBranchId || searchFieldModel.SearchByBranchId == 0)
                    && (x.UnitId == searchFieldModel.SearchByUnitId || searchFieldModel.SearchByUnitId == 0)
                    && (x.Branch.CompanyId == searchFieldModel.SearchByCompanyId || searchFieldModel.SearchByCompanyId == 0));
                totalRecords = branchUnits.Count();
                switch (model.sort)
                {
                    case "Branch.Name":

                        switch (model.sortdir)
                        {
                            case "DESC":
                                branchUnits = branchUnits
                                    .OrderByDescending(r => r.Branch.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                 branchUnits = branchUnits
                                    .OrderBy(r => r.Branch.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    case "Unit.Name":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                branchUnits = branchUnits
                                    .OrderByDescending(r => r.Unit.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                branchUnits = branchUnits
                                    .OrderBy(r => r.Unit.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    default:
                        switch (model.sortdir)
                        {
                            case "DESC":
                                branchUnits = branchUnits
                                    .OrderByDescending(r => r.Branch.Company.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                branchUnits = branchUnits
                                    .OrderBy(r => r.Branch.Company.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                }

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
            return branchUnits.ToList();
        }


        public IEnumerable GetAllUnitsByCompanyId(int companyId)
        {
            IEnumerable units;
            try
            {
                units=Context.BranchUnits.Include(x=>x.Branch.Company).Include(x=>x.Unit).Where(x => x.IsActive == true && x.Branch.CompanyId == companyId)
                    .Select(x => new
                        {
                            BranchUnitId = x.BranchUnitId,
                            UnitName = x.Unit.Name
                        }).OrderBy(x=>x.UnitName).ToList();

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return units;
        }
    }
}
