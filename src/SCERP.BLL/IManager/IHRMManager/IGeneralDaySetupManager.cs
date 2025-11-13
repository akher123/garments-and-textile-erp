using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IGeneralDaySetupManager
    {
        List<GeneralDaySetup> GetGeneralDaySetup(int startPage, int pageSize, out int totalRecords, GeneralDaySetup model, SearchFieldModel searchFieldModel);
        int DeleteGeneralDaySetup(int generalDaySetupId);
        GeneralDaySetup GetGeneralDaySetupById(int generalDaySetupId);
        int EditGeneralDaySetup(GeneralDaySetup model);
        int SaveGeneralDaySetup(GeneralDaySetup model);
        List<GeneralDaySetup> GetGeneralDaySetupByBranchUnitDepartmentId(int branchUnitDepartmentId);
        bool CheckExistingGeneralDaySetup(DateTime? declaredDate);
    }
}
