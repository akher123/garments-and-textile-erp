using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface IOmStyleManager
    {
       List<OM_Style> GetAllStyles();
       List<VStyle> GetStylePaging(OM_Style model, out int totalRecords);
       VStyle GetVStyleById(long styleId);
       string GetNewStyleRefId();
       int EditStyle(OM_Style model);
       int DeleteStyle(string stylerefId);
       int SaveStyle(OM_Style model);
       object GetItemForStyle(string searchKey);
       bool CheckExistingStyle(OM_Style style);
    }
}
