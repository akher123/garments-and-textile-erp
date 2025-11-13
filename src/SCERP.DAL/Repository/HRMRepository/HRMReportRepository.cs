using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Model.HRMModel;
using System.Data.Entity;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class HRMReportRepository : Repository<Employee>, IHRMReportRepository
    {
        private readonly SCERPDBContext _context;

        public HRMReportRepository(SCERPDBContext context)
            : base(context)
        {
            _context = context;
        }

        public List<JobCardInfoModel> GetJobCardInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, string employeeCardId, int Year, int Month, DateTime? fromDate,
            DateTime? toDate, string userName)
        {
            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter { ParameterName = "Year", Value = Year };

                var monthParam = new SqlParameter { ParameterName = "Month", Value = Month };

                var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

                var toDateParam = new SqlParameter { ParameterName = "ToDate", Value = toDate };


                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };



                return Context.Database.SqlQuery<JobCardInfoModel>("SPGetEmployeeJobCard @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                   "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId," +
                                                                   "@EmployeeCardId, @Year, @Month, @FromDate, @ToDate, @UserName", companyIdParam, branchIdParam,
                    branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                    employeeTypeIdParam, employeeCardIdParam, yearParam, monthParam, fromDateParam, toDateParam, userNameParam).ToList();


            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<JobCardInfoModel> GetJobCardInfo10PM(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int Year, int Month, DateTime? fromDate, DateTime? toDate, string userName)
        {
            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter { ParameterName = "Year", Value = Year };

                var monthParam = new SqlParameter { ParameterName = "Month", Value = Month };

                var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

                var toDateParam = new SqlParameter { ParameterName = "ToDate", Value = toDate };

                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };


                return Context.Database.SqlQuery<JobCardInfoModel>("SPGetEmployeeJobCard_10PM @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                   "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId," +
                                                                   "@EmployeeCardId, @Year, @Month, @FromDate, @ToDate, @UserName", companyIdParam, branchIdParam,
                    branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                    employeeTypeIdParam, employeeCardIdParam, yearParam, monthParam, fromDateParam, toDateParam, userNameParam).ToList();
            }

            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<JobCardInfoModel> GetJobCardInfo10PMNoWeekend(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int Year, int Month, DateTime? fromDate, DateTime? toDate, string userName)
        {
            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter { ParameterName = "Year", Value = Year };

                var monthParam = new SqlParameter { ParameterName = "Month", Value = Month };

                var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

                var toDateParam = new SqlParameter { ParameterName = "ToDate", Value = toDate };

                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };

                return Context.Database.SqlQuery<JobCardInfoModel>("SPGetEmployeeJobCard_10PM_NoWeekend @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                   "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId," +
                                                                   "@EmployeeCardId, @Year, @Month, @FromDate, @ToDate, @UserName", companyIdParam, branchIdParam,
                    branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                    employeeTypeIdParam, employeeCardIdParam, yearParam, monthParam, fromDateParam, toDateParam, userNameParam).ToList();
            }

            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<JobCardInfoModel> GetJobCardOriginalNoWeekend(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int Year, int Month, DateTime? fromDate, DateTime? toDate, string userName)
        {
            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter { ParameterName = "Year", Value = Year };

                var monthParam = new SqlParameter { ParameterName = "Month", Value = Month };

                var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

                var toDateParam = new SqlParameter { ParameterName = "ToDate", Value = toDate };

                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };

                return Context.Database.SqlQuery<JobCardInfoModel>("SPGetEmployeeJobCard_Original_NoWeekend @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                   "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId," +
                                                                   "@EmployeeCardId, @Year, @Month, @FromDate, @ToDate, @UserName", companyIdParam, branchIdParam,
                    branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                    employeeTypeIdParam, employeeCardIdParam, yearParam, monthParam, fromDateParam, toDateParam, userNameParam).ToList();
            }

            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<ShortLeaveSummaryModel> GetShortLeaveSummary(int branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, string employeeCardId, DateTime startDate, DateTime endDate)
        {
            var shortLeaveSummary = new List<ShortLeaveSummaryModel>();

            try
            {
                var branchUnitDepartmentIdParam = (branchUnitDepartmentId > 0) ?
                    new ObjectParameter("BranchUnitDepartmentID", branchUnitDepartmentId) :
                    new ObjectParameter("BranchUnitDepartmentID", typeof(int));

                var departmentSectionIdParam = (departmentSectionId > 0) ?
                    new ObjectParameter("DepartmentSectionId", departmentSectionId) :
                    new ObjectParameter("DepartmentSectionId", typeof(int));

                var departmentLineIdParam = (departmentLineId > 0) ?
                    new ObjectParameter("DepartmentLineId", departmentLineId) :
                    new ObjectParameter("DepartmentLineId", typeof(int));

                var employeeCardIdParam = (!string.IsNullOrEmpty(employeeCardId)) ?
                    new ObjectParameter("EmployeeCardNo", employeeCardId) :
                    new ObjectParameter("EmployeeCardNo", typeof(int));

                var fromDateParam = new ObjectParameter("FromDate", startDate);

                var toDateParam = new ObjectParameter("ToDate", endDate);

                var userNameParam = !String.IsNullOrEmpty(PortalContext.CurrentUser.Name) ?
                    new ObjectParameter("UserName", PortalContext.CurrentUser.Name) :
                    new ObjectParameter("UserName", typeof(string));


                var spShortLeaveSummary = Context.SPGetShortLeaveSummary(branchUnitDepartmentIdParam,
                    departmentSectionIdParam, departmentLineIdParam,
                    employeeCardIdParam, fromDateParam, toDateParam,
                    userNameParam);


                foreach (var shortLeave in spShortLeaveSummary)
                {
                    var shortLeaveSummaryModel = new ShortLeaveSummaryModel();

                    if (shortLeave.EmployeeId != null)
                        shortLeaveSummaryModel.EmployeeId = (Guid)shortLeave.EmployeeId;
                    shortLeaveSummaryModel.EmployeeCardId = shortLeave.EmployeeCardId;
                    shortLeaveSummaryModel.Name = shortLeave.Name;
                    shortLeaveSummaryModel.Designation = shortLeave.Designation;
                    shortLeaveSummaryModel.Grade = shortLeave.Grade;
                    shortLeaveSummaryModel.EmployeeType = shortLeave.EmployeeType;
                    shortLeaveSummaryModel.CompanyName = shortLeave.CompanyName;
                    shortLeaveSummaryModel.CompanyAddress = shortLeave.CompanyAddress;
                    shortLeaveSummaryModel.Branch = shortLeave.Branch;
                    shortLeaveSummaryModel.Unit = shortLeave.Unit;
                    shortLeaveSummaryModel.Department = shortLeave.Department;
                    shortLeaveSummaryModel.Section = shortLeave.Section;
                    shortLeaveSummaryModel.Line = shortLeave.Line;
                    shortLeaveSummaryModel.JoiningDate = shortLeave.JoiningDate;
                    shortLeaveSummaryModel.ShortLeaveDate = shortLeave.ShortLeaveDate;
                    shortLeaveSummaryModel.ReasonType = shortLeave.ReasonType;
                    shortLeaveSummaryModel.ReasonName = shortLeave.ReasonName;
                    shortLeaveSummaryModel.FromTime = shortLeave.FromTime;
                    shortLeaveSummaryModel.ToTime = shortLeave.ToTime;
                    shortLeaveSummaryModel.TotalHours = shortLeave.TotalHours;
                    shortLeaveSummaryModel.DepartmentId = shortLeave.DepartmentId;
                    shortLeaveSummaryModel.SectionId = shortLeave.SectionId;
                    shortLeaveSummaryModel.LineId = shortLeave.LineId;
                    shortLeaveSummaryModel.FromDate = shortLeave.FromDate;
                    shortLeaveSummaryModel.ToDate = shortLeave.ToDate;

                    shortLeaveSummary.Add(shortLeaveSummaryModel);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return shortLeaveSummary;

        }

        public List<ShortLeaveDetailModel> GetShortLeaveDetail(int? companyId, int? branchId,
            int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, Guid employeeId, DateTime fromDate, DateTime toDate, int? reasonType)
        {
            var shortLeaveDetail = new List<ShortLeaveDetailModel>();

            try
            {
                var companyIdParam = (companyId > 0) ?
                    new ObjectParameter("CompanyID", companyId) :
                    new ObjectParameter("CompanyID", typeof(int));

                var branchIdParam = (branchId > 0) ?
                    new ObjectParameter("BranchID", branchId) :
                    new ObjectParameter("BranchID", typeof(int));

                var branchUnitIdParam = (branchUnitId > 0) ?
                    new ObjectParameter("BranchUnitID", branchUnitId) :
                    new ObjectParameter("BranchUnitID", typeof(int));

                var branchUnitDepartmentIdParam = (branchUnitDepartmentId > 0) ?
                    new ObjectParameter("BranchUnitDepartmentID", branchUnitDepartmentId) :
                    new ObjectParameter("BranchUnitDepartmentID", typeof(int));

                var departmentSectionIdParam = (departmentSectionId > 0) ?
                    new ObjectParameter("DepartmentSectionId", departmentSectionId) :
                    new ObjectParameter("DepartmentSectionId", typeof(int));

                var departmentLineIdParam = (departmentLineId > 0) ?
                    new ObjectParameter("DepartmentLineId", departmentLineId) :
                    new ObjectParameter("DepartmentLineId", typeof(int));

                var employeeIdParam = (employeeId != Guid.Parse("00000000-0000-0000-0000-000000000000")) ?
                    new ObjectParameter("EmployeeId", employeeId) :
                    new ObjectParameter("EmployeeId", typeof(Guid));


                var startDateParam = new ObjectParameter("FromDate", fromDate);

                var endDateParam = new ObjectParameter("ToDate", toDate);


                var reasonTypeIdParam = (reasonType > 0) ?
                    new ObjectParameter("ReasonType", reasonType) :
                    new ObjectParameter("ReasonType", typeof(byte));


                var userNameParam = !String.IsNullOrEmpty(PortalContext.CurrentUser.Name) ?
                    new ObjectParameter("UserName", PortalContext.CurrentUser.Name) :
                    new ObjectParameter("UserName", typeof(string));


                var spShortLeaveSummary = Context.SPGetShortLeaveDetail(companyIdParam, branchIdParam, branchUnitIdParam, branchUnitDepartmentIdParam,
                    departmentSectionIdParam, departmentLineIdParam, employeeIdParam, startDateParam,
                    endDateParam, reasonTypeIdParam, userNameParam);


                foreach (var shortLeave in spShortLeaveSummary)
                {
                    var shortLeaveDetailModel = new ShortLeaveDetailModel();
                    shortLeaveDetailModel.CompanyName = shortLeave.CompanyName;
                    shortLeaveDetailModel.Branch = shortLeave.Branch;
                    shortLeaveDetailModel.Unit = shortLeave.Unit;
                    shortLeaveDetailModel.Department = shortLeave.Department;
                    shortLeaveDetailModel.Section = shortLeave.Section;
                    shortLeaveDetailModel.Line = shortLeave.Line;
                    shortLeaveDetailModel.Name = shortLeave.Name;
                    shortLeaveDetailModel.EmployeeCardId = shortLeave.EmployeeCardId;
                    shortLeaveDetailModel.EmployeeType = shortLeave.EmployeeType;
                    shortLeaveDetailModel.Grade = shortLeave.Grade;
                    shortLeaveDetailModel.Designation = shortLeave.Designation;
                    shortLeaveDetailModel.Date = shortLeave.Date;
                    shortLeaveDetailModel.MonthDay = shortLeave.MonthDay;
                    shortLeaveDetailModel.ReasonName = shortLeave.ReasonName;
                    shortLeaveDetailModel.ReasonDescription = shortLeave.ReasonDescription;
                    shortLeaveDetailModel.FromTime = shortLeave.FromTime;
                    shortLeaveDetailModel.ToTime = shortLeave.ToTime;
                    shortLeaveDetailModel.TotalHours = shortLeave.TotalHours;
                    shortLeaveDetail.Add(shortLeaveDetailModel);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return shortLeaveDetail;
        }

        public List<AttendanceModel> GetEmployeeAttendanceInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId,
            int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
            int? branchUnitWorkShiftId, DateTime? fromDate, DateTime? toDate, string attendanceStatus,
            int? totalContinuousAbsentDays, bool otEnabled, bool extraOTEnabled, bool weekendOTEnabled)
        {

            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                if (branchUnitWorkShiftId == 0)
                    branchUnitWorkShiftId = -1;
                var branchUnitWorkShiftIdParam = new SqlParameter { ParameterName = "BranchUnitWorkShiftId", Value = branchUnitWorkShiftId };


                if (fromDate == null)
                    fromDate = new DateTime(1900, 01, 01);
                var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };


                if (toDate == null)
                    toDate = new DateTime(1900, 01, 01);
                var toDateParam = new SqlParameter { ParameterName = "ToDate", Value = toDate };


                if (String.IsNullOrEmpty(attendanceStatus))
                    attendanceStatus = string.Empty;
                var attendanceStatusParam = new SqlParameter { ParameterName = "AttendanceStatus", Value = attendanceStatus };

                if (totalContinuousAbsentDays == 0)
                    totalContinuousAbsentDays = -1;
                var totalContinuousAbsentDaysParam = new SqlParameter { ParameterName = "TotalContinuousAbsentDays", Value = totalContinuousAbsentDays };

                var oTHours = -1.00;
                if (otEnabled)
                    oTHours = 0.00;
                var oTHoursParam = new SqlParameter { ParameterName = "OTHours", Value = oTHours };

                var extraOTHours = -1.00;
                if (extraOTEnabled)
                    extraOTHours = 0.00;
                var extraOTHoursParam = new SqlParameter { ParameterName = "ExtraOTHours", Value = extraOTHours };

                var weekendOTHours = -1.00;
                if (weekendOTEnabled)
                    weekendOTHours = 0.00;
                var weekendOTHoursParam = new SqlParameter { ParameterName = "WeekendOTHours", Value = weekendOTHours };

                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = PortalContext.CurrentUser.Name };



                return Context.Database.SqlQuery<AttendanceModel>("HRMSPGetEmployeeAttendanceInfo @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                  "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeCardId, @BranchUnitWorkShiftId, @FromDate, " +
                                                                  "@Todate, @AttendanceStatus, @TotalContinuousAbsentDays, @OTHours, @ExtraOTHours, @WeekendOTHours, @UserName",
                    companyIdParam, branchIdParam, branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                    employeeTypeIdParam, employeeCardIdParam, branchUnitWorkShiftIdParam, fromDateParam, toDateParam,
                    attendanceStatusParam, totalContinuousAbsentDaysParam,
                    oTHoursParam, extraOTHoursParam, weekendOTHoursParam, userNameParam).ToList();



            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

        }

        public List<AttendanceSummaryModel> GetAttendanceSummaryInfo(int companyId, int branchId, int branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? branchUnitWorkShiftId,
            DateTime? transactionDate)
        {


            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (branchUnitWorkShiftId == 0)
                    branchUnitWorkShiftId = -1;
                var branchUnitWorkShiftIdParam = new SqlParameter { ParameterName = "BranchUnitWorkShiftId", Value = branchUnitWorkShiftId };


                var transactionDateParam = new SqlParameter("TransactionDate", transactionDate);

                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = PortalContext.CurrentUser.Name };


                return Context.Database.SqlQuery<AttendanceSummaryModel>("SPGetEmployeeAttendanceSummary @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                         "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @BranchUnitWorkShiftId, @TransactionDate, " +
                                                                         "@UserName", companyIdParam, branchIdParam, branchUnitIdParam,
                    branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam, employeeTypeIdParam,
                    branchUnitWorkShiftIdParam, transactionDateParam, userNameParam).ToList();



            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<AttendanceSummaryByDesignationModel> GetAttendanceSummaryByDesignationInfo(int companyId, int branchId, int branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? branchUnitWorkShiftId,
            DateTime? transactionDate)
        {


            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (branchUnitWorkShiftId == 0)
                    branchUnitWorkShiftId = -1;
                var branchUnitWorkShiftIdParam = new SqlParameter { ParameterName = "BranchUnitWorkShiftId", Value = branchUnitWorkShiftId };


                var transactionDateParam = new SqlParameter("TransactionDate", transactionDate);

                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = PortalContext.CurrentUser.Name };


                return Context.Database.SqlQuery<AttendanceSummaryByDesignationModel>("SPGetEmployeeAttendanceSummaryByDesignation @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                                      "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @BranchUnitWorkShiftId, @TransactionDate, " +
                                                                                      "@UserName", companyIdParam, branchIdParam, branchUnitIdParam,
                    branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam, employeeTypeIdParam,
                    branchUnitWorkShiftIdParam, transactionDateParam, userNameParam).ToList();



            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<EmployeeAllInfoModel> GetEmployeeAllInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId,
            int? bloodGroupId, int? genderId, int? religionId, int? maritalStateId, DateTime? joiningDateBegin,
            DateTime? joiningDateEnd, DateTime? confirmationDateBegin, DateTime? confirmationDateEnd,
            DateTime? quitDateBegin, DateTime? quitDateEnd, int? birthDayMonth, DateTime? mariageAnniversaryDateBegin,
            DateTime? mariageAnniversaryDateEnd, int? permanentCountryId, int? permanentDistrictId, int? educationLevelId, string employeeCardId,
            string employeeName, string mobileNo, int? activeStatus, string userName, DateTime? fromDate)
        {
            try
            {
                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (employeeGradeId == 0)
                    employeeGradeId = -1;
                var employeeGradeIdParam = new SqlParameter { ParameterName = "EmployeeGradeId", Value = employeeGradeId };

                if (employeeDesignationId == 0)
                    employeeDesignationId = -1;
                var employeeDesignationIdParam = new SqlParameter { ParameterName = "EmployeeDesignationId", Value = employeeDesignationId };

                if (bloodGroupId == 0)
                    bloodGroupId = -1;
                var bloodGroupIdParam = new SqlParameter { ParameterName = "BloodGroupId", Value = bloodGroupId };

                if (genderId == 0)
                    genderId = -1;
                var genderIdParam = new SqlParameter { ParameterName = "GenderId", Value = genderId };

                if (religionId == 0)
                    religionId = -1;
                var religionIdParam = new SqlParameter { ParameterName = "ReligionId", Value = religionId };

                if (maritalStateId == 0)
                    maritalStateId = -1;
                var maritalStateIdParam = new SqlParameter { ParameterName = "MaritalStateId", Value = maritalStateId };

                if (joiningDateBegin == null)
                    joiningDateBegin = new DateTime(1900, 01, 01);
                var joiningDateBeginParam = new SqlParameter { ParameterName = "JoiningDateBegin", Value = joiningDateBegin };

                if (joiningDateEnd == null)
                    joiningDateEnd = new DateTime(1900, 01, 01);
                var joiningDateEndParam = new SqlParameter { ParameterName = "JoiningDateEnd", Value = joiningDateEnd };

                if (confirmationDateBegin == null)
                    confirmationDateBegin = new DateTime(1900, 01, 01);
                var confirmationDateBeginParam = new SqlParameter { ParameterName = "ConfirmationDateBegin", Value = confirmationDateBegin };

                if (confirmationDateEnd == null)
                    confirmationDateEnd = new DateTime(1900, 01, 01);
                var confirmationDateEndParam = new SqlParameter { ParameterName = "ConfirmationDateEnd", Value = confirmationDateEnd };

                if (quitDateBegin == null)
                    quitDateBegin = new DateTime(1900, 01, 01);
                var quitDateBeginParam = new SqlParameter { ParameterName = "QuitDateBegin", Value = quitDateBegin };

                if (quitDateEnd == null)
                    quitDateEnd = new DateTime(1900, 01, 01);
                var quitDateEndParam = new SqlParameter { ParameterName = "QuitDateEnd", Value = quitDateEnd };

                if (birthDayMonth == 0)
                    birthDayMonth = -1;
                var birthDayMonthParam = new SqlParameter { ParameterName = "BirthDayMonth", Value = birthDayMonth };

                if (mariageAnniversaryDateBegin == null)
                    mariageAnniversaryDateBegin = new DateTime(1900, 01, 01);
                var mariageAnniversaryDateBeginParam = new SqlParameter { ParameterName = "MariageAnniversaryDateBegin", Value = mariageAnniversaryDateBegin };

                if (mariageAnniversaryDateEnd == null)
                    mariageAnniversaryDateEnd = new DateTime(1900, 01, 01);
                var mariageAnniversaryDateEndParam = new SqlParameter { ParameterName = "MariageAnniversaryDateEnd", Value = mariageAnniversaryDateEnd };

                if (permanentCountryId == 0)
                    permanentCountryId = -1;
                var permanentCountryIdParam = new SqlParameter { ParameterName = "PermanentCountryId", Value = permanentCountryId };

                if (permanentDistrictId == 0)
                    permanentDistrictId = -1;
                var permanentDistrictIdParam = new SqlParameter { ParameterName = "PermanentDistrictId", Value = permanentDistrictId };

                if (educationLevelId == 0)
                    educationLevelId = -1;
                var educationLevelIdParam = new SqlParameter { ParameterName = "EducationLevelId", Value = educationLevelId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                if (String.IsNullOrEmpty(employeeName))
                    employeeName = string.Empty;
                var employeeNameParam = new SqlParameter { ParameterName = "EmployeeName", Value = employeeName };

                if (String.IsNullOrEmpty(mobileNo))
                    mobileNo = string.Empty;
                var mobileNoParam = new SqlParameter { ParameterName = "MobileNo", Value = mobileNo };

                if (activeStatus == 0)
                    activeStatus = -1;
                var activeStatusParam = new SqlParameter { ParameterName = "ActiveStatus", Value = activeStatus };

                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };

                if (fromDate == null)
                    fromDate = new DateTime(1900, 01, 01);
                var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

                return Context.Database.SqlQuery<EmployeeAllInfoModel>("SPGetAllEmployeeInfoReport @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                       "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeGradeId, @EmployeeDesignationId, " +
                                                                       "@BloodGroupId, @GenderId, @ReligionId, @MaritalStateId, @JoiningDateBegin, @JoiningDateEnd, @ConfirmationDateBegin, " +
                                                                       "@ConfirmationDateEnd, @QuitDateBegin, @QuitDateEnd, @BirthDayMonth, @MariageAnniversaryDateBegin, @MariageAnniversaryDateEnd, " +
                                                                       "@PermanentCountryId, @PermanentDistrictId, @EducationLevelId, @EmployeeCardId, @EmployeeName, @MobileNo, @ActiveStatus, @UserName, " +
                                                                       "@FromDate ", companyIdParam, branchIdParam, branchUnitIdParam,
                    branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam, employeeTypeIdParam,
                    employeeGradeIdParam, employeeDesignationIdParam, bloodGroupIdParam, genderIdParam, religionIdParam, maritalStateIdParam,
                    joiningDateBeginParam, joiningDateEndParam, confirmationDateBeginParam, confirmationDateEndParam, quitDateBeginParam,
                    quitDateEndParam, birthDayMonthParam, mariageAnniversaryDateBeginParam, mariageAnniversaryDateEndParam,
                    permanentCountryIdParam, permanentDistrictIdParam, educationLevelIdParam, employeeCardIdParam, employeeNameParam,
                    mobileNoParam, activeStatusParam, userNameParam, fromDateParam).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<EmployeeAllInfoNewModel> GetEmployeeAllInfoNew(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId,
            int? bloodGroupId, int? genderId, int? religionId, int? maritalStateId, DateTime? joiningDateBegin,
            DateTime? joiningDateEnd, DateTime? confirmationDateBegin, DateTime? confirmationDateEnd,
            DateTime? quitDateBegin, DateTime? quitDateEnd, int? birthDayMonth, DateTime? mariageAnniversaryDateBegin,
            DateTime? mariageAnniversaryDateEnd, int? permanentCountryId, int? permanentDistrictId, int? educationLevelId, string employeeCardId,
            string employeeName, string mobileNo, int? activeStatus, string userName, DateTime? fromDate)
        {
            try
            {
                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (employeeGradeId == 0)
                    employeeGradeId = -1;
                var employeeGradeIdParam = new SqlParameter { ParameterName = "EmployeeGradeId", Value = employeeGradeId };

                if (employeeDesignationId == 0)
                    employeeDesignationId = -1;
                var employeeDesignationIdParam = new SqlParameter { ParameterName = "EmployeeDesignationId", Value = employeeDesignationId };

                if (bloodGroupId == 0)
                    bloodGroupId = -1;
                var bloodGroupIdParam = new SqlParameter { ParameterName = "BloodGroupId", Value = bloodGroupId };

                if (genderId == 0)
                    genderId = -1;
                var genderIdParam = new SqlParameter { ParameterName = "GenderId", Value = genderId };

                if (religionId == 0)
                    religionId = -1;
                var religionIdParam = new SqlParameter { ParameterName = "ReligionId", Value = religionId };

                if (maritalStateId == 0)
                    maritalStateId = -1;
                var maritalStateIdParam = new SqlParameter { ParameterName = "MaritalStateId", Value = maritalStateId };

                if (joiningDateBegin == null)
                    joiningDateBegin = new DateTime(1900, 01, 01);
                var joiningDateBeginParam = new SqlParameter { ParameterName = "JoiningDateBegin", Value = joiningDateBegin };

                if (joiningDateEnd == null)
                    joiningDateEnd = new DateTime(1900, 01, 01);
                var joiningDateEndParam = new SqlParameter { ParameterName = "JoiningDateEnd", Value = joiningDateEnd };

                if (confirmationDateBegin == null)
                    confirmationDateBegin = new DateTime(1900, 01, 01);
                var confirmationDateBeginParam = new SqlParameter { ParameterName = "ConfirmationDateBegin", Value = confirmationDateBegin };

                if (confirmationDateEnd == null)
                    confirmationDateEnd = new DateTime(1900, 01, 01);
                var confirmationDateEndParam = new SqlParameter { ParameterName = "ConfirmationDateEnd", Value = confirmationDateEnd };

                if (quitDateBegin == null)
                    quitDateBegin = new DateTime(1900, 01, 01);
                var quitDateBeginParam = new SqlParameter { ParameterName = "QuitDateBegin", Value = quitDateBegin };

                if (quitDateEnd == null)
                    quitDateEnd = new DateTime(1900, 01, 01);
                var quitDateEndParam = new SqlParameter { ParameterName = "QuitDateEnd", Value = quitDateEnd };

                if (birthDayMonth == 0)
                    birthDayMonth = -1;
                var birthDayMonthParam = new SqlParameter { ParameterName = "BirthDayMonth", Value = birthDayMonth };

                if (mariageAnniversaryDateBegin == null)
                    mariageAnniversaryDateBegin = new DateTime(1900, 01, 01);
                var mariageAnniversaryDateBeginParam = new SqlParameter { ParameterName = "MariageAnniversaryDateBegin", Value = mariageAnniversaryDateBegin };

                if (mariageAnniversaryDateEnd == null)
                    mariageAnniversaryDateEnd = new DateTime(1900, 01, 01);
                var mariageAnniversaryDateEndParam = new SqlParameter { ParameterName = "MariageAnniversaryDateEnd", Value = mariageAnniversaryDateEnd };

                if (permanentCountryId == 0)
                    permanentCountryId = -1;
                var permanentCountryIdParam = new SqlParameter { ParameterName = "PermanentCountryId", Value = permanentCountryId };

                if (permanentDistrictId == 0)
                    permanentDistrictId = -1;
                var permanentDistrictIdParam = new SqlParameter { ParameterName = "PermanentDistrictId", Value = permanentDistrictId };

                if (educationLevelId == 0)
                    educationLevelId = -1;
                var educationLevelIdParam = new SqlParameter { ParameterName = "EducationLevelId", Value = educationLevelId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                if (String.IsNullOrEmpty(employeeName))
                    employeeName = string.Empty;
                var employeeNameParam = new SqlParameter { ParameterName = "EmployeeName", Value = employeeName };

                if (String.IsNullOrEmpty(mobileNo))
                    mobileNo = string.Empty;
                var mobileNoParam = new SqlParameter { ParameterName = "MobileNo", Value = mobileNo };

                if (activeStatus == 0)
                    activeStatus = -1;
                var activeStatusParam = new SqlParameter { ParameterName = "ActiveStatus", Value = activeStatus };

                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };

                if (fromDate == null)
                    fromDate = new DateTime(1900, 01, 01);
                var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

                List<EmployeeAllInfoNewModel> list = Context.Database.SqlQuery<EmployeeAllInfoNewModel>("SPGetAllEmployeeInfoReportNew @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                                                        "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeGradeId, @EmployeeDesignationId, " +
                                                                                                        "@BloodGroupId, @GenderId, @ReligionId, @MaritalStateId, @JoiningDateBegin, @JoiningDateEnd, @ConfirmationDateBegin, " +
                                                                                                        "@ConfirmationDateEnd, @QuitDateBegin, @QuitDateEnd, @BirthDayMonth, @MariageAnniversaryDateBegin, @MariageAnniversaryDateEnd, " +
                                                                                                        "@PermanentCountryId, @PermanentDistrictId, @EducationLevelId, @EmployeeCardId, @EmployeeName, @MobileNo, @ActiveStatus, @UserName, " +
                                                                                                        "@FromDate ", companyIdParam, branchIdParam, branchUnitIdParam,
                    branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam, employeeTypeIdParam,
                    employeeGradeIdParam, employeeDesignationIdParam, bloodGroupIdParam, genderIdParam, religionIdParam, maritalStateIdParam,
                    joiningDateBeginParam, joiningDateEndParam, confirmationDateBeginParam, confirmationDateEndParam, quitDateBeginParam,
                    quitDateEndParam, birthDayMonthParam, mariageAnniversaryDateBeginParam, mariageAnniversaryDateEndParam,
                    permanentCountryIdParam, permanentDistrictIdParam, educationLevelIdParam, employeeCardIdParam, employeeNameParam,
                    mobileNoParam, activeStatusParam, userNameParam, fromDateParam).ToList();

                return list;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<AllEmployeeJobCardView> GetAllEmployeeJobCardInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int year, int month, DateTime? fromDate, DateTime? toDate, string attendanceStatus, int employeeActiveStatusId, int employeeCategoryId, bool otEnabled, bool extraOTEnabled, bool weekendOTEnabled)
        {
            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter { ParameterName = "Year", Value = year };

                var monthParam = new SqlParameter { ParameterName = "Month", Value = month };

                var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

                var toDateParam = new SqlParameter { ParameterName = "ToDate", Value = toDate };

                if (String.IsNullOrEmpty(attendanceStatus))
                    attendanceStatus = string.Empty;
                var attendanceStatusParam = new SqlParameter { ParameterName = "AttendanceStatus", Value = attendanceStatus };

                if (employeeActiveStatusId == 0)
                    employeeActiveStatusId = -1;
                var employeeActiveStatusIdParam = new SqlParameter { ParameterName = "EmployeeActiveStatusId", Value = employeeActiveStatusId };

                if (employeeCategoryId == 0)
                    employeeCategoryId = -1;
                var employeeCategoryIdParam = new SqlParameter { ParameterName = "EmployeeCategoryId", Value = employeeCategoryId };

                var oTHours = -1.00;
                if (otEnabled)
                    oTHours = 0.00;
                var oTHoursParam = new SqlParameter { ParameterName = "OTHours", Value = oTHours };

                var extraOTHours = -1.00;
                if (extraOTEnabled)
                    extraOTHours = 0.00;
                var extraOTHoursParam = new SqlParameter { ParameterName = "ExtraOTHours", Value = extraOTHours };

                var weekendOTHours = -1.00;
                if (weekendOTEnabled)
                    weekendOTHours = 0.00;
                var weekendOTHoursParam = new SqlParameter { ParameterName = "WeekendOTHours", Value = weekendOTHours };

                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = PortalContext.CurrentUser.Name };


                return Context.Database.SqlQuery<AllEmployeeJobCardView>("SPGetAllEmployeeJobCard @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                         "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeCardId, @Year, @Month, " +
                                                                         "@FromDate, @ToDate, @AttendanceStatus, @EmployeeActiveStatusId, @EmployeeCategoryId, @OTHours, @ExtraOTHours, @WeekendOTHours, @UserName", companyIdParam, branchIdParam, branchUnitIdParam,
                    branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam, employeeTypeIdParam, employeeCardIdParam,
                    yearParam, monthParam, fromDateParam, toDateParam, attendanceStatusParam, employeeActiveStatusIdParam, employeeCategoryIdParam, oTHoursParam, extraOTHoursParam, weekendOTHoursParam, userNameParam).ToList();



            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<JobCardInfoModel> GetJobCardModelInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, string employeeCardId, int Year, int Month, DateTime? fromDate,
            DateTime? toDate, string userName)
        {
            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter { ParameterName = "Year", Value = Year };

                var monthParam = new SqlParameter { ParameterName = "Month", Value = Month };

                var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

                var toDateParam = new SqlParameter { ParameterName = "ToDate", Value = toDate };


                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };



                List<JobCardInfoModel> jobCardInfo = Context.Database.SqlQuery<JobCardInfoModel>("SPGetEmployeeJobCardModel @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                                                 "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId," +
                                                                                                 "@EmployeeCardId, @Year, @Month, @FromDate, @ToDate, @UserName", companyIdParam, branchIdParam,
                    branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                    employeeTypeIdParam, employeeCardIdParam, yearParam, monthParam, fromDateParam, toDateParam, userNameParam).ToList();

                foreach (var t in jobCardInfo)
                {
                    VwPenaltyEmployee temp = Context.VwPenaltyEmployee.FirstOrDefault(p => p.EmployeeCardId == t.EmployeeCardId && p.PenaltyDate == t.Date && p.IsActive);
                    if (temp != null)
                    {
                        t.Status = "Absent";
                        t.InTime = "";
                        t.OutTime = "";
                        t.Delay = null;
                        t.OTHours = 0.00m;
                        t.Remarks = "";
                    }
                }

                return jobCardInfo;

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<AttendanceModel> GetEmployeeAttendanceModelInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId,
            int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
            int? branchUnitWorkShiftId, DateTime? fromDate, DateTime? toDate, string attendanceStatus,
            int? totalContinuousAbsentDays, bool otEnabled)
        {

            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                if (branchUnitWorkShiftId == 0)
                    branchUnitWorkShiftId = -1;
                var branchUnitWorkShiftIdParam = new SqlParameter { ParameterName = "BranchUnitWorkShiftId", Value = branchUnitWorkShiftId };


                if (fromDate == null)
                    fromDate = new DateTime(1900, 01, 01);
                var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };


                if (toDate == null)
                    toDate = new DateTime(1900, 01, 01);
                var toDateParam = new SqlParameter { ParameterName = "ToDate", Value = toDate };


                if (String.IsNullOrEmpty(attendanceStatus))
                    attendanceStatus = string.Empty;
                var attendanceStatusParam = new SqlParameter { ParameterName = "AttendanceStatus", Value = attendanceStatus };

                if (totalContinuousAbsentDays == 0)
                    totalContinuousAbsentDays = -1;
                var totalContinuousAbsentDaysParam = new SqlParameter { ParameterName = "TotalContinuousAbsentDays", Value = totalContinuousAbsentDays };

                var oTHours = -1.00;
                if (otEnabled)
                    oTHours = 0.00;
                var oTHoursParam = new SqlParameter { ParameterName = "OTHours", Value = oTHours };

                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = PortalContext.CurrentUser.Name };



                return Context.Database.SqlQuery<AttendanceModel>("HRMSPGetEmployeeAttendanceModelInfo @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                  "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeCardId, @BranchUnitWorkShiftId, @FromDate, " +
                                                                  "@Todate, @AttendanceStatus, @TotalContinuousAbsentDays, @OTHours, @UserName",
                    companyIdParam, branchIdParam, branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                    employeeTypeIdParam, employeeCardIdParam, branchUnitWorkShiftIdParam, fromDateParam, toDateParam,
                    attendanceStatusParam, totalContinuousAbsentDaysParam,
                    oTHoursParam, userNameParam).ToList();



            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

        }

        public List<EmployeeLeaveHistoryModel> GetEmployeeLeaveHistoryInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId,
            int? genderId, DateTime? joiningDateBegin, DateTime? joiningDateEnd,
            DateTime? quitDateBegin, DateTime? quitDateEnd, string employeeCardId, string employeeName,
            int? leaveTypeId, int? activeStatus, int? year, string userName, DateTime? fromDate)
        {


            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (employeeGradeId == 0)
                    employeeGradeId = -1;
                var employeeGradeIdParam = new SqlParameter { ParameterName = "EmployeeGradeId", Value = employeeGradeId };

                if (employeeDesignationId == 0)
                    employeeDesignationId = -1;
                var employeeDesignationIdParam = new SqlParameter { ParameterName = "EmployeeDesignationId", Value = employeeDesignationId };


                if (genderId == 0)
                    genderId = -1;
                var genderIdParam = new SqlParameter { ParameterName = "GenderId", Value = genderId };

                if (joiningDateBegin == null)
                    joiningDateBegin = new DateTime(1900, 01, 01);
                var joiningDateBeginParam = new SqlParameter { ParameterName = "JoiningDateBegin", Value = joiningDateBegin };

                if (joiningDateEnd == null)
                    joiningDateEnd = new DateTime(1900, 01, 01);
                var joiningDateEndParam = new SqlParameter { ParameterName = "JoiningDateEnd", Value = joiningDateEnd };

                if (quitDateBegin == null)
                    quitDateBegin = new DateTime(1900, 01, 01);
                var quitDateBeginParam = new SqlParameter { ParameterName = "QuitDateBegin", Value = quitDateBegin };

                if (quitDateEnd == null)
                    quitDateEnd = new DateTime(1900, 01, 01);
                var quitDateEndParam = new SqlParameter { ParameterName = "QuitDateEnd", Value = quitDateEnd };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                if (String.IsNullOrEmpty(employeeName))
                    employeeName = string.Empty;
                var employeeNameParam = new SqlParameter { ParameterName = "EmployeeName", Value = employeeName };

                if (leaveTypeId == 0)
                    leaveTypeId = -1;
                var leaveTypeIdParam = new SqlParameter { ParameterName = "LeaveTypeId", Value = leaveTypeId };

                if (activeStatus == 0)
                    activeStatus = -1;
                var activeStatusParam = new SqlParameter { ParameterName = "ActiveStatus", Value = activeStatus };

                if (year == 0)
                    year = -1;
                var yearParam = new SqlParameter { ParameterName = "Year", Value = year };

                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };

                if (fromDate == null)
                    fromDate = new DateTime(1900, 01, 01);
                var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

                return Context.Database.SqlQuery<EmployeeLeaveHistoryModel>("spHrmGetEmployeeLeaveHistoryReport @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                            "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeGradeId, @EmployeeDesignationId, " +
                                                                            "@GenderId, @JoiningDateBegin, @JoiningDateEnd, @QuitDateBegin, @QuitDateEnd, " +
                                                                            "@EmployeeCardId, @EmployeeName, @LeaveTypeId, @ActiveStatus,@Year, @FromDate,@UserName",
                    companyIdParam, branchIdParam, branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                    employeeTypeIdParam, employeeGradeIdParam, employeeDesignationIdParam, genderIdParam, joiningDateBeginParam,
                    joiningDateEndParam, quitDateBeginParam, quitDateEndParam, employeeCardIdParam, employeeNameParam,
                    leaveTypeIdParam, activeStatusParam, yearParam, fromDateParam, userNameParam).ToList();


            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<ManpowerSummaryModel> GetManpowerSummaryInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? employeeDesignationId,
            int? genderId, DateTime? joiningDateBegin, DateTime? joiningDateEnd,
            DateTime? confirmationDateBegin, DateTime? confirmationDateEnd,
            DateTime? quitDateBegin, DateTime? quitDateEnd, string userName)
        {


            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };


                if (employeeDesignationId == 0)
                    employeeDesignationId = -1;
                var employeeDesignationIdParam = new SqlParameter { ParameterName = "EmployeeDesignationId", Value = employeeDesignationId };


                if (genderId == 0)
                    genderId = -1;
                var genderIdParam = new SqlParameter { ParameterName = "GenderId", Value = genderId };

                if (joiningDateBegin == null)
                    joiningDateBegin = new DateTime(1900, 01, 01);
                var joiningDateBeginParam = new SqlParameter { ParameterName = "JoiningDateBegin", Value = joiningDateBegin };


                if (joiningDateEnd == null)
                    joiningDateEnd = new DateTime(1900, 01, 01);
                var joiningDateEndParam = new SqlParameter { ParameterName = "JoiningDateEnd", Value = joiningDateEnd };


                if (confirmationDateBegin == null)
                    confirmationDateBegin = new DateTime(1900, 01, 01);
                var confirmationDateBeginParam = new SqlParameter { ParameterName = "ConfirmationDateBegin", Value = confirmationDateBegin };


                if (confirmationDateEnd == null)
                    confirmationDateEnd = new DateTime(1900, 01, 01);
                var confirmationDateEndParam = new SqlParameter { ParameterName = "ConfirmationDateEnd", Value = confirmationDateEnd };

                if (quitDateBegin == null)
                    quitDateBegin = new DateTime(1900, 01, 01);
                var quitDateBeginParam = new SqlParameter { ParameterName = "QuitDateBegin", Value = quitDateBegin };


                if (quitDateEnd == null)
                    quitDateEnd = new DateTime(1900, 01, 01);
                var quitDateEndParam = new SqlParameter { ParameterName = "QuitDateEnd", Value = quitDateEnd };


                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };

                return Context.Database.SqlQuery<ManpowerSummaryModel>("spHrmGetManpowerReport @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                       "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeDesignationId, " +
                                                                       "@GenderId, @JoiningDateBegin, @JoiningDateEnd, @ConfirmationDateBegin, " +
                                                                       "@ConfirmationDateEnd, @QuitDateBegin, @QuitDateEnd, " +
                                                                       "@UserName", companyIdParam, branchIdParam, branchUnitIdParam,
                    branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam, employeeTypeIdParam,
                    employeeDesignationIdParam, genderIdParam, joiningDateBeginParam, joiningDateEndParam,
                    confirmationDateBeginParam, confirmationDateEndParam, quitDateBeginParam,
                    quitDateEndParam, userNameParam).ToList();


            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<ManpowerSummaryModel> GetManpowerSummarySkillInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? employeeDesignationId,
            int? genderId, DateTime? joiningDateBegin, DateTime? joiningDateEnd,
            DateTime? confirmationDateBegin, DateTime? confirmationDateEnd,
            DateTime? quitDateBegin, DateTime? quitDateEnd, string userName)
        {

            if (companyId == 0)
                companyId = -1;
            var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

            if (branchId == 0)
                branchId = -1;
            var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

            if (branchUnitId == 0)
                branchUnitId = -1;
            var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

            if (branchUnitDepartmentId == 0)
                branchUnitDepartmentId = -1;
            var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

            if (departmentSectionId == 0)
                departmentSectionId = -1;
            var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

            if (departmentLineId == 0)
                departmentLineId = -1;
            var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

            if (employeeTypeId == 0)
                employeeTypeId = -1;
            var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

            if (employeeDesignationId == 0)
                employeeDesignationId = -1;
            var employeeDesignationIdParam = new SqlParameter { ParameterName = "EmployeeDesignationId", Value = employeeDesignationId };

            if (genderId == 0)
                genderId = -1;
            var genderIdParam = new SqlParameter { ParameterName = "GenderId", Value = genderId };

            if (joiningDateBegin == null)
                joiningDateBegin = new DateTime(1900, 01, 01);
            var joiningDateBeginParam = new SqlParameter { ParameterName = "JoiningDateBegin", Value = joiningDateBegin };

            if (joiningDateEnd == null)
                joiningDateEnd = new DateTime(1900, 01, 01);
            var joiningDateEndParam = new SqlParameter { ParameterName = "JoiningDateEnd", Value = joiningDateEnd };

            if (confirmationDateBegin == null)
                confirmationDateBegin = new DateTime(1900, 01, 01);
            var confirmationDateBeginParam = new SqlParameter { ParameterName = "ConfirmationDateBegin", Value = confirmationDateBegin };

            if (confirmationDateEnd == null)
                confirmationDateEnd = new DateTime(1900, 01, 01);
            var confirmationDateEndParam = new SqlParameter { ParameterName = "ConfirmationDateEnd", Value = confirmationDateEnd };

            if (quitDateBegin == null)
                quitDateBegin = new DateTime(1900, 01, 01);
            var quitDateBeginParam = new SqlParameter { ParameterName = "QuitDateBegin", Value = quitDateBegin };

            if (quitDateEnd == null)
                quitDateEnd = new DateTime(1900, 01, 01);
            var quitDateEndParam = new SqlParameter { ParameterName = "QuitDateEnd", Value = quitDateEnd };

            var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };

            return Context.Database.SqlQuery<ManpowerSummaryModel>("spHrmGetManpowerSkillReport @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                   "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeDesignationId, " +
                                                                   "@GenderId, @JoiningDateBegin, @JoiningDateEnd, @ConfirmationDateBegin, " +
                                                                   "@ConfirmationDateEnd, @QuitDateBegin, @QuitDateEnd, " +
                                                                   "@UserName", companyIdParam, branchIdParam, branchUnitIdParam,
                branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam, employeeTypeIdParam,
                employeeDesignationIdParam, genderIdParam, joiningDateBeginParam, joiningDateEndParam,
                confirmationDateBeginParam, confirmationDateEndParam, quitDateBeginParam,
                quitDateEndParam, userNameParam).ToList();
        }

        public List<EmployeeLeaveDetailModel> GetEmployeeLeaveDetailInfo(int? companyId, int? branchId, int? branchUnitId,
            int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId,
            int? genderId, DateTime? joiningDateBegin, DateTime? joiningDateEnd,
            DateTime? quitDateBegin, DateTime? quitDateEnd, string employeeCardId, string employeeName,
            int? leaveTypeId, DateTime? consumedDateBegin, DateTime? consumedDateEnd, int? activeStatus,
            DateTime? fromDate, string userName)
        {


            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (employeeGradeId == 0)
                    employeeGradeId = -1;
                var employeeGradeIdParam = new SqlParameter { ParameterName = "EmployeeGradeId", Value = employeeGradeId };

                if (employeeDesignationId == 0)
                    employeeDesignationId = -1;
                var employeeDesignationIdParam = new SqlParameter { ParameterName = "EmployeeDesignationId", Value = employeeDesignationId };


                if (genderId == 0)
                    genderId = -1;
                var genderIdParam = new SqlParameter { ParameterName = "GenderId", Value = genderId };

                if (joiningDateBegin == null)
                    joiningDateBegin = new DateTime(1900, 01, 01);
                var joiningDateBeginParam = new SqlParameter { ParameterName = "JoiningDateBegin", Value = joiningDateBegin };

                if (joiningDateEnd == null)
                    joiningDateEnd = new DateTime(1900, 01, 01);
                var joiningDateEndParam = new SqlParameter { ParameterName = "JoiningDateEnd", Value = joiningDateEnd };

                if (quitDateBegin == null)
                    quitDateBegin = new DateTime(1900, 01, 01);
                var quitDateBeginParam = new SqlParameter { ParameterName = "QuitDateBegin", Value = quitDateBegin };

                if (quitDateEnd == null)
                    quitDateEnd = new DateTime(1900, 01, 01);
                var quitDateEndParam = new SqlParameter { ParameterName = "QuitDateEnd", Value = quitDateEnd };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                if (String.IsNullOrEmpty(employeeName))
                    employeeName = string.Empty;
                var employeeNameParam = new SqlParameter { ParameterName = "EmployeeName", Value = employeeName };

                if (leaveTypeId == 0)
                    leaveTypeId = -1;
                var leaveTypeIdParam = new SqlParameter { ParameterName = "LeaveTypeId", Value = leaveTypeId };

                if (consumedDateBegin == null)
                    consumedDateBegin = new DateTime(1900, 01, 01);
                var consumedDateBeginParam = new SqlParameter { ParameterName = "ConsumedDateBegin", Value = consumedDateBegin };

                if (consumedDateEnd == null)
                    consumedDateEnd = new DateTime(1900, 01, 01);
                var consumedDateEndParam = new SqlParameter { ParameterName = "ConsumedDateEnd", Value = consumedDateEnd };

                if (activeStatus == 0)
                    activeStatus = -1;
                var activeStatusParam = new SqlParameter { ParameterName = "ActiveStatus", Value = activeStatus };


                if (fromDate == null)
                    fromDate = new DateTime(1900, 01, 01);
                var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };


                return Context.Database.SqlQuery<EmployeeLeaveDetailModel>("spHrmGetEmployeeLeaveDetailReport @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                           "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeGradeId, @EmployeeDesignationId, " +
                                                                           "@GenderId, @JoiningDateBegin, @JoiningDateEnd, @QuitDateBegin, @QuitDateEnd, " +
                                                                           "@EmployeeCardId, @EmployeeName, @LeaveTypeId, @ConsumedDateBegin, @ConsumedDateEnd, @ActiveStatus, @FromDate, @UserName",
                    companyIdParam, branchIdParam, branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                    employeeTypeIdParam, employeeGradeIdParam, employeeDesignationIdParam, genderIdParam, joiningDateBeginParam,
                    joiningDateEndParam, quitDateBeginParam, quitDateEndParam, employeeCardIdParam, employeeNameParam,
                    leaveTypeIdParam, consumedDateBeginParam, consumedDateEndParam, activeStatusParam, fromDateParam, userNameParam).ToList();


            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<AttendanceSearchByTimeModel> GetAttendanceSearchByTime(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string userName, DateTime? fromDate, TimeSpan fromTime, TimeSpan toTime)
        {
            if (companyId == 0)
                companyId = -1;
            var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

            if (branchId == 0)
                branchId = -1;
            var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

            if (branchUnitId == 0)
                branchUnitId = -1;
            var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

            if (branchUnitDepartmentId == 0)
                branchUnitDepartmentId = -1;
            var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

            if (departmentSectionId == 0)
                departmentSectionId = -1;
            var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

            if (departmentLineId == 0)
                departmentLineId = -1;
            var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

            if (employeeTypeId == 0)
                employeeTypeId = -1;
            var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

            if (employeeGradeId == 0)
                employeeGradeId = -1;
            var employeeGradeIdParam = new SqlParameter { ParameterName = "EmployeeGradeId", Value = employeeGradeId };

            if (employeeDesignationId == 0)
                employeeDesignationId = -1;
            var employeeDesignationIdParam = new SqlParameter { ParameterName = "EmployeeDesignationId", Value = employeeDesignationId };

            var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };

            if (fromDate == null)
                fromDate = new DateTime(1900, 01, 01);
            var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

            var fromTimeParam = new SqlParameter { ParameterName = "FromTime", Value = fromTime };

            var toTimeParam = new SqlParameter { ParameterName = "ToTime", Value = toTime };

            return Context.Database.SqlQuery<AttendanceSearchByTimeModel>("SPGetOverTimeAttendanceByTime @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, @DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId, @EmployeeGradeId, @EmployeeDesignationId, @UserName, @FromDate, @FromTime, @ToTime"
                , companyIdParam, branchIdParam, branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam, employeeTypeIdParam, employeeGradeIdParam, employeeDesignationIdParam, userNameParam, fromDateParam, fromTimeParam, toTimeParam).ToList();
        }

        public List<SPCommCMInfo> GetCMInfo(DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate == null)
                fromDate = new DateTime(1900, 01, 01);
            var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

            if (toDate == null)
                toDate = new DateTime(1900, 01, 01);
            var toDateParam = new SqlParameter { ParameterName = "ToDate", Value = toDate };

            var userNameParam = new SqlParameter { ParameterName = "UserName", Value = PortalContext.CurrentUser.Name };

            return Context.Database.SqlQuery<SPCommCMInfo>("SPCMInfo @FromDate, @Todate, @UserName", fromDateParam, toDateParam, userNameParam).ToList();
        }

        public List<HrmDailyOTReport> GetDailyOtReport(DateTime? fromDate)
        {
            if (fromDate == null)
                fromDate = new DateTime(1900, 01, 01);
            var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

            return Context.Database.SqlQuery<HrmDailyOTReport>("SPHrmDailyOTReport @FromDate", fromDateParam).ToList();
        }

        public List<HrmOTSummaryReport> GetMonthlyOtSummaryReport(DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate == null)
                fromDate = new DateTime(1900, 01, 01);
            var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

            if (toDate == null)
                toDate = new DateTime(1900, 01, 01);
            var toDateParam = new SqlParameter { ParameterName = "ToDate", Value = toDate };

            return Context.Database.SqlQuery<HrmOTSummaryReport>("SPHrmOTSummaryReport @FromDate, @Todate", fromDateParam, toDateParam).ToList();
        }

        public List<SPGetEmployeesForBonus> GetEmployeeBonusInfo()
        {
            return Context.Database.SqlQuery<SPGetEmployeesForBonus>("SPGetEmployeesForBonus").ToList();
        }

        public List<SpHrmCuttingSectionAbsent> GetCuttingAbsentInfo(DateTime? fromDate)
        {
            if (fromDate == null)
                fromDate = new DateTime(1900, 01, 01);
            var fromDateParam = new SqlParameter { ParameterName = "EffectiveDate", Value = fromDate };

            return Context.Database.SqlQuery<SpHrmCuttingSectionAbsent>("SpHrmCuttingSectionAbsent @EffectiveDate", fromDateParam).ToList();
        }

        public DataTable GetMaternityInfo(string employeeCardId, DateTime? date)
        {

            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPGetMaternityReport"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@LeaveStartDate", SqlDbType.DateTime).Value = date;
                cmd.Parameters.Add("@EmployeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;
                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public string GetCompanyNameByCompanyId(string companyId)
        {
            var company = Context.Companies.FirstOrDefault(p => p.CompanyRefId == companyId && p.IsActive == true);

            if (company != null)
                return company.Name;
            else
                return "Not Found !";
        }

        public string CategoryNameById(int categoryId)
        {
            var category = Context.EmployeeCategories.FirstOrDefault(p => p.Id == categoryId && p.IsActive == true);

            if (category != null)
                return category.Title;
            else
                return "not found !";
        }

        public DataTable GetEmployeeEarnLeave(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate, int? activeStatus)
        {

            var employeeCardIdParm = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

            if (companyId == 0)
                companyId = -1;
            var companyIdParam = new SqlParameter { ParameterName = "CompanyID", Value = companyId };

            if (branchId == 0)
                branchId = -1;
            var branchIdParam = new SqlParameter { ParameterName = "BranchID", Value = branchId };

            if (branchUnitId == 0)
                branchUnitId = -1;
            var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitID", Value = branchUnitId };

            if (branchUnitDepartmentId == 0)
                branchUnitDepartmentId = -1;
            var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentID", Value = branchUnitDepartmentId };

            if (departmentSectionId == 0)
                departmentSectionId = -1;
            var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

            if (departmentLineId == 0)
                departmentLineId = -1;
            var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

            if (employeeTypeId == 0)
                employeeTypeId = -1;
            var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeID", Value = employeeTypeId };

            if (employeeGradeId == 0)
                employeeGradeId = -1;
            var employeeGradeIdParam = new SqlParameter { ParameterName = "EmployeeGradeID", Value = employeeGradeId };

            if (employeeDesignationId == 0)
                employeeDesignationId = -1;
            var employeeDesignationIdParam = new SqlParameter { ParameterName = "EmployeeDesignationID", Value = employeeDesignationId };

            var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };

            if (fromDate == null)
                fromDate = new DateTime(1900, 01, 01);
            var fromDateParam = new SqlParameter { ParameterName = "fromDate", Value = fromDate };

            if (toDate == null)
                toDate = new DateTime(1900, 01, 01);
            var toDateParam = new SqlParameter { ParameterName = "toDate", Value = toDate };

            if (activeStatus == 0)
                activeStatus = -1;
            var activeStatusParam = new SqlParameter { ParameterName = "ActiveStatus", Value = activeStatus };

            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPEarnLeaveSheet"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@toDate", SqlDbType.DateTime).Value = toDate;
                cmd.Parameters.Add("@EmployeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@BranchUnitID", SqlDbType.Int).Value = branchUnitId;
                cmd.Parameters.Add("@BranchUnitDepartmentID", SqlDbType.Int).Value = branchUnitDepartmentId;
                cmd.Parameters.Add("@DepartmentSectionId", SqlDbType.Int).Value = departmentSectionId;
                cmd.Parameters.Add("@DepartmentLineId", SqlDbType.Int).Value = departmentLineId;
                cmd.Parameters.Add("@EmployeeTypeID", SqlDbType.Int).Value = employeeTypeId;
                cmd.Parameters.Add("@EmployeeGradeID", SqlDbType.Int).Value = employeeGradeId;
                cmd.Parameters.Add("@EmployeeDesignationID", SqlDbType.Int).Value = employeeDesignationId;
                cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 100).Value = userName;
                cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = activeStatus;

                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {

                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }


        }

        public DataTable GetEmployeeMonthwiseAttendence(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {

            var employeeCardIdParm = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

            if (companyId == 0)
                companyId = -1;
            var companyIdParam = new SqlParameter { ParameterName = "CompanyID", Value = companyId };

            if (branchId == 0)
                branchId = -1;
            var branchIdParam = new SqlParameter { ParameterName = "BranchID", Value = branchId };

            if (branchUnitId == 0)
                branchUnitId = -1;
            var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitID", Value = branchUnitId };

            if (branchUnitDepartmentId == 0)
                branchUnitDepartmentId = -1;
            var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentID", Value = branchUnitDepartmentId };

            if (departmentSectionId == 0)
                departmentSectionId = -1;
            var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

            if (departmentLineId == 0)
                departmentLineId = -1;
            var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

            if (employeeTypeId == 0)
                employeeTypeId = -1;
            var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeID", Value = employeeTypeId };

            if (employeeGradeId == 0)
                employeeGradeId = -1;
            var employeeGradeIdParam = new SqlParameter { ParameterName = "EmployeeGradeID", Value = employeeGradeId };

            if (employeeDesignationId == 0)
                employeeDesignationId = -1;
            var employeeDesignationIdParam = new SqlParameter { ParameterName = "EmployeeDesignationID", Value = employeeDesignationId };

            var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };

            if (fromDate == null)
                fromDate = new DateTime(1900, 01, 01);
            var fromDateParam = new SqlParameter { ParameterName = "fromDate", Value = fromDate };

            if (toDate == null)
                toDate = new DateTime(1900, 01, 01);
            var toDateParam = new SqlParameter { ParameterName = "toDate", Value = toDate };
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPEarnLeaveSheetQuitAttendence"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@toDate", SqlDbType.DateTime).Value = toDate;
                cmd.Parameters.Add("@EmployeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@BranchUnitID", SqlDbType.Int).Value = branchUnitId;
                cmd.Parameters.Add("@BranchUnitDepartmentID", SqlDbType.Int).Value = branchUnitDepartmentId;
                cmd.Parameters.Add("@DepartmentSectionId", SqlDbType.Int).Value = departmentSectionId;
                cmd.Parameters.Add("@DepartmentLineId", SqlDbType.Int).Value = departmentLineId;
                cmd.Parameters.Add("@EmployeeTypeID", SqlDbType.Int).Value = employeeTypeId;
                cmd.Parameters.Add("@EmployeeGradeID", SqlDbType.Int).Value = employeeGradeId;
                cmd.Parameters.Add("@EmployeeDesignationID", SqlDbType.Int).Value = employeeDesignationId;
                cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 100).Value = userName;
                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {

                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }

        }

        public DataTable GetEmployeeDailyAbsent(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {

            var employeeCardIdParm = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

            if (companyId == 0)
                companyId = null;
            var companyIdParam = new SqlParameter { ParameterName = "CompanyID", Value = companyId };

            if (branchId == 0)
                branchId = null;
            var branchIdParam = new SqlParameter { ParameterName = "BranchID", Value = branchId };

            if (branchUnitId == 0)
                branchUnitId = null;
            var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitID", Value = branchUnitId };

            if (branchUnitDepartmentId == 0)
                branchUnitDepartmentId = null;
            var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentID", Value = branchUnitDepartmentId };

            if (departmentSectionId == 0)
                departmentSectionId = null;
            var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

            if (departmentLineId == 0)
                departmentLineId = null;
            var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

            if (employeeTypeId == 0)
                employeeTypeId = null;
            var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeID", Value = employeeTypeId };

            if (employeeGradeId == 0)
                employeeGradeId = null;
            var employeeGradeIdParam = new SqlParameter { ParameterName = "EmployeeGradeID", Value = employeeGradeId };

            if (employeeDesignationId == 0)
                employeeDesignationId = null;
            var employeeDesignationIdParam = new SqlParameter { ParameterName = "EmployeeDesignationID", Value = employeeDesignationId };

            var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };

            if (fromDate == null)
                fromDate = new DateTime(1900, 01, 01);
            var fromDateParam = new SqlParameter { ParameterName = "fromDate", Value = fromDate };

            if (toDate == null)
                toDate = new DateTime(1900, 01, 01);
            var toDateParam = new SqlParameter { ParameterName = "toDate", Value = toDate };
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpDailyAbsentReport"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EffectiveDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@EmployeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@BranchUnitID", SqlDbType.Int).Value = branchUnitId;
                cmd.Parameters.Add("@BranchUnitDepartmentID", SqlDbType.Int).Value = branchUnitDepartmentId;
                cmd.Parameters.Add("@SectionId", SqlDbType.Int).Value = departmentSectionId;
                cmd.Parameters.Add("@LineId", SqlDbType.Int).Value = departmentLineId;
                cmd.Parameters.Add("@EmployeeTypeID", SqlDbType.Int).Value = employeeTypeId;
                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {

                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }


        }

        public DataTable GetEmployeeDailyAttendance(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {

            var employeeCardIdParm = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

            if (companyId == 0)
                companyId = null;
            var companyIdParam = new SqlParameter { ParameterName = "CompanyID", Value = companyId };

            if (branchId == 0)
                branchId = null;
            var branchIdParam = new SqlParameter { ParameterName = "BranchID", Value = branchId };

            if (branchUnitId == 0)
                branchUnitId = null;
            var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitID", Value = branchUnitId };

            if (branchUnitDepartmentId == 0)
                branchUnitDepartmentId = null;
            var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentID", Value = branchUnitDepartmentId };

            if (departmentSectionId == 0)
                departmentSectionId = null;
            var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

            if (departmentLineId == 0)
                departmentLineId = null;
            var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

            if (employeeTypeId == 0)
                employeeTypeId = null;
            var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeID", Value = employeeTypeId };

            if (employeeGradeId == 0)
                employeeGradeId = null;
            var employeeGradeIdParam = new SqlParameter { ParameterName = "EmployeeGradeID", Value = employeeGradeId };

            if (employeeDesignationId == 0)
                employeeDesignationId = null;
            var employeeDesignationIdParam = new SqlParameter { ParameterName = "EmployeeDesignationID", Value = employeeDesignationId };

            var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };

            if (fromDate == null)
                fromDate = new DateTime(1900, 01, 01);
            var fromDateParam = new SqlParameter { ParameterName = "fromDate", Value = fromDate };

            if (toDate == null)
                toDate = new DateTime(1900, 01, 01);
            var toDateParam = new SqlParameter { ParameterName = "toDate", Value = toDate };

            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpDailyAttendanceReport"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EffectiveDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@EmployeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@BranchUnitID", SqlDbType.Int).Value = branchUnitId;
                cmd.Parameters.Add("@BranchUnitDepartmentID", SqlDbType.Int).Value = branchUnitDepartmentId;
                cmd.Parameters.Add("@SectionId", SqlDbType.Int).Value = departmentSectionId;
                cmd.Parameters.Add("@LineId", SqlDbType.Int).Value = departmentLineId;
                cmd.Parameters.Add("@EmployeeTypeID", SqlDbType.Int).Value = employeeTypeId;
                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {

                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetSkillMatrixPoint(string employeeCardId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPReportSkillMatrixPoint"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@employeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;
                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetSkillMatrixPointSecondPart(string employeeCardId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPReportSkillMatrixPointSecondPart"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@employeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;
                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetSkillMatrixAll(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId)
        {

            if (companyId == 0)
                companyId = null;
            var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

            if (branchId == 0)
                branchId = null;
            var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

            if (branchUnitId == 0)
                branchUnitId = null;
            var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

            if (branchUnitDepartmentId == 0)
                branchUnitDepartmentId = null;
            var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

            if (departmentSectionId == 0)
                departmentSectionId = null;
            var departmentSectionIdParam = new SqlParameter { ParameterName = "SectionId", Value = departmentSectionId };

            if (departmentLineId == 0)
                departmentLineId = null;
            var departmentLineIdParam = new SqlParameter { ParameterName = "LineId", Value = departmentLineId };

            if (employeeTypeId == 0)
                employeeTypeId = null;
            var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };


            var employeeCardIdParm = new SqlParameter { ParameterName = "employeeCardId", Value = employeeCardId };


            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPReportSkillMatrix"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchId", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@BranchUnitId", SqlDbType.Int).Value = branchUnitId;
                cmd.Parameters.Add("@BranchUnitDepartmentId", SqlDbType.Int).Value = branchUnitDepartmentId;
                cmd.Parameters.Add("@SectionId", SqlDbType.Int).Value = departmentSectionId;
                cmd.Parameters.Add("@LineId", SqlDbType.Int).Value = departmentLineId;
                cmd.Parameters.Add("@EmployeeTypeId", SqlDbType.Int).Value = employeeTypeId;
                cmd.Parameters.Add("@employeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;

                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetEmployeeDailyAttendanceButPreviousDayAbsent(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {

            var employeeCardIdParm = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

            if (companyId == 0)
                companyId = null;
            var companyIdParam = new SqlParameter { ParameterName = "CompanyID", Value = companyId };

            if (branchId == 0)
                branchId = null;
            var branchIdParam = new SqlParameter { ParameterName = "BranchID", Value = branchId };

            if (branchUnitId == 0)
                branchUnitId = null;
            var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitID", Value = branchUnitId };

            if (branchUnitDepartmentId == 0)
                branchUnitDepartmentId = null;
            var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentID", Value = branchUnitDepartmentId };

            if (departmentSectionId == 0)
                departmentSectionId = null;
            var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

            if (departmentLineId == 0)
                departmentLineId = null;
            var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

            if (employeeTypeId == 0)
                employeeTypeId = null;
            var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeID", Value = employeeTypeId };

            if (employeeGradeId == 0)
                employeeGradeId = null;
            var employeeGradeIdParam = new SqlParameter { ParameterName = "EmployeeGradeID", Value = employeeGradeId };

            if (employeeDesignationId == 0)
                employeeDesignationId = null;
            var employeeDesignationIdParam = new SqlParameter { ParameterName = "EmployeeDesignationID", Value = employeeDesignationId };

            var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };

            if (fromDate == null)
                fromDate = new DateTime(1900, 01, 01);
            var fromDateParam = new SqlParameter { ParameterName = "fromDate", Value = fromDate };

            if (toDate == null)
                toDate = new DateTime(1900, 01, 01);
            var toDateParam = new SqlParameter { ParameterName = "toDate", Value = toDate };

            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpPreviousDayAbsentTodayPresentReport"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EffectiveDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@EmployeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@BranchUnitID", SqlDbType.Int).Value = branchUnitId;
                cmd.Parameters.Add("@BranchUnitDepartmentID", SqlDbType.Int).Value = branchUnitDepartmentId;
                cmd.Parameters.Add("@SectionId", SqlDbType.Int).Value = departmentSectionId;
                cmd.Parameters.Add("@LineId", SqlDbType.Int).Value = departmentLineId;
                cmd.Parameters.Add("@EmployeeTypeID", SqlDbType.Int).Value = employeeTypeId;
                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {

                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetEmployeeDailyAttendanceByDesignation(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {

            var employeeCardIdParm = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

            if (companyId == 0)
                companyId = null;
            var companyIdParam = new SqlParameter { ParameterName = "CompanyID", Value = companyId };

            if (branchId == 0)
                branchId = null;
            var branchIdParam = new SqlParameter { ParameterName = "BranchID", Value = branchId };

            if (branchUnitId == 0)
                branchUnitId = null;
            var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitID", Value = branchUnitId };

            if (branchUnitDepartmentId == 0)
                branchUnitDepartmentId = null;
            var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentID", Value = branchUnitDepartmentId };

            if (departmentSectionId == 0)
                departmentSectionId = null;
            var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

            if (departmentLineId == 0)
                departmentLineId = null;
            var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

            if (employeeTypeId == 0)
                employeeTypeId = null;
            var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeID", Value = employeeTypeId };

            if (employeeGradeId == 0)
                employeeGradeId = null;
            var employeeGradeIdParam = new SqlParameter { ParameterName = "EmployeeGradeID", Value = employeeGradeId };

            if (employeeDesignationId == 0)
                employeeDesignationId = null;
            var employeeDesignationIdParam = new SqlParameter { ParameterName = "EmployeeDesignationID", Value = employeeDesignationId };

            var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };

            if (fromDate == null)
                fromDate = new DateTime(1900, 01, 01);
            var fromDateParam = new SqlParameter { ParameterName = "fromDate", Value = fromDate };

            if (toDate == null)
                toDate = new DateTime(1900, 01, 01);
            var toDateParam = new SqlParameter { ParameterName = "toDate", Value = toDate };

            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpDailyAttendanceByDesignationReport"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EffectiveDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@EmployeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@BranchUnitID", SqlDbType.Int).Value = branchUnitId;
                cmd.Parameters.Add("@BranchUnitDepartmentID", SqlDbType.Int).Value = branchUnitDepartmentId;
                cmd.Parameters.Add("@SectionId", SqlDbType.Int).Value = departmentSectionId;
                cmd.Parameters.Add("@LineId", SqlDbType.Int).Value = departmentLineId;
                cmd.Parameters.Add("@EmployeeTypeID", SqlDbType.Int).Value = employeeTypeId;
                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {

                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetEmployeeWorkingHoursDetails(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {

            var employeeCardIdParm = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

            if (companyId == 0)
                companyId = null;
            var companyIdParam = new SqlParameter { ParameterName = "CompanyID", Value = companyId };

            if (branchId == 0)
                branchId = null;
            var branchIdParam = new SqlParameter { ParameterName = "BranchID", Value = branchId };

            if (branchUnitId == 0)
                branchUnitId = null;
            var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitID", Value = branchUnitId };

            if (branchUnitDepartmentId == 0)
                branchUnitDepartmentId = null;
            var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentID", Value = branchUnitDepartmentId };

            if (departmentSectionId == 0)
                departmentSectionId = null;
            var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

            if (departmentLineId == 0)
                departmentLineId = null;
            var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

            if (employeeTypeId == 0)
                employeeTypeId = null;
            var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeID", Value = employeeTypeId };



            var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };

            if (fromDate == null)
                fromDate = new DateTime(1900, 01, 01);
            var fromDateParam = new SqlParameter { ParameterName = "fromDate", Value = fromDate };

            if (toDate == null)
                toDate = new DateTime(1900, 01, 01);
            var toDateParam = new SqlParameter { ParameterName = "toDate", Value = toDate };

            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpWeeklyEmployeeWorkingHoursDetail"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@toDate", SqlDbType.DateTime).Value = toDate;
                cmd.Parameters.Add("@EmployeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@BranchUnitID", SqlDbType.Int).Value = branchUnitId;
                cmd.Parameters.Add("@BranchUnitDepartmentID", SqlDbType.Int).Value = branchUnitDepartmentId;
                cmd.Parameters.Add("@SectionId", SqlDbType.Int).Value = departmentSectionId;
                cmd.Parameters.Add("@LineId", SqlDbType.Int).Value = departmentLineId;
                cmd.Parameters.Add("@EmployeeTypeID", SqlDbType.Int).Value = employeeTypeId;
                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {

                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public JobCardEditView GetJobCardEditInfo(string jobCardName, string employeeCardId, DateTime? fromDate, DateTime? toDate)
        {
            if (String.IsNullOrEmpty(jobCardName))
                jobCardName = string.Empty;
            var jobCardNameParam = new SqlParameter { ParameterName = "JobCardName", Value = jobCardName };

            if (String.IsNullOrEmpty(employeeCardId))
                employeeCardId = string.Empty;
            var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

            if (fromDate == null)
                fromDate = new DateTime(1900, 01, 01);
            var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

            if (toDate == null)
                toDate = new DateTime(1900, 01, 01);
            var toDateParam = new SqlParameter { ParameterName = "ToDate", Value = toDate };

            JobCardEditView jobcard = Context.Database.SqlQuery<JobCardEditView>("SPJobCardEdit @jobCardName, @employeeCardId, @fromDate, @toDate", jobCardNameParam, employeeCardIdParam, fromDateParam, toDateParam).SingleOrDefault();


            if (String.IsNullOrEmpty(jobCardName))
                jobCardName = string.Empty;
            var jobCardName2Param = new SqlParameter { ParameterName = "JobCardName", Value = jobCardName };

            if (String.IsNullOrEmpty(employeeCardId))
                employeeCardId = string.Empty;
            var employeeCardId2Param = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

            if (fromDate == null)
                fromDate = new DateTime(1900, 01, 01);
            var fromDate2Param = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

            if (toDate == null)
                toDate = new DateTime(1900, 01, 01);
            var toDate2Param = new SqlParameter { ParameterName = "ToDate", Value = toDate };

            if (jobcard != null)
                jobcard.Inout = Context.Database.SqlQuery<EmployeeInOutEditView>("SPEmployeeInOutEdit @JobCardName, @EmployeeCardId, @FromDate, @ToDate", jobCardName2Param, employeeCardId2Param, fromDate2Param, toDate2Param).ToList(); ;

            return jobcard;
        }

        public int EditJobCard(JobCardEditView model)
        {

            if (String.IsNullOrEmpty(model.JobCardName))
                model.JobCardName = string.Empty;
            var jobCardNameParam = new SqlParameter { ParameterName = "JobCardName", Value = model.JobCardName };

            if (String.IsNullOrEmpty(model.EmployeeCardId))
                model.EmployeeCardId = string.Empty;
            var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = model.EmployeeCardId };

            if (model.FromDate == null)
                model.FromDate = new DateTime(1900, 01, 01);
            var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = model.FromDate };

            if (model.ToDate == null)
                model.ToDate = new DateTime(1900, 01, 01);
            var toDateParam = new SqlParameter { ParameterName = "ToDate", Value = model.ToDate };

            var presentDaysParam = new SqlParameter { ParameterName = "PresentDays", Value = model.PresentDays };

            var payDaysParam = new SqlParameter { ParameterName = "PayDays", Value = model.PayDays };

            var lateDaysParam = new SqlParameter { ParameterName = "LateDays", Value = model.LateDays };

            var totalOTHoursParam = new SqlParameter { ParameterName = "TotalOTHours", Value = model.TotalOTHours };

            var osdDaysParam = new SqlParameter { ParameterName = "OSDDays", Value = model.OSDDays };

            var totalExtraOTHoursParam = new SqlParameter { ParameterName = "TotalExtraOTHours", Value = model.TotalExtraOTHours };

            var absentDaysParam = new SqlParameter { ParameterName = "AbsentDays", Value = model.AbsentDays };

            if (model.TotalWeekendOTHours == null)
                model.TotalWeekendOTHours = 0;
            var totalWeekendOTHoursParam = new SqlParameter { ParameterName = "TotalWeekendOTHours", Value = model.TotalWeekendOTHours };

            var leaveDaysParam = new SqlParameter { ParameterName = "LeaveDays", Value = model.LeaveDays };

            if (model.TotalHolidayOTHours == null)
                model.TotalHolidayOTHours = 0;
            var totalHolidayOTHoursParam = new SqlParameter { ParameterName = "TotalHolidayOTHours", Value = model.TotalHolidayOTHours };

            var lwpDaysParam = new SqlParameter { ParameterName = "LWPDays", Value = model.LWPDays };

            if (model.TotalPenaltyOTHours == null)
                model.TotalPenaltyOTHours = 0;
            var totalPenaltyOTHoursParam = new SqlParameter { ParameterName = "TotalPenaltyOTHours", Value = model.TotalPenaltyOTHours };

            var weekendDaysParam = new SqlParameter { ParameterName = "WeekendDays", Value = model.WeekendDays };

            if (model.TotalPenaltyAttendanceDays == null)
                model.TotalPenaltyAttendanceDays = 0;
            var totalPenaltyAttendanceDaysParam = new SqlParameter { ParameterName = "TotalPenaltyAttendanceDays", Value = model.TotalPenaltyAttendanceDays };

            var holidaysParam = new SqlParameter { ParameterName = "Holidays", Value = model.Holidays };

            if (model.TotalPenaltyLeaveDays == null)
                model.TotalPenaltyLeaveDays = 0;
            var totalPenaltyLeaveDaysParam = new SqlParameter { ParameterName = "TotalPenaltyLeaveDays", Value = model.TotalPenaltyLeaveDays };

            var totalDaysParam = new SqlParameter { ParameterName = "TotalDays", Value = model.TotalDays };

            if (model.TotalPenaltyFinancialAmount == null)
                model.TotalPenaltyFinancialAmount = 0;
            var totalPenaltyFinancialAmountParam = new SqlParameter { ParameterName = "TotalPenaltyFinancialAmount", Value = model.TotalPenaltyFinancialAmount };

            var workingDaysParam = new SqlParameter { ParameterName = "WorkingDays", Value = model.WorkingDays };


            JobCardEditView jobcard = Context.Database.SqlQuery<JobCardEditView>("SPEditJobCard @JobCardName, @EmployeeCardId, @fromDate, @toDate, @PresentDays, @PayDays, @LateDays, @TotalOTHours, @OSDDays, @TotalExtraOTHours, @AbsentDays, @TotalWeekendOTHours, @LeaveDays, @TotalHolidayOTHours, @LWPDays, @TotalPenaltyOTHours, @WeekendDays, @TotalPenaltyAttendanceDays, @Holidays, @TotalPenaltyLeaveDays, @TotalDays, @TotalPenaltyFinancialAmount, @WorkingDays", jobCardNameParam, employeeCardIdParam, fromDateParam, toDateParam, presentDaysParam, payDaysParam, lateDaysParam, totalOTHoursParam, osdDaysParam, totalExtraOTHoursParam, absentDaysParam, totalWeekendOTHoursParam, leaveDaysParam, totalHolidayOTHoursParam, lwpDaysParam, totalPenaltyOTHoursParam, weekendDaysParam, totalPenaltyAttendanceDaysParam, holidaysParam, totalPenaltyLeaveDaysParam, totalDaysParam, totalPenaltyFinancialAmountParam, workingDaysParam).SingleOrDefault();

            return 1;
        }

        public EmployeeInOutEditView GetEmployeeInOutInfo(string inOutName, string employeeCardId, DateTime? date)
        {
            if (String.IsNullOrEmpty(inOutName))
                inOutName = string.Empty;
            var inOutNameParam = new SqlParameter { ParameterName = "InOutName", Value = inOutName };

            if (String.IsNullOrEmpty(employeeCardId))
                employeeCardId = string.Empty;
            var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

            if (date < DateTime.Now.AddDays(-1000))
                date = new DateTime(1900, 01, 01);
            var dateParam = new SqlParameter { ParameterName = "Date", Value = date };

            EmployeeInOutEditView jobcard = Context.Database.SqlQuery<EmployeeInOutEditView>("SPEmployeeInOutEdit @inOutName, @employeeCardId, @date", inOutNameParam, employeeCardIdParam, dateParam).SingleOrDefault();
            return jobcard;
        }

        public int EditEmployeeInOut(EmployeeInOutEditView model)
        {
            if (String.IsNullOrEmpty(model.InOutName))
                model.InOutName = string.Empty;
            var inOutNameParam = new SqlParameter { ParameterName = "InOutName", Value = model.InOutName };

            if (String.IsNullOrEmpty(model.EmployeeCardId))
                model.EmployeeCardId = string.Empty;
            var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = model.EmployeeCardId };

            var transactionDateParam = new SqlParameter { ParameterName = "TransactionDate", Value = model.TransactionDate };

            if (String.IsNullOrEmpty(model.Status))
                model.Status = string.Empty;
            var statusParam = new SqlParameter { ParameterName = "Status", Value = model.Status };

            if (model.InTime == null)
                model.InTime = TimeSpan.Parse("00:00:00");
            var inTimeParam = new SqlParameter { ParameterName = "InTime", Value = model.InTime };

            if (model.OutTime == null)
                model.OutTime = TimeSpan.Parse("00:00:00");
            var outTimeParam = new SqlParameter { ParameterName = "OutTime", Value = model.OutTime };

            if (model.LateInMinutes == null)
                model.LateInMinutes = 0;
            var lateInMinutesParam = new SqlParameter { ParameterName = "LateInMinutes", Value = model.LateInMinutes };

            if (model.OTHours == null)
                model.OTHours = 0;
            var oTHoursParam = new SqlParameter { ParameterName = "OTHours", Value = model.OTHours };

            if (model.ExtraOTHours == null)
                model.ExtraOTHours = 0;
            var extraOTHoursParam = new SqlParameter { ParameterName = "ExtraOTHours", Value = model.ExtraOTHours };

            if (model.WeekendOTHours == null)
                model.WeekendOTHours = 0;
            var weekendOTHoursParam = new SqlParameter { ParameterName = "WeekendOTHours", Value = model.WeekendOTHours };

            if (model.HolidayOTHours == null)
                model.HolidayOTHours = 0;
            var holidayOTHoursParam = new SqlParameter { ParameterName = "HolidayOTHours", Value = model.HolidayOTHours };

            if (String.IsNullOrEmpty(model.Remarks))
                model.Remarks = string.Empty;
            var remarksParam = new SqlParameter { ParameterName = "Remarks", Value = model.Remarks };

            EmployeeInOutEditView inOutEdit = Context.Database.SqlQuery<EmployeeInOutEditView>("SPEditEmployeeInOut @InOutName, @EmployeeCardId, @TransactionDate, @Status, @InTime, @OutTime, @LateInMinutes, @OTHours, @ExtraOTHours, @WeekendOTHours, @HolidayOTHours, @Remarks", inOutNameParam, employeeCardIdParam, transactionDateParam, statusParam, inTimeParam, outTimeParam, lateInMinutesParam, oTHoursParam, extraOTHoursParam, weekendOTHoursParam, holidayOTHoursParam, remarksParam).SingleOrDefault();

            return 1;
        }

        public DataTable GetManpowerApprovedEmployee(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, DateTime? effectiveDate, DateTime? fromDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPDailyAttendanceCombined"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchId", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@BranchUnitId", SqlDbType.Int).Value = branchUnitId;
                cmd.Parameters.Add("@BranchUnitDepartmentId", SqlDbType.Int).Value = branchUnitDepartmentId;
                cmd.Parameters.Add("@SectionId", SqlDbType.Int).Value = departmentSectionId;
                cmd.Parameters.Add("@LineId", SqlDbType.Int).Value = departmentLineId;
                cmd.Parameters.Add("@EmployeeTypeId", SqlDbType.Int).Value = employeeTypeId;
                cmd.Parameters.Add("@employeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;
                cmd.Parameters.Add("@EffectiveDate", SqlDbType.DateTime).Value = effectiveDate;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable EmployeeNewJoinAndQuitSummary(int? companyId, int? branchId, int? fromYear, int? toYear, int? fromMonth, int? toMonth, DateTime? fromDate, DateTime? toDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPEmployeeNewJoinAndQuitSummary"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchId", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@FromYear", SqlDbType.Int).Value = fromYear;
                cmd.Parameters.Add("@ToYear", SqlDbType.Int).Value = toYear;

                cmd.Parameters.Add("@FromMonth", SqlDbType.Int).Value = fromMonth;
                cmd.Parameters.Add("@ToMonth", SqlDbType.Int).Value = toMonth;

                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = "superadmin";

                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetAdvanceOTAmount(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, DateTime? date)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPDailyAdvanceOTAmountReport"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchId", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@BranchUnitId", SqlDbType.Int).Value = branchUnitId;
                cmd.Parameters.Add("@BranchUnitDepartmentId", SqlDbType.Int).Value = branchUnitDepartmentId;
                cmd.Parameters.Add("@SectionId", SqlDbType.Int).Value = departmentSectionId;
                cmd.Parameters.Add("@LineId", SqlDbType.Int).Value = departmentLineId;
                cmd.Parameters.Add("@EffectiveDate", SqlDbType.DateTime).Value = date;

                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetEmployeeDailyAbsentRootCause(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpDailyAbsentRootCauseReport"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@BranchUnitID", SqlDbType.Int).Value = branchUnitId;
                cmd.Parameters.Add("@BranchUnitDepartmentID", SqlDbType.Int).Value = branchUnitDepartmentId;
                cmd.Parameters.Add("@SectionId", SqlDbType.Int).Value = departmentSectionId;
                cmd.Parameters.Add("@LineId", SqlDbType.Int).Value = departmentLineId;
                cmd.Parameters.Add("@EmployeeTypeID", SqlDbType.Int).Value = employeeTypeId;
                cmd.Parameters.Add("@EffectiveDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@EmployeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;
                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetTiffinBill(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, string employeeCardId, string userName, DateTime? date, bool all, bool management, bool middleManagement, bool teamMemberA, bool teamMemberB)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPGetTiffinBillAmount"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchId", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@BranchUnitId", SqlDbType.Int).Value = branchUnitId;
                cmd.Parameters.Add("@BranchUnitDepartmentId", SqlDbType.Int).Value = branchUnitDepartmentId;
                cmd.Parameters.Add("@DepartmentSectionId", SqlDbType.Int).Value = departmentSectionId;
                cmd.Parameters.Add("@DepartmentLineId", SqlDbType.Int).Value = departmentLineId;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = date;
                cmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = userName;

                cmd.Parameters.Add("@All", SqlDbType.Bit).Value = all;
                cmd.Parameters.Add("@Management", SqlDbType.Bit).Value = management;
                cmd.Parameters.Add("@MiddleManagement", SqlDbType.Bit).Value = middleManagement;
                cmd.Parameters.Add("@TeamMemberA", SqlDbType.Bit).Value = teamMemberA;
                cmd.Parameters.Add("@TeamMemberB", SqlDbType.Bit).Value = teamMemberB;

                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetTiffinBillDyeing(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, string employeeCardId, string userName, DateTime? date, bool all, bool management, bool middleManagement, bool teamMemberA, bool teamMemberB)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPGetTiffinBillAmountDyeing"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchId", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@BranchUnitId", SqlDbType.Int).Value = branchUnitId;
                cmd.Parameters.Add("@BranchUnitDepartmentId", SqlDbType.Int).Value = branchUnitDepartmentId;
                cmd.Parameters.Add("@DepartmentSectionId", SqlDbType.Int).Value = departmentSectionId;
                cmd.Parameters.Add("@DepartmentLineId", SqlDbType.Int).Value = departmentLineId;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = date;
                cmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = userName;

                cmd.Parameters.Add("@All", SqlDbType.Bit).Value = all;
                cmd.Parameters.Add("@Management", SqlDbType.Bit).Value = management;
                cmd.Parameters.Add("@MiddleManagement", SqlDbType.Bit).Value = middleManagement;
                cmd.Parameters.Add("@TeamMemberA", SqlDbType.Bit).Value = teamMemberA;
                cmd.Parameters.Add("@TeamMemberB", SqlDbType.Bit).Value = teamMemberB;

                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetJobCardSummary(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPGetJobCardSummary"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchId", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@BranchUnitId", SqlDbType.Int).Value = branchUnitId;
                cmd.Parameters.Add("@BranchUnitDepartmentId", SqlDbType.Int).Value = branchUnitDepartmentId;
                cmd.Parameters.Add("@SectionId", SqlDbType.Int).Value = departmentSectionId;
                cmd.Parameters.Add("@LineId", SqlDbType.Int).Value = departmentLineId;
                cmd.Parameters.Add("@EmployeeTypeId", SqlDbType.Int).Value = employeeTypeId;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                cmd.Parameters.Add("@EmployeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;

                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public List<JobCardInfoModel> GetJobCardInfoNoPenalty(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId, int Year, int Month, DateTime? fromDate, DateTime? toDate, string userName)
        {
            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter { ParameterName = "Year", Value = Year };

                var monthParam = new SqlParameter { ParameterName = "Month", Value = Month };

                var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

                var toDateParam = new SqlParameter { ParameterName = "ToDate", Value = toDate };

                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };


                return Context.Database.SqlQuery<JobCardInfoModel>("SPGetEmployeeJobCard_NoPenalty @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                   "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId," +
                                                                   "@EmployeeCardId, @Year, @Month, @FromDate, @ToDate, @UserName", companyIdParam, branchIdParam,
                    branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                    employeeTypeIdParam, employeeCardIdParam, yearParam, monthParam, fromDateParam, toDateParam, userNameParam).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public DataTable GetAgeAndFitnessCertificateInfo(Guid employeeId, string userName, DateTime prepareDate)
        {
            var table = new DataTable();

            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            const string cmdText = @"SPGetAgeAndFitnessCertificate";
            SqlCommand command = new SqlCommand(cmdText, connection);

            command.Parameters.AddWithValue("@employeeId", employeeId);
            command.Parameters.AddWithValue("@userName", userName);
            command.Parameters.AddWithValue("@prepareDate", prepareDate);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    var adapter = new SqlDataAdapter(command);
                    adapter.Fill(table);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                table = null;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }

            return table;
        }

        public DataTable GetJobApplicationInfo(Guid employeeId, string userName, DateTime prepareDate)
        {
            var table = new DataTable();

            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            const string cmdText = @"SPGetJobApplication";
            SqlCommand command = new SqlCommand(cmdText, connection);

            command.Parameters.AddWithValue("@employeeId", employeeId);
            command.Parameters.AddWithValue("@userName", userName);
            command.Parameters.AddWithValue("@prepareDate", prepareDate);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    var adapter = new SqlDataAdapter(command);
                    adapter.Fill(table);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                table = null;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            return table;
        }

        public DataTable GetJobVerificationInfo(Guid employeeId, Guid? userId, DateTime prepareDate)
        {
            var table = new DataTable();

            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            const string cmdText = @"SPGetJobVerification";
            SqlCommand command = new SqlCommand(cmdText, connection);

            command.Parameters.AddWithValue("@employeeId", employeeId);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@prepareDate", prepareDate);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    var adapter = new SqlDataAdapter(command);
                    adapter.Fill(table);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                table = null;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            return table;
        }

        public DataTable GetLeaveApplicationWorkerInfo(Guid employeeId, string userName, DateTime prepareDate)
        {
            var table = new DataTable();

            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            const string cmdText = @"SPGetLeaveApplicationWorker";
            SqlCommand command = new SqlCommand(cmdText, connection);

            command.Parameters.AddWithValue("@employeeId", employeeId);
            command.Parameters.AddWithValue("@userName", userName);
            command.Parameters.AddWithValue("@prepareDate", prepareDate);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    var adapter = new SqlDataAdapter(command);
                    adapter.Fill(table);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                table = null;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            return table;
        }

        public List<EmployeeLeaveData> GetEmployeeLeaveData(string employeeCardId, int year)
        {
            if (Context.Employees.Count(p => p.EmployeeCardId == employeeCardId && p.IsActive) != 1)
                return null;

            var singleOrDefault = Context.Employees.SingleOrDefault(p => p.EmployeeCardId == employeeCardId && p.IsActive);
            if (singleOrDefault == null)
                return null;

            var employeeId = singleOrDefault.EmployeeId;

            var employeeDetail = Context.EmployeeCompanyInfos.Where(x => x.EmployeeId == employeeId && x.FromDate <= DateTime.Now)
                .OrderByDescending(x => x.FromDate)
                .FirstOrDefault();

            if (employeeDetail == null)
                return null;

            var branchUnitId = 0;

            var branchUnitDepartment = Context.BranchUnitDepartments.SingleOrDefault(p => p.BranchUnitDepartmentId == employeeDetail.BranchUnitDepartmentId);

            if (branchUnitDepartment != null)
            {
                branchUnitId = branchUnitDepartment.BranchUnitId;
            }

            var employeeTypeId = Context.EmployeeCompanyInfos.Include(p => p.EmployeeDesignation)
                                .OrderByDescending(p => p.FromDate)
                                .First(p => p.EmployeeId == employeeId && p.FromDate <= DateTime.Now && p.EmployeeDesignation.IsActive == true && p.IsActive == true)
                                .EmployeeDesignation.EmployeeTypeId;


            var employeeIdParam = new SqlParameter { ParameterName = "EmployeeId", Value = employeeId };
            var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };
            var yearParam = new SqlParameter { ParameterName = "Year", Value = year };
            var branchUnitIdParam = new SqlParameter { ParameterName = "@BranchUnitId", Value = branchUnitId };
            var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

            return Context.Database.SqlQuery<EmployeeLeaveData>("HRMSPGetIndividualLeaveHistoryForSpecificYear @EmployeeId, @EmployeeCardId, @Year, @BranchUnitId, @EmployeeTypeId", employeeIdParam, employeeCardIdParam, yearParam, branchUnitIdParam, employeeTypeIdParam).ToList();
        }

        public DataTable GetEmployeeLeaveSummary(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? employeeTypeId, string employeeCardId, DateTime? fromDate, DateTime? toDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPGetEmployeeLeaveSummary"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchId", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@BranchUnitId", SqlDbType.Int).Value = branchUnitId;
                cmd.Parameters.Add("@BranchUnitDepartmentId", SqlDbType.Int).Value = branchUnitDepartmentId;
                cmd.Parameters.Add("@SectionId", SqlDbType.Int).Value = departmentSectionId;
                cmd.Parameters.Add("@EmployeeTypeId", SqlDbType.Int).Value = employeeTypeId;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                cmd.Parameters.Add("@EmployeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;

                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable LeaveApplicationStaff(Guid employeeId, string userName, DateTime prepareDate)
        {
            var table = new DataTable();

            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            const string cmdText = @"SPGetLeaveApplicationStaff";
            SqlCommand command = new SqlCommand(cmdText, connection);

            command.Parameters.AddWithValue("@employeeId", employeeId);
            command.Parameters.AddWithValue("@userName", userName);
            command.Parameters.AddWithValue("@prepareDate", prepareDate);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    var adapter = new SqlDataAdapter(command);
                    adapter.Fill(table);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                table = null;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            return table;
        }

        public DataTable GetNominationFormInfo(Guid employeeId, string userName, DateTime prepareDate)
        {
            var table = new DataTable();

            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            const string cmdText = @"SPGetNominationFormInfo";
            SqlCommand command = new SqlCommand(cmdText, connection);

            command.Parameters.AddWithValue("@employeeId", employeeId);
            command.Parameters.AddWithValue("@userName", userName);
            command.Parameters.AddWithValue("@prepareDate", prepareDate);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    var adapter = new SqlDataAdapter(command);
                    adapter.Fill(table);
                }
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                table = null;
            }

            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }

            return table;
        }

        public DataTable GetManPowerBudget(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {

            var employeeCardIdParm = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

            if (companyId == 0)
                companyId = null;
            var companyIdParam = new SqlParameter { ParameterName = "CompanyID", Value = companyId };

            if (branchId == 0)
                branchId = null;
            var branchIdParam = new SqlParameter { ParameterName = "BranchID", Value = branchId };

            if (branchUnitId == 0)
                branchUnitId = null;
            var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitID", Value = branchUnitId };

            if (branchUnitDepartmentId == 0)
                branchUnitDepartmentId = null;
            var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentID", Value = branchUnitDepartmentId };

            if (departmentSectionId == 0)
                departmentSectionId = null;
            var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

            if (departmentLineId == 0)
                departmentLineId = null;
            var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

            if (employeeTypeId == 0)
                employeeTypeId = null;
            var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeID", Value = employeeTypeId };

            if (employeeGradeId == 0)
                employeeGradeId = null;
            var employeeGradeIdParam = new SqlParameter { ParameterName = "EmployeeGradeID", Value = employeeGradeId };

            if (employeeDesignationId == 0)
                employeeDesignationId = null;
            var employeeDesignationIdParam = new SqlParameter { ParameterName = "EmployeeDesignationID", Value = employeeDesignationId };

            var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };

            if (fromDate == null)
                fromDate = new DateTime(1900, 01, 01);
            var fromDateParam = new SqlParameter { ParameterName = "fromDate", Value = fromDate };

            if (toDate == null)
                toDate = new DateTime(1900, 01, 01);
            var toDateParam = new SqlParameter { ParameterName = "toDate", Value = toDate };

            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpManPowerBudgetReport"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EffectiveDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@EmployeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@BranchUnitID", SqlDbType.Int).Value = branchUnitId;
                cmd.Parameters.Add("@BranchUnitDepartmentID", SqlDbType.Int).Value = branchUnitDepartmentId;
                cmd.Parameters.Add("@SectionId", SqlDbType.Int).Value = departmentSectionId;
                cmd.Parameters.Add("@LineId", SqlDbType.Int).Value = departmentLineId;
                cmd.Parameters.Add("@EmployeeTypeID", SqlDbType.Int).Value = employeeTypeId;
                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetDailyOTDetailWithAmount(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {

            if (companyId == 0)
                companyId = null;
            var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

            if (branchId == 0)
                branchId = null;
            var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

            if (branchUnitId == 0)
                branchUnitId = null;
            var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

            if (branchUnitDepartmentId == 0)
                branchUnitDepartmentId = null;
            var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

            if (departmentSectionId == 0)
                departmentSectionId = null;
            var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

            if (departmentLineId == 0)
                departmentLineId = null;
            var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

            if (employeeTypeId == 0)
                employeeTypeId = null;
            var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

            if (employeeDesignationId == 0)
                employeeDesignationId = null;
            var employeeDesignationIdParam = new SqlParameter { ParameterName = "EmployeeDesignationId", Value = employeeDesignationId };

            var employeeCardIdParm = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

            if (fromDate == null)
                fromDate = new DateTime(1900, 01, 01);
            var fromDateParam = new SqlParameter { ParameterName = "fromDate", Value = fromDate };

            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpDailyOTDetailWithAmount"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchId", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@BranchUnitId", SqlDbType.Int).Value = branchUnitId;
                cmd.Parameters.Add("@BranchUnitDepartmentId", SqlDbType.Int).Value = branchUnitDepartmentId;
                cmd.Parameters.Add("@SectionId", SqlDbType.Int).Value = departmentSectionId;
                cmd.Parameters.Add("@LineId", SqlDbType.Int).Value = departmentLineId;
                cmd.Parameters.Add("@EmployeeTypeId", SqlDbType.Int).Value = employeeTypeId;
                cmd.Parameters.Add("@EmployeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;
                cmd.Parameters.Add("@EffectiveDate", SqlDbType.DateTime).Value = fromDate;

                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetEmployeeEarnLeaveQuit(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, int? departmentLineId, int? employeeTypeId, int? employeeGradeId, int? employeeDesignationId, string employeeCardId, string userName, DateTime? fromDate, DateTime? toDate)
        {

            var employeeCardIdParm = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

            if (companyId == 0)
                companyId = -1;
            var companyIdParam = new SqlParameter { ParameterName = "CompanyID", Value = companyId };

            if (branchId == 0)
                branchId = -1;
            var branchIdParam = new SqlParameter { ParameterName = "BranchID", Value = branchId };

            if (branchUnitId == 0)
                branchUnitId = -1;
            var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitID", Value = branchUnitId };

            if (branchUnitDepartmentId == 0)
                branchUnitDepartmentId = -1;
            var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentID", Value = branchUnitDepartmentId };

            if (departmentSectionId == 0)
                departmentSectionId = -1;
            var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

            if (departmentLineId == 0)
                departmentLineId = -1;
            var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

            if (employeeTypeId == 0)
                employeeTypeId = -1;
            var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeID", Value = employeeTypeId };

            if (employeeGradeId == 0)
                employeeGradeId = -1;
            var employeeGradeIdParam = new SqlParameter { ParameterName = "EmployeeGradeID", Value = employeeGradeId };

            if (employeeDesignationId == 0)
                employeeDesignationId = -1;
            var employeeDesignationIdParam = new SqlParameter { ParameterName = "EmployeeDesignationID", Value = employeeDesignationId };

            var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };

            if (fromDate == null)
                fromDate = new DateTime(1900, 01, 01);
            var fromDateParam = new SqlParameter { ParameterName = "fromDate", Value = fromDate };

            if (toDate == null)
                toDate = new DateTime(1900, 01, 01);
            var toDateParam = new SqlParameter { ParameterName = "toDate", Value = toDate };
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPEarnLeaveSheetQuitEmployee"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@toDate", SqlDbType.DateTime).Value = toDate;
                cmd.Parameters.Add("@EmployeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@BranchUnitID", SqlDbType.Int).Value = branchUnitId;
                cmd.Parameters.Add("@BranchUnitDepartmentID", SqlDbType.Int).Value = branchUnitDepartmentId;
                cmd.Parameters.Add("@DepartmentSectionId", SqlDbType.Int).Value = departmentSectionId;
                cmd.Parameters.Add("@DepartmentLineId", SqlDbType.Int).Value = departmentLineId;
                cmd.Parameters.Add("@EmployeeTypeID", SqlDbType.Int).Value = employeeTypeId;
                cmd.Parameters.Add("@EmployeeGradeID", SqlDbType.Int).Value = employeeGradeId;
                cmd.Parameters.Add("@EmployeeDesignationID", SqlDbType.Int).Value = employeeDesignationId;
                cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 100).Value = userName;
                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetFemaleConsentLetterInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId, string employeeCardId, DateTime? disagreeDate, DateTime? effectiveDate)
        {

            var employeeCardIdParm = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

            if (companyId == 0)
                companyId = -1;
            var companyIdParam = new SqlParameter { ParameterName = "CompanyID", Value = companyId };

            if (branchId == 0)
                branchId = -1;
            var branchIdParam = new SqlParameter { ParameterName = "BranchID", Value = branchId };

            if (branchUnitId == 0)
                branchUnitId = -1;
            var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitID", Value = branchUnitId };

            if (branchUnitDepartmentId == 0)
                branchUnitDepartmentId = -1;
            var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentID", Value = branchUnitDepartmentId };

            if (departmentSectionId == 0)
                departmentSectionId = -1;




            if (disagreeDate == null)
                disagreeDate = new DateTime(1900, 01, 01);


            if (effectiveDate == null)
                effectiveDate = new DateTime(1900, 01, 01);

            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPFemaleConsentLetter"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DisagreeDate", SqlDbType.DateTime).Value = disagreeDate;
                cmd.Parameters.Add("@EffectiveDate", SqlDbType.DateTime).Value = effectiveDate;
                cmd.Parameters.Add("@EmployeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyId;
                cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                cmd.Parameters.Add("@BranchUnitID", SqlDbType.Int).Value = branchUnitId;
                cmd.Parameters.Add("@BranchUnitDepartmentID", SqlDbType.Int).Value = branchUnitDepartmentId;
                cmd.Parameters.Add("@DepartmentSectionId", SqlDbType.Int).Value = departmentSectionId;

                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public List<JobCardInfoModel> GetJobCardModelKnittingDyeingInfo(int? companyId, int? branchId, int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId,
           int? departmentLineId, int? employeeTypeId, string employeeCardId, int Year, int Month, DateTime? fromDate,
           DateTime? toDate, string userName)
        {
            try
            {

                if (companyId == 0)
                    companyId = -1;
                var companyIdParam = new SqlParameter { ParameterName = "CompanyId", Value = companyId };

                if (branchId == 0)
                    branchId = -1;
                var branchIdParam = new SqlParameter { ParameterName = "BranchId", Value = branchId };

                if (branchUnitId == 0)
                    branchUnitId = -1;
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;
                var branchUnitDepartmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = branchUnitDepartmentId };

                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new SqlParameter { ParameterName = "DepartmentSectionId", Value = departmentSectionId };

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new SqlParameter { ParameterName = "DepartmentLineId", Value = departmentLineId };

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                var yearParam = new SqlParameter { ParameterName = "Year", Value = Year };

                var monthParam = new SqlParameter { ParameterName = "Month", Value = Month };

                var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = fromDate };

                var toDateParam = new SqlParameter { ParameterName = "ToDate", Value = toDate };


                var userNameParam = new SqlParameter { ParameterName = "UserName", Value = userName };


                List<JobCardInfoModel> jobCardInfo = Context.Database.SqlQuery<JobCardInfoModel>("SPGetEmployeeJobCardModelKnittingDyeing @CompanyId, @BranchId, @BranchUnitId, @BranchUnitDepartmentId, " +
                                                                                                 "@DepartmentSectionId, @DepartmentLineId, @EmployeeTypeId," +
                                                                                                 "@EmployeeCardId, @Year, @Month, @FromDate, @ToDate, @UserName", companyIdParam, branchIdParam,
                    branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                    employeeTypeIdParam, employeeCardIdParam, yearParam, monthParam, fromDateParam, toDateParam, userNameParam).ToList();

                foreach (var t in jobCardInfo)
                {
                    VwPenaltyEmployee temp = Context.VwPenaltyEmployee.FirstOrDefault(p => p.EmployeeCardId == t.EmployeeCardId && p.PenaltyDate == t.Date && p.IsActive);
                    if (temp != null)
                    {
                        t.Status = "Absent";
                        t.InTime = "";
                        t.OutTime = "";
                        t.Delay = null;
                        t.OTHours = 0.00m;
                        t.Remarks = "";
                    }
                }

                return jobCardInfo;

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public DataTable GetEmployeeLeaveRegister(string employeeCardId, DateTime prepareDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPLeaveRegister"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EffectiveDate", SqlDbType.DateTime).Value = prepareDate;
                cmd.Parameters.Add("@EmployeeCardId", SqlDbType.NVarChar, 100).Value = employeeCardId;

                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetDyeingShiftCount(DateTime fromDate, DateTime toDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPGetDyeingShiftCount"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;

                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetLeaveButPresent(DateTime fromDate, DateTime toDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPGetLeaveButPresent"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;

                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetAbsentOnJoiningDate(DateTime fromDate, DateTime toDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPGetAbsentOnJoiningDate"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;

                cmd.Connection = connection;
                cmd.CommandTimeout = 3600;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }
    }
}