using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeAddressInfoManager
    {
        EmployeeAddressInfoCustomModel GetEmployeeAddressInfoByEmployeeGuidId(Guid employeeId);

        EmployeePresentAddress GetEmployeePresentAddressById(Guid employeeGuid, int id);

        EmployeePermanentAddress GetEmployeePermanentAddressById(Guid employeeGuid, int id);

        int SaveEmployeePresentAddress(EmployeePresentAddress employeePresentAddress);

        int EditEmployeePresentAddress(EmployeePresentAddress employeePresentAddress);

        EmployeePresentAddress GetLatestEmployeePresentAddressByEmployeeGuidId(Guid employeeId);

        int UpdateEmployeePresentAddress(EmployeePresentAddress employeePresentAddress);

        int DeleteEmployeePresentAddress(EmployeePresentAddress employeePresentAddress);


        int SaveEmployeePermanentAddress(EmployeePermanentAddress employeePermanentAddress);

        int EditEmployeePermanentAddress(EmployeePermanentAddress employeePermanentAddress);

        EmployeePermanentAddress GetLatestEmployeePermanentAddressByEmployeeGuidId(Guid employeeId);

        int UpdateEmployeePermanentAddress(EmployeePermanentAddress employeePermanentAddress);

        int DeleteEmployeePermanentAddress(EmployeePermanentAddress employeePermanentAddress);

    }
}
