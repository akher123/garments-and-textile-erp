using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Util;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.HRMManager
{
    public class OvertimeEligibleEmployeeManager : IOvertimeEligibleEmployeeManager
    {
        private readonly IOvertimeEligibleEmployeeRepository _overtimeEligibleEmployeeRepository = null;

        public OvertimeEligibleEmployeeManager(SCERPDBContext context)
        {
            _overtimeEligibleEmployeeRepository = new OvertimeEligibleEmployeeRepository(context);
        }

        public List<VOvertimeEligibleEmployeeDetail> GetOvertimeEligibleEmployeeByPaging(int startPage, int pageSize, out int totalRecords,
            OvertimeEligibleEmployee model, SearchFieldModel searchFieldModel)
        {
            List<VOvertimeEligibleEmployeeDetail> overtimeEligibleEmployees;
            try
            {
                overtimeEligibleEmployees = _overtimeEligibleEmployeeRepository.GetOvertimeEligibleEmployeeByPaging(startPage, pageSize, out totalRecords, model, searchFieldModel);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }
            return overtimeEligibleEmployees;
        }

        public int SaveOvertimeEligibleEmployee(List<OvertimeEligibleEmployee> overtimeEligibleEmployees)
        {
            var saveIndex = 0;
            try
            {
                saveIndex = _overtimeEligibleEmployeeRepository.SaveOvertimeEligibleEmployee(overtimeEligibleEmployees);
            }
            catch (Exception exception)
            {

                throw;
            }
            return saveIndex;
        }

        public OvertimeEligibleEmployee GetOvertimeEligibleEmployee(int overtimeEligibleEmployeeId)
        {
            OvertimeEligibleEmployee overtimeEligibleEmployee;
            try
            {
                overtimeEligibleEmployee = _overtimeEligibleEmployeeRepository.FindOne(x => x.OvertimeEligibleEmployeeId == overtimeEligibleEmployeeId && x.IsActive && x.Status);
            }
            catch (Exception exception)
            {
                throw;
            }
            return overtimeEligibleEmployee;
        }



        public OvertimeEligibleEmployee GetOvertimeEligibleEmployeeByEmpIdAndDate(OvertimeEligibleEmployee overtimeEligibleEmployeepar)
        {
            OvertimeEligibleEmployee overtimeEligibleEmployee;
            try
            {
                overtimeEligibleEmployee = _overtimeEligibleEmployeeRepository.FindOne(x => x.EmployeeId == overtimeEligibleEmployeepar.EmployeeId && x.IsActive && x.Status && x.OvertimeDate==overtimeEligibleEmployeepar.OvertimeDate);
            }
            catch (Exception exception)
            {
                throw;
            }
            return overtimeEligibleEmployee;
        }

        public List<OvertimeEligibleEmployee> GetOvertimeEligibleEmployeeListByEmpIdAndDate(OvertimeEligibleEmployee overtimeEligibleEmployeepar)
        {
            List<OvertimeEligibleEmployee> overtimeEligibleEmployee;
            try
            {
                overtimeEligibleEmployee = _overtimeEligibleEmployeeRepository.GetOvertimeEligibleEmployeeListBySearchKey(overtimeEligibleEmployeepar);
            }
            catch (Exception exception)
            {
                throw;
            }
            return overtimeEligibleEmployee;
        }
        public int EditOvertimeEligibleEmployee(OvertimeEligibleEmployee overtimeEligibleEmployee)
        {
            var saveIndex = 0;
            try
            {
                bool isExist =
                    _overtimeEligibleEmployeeRepository.Exists(
                        x =>
                            x.OvertimeEligibleEmployeeId == overtimeEligibleEmployee.OvertimeEligibleEmployeeId &&
                            x.OvertimeDate == overtimeEligibleEmployee.OvertimeDate &&
                            x.OvertimeHour == overtimeEligibleEmployee.OvertimeHour &&
                            x.EmployeeId == overtimeEligibleEmployee.EmployeeId &&
                            x.Remarks == overtimeEligibleEmployee.Remarks);
                var overTimeEligibleEmployeeObj =
                    GetOvertimeEligibleEmployee(overtimeEligibleEmployee.OvertimeEligibleEmployeeId);
                overTimeEligibleEmployeeObj.EditedBy = PortalContext.CurrentUser.UserId;
                overTimeEligibleEmployeeObj.EditedDate = DateTime.Now;
                if (isExist)
                {
                    overTimeEligibleEmployeeObj.Status = true;
                    saveIndex = _overtimeEligibleEmployeeRepository.Edit(overTimeEligibleEmployeeObj);
                }
                else
                {
                    overTimeEligibleEmployeeObj.Status = false;
                    saveIndex = _overtimeEligibleEmployeeRepository.Edit(overTimeEligibleEmployeeObj);

                    if (saveIndex > 0)
                    {
                        overtimeEligibleEmployee.Status = true;
                        overtimeEligibleEmployee.CreatedBy = PortalContext.CurrentUser.UserId;
                        overtimeEligibleEmployee.CreatedDate = DateTime.Now;
                        overtimeEligibleEmployee.OvertimeEligibleEmployeeId = 0;
                        saveIndex += _overtimeEligibleEmployeeRepository.Save(overtimeEligibleEmployee);
                    }
                }
            }
            catch (Exception exception)
            {
                throw;
            }
            return saveIndex;
        }

        public List<VOvertimeEligibleEmployeeDetail> GetOvertimeEligibleEmployeeBySearchKey(SearchFieldModel searchField)
        {
            List<VOvertimeEligibleEmployeeDetail> overtimeEligibleEmployees;
            try
            {
                overtimeEligibleEmployees = _overtimeEligibleEmployeeRepository.GetOvertimeEligibleEmployeeBySearchKey(searchField);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }
            return overtimeEligibleEmployees;
        }

        public IList<VEmployeeCompanyInfoDetail> GetEmployes(int startPage, int pageSize, out int totalRecords,
            OvertimeEligibleEmployee model, SearchFieldModel searchFieldModel)
        {
            List<VEmployeeCompanyInfoDetail> employees;
            try
            {
                employees = _overtimeEligibleEmployeeRepository.GetEmployes(startPage, pageSize, out totalRecords, model, searchFieldModel);

            }
            catch (Exception)
            {

                throw;
            }
            return employees;
        }

        public int DeleteOvertimeEligibleEmployee(OvertimeEligibleEmployee overtimeEligibleEmployee)
        {
            int deleteIndex;
            try
            {
                deleteIndex = _overtimeEligibleEmployeeRepository.DeleteOvertimeEligibleEmployee(overtimeEligibleEmployee);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return deleteIndex;
        }

        public int SaveNewEmployeeOverTime(Guid employeeId, DateTime joinDate)
        {
            DateTime endDate;
            int day = joinDate.Day;
            int month = joinDate.Month;
            var saveIndex = 0;

            if (day >= 26 && day <= 31 && month != 12)
                endDate = Convert.ToDateTime(joinDate.Year + "/" + (joinDate.Month + 1) + "/25");
            else if (day >= 26 && day <= 31 && month == 12)
                endDate = Convert.ToDateTime((joinDate.Year + 1) + "/" + "01" + "/25");
            else
                endDate = Convert.ToDateTime(joinDate.Year + "/" + joinDate.Month + "/25");

            List<OvertimeEligibleEmployee> overtimeEligibleEmployeeList = new List<OvertimeEligibleEmployee>();

            while (joinDate <= endDate)
            {
                var overTime = new OvertimeEligibleEmployee
                {
                    EmployeeId = employeeId,
                    OvertimeDate = joinDate,
                    OvertimeHour = 2,             
                    CreatedDate = DateTime.Now,
                    CreatedBy = PortalContext.CurrentUser.UserId,
                    IsActive = true,
                    Status = true
                };

                overtimeEligibleEmployeeList.Add(overTime);
                joinDate = joinDate.AddDays(1);
            }

            saveIndex = _overtimeEligibleEmployeeRepository.SaveOvertimeEligibleEmployee(overtimeEligibleEmployeeList);
            return saveIndex;
        }
    }
}
