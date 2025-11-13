using SCERP.BLL.IManager.ICommercialManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.DAL.Repository.CommercialRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommercialModel;
using SCERP.Common;
using System.Transactions;
using SCERP.DAL.IRepository;

namespace SCERP.BLL.Manager.CommercialManager
{
    
    public class CashBbLcManager : BaseManager,ICashBbLcManager
    {
        private readonly ICashBbLcRepository _cashBbLcRepository = null;
        private readonly IRepository<CommCashBbLcDetail> _cashBbLcDetailRepository=null;
        public CashBbLcManager(SCERPDBContext context, IRepository<CommCashBbLcDetail> consDetailRepository)
        {
            _cashBbLcRepository = new CashBbLcRepository(context);
            _cashBbLcDetailRepository = consDetailRepository;
        }

        public bool CheckExistingCashBbLcInfo(CommCashBbLcInfo bblcInfo)
        {
            bool isExist = false;

            if (bblcInfo.BbLcId == 0 && _cashBbLcRepository.Exists(p => p.IsActive == true && p.BbLcNo.Trim().ToLower() == bblcInfo.BbLcNo.Trim().ToLower()))
                isExist = true;

            else if (bblcInfo.BbLcId > 0 && _cashBbLcRepository.Exists(p => p.IsActive == true && p.BbLcId != bblcInfo.BbLcId && p.BbLcNo.Trim().ToLower() == bblcInfo.BbLcNo.Trim().ToLower()))
                isExist = true;

            return isExist;
        }

        public int DeleteCashBbLcInfo(CommCashBbLcInfo bbLcInfo)
        {
            int deletedCommBbLcInfo = 0;
            bbLcInfo.EditedDate = DateTime.Now;
            bbLcInfo.EditedBy = PortalContext.CurrentUser.UserId;
            bbLcInfo.IsActive = false;
            deletedCommBbLcInfo = _cashBbLcRepository.Edit(bbLcInfo);
            return deletedCommBbLcInfo;
        }

        public int EditCashBbLcInfo(CommCashBbLcInfo cashBbLcInfo)
        {
            
            int edited = 0;
            using (var transaction = new TransactionScope())
            {

                
                _cashBbLcDetailRepository.Delete(x => x.BbLcId == cashBbLcInfo.BbLcId);
                edited += _cashBbLcRepository.Edit(cashBbLcInfo);
                foreach (var detail in cashBbLcInfo.CommCashBbLcDetail)
                {
                    detail.BbLcId = cashBbLcInfo.BbLcId;
                    edited += _cashBbLcDetailRepository.Save(detail);
                }
                transaction.Complete();
            }
            return edited;
        }

        public List<CommCashBbLcInfo> GetAllCashBbLcInfos()
        {
            List<CommCashBbLcInfo> commCashBbLcInfo = null;
            commCashBbLcInfo = _cashBbLcRepository.Filter(x => x.IsActive).OrderBy(x => x.BbLcId).ToList();
            return commCashBbLcInfo;
        }

        public List<CommCashBbLcInfo> GetAllCashBbLcInfosByPaging(out int totalRecords, CommCashBbLcInfo model)
        {
            var index = model.PageIndex;
            string compId = PortalContext.CurrentUser.CompId;
            var pageSize = AppConfig.PageSize;

            IQueryable<CommCashBbLcInfo> bbLcInfos = _cashBbLcRepository.GetWithInclude(x => x.CompId == compId && (x.LcRefId == model.LcRefId || model.LcRefId == 0) && x.IsActive
                                                    && (x.BbLcType == model.BbLcType || model.BbLcType == null) && (x.SupplierCompanyRefId == model.SupplierCompanyRefId || model.SupplierCompanyRefId == null) && ((x.BbLcDate >= model.FromDate || model.FromDate == null) && (x.BbLcDate <= model.ToDate || model.ToDate == null)), "CommCashBbLcDetail");
            


          totalRecords = bbLcInfos.Count();

            switch (model.sort)
            {
                case "Id":

                    switch (model.sortdir)
                    {
                        case "DESC":
                            bbLcInfos = bbLcInfos
                                .OrderByDescending(r => r.BbLcId).ThenBy(x => x.BbLcId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            bbLcInfos = bbLcInfos
                                .OrderBy(r => r.BbLcId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    bbLcInfos = bbLcInfos
                        .OrderBy(r => r.BbLcId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return bbLcInfos.ToList();
        }

        public CommCashBbLcInfo GetCashBbLcIdByBbLcNo(string bbLcNo)
        {
            throw new NotImplementedException();
        }

        public CommCashBbLcInfo GetCashBbLcInfoById(int? id)
        {
            CommCashBbLcInfo commCashBbLcInfo = null;
            //commCashBbLcInfo = _cashBbLcRepository.GetCashBbLcInfoById(id);
            commCashBbLcInfo = _cashBbLcRepository.GetWithInclude(x => x.BbLcId == id, "CommCashBbLcDetail").FirstOrDefault();
            return commCashBbLcInfo;
        }

        public List<CommCashBbLcInfo> GetCashBbLcInfoBySearchKey(int searchByCountry, string searchByCashBbLcInfo)
        {
            throw new NotImplementedException();
        }

        public int SaveCashBbLcInfo(CommCashBbLcInfo cashBblcInfo)
        {
            int savedCommCashBbLcInfo = 0;
            cashBblcInfo.CreatedDate = DateTime.Now;
            cashBblcInfo.CreatedBy = PortalContext.CurrentUser.UserId;
            cashBblcInfo.IsActive = true;
            savedCommCashBbLcInfo = _cashBbLcRepository.Save(cashBblcInfo);
            return savedCommCashBbLcInfo;
        }
    }
}
