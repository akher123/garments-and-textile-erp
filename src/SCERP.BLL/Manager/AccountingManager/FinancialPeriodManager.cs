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
    public class FinancialPeriodManager : BaseManager, IFinancialPeriodManager
    {

        private IFinancialPeriodRepository FinancialPeriodRepository = null;

        public FinancialPeriodManager(SCERPDBContext context)
        {
            this.FinancialPeriodRepository = new FinancialPeriodRepository(context);
        }

        public List<Acc_FinancialPeriod> GetAllFinancialPeriods(int page, int records, string sort)
        {
            return FinancialPeriodRepository.GetAllFinancialPeriods(page, records, sort);
        }

        public Acc_FinancialPeriod GetFinancialPeriodById(int? id)
        {
            return FinancialPeriodRepository.GetFinancialPeriodById(id);
        }

        public int SaveFinancialPeriod(Acc_FinancialPeriod aFinancialPeriod)
        {
            aFinancialPeriod.IsActive = true;
            aFinancialPeriod.ActiveStatus = true;

            int savedFinancialPeriod = 0;

            try
            {
                if (
                    FinancialPeriodRepository.Exists(
                        p =>
                            p.PeriodName == aFinancialPeriod.PeriodName && aFinancialPeriod.Id == 0 &&
                            p.IsActive == true))
                    return 2;

                if (aFinancialPeriod.PeriodStartDate > aFinancialPeriod.PeriodEndDate)
                    return 3;

                IQueryable<Acc_FinancialPeriod> fcp = FinancialPeriodRepository.All().Where(p => p.IsActive == true);

                DateTime? dt = new DateTime(1900, 1, 1);

                foreach (var t in fcp)
                {
                    if (dt < t.PeriodEndDate && t.Id != aFinancialPeriod.Id)
                        dt = t.PeriodEndDate;
                }

                //if (aFinancialPeriod.PeriodStartDate <= dt)
                //    return 4;

                foreach (var t in fcp)
                {
                    t.ActiveStatus = false;
                    FinancialPeriodRepository.Save(t);
                }

                aFinancialPeriod.ActiveStatus = true;

                savedFinancialPeriod = FinancialPeriodRepository.Save(aFinancialPeriod);
               
            }
            catch (Exception ex)
            {
                savedFinancialPeriod = 0;
            }

            return savedFinancialPeriod;
        }

        public void DeleteFinancialPeriod(Acc_FinancialPeriod FinancialPeriod)
        {
            FinancialPeriod.IsActive = false;
            FinancialPeriodRepository.Edit(FinancialPeriod);
        }
    }
}
