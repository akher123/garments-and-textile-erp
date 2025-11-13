using System.Collections.Generic;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface ISectionRepository:IRepository<Section>
    {
        List<Section> GetAllSections(int startPage, int pageSize, Section model, out int totalRecords);
    }
}
