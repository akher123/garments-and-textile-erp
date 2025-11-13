using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{

  
    public class InquiryManager : IInquiryManager
    {
        private IInquiryReository _inquiryReository;

        public InquiryManager(IInquiryReository inquiryReository)
        {
            _inquiryReository = inquiryReository;
        }
        public OM_Inquiry GetInquiryById(int inquiryInquiryId)
        {
          return  _inquiryReository.FindOne(x => x.InquiryId == inquiryInquiryId);
        }

        public int SaveInquiry(OM_Inquiry modelInquiry)
        {
           return _inquiryReository.Save(modelInquiry);
        }

        public List<OM_Inquiry> GetInquiriesByPaging(int pageIndex, string sort, string sortdir, string searchString, string compId,out int totalRecords){
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var inquiryList = _inquiryReository.Filter(x => x.CompId == compId  && x.IsActive && (x.BuyerName.Contains(searchString) || string.IsNullOrEmpty(searchString)) && (x.InquiryRef.Contains(searchString) || string.IsNullOrEmpty(searchString)));
            totalRecords = inquiryList.Count();
            inquiryList = inquiryList.OrderByDescending(r => r.InquiryRef).Skip(index * pageSize).Take(pageSize);
            return inquiryList.ToList();
        }

        public string GetInqueryRefId(string compId)
        {
            var dyeingSpChallanRefId = _inquiryReository.Filter(x => x.CompId == compId).Max(x => x.InquiryRef) ?? "0";
            return  dyeingSpChallanRefId.IncrementOne().PadZero(6);
        }
    }
}
