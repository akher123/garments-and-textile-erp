using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Linq;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IReligionManager
    {
        List<Religion> GetAllReligionsByPaging(int startPage, int pageSize, out int totalRecords, Religion religion);

        Religion GetReligionById(int religionId);

        int SaveReligion(Religion religion);

        int EditReligion(Religion religion);

        int DeleteReligion(Religion religion);

        bool CheckExistingReligion(Religion religion);

        List<Religion> GetReligionBySearchKey(string searchKey);
        List<Religion> GetAllReligions();
    }
}
