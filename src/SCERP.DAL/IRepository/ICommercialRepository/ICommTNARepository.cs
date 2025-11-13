using SCERP.Model.CommercialModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.DAL.IRepository.ICommercialRepository
{
    public interface ICommTNARepository : IRepository<CommTNA>
    {
        int UpdateTna(string compId, int commTnaRowId, string key, string value);
    }
}
