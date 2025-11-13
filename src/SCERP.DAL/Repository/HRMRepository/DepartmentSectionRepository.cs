using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;
namespace SCERP.DAL.Repository.HRMRepository
{
    public class DepartmentSectionRepository : Repository<DepartmentSection>, IDepartmentSectionRepository
   {
        public DepartmentSectionRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<DepartmentSection> GetDepartmentSectionByPaging(int startPage, int pageSize, out int totalRecords, DepartmentSection model,
            SearchFieldModel searchFieldModel)
        {
            IQueryable<DepartmentSection> departmentSections;

            try
            {
            
                departmentSections = Context.DepartmentSections.Include(x => x.BranchUnitDepartment.BranchUnit.Branch.Company).Include(x => x.BranchUnitDepartment.BranchUnit.Unit).Include(x => x.BranchUnitDepartment.UnitDepartment.Department)
                    .Include(x => x.BranchUnitDepartment.UnitDepartment.Unit).Include(x => x.BranchUnitDepartment.UnitDepartment.Department).Include(x => x.Section)
                    .Where(
                        x =>
                            x.IsActive &&
                            (x.BranchUnitDepartment.BranchUnit.Branch.CompanyId == searchFieldModel.SearchByCompanyId ||
                             searchFieldModel.SearchByCompanyId == 0)
                            &&
                            (x.BranchUnitDepartment.BranchUnit.BranchId == searchFieldModel.SearchByBranchId ||
                             searchFieldModel.SearchByBranchId == 0)
                            &&
                            (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId ||
                             searchFieldModel.SearchByBranchUnitDepartmentId == 0)
                            &&
                            (x.BranchUnitDepartment.BranchUnitId == searchFieldModel.SearchByBranchUnitId ||
                             searchFieldModel.SearchByBranchUnitId == 0) &&
                               (x.SectionId == searchFieldModel.SearchBySectionId || searchFieldModel.SearchBySectionId == 0));
                totalRecords = departmentSections.Count();
                switch (model.sort)
                {
                    case "BranchUnitDepartment.BranchUnit.Branch.Company.Name":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                departmentSections = departmentSections
                                    .OrderByDescending(r => r.BranchUnitDepartment.BranchUnit.Branch.Company.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                departmentSections = departmentSections
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
                                departmentSections = departmentSections
                                    .OrderByDescending(r => r.BranchUnitDepartment.BranchUnit.Branch.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                departmentSections = departmentSections
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
                                departmentSections = departmentSections

                                    .OrderByDescending(r => r.BranchUnitDepartment.UnitDepartment.Unit.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                departmentSections = departmentSections
                                    .OrderBy(r => r.BranchUnitDepartment.UnitDepartment.Unit.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    case "BranchUnitDepartment.UnitDepartment.Department.Name":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                departmentSections = departmentSections
                                    .OrderByDescending(r => r.BranchUnitDepartment.UnitDepartment.Department.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                departmentSections = departmentSections
                                    .OrderBy(r => r.BranchUnitDepartment.UnitDepartment.Department.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    case "Section.Name":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                departmentSections = departmentSections
                                    .OrderByDescending(r => r.Section.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                departmentSections = departmentSections
                                    .OrderBy(r => r.Section.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    default:
                        departmentSections = departmentSections
                                                       .OrderBy(r => r.Section.Name)
                                                       .Skip(startPage * pageSize)
                                                       .Take(pageSize);
                        break;
                }
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }

            return departmentSections.ToList();
        }

        public List<DepartmentSection> GetDepartmentSectionBySearchKey(SearchFieldModel searchFieldModel)
        {
            IQueryable<DepartmentSection> departmentSections;

            try
            {
                departmentSections =
                Context.DepartmentSections.Include(x => x.BranchUnitDepartment.BranchUnit.Branch.Company).Include(x => x.BranchUnitDepartment.BranchUnit.Unit).Include(x => x.BranchUnitDepartment.UnitDepartment.Department)
                    .Include(x => x.BranchUnitDepartment.UnitDepartment.Unit).Include(x => x.BranchUnitDepartment.UnitDepartment.Department).Include(x => x.Section)
                    .Where(
                        x =>
                            x.IsActive &&
                            (x.BranchUnitDepartment.BranchUnit.Branch.CompanyId == searchFieldModel.SearchByCompanyId ||
                             searchFieldModel.SearchByCompanyId == 0)
                            &&
                            (x.BranchUnitDepartment.BranchUnit.BranchId == searchFieldModel.SearchByBranchId ||
                             searchFieldModel.SearchByBranchId == 0)
                            &&
                            (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId ||
                             searchFieldModel.SearchByBranchUnitDepartmentId == 0)
                            &&
                            (x.BranchUnitDepartment.BranchUnitId == searchFieldModel.SearchByBranchUnitId ||
                             searchFieldModel.SearchByBranchUnitId == 0) &&
                               (x.SectionId == searchFieldModel.SearchBySectionId || searchFieldModel.SearchBySectionId == 0));
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }

            return departmentSections.ToList();
        }

        public DepartmentSection GetDepartmentSectionById(int departmentSectionId)
        {
            DepartmentSection departmentSection;
            try
            {
             
                departmentSection =
                    Context.DepartmentSections.Include(x => x.BranchUnitDepartment.BranchUnit.Branch.Company)
                    .Include(x => x.BranchUnitDepartment.BranchUnit.Unit)
                        .Include(x => x.BranchUnitDepartment.UnitDepartment.Department)
                        .SingleOrDefault(x => x.DepartmentSectionId == departmentSectionId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }
            return departmentSection;
        }

        public List<DepartmentSection> GetDepartmentSectionByBranchUnitDepartmentId(int branchUnitDepartmentId)
        {
            List<DepartmentSection> departmentSections;

            try
            {
                departmentSections = (
                    Context.DepartmentSections.Include(x => x.Section)
                        .Where(x => x.BranchUnitDepartmentId == branchUnitDepartmentId && x.IsActive).ToList()
                        .Select(x => new DepartmentSection()
                        {
                            DisplayMember = x.Section.Name,
                            ValueMember = x.DepartmentSectionId
                        })).OrderBy(x=>x.DisplayMember).ToList();

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }

            return departmentSections.ToList();
        }
   }
}
