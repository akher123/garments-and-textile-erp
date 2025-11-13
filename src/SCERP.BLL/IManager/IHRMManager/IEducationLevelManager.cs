using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Linq;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEducationLevelManager
    {
        List<EducationLevel> GetAllEducationLevelsByPaging(int startPage, int pageSize, out int totalRecords, EducationLevel educationLevel);

        EducationLevel GetEducationLevelById(int? id);

        int SaveEducationLevel(EducationLevel educationLevel);

        int EditEducationLevel(EducationLevel educationLevel);        
        
        int DeleteEducationLevel(EducationLevel educationLevel);

        bool CheckExistingEducationLevel(EducationLevel educationLevel);

        List<EducationLevel> GetEducationLevelBySearchKey(string searchKey);

        List<EducationLevel> GetAllEducationLevels();
    }
}
