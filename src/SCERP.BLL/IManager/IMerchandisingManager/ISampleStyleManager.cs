using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface ISampleStyleManager
    {
        List<OM_SampleStyle> GetSampleStyles(int sampleOrderId);

        OM_SampleStyle GetSampleStyleById(int sampleStyleId);

        int EditSampleStyle(OM_SampleStyle sampleStyle);
        int SaveSampleStyle(OM_SampleStyle sampleStyle);
        int DeleteSampleStyle(OM_SampleStyle sampleStyle);
    }
}
