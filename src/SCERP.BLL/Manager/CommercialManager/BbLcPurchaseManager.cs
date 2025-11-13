using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.DAL.Repository.CommercialRepository;
using SCERP.Model;
using SCERP.Model.CommercialModel;

namespace SCERP.BLL.Manager.CommercialManager
{
    public class BbLcPurchaseManager : BaseManager, IBbLcPurchaseManager
    {
        private readonly IBbLcPurchaseRepository _bbLcPurchaseRepository = null;

        public BbLcPurchaseManager(SCERPDBContext context)
        {
            _bbLcPurchaseRepository = new BbLcPurchaseRepository(context);
        }

        public List<VwBbLcPurchaseCommon> GetAllBbLcPurchasesByPaging(int startPage, int pageSize, out int totalRecords, CommBbLcPurchaseCommon bbLcPurchase)
        {
            List<VwBbLcPurchaseCommon> commBbLcPurchases = null;
            commBbLcPurchases = _bbLcPurchaseRepository.GetAllBbLcPurchasesByPaging(startPage, pageSize, out totalRecords, bbLcPurchase);
            return commBbLcPurchases;
        }

        public List<CommBbLcPurchaseCommon> GetAllBbLcPurchases()
        {
            List<CommBbLcPurchaseCommon> commBbLcPurchase = null;
            commBbLcPurchase = _bbLcPurchaseRepository.Filter(x => x.IsActive).OrderBy(x => x.BbLcPurchaseId).ToList();
            return commBbLcPurchase;
        }

        public CommBbLcPurchaseCommon GetBbLcPurchaseById(int? id)
        {
            CommBbLcPurchaseCommon commBbLcPurchase = null;
            commBbLcPurchase = _bbLcPurchaseRepository.GetBbLcPurchaseById(id);
            return commBbLcPurchase;
        }

        public int? GetBbLcIdByLcNo(string lcNo)
        {
            return _bbLcPurchaseRepository.GetBbLcIdByLcNo(lcNo);
        }

        public string GetOrderNoByOrderRefNo(string orderRefNo)
        {
            return _bbLcPurchaseRepository.GetOrderNoByOrderRefNo(orderRefNo);
        }

        public List<OM_BuyOrdStyle> GetStylesByOrderNo(string orderNo)
        {
            return _bbLcPurchaseRepository.GetStylesByOrderNo(orderNo);
        }

        public List<CommBbLcPurchaseCommon> GetBbLcPurchaseByBbLcId(int lcId)
        {
            return _bbLcPurchaseRepository.GetBbLcPurchaseByBbLcId(lcId);
        }

        public bool CheckExistingBbLcPurchase(CommBbLcPurchaseCommon bbLcPurchase)
        {
            bool isExist = false;
            isExist = _bbLcPurchaseRepository.Exists(p => p.IsActive == true && p.BbLcPurchaseId != bbLcPurchase.BbLcPurchaseId);
            return isExist;
        }

        public int SaveBbLcPurchase(CommBbLcPurchaseCommon bbLcPurchase)
        {
            int savedCommBbLcPurchase = 0;   
            bbLcPurchase.CreatedDate = DateTime.Now;
            bbLcPurchase.CreatedBy = PortalContext.CurrentUser.UserId;
            bbLcPurchase.IsActive = true;
            savedCommBbLcPurchase = _bbLcPurchaseRepository.Save(bbLcPurchase);
            return savedCommBbLcPurchase;
        }

        public int EditBbLcPurchase(CommBbLcPurchaseCommon bbLcPurchase)
        {
            int editedCommBbLcPurchase = 0;
            bbLcPurchase.EditedDate = DateTime.Now;
            bbLcPurchase.EditedBy = PortalContext.CurrentUser.UserId;
            editedCommBbLcPurchase = _bbLcPurchaseRepository.Edit(bbLcPurchase);
            return editedCommBbLcPurchase;
        }

        public int DeleteBbLcPurchase(CommBbLcPurchaseCommon bbLcPurchase)
        {
            int deletedCommBbLcPurchase = 0;
            bbLcPurchase.EditedDate = DateTime.Now;
            bbLcPurchase.EditedBy = PortalContext.CurrentUser.UserId;
            bbLcPurchase.IsActive = false;
            deletedCommBbLcPurchase = _bbLcPurchaseRepository.Edit(bbLcPurchase);
            return deletedCommBbLcPurchase;
        }

        public List<CommBbLcPurchaseCommon> GetBbLcPurchaseBySearchKey(int searchByCountry, string searchBybbLcPurchase)
        {
            List<CommBbLcPurchaseCommon> bbLcPurchases = null;
            bbLcPurchases = _bbLcPurchaseRepository.GetBbLcPurchaseBySearchKey(searchByCountry, searchBybbLcPurchase);
            return bbLcPurchases;
        }

        public List<VwCommBbLcPurchase> GetBbLcPurchaseEditByBbLcId(CommBbLcPurchaseCommon bbLcPurchase)
        {
            return _bbLcPurchaseRepository.GetBbLcPurchaseEditByBbLcId(bbLcPurchase);
        }
    }
}
