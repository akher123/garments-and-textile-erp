using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.Model.CRMModel;


namespace SCERP.DAL.Repository.HRMRepository
{
    public class ProjectDocumentInfoRepository : Repository<CRMDocumentationReport>, IProjectDocumentInfoRepository
    {
        public ProjectDocumentInfoRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public CRMDocumentationReport GetDocumentationReportById(int? id)
        {
            return Context.CRMDocumentationReports.Find(id);
        }

        public List<CRMDocumentationReport> GetAllDocumentationReportsByPaging(int startPage, int pageSize, out int totalRecords, CRMDocumentationReport documentationReport)
        {
            IQueryable<CRMDocumentationReport> documentationReports;

            try
            {
                string searchKey = documentationReport.ReportName;

                documentationReports = Context.CRMDocumentationReports.Where(x => x.IsActive && ((x.ReportName.Replace(" ", "").ToLower().Contains(searchKey.Replace(" ", "").ToLower()))
                                                                                              || (x.Description.Replace(" ", "").ToLower().Contains(searchKey.Replace(" ", "").ToLower()))
                                                                                              || (x.RefNo.Replace(" ", "").ToLower().Contains(searchKey.Replace(" ", "").ToLower()))
                                                                                              || (x.RefNo.Replace(" ", "").ToLower().Contains(searchKey.Replace(" ", "").ToLower()))
                                                                                              || String.IsNullOrEmpty(searchKey))

                    );

                //DateTime? fromDate = documentationReport.FromDate;
                //DateTime? toDate = null;

                //if (documentationReport.ToDate != null)
                //{
                //    toDate = documentationReport.ToDate.Value.AddDays(1);
                //}
                //if (fromDate != null && toDate != null)
                //    documentationReports = documentationReports.Where(p => p.LastUpdateDate >= fromDate && p.LastUpdateDate <= toDate);

                totalRecords = documentationReports.Count();

                switch (documentationReport.sort)
                {
                    case "ReportName":
                        switch (documentationReport.sortdir)
                        {
                            case "DESC":
                                documentationReports = documentationReports
                                    .OrderByDescending(r => r.ReportName)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                documentationReports = documentationReports
                                    .OrderBy(r => r.ReportName)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    default:
                        documentationReports = documentationReports
                            .OrderBy(r => r.ReportName)
                            .Skip(startPage*pageSize)
                            .Take(pageSize);
                        break;
                }
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.Message);
            }

            return documentationReports.ToList();
        }

        public List<CRMDocumentationReport> GetAllDocumentationReportsBySearchKey(string searchKey)
        {
            List<CRMDocumentationReport> documentationReports = null;

            try
            {
                documentationReports = !String.IsNullOrEmpty(searchKey) ? Context.CRMDocumentationReports.Where(x => x.IsActive == true && x.ReportName.ToLower().Contains(searchKey.ToLower())).ToList() : GetAllDocumentationReports();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return documentationReports;
        }

        public List<CRMDocumentationReport> GetAllDocumentationReports()
        {
            return Context.CRMDocumentationReports.Where(x => x.IsActive).OrderBy(x => x.Id).ToList();
        }
    }
}
