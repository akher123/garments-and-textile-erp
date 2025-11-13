using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface IConsigneeManager
    {
        List<OM_Consignee> GetConsignees();
        List<OM_Consignee> GetConsigneesByPaging(OM_Consignee model, out int totalRecords);
        OM_Consignee GetConsigneeById(long consigneeId);
        string GetNewConsigneeRefId();
        int EditConsignee(OM_Consignee model);
        int SaveConsignee(OM_Consignee model);
        int DeleteConsignee(string consigneeRefId);
        bool CheckExistingConsignee(OM_Consignee model);
    }
}
