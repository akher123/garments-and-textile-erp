using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.Repository.MerchandisingRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class PortOfLoadingManager : IPortOfLoadingManager
    {
        private readonly IPortOfLoadingRepository _portOfLoadingRepository;
        private readonly IBuyOrdShipRepository _buyOrdShipRepository;
        private readonly string _compId;
        public PortOfLoadingManager(IPortOfLoadingRepository portOfLoadingRepository, IBuyOrdShipRepository buyOrdShipRepository)
        {
            _buyOrdShipRepository = buyOrdShipRepository;
            _compId = PortalContext.CurrentUser.CompId;
            _portOfLoadingRepository = portOfLoadingRepository;
        }

        public List<OM_PortOfLoading> GetPortOfLoading()
        {
            return _portOfLoadingRepository.All().ToList();
        }

        public List<OM_PortOfLoading> GetPortOfLoadingsByPaging(OM_PortOfLoading model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var portOfLoading = _portOfLoadingRepository.All().Include(x=>x.Country).Where(x => x.CompId == _compId
                && ((x.PortOfLoadingName.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.PortOfLoadingRefId.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                ));
            totalRecords = portOfLoading.Count();
            switch (model.sort)
            {
                case "PortOfLoadingName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            portOfLoading = portOfLoading
                                .OrderByDescending(r => r.PortOfLoadingName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            portOfLoading = portOfLoading
                                .OrderBy(r => r.PortOfLoadingName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "PortOfLoadingRefId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            portOfLoading = portOfLoading
                                .OrderByDescending(r => r.PortOfLoadingRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            portOfLoading = portOfLoading
                                .OrderBy(r => r.PortOfLoadingRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    portOfLoading = portOfLoading
                        .OrderByDescending(r => r.PortOfLoadingRefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return portOfLoading.ToList();
        }

        public string GetNewPortOfLoadingfId()
        {
            return _portOfLoadingRepository.GetNewPortOfLoadingfId(_compId);
        }

        public OM_PortOfLoading GetPortOfLoadingById(int portOfLoadingId)
        {
            return _portOfLoadingRepository.FindOne(x => x.PortOfLoadingId == portOfLoadingId);
        }

        public int EditPortOfLoading(OM_PortOfLoading model)
        {
            var portOfLoading = _portOfLoadingRepository.FindOne(x => x.PortOfLoadingId == model.PortOfLoadingId);
            portOfLoading.PortOfLoadingName = model.PortOfLoadingName;
            portOfLoading.PortType = model.PortType;
            portOfLoading.CountryId = model.CountryId;
        
            return _portOfLoadingRepository.Edit(portOfLoading);
        }

        public int SavePortOfLoading(OM_PortOfLoading model)
        {
            model.PortOfLoadingRefId = _portOfLoadingRepository.GetNewPortOfLoadingfId(_compId);
            model.CompId = _compId;
            return _portOfLoadingRepository.Save(model);
        }

        public int DeletePortOfLoading(string portOfLoadingRefId)
        {
            bool isUsesd = _buyOrdShipRepository.Exists(x => x.PortOfLoadingRefId == portOfLoadingRefId);
            var deleted = 0;
            if (isUsesd)
            {
                deleted = -1;
            }
            else
            {
                deleted = _portOfLoadingRepository.Delete(x => x.PortOfLoadingRefId == portOfLoadingRefId);
            }
            return deleted;
        }

        public bool CheckExistingPortOfLoading(OM_PortOfLoading model)
        {
           return _portOfLoadingRepository.Exists(
                x =>
                    x.CompId == _compId && x.PortOfLoadingId != model.PortOfLoadingId &&
                    x.PortOfLoadingName.ToLower().Equals(model.PortOfLoadingName));
        }
    }
}
