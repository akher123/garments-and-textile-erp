using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface ILineManager
    {
        List<Line> GetAllLinesByPaging(int startPage, int pageSize, Line line, out int totalRecords);
        Line GetLineById(int lineId);
        bool CheckExistingLine(Line line);
        int EditLine(Line line);
        int SaveLine(Line line);
        int DeleteLine(int lineId);
        List<Line> GetAllLinesBySearchKey(string lineName);
        List<Line> GetAllLines();
    }
}
