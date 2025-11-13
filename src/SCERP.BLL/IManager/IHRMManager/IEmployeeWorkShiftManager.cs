using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
   public interface IEmployeeWorkShiftManager
   {
      
       int SaveEmployeeWorkShifts(List<EmployeeWorkShift> employeeWorkShifts);
       EmployeeWorkShift GetEmployeeWorkshiftById(int employeeWorkShiftId);

       int ChangeEmployeeWorkShifts(EmployeeWorkShift model);
       List<VEmployeeWorkShiftDetail> GetAllAssignedEmployeeWorkShift(int startPage, int pageSize, out int totalRecords, EmployeeWorkShift model, SearchFieldModel searchFieldModel);
       int DeleteEmployeeWorkShift(int? id);
       VEmployeeWorkShiftDetail GetEmployeeWorkshiftDetailById(int employeeWorkShiftId);
       List<VEmployeeWorkShiftDetail> GetEmployeeWorkShiftDetailBySearchKey(SearchFieldModel searchField);

       bool CheckEmployeeExistingWorkShift(EmployeeWorkShift model);

       List<EmployeesForWorkShiftCustomModel> GetEmployeesForWorkShift(EmployeeWorkShift model,
           SearchFieldModel searchFieldModel);

       int SaveNewJoiningEmployeeWorkShift(Guid employeeId, DateTime? joiningDate, int branchUnitId);
        int UpdateWorkShiftQuick(List<int> workShiftList, int searchByBranchUnitWorkShiftId);
    }
}
