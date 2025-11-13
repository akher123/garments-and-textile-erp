using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommercialModel;

namespace SCERP.DAL.IRepository.ICommercialRepository
{
    public interface IBankAdviceRepository : IRepository<CommBankAdvice>
    {
        CommBankAdvice GetBankAdviceById(int? id);
        List<CommBankAdvice> GetBankAdviceByExportId(Int64 id);
        CommBankAdvice GetBankAdviceByExportAndHeadId(long? exportId, int? accHeadId, out int count);
        List<CommAccHead> GetAccHead(string type);
        List<CommAccHead> GetAccHead(string type, Int64 exportId);
        List<CommBankAdvice> GetAllBankAdvices();
        List<CommBankAdvice> GetAllBankAdvicesByPaging(int startPage, int pageSize, out int totalRecords, CommBankAdvice bankAdvice);
        List<CommBankAdvice> GetBankAdviceBySearchKey(int searchByCountry, string searchByBankAdvice);
    }
}
