using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface IOmBuyOrdStyleManager
    {
       List<VOMBuyOrdStyle> GetBuyerOrderStyleByOrderNo(string orderNo);
       OM_BuyOrdStyle GetBuyOrdStyleById(long orderStyleId);
       string GetNewStyleRefNo();
       int EditBuyOrdStyle(OM_BuyOrdStyle model);
       int SaveBuyOrdStyle(OM_BuyOrdStyle model);
       int DeleteBuyerOrderStyle(string orderStyleRefId);
       object StyleAutocomplite(string searchString);
       OM_BuyOrdStyle GetBuyOrdStyleByRefId(string orderStyleRefId);

       VOMBuyOrdStyle GetVBuyOrdStyleByRefId(string orderStyleRefId);
       List<VOMBuyOrdStyle> GetBuyerOrderStyles(OM_BuyOrdStyle omBuyOrdStyle, out int totalRecords);
       bool CheckGreaterQty(string orderNo,decimal qty);


       List<VwStyleFollowupStatus> GetStyleFollowupStatusesByPaging
           (int pageIndex, DateTime? fromDate, DateTime? toDate, string merchandiserId, string buyerRefId, string searchString, out int totalRecords);

       IEnumerable GetOrderByBuyer(string buyerRefId);
       IEnumerable GetStyleByOrderNo(string orderNo);
       IEnumerable GetColorsByOrderStyleRefId(string orderStyleRefId);
       int SaveShipQty(OM_BuyOrdStyle model);
       IEnumerable GetOrderAllByBuyer(string buyerRefId);

       List<VOM_BuyOrdStyle> GetBuyerOrderStyles(int pageIndex, string buyerRefId, string orderNo, string orderStyleRefId,string isLocked, out int totalRecords);
       int TnaApproved(int orderStyleId, string compId);
       IEnumerable GetSizeByOrderStyleRefId(string orderStyleRefId);
    }
}
