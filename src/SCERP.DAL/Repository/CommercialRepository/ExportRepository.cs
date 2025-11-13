using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Model;
using SCERP.Model.CommercialModel;
using SCERP.Common;

namespace SCERP.DAL.Repository.CommercialRepository
{
    public class ExportRepository : Repository<CommExport>, IExportRepository
    {
        private readonly string _companyId;

        public ExportRepository(SCERPDBContext context) : base(context)
        {
            this._companyId = PortalContext.CurrentUser.CompId;
        }

        public List<OM_BuyOrdStyle> GetBuyerStyleByOrderNo(string orderNo)
        {
            List<OM_BuyOrdStyle> style = Context.OM_BuyOrdStyle.Where(p => p.CompId == _companyId && p.OrderNo == orderNo).ToList();
            return style;
        }
    }
}
