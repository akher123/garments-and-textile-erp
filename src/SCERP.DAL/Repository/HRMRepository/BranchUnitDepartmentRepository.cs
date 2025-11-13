using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class BranchUnitDepartmentRepository : Repository<BranchUnitDepartment>, IBranchUnitDepartmentRepository
    {
        public BranchUnitDepartmentRepository(SCERPDBContext context)
            : base(context)
        {

        }
        public List<BranchUnitDepartment> GetAllBranchUnitDepartment(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel,
            BranchUnitDepartment model)
        {
            IQueryable<BranchUnitDepartment> branchUnitDepartments;
            try
            {
                branchUnitDepartments =
                    Context.BranchUnitDepartments.Include(x => x.BranchUnit.Branch.Company)
                        .Include(x => x.UnitDepartment.Unit).Include(x => x.UnitDepartment.Department)
                        .Where(
                            x =>
                                x.IsActive &&
                                (x.BranchUnit.Branch.CompanyId == searchFieldModel.SearchByCompanyId ||
                                 searchFieldModel.SearchByCompanyId == 0)
                                &&
                                (x.BranchUnit.BranchId == searchFieldModel.SearchByBranchId ||
                                 searchFieldModel.SearchByBranchId == 0)
                                &&
                                (x.UnitDepartmentId == searchFieldModel.SearchByUnitDepartmentId ||
                                 searchFieldModel.SearchByUnitDepartmentId == 0)
                                &&
                                (x.BranchUnitId == searchFieldModel.SearchByBranchUnitId ||
                                 searchFieldModel.SearchByBranchUnitId == 0));
                totalRecords = branchUnitDepartments.Count();
                switch (model.sort)
                {
                    case "BranchUnit.Branch.Company.Name":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                branchUnitDepartments = branchUnitDepartments
                                    .OrderByDescending(r => r.BranchUnit.Branch.Company.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                branchUnitDepartments = branchUnitDepartments
                                    .OrderBy(r => r.BranchUnit.Branch.Company.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    case "BranchUnit.Branch.Name":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                branchUnitDepartments = branchUnitDepartments
                                    .OrderByDescending(r => r.BranchUnit.Branch.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                branchUnitDepartments = branchUnitDepartments
                                    .OrderBy(r => r.BranchUnit.Branch.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    case "UnitDepartment.Unit.Name":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                branchUnitDepartments = branchUnitDepartments
                                    .OrderByDescending(r => r.UnitDepartment.Unit.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                branchUnitDepartments = branchUnitDepartments
                                    .OrderBy(r => r.UnitDepartment.Unit.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    case "UnitDepartment.Department.Name":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                branchUnitDepartments = branchUnitDepartments
                                    .OrderByDescending(r => r.UnitDepartment.Department.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                branchUnitDepartments = branchUnitDepartments
                                    .OrderBy(r => r.UnitDepartment.Department.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    default:
                        branchUnitDepartments = branchUnitDepartments
                               .OrderBy(r => r.UnitDepartment.Department.Name)
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
            return branchUnitDepartments.ToList();
        }

        public BranchUnitDepartment GetBranchUnitDepartmentById(int branchUnitDepartmentId)
        {
            BranchUnitDepartment branchUnitDepartment;
            try
            {
                branchUnitDepartment =
                    Context.BranchUnitDepartments.Include(x => x.BranchUnit.Branch.Company)
                        .Include(x => x.UnitDepartment.Unit)
                        .SingleOrDefault(x => x.BranchUnitDepartmentId == branchUnitDepartmentId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }
            return branchUnitDepartment;
        }

        public List<BranchUnitDepartment> GetBranchUnitDepartmentBySearchKey(SearchFieldModel searchFieldModel)
        {
            IQueryable<BranchUnitDepartment> branchUnitDepartments;
            try
            {
                branchUnitDepartments =
                    Context.BranchUnitDepartments.Include(x => x.BranchUnit.Branch)
                        .Include(x => x.BranchUnit.Branch.Company)
                        .Include(x => x.BranchUnit.Unit).Include(x => x.UnitDepartment.Department)
                        .Where(
                            x =>
                                x.IsActive &&
                                (x.BranchUnit.Branch.CompanyId == searchFieldModel.SearchByCompanyId ||
                                 searchFieldModel.SearchByCompanyId == 0)
                                &&
                                (x.BranchUnit.BranchId == searchFieldModel.SearchByBranchId ||
                                 searchFieldModel.SearchByBranchId == 0)
                                &&
                                (x.UnitDepartmentId == searchFieldModel.SearchByUnitDepartmentId ||
                                 searchFieldModel.SearchByUnitDepartmentId == 0)
                                &&
                                (x.BranchUnitId == searchFieldModel.SearchByBranchUnitId ||
                                 searchFieldModel.SearchByBranchUnitId == 0));

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return branchUnitDepartments.ToList();
        }

        public List<Unit> GetUnitsByBranchId(int searchByBranchId)
        {
            List<Unit> units;
            try
            {
                units =
                    Context.BranchUnitDepartments.Include(x => x.BranchUnit.Unit)
                        .Where(x => x.IsActive).OrderBy(x => x.UnitDepartment.Unit.Name)
                        .Select(x => x.UnitDepartment.Unit)
                        .ToList();
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);

            }
            return units;
        }

        public List<Department> GetDepartmentsByBranchId(int searchByUnitId)
        {
            List<Department> departments = null;
            try
            {

                departments =
              Context.BranchUnitDepartments.Include(x => x.UnitDepartment.Department)
                  .Where(x => x.IsActive).OrderBy(x => x.UnitDepartment.Department.Name)
                  .Select(x => x.UnitDepartment.Department)
                  .ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);

            }

            return departments;
        }

        public List<BranchUnitDepartment> GetBranchUnitDepartment()
        {
            IQueryable<BranchUnitDepartment> branchUnitDepartments;
            try
            {
                branchUnitDepartments =
                    Context.BranchUnitDepartments.Include(x => x.BranchUnit.Branch)
                        .Include(x => x.BranchUnit.Unit).Include(x => x.UnitDepartment.Department)
                        .Where(x => x.IsActive);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return branchUnitDepartments.ToList();
        }
    }
}

