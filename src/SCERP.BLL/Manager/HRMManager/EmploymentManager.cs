using System;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using System.Linq;
using SCERP.Model;
using System.Collections.Generic;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EmploymentManager : BaseManager, IEmploymentManager
    {

        private readonly IEmploymentRepository _employmentRepository = null;

        public EmploymentManager(SCERPDBContext context)
        {
            _employmentRepository = new EmploymentRepository(context);
        }

        public List<Employment> GetEmploymentsByEmployeeId(Guid employeeId)
        {
            IQueryable<Employment> employments;
            try
            {
                employments = _employmentRepository.Filter(x => x.IsActive && x.EmployeeId == employeeId).OrderBy(x=>x.CompanyName);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employments.ToList();
        }

        public Employment GetEmploymentById(int id)
        {
            Employment employment;
            try
            {
                employment = _employmentRepository.FindOne(x => x.Id == id);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employment;
        }

        public Employment GetEmploymentById(Guid? employeeId, int? id)
        {
            Employment employment;

            try
            {
                employment = _employmentRepository.FindOne(x => x.EmployeeId == employeeId && x.Id == id);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employment;
        }

        public int EditEmployment(Employment employment)
        {
            var edit = 0;
            try
            {
                employment.EditedBy = PortalContext.CurrentUser.UserId;
                employment.EditedDate = DateTime.Now;
                edit = _employmentRepository.Edit(employment);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return edit;
        }

        public int SaveEmployment(Employment employment)
        {
            int save = 0;
            try
            {
                employment.CreatedBy = PortalContext.CurrentUser.UserId;
                employment.CreatedDate = DateTime.Now;
                employment.IsActive = true;
                save= _employmentRepository.Save(employment);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return save;
        }


        public int DeleteEmployment(Employment employment)
        {
            var status =0;
            try
            {
                employment.EditedBy = PortalContext.CurrentUser.UserId;
                employment.EditedDate = DateTime.Now;
                employment.IsActive = false;
                status = _employmentRepository.Edit(employment);
            }
            catch (Exception exception)
            {
              throw new Exception(exception.Message);
            }

            return status;
        }

        public bool CheckExistingEmploymentInfo(Employment employment)
        {
            var isExist = false;
            try
            {
                isExist =
                    _employmentRepository.Exists(
                        x =>
                            (x.IsActive == true) &&
                            (x.Id != employment.Id) &&
                            (x.EmployeeId == employment.EmployeeId) &&
                            (x.CompanyName.Replace(" ", "").ToLower() == employment.CompanyName.Replace(" ", "").ToLower()) &&
                            (x.Department.Replace(" ", "").ToLower() == employment.Department.Replace(" ", "").ToLower()) &&
                            (x.Designation.Replace(" ", "").ToLower() == employment.Designation.Replace(" ", "").ToLower()));
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return isExist;
        }
    }
}
