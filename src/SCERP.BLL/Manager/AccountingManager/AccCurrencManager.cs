using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.DAL.Repository.AccountingRepository;
using SCERP.Model.AccountingModel;

namespace SCERP.BLL.Manager.AccountingManager
{
    public class AccCurrencManager : IAccCurrencManager
    {
        private readonly IAccCurrencyRepository _accCurrencyRepository;
        private IVoucherDetailRepository _voucherDetailRepository;
        public AccCurrencManager(SCERPDBContext context)
        {
            
            _accCurrencyRepository = new AccCurrencyRepository(context);
            _voucherDetailRepository=new VoucherDetailRepository(context);
        }
        public Acc_Currency GetCurrencyById(int currencyId)
        {
            return _accCurrencyRepository.FindOne(x => x.CurrencyId == currencyId && x.ActiveStatus);
        }

        public int SaveCurrency(Acc_Currency model)
        {
            return _accCurrencyRepository.Save(model);
        }

        public List<Acc_Currency> GetCurrencies()
        {
            return _accCurrencyRepository.Filter(x => x.ActiveStatus).ToList();
        }

        public int CurrencyEdit(Acc_Currency model)
        {
            var currency = _accCurrencyRepository.FindOne(x => x.CurrencyId == model.CurrencyId);
            currency.FirstCurName = model.FirstCurName;
            currency.FirstCurValue = model.FirstCurValue;
            currency.FirstCurSymbol = model.FirstCurSymbol;

            currency.SecendCurName = model.SecendCurName;
            currency.SecendCurValue = model.SecendCurValue;
            currency.SecendCurSymbol = model.SecendCurSymbol;

            currency.ThirdCurName = model.ThirdCurName;
            currency.ThirdCurValue = model.ThirdCurValue;
            currency.ThirdCurSymbol = model.ThirdCurSymbol;
            currency.ActiveStatus = model.ActiveStatus;

            int intdex = _voucherDetailRepository.UpdateVoucherDetaiByCurrency(model);
            return _accCurrencyRepository.Edit(currency);
        }
    }
}
