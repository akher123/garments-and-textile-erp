using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IOutStationDutyManager
    {
        int SaveOutStationDuty(OutStationDuty mode);
        List<VOutStationDutyDetail> GetAllOutStationDutyDetail(int startPage, int pageSize, OutStationDuty model, SearchFieldModel searchFieldModel, out int totalRecords);
        OutStationDuty GetOutStationDutyById(int outStationDutyId);
        int EditOutStationDuty(OutStationDuty model);
        int DeleteOutstationDutyById(int outStationDutyId);
        List<VOutStationDutyDetail> GetOutStationDutyBySearchKey(SearchFieldModel searchField);
    }
}
