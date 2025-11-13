using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface ICuttingTagSupplierManager
    {
       PROD_CuttingTagSupplier GetCuttingTagByCuttingTagId(long cuttingTagSupplierId);
       int EditCuttingTagSupplier(PROD_CuttingTagSupplier model);
       int SaveCuttingTagSupplier(PROD_CuttingTagSupplier model);
       int DeleteCuttingTagSupplier(long cuttingTagSupplierId);
       bool IsCuttingTagSupplierExist(PROD_CuttingTagSupplier model);
    
    }
}
