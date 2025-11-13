using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IGenderRepository : IRepository<Gender>
    {
        List<Gender> GetAllGenders();
        List<Gender> GetGenders(int startPage, int pageSize, Gender gender, out int totalRecords);
    }
}
