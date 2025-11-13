using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.ICommonManager
{
    public interface IMeasurementUnitManager
    {
        List<MeasurementUnit> GetMeasurementUnits();
        List<MeasurementUnit> GetMeasurementUnitByPaging(MeasurementUnit model, out int totalRecords);
        MeasurementUnit GetMeasurementUnitById(int unitId);
        int EditMeasurementUnit(MeasurementUnit model);
        int SaveMeasurementUnit(MeasurementUnit model);
        int DeleteMeasurementUnit(int unitId);
  
        bool IsExistMeasurementUnit(MeasurementUnit model);
    }
}
