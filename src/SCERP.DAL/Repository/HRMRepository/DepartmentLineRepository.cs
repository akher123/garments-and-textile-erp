using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class DepartmentLineRepository : Repository<DepartmentLine>, IDepartmentLineRepository
    {
        public DepartmentLineRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public List<DepartmentLine> GetDepartmentLine(int startPage, int pageSize, out int totalRecords, DepartmentLine model, SearchFieldModel searchFieldModel)
        {
            IQueryable<DepartmentLine> departmentLines;
            try
            {
                departmentLines =
                Context.DepartmentLines.Include(x => x.BranchUnitDepartment.BranchUnit.Branch.Company).Include(x => x.BranchUnitDepartment.BranchUnit.Unit).Include(x => x.BranchUnitDepartment.UnitDepartment.Department)
                    .Include(x => x.BranchUnitDepartment.UnitDepartment.Unit).Include(x => x.BranchUnitDepartment.UnitDepartment.Department).Include(x => x.Line)
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
                               (x.LineId == searchFieldModel.SearchByLineId || searchFieldModel.SearchByLineId == 0));
                totalRecords = departmentLines.Count();
                switch (model.sort)
                {
                    case "BranchUnitDepartment.BranchUnit.Branch.Company.Name":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                departmentLines = departmentLines
                                    .OrderByDescending(r => r.BranchUnitDepartment.BranchUnit.Branch.Company.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                departmentLines = departmentLines
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
                                departmentLines = departmentLines
                                    .OrderByDescending(r => r.BranchUnitDepartment.BranchUnit.Branch.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                departmentLines = departmentLines
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
                                departmentLines = departmentLines
                                    .OrderByDescending(r => r.BranchUnitDepartment.UnitDepartment.Unit.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                departmentLines = departmentLines
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
                                departmentLines = departmentLines
                                    .OrderByDescending(r => r.BranchUnitDepartment.UnitDepartment.Department.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                departmentLines = departmentLines
                                    .OrderBy(r => r.BranchUnitDepartment.UnitDepartment.Department.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    case "Line.Name":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                departmentLines = departmentLines
                                    .OrderByDescending(r => r.Line.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                departmentLines = departmentLines
                                    .OrderBy(r => r.Line.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;

                    default:
                        switch (model.sortdir)
                        {
                            case "DESC":
                                departmentLines = departmentLines
                                     .OrderByDescending(r => r.Line.Name)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                            default:
                                departmentLines = departmentLines
                                  .OrderBy(r => r.Line.Name)
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

            return departmentLines.ToList();
        }
        public List<DepartmentLine> GetDepartmentLineBySearchKey(SearchFieldModel searchFieldModel)
        {
            IQueryable<DepartmentLine> departmentLines;

            try
            {
                departmentLines =
                Context.DepartmentLines.Include(x => x.BranchUnitDepartment.BranchUnit.Branch.Company).Include(x => x.BranchUnitDepartment.BranchUnit.Unit).Include(x => x.BranchUnitDepartment.UnitDepartment.Department)
                    .Include(x => x.BranchUnitDepartment.UnitDepartment.Unit).Include(x => x.BranchUnitDepartment.UnitDepartment.Department).Include(x => x.Line)
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
                               (x.LineId == searchFieldModel.SearchByLineId || searchFieldModel.SearchByLineId == 0));
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }

            return departmentLines.ToList();
        }

        public DepartmentLine GetDepartmentLineById(int departmentLineId)
        {
            DepartmentLine department;
            try
            {
                department =
                    Context.DepartmentLines.Include(x => x.BranchUnitDepartment.BranchUnit.Branch.Company)
                    .Include(x => x.BranchUnitDepartment.BranchUnit.Unit)
                        .Include(x => x.BranchUnitDepartment.UnitDepartment.Department)
                        .SingleOrDefault(x => x.DepartmentLineId == departmentLineId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }
            return department;
        }

        public List<DepartmentLine> GetDepartmentLineByBranchUnitDepartmentId(int branchUnitDepartmentId)
        {
            List<DepartmentLine> departmentLines;

            try
            {

                departmentLines = (
                    Context.DepartmentLines.Include(x => x.Line)
                        .Where(x => x.BranchUnitDepartmentId == branchUnitDepartmentId && x.IsActive).ToList()
                        .Select(x => new DepartmentLine()
                        {
                            DisplayMember = x.Line.Name,
                            ValueMember = x.DepartmentLineId
                        })).OrderBy(x => x.DisplayMember).ToList();

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }

            return departmentLines.ToList();
        }
    }
}
