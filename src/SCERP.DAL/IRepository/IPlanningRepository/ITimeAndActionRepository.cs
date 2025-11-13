using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.IRepository.IPlanningRepository
{
    public interface ITimeAndActionRepository : IRepository<OM_TNA>
    {
        int UpdateTna(string compId, int tnaRowId, string key, string value);
        object GetStyleWiseTna(string orderNo, string orderStyleRefId, string buyerRefId, string compId, string searchKey);
        DataTable GetHorizontalTna(string orderNo, string orderStyleRefId, string buyerRefId, string compId);
    }
}
