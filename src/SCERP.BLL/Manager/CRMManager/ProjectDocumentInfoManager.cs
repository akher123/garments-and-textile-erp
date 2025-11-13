using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.ICRMRepository;

using SCERP.DAL.Repository.CRMRepository;

using SCERP.Model;
using SCERP.Model.CRMModel;

namespace SCERP.BLL.Manager.CRMManager
{
    public class ProjectDocumentInfoManager : BaseManager, SCERP.BLL.IManager.ICRMManager.IProjectDocumentInfoManager
    {
        private readonly IProjectDocumentInfoRepository _documentationReportRepository = null;

        public ProjectDocumentInfoManager(SCERPDBContext context)
        {
            this._documentationReportRepository = new ProjectDocumentInfoRepository(context);
        }

        public List<CRMDocumentationReport> GetAllDocumentationReportsByPaging(int startPage, int pageSize, out int totalRecords, CRMDocumentationReport documentationReport)
        {
            var documentationReports = new List<CRMDocumentationReport>();

            try
            {
                documentationReports = _documentationReportRepository.GetAllDocumentationReportsByPaging(startPage, pageSize, out totalRecords, documentationReport).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                totalRecords = 0;
            }

            return documentationReports;
        }

        public List<CRMDocumentationReport> GetAllDocumentationReports()
        {
            var documentationReports = new List<CRMDocumentationReport>();

            try
            {
                documentationReports = _documentationReportRepository.GetAllDocumentationReports();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return documentationReports;
        }

        public CRMDocumentationReport GetDocumentationReportById(int? id)
        {
            var documentationReport = new CRMDocumentationReport();

            try
            {
                documentationReport = _documentationReportRepository.GetDocumentationReportById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);

            }

            return documentationReport;
        }

        public List<CRMDocumentationReport> GetDocumentationReportBySearchKey(string searchKey)
        {
            var documentationReports = new List<CRMDocumentationReport>();

            try
            {
                documentationReports = _documentationReportRepository.Filter(x => x.ReportName.Replace(" ", "").ToLower().Contains(searchKey.Replace(" ", "").ToLower()) || String.IsNullOrEmpty(searchKey)).OrderBy(x => x.ReportName).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return documentationReports;
        }

        public bool CheckExistingDocumentationReport(CRMDocumentationReport documentationReport)
        {
            bool isExist = false;

            try
            {
                isExist = _documentationReportRepository.Exists(x => x.IsActive && x.Id != documentationReport.Id && x.RefNo.Replace(" ", "").ToLower().Equals(documentationReport.RefNo.Replace(" ", "").ToLower()));
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return isExist;
        }

        public int SaveDocumentationReport(CRMDocumentationReport documentationReport)
        {
            var savedDocumentationReport = 0;
            try
            {
                documentationReport.IsActive = true;
                documentationReport.CreatedDate = DateTime.Now;
                documentationReport.CreatedBy = PortalContext.CurrentUser.UserId;
                savedDocumentationReport = _documentationReportRepository.Save(documentationReport);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                savedDocumentationReport = 0;
            }

            return savedDocumentationReport;
        }

        public int EditDocumentationReport(CRMDocumentationReport documentationReport)
        {
            var edit = 0;

            try
            {
                documentationReport.EditedDate = DateTime.Now;
                documentationReport.EditedBy = PortalContext.CurrentUser.UserId;
                edit = _documentationReportRepository.Edit(documentationReport);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return edit;
        }

        public int DeleteDocumentationReport(CRMDocumentationReport documentationReport)
        {
            var deleted = 0;

            try
            {
                documentationReport.IsActive = false;
                deleted = _documentationReportRepository.Edit(documentationReport);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return deleted;
        }

        public List<CRMCollaborator> GetAllResponsiblePerson()
        {
            return _documentationReportRepository.GetAllResponsiblePerson();
        }

        public List<SCERP.Model.Module> GetAllModulesInfo()
        {
            return _documentationReportRepository.GetAllModulesInfo();
        }
    }
}
