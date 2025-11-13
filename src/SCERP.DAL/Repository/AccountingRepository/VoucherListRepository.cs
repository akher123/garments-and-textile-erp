using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model;


namespace SCERP.DAL.Repository.AccountingRepository
{
    public class VoucherListRepository : Repository<Acc_VoucherMaster>, IVoucherListRepository
    {
        public VoucherListRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public List<VoucherList> GetAllVoucherList(int page, int records, string sort, int? fpId, DateTime? FromDate, DateTime? ToDate, string VoucherType, string VoucherNo)         
        {

            var tempAmount = from p in Context.Acc_VoucherDetail
                             group p by p.RefId
                                 into g
                                 select new
                                 {
                                     id = g.Key,
                                     Amount = g.Sum(p => p.Debit)
                                 };

            var temp = Context.Acc_VoucherMaster.Where(p => p.IsActive == false);

            if (fpId > 0)
                temp = Context.Acc_VoucherMaster.Where(p => p.FinancialPeriodId == fpId);

            DateTime dateTime;
            if (DateTime.TryParse(FromDate.ToString(), out dateTime) && DateTime.TryParse(ToDate.ToString(), out dateTime))
            {             
                temp = temp.Where(p => p.VoucherDate >= FromDate && p.VoucherDate <= ToDate);
            }          

            if (!string.IsNullOrEmpty(VoucherType))
                temp = temp.Where(p => p.VoucherType == VoucherType);

            if (!string.IsNullOrEmpty(VoucherNo))
            {
                long voucherNo = Convert.ToInt64(VoucherNo);
                temp = temp.Where(p => p.VoucherNo == voucherNo);
            }

            if (string.IsNullOrEmpty(VoucherType) && string.IsNullOrEmpty(VoucherNo) && FromDate == null && ToDate == null)
            {
                temp = Context.Acc_VoucherMaster.Where(p => p.IsActive == true).OrderByDescending(p => p.Id).Skip(0).Take(10);
            }

            var voucher = (from p in temp
                join q in tempAmount on p.Id equals q.id
                where p.IsActive == true
                select new VoucherList
                {
                    Id = p.Id,
                    Date = p.VoucherDate,
                    VoucherType = p.VoucherType,
                    VoucherNo = p.VoucherNo,
                    Particulars = p.Particulars,
                    Amount = q.Amount.Value
                }).OrderByDescending(p => p.Id).ToList();
       
            voucher = voucher.Skip(page * records).Take(records).ToList();
            return voucher;
        }

        public Acc_VoucherMaster GetVoucherMasterById(long? id)
        {
            return Context.Acc_VoucherMaster.Find(id);
        }

        public IQueryable<Acc_FinancialPeriod> GetFinancialPeriod()
        {
            return Context.Acc_FinancialPeriod.Where(x => x.IsActive == true).OrderByDescending(p=>p.SortOrder);
        }

        public string CheckPeriodFromToDate(int Id, DateTime fromDate, DateTime toDate)
        {
            var fp = Context.Acc_FinancialPeriod.FirstOrDefault(p => p.IsActive == true && p.Id == Id);

            if (fromDate < fp.PeriodStartDate || toDate > fp.PeriodEndDate)
                return "Please select FromDate and ToDate within Financial Period";
            else
                return "Success";
        }

        public List<DateTime> GetPeriodFromToDate(int Id)
        {
            var fp = Context.Acc_FinancialPeriod.FirstOrDefault(p => p.IsActive == true && p.Id == Id);

            List<DateTime> dt = new List<DateTime>();
            dt.Add(fp.PeriodStartDate.Value);
            dt.Add((fp.PeriodEndDate.Value));

            return dt;
        }     
    }
}
