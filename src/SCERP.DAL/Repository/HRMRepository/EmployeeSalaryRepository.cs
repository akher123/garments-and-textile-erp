using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Data;
using System.Data.Entity;
using System;
using System.Collections.Generic;


namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeSalaryRepository : Repository<EmployeeSalary>, IEmployeeSalaryRepository
    {
        public EmployeeSalaryRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public EmployeeSalary GetEmployeeSalaryById(int? id)
        {
            return Context.EmployeeSalaries.Find(id);
        }


        public List<EmployeeSalary> GetAllEmployeeSalarys(int page, int records, string sort)
        {
            var employeeSalary = Context.EmployeeSalaries.Where(r => r.IsActive == true).ToList();

            switch (sort)
            {
                case "GrossSalary":
                    employeeSalary = employeeSalary.OrderBy(r => r.GrossSalary).ToList();
                    break;

                case "BasicSalary":
                    employeeSalary = employeeSalary.OrderBy(r => r.BasicSalary).ToList();
                    break;

                case "HouseRent":
                    employeeSalary = employeeSalary.OrderBy(r => r.HouseRent).ToList();
                    break;

                case "MedicalAllowance":
                    employeeSalary = employeeSalary.OrderBy(r => r.MedicalAllowance).ToList();
                    break;

                case "FoodAllowance":
                    employeeSalary = employeeSalary.OrderBy(r => r.FoodAllowance).ToList();
                    break;

                case "Conveyance":
                    employeeSalary = employeeSalary.OrderBy(r => r.Conveyance).ToList();
                    break;

                default:
                    employeeSalary = employeeSalary.OrderBy(r => r.Id).ToList();
                    break;
            }

            employeeSalary = employeeSalary.Skip(page * records).Take(records).ToList();
            return employeeSalary;
        }


        public List<EmployeeSalary> GetEmployeeSalaryById(Guid employeeId)
        {
            List<EmployeeSalary> employeeSalaries = null;

            try
            {
                employeeSalaries = Context.EmployeeSalaries.Include(p => p.Employee).Where(p => p.EmployeeId == employeeId & p.IsActive == true).OrderByDescending(x => x.Id).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeeSalaries;
        }

        public IQueryable<EmployeeSalary> GetEmployeeSalary()
        {
            return Context.EmployeeSalaries.Include(p => p.Employee).Where(p => p.IsActive == true);
        }

        public EmployeeSalary GetEmployeeSalary(int id)
        {
            var employeeSalary = Context.EmployeeSalaries.Include(p => p.Employee).SingleOrDefault(p => p.Id == id);
            return employeeSalary;
        }

        public IQueryable<Employee> GetEmployee()
        {
            return Context.Employees.Where(p => p.IsActive == true);
        }


        public EmployeeSalary GetEmployeeSalaryInfoById(Guid employeeId, int id)
        {
            IQueryable<EmployeeSalary> employeeSalaries;
            try
            {
                employeeSalaries =
                    Context.EmployeeSalaries
                        .Where(x => x.IsActive)
                        .Where(x => (x.EmployeeId == employeeId) && (x.Id == id));
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeeSalaries.ToList().FirstOrDefault();
        }


        public EmployeeSalary GetLatestEmployeeSalaryInfoByEmployeeGuidId(EmployeeSalary employeeSalary)
        {
            IQueryable<EmployeeSalary> employeeSalaries;
            try
            {
                employeeSalaries = Context
                    .EmployeeSalaries
                    .Where(x => x.IsActive
                           && x.EmployeeId == employeeSalary.EmployeeId
                           && x.FromDate <= employeeSalary.FromDate
                           && x.Id != employeeSalary.Id)
                    .OrderByDescending(x => x.FromDate);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeeSalaries.ToList().FirstOrDefault();
        }

        public int UpdateEmployeeSalaryInfoDate(EmployeeSalary employeeSalary)
        {
            var update = 0;
            try
            {
                update = base.Edit(employeeSalary);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return update;
        }

        public int GetEmployeeTypeByEmployeeId(Guid EmployeeId)
        {
            string sqlQuery = String.Format(@"EXEC SPEmployeeInfo '{0}'", EmployeeId);

            DataTable dt = ExecuteQuery(sqlQuery);

            string employeeType = dt.Rows[0]["EmployeeType"].ToString();

            if (employeeType.Contains("Team"))
                return 1;
            else
                return 2;
        }
    }
}