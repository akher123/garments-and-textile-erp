using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.HRMManager
{
    public class QuitTypeManager : BaseManager, IQuitTypeManager
    {
        private readonly IQuitTypeRepository _quitTypeRepository= null;

        public QuitTypeManager(SCERPDBContext context)
        {
            _quitTypeRepository = new QuitTypeRepository(context);
        }



        public List<QuitType> GetAllQuitTypes()
        {
            List<QuitType> quitTypes = null;

            try
            {
                quitTypes = _quitTypeRepository.Filter(x => x.IsActive).OrderBy(x => x.Type).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return quitTypes;
        }

        public List<QuitType> GetAllQuitTypesByPaging(int startPage, int pageSize, QuitType model, out int totalRecords)
        {
            List<QuitType> quitTypes;
            try
            {
                quitTypes = _quitTypeRepository.GetAllQuitTypesByPaging(startPage, pageSize, model, out totalRecords);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);

            }
            return quitTypes;
        }

        public QuitType GetQuitTypeById(int quitTypeId)
        {
            QuitType quitType;
            try
            {
                quitType =
                    _quitTypeRepository.FindOne(x => x.IsActive && x.QuitTypeId == quitTypeId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return quitType;
        }

        public int EditQuitType(QuitType model)
        {
            var editIndex = 0;
            try
            {
                var quitTypeObj = _quitTypeRepository.FindOne(x => x.IsActive && x.QuitTypeId == model.QuitTypeId);
                quitTypeObj.Type = model.Type;
                quitTypeObj.TypeInBengali = model.TypeInBengali;
                quitTypeObj.Description = model.Description;
                quitTypeObj.EditedDate = DateTime.Now;
                quitTypeObj.EditedBy = PortalContext.CurrentUser.UserId;
                editIndex = _quitTypeRepository.Edit(quitTypeObj);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return editIndex;
        }

        public int SaveQuitType(QuitType model)
        {
            var saveIndex = 0;
            try
            {
                model.CreatedBy = PortalContext.CurrentUser.UserId;
                model.CreatedDate = DateTime.Now;
                model.IsActive = true;
                saveIndex = _quitTypeRepository.Save(model);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return saveIndex;
        }

        public bool IsExistQuitType(QuitType model)
        {
            bool isExist;
            try
            {
                isExist = _quitTypeRepository.Exists(x => x.IsActive&&x.QuitTypeId!=model.QuitTypeId && (x.Type.Replace("", " ").ToLower() == model.Type.Replace("", " ").ToLower()));
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return isExist;
        }

        public int DeleteQuitType(int quitTypeId)
        {
            var deleteIndex = 0;
            try
            {
                var quitTypeObj = _quitTypeRepository.FindOne(x => x.IsActive && x.QuitTypeId == quitTypeId);
                quitTypeObj.EditedDate = DateTime.Now;
                quitTypeObj.EditedBy = PortalContext.CurrentUser.UserId;
                quitTypeObj.IsActive = false;
                deleteIndex = _quitTypeRepository.Edit(quitTypeObj);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return deleteIndex;
        }

        public List<QuitType> GetAllQuitTypesBySearchKey(QuitType model)
        {
            List<QuitType> quitTypes;
            try
            {
                quitTypes = _quitTypeRepository.GetAllQuitTypesBySearchKey(model);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);

            }
            return quitTypes;
        }
    }
}
