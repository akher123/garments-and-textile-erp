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
    public interface IUnitManager
    {
        List<Unit> GetAllUnits(int startPage, int pageSize, Unit unit, out int totalRecords);
        Unit GetUnitById(int unitId);
        bool IsExistUnit(Unit model);
        int EditUnit(Unit model);
        int SaveUnit(Unit model);
        int DeleteUnit(int unitId);
        List<Unit> GetAllUnitsBySearchKey(Unit model);
        List<Unit> GetAllUnits();
        IEnumerable GetAllUnitsByCompanyId(int companyId);

       
    }


}
