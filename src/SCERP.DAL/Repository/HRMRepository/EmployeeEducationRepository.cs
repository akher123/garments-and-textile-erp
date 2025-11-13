using System.Data.Entity;
using System.Linq;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class  EmployeeEducationRepository:Repository<EmployeeEducation>,IEmployeeEducationRepository
    {
        public EmployeeEducationRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
