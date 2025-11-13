using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Objects;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model;
using SCERP.Model.AccountingModel;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.AccountingRepository
{
    public class JournalVoucherEntryRepository : Repository<Acc_VoucherMaster>, IJournalVoucherEntryRepository
    {
        public JournalVoucherEntryRepository(SCERPDBContext context)
            : base(context)
        {

        }


        public string SaveMaster(Acc_VoucherMaster voucherMaster)
        {
            try
            {
                {
                    Context.Acc_VoucherMaster.Add(voucherMaster);
                    var saved = Context.SaveChanges();
                }
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

                if (voucherDetail.RefId > 0)
                {
                    Id = voucherDetail.RefId;
                }
                else
                {
                    var temp =
                        Context.Acc_VoucherMaster.Where(
                            p => p.IsActive == true)
                            .OrderByDescending(p => p.Id)
                            .FirstOrDefault();

                    if (temp == null)
                        Id = 1;
                    else
                        Id = temp.Id;
                }

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

        public Acc_VoucherMaster GetAccVoucherMasterById(long Id)
        {
            return Context.Acc_VoucherMaster.SingleOrDefault(p => p.IsActive == true && p.Id == Id);
        }

        public string GetFinancialPeriodById(int fpId)
        {
            return Context.Acc_FinancialPeriod.SingleOrDefault(p => p.IsActive == true && p.Id == fpId).PeriodName;
        }

        public IQueryable<Acc_VoucherDetail> GetGlVoucherDetail(long Id)
        {
            return
                Context.Acc_VoucherDetail.Include("Acc_GLAccounts")
                    .Where(p => p.RefId == Id)
                    .OrderBy(p => p.Acc_GLAccounts.Id);
        }

        public List<string> GetAccountNames()
        {
            var hiddenStatus = (from p in Context.Acc_GLAccounts_Hidden_Statuses
                               select p.Status).FirstOrDefault();


            var hiddenList = from p in Context.Acc_GLAccounts_Hiddens
                             where p.IsActive == true
                             select p;

            var temp = from p in Context.Acc_GLAccounts
                       where p.IsActive == true
                       select p;

            if (hiddenStatus == true)
            {
                foreach (var qs in hiddenList)
                {
                    temp = temp.Where(x => x.AccountCode != qs.AccountCode);
                }
            }

            List<string> lt = new List<string>();

            foreach (var t in temp)
            {
                lt.Add((t.AccountName + "-" + t.AccountCode));
            }

            return lt;
        }

        public List<string> GetAccountNamesThirdLayer()
        {
            var temp = from p in Context.Acc_ControlAccounts
                       where p.IsActive == true && p.ControlLevel==4
                       select p;

            List<string> lt = new List<string>();

            foreach (var t in temp)
            {
                lt.Add((t.ControlName + "-" + t.ControlCode));
            }

            return lt;
        }

        public List<string> GetSubGroupAndControlNames()
        {
            var temp = from p in Context.Acc_ControlAccounts
                       where p.IsActive == true && p.ControlCode > 10000
                       orderby p.ControlCode

                       select p;

            List<string> lt = new List<string>();

            foreach (var t in temp)
            {
                lt.Add((t.ControlName + "-" + t.ControlCode));
            }
            return lt;
        }

        public List<string> GetControlNames()
        {
            var temp = from p in Context.Acc_ControlAccounts
                       where p.IsActive == true && p.ControlCode > 1000000
                orderby p.ControlCode
               
                select p;

            List<string>lt = new List<string>();

            foreach (var t in temp)
            {
                lt.Add((t.ControlName + "-" + t.ControlCode));
            }
            return lt;
        }

        public List<string> GetControlSummaryNames()
        {
            var temp = from p in Context.Acc_ControlAccounts
                       where p.IsActive == true && p.ControlLevel == 3
                       orderby p.ControlCode

                       select p;

            List<string> lt = new List<string>();

            foreach (var t in temp)
            {
                lt.Add((t.ControlName + "-" + t.ControlCode));
            }
            return lt;
        }

        public string GetAccountNamesById(int Id)
        {
            var temp = Context.Acc_GLAccounts.FirstOrDefault(p => p.Id == Id && p.IsActive == true);
            return (temp.AccountCode + "-" + temp.AccountName).ToLower();
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
            var accountId = (from p in Context.Acc_GLAccounts
                where p.IsActive == true && p.AccountCode == AccountCode
                select p.Id).FirstOrDefault();

            return accountId;
        }

        public IQueryable<Acc_CompanySector> GetAllCompanySector()
        {
            return Context.Acc_CompanySector.Where(x => x.IsActive == true).OrderBy(x => x.SortOrder);
        }

        public List<Acc_CompanySector> GetAllActiveCompanySectory(Guid? employeeId)
        {
            int? companyId = 0;
            var activecompanySector = Context.Acc_ActiveCompanySector.FirstOrDefault(p => p.EmployeeId == employeeId && p.IsActive == true);

            if (activecompanySector != null)
                companyId = activecompanySector.CompanyId;

            var companySectory = Context.Acc_CompanySector.Where(p => p.IsActive == true);

            foreach (var t in companySectory)
            {
                if (t.Id == companyId)
                    t.IsActive = true;
                else
                    t.IsActive = false;
            }
            return companySectory.ToList();
        }

        public int SaveActiveCompanySector(Guid? employeeId, int? companySectorId)
        {
            try
            {
                var companySectory = (from p in Context.Acc_ActiveCompanySector
                                      where p.EmployeeId == employeeId && p.IsActive == true
                                      select p).FirstOrDefault();

                if (companySectory != null)
                    companySectory.CompanyId = companySectorId;
                else
                {
                    var sector = new Acc_ActiveCompanySector();
                    sector.EmployeeId = employeeId;
                    sector.CompanyId = companySectorId;
                    sector.IsActive = true;

                    Context.Acc_ActiveCompanySector.Add(sector);
                }

                Context.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
         
            return 1;
        }

        public IQueryable<Acc_CostCentre> GetAllCostCentres(int sectorId)
        {
            return Context.Acc_CostCentre.Where(r => r.IsActive == true && r.SectorId == sectorId).OrderBy(x => x.SortOrder);                                    
        }

        public IQueryable<Acc_CostCentreMultiLayer> GetAllCostCentres()
        {
            return Context.Acc_CostCentreMultiLayer.Where(r => r.IsActive == true && r.ItemLevel == 3).OrderBy(x => x.SortOrder);
        }

        public IQueryable<Acc_GLAccounts> GetGlAccountsByMasterId(long id)
        {
            var glId = from p in Context.Acc_VoucherDetail
                where p.RefId == id
                select p.GLID;

            return Context.Acc_GLAccounts.Where(s => glId.Contains(s.Id));
        }

        public long GetLatestVoucherNo()
        {
            long latestVoucherNo = 1;

            var accVoucherMaster = Context.Acc_VoucherMaster.OrderByDescending(p => p.VoucherNo).FirstOrDefault();

            if (accVoucherMaster != null)
            {
                latestVoucherNo = accVoucherMaster.VoucherNo + 1;
            }
            return latestVoucherNo;
        }

        public string CheckGLHeadValidation(string glHead)
        {
            try
            {
                var glHeadCode2 = glHead.Substring(glHead.Length - 10, 10);

                var glHeadCode = Convert.ToDecimal(glHeadCode2);

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

        public long GetVoucherNo(int fiscalPeriodId)
        {
            try
            {
                var temp =
                    Context.Acc_VoucherMaster.Where(
                        p => p.IsActive == true && p.FinancialPeriodId == fiscalPeriodId && p.VoucherType == "JV")
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
