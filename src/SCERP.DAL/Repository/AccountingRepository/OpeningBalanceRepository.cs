using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.AccountingRepository
{
    public class OpeningBalanceRepository : Repository<Acc_OpeningClosing>, IOpeningBalanceRepository
    {
        public OpeningBalanceRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public override IQueryable<Acc_OpeningClosing> All()
        {
            return Context.Acc_OpeningClosing.Where(x => x.IsActive == true);
        }

        public Acc_OpeningClosing GetOpeningBalaceById(long? id)
        {
            return Context.Acc_OpeningClosing.Find(id);
        }

        public List<Acc_OpeningClosing> GetAllOpeningBalaces(int page, int records, string sort,
            ref List<decimal> totalAmount, int? fpId, int? sectorId, string glId)
        {
            var openingBalance =
                Context.Acc_OpeningClosing.Include("Acc_CompanySector")
                    .Include("Acc_FinancialPeriod")
                    //.Include("Acc_GLAccounts")
                    .Where(p => p.IsActive == true)
                    .ToList();

            if (fpId > 0)
                openingBalance = openingBalance.Where(p => p.FpId == fpId).ToList();

            if (sectorId > 0)
                openingBalance = openingBalance.Where(p => p.SectorId == sectorId).ToList();

            if (!string.IsNullOrEmpty(glId) && glId.Length > 10)
            {
                glId = glId.Substring(0, 10);

                decimal AccCode;

                if (Decimal.TryParse(glId, out AccCode))
                {
                    int? temp = (from p in Context.Acc_GLAccounts
                        where p.AccountCode == AccCode
                        select p).FirstOrDefault().Id;

                    openingBalance = openingBalance.Where(p => p.GlId == temp).ToList();
                }
            }

            foreach (var t in openingBalance)
            {
                if (t.Debit != null) totalAmount[0] += t.Debit.Value;
                if (t.Credit != null) totalAmount[1] += t.Credit.Value;
                //totalAmount[2] += (t.Debit.Value - t.Credit.Value == null ? 0 : t.Credit.Value);
            }

            switch (sort)
            {
                //case "Title":
                //    OpeningBalace = OpeningBalace.OrderBy(r => r.Title).ToList();
                //    break;
                //case "Description":
                //    OpeningBalace = OpeningBalace.OrderBy(r => r.Description).ToList();
                //    break;
                //default:
                //    OpeningBalace = OpeningBalace.OrderBy(r => r.Id).ToList();
                //    break;
            }

            openingBalance = openingBalance.Skip(page*records).Take(records).ToList();
            return openingBalance;
        }

        public IQueryable<Acc_OpeningClosing> GetAllOpeningBalaces()
        {
            var openingBalance = Context.Acc_OpeningClosing.Where(r => r.IsActive == true);
            return openingBalance;
        }

        public IQueryable<Acc_FinancialPeriod> GetFinancialPeriod()
        {
            return Context.Acc_FinancialPeriod.Where(x => x.IsActive == true).OrderByDescending(p => p.SortOrder);
        }

        public IQueryable<Acc_CompanySector> GetSector()
        {
            return Context.Acc_CompanySector.Where(x => x.IsActive == true);
        }

        public IQueryable<Acc_GLAccounts> GetGLAccounts()
        {
            return Context.Acc_GLAccounts.Where(x => x.IsActive == true);
        }

        public List<string> GetAccountNames()
        {
            var temp = from p in Context.Acc_GLAccounts
                where p.IsActive == true
                select p;

            List<string> lt = new List<string>();

            foreach (var t in temp)
            {
                lt.Add((t.AccountCode + "-" + t.AccountName).ToLower());
            }

            return lt;
        }

        public string GetAccountNamesById(int Id)
        {
            var temp = Context.Acc_GLAccounts.FirstOrDefault(p => p.Id == Id && p.IsActive == true);
            if (temp == null)
                return "";
            else
                return (temp.AccountCode + "-" + temp.AccountName).ToLower();
        }

        public int Save(Acc_OpeningClosing open, string accountHead)
        {
            int count = 0;
            int accountId = 0;
            long refId = 392634;
            DateTime date = Convert.ToDateTime("2016-06-30");
            decimal accountCode = 0;
            decimal? difference = 0;

            accountCode = Convert.ToDecimal(accountHead);

            if (open.Debit >= 0 && open.Credit >= 0)
                return 0;

            if (open.Debit == null && open.Credit == null)
                return 0;
       
            count = Context.Acc_GLAccounts.Count(p => p.AccountCode == accountCode);
            if (count == 0)
                return 0;

            var accGlAccounts = Context.Acc_GLAccounts.FirstOrDefault(p => p.AccountCode == accountCode);
            if (accGlAccounts != null) accountId = accGlAccounts.Id;

            decimal? debit = (from p in Context.Acc_VoucherDetail
                join q in Context.Acc_VoucherMaster on p.RefId equals q.Id
                where p.GLID == accountId && q.VoucherDate <= date
                select p.Debit).Sum();

            decimal? credit = (from p in Context.Acc_VoucherDetail
                join q in Context.Acc_VoucherMaster on p.RefId equals q.Id
                where p.GLID == accountId && q.VoucherDate <= date
                select p.Credit).Sum();

            var voucherDetail = new Acc_VoucherDetail();

            if (debit == null)
                debit = 0;
            if (credit == null)
                credit = 0;

            if (open.Debit > 0)
            {
                difference = open.Debit - (debit - credit);

                if (difference > 0)
                {
                    voucherDetail = new Acc_VoucherDetail
                    {
                        RefId = refId,
                        GLID = accountId,
                        Particulars = "Opening Balance",
                        Debit = difference,
                        Credit = 0,
                        FirstCurValue = 1
                    };
                }
                else
                {
                    voucherDetail = new Acc_VoucherDetail
                    {
                        RefId = refId,
                        GLID = accountId,
                        Particulars = "Opening Balance",
                        Debit = 0,
                        Credit = -difference,
                        FirstCurValue = 1
                    };
                }
            }

            else if (open.Credit > 0)
            {
                difference = open.Credit + (debit - credit);

                if (difference > 0)
                {
                    voucherDetail = new Acc_VoucherDetail
                    {
                        RefId = refId,
                        GLID = accountId,
                        Particulars = "Opening Balance",
                        Debit = 0,
                        Credit = difference,
                        FirstCurValue = 1
                    };
                }
                else
                {
                    voucherDetail = new Acc_VoucherDetail
                    {
                        RefId = refId,
                        GLID = accountId,
                        Particulars = "Opening Balance",
                        Debit = -difference,
                        Credit = 0,
                        FirstCurValue = 1
                    };
                }
            }

            if (voucherDetail.Debit == null)
                voucherDetail.Debit = 0;
            if (voucherDetail.Credit == null)
                voucherDetail.Credit = 0;
            if (voucherDetail.RefId == null)
                voucherDetail.RefId = refId;
            if (voucherDetail.GLID == null)            
                voucherDetail.GLID = accountId;
            if (voucherDetail.FirstCurValue == 0)
                voucherDetail.FirstCurValue = 1;
            if (voucherDetail.SecendCurValue == 0)
                voucherDetail.SecendCurValue = 85;
            if (voucherDetail.ThirdCurValue == 0)
                voucherDetail.ThirdCurValue = 78;

            Context.Acc_VoucherDetail.Add(voucherDetail);
            Context.SaveChanges();

            return 1;
        }
    }
}