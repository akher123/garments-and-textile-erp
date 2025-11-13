using System;
using System.Collections.Generic;
using SCERP.BLL.IManager.ICRMManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.ICRMRepository;
using SCERP.DAL.Repository.CRMRepository;
using SCERP.Model.CRMModel;

namespace SCERP.BLL.Manager.CRMManager
{
    public class CRMReportManager : ICRMReportManager
    {
        private ICRMReportRepository crmReportRepository = null;

        public CRMReportManager(SCERPDBContext context)
        {
            crmReportRepository = new CRMReportRepository(context);
        }

        public List<SPCRMDocumentationReport> GetDocumentReport(int moduleId, DateTime? fromDate, DateTime? toDate, string searchString)
        {
            return crmReportRepository.GetDocumentReport(moduleId, fromDate, toDate, searchString);
        }
    }
}