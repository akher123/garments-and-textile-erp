using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeEducationManager
    {
        List<EmployeeEducation> GetEmployeeEducationsByEmployeeId(Guid employeeId);

        EmployeeEducation GetEmployeeEducationById(int id);

        List<EducationLevel> GetAllEducationLevels();

        int SaveEmployeeEeducation(EmployeeEducation employeeeducation);

        int EditEmployeeEducation(EmployeeEducation employeeeducation);

        int DeleteEmployeeEducation(EmployeeEducation employeeeducation);

        EducationLevel GeEmployeeEducationLevelById(int? id);

        EmployeeEducation GetEmployeeEducationById(Guid? employeeId, int? id);

        bool CheckExistingEmployeeEducationInfo(EmployeeEducation employeeEducation);
    }
}
