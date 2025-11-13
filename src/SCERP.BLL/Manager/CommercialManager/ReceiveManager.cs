using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.DAL.Repository.CommercialRepository;
using SCERP.Model.CommercialModel;


namespace SCERP.BLL.Manager.CommercialManager
{
    public class ReceiveManager : BaseManager, IReceiveManager
    {
        private readonly IReceiveRepository _iReceiveRepository = null;

        public ReceiveManager(SCERPDBContext context)
        {
            _iReceiveRepository = new ReceiveRepository(context);
        }

        public List<CommReceive> GetAllReceivesByPaging(int startPage, int pageSize, out int totalRecords, CommReceive receive)
        {
            List<CommReceive> commReceives = null;
            commReceives = _iReceiveRepository.GetAllReceivesByPaging(startPage, pageSize, out totalRecords, receive);
            return commReceives;
        }

        public List<CommReceive> GetAllReceives()
        {
            List<CommReceive> commReceive = null;
            commReceive = _iReceiveRepository.Filter(x => x.IsActive).OrderBy(x => x.ReceiveId).ToList();
            return commReceive;
        }

        public CommReceive GetReceiveById(int? id)
        {
            CommReceive commReceive = null;
            commReceive = _iReceiveRepository.GetReceiveById(id);
            return commReceive;
        }

        public bool CheckExistingReceive(CommReceive receive)
        {
            bool isExist = false;
            isExist = _iReceiveRepository.Exists(p => p.IsActive == true && p.ReceiveId != receive.ReceiveId);
            return isExist;
        }

        public int SaveReceive(CommReceive receive)
        {
            int savedCommReceive = 0;
            receive.CreatedDate = DateTime.Now;
            receive.CreatedBy = PortalContext.CurrentUser.UserId;
            receive.IsActive = true;
            savedCommReceive = _iReceiveRepository.Save(receive);
            return savedCommReceive;
        }

        public int EditReceive(CommReceive receive)
        {
            int editedCommReceive = 0;
            receive.EditedDate = DateTime.Now;
            receive.EditedBy = PortalContext.CurrentUser.UserId;
            editedCommReceive = _iReceiveRepository.Edit(receive);
            return editedCommReceive;
        }

        public int DeleteReceive(CommReceive receive)
        {
            int deletedCommReceive = 0;
            receive.EditedDate = DateTime.Now;
            receive.EditedBy = PortalContext.CurrentUser.UserId;
            receive.IsActive = false;
            deletedCommReceive = _iReceiveRepository.Edit(receive);
            return deletedCommReceive;
        }

        public List<CommReceive> GetReceiveBySearchKey(int searchByCountry, string searchByReceive)
        {
            List<CommReceive> receives = null;
            receives = _iReceiveRepository.GetReceiveBySearchKey(searchByCountry, searchByReceive);
            return receives;
        }
    }
}