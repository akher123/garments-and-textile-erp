using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.DAL;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeEducationManager : BaseManager, IEmployeeEducationManager
    {
        private readonly IEmployeeEducationRepository _employeeEducationRepository = null;
        private readonly IEducationLevelRepository _educationLevelRepository = null;

        public EmployeeEducationManager(SCERPDBContext context)
        {
            _employeeEducationRepository = new EmployeeEducationRepository(context);
            _educationLevelRepository = new EducationLevelRepository(context);
        }

        public List<EmployeeEducation> GetEmployeeEducationsByEmployeeId(Guid employeeId)
        {
            IQueryable<EmployeeEducation> employeeEducations;
            try
            {
                employeeEducations = _employeeEducationRepository.Filter(x => x.IsActive && x.EmployeeId == employeeId).Include(x => x.EducationLevel);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return employeeEducations.ToList();
        }

        public EmployeeEducation GetEmployeeEducationById(int id)
        {
            EmployeeEducation employeeEducation;

            try
            {
                employeeEducation = _employeeEducationRepository.FindOne(x => x.Id == id);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return employeeEducation;
        }

        public EmployeeEducation GetEmployeeEducationById(Guid? employeeId, int? id)
        {
            EmployeeEducation employeeEducation;

            try
            {
                employeeEducation = _employeeEducationRepository.FindOne(x => x.EmployeeId == employeeId && x.Id == id);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return employeeEducation;
        }

        public List<EducationLevel> GetAllEducationLevels()
        {
            IQueryable<EducationLevel> allEducationLevels;
            try
            {
                allEducationLevels = _educationLevelRepository.Filter(x => x.IsActive);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return allEducationLevels.ToList();
        }

        public int EditEmployeeEducation(EmployeeEducation employeeeducation)
        {
            var edit = 0;
            try
            {
                employeeeducation.EditedBy = PortalContext.CurrentUser.UserId;
                employeeeducation.EditedDate = DateTime.Now;
                edit = _employeeEducationRepository.Edit(employeeeducation);
            }
            catch (Exception)
            {

                edit = 0;
            }
            return edit;
        }

        public int SaveEmployeeEeducation(EmployeeEducation employeeeducation)
        {
            try
            {

                employeeeducation.CreatedBy = PortalContext.CurrentUser.UserId;
                employeeeducation.CreatedDate = DateTime.Now;
                employeeeducation.IsActive = true;
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return _employeeEducationRepository.Save(employeeeducation);
        }


        public int DeleteEmployeeEducation(EmployeeEducation employeeeducation)
        {
            var status = 1;
            try
            {

                employeeeducation.IsActive = false;
                _employeeEducationRepository.Edit(employeeeducation);
                return status;
            }
            catch (Exception)
            {
                status = 0;
                return status;
            }

        }

        public EducationLevel GeEmployeeEducationLevelById(int? id)
        {
            return _educationLevelRepository.GetById(id);
        }

        public bool CheckExistingEmployeeEducationInfo(EmployeeEducation employeeEducation)
        {
            var isExist = false;
            try
            {
                isExist =
                    _employeeEducationRepository.Exists(
                        x =>
                            (x.IsActive == true) &&
                            (x.Id != employeeEducation.Id) &&
                            (x.ExamTitle.Replace(" ", "").ToLower() == employeeEducation.ExamTitle.Replace(" ", "").ToLower()) &&
                            (x.EmployeeId == employeeEducation.EmployeeId));
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return isExist;
        }

    }
}
