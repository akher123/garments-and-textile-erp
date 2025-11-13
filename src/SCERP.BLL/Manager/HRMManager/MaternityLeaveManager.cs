using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.Manager.HRMManager
{
    public class MaternityLeaveManager : BaseManager, IMaternityLeaveManager
    {
        protected readonly IMaternityLeaveRepository MaternityLeaveRepository = null;

        public MaternityLeaveManager(SCERPDBContext context)
        {
            MaternityLeaveRepository = new MaternityLeaveRepository(context);
        }

        public List<MaternityLeaveInfo> GetMaternityLeaveInfoByPaging(int startPage, int pageSize, out int totalRecords, HrmMaternityPayment model, SearchFieldModel searchFieldModel)
        {
            return MaternityLeaveRepository.GetMaternityLeaveInfoByPaging(startPage, pageSize, out totalRecords, model, searchFieldModel);
        }

        public HrmMaternityPayment GetMaternityPaymentById(int maternityPaymentId)
        {
            return MaternityLeaveRepository.GetMaternityPaymentById(maternityPaymentId);
        }

        public int SaveMaternityPayment(HrmMaternityPayment maternity)
        {
            var savedMaternity = 0;

            maternity.CompId = "001";
            maternity.CreatedDate = DateTime.Now;
            maternity.CreatedBy = PortalContext.CurrentUser.UserId;
            maternity.IsActive = true;
            savedMaternity = MaternityLeaveRepository.Save(maternity);

            return savedMaternity;
        }

        public int EditMaternityPayment(HrmMaternityPayment maternity)
        {
            var editMaternity = 0;

            maternity.CompId = "001";
            maternity.EditedDate = DateTime.Now;
            maternity.EditedBy = PortalContext.CurrentUser.UserId;
            editMaternity = MaternityLeaveRepository.Edit(maternity);

            return editMaternity;
        }


        public Employee GetEmployeeIdByCardId(string employeeCardId)
        {
            return MaternityLeaveRepository.GetEmployeeIdByCardId(employeeCardId);
        }

        public Employee GetEmployeeCardIdByEmployeeId(Guid employeeId)
        {
            return MaternityLeaveRepository.GetEmployeeCardIdByEmployeeId(employeeId);
        }
    }
}
