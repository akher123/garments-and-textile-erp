using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface IBuyOrdShipManager
    {
       List<VwBuyOrdShip> GetBuyOrdShipPaging(string orderStyleRefId, out int totalRecords);
       int DeleteBuyOrdShip(string orderShipRefId);
       int EditBuyOrdShip(OM_BuyOrdShip model);
       int SaveBuyOrdShip(OM_BuyOrdShip model);
       string GetNewOrderShipRefId();
       OM_BuyOrdShip GetBuyerById(int buyerId);
       DataTable UpdateTempAssort(string orderStyleRefId);

       DataTable GetShipAssort(string orderShipRefId,string orderStyleRefId);
       string GetNewLotNo(string orderStyleRefId);
       bool CheckShipGreaterQty(string orderStyleRefId, decimal getValueOrDefault);
       int SaveShipmentOfOder(OM_BuyOrdShip buyOrdShip);
       DataTable GetTotalShipAssort(string orderShipRefId, string orderStyleRefId);

       OrderSheet GetOrderSheetDetail(string orderNo);
       IEnumerable GetStyleWiseShipment(string orderStyleRefId, string compId);
        Dictionary<VwBuyOrdShip, DataTable> GetBuyOrdShipByeStyle(string orderStyleRefId);
    }
}
