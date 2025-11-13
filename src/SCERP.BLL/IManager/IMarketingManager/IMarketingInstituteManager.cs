using SCERP.Model.MarketingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.IManager.IMarketingManager
{
    public interface IMarketingInstituteManager
    {
        List<MarketingInstitute> GetMarketingtInstitute
            (int pageIndex, string sort, string sortdir, string searchString, out int totalRecords);

        List<MarketingInstitute> GetAllMarketingtInstitute();
        MarketingInstitute GetMarketingtInstituteById(int marketingInstituteId);
        List<MarketingStatus> GetAllMarketingStatus();
        int EditMarketingtInstitute(MarketingInstitute marketingInstitute);
        int SaveMarketingtInstitute(MarketingInstitute marketingInstitute);
        int DeleteMarketingtInstitute(MarketingInstitute marketingInstitute);

        
    }
}
