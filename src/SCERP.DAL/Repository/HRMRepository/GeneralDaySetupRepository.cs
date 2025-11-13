using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class GeneralDaySetupRepository : Repository<GeneralDaySetup>, IGeneralDaySetupRepository
    {
        public GeneralDaySetupRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public List<GeneralDaySetup> GetGeneralDaySetup(int startPage, int pageSize, out int totalRecords, GeneralDaySetup model, SearchFieldModel searchFieldModel)
        {

            IQueryable<GeneralDaySetup> generalDaySetups;

            try
            {
                 generalDaySetups = Context.GeneralDaySetups.Include(x => x.BranchUnitDepartment.BranchUnit.Branch.Company)
                    .Include(x => x.BranchUnitDepartment.BranchUnit.Unit)
                    .Include(x => x.BranchUnitDepartment.UnitDepartment.Department)
                    .Include(x => x.BranchUnitDepartment.UnitDepartment.Unit)
                    .Include(x => x.BranchUnitDepartment.UnitDepartment.Department)
                    .Where(
                        x =>
                            x.IsActive &&
                            (x.BranchUnitDepartment.BranchUnit.Branch.CompanyId == searchFieldModel.SearchByCompanyId || searchFieldModel.SearchByCompanyId == 0)
                            && (x.BranchUnitDepartment.BranchUnit.BranchId == searchFieldModel.SearchByBranchId || searchFieldModel.SearchByBranchId == 0)
                            && (x.BranchUnitDepartment.BranchUnitId == searchFieldModel.SearchByBranchUnitId || searchFieldModel.SearchByBranchUnitId == 0)
                            && (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId ||searchFieldModel.SearchByBranchUnitDepartmentId == 0));

                totalRecords = generalDaySetups.Count();

                switch (model.sort)
                {
                    case "BranchUnitDepartment.BranchUnit.Branch.Company.Name":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                generalDaySetups = generalDaySetups
                                    .OrderByDescending(r => r.BranchUnitDepartment.BranchUnit.Branch.Company.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                generalDaySetups = generalDaySetups
                                    .OrderBy(r => r.BranchUnitDepartment.BranchUnit.Branch.Company.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    case "BranchUnitDepartment.BranchUnit.Branch.Name":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                generalDaySetups = generalDaySetups
                                    .OrderByDescending(r => r.BranchUnitDepartment.BranchUnit.Branch.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                generalDaySetups = generalDaySetups
                                    .OrderBy(r => r.BranchUnitDepartment.BranchUnit.Branch.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    case "BranchUnitDepartment.UnitDepartment.Unit.Name":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                generalDaySetups = generalDaySetups
                                    .OrderByDescending(r => r.BranchUnitDepartment.UnitDepartment.Unit.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                generalDaySetups = generalDaySetups
                                    .OrderBy(r => r.BranchUnitDepartment.UnitDepartment.Unit.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;

                    default:
                        switch (model.sortdir)
                        {
                            case "DESC":
                                generalDaySetups = generalDaySetups
                                    .OrderByDescending(r => r.BranchUnitDepartment.UnitDepartment.Department.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                generalDaySetups = generalDaySetups
                                    .OrderBy(r => r.BranchUnitDepartment.UnitDepartment.Department.Name)
                                    .Skip(startPage * pageSize)
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

            return generalDaySetups.ToList();
        }


        public GeneralDaySetup GetGeneralDaySetupById(int generalDaySetupId)
        {
            GeneralDaySetup generalDaySetup;
            try
            {
                generalDaySetup =
                    Context.GeneralDaySetups.Include(x => x.BranchUnitDepartment.BranchUnit.Branch.Company)
                    .Include(x => x.BranchUnitDepartment.BranchUnit.Unit)
                        .Include(x => x.BranchUnitDepartment.UnitDepartment.Department)
                        .SingleOrDefault(x => x.GeneralDaySetupId == generalDaySetupId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }
            return generalDaySetup;
        }

        public List<GeneralDaySetup> GetGeneralDaySetupByBranchUnitDepartmentId(int branchUnitDepartmentId)
        {
            List<GeneralDaySetup> generalDaySetups;

            try
            {

                generalDaySetups = (
                    Context.GeneralDaySetups
                        .Where(x => x.BranchUnitDepartmentId == branchUnitDepartmentId && x.IsActive).ToList()
                        .Select(x => new GeneralDaySetup()
                        {
                            DisplayMember = x.DeclaredDate.ToString(),
                            ValueMember = x.GeneralDaySetupId
                        })).OrderBy(x => x.DisplayMember).ToList();

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }

            return generalDaySetups.ToList();
        }

        public bool CheckExistingGeneralDaySetup(DateTime? declaredDate)
        {
            var generalDaySetup =
                Context.GeneralDaySetups.FirstOrDefault(x => x.DeclaredDate == declaredDate && x.IsActive);
            return generalDaySetup == null;
        }
    }
}
