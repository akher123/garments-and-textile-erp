using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IReligionRepository:IRepository<Religion>
    {
        Religion GetReligionById(int religionId);
        List<Religion> GetAllReligionsByPaging(int startPage, int pageSize, out int totalRecords,Religion religion);
        List<Religion> GetAllReligions();
    }
}
