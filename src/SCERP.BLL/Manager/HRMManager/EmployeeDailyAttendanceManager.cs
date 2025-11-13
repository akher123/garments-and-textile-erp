using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;
using System.Data.SqlClient;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeDailyAttendanceManager : BaseManager, IEmployeeDailyAttendanceManager
    {
        private readonly IEmployeeDailyAttendanceRepository _employeeDailyAttendanceRepository = null;
        readonly IEmployeeCompanyInfoRepository _employeeCompanyInfoRepository=null;
        private readonly IEmployeeRepository _employeeRepository = null;

        public EmployeeDailyAttendanceManager(SCERPDBContext context)
        {
            _employeeDailyAttendanceRepository=new EmployeeDailyAttendanceRepository(context);
            _employeeCompanyInfoRepository=new EmployeeCompanyInfoRepository(context);
            _employeeRepository = new EmployeeRepository(context);
        }

        public IList<VEmployeeDailyAttendanceDetail> GetEmployeeDailyAttendanceByPaging(int startPage, int pageSize, out int totalRecords,
            EmployeeDailyAttendance model, SearchFieldModel searchFieldModel)
        {
            List<VEmployeeDailyAttendanceDetail> employeeDailyAttendances;
            try
            {
                employeeDailyAttendances = _employeeDailyAttendanceRepository.GetEmployeeDailyAttendanceByPaging(startPage, pageSize, out totalRecords, model, searchFieldModel);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }
            return employeeDailyAttendances.ToList();
        }

        public VEmployeeCompanyInfoDetail GetEmployeeByEmployeeCardId(string employeeCardId)
        {
          return  _employeeCompanyInfoRepository.GetEmployeeCompanyInfoByEmployeeCardId(employeeCardId);
        }

        public int SaveEmployeeDailyAttendance(EmployeeDailyAttendance dailyAttendance)
        {
            int saveIndex=0;

            try
            {
                dailyAttendance.EmployeeCardId = dailyAttendance.EmployeeCardId.TrimStart('0');
                dailyAttendance.IsActive = true;
                dailyAttendance.IsFromMachine = false;
                dailyAttendance.CreatedDate = DateTime.Now;
                dailyAttendance.CreatedBy = PortalContext.CurrentUser.UserId;
                saveIndex = _employeeDailyAttendanceRepository.Save(dailyAttendance);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }
            return saveIndex;
        }

        public int SaveBulkAttendance(EmployeeDailyAttendance dailyAttendance)
        {
            int saveIndex = 0;

            try
            {
                dailyAttendance.EmployeeCardId = dailyAttendance.EmployeeCardId.TrimStart('0');
                dailyAttendance.IsActive = true;
                dailyAttendance.IsFromMachine = false;
                dailyAttendance.CreatedDate = DateTime.Now;
                dailyAttendance.CreatedBy = PortalContext.CurrentUser.UserId;
                saveIndex = _employeeDailyAttendanceRepository.Save(dailyAttendance);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }
            return saveIndex;
        }

        public EmployeeDailyAttendance GetEmployeeDailyAttendance(int employeeDailyAttendanceId)
        {
            EmployeeDailyAttendance employeeDailyAttendance;
            try
            {
                employeeDailyAttendance =
                    _employeeDailyAttendanceRepository.GetEmployeeDailyAttendance(employeeDailyAttendanceId);
            }
            catch (Exception exception)
            {
                
                throw new Exception(exception.Message,exception.InnerException);
            }
            return employeeDailyAttendance;
        }

        public int EditeEmployeeDailyAttendance(EmployeeDailyAttendance dailyAttendance)
        {
            var edit = 0;
            try
            {
                var employeeDailyAttendance =
                    _employeeDailyAttendanceRepository.FindOne(x => x.Id == dailyAttendance.Id);
                employeeDailyAttendance.EmployeeCardId = dailyAttendance.EmployeeCardId.TrimStart('0');
                employeeDailyAttendance.TransactionDateTime = dailyAttendance.TransactionDateTime;
                employeeDailyAttendance.IsFromMachine = dailyAttendance.IsFromMachine;
                employeeDailyAttendance.FunctionKey = dailyAttendance.FunctionKey;
                employeeDailyAttendance.Remarks = dailyAttendance.Remarks;
                employeeDailyAttendance.EditedDate = DateTime.Now;
                employeeDailyAttendance.EditedBy = PortalContext.CurrentUser.UserId;
               edit = _employeeDailyAttendanceRepository.Edit(employeeDailyAttendance);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }
            return edit;
        }

        public int EditBulkAttendance(EmployeeDailyAttendance dailyAttendance)
        {
            var edit = 0;
            try
            {
                var employeeDailyAttendance =
                    _employeeDailyAttendanceRepository.FindOne(x => x.Id == dailyAttendance.Id);
                employeeDailyAttendance.EmployeeCardId = dailyAttendance.EmployeeCardId.TrimStart('0');
                employeeDailyAttendance.TransactionDateTime = dailyAttendance.TransactionDateTime;
                employeeDailyAttendance.IsFromMachine = dailyAttendance.IsFromMachine;
                employeeDailyAttendance.FunctionKey = dailyAttendance.FunctionKey;
                employeeDailyAttendance.Remarks = dailyAttendance.Remarks;
                employeeDailyAttendance.EditedDate = DateTime.Now;
                employeeDailyAttendance.EditedBy = PortalContext.CurrentUser.UserId;
                edit = _employeeDailyAttendanceRepository.Edit(employeeDailyAttendance);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }
            return edit;
        }

        public int DeleteEmployeeDailyAttendance(int id)
        {
            var edit = 0;
            try
            {
                var employeeDailyAttendance =
                    _employeeDailyAttendanceRepository.FindOne(x => x.Id == id);
              
                employeeDailyAttendance.EditedDate = DateTime.Now;
                employeeDailyAttendance.EditedBy = PortalContext.CurrentUser.UserId;
                employeeDailyAttendance.IsActive = false;

                edit = _employeeDailyAttendanceRepository.Edit(employeeDailyAttendance);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }
            return edit;
        }

        public bool ImportMachineAttendanceData(DateTime? fromDate, DateTime? toDate)
        {
            bool importMachineAttendanceDataStatus;
            try
            {
                var startDate = new SqlParameter("@FromDate", fromDate);
                var endDate = new SqlParameter("@ToDate", toDate);
                var parameters = new object[] { startDate, endDate };
                importMachineAttendanceDataStatus = _employeeDailyAttendanceRepository.ImportMachineAttendanceData(parameters);
            }
            catch (Exception exception)
            {
                
                throw new Exception(exception.Message);
            }


            return importMachineAttendanceDataStatus;
        }

        public IList<VEmployeeCompanyInfoDetail> GetEmployees(int startPage, int pageSize, out int totalRecords,
         EmployeeDailyAttendance model, SearchFieldModel searchFieldModel)
        {
            List<VEmployeeCompanyInfoDetail> employees;
            try
            {
                employees = _employeeDailyAttendanceRepository.GetEmployes(startPage, pageSize, out totalRecords, model, searchFieldModel);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return employees;
        }

        public int SaveEmployeeDailyAttendances(List<EmployeeDailyAttendance> employeeDailyAttendances)
        {

            foreach (var employeeDailyAttendance in employeeDailyAttendances)
            {
                employeeDailyAttendance.EmployeeCardId =
                    _employeeRepository.GetEmployeeById(employeeDailyAttendance.EmployeeId).EmployeeCardId;
            }

            return _employeeDailyAttendanceRepository.SaveEmployeeDailyAttendances(employeeDailyAttendances);
        }


        public int ProcessEmployeeInOut(SearchFieldModel searchFieldModel)
        {
            return _employeeDailyAttendanceRepository.ProcessEmployeeInOut(searchFieldModel);
        }
    }
}
