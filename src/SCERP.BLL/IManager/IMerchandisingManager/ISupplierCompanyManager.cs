using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface ISupplierCompanyManager
    {
        List<Mrc_SupplierCompany> GetAllSupplierCompany();
        Mrc_SupplierCompany GetSupplierCompanyById(int supplierCompanyId);
        int EditSupplierCompany(Mrc_SupplierCompany supplierCompany);
        int SaveSupplierCompany(Mrc_SupplierCompany supplierCompany);
        int DeleteSupplierCompany(int? id);
        List<Mrc_SupplierCompany> GetSupplierCompanyByPaging(Mrc_SupplierCompany model, out int totalRecors);
  
    }
}
