using SCERP.Model.MarketingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.IManager.IMarketingManager
{
    public interface IMarketingInquiryManager
    {
        List<MarketingInquiry> GetMarketingInquiry
           (int pageIndex, string sort, string sortdir, string searchString,int marketingPersonId, out int totalRecords);

        MarketingInquiry GetMarketingInquiryById(int inquiryId);
        List<MarketingStatus> GetAllMarketingStatus();
        List<MarketingInquiry> GetAllMarketingInquiry();
        int EditMarketingInquiry(MarketingInquiry marketingInquiry);
        int SaveMarketingInquiry(MarketingInquiry marketingInquiry);
        int DeleteMarketingInquiry(MarketingInquiry marketingInquiry);
        List<MarketingPerson> GetMarketingPersons();
    }
}
