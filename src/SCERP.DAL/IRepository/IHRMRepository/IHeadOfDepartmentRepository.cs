using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
   public interface IHeadOfDepartmentRepository:IRepository<HeadOfDepartment>
   {
       //List<HeadOfDepartment> GetAllHeadOfDepartments(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel, HeadOfDepartment model);

       //HeadOfDepartment GetHeadOfDepartmentsById(int branchUnitDepartmentId);

       //List<HeadOfDepartment> GetHeadOfDepartmentsBySearchKey(SearchFieldModel searchFieldModel);

   }
}
