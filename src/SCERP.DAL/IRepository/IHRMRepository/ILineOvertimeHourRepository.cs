using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.HRMModel;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface ILineOvertimeHourRepository : IRepository<LineOvertimeHour>
    {
        List<LineOvertimeHour> GetLineOvertimeHoureByOtDate(DateTime? otDate);
        bool SendOvertimeHour(Guid? prepairedBy, DateTime? otDate);
        DataTable GetOvertimeHoureByOtDate(DateTime? otDate, bool all, bool garments, bool knitting, bool dyeing);
        DataTable GetLineWiseEmployeeOTHours(int departmentLineId, DateTime? otDate);
    }
}
