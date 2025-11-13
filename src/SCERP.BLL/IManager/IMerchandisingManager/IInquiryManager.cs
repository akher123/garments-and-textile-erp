using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface IInquiryManager
    {
        OM_Inquiry GetInquiryById(int inquiryInquiryId);
        int SaveInquiry(OM_Inquiry inquiry);
        List<OM_Inquiry> GetInquiriesByPaging
            
            (int modelPageIndex, string modelSort, string modelSortdir,string searchString, string compId, out int totalRecords);

        string GetInqueryRefId(string currentUserCompId);
    }
}
