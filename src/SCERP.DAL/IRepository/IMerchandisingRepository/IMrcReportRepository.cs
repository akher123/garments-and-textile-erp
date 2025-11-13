using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
    public interface IMrcReportRepository : IRepository<Employee>
    {
        List<SpecSheetModel> GetSpecSheetDetail(int id);
        List<SpecSheetModel> GetSpecSheetList(int? buyerId, string styleNo, string jobNo, DateTime? fromDate, DateTime? toDate);
    }
}
