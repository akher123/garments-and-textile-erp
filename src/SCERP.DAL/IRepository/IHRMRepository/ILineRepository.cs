using System.Collections.Generic;
using SCERP.Model;
namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface ILineRepository : IRepository<Line>
    {
        List<Line> GetAllLinesByPaging(int startPage, int pageSize, Line line, out int totalRecords);
        Line GetLineById(int lineId);
    }
}
