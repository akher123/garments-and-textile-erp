using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeWorkShiftRepository : IRepository<EmployeeWorkShift>
    {

        List<VEmployeeWorkShiftDetail> GetAllAssignedEmployeeWorkShift(int startPage, int pageSize, out int totalRecords, EmployeeWorkShift model, SearchFieldModel searchFieldModel);


        int SaveEmployeeWorkShifts(List<EmployeeWorkShift> employeeWorkShifts);
        EmployeeWorkShift GetEmployeeWorkshiftById
            (int employeeWorkShiftId);

        VEmployeeWorkShiftDetail GetEmployeeWorkshiftDetailById
            (int employeeWorkShiftId);

        List<VEmployeeWorkShiftDetail> GetEmployeeWorkShiftDetailBySearchKey(SearchFieldModel searchField);

        List<EmployeesForWorkShiftCustomModel> GetEmployeesForWorkShift(EmployeeWorkShift model,
            SearchFieldModel searchFieldModel);

        int SaveNewJoiningEmployeeWorkShift(Guid employeeId, DateTime? joiningDate, int branchUnitId);
        int UpdateWorkShiftQuick(List<int> workShiftList, int searchByBranchUnitWorkShiftId);
    }
}
