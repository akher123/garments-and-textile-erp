using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class InventoryGroupRepository : Repository<Inventory_Group>, IInventoryGroupRepository
    {
        public InventoryGroupRepository(SCERPDBContext context) : base(context)
        {

        }

        public string GetMaxGroupCode()
        {

            var maxGroupCode = Context.Database.SqlQuery<string>("select RIGHT('00'+ CONVERT(VARCHAR(2),isnull(max(GroupCode),00)+1),2)  as GroupCode from Inventory_Group").SingleOrDefault();
            return maxGroupCode;
        }

        public List<Inventory_Group> AutocompliteGroup( string groupName)
        {
            return Context.Inventory_Group
                .Where(x => x.IsActive&&(x.GroupName.Replace(" ","")
                    .ToLower().Contains(groupName.Replace(" ","").ToLower())|| String.IsNullOrEmpty(groupName))).OrderBy(x=>x.GroupCode)
                    .ToList();
        }
    }
}
