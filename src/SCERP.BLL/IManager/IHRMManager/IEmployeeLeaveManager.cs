
using System.Collections.Generic;
using System.Linq;
using SCERP.Model;
using System;
using SCERP.Model.HRMModel;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeLeaveManager
    {
        List<EmployeeLeave> GetAllAppliedEmployeeLeavesByPaging(int startPage, int pageSize, EmployeeLeave employeeLeave,
            out int totalRecords);
        List<EmployeeLeave> GetAllEmployeeLeavesByPaging(int startPage, int pageSize, EmployeeLeave employeeLeave, out int totalRecords);
        List<EmployeeLeave> GetAllRecommendedEmployeeLeavesByPaging(int startPage, int pageSize, out int totalRecords, EmployeeLeave employeeLeave);
        
        List<EmployeeLeave> GetAllEmployeeLeaves();

        EmployeeLeave GetEmployeeLeaveById(int? id);

        int SaveEmployeeLeave(EmployeeLeave EmployeeLeave);

        int EditEmployeeLeave(EmployeeLeave EmployeeLeave);

        int DeleteEmployeeLeave(EmployeeLeave EmployeeLeave);

        List<EmployeeLeave> GetAllEmployeeLeavesBySearchKey(string employeeCardId, DateTime fromDate, DateTime toDate);

        bool CheckEmployeeLeaveExistence(EmployeeLeave employeeLeave);
       
        //List<Employee> GetAuthorizedPersons(int type);

        List<Employee> GetAuthorizedPersons(int processKeyId, int authorizationId);
        
        //bool CheckAuthorizedPerson(Guid? employeeId);
        //bool CheckLeaveApprovalPerson(Guid? employeeId);
        //bool CheckAuthorizedPerson(Guid? employeeId, int authorizationId);

        bool CheckAuthorizedPerson(Guid? employeeId, int processKeyId, int authorizationId);

        IQueryable<LeaveType> GetAllLeaveType();
        List<string> GetEmployeeData(string employeeCardId);
        //IQueryable<EmployeeLeaveData> GetEmployeeLeaveData(string employeeCardId);
        //bool CheckLeaveValidity(int? typeId, int? totalDays, Guid? employeeId);
        bool CheckLeaveValidity(Guid employeeId, string employeeCardId, int year, int leaveTypeId, int appliedTotalDays);
        List<EmployeeLeaveData> GetEmployeeLeaveData(string employeeCardId, int year);

        int SaveIndividualLeaveHistoryForSpecificYear(Guid employeeId, string employeeCardId, int year, int branchUnitId, int employeeTypeId);

        List<EmployeeLeaveHistoryIndividual> GetEmployeeLeaveSummaryIndividual(Guid employeeId, DateTime date);

        List<EmployeeSalaryIndividual> GetEmployeeSalarySummaryIndividual(Guid employeeId, DateTime date);

        List<EmployeeAttendanceIndividual> GetEmployeeAttendanceIndividual(Guid employeeId, DateTime date);

        List<EmployeePenaltyIndividual> GetEmployeePenaltyIndividual(Guid employeeId, DateTime date);

        List<EmployeeBasicInfo> GetEmployeeBasicInfo(string EmployeeCardId, DateTime date);
    }
}