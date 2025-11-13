using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.HRMModel;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeLeaveRepository : IRepository<EmployeeLeave>
    {
        EmployeeLeave GetEmployeeLeaveById(int? id);
        List<EmployeeLeave> GetAllAppliedEmployeeLeavesByPaging(int startPage, int pageSize, out int totalRecords,EmployeeLeave employeeLeave);
        List<EmployeeLeave> GetAllEmployeeLeavesByPaging(int startPage, int pageSize, out int totalRecords, EmployeeLeave employeeLeave);
        List<EmployeeLeave> GetAllRecommendedEmployeeLeavesByPaging(int startPage, int pageSize, out int totalRecords, EmployeeLeave employeeLeave);
        List<EmployeeLeave> GetAllEmployeeLeaves();
        List<EmployeeLeave> GetAllEmployeeLeavesBySearchKey(string employeeCardId, DateTime fromDate, DateTime toDate);
        IQueryable<LeaveType> GetAllLeaveType();
        List<string> GetEmployeeData(string employeeCardId);

        //IQueryable<EmployeeLeaveData> GetEmployeeLeaveData(string employeeCardId);
       
        //bool CheckLeaveValidity(int? typeId, int? totalDays, Guid? employeeId);

        bool CheckLeaveValidity(Guid employeeId, string employeeCardId, int year, int leaveTypeId, int appliedTotalDays);

        int SaveEmployeeLeave(EmployeeLeave employeeLeave);

        int UpdateEmployeeLeave(EmployeeLeave employeeLeave);

        int DeleteEmployeeLeave(EmployeeLeave employeeLeave);

        List<EmployeeLeaveData> GetEmployeeLeaveData(string employeeCardId, int year);

        int SaveIndividualLeaveHistoryForSpecificYear(Guid employeeId, string employeeCardId, int year, int branchUnitId, int employeeTypeId);

        bool CheckEmployeeLeaveExistence(EmployeeLeave employeeLeave);

        List<EmployeeLeaveHistoryIndividual> GetEmployeeLeaveSummaryIndividual(Guid employeeId, DateTime date);

        List<EmployeeSalaryIndividual> GetEmployeeSalarySummaryIndividual(Guid employeeId, DateTime date);

        List<EmployeeAttendanceIndividual> GetEmployeeAttendanceIndividual(Guid employeeId, DateTime date);

        List<EmployeePenaltyIndividual> GetEmployeePenaltyIndividual(Guid employeeId, DateTime date);

        List<EmployeeBasicInfo> GetEmployeeBasicInfo(string EmployeeCardId, DateTime date);
    }
}