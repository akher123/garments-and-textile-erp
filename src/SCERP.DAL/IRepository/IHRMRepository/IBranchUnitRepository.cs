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
    public interface IBranchUnitRepository:IRepository<BranchUnit>
    {
        List<BranchUnit> GetAllBranchUnit(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel, BranchUnit model);
        IEnumerable GetAllUnitsByCompanyId(int companyId);
    }
}
