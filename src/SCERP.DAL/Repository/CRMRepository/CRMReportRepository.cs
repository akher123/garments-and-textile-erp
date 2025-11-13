using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using SCERP.DAL.IRepository.ICRMRepository;
using SCERP.Model;
using SCERP.Model.CRMModel;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.CRMRepository
{
    public class CRMReportRepository : ICRMReportRepository
    {
        private readonly SCERPDBContext _context;
        private SqlConnection _connection;

        public CRMReportRepository(SCERPDBContext context)
        {
            this._context = context;
            _connection = (SqlConnection) _context.Database.Connection;
        }

        public List<SPCRMDocumentationReport> GetDocumentReport(int moduleId, DateTime? fromDate, DateTime? toDate, string searchString)
        {
            List<SPCRMDocumentationReport> documentReport;
            documentReport = _context.Database.SqlQuery<SPCRMDocumentationReport>("SPCRMDocumentationReport @ModuleId, @FromDate, @ToDate, @SearchString", new SqlParameter("ModuleId", moduleId), new SqlParameter("FromDate", fromDate), new SqlParameter("ToDate", toDate), new SqlParameter("SearchString", searchString)).ToList();
            return documentReport;
        }
    }
}
