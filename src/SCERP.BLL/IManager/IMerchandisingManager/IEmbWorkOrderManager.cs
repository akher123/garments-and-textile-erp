using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface IEmbWorkOrderManager
    {
        List<OM_EmbWorkOrder> GetEmbWorkOrders(int pageIndex, string sort, string sortdir, out int totalRecords, string searchString,long partyId);
        string GetNewRefId(string compId);
        OM_EmbWorkOrder GetEmbWorkOrderById(int embWorkOrderId);
        int SaveEmbWorkOrder(OM_EmbWorkOrder embWorkOrder);
        int DeleteEmbWorkOrder(int embWorkOrderId);
        IEnumerable GetEmbWorkOrderDetails(int workOrderId);
        OM_EmbWorkOrderDetail GetEmbWorkOrderDetailById(int embWorkOrderDetailId);
        int SaveEmbWorkOrderDetail(OM_EmbWorkOrderDetail embWorkOrderDetail);
        int DeleteEmbWorkOrderDetail(int embWorkOrderDetailId);
    }
}
