using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Model.HRMModel;


namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeManager
    {
        IQueryable<Department> GetAllDepartments();

        Guid SaveEmployeeInfo(EmployeeViewModel employeeViewModel);

        Employee GetEmployeeById(Guid employeeIdGuid);

        int EditEmployeePersonalInfo(Employee employee);

        int EditEmployeeDepartmentalInfo(EmployeeCompanyInfo employeeCompanyInfo);

        Guid SaveEmployeeMandatoryInfo(EmployeeMandatoryInfoCustomModel employeeMandatoryInfoViewModel);

        EmployeePresentAddress GetEmployeePresentAddressById(Guid employeeIdGuid);

        EmployeePermanentAddress GetEmployeePermanentAddressById(Guid employeeIdGuid);

        int EditEmployeeAddressInfo(EmployeeAddressViewModel employeeAddressViewModel);

        EmployeeMandatoryInfoCustomModel GetEmployeeMandatoryInfoByEmployeeId(Guid? employeeId,DateTime? effectiveDate);

        int EditEmployeeMandatoryInfo(SCERP.Model.Custom.EmployeeMandatoryInfoCustomModel employeeMandatoryInfoViewModel);

        int DeleteEmployee(Guid employeeGuid);

        int EditEmployeePhoto(EmployeeMandatoryInfoViewModel employeeMandatoryInfoViewModel);

        List<EmployeeDesignation> GetDesignationByGradeId(int id);

        List<EmployeeGrade> GetGetGradeByGradeId(int id);

        Employee GetEmployeeByCardId(string employeeCardId);

        Employee GetAnyEmployeeByCardId(string employeeCardId);

        List<Employee> GetAallEmployeeByDepartmentId(int departmentId);

        bool CheckExistingEmployeeCardNumber(Employee employee);

        List<EmployeeInfoCustomModel> GetAllEmployeeInfoByPaging(int startPage, int pageSize, SearchFieldModel searchFieldModel, EmployeeInfoCustomModel model, out int totalRecords);

        int EditEmployeeQuitInfo(Employee employee);

        object GetEmployeesByCardIdAndName(string serachString);
        string GetEmployeeNameByEmployeeId(Guid employeeId);
        Guid GetEmployeeIdByEmployeeCardId(string employeeCardId);
        object GetEmployeesBySearchCharacter(string searchCharacter);

        List<SkillMatrixMachineType> GetAllMachineTypes();
        List<SkillMatrixDetailProcess> GetSkillMatrixDetailByMachineTypeId(int? machineTypeId, Guid employeeId);
        SkillMatrixPointTable GetAllSkillPoint(Guid employeeId);
        string SaveSkillMatrix(List<string> values, Guid employeeId, string employeeCardId);
        int SaveSkillMatrixPoint(SkillMatrixPointTable model);
        string GetEmployeeImageUrlById(Guid? userId);
    }
}
