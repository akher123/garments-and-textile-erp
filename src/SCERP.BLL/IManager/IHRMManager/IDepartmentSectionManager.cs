using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
   public interface IDepartmentSectionManager
    {
        List<Model.DepartmentSection> GetDepartmentSectionByPaging(int startPage, int _pageSize, out int totalRecords, DepartmentSection model, Model.Custom.SearchFieldModel searchFieldModel);
       DepartmentSection GetDepartmentSectionById(int departmentSectionId);
       bool IsExistDepartmentSection(DepartmentSection model);
       int EditDepartmentSection(DepartmentSection model);
       int SaveDepartmentSection(DepartmentSection model);
       List<DepartmentSection> GetDepartmentSectionBySearchKey(SearchFieldModel searchFieldModel);
       int DeleteDepartmentSection(int departmentSectionId);
       List<DepartmentSection> GetDepartmentSectionByBranchUnitDepartmentId(int searchByBranchUnitDepartmentId);
    }
}
