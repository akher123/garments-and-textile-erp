using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class UnitDepartmentRepository : Repository<UnitDepartment>, IUnitDepartmentRepository
    {
        public UnitDepartmentRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public List<UnitDepartment> GetAllUnitDepartmentsByPaging(int startPage, int pageSize, UnitDepartment unitDepartment, out int totalRecords)
        {
            IQueryable<UnitDepartment> unitDepartments = null;
            try
            {
                unitDepartments = Context.UnitDepartments
                    .Where(
                        x => x.IsActive == true && (x.UnitId == unitDepartment.UnitId || unitDepartment.UnitId == 0) &&
                             (x.DepartmentId == unitDepartment.DepartmentId || unitDepartment.DepartmentId == 0))
                    .Include(x => x.Unit)
                    .Include(x => x.Department);
                totalRecords = unitDepartments.Count();

                switch (unitDepartment.sort)
                {
                    case "Unit.Name":

                        switch (unitDepartment.sortdir)
                        {
                            case "DESC":
                                unitDepartments = unitDepartments
                                    .OrderByDescending(r => r.Unit.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                 
                                break;
                            default:
                                unitDepartments = unitDepartments
                                    .Include(x => x.Department)
                                    .OrderBy(r => r.Unit.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                
                                break;
                        }
                        break;

                    default:
                        switch (unitDepartment.sortdir)
                        {
                            case "DESC":
                                unitDepartments = unitDepartments
                                    .OrderByDescending(r => r.Department.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                 
                                break;
                            default:
                                unitDepartments = unitDepartments
                                    .OrderBy(r => r.Department.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                  
                                break;
                        }
                        break;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return unitDepartments.ToList();
        }

        public IEnumerable GetAllUnitDepatmeByBranchUnitId(int branchUnitId)
        {
            var unitDepartments =( from branchUnit in Context.BranchUnits
                join udept in Context.UnitDepartments on branchUnit.UnitId equals udept.UnitId
                where branchUnit.BranchUnitId == branchUnitId
                select new
                {
                    UnitDepartmentId = udept.UnitDepartmentId,
                    DepartmentName = udept.Department.Name
                }).OrderBy(x => x.DepartmentName);
            return unitDepartments;
        }
    }
}
