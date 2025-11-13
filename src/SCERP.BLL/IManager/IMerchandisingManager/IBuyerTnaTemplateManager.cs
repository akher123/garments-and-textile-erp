using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface IBuyerTnaTemplateManager
    {
        List<BuyerTnaTemplateModel> GetTemplates(string compId, string modelBuyerRefId, int modelTemplateTypeId);
        int SaveBuyerTnaTemplateLayout(List<OM_BuyerTnaTemplate> buyerTnaTemplates);
    }
}
