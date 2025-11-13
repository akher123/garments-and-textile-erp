using System.Collections.Generic;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.IManager.ICommonManager
{
   public interface IColorManager
    {
       int SaveColor(Color model);
       List<Color> GetColorstByPaging(Color model, out int totalRecords);
       Color GetColorById(long colorId);
       int EditColor(Color model);
       string GetNewColorRef();

       List<Color> GetColors();
       object AutoCompliteColor(string searchString);
       int DeleteColor(long colorId);
    }
}
