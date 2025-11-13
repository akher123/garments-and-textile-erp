using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SCERP.DAL.Repository.HRMRepository
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {

        public DepartmentRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public Department GetDepartmentById(int? id)
        {
            return Context.Departments.Find(id);
        }

        public List<Department> GetAllDepartmentsByPaging(int startPage, int pageSize, out int totalRecords, Department department)
        {
            IQueryable<Department> departments;

            try
            {
                string searchKey = department.Name;
                departments = Context.Departments.Where( x => x.IsActive &&((x.Name.Replace(" ", "")
                            .ToLower()
                            .Contains(searchKey.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(searchKey)));
                totalRecords = departments.Count();
                switch (department.sort)
                {
                    case "Name":
                        switch (department.sortdir)
                        {
                            case "DESC":
                                departments = departments
                                    .OrderByDescending(r => r.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                departments = departments
                                    .OrderBy(r => r.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    default:
                        departments = departments
                            .OrderBy(r => r.Name)
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

            return departments.ToList();
        }
        public List<Department> GetAllDepartmentsBySearchKey(string searchKey)
        {
            List<Department> departments = null;
            try
            {
                departments = !String.IsNullOrEmpty(searchKey) ? Context.Departments.Where(x => x.IsActive == true && x.Name.ToLower().Contains(searchKey.ToLower())).ToList() : GetAllDepartments();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);

            }
            return departments;
        }


        public List<Department> GetAllDepartments()
        {
            return Context.Departments.Where(x => x.IsActive).OrderBy(x => x.Name).ToList();
        }
    }
}
