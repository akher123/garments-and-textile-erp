using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.Common;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeDesignationManager : BaseManager, IEmployeeDesignationManager
    {

        private readonly IEmployeeDesignationRepository _employeeDesignationRepository = null;


        public EmployeeDesignationManager(SCERPDBContext context)
        {
            _employeeDesignationRepository = new EmployeeDesignationRepository(context);
        }

     
        public List<EmployeeDesignation> GetAllEmployeeDesignationsByPaging(int startPage, int pageSize, EmployeeDesignation employeeDesignation, out int totalRecords)
        {
            List<EmployeeDesignation> employeeDesignations = null;
            try
            {
                employeeDesignations = _employeeDesignationRepository.GetAllEmployeeDesignationsByPaging(startPage, pageSize, out totalRecords, employeeDesignation).ToList();
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                Errorlog.WriteLog(exception);
            }

            return employeeDesignations;
        }

        public List<EmployeeDesignation> GetAllEmployeeDesignation()
        {
            List<EmployeeDesignation> employeeDesignations = null;
            try
            {
                employeeDesignations = _employeeDesignationRepository.Filter(x => x.IsActive).Include(x => x.EmployeeType).Include(x => x.EmployeeGrade).OrderBy(x => x.Title).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return employeeDesignations;
        }

        public EmployeeDesignation GetEmployeeDesignationById(int? id)
        {
            EmployeeDesignation employeeDesignation = null;
            try
            {
                employeeDesignation = _employeeDesignationRepository.GetEmployeeDesignationById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return employeeDesignation;
        }
   
        public bool CheckExistingEmployeeDesignation(EmployeeDesignation employeeDesignation)
        {
            bool isExist = false;
            try
            {
                isExist =
                    _employeeDesignationRepository.Exists(
                        x =>
                            x.IsActive &&
                            x.Id != employeeDesignation.Id &&
                            x.EmployeeTypeId == employeeDesignation.EmployeeType.Id &&
                            x.GradeId == employeeDesignation.GradeId &&
                            x.Title.Replace(" ", "").ToLower().Equals(employeeDesignation.Title.Replace(" ", "").ToLower()));
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return isExist;
        }
             
        public List<EmployeeDesignation> GetAllEmployeeDesignationsBySearchKey(int searchByEmployeeTypeId, int searchByEmployeeGradeId, string searchByEmployeeDesignationTitle)
        {
            var employeeDesignations = new List<EmployeeDesignation>();

            try
            {
                employeeDesignations = _employeeDesignationRepository.GetAllEmployeeDesignationsBySearchKey(searchByEmployeeTypeId, searchByEmployeeGradeId, searchByEmployeeDesignationTitle);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return employeeDesignations;
        }

        public int SaveEmployeeDesignation(EmployeeDesignation employeeDesignation)
        {
            var savedEmployeeDesignation = 0;
            try
            {
                employeeDesignation.CreatedDate = DateTime.Now;
                employeeDesignation.CreatedBy = PortalContext.CurrentUser.UserId;
                employeeDesignation.IsActive = true;
                savedEmployeeDesignation = _employeeDesignationRepository.Save(employeeDesignation);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return savedEmployeeDesignation;
        }

        public int EditEmployeeDesignation(EmployeeDesignation employeeDesignation)
        {
            var editEmployeeDesignation = 0;
            try
            {
                employeeDesignation.EditedDate = DateTime.Now;
                employeeDesignation.EditedBy = PortalContext.CurrentUser.UserId;
                editEmployeeDesignation = _employeeDesignationRepository.Edit(employeeDesignation);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return editEmployeeDesignation;
        }

        public int DeleteEmployeeDesignation(Model.EmployeeDesignation employeeDesignation)
        {

            var deleteEmployeeDesignation = 0;
            try
            {
                employeeDesignation.IsActive = false;
                employeeDesignation.EditedDate = DateTime.Now;
                employeeDesignation.EditedBy = PortalContext.CurrentUser.UserId;

                deleteEmployeeDesignation = _employeeDesignationRepository.Edit(employeeDesignation);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return deleteEmployeeDesignation;
            
        }
         
        public List<EmployeeDesignation> GetEmployeeDesignationByEmployeeGrade(int? employeeGradeId)
        {
            List<EmployeeDesignation> employeeDesignations = null;
            try
            {
                employeeDesignations = _employeeDesignationRepository.GetEmployeeDesignationByEmployeeGrade(employeeGradeId).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return employeeDesignations;
        }


        public List<EmployeeDesignation> GetEmployeeDesignationByEmployeeType(int employeeTypeId)
        {
            List<EmployeeDesignation> employeeDesignations = null;
            try
            {
                employeeDesignations = _employeeDesignationRepository.GetEmployeeDesignationByEmployeeType(employeeTypeId).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return employeeDesignations;
        }

        public IEnumerable<EmployeeDesignationViewModel> GetRestEmployeeDesignations()
        {
            IEnumerable<EmployeeDesignationViewModel> employeeDesignations = null;

            try
            {
                employeeDesignations = _employeeDesignationRepository.GetRestEmployeeDesignations();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return employeeDesignations;
        }
    }
}
