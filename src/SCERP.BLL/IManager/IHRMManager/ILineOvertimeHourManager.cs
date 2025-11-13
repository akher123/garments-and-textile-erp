using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface ILineOvertimeHourManager
    {
        List<LineOvertimeHour> GetLineOvertimeHoureByOtDate(DateTime? otDate);
        bool SendOvertimeHour(Guid? prepairedBy, DateTime? otDate);
        DataTable GetOvertimeHoureByOtDate(DateTime date, bool all, bool garments, bool knitting, bool dyeing);
        int SecondApprovedOtHours(long[] lineOvertimeHourIds, Guid? userId);
        DataTable GetLineWiseEmployeeOTHours(string transactionDate, int departmentLineId);
        int FirsApprovedOtHours(long[] lineOvertimeHourIds, Guid? userId);
    }
}
