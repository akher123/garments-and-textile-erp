using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface IBuyerOrderManager
    {
        List<Model.VBuyerOrder> GetBuyerOrderPaging(string closed, int pageIndex, string sort, string sortdir, string searchString,
            DateTime? fromDate, DateTime? toDate, out int totalRecords);
        List<VBuyerOrder> GetBuyerWithoutLcOrderPaging(VBuyerOrder model, out int totalRecords);

        List<VBuyerOrder> GetBuyerLcOrderPaging(VBuyerOrder model, out int totalRecords);
        int EditBuyerOrder(OM_BuyerOrder model);
        int SaveBuyerOrder(OM_BuyerOrder model);
        OM_BuyerOrder GetBuyerOrderById(long buyerOrderId);
        string GetNewRefNo();
        int DeleteBuyerOrder(string itemStoreId);
        int EditLcOrder(OM_BuyerOrder model);
        VBuyerOrder GetBuyerOrderByOrderNo(string orderNo);
        OrderSheet GetOrderSheet(string orderNo);
        IEnumerable<VBuyerOrder> GetVBuyerOrder(VBuyerOrder model);
        DataTable GetMerchaiserWiseOrderDataTable(DateTime? fromDate,DateTime? toDate);
        IEnumerable<VOM_BuyOrdStyle> GetOrderDetailsByOrderNo(string orderNo);
        OM_BuyerOrder GetBuyerLcOrderByOrderNo(string orderNo);
        IEnumerable GetOrderByBuyer(string buyerRefId);

        int UpdateOrderStatus(long buyerOrderId, string closed);
    }
}
