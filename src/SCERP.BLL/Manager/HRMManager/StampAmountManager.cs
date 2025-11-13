using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SCERP.BLL.Manager.HRMManager
{
    public class StampAmountManager : BaseManager, IStampAmountManager
    {

        private readonly IStampAmountRepository _stampAmountRepository = null;

        public StampAmountManager(SCERPDBContext context)
        {
            _stampAmountRepository = new StampAmountRepository(context);
        }

        public List<StampAmount> GetAllStampAmountsByPaging(int startPage, int pageSize, out int totalRecords, StampAmount stampAmount)
        {
            List<StampAmount> stampAmounts = null;
            try
            {
                stampAmounts = _stampAmountRepository.GetAllStampAmountsByPaging(startPage, pageSize, out totalRecords, stampAmount).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                totalRecords = 0;
            }

            return stampAmounts;
        }

        public List<StampAmount> GetAllStampAmounts()
        {
            List<StampAmount> stampAmount = null;

            try
            {
                stampAmount = _stampAmountRepository.Filter(x => x.IsActive).OrderBy(x => x.FromDate).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                stampAmount = null;
            }

            return stampAmount;
        }

        public StampAmount GetStampAmountById(int? id)
        {
            StampAmount stampAmount = null;
            try
            {
                stampAmount = _stampAmountRepository.GetStampAmountById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                stampAmount = null;
            }

            return stampAmount;
        }

        public bool CheckExistingStampAmount(StampAmount stampAmount)
        {
            bool isExist = false;
            try
            {
                isExist = _stampAmountRepository.Exists(x => x.IsActive == true && x.FromDate >= stampAmount.FromDate);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return isExist;
        }

        public int SaveStampAmount(StampAmount stampAmount)
        {
            var savedStampAmount = 0;
            try
            {
                stampAmount.CreatedDate = DateTime.Now;
                stampAmount.CreatedBy = PortalContext.CurrentUser.UserId;
                stampAmount.IsActive = true;
                savedStampAmount = _stampAmountRepository.Save(stampAmount);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return savedStampAmount;
        }

        public int EditStampAmount(StampAmount stampAmount)
        {
            var editedStampAmount = 0;
            try
            {
                stampAmount.EditedDate = DateTime.Now;
                stampAmount.EditedBy = PortalContext.CurrentUser.UserId;
                editedStampAmount = _stampAmountRepository.Edit(stampAmount);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return editedStampAmount;
        }

        public int DeleteStampAmount(StampAmount stampAmount)
        {
            var deletedStampAmount = 0;
            try
            {
                stampAmount.EditedDate = DateTime.Now;
                stampAmount.EditedBy = PortalContext.CurrentUser.UserId;
                stampAmount.IsActive = false;
                deletedStampAmount = _stampAmountRepository.Edit(stampAmount);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return deletedStampAmount;
        }

        public List<StampAmount> GetStampAmountBySearchKey(decimal searchByAmount, DateTime searchByFromDate, DateTime searchByToDate)
        {
            var stampAmounts = new List<StampAmount>();

            try
            {
                stampAmounts = _stampAmountRepository.GetStampAmountBySearchKey(searchByAmount, searchByFromDate, searchByToDate);
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
                stampAmount = _stampAmountRepository.GetLatestStampAmountInfo();
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
                stampAmount.EditedBy = PortalContext.CurrentUser.UserId;
                stampAmount.EditedDate = DateTime.Now;
                updated = _stampAmountRepository.UpdateLatestStampInfoDate(stampAmount);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return updated;
        }
    }
}
