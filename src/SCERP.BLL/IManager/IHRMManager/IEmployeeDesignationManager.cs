using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
   public interface IEmployeeDesignationManager
   {
       List<EmployeeDesignation> GetAllEmployeeDesignationsByPaging(int startPage, int pageSize,
           EmployeeDesignation employeeDesignation, out int totalRecords);

       int SaveEmployeeDesignation(EmployeeDesignation employeeDesignation);

       List<EmployeeDesignation> GetAllEmployeeDesignation();

       EmployeeDesignation GetEmployeeDesignationById(int? id);

       int EditEmployeeDesignation(EmployeeDesignation employeeDesignation);

       int DeleteEmployeeDesignation(EmployeeDesignation employeeDesignation);

       List<EmployeeDesignation> GetEmployeeDesignationByEmployeeGrade(int? employeeGradeId);

       IEnumerable<EmployeeDesignationViewModel> GetRestEmployeeDesignations();

       bool CheckExistingEmployeeDesignation(EmployeeDesignation employeeDesignation);

       List<EmployeeDesignation> GetAllEmployeeDesignationsBySearchKey(int searchByEmployeeTypeId,
           int searchByEmployeeGradeId, string searchByEmployeeDesignationTitle);

       List<EmployeeDesignation> GetEmployeeDesignationByEmployeeType(int employeeTypeId);
   }
}
