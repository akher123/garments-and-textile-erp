using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ConsigneeManager : IConsigneeManager
    {
        private readonly IBuyerOrderRepository _buyerOrderRepository;
        private readonly IConsigneeRepository _consigneeRepository;
        private readonly string _compId;
        public ConsigneeManager(IBuyerOrderRepository buyerOrderRepository, IConsigneeRepository consigneeRepository)
        {
            _buyerOrderRepository = buyerOrderRepository;
            _consigneeRepository = consigneeRepository;
            _compId = PortalContext.CurrentUser.CompId;
        }

        public List<OM_Consignee> GetConsignees()
        {
            return _consigneeRepository.Filter(x => x.CompId == _compId).OrderBy(x => x.ConsigneeName).ToList();
        }

        public List<OM_Consignee> GetConsigneesByPaging(OM_Consignee model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var consigneeList = _consigneeRepository.Filter(x => x.CompId == _compId
                && (
                (x.ConsigneeRefId.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.Phone.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.Fax.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.EMail.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                  || (x.ConsigneeName.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.BuyerRefId.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))));
            totalRecords = consigneeList.Count();
            switch (model.sort)
            {
                case "ConsigneeName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            consigneeList = consigneeList
                                .OrderByDescending(r => r.ConsigneeName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            consigneeList = consigneeList
                                .OrderBy(r => r.ConsigneeName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "ConsigneeRefId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            consigneeList = consigneeList
                                .OrderByDescending(r => r.ConsigneeRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            consigneeList = consigneeList
                                .OrderBy(r => r.ConsigneeRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    consigneeList = consigneeList
                        .OrderBy(r => r.ConsigneeRefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return consigneeList.ToList();
        }

        public OM_Consignee GetConsigneeById(long consigneeId)
        {
            return _consigneeRepository.FindOne(x => x.ConsigneeId == consigneeId && x.CompId == _compId);
        }

        public string GetNewConsigneeRefId()
        {
            return _consigneeRepository.GetNewConsigneeRefId(_compId);
        }

        public int EditConsignee(OM_Consignee model)
        {
            var consinee = _consigneeRepository.FindOne(x => x.ConsigneeId == model.ConsigneeId && x.CompId == _compId);
            consinee.ConsigneeName = model.ConsigneeName;
            consinee.Address1 = model.Address1;
            consinee.Address2 = model.Address2;
            consinee.Address3 = model.Address3;
            consinee.Address3 = model.Address3;
            consinee.CountryId = model.CountryId;
            consinee.CItyId = model.CItyId;
            consinee.Phone = model.Phone;
            consinee.Fax = model.Fax;
            consinee.EMail = model.EMail;
            consinee.BuyerRefId = model.BuyerRefId;
            return _consigneeRepository.Edit(consinee);
        }

        public int SaveConsignee(OM_Consignee model)
        {
            model.CompId = _compId;
            return _consigneeRepository.Save(model);
        }

        public int DeleteConsignee(string consigneeRefId)
        {
            var isUsesd = _buyerOrderRepository.Exists(x => x.ConsigneeRefId == consigneeRefId&&x.CompId==_compId);
            var deleted = 0;
            if (isUsesd)
            {
                deleted = -1;
            }
            else
            {
                deleted = _consigneeRepository.Delete(x => x.ConsigneeRefId == consigneeRefId && x.CompId == _compId);
            }
            return deleted;
        }

        public bool CheckExistingConsignee(OM_Consignee model)
        {
          return  _consigneeRepository.Exists(
                x =>
                    x.CompId == _compId && x.ConsigneeId != model.ConsigneeId &&
                    x.ConsigneeName==model.ConsigneeName);
        }
    }
}
