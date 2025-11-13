using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface IOmBrandManager
    {
       List<OM_Brand> GetBrands();
       List<OM_Brand> GetOmBrandsByPaging(OM_Brand model, out int totalRecords);
       OM_Brand GetOmBrandById(int brandId);
       string GetNewBrandRefId();
       int EditOmBrand  (OM_Brand model);
       int SaveOmBrand(OM_Brand model);
       int DeleteOmBrand(string brandRefId);
       bool CheckExistingBrand(OM_Brand model);
    }
}
