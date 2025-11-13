using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface IOmBuyerManager
    {
        List<OM_Buyer> GetAllBuyers();
        List<OM_Buyer> GetBuyersByPaging(OM_Buyer model, out int totalRecords);
        OM_Buyer GetBuyerById(long buyerId);
        string GetNewBuyerRefId();
        int EditBuyer(OM_Buyer model);
        int SaveBuyer(OM_Buyer model);
        int DeleteDelete(string buyerRefId);
        bool CheckExistingBuyer(OM_Buyer model);
       object GetCuttingProcessStyleActiveBuyers();
        OM_Buyer GetBuyerByRefId(string buyerRefId, string compId);
    }
}
