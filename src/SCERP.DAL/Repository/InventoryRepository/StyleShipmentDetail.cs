using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class StyleShipmentDetail : Repository<Inventory_StyleShipmentDetail>, IStyleShipmentDetail
   {
        public StyleShipmentDetail(SCERPDBContext context) : base(context)
        {
        }

        public List<VwInventoryStyleShipment> GetShipmentStyleRefIds(long styleShipmentId)
        {
            var detailStyleRefIds = (from style in Context.VOMBuyOrdStyles
                join detail in Context.Inventory_StyleShipmentDetail.Where(x=>x.StyleShipmentId==styleShipmentId) on style.OrderStyleRefId equals
                    detail.OrderStyleRefId                                   
                select new {style.OrderStyleRefId, style.StyleName, detail.StyleShipmentId}
                ).Distinct()
                .ToList()
                .Select(x => new VwInventoryStyleShipment()
                {
                    OrderStyleRefId = x.OrderStyleRefId,
                    StyleName = x.StyleName,
                    StyleShipmentId = x.StyleShipmentId
                }).ToList();

            return detailStyleRefIds;


        }
   }
}
