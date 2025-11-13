using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.DAL.Repository.CommercialRepository;
using SCERP.Model.CommercialModel;

namespace SCERP.BLL.Manager.CommercialManager
{
    public class BbLcManager : BaseManager, IBbLcManager
    {
        private readonly IBbLcRepository _bbLcRepository = null;
        private readonly IRepository<CommBbLcItemDetails> _bbLcItemDetailsRepository = null;
        public BbLcManager(SCERPDBContext context, IRepository<CommBbLcItemDetails> bbLcItemDetailsRepository)
        {
            _bbLcRepository = new BbBbLcRepository(context);
            _bbLcItemDetailsRepository = bbLcItemDetailsRepository;
        }

        public List<CommBbLcInfo> GetAllBbLcInfosByPaging(out int totalRecords, CommBbLcInfo model)
        {

            var index = model.PageIndex;
            string compId = PortalContext.CurrentUser.CompId;
            var pageSize = AppConfig.PageSize;

            IQueryable<CommBbLcInfo> bbLcInfos = _bbLcRepository.GetWithInclude(x => x.CompId == compId
                                               && (x.IsActive)
                                               && (x.IssuingBankId == model.IssuingBankId || model.IssuingBankId == null)
                                               && (x.BbLcNo.Contains(model.BbLcNo) || model.BbLcNo == "")
                                               && (x.LcRefId == model.LcRefId || model.LcRefId == 0)
                                               && (x.BbLcType == model.BbLcType || model.BbLcType == null)
                                               && (x.SupplierCompanyRefId == model.SupplierCompanyRefId || model.SupplierCompanyRefId == null)
                                               && ((x.BbLcDate >= model.FromDate || model.FromDate == null) && (x.BbLcDate <= model.ToDate || model.ToDate == null))
                                               && ((x.MatureDate >= model.MaturityDateFrom || model.MaturityDateFrom == null) && (x.MatureDate <= model.MaturityDateTo || model.MaturityDateTo == null))
                                               && ((x.ExpiryDate >= model.ExpiryDateFrom || model.ExpiryDateFrom == null) && (x.ExpiryDate <= model.ExpiryDateTo || model.ExpiryDateTo == null))
                                               && (x.MatureDate == null || model.DonothaveMaturityDate == false)
                                               && (x.ExpiryDate == null || model.DonothaveExpiryDate == false)
                                               && (x.COMMLcInfo.RStatus == "O"), "COMMLcInfo", "Mrc_SupplierCompany");

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
                        .OrderByDescending(r => r.BbLcId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return bbLcInfos.ToList();
        }
        public List<CommBbLcInfo> GetAllBbLcInfos()
        {
            List<CommBbLcInfo> commBbLcInfo = null;
            commBbLcInfo = _bbLcRepository.Filter(x => x.IsActive).OrderBy(x => x.BbLcId).ToList();
            return commBbLcInfo;
        }
        public CommBbLcInfo GetBbLcInfoById(int? id)
        {
            CommBbLcInfo commBbLcInfo = null;
            commBbLcInfo = _bbLcRepository.GetWithInclude(x => x.BbLcId == id, "CommBbLcItemDetails").FirstOrDefault();
            return commBbLcInfo;
        }
        public CommBbLcInfo GetBbLcIdByBbLcNo(string bbLcNo)
        {
            return _bbLcRepository.GetBbLcIdByBbLcNo(bbLcNo);
        }
        public bool CheckExistingBbLcInfo(CommBbLcInfo bblcInfo)
        {
            bool isExist = false;

            if (bblcInfo.BbLcId == 0 && _bbLcRepository.Exists(p => p.IsActive == true && p.BbLcNo.Trim().ToLower() == bblcInfo.BbLcNo.Trim().ToLower()))
                isExist = true;

            else if (bblcInfo.BbLcId > 0 && _bbLcRepository.Exists(p => p.IsActive == true && p.BbLcId != bblcInfo.BbLcId && p.BbLcNo.Trim().ToLower() == bblcInfo.BbLcNo.Trim().ToLower()))
                isExist = true;
            return isExist;
        }

        public int SaveBbLcInfo(CommBbLcInfo bblcInfo)
        {
            int savedCommBbLcInfo = 0;
            bblcInfo.CreatedDate = DateTime.Now;
            bblcInfo.CreatedBy = PortalContext.CurrentUser.UserId;
            bblcInfo.IsActive = true;
            savedCommBbLcInfo = _bbLcRepository.Save(bblcInfo);
            return savedCommBbLcInfo;
        }

        public int EditBbLcInfo(CommBbLcInfo bblcInfo)
        {
            int editedCommBbLcInfo = 0;
            bblcInfo.EditedDate = DateTime.Now;
            bblcInfo.EditedBy = PortalContext.CurrentUser.UserId;
            editedCommBbLcInfo = _bbLcRepository.Edit(bblcInfo);
            return editedCommBbLcInfo;
        }

        public int DeleteBbLcInfo(CommBbLcInfo bblcInfo)
        {
            int deletedCommBbLcInfo = 0;
            bblcInfo.EditedDate = DateTime.Now;
            bblcInfo.EditedBy = PortalContext.CurrentUser.UserId;
            bblcInfo.IsActive = false;
            deletedCommBbLcInfo = _bbLcRepository.Edit(bblcInfo);
            return deletedCommBbLcInfo;
        }

        public List<CommBbLcInfo> GetBbLcInfoBySearchKey(int searchByCountry, string searchByBbLcInfo)
        {
            List<CommBbLcInfo> bblcInfos = null;
            bblcInfos = _bbLcRepository.GetBbLcInfoBySearchKey(searchByCountry, searchByBbLcInfo);
            return bblcInfos;
        }
    }
}
