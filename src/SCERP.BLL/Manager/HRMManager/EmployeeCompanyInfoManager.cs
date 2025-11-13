using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.DAL;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeCompanyInfoManager : BaseManager, IEmployeeCompanyInfoManager
    {
        private readonly IEmployeeCompanyInfoRepository _employeeCompanyInfoRepository = null;


        public EmployeeCompanyInfoManager(SCERPDBContext context)
        {
            _employeeCompanyInfoRepository = new EmployeeCompanyInfoRepository(context);
        }

        public EmployeeCompanyInfo GetEmployeeLatestCompanyInfoByEmployeeGuidId(EmployeeCompanyInfo employeeCompanyInfo)
        {
            EmployeeCompanyInfo empCompanyInfo;

            try
            {
                empCompanyInfo = _employeeCompanyInfoRepository.GetEmployeeLatestCompanyInfoByEmployeeGuidId(employeeCompanyInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return empCompanyInfo;
        }

        public EmployeeCompanyInfo GetEmployeeLatestJobInfoByEmployeeId(EmployeeCompanyInfo employeeCompanyInfo)
        {
            EmployeeCompanyInfo employeeLatestJobInfo;

            try
            {
                employeeLatestJobInfo = _employeeCompanyInfoRepository.GetEmployeeLatestJobInfoByEmployeeId(employeeCompanyInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return employeeLatestJobInfo;
        }

        public List<VEmployeeCompanyInfoDetail> AutocompliteGetEmployeeInfo(string employeeCardId, int? branchUnitDepartmentId)
        {
            var employeeCompanyInfo = _employeeCompanyInfoRepository.AutocompliteGetEmployeeInfo(employeeCardId, branchUnitDepartmentId);
            return employeeCompanyInfo;
        }

        public List<EmployeeCompanyInfo> GetEmployeeCompanyInfosByEmployeeGuidId(EmployeeCompanyInfo employeeCompanyInfo)
        {
            List<EmployeeCompanyInfo> empCompanyInfos = null;

            try
            {
                empCompanyInfos = _employeeCompanyInfoRepository.GetEmployeeCompanyInfosByEmployeeGuidId(employeeCompanyInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return empCompanyInfos;
        }

        public int SaveEmployeeCompanyInfo(EmployeeCompanyInfo employeeCompanyInfo)
        {
            var saved = 0;
            try
            {
                employeeCompanyInfo.CreatedBy = PortalContext.CurrentUser.UserId;
                employeeCompanyInfo.CreatedDate = DateTime.Now;
                employeeCompanyInfo.IsActive = true;
                saved = _employeeCompanyInfoRepository.Save(employeeCompanyInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return saved;
        }

        public int EditEmployeeCompanyInfo(EmployeeCompanyInfo employeeCompanyInfo)
        {
            var edit = 0;
            try
            {
                employeeCompanyInfo.EditedBy = PortalContext.CurrentUser.UserId;
                employeeCompanyInfo.EditedDate = DateTime.Now;
                edit = _employeeCompanyInfoRepository.Edit(employeeCompanyInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return edit;
        }

        public int DeleteEmployeeCompanyInfo(EmployeeCompanyInfo employeeCompanyInfo)
        {
            var deleted = 0;
            try
            {
                employeeCompanyInfo.EditedBy = PortalContext.CurrentUser.UserId;
                employeeCompanyInfo.EditedDate = DateTime.Now;
                employeeCompanyInfo.IsActive = false;
                deleted = _employeeCompanyInfoRepository.Edit(employeeCompanyInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return deleted;
        }

        public int UpdateEmployeeCompanyInfoDate(EmployeeCompanyInfo employeeCompanyInfo)
        {
            var updated = 0;
            try
            {
                employeeCompanyInfo.EditedBy = PortalContext.CurrentUser.UserId;
                employeeCompanyInfo.EditedDate = DateTime.Now;
                updated = _employeeCompanyInfoRepository.UpdateEmployeeCompanyInfoDate(employeeCompanyInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return updated;
        }

        public EmployeeCompanyInfo GetEmployeeCompanyInfoById(Guid? employeeId, int id)
        {
            EmployeeCompanyInfo employeeCompanyInfo;

            try
            {
                employeeCompanyInfo = _employeeCompanyInfoRepository.GetEmployeeCompanyInfoById(employeeId, id);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeeCompanyInfo;
        }

        public bool CheckExistingEmployeeCompanyInfo(EmployeeCompanyInfoCustomModel employeeCompanyInfo)
        {
            var isExist = false;
            try
            {
                isExist =
                    _employeeCompanyInfoRepository.Exists(
                        x =>
                            x.IsActive == true &&
                            x.EmployeeCompanyInfoId != employeeCompanyInfo.EmployeeCompanyInfoId &&
                            x.EmployeeId == employeeCompanyInfo.EmployeeCompanyInfo.EmployeeId &&
                            x.BranchUnitDepartmentId == employeeCompanyInfo.BranchUnitDepartmentId &&
                            x.DesignationId == employeeCompanyInfo.EmployeeDesignationId &&
                            x.JobTypeId == employeeCompanyInfo.JobTypeId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return isExist;
        }

        public List<EmployeeCompanyInfoModel> GetEmployeesLatestCompanyInfo(int startPage, int pageSize, EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel)
        {
            List<EmployeeCompanyInfoModel> empCompanyInfos = null;

            try
            {
                empCompanyInfos = _employeeCompanyInfoRepository.GetEmployeesLatestCompanyInfo(startPage, pageSize, model, searchFieldModel);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return empCompanyInfos;
        }

        public List<EmployeeCompanyInfoModel> GetEmployeeIndividualHoliday(int startPage, int pageSize, EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel)
        {
            List<EmployeeCompanyInfoModel> empCompanyInfos = null;
            empCompanyInfos = _employeeCompanyInfoRepository.GetEmployeeIndividualHoliday(startPage, pageSize, model, searchFieldModel);
            return empCompanyInfos;
        }

        public List<EmployeeCompanyInfoModel> GetEmployeesForAssigingLine(EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel)
        {
            List<EmployeeCompanyInfoModel> empCompanyInfoModels = null;

            try
            {
                empCompanyInfoModels = _employeeCompanyInfoRepository.GetEmployeesForAssigingLine(model, searchFieldModel);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return empCompanyInfoModels;
        }

        public List<EmployeeCompanyInfoModel> GetEmployeesForIndividualHoliday(EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel)
        {
            List<EmployeeCompanyInfoModel> empCompanyInfoModels = null;

            empCompanyInfoModels = _employeeCompanyInfoRepository.GetEmployeesForIndividualHoliday(model, searchFieldModel);

            return empCompanyInfoModels;
        }

        public int AssignBulkEmployeeLine(SearchFieldModel searchFieldModel, EmployeeCompanyInfoModel model)
        {
            var assignEmployeeLine = 0;
            try
            {
                assignEmployeeLine = _employeeCompanyInfoRepository.AssignBulkEmployeeLine(searchFieldModel, model);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return assignEmployeeLine;
        }

        public Company GetCompanyRefIdByEmployeeId(Guid? employeeId)
        {
            var employeeCompanyInfo
                = _employeeCompanyInfoRepository.Filter(x => x.IsActive == true)
                    .Include(x => x.BranchUnitDepartment.BranchUnit.Branch.Company)
                    .FirstOrDefault(x => x.EmployeeId == employeeId);
            if (employeeCompanyInfo != null)
                //return employeeCompanyInfo
                //    .BranchUnitDepartment.BranchUnit.Branch.Company.CompanyRefId;
                return employeeCompanyInfo.BranchUnitDepartment.BranchUnit.Branch.Company;
            else
                throw new NullReferenceException("Not valid User");
        }

        public VEmployeeCompanyInfoDetail GetEmployeeByEmployeeCardId(string employeeCardId)
        {
            return _employeeCompanyInfoRepository.GetEmployeeCompanyInfoByEmployeeCardId(employeeCardId);
        }

        public List<EmployeeCompanyInfoModel> GetEmployeesForAssigingJobType(EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel)
        {
            List<EmployeeCompanyInfoModel> empCompanyInfoModels = null;

            try
            {
                empCompanyInfoModels = _employeeCompanyInfoRepository.GetEmployeesForAssigingJobType(model, searchFieldModel);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return empCompanyInfoModels;
        }

        public int AssignBulkEmployeeJobType(SearchFieldModel searchFieldModel, EmployeeCompanyInfoModel model)
        {
            var assignEmployeeLine = 0;
            try
            {
                assignEmployeeLine = _employeeCompanyInfoRepository.AssignBulkEmployeeJobType(searchFieldModel, model);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return assignEmployeeLine;
        }

        public VEmployeeCompanyInfoDetail GetEmployeeByEmployeeId(Guid employeeId)
        {
            return _employeeCompanyInfoRepository.GetEmployeeByEmployeeId(employeeId);
        }

        public int AssignIndividualHoliday(SearchFieldModel searchFieldModel, EmployeeCompanyInfoModel model)
        {
            return _employeeCompanyInfoRepository.AssignIndividualHoliday(searchFieldModel, model);
        }

        public int AssignIndividualWeekend(SearchFieldModel searchFieldModel, EmployeeCompanyInfoModel model)
        {
            return _employeeCompanyInfoRepository.AssignIndividualWeekend(searchFieldModel, model);
        }
    }
}
