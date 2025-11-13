
using System.Collections.Generic;
using System.Linq;
using SCERP.Model;
using System;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeShortLeaveManager
    {
        List<VEmployeeShortLeave> GetAllEmployeeShortLeavesByPaging(int startPage, int pageSize, out int totalRecords, ShortLeaveModel employeeShortLeave);
        EmployeeShortLeave GetEmployeeShortLeaveById(int? id);
        int SaveShortLeave(EmployeeShortLeave shortLeave);
        int EditShortLeave(EmployeeShortLeave shortLeave);
        int DeleteShortLeave(EmployeeShortLeave shortLeave);
    }
}
