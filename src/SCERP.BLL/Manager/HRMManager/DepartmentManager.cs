using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCERP.BLL.Manager.HRMManager
{
    public class DepartmentManager : BaseManager, IDepartmentManager
    {
        private readonly IDepartmentRepository _departmentRepository = null;

        public DepartmentManager(SCERPDBContext context)
        {
            this._departmentRepository = new DepartmentRepository(context);
        }


        public List<Department> GetAllDepartmentsByPaging(int startPage, int pageSize, out int totalRecords, Department department)
        {
            var departments = new List<Department>();
            try
            {
                departments = _departmentRepository.GetAllDepartmentsByPaging(startPage, pageSize, out totalRecords, department).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                totalRecords = 0;
            }

            return departments;
        }


        public List<Department> GetAllDepartments()
        {
            var departments = new List<Department>();
            try
            {
                departments = _departmentRepository.GetAllDepartments();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            
            }
            return departments;
        }

        public Department GetDepartmentById(int? id)
        {
            var department = new Department();
            try
            {
                department = _departmentRepository.GetDepartmentById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
          
            }

            return department;
        }

        public List<Department> GetDepartmentBySearchKey(string searchKey)
        {
            var departments = new List<Department>();
            try
            {
                departments =
                    _departmentRepository.Filter(
                        x => x.Name.Replace(" ", "").ToLower().Contains(searchKey.Replace(" ", "")
                            .ToLower()) || String.IsNullOrEmpty(searchKey)).OrderBy(x => x.Name).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return departments;
        }


        public bool CheckExistingDepartment(Department department)
        {
            bool isExist = false;
            try
            {
                isExist =
                    _departmentRepository.Exists(
                        x =>
                            x.IsActive &&
                            x.Id != department.Id &&
                            x.Name.Replace(" ", "").ToLower().Equals(department.Name.Replace(" ", "").ToLower()));
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return isExist;
        }


        public int SaveDepartment(Department department)
        {
            var savedDepartment = 0;
            try
            {
                department.IsActive = true;
                department.CreatedDate = DateTime.Now;
                department.CreatedBy = PortalContext.CurrentUser.UserId;
                savedDepartment = _departmentRepository.Save(department);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                savedDepartment = 0;
            }

            return savedDepartment;
        }


        public int EditDepartment(Department department)
        {
            var edit = 0;
            try
            {
                department.EditedDate = DateTime.Now;
                department.EditedBy = PortalContext.CurrentUser.UserId;
                edit = _departmentRepository.Edit(department);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
             
            }
            return edit;
        }


        public int DeleteDepartment(Department department)
        {
            var deleted = 0;
            try
            {
                department.IsActive = false;
                deleted = _departmentRepository.Edit(department);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
               
            }
            return deleted;
        }
    }
}
