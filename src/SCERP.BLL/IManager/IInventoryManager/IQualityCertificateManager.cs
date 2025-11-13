using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;


namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface IQualityCertificateManager
    {
        string GetNewQCReferenceNo();
        int SaveQualityCertificate(Inventory_QualityCertificate qcCertificate);
        List<VQualityCertificate> GetQualityCertificateByPaging(out int totalRecords, VQualityCertificate model);
        List<VQualityCertificateDetail> GetQualityCertificateDetailIds(long itemStoreId, int qualityCertificateId);
        int EditQualityCertificate(Inventory_QualityCertificate qualityCertificate);
        Inventory_QualityCertificate GetQualityCertificateById(int qualityCertificateId);
        int DeleteQualityCertificate(int? qualityCertificateId);
    }
}
