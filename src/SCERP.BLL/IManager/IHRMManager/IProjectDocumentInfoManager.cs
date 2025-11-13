using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CRMModel;


namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IProjectDocumentInfoManager
    {
        List<CRMDocumentationReport> GetAllDocumentationReportsByPaging(int startPage, int pageSize, out int totalRecords, CRMDocumentationReport documentationReport);

        List<CRMDocumentationReport> GetAllDocumentationReports();

        CRMDocumentationReport GetDocumentationReportById(int? id);

        int SaveDocumentationReport(CRMDocumentationReport documentationReport);

        int EditDocumentationReport(CRMDocumentationReport documentationReport);

        int DeleteDocumentationReport(CRMDocumentationReport documentationReport);

        bool CheckExistingDocumentationReport(CRMDocumentationReport documentationReport);

        List<CRMDocumentationReport> GetDocumentationReportBySearchKey(string searchKey);
    }
}
