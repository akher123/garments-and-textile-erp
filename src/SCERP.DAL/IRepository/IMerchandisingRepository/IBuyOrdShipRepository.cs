using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
    public interface IBuyOrdShipRepository:IRepository<OM_BuyOrdShip>
    {
        string GetNewOrderShipRefId(string compId);
        DataTable UpdateTempAssort(OM_BuyOrdShip buyerOrderShips);
        List<VBuyOrdStyleSize> GetBuyOrdStyleSize(OM_BuyOrdShip buyerOrderShips);

        string GetNewLotNo(string compId, string orderStyleRefId);
        DataTable UpdatedShipmentTotalTempAssort(OM_BuyOrdShip orderShip);
        IQueryable<VwBuyOrdShip> GetBuyOrdShips(string orderStyleRefId, string compId);
    }
}
