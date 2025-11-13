using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEducationLevelRepository:IRepository<EducationLevel>
    {
        EducationLevel GetEducationLevelById(int? id);

        List<EducationLevel> GetAllEducationLevelsByPaging(int startPage, int pageSize, out int totalRecords,
            EducationLevel educationLevel);
    }
}
