using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Data;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeJobCardManager
    {
        DataTable GetEmployeeJobCardInfo(Guid? employeeId, DateTime? startDate, DateTime? endDate);
    }
}
