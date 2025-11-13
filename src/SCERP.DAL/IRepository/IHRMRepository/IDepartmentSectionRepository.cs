using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IDepartmentSectionRepository:IRepository<DepartmentSection>
    {
        List<DepartmentSection> GetDepartmentSectionByPaging(int startPage, int pageSize, out int totalRecords, DepartmentSection model, SearchFieldModel searchFieldModel);
        List<DepartmentSection> GetDepartmentSectionBySearchKey(SearchFieldModel searchFieldModel);
        DepartmentSection GetDepartmentSectionById(int departmentSectionId);
        List<DepartmentSection> GetDepartmentSectionByBranchUnitDepartmentId
            (int branchUnitDepartmentId);
    }
}
