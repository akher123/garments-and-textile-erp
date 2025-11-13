using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Linq;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IGenderManager
    {       
        List<Gender> GetAllGenders();
        List<Gender> GetGenders(int startPage, int pageSize, Gender gender, out int totalRecords);
        Gender GetGenderById(byte genderId);
        bool IsExistGender(Gender model);
        int EditGender(Gender model);
        int SaveGender(Gender model);
        int DeleteGender(int genderId);
       
    }
}
