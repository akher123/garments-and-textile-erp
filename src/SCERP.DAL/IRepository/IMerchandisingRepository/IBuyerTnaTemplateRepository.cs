using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
    public interface IBuyerTnaTemplateRepository:IRepository<OM_BuyerTnaTemplate>
    {
        List<BuyerTnaTemplateModel> GetTemplates(string compId, string buyerRefId, int templateTypeId);

        int CreateTnaByBuyerTemplate(string compId, string buyerRefId, int defaultTemplate, string orderStyleRefId, string date);
    }
}
