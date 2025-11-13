using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;


namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeTypeRepository : Repository<EmployeeType>, IEmployeeTypeRepository
    {
        public EmployeeTypeRepository(SCERPDBContext context)
            : base(context)
        {

        }
        public override IQueryable<EmployeeType> All()
        {
            return Context.EmployeeTypes.Where(x => x.IsActive == true);
        }

        public EmployeeType GetEmployeeTypeById(int? id)
        {
            return Context.EmployeeTypes.Find(id);
        }

     
        public List<EmployeeType> GetAllEmployeeTypesByPaging(int startPage, int pageSize, out int totalRecords, EmployeeType employeeType)
        {
            IQueryable<EmployeeType> employeeTypes;

            try
            {
                string searchKey = employeeType.Title;
                employeeTypes = Context.EmployeeTypes.Where(
                    x =>
                        x.IsActive == true &&
                        ((x.Title.Replace(" ", "")
                            .ToLower()
                            .Contains(searchKey.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(searchKey)));

                totalRecords = employeeTypes.Count();
                switch (employeeType.sortdir)
                {
                    case "DESC":
                        employeeTypes = employeeTypes
                            .OrderByDescending(r => r.Title)
                            .Skip(startPage*pageSize)
                            .Take(pageSize);
                        break;
                    default:
                        employeeTypes = employeeTypes
                            .OrderBy(r => r.Title)
                            .Skip(startPage*pageSize)
                            .Take(pageSize);
                        break;
                }
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.Message);
            }

            return employeeTypes.ToList();
        }
          

        public List<EmployeeType> GetAllEmployeeTypes()
        {
            List<EmployeeType> employeeTypes;

            try
            {
                employeeTypes = Context.EmployeeTypes.Where(r => r.IsActive == true).OrderBy(x => x.Title).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                employeeTypes = null;
            }

            return employeeTypes;
        }
    }
}
