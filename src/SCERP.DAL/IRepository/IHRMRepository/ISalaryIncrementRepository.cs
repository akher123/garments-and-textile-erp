using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Data;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface ISalaryIncrementRepository
    {
        DataTable GetSalaryIncrementInfo(DateTime fromDate, DateTime toDate, string employeeId, string userName);
    }
}
