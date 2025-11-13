using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface IOmColorManager
    {
        object ColorAutoComplite(string serachString,string typeId);
        List<OM_Color> GetOmColorByPaging(OM_Color model, out int totalRecords);
        OM_Color GetOmColorById(int colorId);
        string GetNewOmColorRefId();
        int EditOmColor(OM_Color model);
        int DeleteOmColor(string sizeRefId);
        int SaveOmColor(OM_Color model);
        bool CheckExistingColor(OM_Color color);
        List<OM_Color> GetOmColors();
        List<VwLot> GetLotByPaging(OM_Color model, out int totalRecords);
        object LotAutocomplite(string serachString, string typeId);
        object AutoCompleteColor(string searchString);
        int SaveLot(OM_Color color);
        int EditLot(OM_Color color);
        object GetLotDetails(string lotId);
    }
}
