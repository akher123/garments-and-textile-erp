using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using System.Linq;

namespace SCERP.BLL.Manager.HRMManager
{
    public class MaritalStatusManager : BaseManager, IMaritalStatusManager
    {
        private readonly IMaritalStatusRepository _maritalStatusRepository = null;

        public MaritalStatusManager(SCERPDBContext context)
        {
            this._maritalStatusRepository = new MaritalStatusRepository(context);
        }


        public List<MaritalState> GetAllMaritalStatusesByPaging(int startPage, int pageSize, out int totalRecords, MaritalState maritalStatus)
        {
            var maritalStatuses = new List<MaritalState>();
            try
            {
                maritalStatuses = _maritalStatusRepository.GetAllMaritalStatusesByPaging(startPage, pageSize, out totalRecords, maritalStatus).ToList();
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.InnerException.Message);
            }

            return maritalStatuses;
        }

        public MaritalState GetMaritalStatusById(int maritalStateId)
        {
            MaritalState maritalStatus = null;
            try
            {
                maritalStatus = _maritalStatusRepository.GetMaritalStatusById(maritalStateId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }

            return maritalStatus;
        }



        public int SaveMaritalStatus(MaritalState maritalStatus)
        {
            var savedReligion = 0;
            try
            {
                maritalStatus.CreatedDate = DateTime.Now;
                maritalStatus.CreatedBy = PortalContext.CurrentUser.UserId;
                maritalStatus.IsActive = true;
                savedReligion = _maritalStatusRepository.Save(maritalStatus);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }

            return savedReligion;
        }

        public int EditMaritalStatus(MaritalState maritalStatus)
        {
            var editedReligion = 0;
            try
            {
                maritalStatus.EditedDate = DateTime.Now;
                maritalStatus.EditedBy = PortalContext.CurrentUser.UserId;
                editedReligion = _maritalStatusRepository.Edit(maritalStatus);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }

            return editedReligion;
        }


        public int DeleteMaritalStatus(MaritalState maritalStatus)
        {
            var deletedReligion = 0;
            try
            {
                maritalStatus.EditedDate = DateTime.Now;
                maritalStatus.EditedBy = PortalContext.CurrentUser.UserId;
                maritalStatus.IsActive = false;
                deletedReligion = _maritalStatusRepository.Edit(maritalStatus);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }

            return deletedReligion;
        }

        public bool CheckExistingMaritalStatus(MaritalState maritalStatus)
        {
            bool isExist = false;
            try
            {
                isExist =
                    _maritalStatusRepository.Exists(
                        x =>
                            x.IsActive == true &&
                            x.MaritalStateId != maritalStatus.MaritalStateId &&
                            x.Title.Replace(" ", "").ToLower().Equals(maritalStatus.Title.Replace(" ", "").ToLower()));
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }
            return isExist;
        }

        public List<MaritalState> GetMaritalStatusBySearchKey(string searchKey)
        {
            List<MaritalState> maritalStatuses;
            try
            {
                maritalStatuses = !String.IsNullOrEmpty(searchKey)
                    ? _maritalStatusRepository.Filter(x => x.Title.Replace(" ", "").ToLower().Contains(searchKey.Replace(" ", "").ToLower())).ToList()
                    : _maritalStatusRepository.Filter(x => x.IsActive == true).ToList();

            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }
            return maritalStatuses;
        }

        public List<MaritalState> GetAllMaritalStatuses()
        {
            List<MaritalState> maritalStatuses = null;
            try
            {
                maritalStatuses = _maritalStatusRepository.GetAllMaritalStatuses();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }

            return maritalStatuses;
        }

    }
}
