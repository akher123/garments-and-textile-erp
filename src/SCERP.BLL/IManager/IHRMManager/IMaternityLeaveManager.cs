using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IMaternityLeaveManager
    {
        List<MaternityLeaveInfo> GetMaternityLeaveInfoByPaging(int startPage, int pageSize, out int totalRecords, HrmMaternityPayment model, SearchFieldModel searchFieldModel);

        HrmMaternityPayment GetMaternityPaymentById(int maternityPaymentId);

        int SaveMaternityPayment(HrmMaternityPayment maternity);

        int EditMaternityPayment(HrmMaternityPayment maternity);

        Employee GetEmployeeIdByCardId(string employeeCardId);

        Employee GetEmployeeCardIdByEmployeeId(Guid employeeId);
    }
}
