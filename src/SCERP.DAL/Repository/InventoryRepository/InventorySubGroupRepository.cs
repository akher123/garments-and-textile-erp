using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class InventorySubGroupRepository : Repository<Inventory_SubGroup>, IInventorySubGroupRepository
    {
        public InventorySubGroupRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<Inventory_SubGroup> AutocompliteSubGroup(string subGroupName, int groupId, int branchId)
        {
            return
                Context.Inventory_SubGroup.Include(x => x.Inventory_Group)
                    .Where(
                        x =>
                            x.IsActive && x.GroupId == groupId &&
                            (x.SubGroupName.Replace(" ", "").ToLower().Contains(subGroupName.Replace(" ", "").ToLower()) ||
                             String.IsNullOrEmpty(subGroupName))).OrderBy(x=>x.SubGroupCode)
                    .ToList().Select(x=>new Inventory_SubGroup()
                    {
                        SubGroupName = x.SubGroupName,
                        SubGroupCode = x.SubGroupCode,
                        SubGroupId = x.SubGroupId
                    }).ToList();
        }
    }
}
