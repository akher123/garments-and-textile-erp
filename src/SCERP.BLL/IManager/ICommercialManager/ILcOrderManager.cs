using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.ICommercialManager
{
    public interface ILcOrderManager
    {
        List<Model.VBuyerOrder> GetBuyerOrderPaging(VBuyerOrder model, out int totalRecords);
        int SaveBuyerOrder(OM_BuyerOrder model);
        OM_BuyerOrder GetBuyerOrderById(long buyerOrderId);
        int DeleteBuyerOrder(string itemStoreId);
    }
}
