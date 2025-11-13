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
   public interface IBranchUnitDepartmentRepository:IRepository<BranchUnitDepartment>
   {
       List<BranchUnitDepartment> GetAllBranchUnitDepartment(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel, BranchUnitDepartment model);

       BranchUnitDepartment GetBranchUnitDepartmentById(int branchUnitDepartmentId);
       List<BranchUnitDepartment> GetBranchUnitDepartmentBySearchKey(SearchFieldModel searchFieldModel);
       List<Unit> GetUnitsByBranchId(int searchByBranchId);
       List<Department> GetDepartmentsByBranchId(int searchByUnitId);


       List<BranchUnitDepartment> GetBranchUnitDepartment();
   
   }
}
