using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;
namespace SCERP.DAL.Repository.HRMRepository
{
    public class SalarySetupRepository : Repository<SalarySetup>, ISalarySetupRepository
    {
        public SalarySetupRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public List<SalarySetup> GetAllSalarySetupByPaging(int startPage, int pageSize, SalarySetup salarySetup, out int totalRecords)
        {
            IQueryable<SalarySetup> salarySetups;
            try
            {
                salarySetups = Context.SalarySetups.Include(x => x.EmployeeGrade.EmployeeType).Where(x => x.IsActive
                                                                                                             && (x.EmployeeGradeId == salarySetup.EmployeeGradeId || salarySetup.EmployeeGradeId == 0)
                                                                                                             && (x.EmployeeGrade.EmployeeTypeId == salarySetup.EmployeeGrade.EmployeeTypeId || salarySetup.EmployeeGrade.EmployeeTypeId == 0)
                    );
                totalRecords = salarySetups.Count();
                switch (salarySetup.sort)
                {
                    case "EmployeeGrade.Name":
                        switch (salarySetup.sortdir)
                        {
                            case "DESC":
                                salarySetups = salarySetups.OrderByDescending(r => r.EmployeeGrade.Name).Skip(startPage*pageSize).Take(pageSize);
                                break;
                            default:
                                salarySetups = salarySetups.OrderBy(r => r.EmployeeGrade.Name).Skip(startPage*pageSize).Take(pageSize);
                                break;
                        }
                        break;

                    case "EmployeeGrade.EmployeeType.Title":
                        switch (salarySetup.sortdir)
                        {
                            case "DESC":
                                salarySetups = salarySetups.OrderByDescending(r => r.EmployeeGrade.EmployeeType.Title).Skip(startPage*pageSize).Take(pageSize);
                                break;
                            default:
                                salarySetups = salarySetups.OrderBy(r => r.EmployeeGrade.EmployeeType.Title).Skip(startPage*pageSize).Take(pageSize);
                                break;
                        }
                        break;

                    default:
                        switch (salarySetup.sortdir)
                        {
                            case "DESC":
                                salarySetups = salarySetups.OrderBy(r => r.EmployeeGrade.EmployeeType.Title).ThenBy(r => r.EmployeeGrade.Name).Skip(startPage*pageSize).Take(pageSize);
                                break;

                            default:

                                salarySetups = salarySetups.OrderBy(r => r.EmployeeGrade.EmployeeType.Title).ThenBy(r => r.EmployeeGrade.Name).Skip(startPage*pageSize).Take(pageSize);
                                break;
                        }
                        break;
                }
            }
            catch (Exception exception)
            {
                throw;
            }

            return salarySetups.ToList();
        }

        public List<SalarySetup> GetAllSalarySetupBySearchKey(SalarySetup salarySetup)
        {
            IQueryable<SalarySetup> salarySetups;
            try
            {
                salarySetups = Context.SalarySetups.Include(x => x.EmployeeGrade.EmployeeType).Where(x => x.IsActive
                    && (x.EmployeeGradeId == salarySetup.EmployeeGradeId || salarySetup.EmployeeGradeId == 0)
                    && (x.EmployeeGrade.EmployeeTypeId == salarySetup.EmployeeGrade.EmployeeTypeId || salarySetup.EmployeeGrade.EmployeeTypeId == 0))
                    .OrderBy(x => x.EmployeeGrade.Name);
            }
            catch (Exception)
            {

                throw;
            }
            return salarySetups.ToList();
        }

        public List<SalarySetup> GetAllSalarySetup(int page, int records, string sort, int? GradeId)
        {
            return Context.SalarySetups.Include("EmployeeType").Include("EmployeeGrade").Where(p => p.IsActive == true && p.EmployeeGrade.Id == GradeId).ToList();
        }

        public IQueryable<EmployeeType> GetEmployeeTypes()
        {
            return Context.EmployeeTypes.Where(p => p.IsActive == true);
        }

        public List<EmployeeGrade> GetEmpGradeByEmpType(int employeeTypeId)
        {
            return Context.EmployeeGrades.Where(p => p.EmployeeTypeId == employeeTypeId & p.IsActive == true).ToList();
        }

        public SalarySetup GetSalarySetupById(int? id)
        {
            return Context.SalarySetups.Include(x => x.EmployeeGrade).FirstOrDefault(x => x.Id == id && x.IsActive);
        }

        public SalarySetup GetLatestSalarySetupInfoByGrade(SalarySetup salarySetup)
        {
            SalarySetup salarySetupNew = null;

            try
            {
                salarySetupNew =
                    Context.SalarySetups.Where(x => x.IsActive && x.EmployeeGradeId == salarySetup.EmployeeGradeId)
                        .OrderByDescending(x => x.FromDate)
                        .ToList()
                        .FirstOrDefault();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return salarySetupNew;
        }

        public SalarySetup GetSalarySetupByEmployeeGrade(int employeeGradeId, DateTime? effectiveDate)
        {
            try
            {
                return
                        Context.SalarySetups
                        .Where(x => x.IsActive &&
                                    x.EmployeeGradeId == employeeGradeId &&
                                    x.FromDate <= effectiveDate)
                        .OrderByDescending(x => x.FromDate)
                        .FirstOrDefault();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

        }

    }
}
