using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Model.HRMModel;


namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeRepository:IRepository<Employee>
    {
        IQueryable<EmployeeViewModel> GetEmployees(int? employeeDepartment, int? employeeWorkGroup,
            int? employeeWorkShift, int? employeeGrade, int? employeeType, int? employeeDesignationId, string searchByEmployeeCardId,
            string searchByName, string searchByPhone, bool isNewSearch);


        int SaveEmployeeInfo(EmployeeViewModel employeeViewModel);

        Employee GetEmployeeById(Guid employeeIdGuid);

        int SaveEmployeeMandatoryInfo(SCERP.Model.Custom.EmployeeMandatoryInfoCustomModel employeeMandatoryInfoViewModel);

        //EmployeeMandatoryInfoViewModel GetEmployeeMandatoryInfoByEmployeeId(Guid employeeGuid);

        EmployeeMandatoryInfoCustomModel GetEmployeeMandatoryInfoByEmployeeId(Guid? employeeGuid,
            DateTime? effectiveDate);

        IQueryable<EmployeeViewModel> GetEmployees(int? employeeDepartment, int? employeeType,
            int? employeeDesignationId,
            string searchByEmployeeCardId, string searchByName, bool searchAll);

        Employee GetEmployeeByCardId(string employeeCardId);

        Employee GetAnyEmployeeByCardId(string employeeCardId);

        List<Employee> GetMerchandiser();

        List<EmployeeInfoCustomModel> GetAllEmployeeInfoByPaging(int startPage, int pageSize,
            SearchFieldModel searchFieldModel, EmployeeInfoCustomModel model, out int totalRecords);

        //List<SPGetAllEmployeeInfo_Result> SPGetAllEmployeeInfo(int startPage, int pageSize,
        //    SearchFieldModel searchFieldModel, EmployeeInfoCustomModel model, out int totalRecords);

        bool CheckExistingEmployeeCardNumber(Employee employee);

        List<SkillMatrixMachineType> GetAllMachineTypes();
        List<SkillMatrixDetailProcess> GetSkillMatrixDetailByMachineTypeId(int? machineTypeId, Guid employeeId);
        SkillMatrixPointTable GetAllSkillPoint(Guid employeeId);
        string SaveSkillMatrix(List<string> values, Guid employeeId, string employeeCardId);
        int SaveSkillMatrixPoint(SkillMatrixPointTable model);
    }
}
