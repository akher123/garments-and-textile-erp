using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using SCERP.Common;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model;


namespace SCERP.DAL.Repository.AccountingRepository
{
    public class BankReconcilationRepository : Repository<Acc_VoucherMaster>, IBankReconcilationRepository
    {
        public BankReconcilationRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public List<VoucherList> GetAllVoucherList(int page, int records, string sort,
            int? fpId, DateTime? FromDate, DateTime? ToDate, string AccountName, string SectorId)
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
            var bankReconcil = Context.Acc_BankReconcilationMaster.Where(p => p.GLID != 0);

            if (fpId > 0)
            {
                temp = Context.Acc_VoucherMaster.Where(p => p.FinancialPeriodId == fpId);
                bankReconcil = bankReconcil.Where(p => p.FinancialPeriodId == fpId);
            }

            DateTime dateTime;
            if (DateTime.TryParse(FromDate.ToString(), out dateTime) &&
                DateTime.TryParse(ToDate.ToString(), out dateTime))
            {
                temp = temp.Where(p => p.VoucherDate >= FromDate && p.VoucherDate <= ToDate);
                bankReconcil = bankReconcil.Where(p => p.FromDate >= FromDate && p.ToDate <= ToDate);
            }

            decimal accCode = 0;
            if (!string.IsNullOrEmpty(AccountName))
            {
                accCode = Convert.ToDecimal(AccountName.Substring(0, 10));

                var GlId = Context.Acc_GLAccounts.FirstOrDefault(p => p.AccountCode == accCode).Id;

                var detail = from p in Context.Acc_VoucherDetail
                    where p.GLID == GlId
                    select p.RefId;

                temp = temp.Where(p => detail.Contains(p.Id));
            }

            if (!string.IsNullOrEmpty(SectorId))
            {
                int sectorId = Convert.ToInt32(SectorId);
                temp = temp.Where(p => p.SectorId == sectorId);
                bankReconcil = bankReconcil.Where(p => p.SectorId == sectorId);
            }

            var voucherId = from p in bankReconcil
                join q in Context.Acc_BankReconciliationDetail
                    on p.Id equals q.RefId
                select q.VoucherId;

            var voucher = (from p in temp
                join q in tempAmount on p.Id equals q.id
                join r in Context.Acc_BankReconciliationDetail on p.Id equals r.VoucherId into gp
                from g in gp.DefaultIfEmpty()
                where p.IsActive == true
                select new VoucherList
                {
                    Id = p.Id,
                    Date = p.VoucherDate,
                    VoucherType = p.VoucherType,
                    VoucherNo = p.VoucherNo,
                    Particulars = p.Particulars,
                    Amount = q.Amount.Value,
                    Reconciled = g.IsActive.Value == null ? false : true
                }).ToList();

            voucher = voucher.Skip(page*records).Take(records).ToList();
            return voucher;
        }

        public Acc_VoucherMaster GetVoucherMasterById(long? id)
        {
            return Context.Acc_VoucherMaster.Find(id);
        }

        public IQueryable<Acc_FinancialPeriod> GetFinancialPeriod()
        {
            return Context.Acc_FinancialPeriod.Where(x => x.IsActive == true);
        }

        public IQueryable<Acc_CompanySector> GetAllCompanySector()
        {
            return Context.Acc_CompanySector.Where(x => x.IsActive == true).OrderBy(x => x.SortOrder);
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
            List<DateTime> dt = new List<DateTime>();

            try
            {
                var fp = Context.Acc_FinancialPeriod.FirstOrDefault(p => p.IsActive == true && p.Id == Id);
                if (fp != null)
                {
                    dt.Add(fp.PeriodStartDate.Value);
                    dt.Add((fp.PeriodEndDate.Value));
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return dt;
        }

        public string SaveBankReconMaster(string sectorId, string fpId, string accountName, DateTime? fromDate,
            DateTime? toDate)
        {
            string message = "";
            try
            {
                Acc_BankReconcilationMaster master = new Acc_BankReconcilationMaster();

                   var BankMaster = Context.Acc_BankReconcilationMaster.Where(p => p.Id > 0);

                if (!string.IsNullOrEmpty(fpId))
                {
                    int Fpid = Convert.ToInt32(fpId);
                    master.FinancialPeriodId = Fpid;
                      BankMaster = BankMaster.Where(p => p.FinancialPeriodId == Fpid);
                }

                if (!string.IsNullOrEmpty(sectorId))
                {
                    int SectorId = Convert.ToInt32(sectorId);
                    master.SectorId = SectorId;
                      BankMaster = BankMaster.Where(p => p.SectorId == SectorId);
                }

                decimal AccountCode;
                int GLId = 0;

                if (!string.IsNullOrEmpty((accountName)))
                {
                    AccountCode = Convert.ToDecimal(accountName.Substring(0, 10));

                    GLId = (from p in Context.Acc_GLAccounts
                        where p.IsActive == true && p.AccountCode == AccountCode
                        select p.Id).FirstOrDefault();

                     BankMaster = BankMaster.Where(p => p.GLID == GLId);
                }

                DateTime dateTime;
                if (DateTime.TryParse(fromDate.ToString(), out dateTime) &&
                    DateTime.TryParse(toDate.ToString(), out dateTime))
                {
                     BankMaster = BankMaster.Where(p => p.FromDate >= fromDate && p.ToDate <= toDate);
                }
                var count = BankMaster.Count();
                master.GLID = GLId;
                master.FromDate = fromDate.Value;
                master.ToDate = toDate.Value;
                master.IsActive = true;

                Context.Acc_BankReconcilationMaster.Add(master);
                Context.SaveChanges();

                message= "Success";
            }
            catch (Exception ex)
            {
                Errorlog.WriteLog(ex);
                message= "Error";
            }
            return message;
        }

        public string SaveBankReconDetail(List<long> detail)
        {
            try
            {
                var refId = Context.Acc_BankReconcilationMaster.Max(p => p.Id);
                Acc_BankReconcilationMaster mast =
                    Context.Acc_BankReconcilationMaster.SingleOrDefault(p => p.Id == refId);

                var duplicate = Context.Acc_BankReconciliationDetail.Where(p => detail.Contains(p.VoucherId.Value));

                if (duplicate.Count() > 0)
                {
                    Context.Acc_BankReconcilationMaster.Remove(mast);
                    Context.SaveChanges();
                    return "Error";
                }

                foreach (var dtl in detail)
                {
                    Acc_BankReconciliationDetail det = new Acc_BankReconciliationDetail();

                    det.RefId = Convert.ToInt32(refId);
                    det.VoucherId = Convert.ToInt64(dtl);
                    det.IsActive = true;

                    Context.Acc_BankReconciliationDetail.Add(det);
                    Context.SaveChanges();
                }
                return refId.ToString();
            }
            catch (Exception ex)
            {
                Errorlog.WriteLog(ex);
                return "Error";
            }
        }

        public string SaveManualDetail(Acc_BankVoucherManual bankVoucherManual)
        {
            try
            {
                Context.Acc_BankVoucherManual.Add(bankVoucherManual);
                Context.SaveChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                Errorlog.WriteLog(ex);
                return "Error";
            }
        }

        public List<string> GetAccountNames()
        {
            List<string> lt = new List<string>();

            try
            {
                var temp = from p in Context.Acc_GLAccounts
                           where p.IsActive == true && p.AccountType == "BANK"
                           select p;

                foreach (var t in temp)
                {
                    lt.Add((t.AccountCode + "-" + t.AccountName).ToLower());
                }              
            }
            catch (Exception exception)
            {
               Errorlog.WriteLog(exception);          
            }
            return lt;
        }

        public string CheckDuplicate(List<string> voucherId)
        {
            string message = "";
            try
            {
                int count = 0;

                foreach (var t in voucherId)
                {
                    int VoucherId = Convert.ToInt32(t);
                    var temp = Context.Acc_BankReconciliationDetail.Where(p => p.VoucherId == VoucherId);
                    if (temp.Any())
                    {
                        count++;
                        break;
                    }
                }

                message = count > 0 ? "Duplicate" : "Success";
            }
            catch (Exception exception)
            {                
               Errorlog.WriteLog(exception);
            }
            return message;
        }
    }
}
