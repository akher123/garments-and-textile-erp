using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IQuitTypeRepository : IRepository<QuitType>
    {
        List<QuitType> GetAllQuitTypes();
        List<QuitType> GetAllQuitTypesByPaging(int startPage, int pageSize, QuitType model, out int totalRecords);
        List<QuitType> GetAllQuitTypesBySearchKey(QuitType model);
    }
}
