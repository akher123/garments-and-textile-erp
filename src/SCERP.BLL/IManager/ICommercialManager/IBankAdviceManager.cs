using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommercialModel;

namespace SCERP.BLL.IManager.ICommercialManager
{
    public interface IBankAdviceManager
    {
        List<CommBankAdvice> GetAllBankAdvicesByPaging(int startPage, int pageSize, out int totalRecords, CommBankAdvice bankAdvice);

        List<CommBankAdvice> GetAllBankAdvices();

        CommBankAdvice GetBankAdviceById(int? id);

        List<CommAccHead> GetAccHead(string type);
        CommBankAdvice GetBankAdviceByExportAndHeadId(long? exportId, int? accHeadId, out int count);

        List<CommAccHead> GetAccHead(string type, Int64 exportId);

        List<CommBankAdvice> GetBankAdviceByExportId(Int64 id);

        int SaveBankAdvice(CommBankAdvice bankAdvice);

        int EditBankAdvice(CommBankAdvice bankAdvice);

        int DeleteBankAdvice(CommBankAdvice bankAdvice);

        bool CheckExistingBankAdvice(CommBankAdvice bankAdvice);

        List<CommBankAdvice> GetBankAdviceBySearchKey(int searchByCountry, string searchByCommBankAdvice);
    }
}
