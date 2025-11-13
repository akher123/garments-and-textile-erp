using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.Repository.MerchandisingRepository;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class OmBuyerManager : IOmBuyerManager
    {
        private readonly OmBuyerRepository _omBuyerRepository;
        private readonly string _compId;
        private readonly IBuyerOrderRepository _buyerOrderRepository;
        private readonly IOmBuyerRepository _buyerRepository;
        public OmBuyerManager(IOmBuyerRepository buyerRepository, OmBuyerRepository omBuyerRepository, IBuyerOrderRepository buyerOrderRepository)
        {
            _buyerOrderRepository = buyerOrderRepository;
            _compId = PortalContext.CurrentUser.CompId;
            _omBuyerRepository = omBuyerRepository;
            _buyerRepository = buyerRepository;
        }

        public List<OM_Buyer> GetAllBuyers()
        {
            return _omBuyerRepository.Filter(x => x.CompId == _compId).OrderBy(x => x.BuyerName).ToList();
        }

        public List<OM_Buyer> GetBuyersByPaging(OM_Buyer model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var buyerList= _omBuyerRepository.Filter(x => x.CompId == _compId
                && (
                (x.BuyerName.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.Phone.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.Fax.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.EMail.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.BuyerRefId.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))));
            totalRecords = buyerList.Count();
            switch (model.sort)
            {
                case "BuyerName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            buyerList = buyerList
                                .OrderByDescending(r => r.BuyerName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            buyerList = buyerList
                                .OrderBy(r => r.BuyerName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "BuyerRefId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            buyerList = buyerList
                                .OrderByDescending(r => r.BuyerRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            buyerList = buyerList
                                .OrderBy(r => r.BuyerRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    buyerList = buyerList
                        .OrderByDescending(r => r.BuyerRefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return buyerList.ToList();
        }

        public OM_Buyer GetBuyerById(long buyerId)
        {
            return _omBuyerRepository.FindOne(x => x.BuyerId == buyerId && x.CompId == _compId);
        }

        public string GetNewBuyerRefId()
        {
            return _omBuyerRepository.GetNewBuyerRefId(_compId);
        }

        public int EditBuyer(OM_Buyer model)
        {
            var buyer = _omBuyerRepository.FindOne(x => x.BuyerId == model.BuyerId && x.CompId == _compId);
            buyer.BuyerName = model.BuyerName;
            buyer.Address1 = model.Address1;
            buyer.Address2 = model.Address2;
            buyer.Address3 = model.Address3;
            buyer.Address3 = model.Address3;
            buyer.CountryId = model.CountryId;
            buyer.CityId = model.CityId;
            buyer.Phone = model.Phone;
            buyer.Fax = model.Fax;
            buyer.EMail = model.EMail;
            return _omBuyerRepository.Edit(buyer);
        }

        public int SaveBuyer(OM_Buyer model)
        {
            model.CompId = _compId;
                bool isExist=   _omBuyerRepository.Exists(
                x =>
                    x.CompId == _compId &&
                    x.BuyerName.Trim().Replace(" ", "").ToLower() == model.BuyerName.Trim().Replace(" ", "").ToLower());
            if (!isExist)
            {
                model.BuyerRefId = _omBuyerRepository.GetNewBuyerRefId(_compId);
                return _omBuyerRepository.Save(model);
            }
            else
            {
                throw new Exception(model.BuyerName + " Buyer Name Already Exist !");
            }
            

        }

        public int DeleteDelete(string buyerRefId)
        {
            var isUsesd = _buyerOrderRepository.Exists(x => x.BuyerRefId == buyerRefId && x.CompId == _compId);
            var deleted = 0;
            if (isUsesd)
            {
                deleted = -1;
            }
            else
            {
                deleted = _omBuyerRepository.Delete(x => x.BuyerRefId == buyerRefId&&x.CompId==_compId);
            }
            return deleted;
        }

        public bool CheckExistingBuyer(OM_Buyer model)
        {
           return _omBuyerRepository.Exists(
                x =>
                    x.CompId == _compId && x.BuyerId != model.BuyerId &&
                    x.BuyerName.Replace(" ", "").ToLower().Equals(model.BuyerName.Replace(" ", "").ToLower()));

        }

        public object GetCuttingProcessStyleActiveBuyers()
        {
            return _buyerRepository.GetCuttingProcessStyleActiveBuyers();
        }

        public OM_Buyer GetBuyerByRefId(string buyerRefId, string compId)
        {
            return _omBuyerRepository.FindOne(x => x.BuyerRefId == buyerRefId && x.CompId == _compId);
        }
    }
}
