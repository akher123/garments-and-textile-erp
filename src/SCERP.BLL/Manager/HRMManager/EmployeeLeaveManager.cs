using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.Model.HRMModel;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeLeaveManager : BaseManager, IEmployeeLeaveManager
    {

        private readonly IEmployeeLeaveRepository _employeeLeaveRepository = null;
        private readonly IAuthorizedPersonRepository _authorizedPersonRepository = null;

        public EmployeeLeaveManager(SCERPDBContext context)
        {
            this._employeeLeaveRepository = new EmployeeLeaveRepository(context);
            this._authorizedPersonRepository = new AuthorizedPersonRepository(context);
        }

        public List<EmployeeLeave> GetAllEmployeeLeavesByPaging(int startPage, int pageSize, EmployeeLeave employeeLeave, out int totalRecords)
        {
            List<EmployeeLeave> employeeLeaves = null;

            try
            {
                employeeLeaves = _employeeLeaveRepository.GetAllEmployeeLeavesByPaging(startPage, pageSize, out totalRecords, employeeLeave).ToList();
            }
            catch (Exception exception)
            {
                totalRecords = 0;

                Errorlog.WriteLog(exception);
            }

            return employeeLeaves;
        }

        public List<EmployeeLeave> GetAllRecommendedEmployeeLeavesByPaging(int startPage, int pageSize, out int totalRecords, EmployeeLeave employeeLeave)
        {
            List<EmployeeLeave> employeeLeaves = null;

            try
            {
                employeeLeaves = _employeeLeaveRepository.GetAllRecommendedEmployeeLeavesByPaging(startPage, pageSize, out totalRecords, employeeLeave).ToList();
            }
            catch (Exception exception)
            {
                totalRecords = 0;

                Errorlog.WriteLog(exception);
            }

            return employeeLeaves;
        }

        public List<EmployeeLeave> GetAllAppliedEmployeeLeavesByPaging(int startPage, int pageSize, EmployeeLeave employeeLeave, out int totalRecords)
        {
            List<EmployeeLeave> employeeLeaves = null;

            try
            {
                employeeLeaves = _employeeLeaveRepository.GetAllAppliedEmployeeLeavesByPaging(startPage, pageSize, out totalRecords, employeeLeave).ToList();
            }
            catch (Exception exception)
            {
                totalRecords = 0;

                Errorlog.WriteLog(exception);
            }

            return employeeLeaves;
        }

        public List<string> GetEmployeeData(string employeeCardId)
        {
            return _employeeLeaveRepository.GetEmployeeData(employeeCardId);
        }

        public List<EmployeeLeaveData> GetEmployeeLeaveData(string employeeCardId, int year)
        {
            return _employeeLeaveRepository.GetEmployeeLeaveData(employeeCardId, year);
        }

        public List<EmployeeLeave> GetAllEmployeeLeavesBySearchKey(string employeeCardId, DateTime fromDate, DateTime toDate)
        {
            var employeeLeaves = new List<EmployeeLeave>();

            try
            {
                employeeLeaves = _employeeLeaveRepository.GetAllEmployeeLeavesBySearchKey(employeeCardId, fromDate, toDate);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return employeeLeaves;
        }

        public List<Employee> GetAuthorizedPersons(int processKeyId, int autorizationId)
        {
            return _authorizedPersonRepository.GetAuthorizedPersons(processKeyId, autorizationId);
        }

        public bool CheckAuthorizedPerson(Guid? employeeId, int processKeyId, int authorizationId)
        {
            return _authorizedPersonRepository.CheckAuthorizedPerson(employeeId, processKeyId, authorizationId);
        }

        public IQueryable<LeaveType> GetAllLeaveType()
        {
            return _employeeLeaveRepository.GetAllLeaveType();
        }

        public EmployeeLeave GetEmployeeLeaveById(int? id)
        {
            EmployeeLeave employeeLeave = null;
            try
            {
                employeeLeave = _employeeLeaveRepository.GetEmployeeLeaveById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                employeeLeave = null;
            }

            return employeeLeave;
        }

        public List<EmployeeLeave> GetAllEmployeeLeaves()
        {
            List<EmployeeLeave> employeeLeaveList = null;

            try
            {
                employeeLeaveList = _employeeLeaveRepository.GetAllEmployeeLeaves();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                employeeLeaveList = null;
            }

            return employeeLeaveList;
        }

        public int SaveEmployeeLeave(EmployeeLeave employeeLeave)
        {
            var savedEmployeeLeave = 0;
            try
            {
                employeeLeave.CreatedDate = DateTime.Now;
                employeeLeave.CreatedBy = PortalContext.CurrentUser.UserId;
                employeeLeave.IsActive = true;
                //savedEmployeeLeave = _employeeLeaveRepository.Save(employeeLeave);
                savedEmployeeLeave = _employeeLeaveRepository.SaveEmployeeLeave(employeeLeave);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                savedEmployeeLeave = 0;
            }

            return savedEmployeeLeave;
        }

        public int EditEmployeeLeave(EmployeeLeave employeeLeave)
        {
            var editedEmployeeLeave = 0;
            try
            {
                employeeLeave.EditedDate = DateTime.Now;
                employeeLeave.EditedBy = PortalContext.CurrentUser.UserId;
                employeeLeave.IsActive = true;

                //editedEmployeeLeave = _employeeLeaveRepository.Edit(employeeLeave);
                editedEmployeeLeave = _employeeLeaveRepository.UpdateEmployeeLeave(employeeLeave);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                editedEmployeeLeave = 0;
            }

            return editedEmployeeLeave;
        }

        public int DeleteEmployeeLeave(EmployeeLeave employeeLeave)
        {
            var deletedEmployeeLeave = 0;
            try
            {
                employeeLeave.EditedDate = DateTime.Now;
                employeeLeave.EditedBy = PortalContext.CurrentUser.UserId;
                employeeLeave.IsActive = false;
                //deletedEmployeeLeave = _employeeLeaveRepository.Edit(employeeLeave);
                deletedEmployeeLeave = _employeeLeaveRepository.DeleteEmployeeLeave(employeeLeave);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                deletedEmployeeLeave = 0;
            }

            return deletedEmployeeLeave;
        }

        public bool CheckEmployeeLeaveExistence(EmployeeLeave employeeLeave)
        {
            bool isExist = false;
            isExist = _employeeLeaveRepository.CheckEmployeeLeaveExistence(employeeLeave);
            return isExist;
        }

        public bool CheckLeaveValidity(Guid employeeId, string employeeCardId, int year, int leaveTypeId, int appliedTotalDays)
        {
            return _employeeLeaveRepository.CheckLeaveValidity(employeeId, employeeCardId, year, leaveTypeId, appliedTotalDays);
        }

        public int SaveIndividualLeaveHistoryForSpecificYear(Guid employeeId, string employeeCardId, int year,
            int branchUnitId, int employeeTypeId)
        {
            return _employeeLeaveRepository.SaveIndividualLeaveHistoryForSpecificYear(employeeId, employeeCardId, year,
                branchUnitId, employeeTypeId);
        }

        public List<EmployeeLeaveHistoryIndividual> GetEmployeeLeaveSummaryIndividual(Guid employeeId, DateTime date)
        {
            return _employeeLeaveRepository.GetEmployeeLeaveSummaryIndividual(employeeId, date);
        }

        public List<EmployeeSalaryIndividual> GetEmployeeSalarySummaryIndividual(Guid employeeId, DateTime date)
        {
            return _employeeLeaveRepository.GetEmployeeSalarySummaryIndividual(employeeId, date);
        }

        public List<EmployeeAttendanceIndividual> GetEmployeeAttendanceIndividual(Guid employeeId, DateTime date)
        {
            return _employeeLeaveRepository.GetEmployeeAttendanceIndividual(employeeId, date);
        }

        public List<EmployeePenaltyIndividual> GetEmployeePenaltyIndividual(Guid employeeId, DateTime date)
        {
            return _employeeLeaveRepository.GetEmployeePenaltyIndividual(employeeId, date);
        }

        public List<EmployeeBasicInfo> GetEmployeeBasicInfo(string EmployeeCardId, DateTime date)
        {
            return _employeeLeaveRepository.GetEmployeeBasicInfo(EmployeeCardId, date);
        }
    }
}