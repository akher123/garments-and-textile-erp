using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.AccountingRepository
{
    public class BankVoucherEntryRepository : Repository<Acc_VoucherMaster>, IBankVoucherEntryRepository
    {
        public BankVoucherEntryRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public string SaveMaster(Acc_VoucherMaster voucherMaster)
        {
            try
            {
                Context.Acc_VoucherMaster.Add(voucherMaster);
                var saved = Context.SaveChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string SaveDetail(Acc_VoucherDetail voucherDetail)
        {
            try
            {
                long? Id = 1;

                var temp =
                    Context.Acc_VoucherMaster.Where(
                        p => p.IsActive == true)
                        .OrderByDescending(p => p.Id)
                        .FirstOrDefault();

                if (temp == null)
                    Id = 1;
                else
                    Id = temp.Id;

                var count = 1;

                voucherDetail.RefId = Id;

                Context.Acc_VoucherDetail.Add(voucherDetail);
                Context.SaveChanges();

                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public IQueryable<Acc_VoucherDetail> GetBankVoucherDetail(long Id)
        {
            var voucherType = Context.Acc_VoucherMaster.FirstOrDefault(p => p.Id == Id).VoucherType;

            var temp = Context.Acc_VoucherDetail.Include("Acc_GLAccounts")
                .Where(p => p.RefId == Id).OrderByDescending(p => p.Credit);

            if (voucherType == "BR")
                temp = temp.OrderByDescending(p => p.Debit);

            return temp;
        }

        public List<string> GetAccountNames()
        {
            var temp = from p in Context.Acc_GLAccounts
                       where p.IsActive == true && p.AccountType != "Bank"
                       select p;

            List<string> lt = new List<string>();

            foreach (var t in temp)
            {
                lt.Add((t.AccountCode + "-" + t.AccountName).ToLower());
            }

            return lt;
        }

        public string DeleteDetail(long Id)
        {
            try
            {
                List<Acc_VoucherDetail> detail = Context.Acc_VoucherDetail.Where(p => p.RefId == Id).ToList();

                foreach (var t in detail)
                {
                    Context.Acc_VoucherDetail.Remove(t);
                    Context.SaveChanges();
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public List<string> GetBankAccountNames()
        {
            var temp = from p in Context.Acc_GLAccounts
                       where p.IsActive == true && p.AccountType == "Bank"
                       select p;

            List<string> lt = new List<string>();

            foreach (var t in temp)
            {
                lt.Add((t.AccountCode + "-" + t.AccountName).ToLower());
            }

            return lt;
        }

        public string GetFinancialPeriodById(int fpId)
        {
            return Context.Acc_FinancialPeriod.SingleOrDefault(p => p.IsActive == true && p.Id == fpId).PeriodName;
        }

        public string GetBankInHand()
        {
            var temp = (from p in Context.Acc_GLAccounts
                        where p.IsActive == true && p.AccountType == "Bank" && p.AccountName.Contains("Cash at Bank")
                        select p).FirstOrDefault();

            return (temp.AccountCode + "-" + temp.AccountName);
        }

        public string GetPeriodName()
        {
            var PeriodName = (from p in Context.Acc_FinancialPeriod
                              where p.IsActive == true && p.ActiveStatus == true
                              orderby p.SortOrder
                              select p.PeriodName).FirstOrDefault();

            return PeriodName;
        }

        public int GetPeriodId()
        {
            var PeriodId = (from p in Context.Acc_FinancialPeriod
                            where p.IsActive == true && p.ActiveStatus == true
                            orderby p.SortOrder
                            select p.Id).FirstOrDefault();

            return PeriodId;
        }

        public int GetAccountId(decimal AccountCode)
        {
            var AccountId = (from p in Context.Acc_GLAccounts
                             where p.IsActive == true && p.AccountCode == AccountCode
                             select p.Id).FirstOrDefault();

            return AccountId;
        }

        public Acc_VoucherMaster GetAccVoucherMasterById(long Id)
        {
            return Context.Acc_VoucherMaster.SingleOrDefault(p => p.IsActive == true && p.Id == Id);
        }

        public IQueryable<Acc_CompanySector> GetAllCompanySector()
        {
            return Context.Acc_CompanySector.Where(x => x.IsActive == true).OrderBy(x => x.SortOrder);
        }

        public IQueryable<Acc_CostCentre> GetAllCostCentres(int sectorId)
        {
            return
                Context.Acc_CostCentre.Where(r => r.IsActive == true && r.SectorId == sectorId)
                    .OrderBy(x => x.SortOrder);
        }

        public string CheckGLHeadValidation(string glHead)
        {
            try
            {
                var glHeadCode = Convert.ToDecimal(glHead.Substring(0, 10));

                var temp = from p in Context.Acc_GLAccounts
                           where p.AccountCode == glHeadCode
                           select p;

                if (temp.Count() == 1)
                    return "1";
                else
                    return "0";
            }
            catch (Exception)
            {
                return "0";
            }
        }

        public long GetVoucherNo(int fiscalPeriodId, string voucherType)
        {
            try
            {
                var temp =
                    Context.Acc_VoucherMaster.Where(
                        p => p.IsActive == true && p.FinancialPeriodId == fiscalPeriodId && p.VoucherType == voucherType)
                        .OrderByDescending(p => p.VoucherNo)
                        .FirstOrDefault();

                if (temp == null)
                    return 1;
                else
                    return temp.VoucherNo + 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
