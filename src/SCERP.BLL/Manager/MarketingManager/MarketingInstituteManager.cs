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
    public class MarketingInstituteManager : IMarketingInstituteManager
    {
        private IRepository<MarketingInstitute> _marketingInstituteRepository;

        private IRepository<MarketingStatus> _marketingStatusRepository;

        public MarketingInstituteManager(IRepository<MarketingInstitute> marketingInstituteRepository, IRepository<MarketingStatus> marketingStatusRepository)
        {
            _marketingInstituteRepository = marketingInstituteRepository;
            _marketingStatusRepository = marketingStatusRepository;
        }

        public List<MarketingInstitute> GetAllMarketingtInstitute()
        {
            return _marketingInstituteRepository.All().OrderBy(x=>x.InstituteName).ToList();
        }

        public List<MarketingInstitute> GetMarketingtInstitute(int pageIndex, string sort, string sortdir, string searchString, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var marketingInstitutes =
                _marketingInstituteRepository.Filter(x => x.InstituteName.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString)
                                                 || x.District.Contains(searchString) || String.IsNullOrEmpty(searchString) || x.DecisionMaker.Contains(searchString) || String.IsNullOrEmpty(searchString) || x.Telephone.Contains(searchString) || String.IsNullOrEmpty(searchString));
            totalRecords = marketingInstitutes.Count();
            switch (sort)
            {
                case "Name":
                    switch (sortdir)
                    {
                        case "DESC":
                            marketingInstitutes = marketingInstitutes
                                .OrderByDescending(r => r.InstituteId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            marketingInstitutes = marketingInstitutes
                                .OrderBy(r => r.InstituteId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    marketingInstitutes = marketingInstitutes
                        .OrderByDescending(r => r.InstituteId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return marketingInstitutes.ToList();
        }

        public MarketingInstitute GetMarketingtInstituteById(int marketingInstituteId)
        {
            return _marketingInstituteRepository.FindOne(x => x.InstituteId == marketingInstituteId);
        }

        public List<MarketingStatus> GetAllMarketingStatus()
        {
            return _marketingStatusRepository.All().ToList();
        }

        

        public int EditMarketingtInstitute(MarketingInstitute marketingInstitute)
        {
            MarketingInstitute model = _marketingInstituteRepository.FindOne(x => x.InstituteId == marketingInstitute.InstituteId);
            model.InstituteName = marketingInstitute.InstituteName;
            model.District = marketingInstitute.District;
            model.Address = marketingInstitute.Address;
            model.DecisionMaker = marketingInstitute.DecisionMaker;
            model.Designation = marketingInstitute.Designation;
            model.Mobile = marketingInstitute.Mobile;
            model.Telephone = marketingInstitute.Telephone;
            model.Email = marketingInstitute.Email;
            model.WebSite = marketingInstitute.WebSite;
            model.IsAvailable = marketingInstitute.IsAvailable;
            model.Remarks = marketingInstitute.Remarks;
            model.StatusId = marketingInstitute.StatusId;
            model.ClientEntryDate = marketingInstitute.ClientEntryDate;
            model.IsActive = marketingInstitute.IsActive;
            return _marketingInstituteRepository.Edit(model);
        }

        public int SaveMarketingtInstitute(MarketingInstitute marketingInstitute)
        {
            return _marketingInstituteRepository.Save(marketingInstitute);
        }

        public int DeleteMarketingtInstitute(MarketingInstitute marketingInstitute)
        {
            return _marketingInstituteRepository.DeleteOne(marketingInstitute);
        }
    }
}
