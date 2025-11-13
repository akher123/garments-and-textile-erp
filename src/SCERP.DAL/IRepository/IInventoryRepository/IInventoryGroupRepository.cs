using SCERP.Model;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
 
    public interface IInventoryGroupRepository:IRepository<Inventory_Group>
    {
        string GetMaxGroupCode();
        System.Collections.Generic.List<Inventory_Group> AutocompliteGroup( string groupName);
    }
}
