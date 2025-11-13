using System.Collections.Generic;
using SCERP.Model;
namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface IMerchandiserManager
    {
        IEnumerable<OM_Merchandiser> GetMerchandisers();
        List<OM_Merchandiser> GetMerchandiserByPaging(OM_Merchandiser model, out int totalRecords);
        OM_Merchandiser GetMerchandiserById(int merchandiserId);
        string GetMerchandiserRefId();
        int EditMerchandiser(OM_Merchandiser model);
        int SaveMerchandiser(OM_Merchandiser model);
        int DeleteMerchandiser(string empId);
        IEnumerable<OM_Merchandiser> GetPermitedMerchandisers();


   
    }
}
