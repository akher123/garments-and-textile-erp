using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface IOmCategoryManager
    {
       List<OM_Category> GetCategories();
       List<OM_Category> GetCategoriesByPaging(OM_Category model, out int totalRecords);
       string GetNewCategoryRefId();
       OM_Category GetCategoryById(int catergoryId);
       int EditCatergory(OM_Category model);
       int SaveCatergory(OM_Category model);
       int DeleteCategory(string catRefId);

       bool CheckExistingCategory(OM_Category model);
    }
}
