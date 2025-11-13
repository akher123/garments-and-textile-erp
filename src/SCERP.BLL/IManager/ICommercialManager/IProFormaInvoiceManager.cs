using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.IManager.ICommercialManager
{
    public interface IProFormaInvoiceManager
    {
        List<ProFormaInvoice> GetProFormaInvoiceByPaging(int pageIndex, string sort, string sortdir, string searchString, out int totalRecords);
        ProFormaInvoice GetProFormaInvoiceById(int piId);
        string GetNewRefId(string compId);
        int EditProFormaInvoice(ProFormaInvoice proFormaInvoice);
        int SaveProFormaInvoice(ProFormaInvoice proFormaInvoice);
        bool IsPiExist(int supplierId, string piNo, int PiId);
        int DeleteProFormaInvoice(int piId);
        List<ProFormaInvoice> GetPiBySupplier(int supplierId, string compId);
        ProFormaInvoice GetProFormaInvoiceByRefId(string compId, string purchaseOrderNo);
        List<ProFormaInvoice> GetProFormaInvoiceBySupplierIds(int[] supplierIds);
        List<Mrc_SupplierCompany> GetAllSuppliers(string compId);
    }
}
