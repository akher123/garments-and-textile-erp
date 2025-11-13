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
    public class LeaveTypeManager: BaseManager,ILeaveTypeManager
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository = null;


        public LeaveTypeManager(SCERPDBContext context)
        {
            this._leaveTypeRepository = new LeaveTypeRepository(context);
        }

        public LeaveType GetLeaveTypeById(int? id)
        {
            LeaveType leaveType = null;
            try
            {
                leaveType = _leaveTypeRepository.GetLeaveTypeById(id);
            }
            catch (Exception ex)
            {
              throw new Exception(ex.Message);
            }

            return leaveType;
        }

   

        public int SaveLeaveType(LeaveType leaveType)
        {
            var savedLeaveType = 0;
            try
            {
                leaveType.CreatedDate = DateTime.Now;
                leaveType.CreatedBy = PortalContext.CurrentUser.UserId;
                leaveType.IsActive = true;
                savedLeaveType = _leaveTypeRepository.Save(leaveType);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return savedLeaveType;
        }

        public int EditLeaveType(LeaveType leaveType)
        {
          
            var editedLeaveType = 0;
            try
            {
                leaveType.EditedDate = DateTime.Now;
                leaveType.EditedBy = PortalContext.CurrentUser.UserId;
                editedLeaveType = _leaveTypeRepository.Edit(leaveType);
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }

            return editedLeaveType;
        }


        public int DeleteLeaveType(LeaveType leaveType)
        {
         

            var deleteIndex = 0;
            try
            {
                leaveType.EditedDate = DateTime.Now;
                leaveType.EditedBy = PortalContext.CurrentUser.UserId;
                leaveType.IsActive = false;
                deleteIndex = _leaveTypeRepository.Edit(leaveType);
            }
            catch (Exception ex)
            {
              throw new Exception(ex.Message);
            }

            return deleteIndex;
        }

        public List<LeaveType> GetAllLeaveTypes(int startPage, int pageSize, out int totalRecords, LeaveType leaveType)
        {
            List<LeaveType> leaveTypeList;
            try
            {
                leaveTypeList = _leaveTypeRepository.GetAllLeaveTypes(startPage, pageSize, out totalRecords, leaveType);
            }
            catch (Exception exception)
            {
              
              throw new Exception(exception.Message);
            }

            return leaveTypeList;
        }

        public bool IsLeaveTypeExist(LeaveType model)
        {
            bool isExist;
            try
            {
                isExist =
                    _leaveTypeRepository.Exists(
                        x =>
                            x.Id != model.Id &&
                            x.Title.Replace(" ", "").ToLower().Equals(model.Title.Replace(" ", "").ToLower()));
            }
            catch (Exception exception)
            {

              throw new Exception(exception.Message);
            }
            return isExist;
        }

        public List<LeaveType> GetLeaveTypeBySearchKey(LeaveType leaveType)
        {
            List<LeaveType> leaveTypeList;
            try
            {

                leaveTypeList = _leaveTypeRepository.GetLeaveTypeBySearchKey(leaveType);
            }
            catch (Exception exception)
            {
               throw  new Exception(exception.Message);
            }

            return leaveTypeList;
        }

        public List<LeaveType> GetAllLeaveTypes()
        {
            List<LeaveType> leaveTypes = null;

            try
            {
                leaveTypes = _leaveTypeRepository.Filter(x => x.IsActive).OrderBy(x => x.Title).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return leaveTypes;
        }
    }
}
