using System;
using System.Data.Entity;
using System.Linq;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Collections.Generic;
using SCERP.Common;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeGradeRepository : Repository<EmployeeGrade>, IEmployeeGradeRepository
    {
        public EmployeeGradeRepository(SCERPDBContext context)
            : base(context)
        {

        }
        public override IQueryable<EmployeeGrade> All()
        {
            return Context.EmployeeGrades.Where(x => x.IsActive == true);
        }

        public EmployeeGrade GetEmployeeGradeById(int? id)
        {
            return Context.EmployeeGrades.Find(id);
        }


        public List<EmployeeGrade> GetAllEmployeeGradesByPaging(int startPage, int pageSize, out int totalRecords, EmployeeGrade employeeGrade)
        {
            IQueryable<EmployeeGrade> employeeGrades = null;

            try
            {
                string searchByEmployeeGrade = employeeGrade.Name;
                int searchByEmployeeType = employeeGrade.EmployeeTypeId;



                employeeGrades = Context.EmployeeGrades.Include(x => x.EmployeeType).Where(
                    x =>
                        x.IsActive == true &&
                        ((x.Name.Replace(" ", "")
                            .ToLower()
                            .Contains(searchByEmployeeGrade.Replace(" ", "").ToLower())) ||
                         String.IsNullOrEmpty(searchByEmployeeGrade))
                        && (x.EmployeeTypeId == searchByEmployeeType || searchByEmployeeType == 0));
                totalRecords = employeeGrades.Count();

                switch (employeeGrade.sort)
                {
                    case "EmployeeType.Title":

                        switch (employeeGrade.sortdir)
                        {
                            case "DESC":
                                employeeGrades = employeeGrades
                                    .OrderByDescending(r => r.EmployeeType.Title).ThenBy(x=>x.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                employeeGrades = employeeGrades
                                 .OrderBy(r => r.EmployeeType.Title).ThenBy(x => x.Name)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                        }
                        break;
                    case "Name":
                        switch (employeeGrade.sortdir)
                        {
                            case "DESC":
                                employeeGrades = employeeGrades
                               .OrderByDescending(r => r.Name)
                               .Skip(startPage * pageSize)
                               .Take(pageSize);
                                break;
                            default:
                                employeeGrades = employeeGrades
                              .OrderBy(r => r.Name)
                              .Skip(startPage * pageSize)
                              .Take(pageSize);
                                break;
                        }
                        break;
                    default:
                        employeeGrades = employeeGrades
                               .OrderBy(r => r.Name)
                               .Skip(startPage * pageSize)
                               .Take(pageSize);
                        break;
                }
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return employeeGrades.ToList();
        }


        public List<EmployeeGrade> GetAllEmployeeGradesBySearchKey(string searchByEmployeeGrade, int searchByEmployeeType)
        {
            List<EmployeeGrade> employeeGrades = null;

            try
            {
                employeeGrades = Context.EmployeeGrades.Where(
                    x =>
                        x.IsActive == true &&
                        ((x.Name.Replace(" ", "")
                            .ToLower()
                            .Contains(searchByEmployeeGrade.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(searchByEmployeeGrade))
                        && (x.EmployeeTypeId == searchByEmployeeType || searchByEmployeeType == 0)).Include(x => x.EmployeeType).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return employeeGrades;
        }

        public List<EmployeeGrade> GetAllEmployeeGrades()
        {
            var employeeGradeList = Context.EmployeeGrades.Where(x => x.IsActive == true).OrderBy(r => r.Name).ToList();
            return employeeGradeList;
        }

        public List<EmployeeType> GetAllEmployeeType()
        {
            return Context.EmployeeTypes.Where(x => x.IsActive == true).OrderBy(y => y.Title).ToList();
        }

        public List<EmployeeGrade> GetGradeByEmployeeTypeId(int employeeTypeId)
        {
            var employeeGradeList = Context.EmployeeGrades.Where(x => x.EmployeeTypeId == employeeTypeId && x.IsActive).OrderBy(r => r.Name).ToList();
            return employeeGradeList;
        }

    }
}
