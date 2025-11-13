using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class MaterialReceiveAgainstPoDetailRepository : Repository<Inventory_MaterialReceiveAgainstPoDetail>, IMaterialReceiveAgainstPoDetailRepository
   {
        public MaterialReceiveAgainstPoDetailRepository(SCERPDBContext context) : base(context)
        {
        }

        public VwMaterialReceiveAgainstPoDetail GetMaterialReceiveAgainstPoDetail(int itemId, long sourceId, string colorRefId,
            string compId)
        {

            string sql = @"select top(1)* from VwMaterialReceiveAgainstPoDetail where MaterialReceiveAgstPoId='{0}' and ItemId='{1}' and ColorRefId='{2}' and CompId='{3}'";
            return  Context.Database.SqlQuery<VwMaterialReceiveAgainstPoDetail>(string.Format(sql,sourceId,itemId,colorRefId,compId)).FirstOrDefault();
        }
   }
}
