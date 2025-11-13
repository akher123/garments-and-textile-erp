using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
   public interface IEmployeeDesignationRepository:IRepository<EmployeeDesignation>
    {
       EmployeeDesignation GetEmployeeDesignationById(int? id);
       IQueryable<EmployeeDesignation> GetEmployeeDesignationByEmployeeGrade(int? employeeGradeId);
       IEnumerable<EmployeeDesignationViewModel> GetRestEmployeeDesignations();

       List<EmployeeDesignation> GetAllEmployeeDesignationsByPaging(int startPage, int pageSize, out int totalRecords,
           EmployeeDesignation employeeDesignation);

       List<EmployeeDesignation> GetAllEmployeeDesignationsBySearchKey(int searchByEmployeeTypeId,
           int searchByEmployeeGradeId, string searchByEmployeeDesignationTitle);

       IQueryable<EmployeeDesignation> GetEmployeeDesignationByEmployeeType(int employeeTypeId);
    }
}
