using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface IOrderTypeManager
    {
        List<OM_OrderType> GetOrdertypes();
        List<OM_OrderType> GetOrderTypesByPaging(OM_OrderType model, out int totalRecords);
        OM_OrderType GetOrderTypeById(int orderTypeId);
        string GetNewOTypeRefId();
        int EditOrderType(OM_OrderType model);
        int SaveOrderType(OM_OrderType model);
        int DeleteOType(string oTypeRefId);
        bool CheckExistingOrderType(OM_OrderType model);
    }
}
