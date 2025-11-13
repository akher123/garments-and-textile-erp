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
    public class LcStyleManager : BaseManager, ILcStyleManager
    {
        private readonly ILcStyleRepository _iLcStyleRepository = null;

        public LcStyleManager(SCERPDBContext context)
        {
            _iLcStyleRepository = new LcStyleRepository(context);
        }

        public List<VwCommLcStyle> GetAllLcStylesByPaging(int startPage, int pageSize, out int totalRecords, COMMLcStyle lcStyle)
        {
            List<VwCommLcStyle> commLcStyles = null;
            commLcStyles = _iLcStyleRepository.GetAllLcStylesByPaging(startPage, pageSize, out totalRecords, lcStyle);
            return commLcStyles;
        }

        public List<COMMLcStyle> GetAllLcStyles()
        {
            List<COMMLcStyle> commLcStyle = null;
            commLcStyle = _iLcStyleRepository.Filter(x => x.IsActive).OrderBy(x => x.LcStyleId).ToList();
            return commLcStyle;
        }

        public COMMLcStyle GetLcStyleById(int? id)
        {
            COMMLcStyle commLcStyle = null;
            commLcStyle = _iLcStyleRepository.GetLcStyleById(id);
            return commLcStyle;
        }

        public int? GetLcIdByLcNo(string lcNo)
        {
            return _iLcStyleRepository.GetLcIdByLcNo(lcNo);
        }

        public string GetOrderNoByOrderRefNo(string orderRefNo)
        {
            return _iLcStyleRepository.GetOrderNoByOrderRefNo(orderRefNo);
        }

        public List<OM_BuyOrdStyle> GetStylesByOrderNo(string orderNo)
        {
            return _iLcStyleRepository.GetStylesByOrderNo(orderNo);
        }

        public List<COMMLcStyle> GetLcStyleByLcId(int lcId)
        {
            return _iLcStyleRepository.GetLcStyleByLcId(lcId);
        }

        public bool CheckExistingLcStyle(COMMLcStyle lcStyle)
        {
            bool isExist = false;
            isExist = _iLcStyleRepository.Exists(p => p.IsActive == true && p.LcStyleId != lcStyle.LcStyleId);
            return isExist;
        }

        public int SaveLcStyle(COMMLcStyle lcStyle)
        {
            int savedCommLcStyle = 0;
            lcStyle.CreatedDate = DateTime.Now;
            lcStyle.CreatedBy = PortalContext.CurrentUser.UserId;
            lcStyle.IsActive = true;
            savedCommLcStyle = _iLcStyleRepository.Save(lcStyle);
            return savedCommLcStyle;
        }

        public int EditLcStyle(COMMLcStyle lcStyle)
        {
            int editedCommLcStyle = 0;
            lcStyle.EditedDate = DateTime.Now;
            lcStyle.EditedBy = PortalContext.CurrentUser.UserId;
            editedCommLcStyle = _iLcStyleRepository.Edit(lcStyle);
            return editedCommLcStyle;
        }

        public int DeleteLcStyle(COMMLcStyle lcStyle)
        {
            int deletedCommLcStyle = 0;
            lcStyle.EditedDate = DateTime.Now;
            lcStyle.EditedBy = PortalContext.CurrentUser.UserId;
            lcStyle.IsActive = false;
            deletedCommLcStyle = _iLcStyleRepository.Edit(lcStyle);
            return deletedCommLcStyle;
        }

        public List<COMMLcStyle> GetLcStyleBySearchKey(int searchByCountry, string searchByLcStyle)
        {
            List<COMMLcStyle> lcStyles = null;
            lcStyles = _iLcStyleRepository.GetLcStyleBySearchKey(searchByCountry, searchByLcStyle);
            return lcStyles;
        }

        public List<VwCommLcStyle> GetLcStyleEditByLcId(COMMLcStyle lcStyle)
        {
            return _iLcStyleRepository.GetLcStyleEditByLcId(lcStyle);
        }
    }
}