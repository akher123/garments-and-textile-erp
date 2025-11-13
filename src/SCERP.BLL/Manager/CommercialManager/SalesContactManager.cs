using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.DAL.IRepository;
using SCERP.Model.CommercialModel;

namespace SCERP.BLL.Manager.CommercialManager
{
    public class SalesContactManager : ISalesContactManager
    {
        private readonly IRepository<CommSalseContact> _salseContactRepository;

        public SalesContactManager(IRepository<CommSalseContact> salseContactRepository)
        {
            _salseContactRepository = salseContactRepository;
        }

        public List<CommSalseContact> GetSalesContacts(int lcId)
        {
            return _salseContactRepository.Filter(x => x.IsActive == true && x.LcId == lcId).ToList();
        }

        public List<CommSalseContact> GetAllSalesContacts()
        {
            return _salseContactRepository.All().ToList();
        }

        public int EditSalseContact(CommSalseContact model)
        {
            CommSalseContact salseContact=  _salseContactRepository.FindOne(x => x.SalseContactId == model.SalseContactId);
            salseContact.BuyerId = model.BuyerId;
            salseContact.LcNo = model.LcNo;
            salseContact.LcDate = model.LcDate;
            salseContact.Quantity = model.Quantity;
            salseContact.ShipmentDate = model.ShipmentDate;
            salseContact.Amount = model.Amount;
            salseContact.LcIssuingBank = model.LcIssuingBank;
            salseContact.LcIssuingBankAddress = model.LcIssuingBankAddress;
            salseContact.ReceivingBankAddress = model.ReceivingBankAddress;
            salseContact.ReceivingBankId = model.ReceivingBankId;
            salseContact.Description = model.Description;
            salseContact.EditedBy = model.EditedBy;
            salseContact.EditedDate = model.EditedDate;
            salseContact.ExpiryDate = model.ExpiryDate;
            salseContact.MatureDate = model.MatureDate;
            salseContact.ExtensionDate = model.ExtensionDate;
            salseContact.CashIncentiveDate = model.CashIncentiveDate;
            salseContact.CashIncentiveAmount = model.CashIncentiveAmount;
            salseContact.AuditAmount = model.AuditAmount;
            salseContact.AuditDate = model.AuditDate;

            return _salseContactRepository.Edit(salseContact);

        }

        public int SaveSalseContact(CommSalseContact modelSalseContact)
        {
            return _salseContactRepository.Save(modelSalseContact);
        }

        public CommSalseContact GetSalseContactById(int salseContactId)
        {
            return _salseContactRepository.FindOne(x => x.SalseContactId == salseContactId);
        }

        

        public int DeleteSalesContact(int salseContactId)
        {
            var sacontact = _salseContactRepository.FindOne(x => x.SalseContactId == salseContactId);
            sacontact.IsActive = false;
            return _salseContactRepository.Edit(sacontact);
        }
    }
}
