using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model.HRMModel;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeManualOverTimeRepository:Repository<EmployeeManualOverTime>,IEmployeeManualOverTimeRepository
    {
        public EmployeeManualOverTimeRepository(SCERPDBContext context) : base(context)
        {


        }
    }
}
