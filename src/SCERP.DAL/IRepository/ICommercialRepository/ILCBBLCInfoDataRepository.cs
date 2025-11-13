using SCERP.Model.CommercialModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.DAL.IRepository.ICommercialRepository
{
    public interface ILCBBLCInfoDataRepository : IRepository<CommLCBBLCInfoData>
    {
        int UpdateTna(string compId, int tnaRowId, string key, string value);
    }
}
