using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.DAL.Repository.CommercialRepository;
using SCERP.Model.CommercialModel;

namespace SCERP.BLL.Manager.CommercialManager
{
    public class BankAdviceManager : BaseManager, IBankAdviceManager
    {
        private readonly IBankAdviceRepository _iBankAdviceRepository = null;

        public BankAdviceManager(SCERPDBContext context)
        {
            _iBankAdviceRepository = new BankAdviceRepository(context);
        }

        public List<CommBankAdvice> GetAllBankAdvicesByPaging(int startPage, int pageSize, out int totalRecords, CommBankAdvice bankAdvice)
        {
            List<CommBankAdvice> commBankAdvices = null;
            commBankAdvices = _iBankAdviceRepository.GetAllBankAdvicesByPaging(startPage, pageSize, out totalRecords, bankAdvice);
            return commBankAdvices;
        }

        public List<CommBankAdvice> GetAllBankAdvices()
        {
            List<CommBankAdvice> commBankAdvice = null;
            commBankAdvice = _iBankAdviceRepository.Filter(x => x.IsActive).OrderBy(x => x.BankAdviceId).ToList();
            return commBankAdvice;
        }

        public CommBankAdvice GetBankAdviceById(int? id)
        {
            CommBankAdvice commBankAdvice = null;
            commBankAdvice = _iBankAdviceRepository.GetBankAdviceById(id);
            return commBankAdvice;
        }

        public CommBankAdvice GetBankAdviceByExportAndHeadId(long? exportId, int? accHeadId, out int count)
        {
            CommBankAdvice commBankAdvice = null;
            commBankAdvice = _iBankAdviceRepository.GetBankAdviceByExportAndHeadId(exportId, accHeadId, out count);
            return commBankAdvice;
        }

        public List<CommBankAdvice> GetBankAdviceByExportId(Int64 id)
        {
            List<CommBankAdvice> bankAdvices = null;
            bankAdvices = _iBankAdviceRepository.GetBankAdviceByExportId(id);
            return bankAdvices;
        }

        public List<CommAccHead> GetAccHead(string type)
        {
            List<CommAccHead> accHead = null;
            accHead = _iBankAdviceRepository.GetAccHead(type);
            return accHead;
        }

        public List<CommAccHead> GetAccHead(string type, Int64 exportId)
        {
            List<CommAccHead> accHead = null;
            accHead = _iBankAdviceRepository.GetAccHead(type, exportId);     
            return accHead;
        }

        public bool CheckExistingBankAdvice(CommBankAdvice bankAdvice)
        {
            bool isExist = false;
            isExist = _iBankAdviceRepository.Exists(p => p.IsActive == true && p.BankAdviceId != bankAdvice.BankAdviceId);
            return isExist;
        }

        public int SaveBankAdvice(CommBankAdvice bankAdvice)
        {
            int savedCommBankAdvice = 0;
            bankAdvice.CreatedDate = DateTime.Now;
            bankAdvice.CreatedBy = PortalContext.CurrentUser.UserId;
            bankAdvice.IsActive = true;
            savedCommBankAdvice = _iBankAdviceRepository.Save(bankAdvice);
            return savedCommBankAdvice;
        }

        public int EditBankAdvice(CommBankAdvice bankAdvice)
        {
            int editedCommBankAdvice = 0;
            bankAdvice.EditedDate = DateTime.Now;
            bankAdvice.EditedBy = PortalContext.CurrentUser.UserId;
            editedCommBankAdvice = _iBankAdviceRepository.Edit(bankAdvice);
            return editedCommBankAdvice;
        }

        public int DeleteBankAdvice(CommBankAdvice bankAdvice)
        {
            int deletedCommBankAdvice = 0;
            bankAdvice.EditedDate = DateTime.Now;
            bankAdvice.EditedBy = PortalContext.CurrentUser.UserId;
            bankAdvice.IsActive = false;
            deletedCommBankAdvice = _iBankAdviceRepository.Edit(bankAdvice);
            return deletedCommBankAdvice;
        }

        public List<CommBankAdvice> GetBankAdviceBySearchKey(int searchByCountry, string searchByBankAdvice)
        {
            List<CommBankAdvice> bankAdvices = null;
            bankAdvices = _iBankAdviceRepository.GetBankAdviceBySearchKey(searchByCountry, searchByBankAdvice);
            return bankAdvices;
        }

    }
}
