using System.Collections.Generic;
using SCERP.Model;


namespace SCERP.DAL.IRepository.IInventoryRepository
{
    public interface IQualityCertificateRepository : IRepository<Inventory_QualityCertificate>
    {
        List<VQualityCertificate> GetQualityCertificateByPaging(out int totalRecords, VQualityCertificate model);
        List<VQualityCertificateDetail> GetQualityCertificateDetailIds(long itemStoreId, int qualityCertificateId);
        string GetNewQCReferenceNo();
    }
}
