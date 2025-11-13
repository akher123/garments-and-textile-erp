using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Custom;
using SCERP.Model.CRMModel;

namespace SCERP.DAL.IRepository.ICRMRepository
{
    public interface ICRMReportRepository
    {
        List<SPCRMDocumentationReport> GetDocumentReport(int moduleId, DateTime? fromDate, DateTime? toDate, string searchString);
    }
}
