using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;


namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface ISalarySetupRepository : IRepository<SalarySetup>
    {
        List<SalarySetup> GetAllSalarySetup(int page, int records, string sort, int? GradeId);
        IQueryable<EmployeeType> GetEmployeeTypes();
        List<EmployeeGrade> GetEmpGradeByEmpType(int employeeTypeId);
        SalarySetup GetSalarySetupById(int? id);

        List<SalarySetup> GetAllSalarySetupByPaging(int startPage, int pageSize, SalarySetup SalarySetup,
                out int totalRecords);

        List<SalarySetup> GetAllSalarySetupBySearchKey(SalarySetup salarySetup);

        SalarySetup GetLatestSalarySetupInfoByGrade(SalarySetup salarySetup);

        SalarySetup GetSalarySetupByEmployeeGrade(int employeeGradeId, DateTime? effectiveDate);

    }
}
