using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public interface ICompanySectorRepository : IRepository<Acc_CompanySector>
    {
        Acc_CompanySector GetCompanySectorById(int? id);
        List<Acc_CompanySector> GetAllCompanySectors(int page, int records, string sort);
        IQueryable<Acc_CompanySector> GetAllCompanySectors();
    }
}
