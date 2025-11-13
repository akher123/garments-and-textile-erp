using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Data;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface ISalaryIncrementManager
    {
        string GetSalaryIncrementInfo(DateTime fromDate, DateTime toDate, string employeeId, string userName);
    }
}
