using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface ISectionManager
    {
        List<Section> GetAllSections(int startPage, int pageSize, Section model, out int totalRecords);
        Section GetSectionById(int sectionId);
        bool IsExistSection(Section model);
        int EditSection(Section model);
        int SaveSection(Section model);
        int DeleteSection(int sectionId);
        List<Section> GetAllSectionBySearchKey(string searchKey);

        IList<Section> GetListOfSection();
        List<Section> GetSectionByDepartment(int? departmentId);
    }
}
