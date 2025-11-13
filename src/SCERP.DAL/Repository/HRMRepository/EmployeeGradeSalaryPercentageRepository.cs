using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeGradeSalaryPercentageRepository : Repository<EmployeeGradeSalaryPercentage>, IEmployeeGradeSalaryPercentageRepository
    {
        public EmployeeGradeSalaryPercentageRepository(SCERPDBContext context) : base(context)
        {

        }
        public List<EmployeeGradeSalaryPercentage> GetEmployeeTypeSalaryPercentages(int startPage, int pageSize, EmployeeGradeSalaryPercentage model, SearchFieldModel searchField,
            out int totalRecords)
        {
            IQueryable<EmployeeGradeSalaryPercentage> employeeTypeSalaryPercentages;
            try
            {
                employeeTypeSalaryPercentages =
                    Context.EmployeeGradeSalaryPercentages.Include(x => x.EmployeeGrade.EmployeeType).Where(
                            x =>
                                x.IsActive && (x.Status == (Int16)StatusValue.Active) &&
                                (x.EmployeeGradeId == searchField.SearchByEmployeeGradeId || searchField.SearchByEmployeeGradeId == 0) &&
                                (x.EmployeeGrade.EmployeeTypeId == searchField.SearchByEmployeeTypeId || searchField.SearchByEmployeeTypeId == 0));
                totalRecords = employeeTypeSalaryPercentages.Count();

                switch (model.sortdir)
                {
                    case "DESC":
                        employeeTypeSalaryPercentages = employeeTypeSalaryPercentages
                            .OrderByDescending(r => r.EmployeeGrade.EmployeeType.Title).ThenByDescending(r => r.EmployeeGrade.Name)
                            .Skip(startPage * pageSize)
                            .Take(pageSize);

                        break;
                    default:
                        employeeTypeSalaryPercentages = employeeTypeSalaryPercentages
                              .OrderBy(r => r.EmployeeGrade.EmployeeType.Title).ThenBy(r => r.EmployeeGrade.Name)
                               .Skip(startPage * pageSize)
                               .Take(pageSize);
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return employeeTypeSalaryPercentages.ToList();
        }

        public List<EmployeeGradeSalaryPercentage> GetEmployeeTypeSalaryPercentageBySearchKey(SearchFieldModel searchField)
        {
            IQueryable<EmployeeGradeSalaryPercentage> employeeTypeSalaryPercentages;
            try
            {
                employeeTypeSalaryPercentages =
      Context.EmployeeGradeSalaryPercentages.Include(x => x.EmployeeGrade.EmployeeType).Where(
              x =>
                  x.IsActive && (x.Status == (Int16)StatusValue.Active) &&
                  (x.EmployeeGradeId == searchField.SearchByEmployeeGradeId || searchField.SearchByEmployeeGradeId == 0) &&
                  (x.EmployeeGrade.EmployeeTypeId == searchField.SearchByEmployeeTypeId || searchField.SearchByEmployeeTypeId == 0));

            }
            catch (Exception)
            {

                throw;
            }
            return employeeTypeSalaryPercentages.ToList();
        }

        public EmployeeGradeSalaryPercentage GetEmployeeGradeSalaryPercentageById(int employeeGradeSalaryPercentageId)
        {

            EmployeeGradeSalaryPercentage employeeTypeSalaryPercentage;
            try
            {
                employeeTypeSalaryPercentage =
                    Context.EmployeeGradeSalaryPercentages.Include(x => x.EmployeeGrade.EmployeeType).FirstOrDefault(x => x.IsActive && x.EmployeeGradeSalaryPercentageId == employeeGradeSalaryPercentageId);
            }
            catch (Exception)
            {

                throw;
            }
            return employeeTypeSalaryPercentage;
        }

        public EmployeeGradeSalaryPercentage GetEmployeeGradeSalaryPercentangeByEmployeeGradeAndTypeId(int employeeGradeId, int employeeTypeId)
        {
            EmployeeGradeSalaryPercentage employeeGradeSalaryPercentage;

            try
            {
                employeeGradeSalaryPercentage = Context.EmployeeGradeSalaryPercentages.Include(x => x.EmployeeGrade.EmployeeType).Where(x => x.IsActive && (x.Status == (Int16)StatusValue.Active)).FirstOrDefault(x => x.EmployeeGradeId == employeeGradeId && x.EmployeeGrade.EmployeeTypeId == employeeTypeId);
            }
            catch (Exception)
            {
                throw;
            }
            return employeeGradeSalaryPercentage;
        }
    }
}