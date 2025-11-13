using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface ISampleOrderManager
    {
        List<OM_SampleOrder> GetSampleOrder
            (int pageIndex, string sort, string sortdir, string searchString, out int totalRecords);

        List<OM_SampleOrder> GetAllSampleOrder();
        OM_SampleOrder GetSampleOrderById(int sampleOrderId);
        int EditSampleOrder(OM_SampleOrder sampleOrder);
        int SaveSampleOrder(OM_SampleOrder sampleOrder);
        int DeleteSampleOrder(OM_SampleOrder sampleOrder);
        string GetNewRefId(string compId);
        OM_SampleOrder GetSampleOrderByRefId(string searchString);
    }
}
