using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeFamilyInfoManager
    {
        List<EmployeeFamilyInfo> GetEmployeeFamilyInfoByEmployeeGuidId(Guid employeeId);

        int SaveEmployeeFamilyInfo(EmployeeFamilyInfo employeeFamilyInfo);

        int EditEmployeeFamilyInfo(EmployeeFamilyInfo employeeFamilyInfo);

        int DeleteEmployeeFamilyInfo(EmployeeFamilyInfo employeeFamilyInfo);

        EmployeeFamilyInfo GetEmployeeFamilyInfoById(Guid employeeId, int id);

        bool CheckExistingEmployeeFamilyInfo(EmployeeFamilyInfo employeeFamilyInfo);

    }
}
