using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class PaymentTermManager : IPaymentTermManager
    {
        private readonly IPaymentTermRepository _paymentTermRepository;
        private readonly IBuyerOrderRepository _buyerOrderRepository;
        private readonly string _compId;
        public PaymentTermManager(IPaymentTermRepository paymentTermRepository, IBuyerOrderRepository buyerOrderRepository)
        {
            _compId = PortalContext.CurrentUser.CompId;
            _paymentTermRepository = paymentTermRepository;
            _buyerOrderRepository = buyerOrderRepository;
        }

        public List<OM_PaymentTerm> GetPaymentTerms()
        {
            return _paymentTermRepository.Filter(x=>x.CompId==_compId).OrderBy(x=>x.PayTerm).ToList();
        }

        public List<OM_PaymentTerm> GetPaymentTermByPaging(OM_PaymentTerm model, out int totalRecords)
        {

            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var paymentTerms = _paymentTermRepository.Filter(x => x.CompId == _compId &&
              ((x.PayTerm.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.PayTermRefId.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))));
            totalRecords = paymentTerms.Count();
            switch (model.sort)
            {
                case "PayTerm":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            paymentTerms = paymentTerms
                                .OrderByDescending(r => r.PayTerm)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            paymentTerms = paymentTerms
                                .OrderBy(r => r.PayTerm)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "PayTermRefId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            paymentTerms = paymentTerms
                                .OrderByDescending(r => r.PayTermRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            paymentTerms = paymentTerms
                                .OrderBy(r => r.PayTermRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "PayType":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            paymentTerms = paymentTerms
                                .OrderByDescending(r => r.PayType)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            paymentTerms = paymentTerms
                                .OrderBy(r => r.PayType)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    paymentTerms = paymentTerms
                        .OrderByDescending(r => r.PayTermRefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
        
            return paymentTerms.ToList();
        }

        public OM_PaymentTerm GetPaymentTermById(int payentTermId)
        {
            return _paymentTermRepository.FindOne(x => x.PayentTermId == payentTermId&&x.CompId==_compId);
        }

        public string GetPayTermRef()
        {
            return _paymentTermRepository.GetPayTermRef(_compId);
        }

        public int EditPaymentTerm(OM_PaymentTerm model)
        {
            var paymentTerm = _paymentTermRepository.FindOne(x => x.PayentTermId == model.PayentTermId && x.CompId == _compId);
          paymentTerm.PayTerm = model.PayTerm;
          paymentTerm.PayTermRefId = model.PayTermRefId;
          paymentTerm.ECGCPerc = model.ECGCPerc;
          paymentTerm.InsurancePerc = model.InsurancePerc;
          paymentTerm.CreditDays = model.CreditDays;
          paymentTerm.PayType = model.PayType;
          return _paymentTermRepository.Edit(paymentTerm);
        }

        public int SavePaymentTerm(OM_PaymentTerm model)
        {
            model.CompId = _compId;
            model.PayTermRefId = _paymentTermRepository.GetPayTermRef(_compId);
            return _paymentTermRepository.Save(model);
        }

        public int DeletePaymentTerm(string payTermRefId)
        {
            var isUsesd = _buyerOrderRepository.Exists(x => x.PayTermRefId == payTermRefId && x.CompId == _compId);
            var deleted = 0;
            if (isUsesd)
            {
                deleted = -1;
            }
            else
            {
                deleted = _paymentTermRepository.Delete(x => x.PayTermRefId == payTermRefId && x.CompId == _compId);
            }
            return deleted;
        }

        public bool CheckExistingPaymentTerm(OM_PaymentTerm model)
        {
           return _paymentTermRepository.Exists(
                x => x.CompId == _compId && x.PayentTermId != model.PayentTermId && x.PayTerm == model.PayTerm);
        }
    }
}
