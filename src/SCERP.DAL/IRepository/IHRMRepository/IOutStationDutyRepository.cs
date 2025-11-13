using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IOutStationDutyRepository:IRepository<OutStationDuty>
    {
        List<VOutStationDutyDetail> GetAllOutStationDutyDetail
            
            (int startPage, int pageSize, OutStationDuty model, SearchFieldModel searchFieldModel, out int totalRecords);

        OutStationDuty GetOutStationDutyById(int outStationDutyId);
        List<VOutStationDutyDetail> GetOutStationDutyBySearchKey(SearchFieldModel searchField);
    }
}
