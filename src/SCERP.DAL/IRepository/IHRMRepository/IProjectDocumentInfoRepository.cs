using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CRMModel;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IProjectDocumentInfoRepository : IRepository<CRMDocumentationReport>
    {
        CRMDocumentationReport GetDocumentationReportById(int? id);

        List<CRMDocumentationReport> GetAllDocumentationReports();

        List<CRMDocumentationReport> GetAllDocumentationReportsByPaging(int startPage, int pageSize, out int totalRecords, CRMDocumentationReport documentationReport);

        List<CRMDocumentationReport> GetAllDocumentationReportsBySearchKey(string searchKey);
    }
}
