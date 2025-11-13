using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IUnitRepository : IRepository<Unit>
    {
        List<Unit> GetAllUnits(int startPage, int pageSize, Unit unit, out int totalRecords);
    }
}