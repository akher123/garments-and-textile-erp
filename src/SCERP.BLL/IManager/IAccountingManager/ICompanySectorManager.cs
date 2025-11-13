using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IAccountingManager
{
    public interface ICompanySectorManager
    {
        List<Acc_CompanySector> GetAllCompanySectors(int page, int records, string sort);

        Acc_CompanySector GetCompanySectorById(int? id);

        int SaveCompanySector(Acc_CompanySector aCompanySector);

        void DeleteCompanySector(Acc_CompanySector CompanySector);
    }
}
