using System.Data;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CRMModel;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.ICRMManager
{
    public interface ICRMReportManager
    {
        List<SPCRMDocumentationReport> GetDocumentReport(int moduleId, DateTime? fromDate, DateTime? toDate, string searchString);
    }
}
