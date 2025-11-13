using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface ISalarySetupManager
    {

        List<SalarySetup> GetAllSalarySetup(int page, int records, string sort, int? GradeId);

        IQueryable<EmployeeType> GetEmployeeTypes();
  
        List<EmployeeGrade> GetEmpGradeByEmpType(int employeeTypeId);

        SalarySetup GetSalarySetupById(int? id);

        int SaveSalarySetup(SalarySetup salary);

        int DeleteSalarySetup(SalarySetup salary);
        List<SalarySetup> GetAllSalarySetupesByPaging(int startPage, int pageSize, out int totalRecords,  SalarySetup SalarySetup);


        int EditSalarySetup(SalarySetup model);

        List<SalarySetup> GetAllSalarySetupNewBySearchKey(SalarySetup salarySetup);

        SalarySetup GetLatestSalarySetupInfoByGrade(SalarySetup salarySetup);

        int UpdateSalarySetupInfoDate(SalarySetup salarySetupNew);

        bool CheckMinimumSalary(EmployeeSalary employeeSalary, SalarySetup salarySetupNew);

        bool CheckSumOfAllSalary(EmployeeSalary employeeSalary);

        SalarySetup GetSalarySetupByEmployeeGrade(int employeeGradeId, DateTime? effectiveDate);

    }
}
