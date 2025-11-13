using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
    public interface IOmColorRepository:IRepository<OM_Color>
    {
        string GetNewOmColorRefId(string compId);
        IQueryable<VwLot> GetLots(string compId,string typeId);
    }
}
