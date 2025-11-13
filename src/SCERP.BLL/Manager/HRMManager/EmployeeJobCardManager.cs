using System;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using System.Data;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeJobCardManager : BaseManager, IEmployeeJobCardManager
    {
        private readonly IEmployeeJobCardRepository _employeeJobCardRepository = null;

        public EmployeeJobCardManager(SCERPDBContext context)
        {
            _employeeJobCardRepository = new EmployeeJobCardRepository(context);
        }

        public DataTable GetEmployeeJobCardInfo(Guid? employeeId, DateTime? startDate, DateTime? endDate)
        {
            DataTable employeeJobCard = _employeeJobCardRepository.GetEmployeeJobCardInfo(employeeId, startDate, endDate);
            return employeeJobCard;
        }
    }
}
