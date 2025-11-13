using System;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.DAL.Repository.AccountingRepository;
using System.Linq;
using SCERP.Model;
using System.Collections.Generic;

namespace SCERP.BLL.Manager.AccountingManager
{
    public class VoucherListManager : BaseManager, IVoucherListManager
    {
        private IVoucherListRepository voucherListRepository = null;

        public VoucherListManager(SCERPDBContext context)
        {
            voucherListRepository = new VoucherListRepository(context);
        }

        public List<VoucherList> GetAllVoucherList(int page, int records, string sort, int? fpId, DateTime? FromDate, DateTime? ToDate, string VoucherType, string VoucherNo)           
        {
            return voucherListRepository.GetAllVoucherList(page, records, sort, fpId, FromDate, ToDate, VoucherType, VoucherNo);                
        }

        public IQueryable<Acc_FinancialPeriod> GetFinancialPeriod()
        {
            return voucherListRepository.GetFinancialPeriod();
        }

        public Acc_VoucherMaster GetVoucherMasterById(long? id)
        {
            return voucherListRepository.GetVoucherMasterById(id);
        }

        public string CheckPeriodFromToDate(int Id, DateTime fromDate, DateTime toDate)
        {
            return voucherListRepository.CheckPeriodFromToDate(Id, fromDate, toDate);
        }

        public List<DateTime> GetPeriodFromToDate(int Id)
        {
            return voucherListRepository.GetPeriodFromToDate(Id);
        }

        public void DeleteVoucherList(Acc_VoucherMaster voucherMaster)
        {
            voucherMaster.IsActive = false;
            voucherListRepository.Edit(voucherMaster);
        }
    }
}
