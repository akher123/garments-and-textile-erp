using SCERP.BLL.IManager.IMarketingManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.Model.MarketingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.Manager.MarketingManager
{
    public class MarketingInquiryManager : IMarketingInquiryManager
    {

        private IRepository<MarketingInquiry> _marketingInquiryRepository;

        private IRepository<MarketingStatus> _marketingStatusRepository;
        private IRepository<MarketingPerson> _marketingPersonRepository; 

        public MarketingInquiryManager(IRepository<MarketingInquiry> marketingInquiryRepository, IRepository<MarketingStatus> marketingStatusRepository, IRepository<MarketingPerson> marketingPersonRepository)
        {
            _marketingInquiryRepository = marketingInquiryRepository;
            _marketingStatusRepository = marketingStatusRepository;
            _marketingPersonRepository = marketingPersonRepository;
        }

        public int DeleteMarketingInquiry(MarketingInquiry marketingInquiry)
        {
            return _marketingInquiryRepository.DeleteOne(marketingInquiry);
        }

        public List<MarketingPerson> GetMarketingPersons()
        {
           return _marketingPersonRepository.Filter(x => x.IsActive).ToList();
        }

        public int EditMarketingInquiry(MarketingInquiry marketingInquiry)
        {
            MarketingInquiry model = _marketingInquiryRepository.FindOne(x => x.InquiryId == marketingInquiry.InquiryId);
            model.InstituteId = marketingInquiry.InstituteId;
            model.InquiryContactPerson = marketingInquiry.InquiryContactPerson;
            model.Mobile = marketingInquiry.Mobile;
            model.Telephone = marketingInquiry.Telephone;
            model.Email = marketingInquiry.Email;
            model.Remarks = marketingInquiry.Remarks;
            model.IsActive = marketingInquiry.IsActive;
            model.Amount = marketingInquiry.Amount;
            model.OthersAmount = marketingInquiry.OthersAmount;
            model.BillNo = marketingInquiry.BillNo;
            model.MarketingPersonId = marketingInquiry.MarketingPersonId;
            return _marketingInquiryRepository.Edit(model);
        }

        public List<MarketingStatus> GetAllMarketingStatus()
        {
            return _marketingStatusRepository.All().ToList();
        }

        public List<MarketingInquiry> GetAllMarketingInquiry()
        {
            return _marketingInquiryRepository.All().ToList();
        }

        public List<MarketingInquiry> GetMarketingInquiry(int pageIndex, string sort, string sortdir, string searchString,int marketingPersonId, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var marketingInquirys =
                _marketingInquiryRepository.GetWithInclude(x =>(x.MarketingPersonId==marketingPersonId|| marketingPersonId==0) && ( x.BillNo.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString)
                                                 || x.Mobile.Contains(searchString) || String.IsNullOrEmpty(searchString)), "MarketingInstitute");
            totalRecords = marketingInquirys.Count();
            switch (sort)
            {
                case "Name":
                    switch (sortdir)
                    {
                        case "DESC":
                            marketingInquirys = marketingInquirys
                                .OrderByDescending(r => r.InquiryId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            marketingInquirys = marketingInquirys
                                .OrderBy(r => r.InquiryId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    marketingInquirys = marketingInquirys
                        .OrderByDescending(r => r.InquiryId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return marketingInquirys.ToList();
        }

        public MarketingInquiry GetMarketingInquiryById(int inquiryId)
        {
            return _marketingInquiryRepository.FindOne(x => x.InquiryId == inquiryId);
        }

        public int SaveMarketingInquiry(MarketingInquiry marketingInquiry)
        {
            return _marketingInquiryRepository.Save(marketingInquiry);
        }
    }
}
