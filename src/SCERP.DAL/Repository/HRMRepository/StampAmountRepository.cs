using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Linq;


namespace SCERP.DAL.Repository.HRMRepository
{
    public class StampAmountRepository : Repository<StampAmount>, IStampAmountRepository
    {
        public StampAmountRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public StampAmount GetStampAmountById(int? id)
        {
            return Context.StampAmount.FirstOrDefault(x => x.StampAmountId == id);
        }

        public List<StampAmount> GetAllStampAmountsByPaging(int startPage, int pageSize, out int totalRecords, StampAmount stampAmount)
        {
            IQueryable<StampAmount> stampAmounts = Context.StampAmount.Where(p => p.IsActive == true); 

            try
            {
                DateTime? searchByFromDate = null;
                if (stampAmount.FromDate > new DateTime(2000, 1, 1))
                    searchByFromDate = stampAmount.FromDate;

                var searchByToDate = stampAmount.ToDate;

                if (searchByFromDate != null && searchByToDate != null)
                    stampAmounts = stampAmounts.Where(x => x.FromDate <= searchByFromDate && x.ToDate >= searchByToDate);
                totalRecords = stampAmounts.Count();

                switch (stampAmount.sort)
                {
                    case "Amount":

                        switch (stampAmount.sortdir)
                        {
                            case "DESC":
                                stampAmounts = stampAmounts
                                    .OrderByDescending(r => r.Amount)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;

                            default:
                                stampAmounts = stampAmounts
                                    .OrderBy(r => r.Amount)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;

                    case "FromDate":

                        switch (stampAmount.sortdir)
                        {
                            case "DESC":
                                stampAmounts = stampAmounts
                                    .OrderByDescending(r => r.FromDate)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;

                            default:
                                stampAmounts = stampAmounts
                                    .OrderBy(r => r.FromDate)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;

                    case "ToDate":

                        switch (stampAmount.sortdir)
                        {
                            case "DESC":
                                stampAmounts = stampAmounts
                                    .OrderByDescending(r => r.ToDate)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;

                            default:
                                stampAmounts = stampAmounts
                                    .OrderBy(r => r.ToDate)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;


                    default:
                        stampAmounts = stampAmounts.OrderBy(r => r.FromDate).Skip(startPage*pageSize).Take(pageSize);
                        break;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return stampAmounts.ToList();
        }

        public List<StampAmount> GetAllStampAmounts()
        {
            return Context.StampAmount.Where(x => x.IsActive == true).OrderBy(y => y.FromDate).ToList();
        }

        public List<StampAmount> GetStampAmountBySearchKey(decimal searchByAmount, DateTime searchByFromDate, DateTime searchByToDate)
        {
            List<StampAmount> stampAmounts = null;

            try
            {
                stampAmounts = Context.StampAmount.Where(x => x.IsActive == true && x.Amount == searchByAmount && x.FromDate <= searchByFromDate && x.ToDate >= searchByToDate).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return stampAmounts;
        }

        public StampAmount GetLatestStampAmountInfo()
        {
            StampAmount stampAmount = null;
            try
            {
                stampAmount = Context.StampAmount.FirstOrDefault(x => x.IsActive == true && x.FromDate <= DateTime.Now);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
            return stampAmount;
        }

        public int UpdateLatestStampInfoDate(StampAmount stampAmount)
        {
            var updated = 0;
            try
            {
                updated = Edit(stampAmount);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return updated;
        }
    }
}
