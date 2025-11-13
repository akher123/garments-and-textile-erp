using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.CommercialModel;

namespace SCERP.DAL.IRepository.ICommercialRepository
{
    public interface IExportRepository : IRepository<CommExport>
    {
        List<OM_BuyOrdStyle> GetBuyerStyleByOrderNo(string orderNo);
    }
}
