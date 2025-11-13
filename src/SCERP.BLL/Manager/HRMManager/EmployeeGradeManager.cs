using System;
using System.Data.Entity;
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
    public class EmployeeGradeManager : BaseManager, IEmployeeGradeManager
    {
        private readonly IEmployeeGradeRepository _employeeGradeRepository = null;

        public EmployeeGradeManager(SCERPDBContext context)
        {
            _employeeGradeRepository = new EmployeeGradeRepository(context);
        }

        public List<EmployeeGrade> GetAllEmployeeGradesByPaging(int startPage, int pageSize, EmployeeGrade employeeGrade, out int totalRecords)
        {
            List<EmployeeGrade> employeeGrades = null;
            try
            {
                employeeGrades = _employeeGradeRepository.GetAllEmployeeGradesByPaging(startPage, pageSize, out totalRecords, employeeGrade).ToList();
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                Errorlog.WriteLog(exception);
            }

            return employeeGrades;
        }

        public EmployeeGrade GetEmployeeGradeById(int? id)
        {
            return _employeeGradeRepository.GetEmployeeGradeById(id);
        }

        public bool CheckExistingEmployeeGrade(EmployeeGrade employeeGrade)
        {
            bool isExist = false;
            try
            {
                isExist =
                    _employeeGradeRepository.Exists(
                        x =>
                            x.IsActive == true &&
                            x.Id != employeeGrade.Id &&
                            x.EmployeeTypeId == employeeGrade.EmployeeTypeId &&
                            x.Name.Replace(" ", "").ToLower().Equals(employeeGrade.Name.Replace(" ", "").ToLower()));
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return isExist;
        }

        public List<EmployeeGrade> GetAllEmployeeGradesBySearchKey(string name, int employeeTypeId)
        {
            var employeeGrades = new List<EmployeeGrade>();

            try
            {
                employeeGrades = _employeeGradeRepository.GetAllEmployeeGradesBySearchKey(name, employeeTypeId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return employeeGrades;
        }

        public int SaveEmployeeGrade(EmployeeGrade employeeGrade)
        {
            int savedEmployeeGrade= 0;
            try
            {
                employeeGrade.CreatedDate = DateTime.Now;
                employeeGrade.CreatedBy = PortalContext.CurrentUser.UserId;
                employeeGrade.IsActive = true;
                savedEmployeeGrade = _employeeGradeRepository.Save(employeeGrade);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                savedEmployeeGrade = 0;
            }

            return savedEmployeeGrade;
        }

        public int EditEmployeeGrade(EmployeeGrade employeeGrade)
        {
            var editedEmployeeGrade = 0;
            try
            {
                employeeGrade.EditedDate = DateTime.Now;
                employeeGrade.EditedBy = PortalContext.CurrentUser.UserId;
                editedEmployeeGrade = _employeeGradeRepository.Edit(employeeGrade);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                editedEmployeeGrade = 0;
            }

            return editedEmployeeGrade;
        }

      
        public int DeleteEmployeeGrade(EmployeeGrade employeeGrade)
        {

            int deletedEmployeeGrade = 0;
            try
            {
                employeeGrade.EditedDate = DateTime.Now;
                employeeGrade.EditedBy = PortalContext.CurrentUser.UserId;
                employeeGrade.IsActive = false;

                deletedEmployeeGrade = _employeeGradeRepository.Edit(employeeGrade);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                deletedEmployeeGrade = 0;
            }

            return deletedEmployeeGrade;
        }

      

        public EmployeeGrade GetEmployeeGradeDetail(int employeeGradeId)
        {
            return _employeeGradeRepository.FindOne(x => x.Id.Equals(employeeGradeId) && x.IsActive==true);
        }


        public List<EmployeeGrade> GetEmployeeGradeByEmployeeTypeId(int id)
        {
            List<EmployeeGrade> employeeGrades = null;
            try
            {
                employeeGrades = _employeeGradeRepository.GetGradeByEmployeeTypeId(id);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeeGrades;
        }

        public List<EmployeeGrade> GetAllEmployeeGrades()
        {
            List<EmployeeGrade> employeeGrades = null;
            try
            {
                employeeGrades = _employeeGradeRepository.All().Where(x => x.IsActive == true).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeeGrades;

        }

    }
}
