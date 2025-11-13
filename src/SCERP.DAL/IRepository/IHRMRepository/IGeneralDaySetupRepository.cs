using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IGeneralDaySetupRepository : IRepository<GeneralDaySetup>
    {
        List<GeneralDaySetup> GetGeneralDaySetup(int startPage, int pageSize, out int totalRecords, GeneralDaySetup model, SearchFieldModel searchFieldModel);
        GeneralDaySetup GetGeneralDaySetupById(int generalDaySetupId);
        List<GeneralDaySetup> GetGeneralDaySetupByBranchUnitDepartmentId(int branchUnitDepartmentId);
        bool CheckExistingGeneralDaySetup(DateTime? declaredDate);
    }
}
