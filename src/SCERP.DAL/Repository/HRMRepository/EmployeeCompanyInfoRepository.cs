using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Model.HRMModel;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeCompanyInfoRepository : Repository<EmployeeCompanyInfo>, IEmployeeCompanyInfoRepository
    {
        public EmployeeCompanyInfoRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public EmployeeCompanyInfo GetEmployeeLatestCompanyInfoByEmployeeGuidId(EmployeeCompanyInfo employeeCompanyInfo)
        {
            var employeeLatestCompanyInfo = new EmployeeCompanyInfo();

            try
            {

                var fromDateParam = (employeeCompanyInfo.FromDate.HasValue) ?
                    new ObjectParameter("FromDate", employeeCompanyInfo.FromDate) :
                    new ObjectParameter("FromDate", typeof (DateTime));

                var employeeIdParam = (employeeCompanyInfo.EmployeeId != Guid.Parse("00000000-0000-0000-0000-000000000000")) ?
                    new ObjectParameter("EmployeeID", employeeCompanyInfo.EmployeeId) :
                    new ObjectParameter("EmployeeID", typeof (Guid));

                var employeeCompanyInfoIdParam = (employeeCompanyInfo.EmployeeCompanyInfoId != 0) ?
                    new ObjectParameter("EmployeeCompanyInfoId", employeeCompanyInfo.EmployeeCompanyInfoId) :
                    new ObjectParameter("EmployeeCompanyInfoId", typeof (int));

                var employeeCompanyInfoResult = Context.SPGetSpecificEmployeeCompanyInfo(fromDateParam, employeeIdParam, employeeCompanyInfoIdParam).FirstOrDefault();

                if (employeeCompanyInfoResult != null)
                {
                    employeeLatestCompanyInfo.EmployeeId = employeeCompanyInfoResult.EmployeeId;
                    employeeLatestCompanyInfo.EmployeeCompanyInfoId = employeeCompanyInfoResult.EmployeeCompanyInfoId;
                    employeeLatestCompanyInfo.BranchUnitId = employeeCompanyInfoResult.BranchUnitId;
                    employeeLatestCompanyInfo.BranchUnitDepartmentId = employeeCompanyInfoResult.BranchUnitDepartmentId;
                    employeeLatestCompanyInfo.DesignationId = employeeCompanyInfoResult.DesignationId;
                    employeeLatestCompanyInfo.DepartmentSectionId = employeeCompanyInfoResult.DepartmentSectionId;
                    employeeLatestCompanyInfo.DepartmentLineId = employeeCompanyInfoResult.DepartmentLineId;
                    employeeLatestCompanyInfo.JobTypeId = employeeCompanyInfoResult.JobTypeId;
                    employeeLatestCompanyInfo.PunchCardNo = employeeCompanyInfoResult.PunchCardNo;
                    employeeLatestCompanyInfo.IsEligibleForOvertime = employeeCompanyInfoResult.IsEligibleForOvertime;
                    employeeLatestCompanyInfo.FromDate = employeeCompanyInfoResult.FromDate;
                    employeeLatestCompanyInfo.ToDate = employeeCompanyInfoResult.ToDate;
                    employeeLatestCompanyInfo.IsActive = employeeCompanyInfoResult.IsActive;
                    employeeLatestCompanyInfo.EmployeeTypeId = employeeCompanyInfoResult.EmployeeTypeId;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return employeeLatestCompanyInfo;

        }

        public EmployeeCompanyInfo GetEmployeeLatestJobInfoByEmployeeId(EmployeeCompanyInfo employeeCompanyInfo)
        {
            EmployeeCompanyInfo employeeLatestJobInfo;

            try
            {
                employeeLatestJobInfo = Context.EmployeeCompanyInfos
                    .Where(x => x.IsActive && x.EmployeeId == employeeCompanyInfo.EmployeeId && x.FromDate <= employeeCompanyInfo.FromDate)
                    .Include(x => x.EmployeeDesignation.EmployeeGrade.EmployeeType)
                    .OrderByDescending(x => x.FromDate)
                    .FirstOrDefault();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return employeeLatestJobInfo;
        }

        public List<VEmployeeCompanyInfoDetail> AutocompliteGetEmployeeInfo(string employeeCardId, int? branchUnitDepartmentId)
        {
            return Context.VEmployeeCompanyInfoDetails.Where(
                x => (x.BranchUnitDepartmentId == branchUnitDepartmentId || branchUnitDepartmentId == null) && ((x.EmployeeCardId.Trim().Contains(employeeCardId) || String.IsNullOrEmpty(employeeCardId)) || (x.EmployeeName.Trim().Contains(employeeCardId) || String.IsNullOrEmpty(employeeCardId)))).ToList();
        }

        public List<EmployeeCompanyInfo> GetEmployeeCompanyInfosByEmployeeGuidId(EmployeeCompanyInfo employeeCompanyInfo)
        {
            var employeeCompanyInfos = new List<EmployeeCompanyInfo>();

            try
            {

                var fromDateParam = (employeeCompanyInfo.FromDate.HasValue) ?
                    new ObjectParameter("FromDate", employeeCompanyInfo.FromDate) :
                    new ObjectParameter("FromDate", typeof (DateTime));

                var employeeIdParam = (employeeCompanyInfo.EmployeeId != Guid.Parse("00000000-0000-0000-0000-000000000000")) ?
                    new ObjectParameter("EmployeeID", employeeCompanyInfo.EmployeeId) :
                    new ObjectParameter("EmployeeID", typeof (Guid));

                var employeeCompanyInfoIdParam = (employeeCompanyInfo.EmployeeCompanyInfoId != 0) ?
                    new ObjectParameter("EmployeeCompanyInfoId", employeeCompanyInfo.EmployeeCompanyInfoId) :
                    new ObjectParameter("EmployeeCompanyInfoId", typeof (int));

                var specificEmployeeCompanyInfos = Context.SPGetSpecificEmployeeCompanyInfo(fromDateParam, employeeIdParam, employeeCompanyInfoIdParam);

                foreach (var employeeCompanyInfoResult in specificEmployeeCompanyInfos)
                {
                    var empCompanyInfo = new EmployeeCompanyInfoCustomModel
                    {
                        EmployeeCompanyInfoId = employeeCompanyInfoResult.EmployeeCompanyInfoId,
                        Company = {Name = employeeCompanyInfoResult.CompanyName},
                        Branch = {Name = employeeCompanyInfoResult.BranchName},
                        Unit = {Name = employeeCompanyInfoResult.UnitName},
                        Department = {Name = employeeCompanyInfoResult.DepartmentName},

                        DepartmentSection =
                        {
                            Section = new Section
                            {
                                Name = employeeCompanyInfoResult.SectionName
                            }
                        },

                        DepartmentLine =
                        {
                            Line = new Line
                            {
                                Name = employeeCompanyInfoResult.LineName
                            }
                        },

                        EmployeeType = {Title = employeeCompanyInfoResult.EmployeeType},
                        EmployeeGrade = {Name = employeeCompanyInfoResult.EmployeeGrade},
                        EmployeeDesignation = {Title = employeeCompanyInfoResult.EmployeeDesignation},
                        SkillSet = {Title = employeeCompanyInfoResult.JobType},
                        IsEligibleForOvertime = employeeCompanyInfoResult.IsEligibleForOvertime,
                        PunchCardNo = employeeCompanyInfoResult.PunchCardNo,
                        FromDate = employeeCompanyInfoResult.FromDate,
                        ToDate = employeeCompanyInfoResult.ToDate
                    };


                    employeeCompanyInfos.Add(empCompanyInfo);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return employeeCompanyInfos;
        }

        public IList<EmployeeCompanyInfo> GetEmployeCompanyInfos(SearchFieldModel searchFieldModel)
        {
            IQueryable<EmployeeCompanyInfo> employeeCompanyInfos;
            try
            {
                employeeCompanyInfos =
                    Context.EmployeeCompanyInfos.Include(x => x.Employee)
                        .Where(x => x.IsActive)
                        .Include(x => x.EmployeeDesignation.EmployeeGrade.EmployeeType)
                        .Include(x => x.BranchUnitDepartment.BranchUnit.Branch)
                        .Include(x => x.BranchUnitDepartment.BranchUnit.Unit)
                        .Include(x => x.BranchUnitDepartment.UnitDepartment.Department)
                        .Where(
                            x => x.IsActive && (x.Employee.EmployeeCardId == searchFieldModel.SearchByEmployeeCardId || searchFieldModel.SearchByEmployeeCardId == null)
                                 && (x.BranchUnitDepartment.BranchUnit.Branch.CompanyId == searchFieldModel.SearchByCompanyId)
                                 && (x.BranchUnitDepartment.BranchUnit.BranchId == searchFieldModel.SearchByBranchId)
                                 && (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId)
                                 && (x.BranchUnitDepartment.BranchUnitId == searchFieldModel.SearchByBranchUnitId)).OrderBy(x => x.Employee.EmployeeCardId);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }
            return employeeCompanyInfos.ToList();
        }

        public VEmployeeCompanyInfoDetail GetEmployeeCompanyInfoByEmployeeCardId(string employeeCardId)
        {
            var employee = new VEmployeeCompanyInfoDetail();

            try
            {

                IEnumerable<int> companyIdList = PortalContext.CurrentUser.PermissionContext.CompanyList.Select(x => x.CompanyId);
                IEnumerable<int> branchIdList = PortalContext.CurrentUser.PermissionContext.BranchList.Select(x => x.BranchId);
                IEnumerable<int> branchUnitIdList = PortalContext.CurrentUser.PermissionContext.UnitList.Select(x => x.BranchUnitId);
                IEnumerable<int> branchUnitDepartmentIdList = PortalContext.CurrentUser.PermissionContext.DepartmentList.Select(x => x.BranchUnitDepartmentId);
                IEnumerable<int> employeeTypeList = PortalContext.CurrentUser.PermissionContext.EmployeeTypeList.Select(x => x.Id);


                employee =
                    Context.VEmployeeCompanyInfoDetails.SingleOrDefault(x => companyIdList.Contains(x.CompanyId) &&
                                                                             branchIdList.Contains(x.BranchId) &&
                                                                             branchUnitIdList.Contains(x.BranchUnitId) &&
                                                                             branchUnitDepartmentIdList.Contains(x.BranchUnitDepartmentId) &&
                                                                             employeeTypeList.Contains(x.EmployeeTypeId) &&
                                                                             (x.EmployeeCardId == employeeCardId));

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }

            return employee;
        }

        public EmployeeCompanyInfo GetEmployeeCompanyInfoById(Guid? employeeId, int id)
        {
            IQueryable<EmployeeCompanyInfo> employeeCompanyInfos;
            try
            {
                employeeCompanyInfos =
                    Context.EmployeeCompanyInfos
                        .Where(x => x.IsActive)
                        .Include(x => x.EmployeeDesignation.EmployeeGrade.EmployeeType)
                        .Include(x => x.BranchUnitDepartment.BranchUnit.Branch.Company)
                        .Include(x => x.BranchUnitDepartment.BranchUnit.Unit)
                        .Include(x => x.BranchUnitDepartment.UnitDepartment.Department)
                        .Include(x => x.DepartmentSection)
                        .Include(x => x.DepartmentLine)
                        .Include(x => x.EmployeeDesignation.EmployeeGrade)
                        .Include(x => x.EmployeeDesignation.EmployeeGrade.EmployeeType)
                        .Include(x => x.SkillSet)
                        .Where(x => (x.EmployeeId == employeeId || employeeId == null) && (x.EmployeeCompanyInfoId == id) && (x.IsActive == true));
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
            return employeeCompanyInfos.ToList().FirstOrDefault();
        }

        public int UpdateEmployeeCompanyInfoDate(EmployeeCompanyInfo employeeCompanyInfo)
        {
            var update = 0;
            try
            {
                employeeCompanyInfo.EditedDate = DateTime.Now;
                employeeCompanyInfo.EditedBy = PortalContext.CurrentUser.UserId;
                employeeCompanyInfo.IsActive = true;
                update = base.Edit(employeeCompanyInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return update;
        }

        public List<EmployeeCompanyInfoModel> GetEmployeesLatestCompanyInfo(int startPage, int pageSize, EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel)
        {
            try
            {

                var startPageParam = new SqlParameter {ParameterName = "StartRowIndex", Value = startPage};

                var pageSizeParam = new SqlParameter {ParameterName = "MaxRows", Value = pageSize};


                int companyId = -1;
                if (searchFieldModel.SearchByCompanyId > 0)
                    companyId = searchFieldModel.SearchByCompanyId;
                var companyIdParam = new SqlParameter {ParameterName = "CompanyId", Value = companyId};

                int branchId = -1;
                if (searchFieldModel.SearchByBranchId > 0)
                    branchId = searchFieldModel.SearchByBranchId;
                var branchIdParam = new SqlParameter {ParameterName = "BranchId", Value = branchId};

                int branchUnitId = -1;
                if (searchFieldModel.SearchByBranchUnitId > 0)
                    branchUnitId = searchFieldModel.SearchByBranchUnitId;
                var branchUnitIdParam = new SqlParameter {ParameterName = "BranchUnitId", Value = branchUnitId};


                int branchUnitDepartmentId = -1;
                if (searchFieldModel.SearchByBranchUnitDepartmentId > 0)
                    branchUnitDepartmentId = searchFieldModel.SearchByBranchUnitDepartmentId;
                var branchUnitDepartmentIdParam = new SqlParameter {ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId};

                int departmentSectionId = -1;
                if (searchFieldModel.SearchByDepartmentSectionId > 0)
                    departmentSectionId = searchFieldModel.SearchByDepartmentSectionId;
                var departmentSectionIdParam = new SqlParameter {ParameterName = "DepartmentSectionId", Value = departmentSectionId};

                int departmentLineId = -1;
                if (searchFieldModel.SearchByDepartmentLineId > 0)
                    departmentLineId = searchFieldModel.SearchByDepartmentLineId;
                var departmentLineIdParam = new SqlParameter {ParameterName = "DepartmentLineId", Value = departmentLineId};


                int employeeTypeId = -1;
                if (searchFieldModel.SearchByEmployeeTypeId > 0)
                    employeeTypeId = searchFieldModel.SearchByEmployeeTypeId;
                var employeeTypeIdParam = new SqlParameter {ParameterName = "EmployeeTypeId", Value = employeeTypeId};

                int employeeJobTypeId = -1;
                if (searchFieldModel.ExistingJobTypeId > 0)
                    employeeJobTypeId = searchFieldModel.ExistingJobTypeId;
                var employeeJobTypeIdParam = new SqlParameter {ParameterName = "EmployeeJobTypeId", Value = employeeJobTypeId};

                string employeeCardId = string.Empty;
                if (!String.IsNullOrEmpty(searchFieldModel.SearchByEmployeeCardId))
                    employeeCardId = searchFieldModel.SearchByEmployeeCardId;
                var employeeCardIdParam = new SqlParameter {ParameterName = "EmployeeCardId", Value = employeeCardId};

                DateTime? joiningDateBegin = searchFieldModel.JoiningDateBegin;
                if (joiningDateBegin == null)
                    joiningDateBegin = new DateTime(1900, 01, 01);
                var joiningDateBeginParam = new SqlParameter {ParameterName = "JoiningDateBegin", Value = joiningDateBegin};

                DateTime? joiningDateeEnd = searchFieldModel.JoiningDateEnd;
                if (joiningDateeEnd == null)
                    joiningDateeEnd = new DateTime(1900, 01, 01);
                var joiningDateEndParam = new SqlParameter {ParameterName = "JoiningDateEnd", Value = joiningDateeEnd};

                DateTime? upToDate = new DateTime(1900, 01, 01);
                if (searchFieldModel.UpToDate != null)
                    upToDate = searchFieldModel.UpToDate;
                var upToDateParam = new SqlParameter {ParameterName = "UpToDate", Value = upToDate};


                var userNameParam = new SqlParameter {ParameterName = "UserName", Value = PortalContext.CurrentUser.Name};



                var employeeCompanyInfoes = Context.Database.SqlQuery<EmployeeCompanyInfoModel>("HRMSPGetEmployeesLatestCompanyInfo @StartRowIndex, @MaxRows, @CompanyId, " +
                                                                                                "@BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                                                "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeJobTypeId, " +
                                                                                                "@EmployeeCardId, @JoiningDateBegin, @JoiningDateEnd, @UpToDate, @UserName", startPageParam, pageSizeParam,
                    companyIdParam, branchIdParam, branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam,
                    departmentLineIdParam, employeeTypeIdParam, employeeJobTypeIdParam, employeeCardIdParam, joiningDateBeginParam, joiningDateEndParam,
                    upToDateParam, userNameParam).ToList();

                return employeeCompanyInfoes;


            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<EmployeeCompanyInfoModel> GetEmployeeIndividualHoliday(int startPage, int pageSize, EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel)
        {

            var startPageParam = new SqlParameter {ParameterName = "StartRowIndex", Value = startPage};
            var pageSizeParam = new SqlParameter {ParameterName = "MaxRows", Value = pageSize};

            int companyId = -1;
            if (searchFieldModel.SearchByCompanyId > 0)
                companyId = searchFieldModel.SearchByCompanyId;
            var companyIdParam = new SqlParameter {ParameterName = "CompanyId", Value = companyId};

            int branchId = -1;
            if (searchFieldModel.SearchByBranchId > 0)
                branchId = searchFieldModel.SearchByBranchId;
            var branchIdParam = new SqlParameter {ParameterName = "BranchId", Value = branchId};

            int branchUnitId = -1;
            if (searchFieldModel.SearchByBranchUnitId > 0)
                branchUnitId = searchFieldModel.SearchByBranchUnitId;
            var branchUnitIdParam = new SqlParameter {ParameterName = "BranchUnitId", Value = branchUnitId};

            int branchUnitDepartmentId = -1;
            if (searchFieldModel.SearchByBranchUnitDepartmentId > 0)
                branchUnitDepartmentId = searchFieldModel.SearchByBranchUnitDepartmentId;
            var branchUnitDepartmentIdParam = new SqlParameter {ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId};

            int departmentSectionId = -1;
            if (searchFieldModel.SearchByDepartmentSectionId > 0)
                departmentSectionId = searchFieldModel.SearchByDepartmentSectionId;
            var departmentSectionIdParam = new SqlParameter {ParameterName = "DepartmentSectionId", Value = departmentSectionId};

            int departmentLineId = -1;
            if (searchFieldModel.SearchByDepartmentLineId > 0)
                departmentLineId = searchFieldModel.SearchByDepartmentLineId;
            var departmentLineIdParam = new SqlParameter {ParameterName = "DepartmentLineId", Value = departmentLineId};

            int employeeTypeId = -1;
            if (searchFieldModel.SearchByEmployeeTypeId > 0)
                employeeTypeId = searchFieldModel.SearchByEmployeeTypeId;
            var employeeTypeIdParam = new SqlParameter {ParameterName = "EmployeeTypeId", Value = employeeTypeId};

            int employeeJobTypeId = -1;
            if (searchFieldModel.ExistingJobTypeId > 0)
                employeeJobTypeId = searchFieldModel.ExistingJobTypeId;
            var employeeJobTypeIdParam = new SqlParameter {ParameterName = "EmployeeJobTypeId", Value = employeeJobTypeId};

            string employeeCardId = string.Empty;
            if (!String.IsNullOrEmpty(searchFieldModel.SearchByEmployeeCardId))
                employeeCardId = searchFieldModel.SearchByEmployeeCardId;
            var employeeCardIdParam = new SqlParameter {ParameterName = "EmployeeCardId", Value = employeeCardId};

            DateTime? joiningDateBegin = searchFieldModel.JoiningDateBegin;
            if (joiningDateBegin == null)
                joiningDateBegin = new DateTime(1900, 01, 01);
            var joiningDateBeginParam = new SqlParameter {ParameterName = "JoiningDateBegin", Value = joiningDateBegin};

            DateTime? joiningDateeEnd = searchFieldModel.JoiningDateEnd;
            if (joiningDateeEnd == null)
                joiningDateeEnd = new DateTime(1900, 01, 01);
            var joiningDateEndParam = new SqlParameter {ParameterName = "JoiningDateEnd", Value = joiningDateeEnd};

            DateTime? upToDate = new DateTime(1900, 01, 01);
            if (searchFieldModel.UpToDate != null)
                upToDate = searchFieldModel.UpToDate;
            var upToDateParam = new SqlParameter {ParameterName = "UpToDate", Value = upToDate};

            var userNameParam = new SqlParameter {ParameterName = "UserName", Value = PortalContext.CurrentUser.Name};

            var employeeCompanyInfoes = Context.Database.SqlQuery<EmployeeCompanyInfoModel>("HRMSPGetEmployeeIndividualHoliday @StartRowIndex, @MaxRows, @CompanyId, " +
                                                                                            "@BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                                            "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeJobTypeId, " +
                                                                                            "@EmployeeCardId, @JoiningDateBegin, @JoiningDateEnd, @UpToDate, @UserName", startPageParam, pageSizeParam,

                companyIdParam, branchIdParam, branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam,
                departmentLineIdParam, employeeTypeIdParam, employeeJobTypeIdParam, employeeCardIdParam, joiningDateBeginParam, joiningDateEndParam,
                upToDateParam, userNameParam).ToList();

            return employeeCompanyInfoes;
        }

        public List<EmployeeCompanyInfoModel> GetEmployeesForAssigingLine(EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel)
        {
            try
            {
                int companyId = -1;
                if (searchFieldModel.SearchByCompanyId > 0)
                    companyId = searchFieldModel.SearchByCompanyId;
                var companyIdParam = new SqlParameter {ParameterName = "CompanyId", Value = companyId};

                int branchId = -1;
                if (searchFieldModel.SearchByBranchId > 0)
                    branchId = searchFieldModel.SearchByBranchId;
                var branchIdParam = new SqlParameter {ParameterName = "BranchId", Value = branchId};

                int branchUnitId = -1;
                if (searchFieldModel.SearchByBranchUnitId > 0)
                    branchUnitId = searchFieldModel.SearchByBranchUnitId;
                var branchUnitIdParam = new SqlParameter {ParameterName = "BranchUnitId", Value = branchUnitId};


                int branchUnitDepartmentId = -1;
                if (searchFieldModel.SearchByBranchUnitDepartmentId > 0)
                    branchUnitDepartmentId = searchFieldModel.SearchByBranchUnitDepartmentId;
                var branchUnitDepartmentIdParam = new SqlParameter {ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId};

                int departmentSectionId = -1;
                if (searchFieldModel.SearchByDepartmentSectionId > 0)
                    departmentSectionId = searchFieldModel.SearchByDepartmentSectionId;
                var departmentSectionIdParam = new SqlParameter {ParameterName = "DepartmentSectionId", Value = departmentSectionId};

                int departmentLineId = -1;
                if (searchFieldModel.SearchByDepartmentLineId > 0)
                    departmentLineId = searchFieldModel.SearchByDepartmentLineId;
                var departmentLineIdParam = new SqlParameter {ParameterName = "DepartmentLineId", Value = departmentLineId};


                int employeeTypeId = -1;
                if (searchFieldModel.SearchByEmployeeTypeId > 0)
                    employeeTypeId = searchFieldModel.SearchByEmployeeTypeId;
                var employeeTypeIdParam = new SqlParameter {ParameterName = "EmployeeTypeId", Value = employeeTypeId};

                string employeeCardId = string.Empty;
                if (!String.IsNullOrEmpty(searchFieldModel.SearchByEmployeeCardId))
                    employeeCardId = searchFieldModel.SearchByEmployeeCardId;
                var employeeCardIdParam = new SqlParameter {ParameterName = "EmployeeCardId", Value = employeeCardId};

                DateTime? joiningDateBegin = searchFieldModel.JoiningDateBegin;
                if (joiningDateBegin == null)
                    joiningDateBegin = new DateTime(1900, 01, 01);
                var joiningDateBeginParam = new SqlParameter {ParameterName = "JoiningDateBegin", Value = joiningDateBegin};

                DateTime? joiningDateeEnd = searchFieldModel.JoiningDateEnd;
                if (joiningDateeEnd == null)
                    joiningDateeEnd = new DateTime(1900, 01, 01);
                var joiningDateEndParam = new SqlParameter {ParameterName = "JoiningDateEnd", Value = joiningDateeEnd};

                DateTime? upToDate = new DateTime(1900, 01, 01);
                if (searchFieldModel.UpToDate != null)
                    upToDate = searchFieldModel.UpToDate;
                var fromDateParam = new SqlParameter {ParameterName = "UpToDate", Value = upToDate};


                var userNameParam = new SqlParameter {ParameterName = "UserName", Value = PortalContext.CurrentUser.Name};



                var employeeCompanyInfoes = Context.Database.SqlQuery<EmployeeCompanyInfoModel>("HRMSPGetEmployeesForAssigingLine @CompanyId, " +
                                                                                                "@BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                                                "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, " +
                                                                                                "@EmployeeCardId, @JoiningDateBegin, @JoiningDateEnd, @UpToDate, @UserName",
                    companyIdParam, branchIdParam, branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam,
                    departmentLineIdParam, employeeTypeIdParam, employeeCardIdParam, joiningDateBeginParam, joiningDateEndParam, fromDateParam,
                    userNameParam).ToList();

                return employeeCompanyInfoes;


            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<EmployeeCompanyInfoModel> GetEmployeesForIndividualHoliday(EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel)
        {
            try
            {
                int companyId = -1;
                if (searchFieldModel.SearchByCompanyId > 0)
                    companyId = searchFieldModel.SearchByCompanyId;
                var companyIdParam = new SqlParameter {ParameterName = "CompanyId", Value = companyId};

                int branchId = -1;
                if (searchFieldModel.SearchByBranchId > 0)
                    branchId = searchFieldModel.SearchByBranchId;
                var branchIdParam = new SqlParameter {ParameterName = "BranchId", Value = branchId};

                int branchUnitId = -1;
                if (searchFieldModel.SearchByBranchUnitId > 0)
                    branchUnitId = searchFieldModel.SearchByBranchUnitId;
                var branchUnitIdParam = new SqlParameter {ParameterName = "BranchUnitId", Value = branchUnitId};


                int branchUnitDepartmentId = -1;
                if (searchFieldModel.SearchByBranchUnitDepartmentId > 0)
                    branchUnitDepartmentId = searchFieldModel.SearchByBranchUnitDepartmentId;
                var branchUnitDepartmentIdParam = new SqlParameter {ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId};

                int departmentSectionId = -1;
                if (searchFieldModel.SearchByDepartmentSectionId > 0)
                    departmentSectionId = searchFieldModel.SearchByDepartmentSectionId;
                var departmentSectionIdParam = new SqlParameter {ParameterName = "DepartmentSectionId", Value = departmentSectionId};

                int departmentLineId = -1;
                if (searchFieldModel.SearchByDepartmentLineId > 0)
                    departmentLineId = searchFieldModel.SearchByDepartmentLineId;
                var departmentLineIdParam = new SqlParameter {ParameterName = "DepartmentLineId", Value = departmentLineId};


                int employeeTypeId = -1;
                if (searchFieldModel.SearchByEmployeeTypeId > 0)
                    employeeTypeId = searchFieldModel.SearchByEmployeeTypeId;
                var employeeTypeIdParam = new SqlParameter {ParameterName = "EmployeeTypeId", Value = employeeTypeId};

                string employeeCardId = string.Empty;
                if (!String.IsNullOrEmpty(searchFieldModel.SearchByEmployeeCardId))
                    employeeCardId = searchFieldModel.SearchByEmployeeCardId;
                var employeeCardIdParam = new SqlParameter {ParameterName = "EmployeeCardId", Value = employeeCardId};

                DateTime? joiningDateBegin = searchFieldModel.JoiningDateBegin;
                if (joiningDateBegin == null)
                    joiningDateBegin = new DateTime(1900, 01, 01);
                var joiningDateBeginParam = new SqlParameter {ParameterName = "JoiningDateBegin", Value = joiningDateBegin};

                DateTime? joiningDateeEnd = searchFieldModel.JoiningDateEnd;
                if (joiningDateeEnd == null)
                    joiningDateeEnd = new DateTime(1900, 01, 01);
                var joiningDateEndParam = new SqlParameter {ParameterName = "JoiningDateEnd", Value = joiningDateeEnd};

                DateTime? upToDate = new DateTime(1900, 01, 01);
                if (searchFieldModel.UpToDate != null)
                    upToDate = searchFieldModel.UpToDate;
                var fromDateParam = new SqlParameter {ParameterName = "UpToDate", Value = upToDate};


                var userNameParam = new SqlParameter {ParameterName = "UserName", Value = PortalContext.CurrentUser.Name};



                var employeeCompanyInfoes = Context.Database.SqlQuery<EmployeeCompanyInfoModel>("HRMSPGetEmployeesForAssigingLine @CompanyId, " +
                                                                                                "@BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                                                "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, " +
                                                                                                "@EmployeeCardId, @JoiningDateBegin, @JoiningDateEnd, @UpToDate, @UserName",
                    companyIdParam, branchIdParam, branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam,
                    departmentLineIdParam, employeeTypeIdParam, employeeCardIdParam, joiningDateBeginParam, joiningDateEndParam, fromDateParam,
                    userNameParam).ToList();

                return employeeCompanyInfoes;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }


        public int AssignBulkEmployeeLine(SearchFieldModel searchFieldModel, EmployeeCompanyInfoModel model)
        {

            var processedResult = 0;

            var dataTableEmployeeId = new DataTable();
            dataTableEmployeeId.Columns.Add("EmployeeId", typeof (Guid));
            foreach (var employeeId in searchFieldModel.EmployeeIdList)
            {
                var dataRow = dataTableEmployeeId.NewRow();
                dataRow[0] = employeeId;
                dataTableEmployeeId.Rows.Add(dataRow);
            }

            try
            {
                var connectionString = Context.Database.Connection.ConnectionString;
                SqlConnection conn = new SqlConnection(connectionString);

                using (conn)
                {
                    SqlCommand cmd = new SqlCommand("dbo.HRMSPAssignEmployeeLine", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter employeeIdCardListParam = cmd.Parameters.AddWithValue("@EmployeeList", dataTableEmployeeId);
                    employeeIdCardListParam.SqlDbType = SqlDbType.Structured;

                    int targetDepartmentLineId = -1;
                    if (searchFieldModel.TargetDepartmentLineId > 0)
                        targetDepartmentLineId = searchFieldModel.TargetDepartmentLineId;
                    SqlParameter targetDepartmentLineIdParam = cmd.Parameters.AddWithValue("@TargetDepartmentLineId", targetDepartmentLineId);
                    targetDepartmentLineIdParam.SqlDbType = SqlDbType.Int;

                    DateTime? effectiveFromDate = new DateTime(1900, 01, 01);
                    if (searchFieldModel.EffectiveFromDate != null)
                        effectiveFromDate = searchFieldModel.EffectiveFromDate;
                    SqlParameter effectiveFromDateParam = cmd.Parameters.AddWithValue("@EffectiveFromDate", effectiveFromDate);
                    effectiveFromDateParam.SqlDbType = SqlDbType.DateTime;

                    SqlParameter userNameParam = cmd.Parameters.AddWithValue("@UserName", PortalContext.CurrentUser.Name);
                    userNameParam.SqlDbType = SqlDbType.NVarChar;

                    conn.Open();
                    cmd.CommandTimeout = 36000;
                    processedResult = Convert.ToInt16(cmd.ExecuteScalar());
                    conn.Close();
                }
            }
            catch (Exception exception)
            {
                processedResult = 0;
            }
            return processedResult;
        }

        public int AssignIndividualHoliday(SearchFieldModel searchFieldModel, EmployeeCompanyInfoModel model)
        {
            var processedResult = 0;

            EmployeeHoliday holiday = new EmployeeHoliday();

            foreach (var employeeId in searchFieldModel.EmployeeIdList)
            {
                if (model.FromDate != null) holiday.FromDate = model.FromDate.Value;
                if (model.ToDate != null) holiday.ToDate = model.ToDate.Value;

                holiday.EmployeeId = employeeId;
                var firstOrDefault = Context.Employees.FirstOrDefault(p => p.EmployeeId == employeeId);
                if (firstOrDefault != null) holiday.EmployeeCardId = firstOrDefault.EmployeeCardId;

                holiday.CreatedBy = PortalContext.CurrentUser.UserId;
                holiday.CreatedDate = DateTime.Now;
                holiday.EditedBy = PortalContext.CurrentUser.UserId;
                holiday.EditedDate = DateTime.Now;
                holiday.IsActive = true;

                Context.EmployeeHolidays.Add(holiday);
                processedResult = Context.SaveChanges();

            }
            return processedResult;
        }

        public int AssignIndividualWeekend(SearchFieldModel searchFieldModel, EmployeeCompanyInfoModel model)
        {
            var processedResult = 0;

            EmployeeWeekend weekend = new EmployeeWeekend();

            foreach (var employeeId in searchFieldModel.EmployeeIdList)
            {
                if (model.FromDate != null) weekend.FromDate = model.FromDate.Value;
                if (model.ToDate != null) weekend.ToDate = model.ToDate.Value;

                weekend.EmployeeId = employeeId;
                var firstOrDefault = Context.Employees.FirstOrDefault(p => p.EmployeeId == employeeId);
                if (firstOrDefault != null) weekend.EmployeeCardId = firstOrDefault.EmployeeCardId;

                weekend.CreatedBy = PortalContext.CurrentUser.UserId;
                weekend.CreatedDate = DateTime.Now;
                weekend.EditedBy = PortalContext.CurrentUser.UserId;
                weekend.EditedDate = DateTime.Now;
                weekend.IsActive = true;

                Context.EmployeeWeekends.Add(weekend);
                processedResult = Context.SaveChanges();

            }
            return processedResult;
        }


        public List<EmployeeCompanyInfoModel> GetEmployeesForAssigingJobType(EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel)
        {
            try
            {
                int companyId = -1;
                if (searchFieldModel.SearchByCompanyId > 0)
                    companyId = searchFieldModel.SearchByCompanyId;
                var companyIdParam = new SqlParameter {ParameterName = "CompanyId", Value = companyId};

                int branchId = -1;
                if (searchFieldModel.SearchByBranchId > 0)
                    branchId = searchFieldModel.SearchByBranchId;
                var branchIdParam = new SqlParameter {ParameterName = "BranchId", Value = branchId};

                int branchUnitId = -1;
                if (searchFieldModel.SearchByBranchUnitId > 0)
                    branchUnitId = searchFieldModel.SearchByBranchUnitId;
                var branchUnitIdParam = new SqlParameter {ParameterName = "BranchUnitId", Value = branchUnitId};


                int branchUnitDepartmentId = -1;
                if (searchFieldModel.SearchByBranchUnitDepartmentId > 0)
                    branchUnitDepartmentId = searchFieldModel.SearchByBranchUnitDepartmentId;
                var branchUnitDepartmentIdParam = new SqlParameter {ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId};

                int departmentSectionId = -1;
                if (searchFieldModel.SearchByDepartmentSectionId > 0)
                    departmentSectionId = searchFieldModel.SearchByDepartmentSectionId;
                var departmentSectionIdParam = new SqlParameter {ParameterName = "DepartmentSectionId", Value = departmentSectionId};

                int departmentLineId = -1;
                if (searchFieldModel.SearchByDepartmentLineId > 0)
                    departmentLineId = searchFieldModel.SearchByDepartmentLineId;
                var departmentLineIdParam = new SqlParameter {ParameterName = "DepartmentLineId", Value = departmentLineId};


                int employeeTypeId = -1;
                if (searchFieldModel.SearchByEmployeeTypeId > 0)
                    employeeTypeId = searchFieldModel.SearchByEmployeeTypeId;
                var employeeTypeIdParam = new SqlParameter {ParameterName = "EmployeeTypeId", Value = employeeTypeId};

                string employeeCardId = string.Empty;
                if (!String.IsNullOrEmpty(searchFieldModel.SearchByEmployeeCardId))
                    employeeCardId = searchFieldModel.SearchByEmployeeCardId;
                var employeeCardIdParam = new SqlParameter {ParameterName = "EmployeeCardId", Value = employeeCardId};

                DateTime? joiningDateBegin = searchFieldModel.JoiningDateBegin;
                if (joiningDateBegin == null)
                    joiningDateBegin = new DateTime(1900, 01, 01);
                var joiningDateBeginParam = new SqlParameter {ParameterName = "JoiningDateBegin", Value = joiningDateBegin};

                DateTime? joiningDateeEnd = searchFieldModel.JoiningDateEnd;
                if (joiningDateeEnd == null)
                    joiningDateeEnd = new DateTime(1900, 01, 01);
                var joiningDateEndParam = new SqlParameter {ParameterName = "JoiningDateEnd", Value = joiningDateeEnd};

                int? existingJobTypeId = -1;
                if (searchFieldModel.ExistingJobTypeId > 0)
                    existingJobTypeId = searchFieldModel.ExistingJobTypeId;
                var existingJobTypeIdParam = new SqlParameter {ParameterName = "ExistingJobTypeId", Value = existingJobTypeId};


                DateTime? upToDate = new DateTime(1900, 01, 01);
                if (searchFieldModel.UpToDate != null)
                    upToDate = searchFieldModel.UpToDate;
                var fromDateParam = new SqlParameter {ParameterName = "UpToDate", Value = upToDate};


                var userNameParam = new SqlParameter {ParameterName = "UserName", Value = PortalContext.CurrentUser.Name};



                var employeeCompanyInfoes = Context.Database.SqlQuery<EmployeeCompanyInfoModel>("spHrmGetEmployeesForAssigingJobType @CompanyId, " +
                                                                                                "@BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                                                "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, " +
                                                                                                "@EmployeeCardId, @JoiningDateBegin, @JoiningDateEnd, @ExistingJobTypeId,@UpToDate, @UserName",
                    companyIdParam, branchIdParam, branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam,
                    departmentLineIdParam, employeeTypeIdParam, employeeCardIdParam, joiningDateBeginParam, joiningDateEndParam,
                    fromDateParam, existingJobTypeIdParam, userNameParam).ToList();

                return employeeCompanyInfoes;


            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public int AssignBulkEmployeeJobType(SearchFieldModel searchFieldModel, EmployeeCompanyInfoModel model)
        {

            var processedResult = 0;

            var dataTableEmployeeId = new DataTable();
            dataTableEmployeeId.Columns.Add("EmployeeId", typeof (Guid));
            foreach (var employeeId in searchFieldModel.EmployeeIdList)
            {
                var dataRow = dataTableEmployeeId.NewRow();
                dataRow[0] = employeeId;
                dataTableEmployeeId.Rows.Add(dataRow);
            }

            try
            {
                var connectionString = Context.Database.Connection.ConnectionString;
                SqlConnection conn = new SqlConnection(connectionString);

                using (conn)
                {
                    SqlCommand cmd = new SqlCommand("dbo.spHrmAssignEmployeeJobType", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter employeeIdCardListParam = cmd.Parameters.AddWithValue("@EmployeeList", dataTableEmployeeId);
                    employeeIdCardListParam.SqlDbType = SqlDbType.Structured;

                    int targetJobTypeId = -1;
                    if (searchFieldModel.TargetJobTypeId > 0)
                        targetJobTypeId = searchFieldModel.TargetJobTypeId;
                    SqlParameter targetJobTypeIdParam = cmd.Parameters.AddWithValue("@TargetJobTypeId", targetJobTypeId);
                    targetJobTypeIdParam.SqlDbType = SqlDbType.Int;

                    DateTime? effectiveFromDate = new DateTime(1900, 01, 01);
                    if (searchFieldModel.EffectiveFromDate != null)
                        effectiveFromDate = searchFieldModel.EffectiveFromDate;
                    SqlParameter effectiveFromDateParam = cmd.Parameters.AddWithValue("@EffectiveFromDate", effectiveFromDate);
                    effectiveFromDateParam.SqlDbType = SqlDbType.DateTime;

                    SqlParameter userNameParam = cmd.Parameters.AddWithValue("@UserName", PortalContext.CurrentUser.Name);
                    userNameParam.SqlDbType = SqlDbType.NVarChar;

                    conn.Open();
                    cmd.CommandTimeout = 36000;
                    processedResult = Convert.ToInt16(cmd.ExecuteScalar());
                    conn.Close();
                }
            }
            catch (Exception exception)
            {
                processedResult = 0;
            }
            return processedResult;
        }

        public VEmployeeCompanyInfoDetail GetEmployeeByEmployeeId(Guid employeeId)
        {
            return Context.VEmployeeCompanyInfoDetails.First(x => x.EmployeeId == employeeId);
        }
    }
}
