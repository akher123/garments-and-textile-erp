using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.CommercialModel;


namespace SCERP.BLL.IManager.ICommercialManager
{
    public interface IBbLcPurchaseManager
    {
        List<VwBbLcPurchaseCommon> GetAllBbLcPurchasesByPaging(int startPage, int pageSize, out int totalRecords, CommBbLcPurchaseCommon bbLcPurchase);

        List<CommBbLcPurchaseCommon> GetAllBbLcPurchases();

        CommBbLcPurchaseCommon GetBbLcPurchaseById(int? id);

        string GetOrderNoByOrderRefNo(string orderRefNo);

        int? GetBbLcIdByLcNo(string lcNo);

        int SaveBbLcPurchase(CommBbLcPurchaseCommon bbLcPurchase);

        List<OM_BuyOrdStyle> GetStylesByOrderNo(string orderNo);

        int EditBbLcPurchase(CommBbLcPurchaseCommon bbLcPurchase);

        List<CommBbLcPurchaseCommon> GetBbLcPurchaseByBbLcId(int bbLcId);

        int DeleteBbLcPurchase(CommBbLcPurchaseCommon bbLcPurchase);

        bool CheckExistingBbLcPurchase(CommBbLcPurchaseCommon bbLcPurchase);

        List<CommBbLcPurchaseCommon> GetBbLcPurchaseBySearchKey(int searchByCountry, string searchByCommBbLcPurchase);

        List<VwCommBbLcPurchase> GetBbLcPurchaseEditByBbLcId(CommBbLcPurchaseCommon bbLcPurchase);
    }
}
