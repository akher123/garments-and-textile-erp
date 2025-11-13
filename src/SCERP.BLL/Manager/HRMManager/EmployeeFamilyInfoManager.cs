using System;
using System.Collections.Generic;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.DAL;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeFamilyInfoManager : BaseManager, IEmployeeFamilyInfoManager
    {
        private readonly IEmployeeFamilyInfoRepository _employeeFamilyInfoRepository = null;


        public EmployeeFamilyInfoManager(SCERPDBContext context)
        {
            _employeeFamilyInfoRepository = new EmployeeFamilyInfoRepository(context);
        }

        public List<EmployeeFamilyInfo> GetEmployeeFamilyInfoByEmployeeGuidId(Guid employeeId)
        {
            List<EmployeeFamilyInfo> employeeFamilyInfos;
            try
            {
                employeeFamilyInfos = _employeeFamilyInfoRepository.GetEmployeeFamilyInfoByEmployeeGuidId(employeeId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeeFamilyInfos;
        }

        public int SaveEmployeeFamilyInfo(EmployeeFamilyInfo employeeFamilyInfo)
        {
            var saved = 0;
            try
            {
                employeeFamilyInfo.CreatedBy = PortalContext.CurrentUser.UserId;
                employeeFamilyInfo.CreatedDate = DateTime.Now;
                employeeFamilyInfo.IsActive = true;
                saved = _employeeFamilyInfoRepository.Save(employeeFamilyInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return saved;
        }

        public int EditEmployeeFamilyInfo(EmployeeFamilyInfo employeeFamilyInfo)
        {
            var edit = 0;
            try
            {
                employeeFamilyInfo.EditedBy = PortalContext.CurrentUser.UserId;
                employeeFamilyInfo.EditedDate = DateTime.Now;
                edit = _employeeFamilyInfoRepository.Edit(employeeFamilyInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return edit;
        }

        public int DeleteEmployeeFamilyInfo(EmployeeFamilyInfo employeeFamilyInfo)
        {
            var deleted = 0;
            try
            {
                employeeFamilyInfo.EditedBy = PortalContext.CurrentUser.UserId;
                employeeFamilyInfo.EditedDate = DateTime.Now;
                employeeFamilyInfo.IsActive = false;
                deleted = _employeeFamilyInfoRepository.Edit(employeeFamilyInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return deleted;
        }

        public EmployeeFamilyInfo GetEmployeeFamilyInfoById(Guid employeeId, int id)
        {
            EmployeeFamilyInfo employeeFamilyInfo;

            try
            {
                employeeFamilyInfo = _employeeFamilyInfoRepository.GetEmployeeFamilyInfoById(employeeId, id);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeeFamilyInfo;
        }

        public bool CheckExistingEmployeeFamilyInfo(EmployeeFamilyInfo employeeFamilyInfo)
        {
            var isExist = false;
            try
            {
                isExist =
                    _employeeFamilyInfoRepository.Exists(
                        x =>
                            (x.IsActive == true) &&
                            (x.EmployeeFamilyInfoId != employeeFamilyInfo.EmployeeFamilyInfoId) &&
                            (x.NameOfChild.Replace(" ","").ToLower() == employeeFamilyInfo.NameOfChild.Replace(" ","").ToLower()) &&
                            (x.EmployeeId == employeeFamilyInfo.EmployeeId));
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return isExist;
        }
    }
}
