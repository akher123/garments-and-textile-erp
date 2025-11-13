using System.Runtime.Remoting.Contexts;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeShortLeaveManager : BaseManager, IEmployeeShortLeaveManager
    {
        private readonly IEmployeeShortLeaveRepository _shortLeaveRepository = null;

        public EmployeeShortLeaveManager(SCERPDBContext context)
        {
            this._shortLeaveRepository = new EmployeeShortLeaveRepository(context);
        }

        public List<VEmployeeShortLeave> GetAllEmployeeShortLeavesByPaging(int startPage, int pageSize, out int totalRecords, ShortLeaveModel employeeShortLeave)
        {
            return _shortLeaveRepository.GetAllEmployeeShortLeavesByPaging(startPage, pageSize, out totalRecords, employeeShortLeave);
        }

        public EmployeeShortLeave GetEmployeeShortLeaveById(int? id)
        {
            return _shortLeaveRepository.GetEmployeeShortLeaveById(id);
        }

        public int SaveShortLeave(EmployeeShortLeave shortLeave)
        {
            var savedShortLeave = 0;
            try
            {
                var duplicateCheck = _shortLeaveRepository.CheckDuplicateDateTime(shortLeave);

                if (duplicateCheck == 2)
                    return duplicateCheck;

                shortLeave.CreatedDate = DateTime.Now;
                shortLeave.CreatedBy = PortalContext.CurrentUser.UserId;
                shortLeave.IsActive = true;
                savedShortLeave = _shortLeaveRepository.Save(shortLeave);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                savedShortLeave = 0;
            }

            return savedShortLeave;
        }

        public int EditShortLeave(EmployeeShortLeave shortLeave)
        {
            var editedShortLeave = 0;
            try
            {
                shortLeave.EditedDate = DateTime.Now;
                shortLeave.EditedBy = PortalContext.CurrentUser.UserId;
                editedShortLeave = _shortLeaveRepository.Edit(shortLeave);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                editedShortLeave = 0;
            }

            return editedShortLeave;
        }

        public int DeleteShortLeave(EmployeeShortLeave shortLeave)
        {
            var editedShortLeave = 0;
            try
            {
                shortLeave.EditedDate = DateTime.Now;
                shortLeave.EditedBy = PortalContext.CurrentUser.UserId;
                shortLeave.IsActive = false;
                editedShortLeave = _shortLeaveRepository.Edit(shortLeave);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                editedShortLeave = 0;
            }

            return editedShortLeave;
        }
    }
}
