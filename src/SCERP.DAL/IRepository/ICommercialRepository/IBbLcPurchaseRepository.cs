using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.CommercialModel;


namespace SCERP.DAL.IRepository.ICommercialRepository
{
    public interface IBbLcPurchaseRepository : IRepository<CommBbLcPurchaseCommon>
    {
        CommBbLcPurchaseCommon GetBbLcPurchaseById(int? id);
        int? GetBbLcIdByLcNo(string lcNo);
        string GetOrderNoByOrderRefNo(string orderRefNo);
        List<OM_BuyOrdStyle> GetStylesByOrderNo(string orderNo);
        List<CommBbLcPurchaseCommon> GetAllBbLcPurchases();
        List<CommBbLcPurchaseCommon> GetBbLcPurchaseByBbLcId(int bbLcId);
        List<VwBbLcPurchaseCommon> GetAllBbLcPurchasesByPaging(int startPage, int pageSize, out int totalRecords, CommBbLcPurchaseCommon bbLcPurchase);
        List<CommBbLcPurchaseCommon> GetBbLcPurchaseBySearchKey(int searchByCountry, string searchByBbLcPurchase);
        List<VwCommBbLcPurchase> GetBbLcPurchaseEditByBbLcId(CommBbLcPurchaseCommon bbLcPurchase);
    }
}