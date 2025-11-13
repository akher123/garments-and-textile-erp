using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using System.IO;
using iTextSharp.text;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Model.HRMModel;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using System.Globalization;
using System.Text;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Web.Helpers;
using SCERP.Web.Models;
using System.Collections;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class ReportController : BaseHrmController
    {
        private readonly IPenaltyManager _penaltyManager;

        public ReportController(IPenaltyManager penaltyManager)
        {
            _penaltyManager = penaltyManager;
        }

        public ActionResult Index()
        {
            return null;
        }

        public ActionResult AbsentOTPenaltyReport(AbsentOtPenaltyViewModel model, int reportTypeId)
        {
            ModelState.Clear();
            var reportType = new ReportType();
            switch (reportTypeId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 3:
                    reportType = ReportType.Excel;
                    break;
            }
            model.PenaltyEmployees = _penaltyManager.GetAbsentOtPenaltyEmployee(model.SearchFieldModel);
            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "AbsentOTPenalty.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            DateTime startDate = model.SearchFieldModel.StartDate.GetValueOrDefault();
            ReportParameter[] parameters =
            {
                new ReportParameter("StartDate", startDate.ToString("dd/MM/yyyy"))
            };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.PenaltyEmployees.OrderBy(x => x.EmployeeName)) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.25, MarginRight = 0.25, MarginBottom = .25 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, parameters.ToList());
        }

        //[AjaxAuthorize(Roles = "jobcard-1,jobcard-2,jobcard-3")]
        public ActionResult JobCardIndex(DynamicReportHeadViewModel model)
        {
            ModelState.Clear();

            try
            {


                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                //model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(PortalContext.CurrentUser.CompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        //[AjaxAuthorize(Roles = "jobcard-1,jobcard-2,jobcard-3")]
        public ActionResult JobCard(DynamicReportHeadViewModel model)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "JobCard.rdlc");
                if (System.IO.File.Exists(path))
                    lr.ReportPath = path;
                else
                    return View("Index");

                int? companyId = model.SearchFieldModel.SearchByCompanyId;
                // int? companyId = PortalContext.CurrentUser.CompanyId;
                int? branchId = model.SearchFieldModel.SearchByBranchId;
                int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
                int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
                int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
                int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
                int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
                string employeeCardId = model.SearchFieldModel.SearchByEmployeeCardId;

                int year = Convert.ToInt32(model.SearchFieldModel.SelectedYear);
                int month = Convert.ToInt32(model.SearchFieldModel.SelectedMonth);

                DateTime? fromDate = model.SearchFieldModel.StartDate;
                DateTime? toDate = model.SearchFieldModel.EndDate;

                string userName = PortalContext.CurrentUser.Name;


                List<JobCardInfoModel> jobCardInfo = HrmReportManager.GetJobCardInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                    departmentLineId, employeeTypeId, employeeCardId, year, month,
                    fromDate, toDate, userName);


                ReportDataSource rd = new ReportDataSource("DataSetEmployeeJobCard", jobCardInfo);

                ReportParameter[] parameters = new ReportParameter[47];

                List<string> reportHeads = model.ReportHeads;

                if (reportHeads != null)
                {
                    if (reportHeads.Contains("Branch"))
                        parameters[0] = new ReportParameter("Param_Branch", "True");
                    else
                        parameters[0] = new ReportParameter("Param_Branch", "False");

                    if (reportHeads.Contains("Unit"))
                        parameters[1] = new ReportParameter("Param_Unit", "True");
                    else
                        parameters[1] = new ReportParameter("Param_Unit", "False");

                    if (reportHeads.Contains("Department"))
                        parameters[2] = new ReportParameter("Param_Department", "True");
                    else
                        parameters[2] = new ReportParameter("Param_Department", "False");

                    if (reportHeads.Contains("Section"))
                        parameters[3] = new ReportParameter("Param_Section", "True");
                    else
                        parameters[3] = new ReportParameter("Param_Section", "False");

                    if (reportHeads.Contains("Line"))
                        parameters[4] = new ReportParameter("Param_Line", "True");
                    else
                        parameters[4] = new ReportParameter("Param_Line", "False");

                    if (reportHeads.Contains("EmployeeType"))
                        parameters[5] = new ReportParameter("Param_EmployeeType", "True");
                    else
                        parameters[5] = new ReportParameter("Param_EmployeeType", "False");

                    if (reportHeads.Contains("EmployeeGrade"))
                        parameters[6] = new ReportParameter("Param_EmployeeGrade", "True");
                    else
                        parameters[6] = new ReportParameter("Param_EmployeeGrade", "False");

                    if (reportHeads.Contains("EmployeeDesignation"))
                        parameters[7] = new ReportParameter("Param_EmployeeDesignation", "True");
                    else
                        parameters[7] = new ReportParameter("Param_EmployeeDesignation", "False");

                    if (reportHeads.Contains("MobileNo"))
                        parameters[8] = new ReportParameter("Param_MobileNo", "True");
                    else
                        parameters[8] = new ReportParameter("Param_MobileNo", "False");

                    if (reportHeads.Contains("JoiningDate"))
                        parameters[9] = new ReportParameter("Param_JoiningDate", "True");
                    else
                        parameters[9] = new ReportParameter("Param_JoiningDate", "False");

                    if (reportHeads.Contains("QuitDate"))
                        parameters[10] = new ReportParameter("Param_QuitDate", "True");
                    else
                        parameters[10] = new ReportParameter("Param_QuitDate", "False");

                    if (reportHeads.Contains("GrossSalary"))
                        parameters[11] = new ReportParameter("Param_GrossSalary", "True");
                    else
                        parameters[11] = new ReportParameter("Param_GrossSalary", "False");

                    if (reportHeads.Contains("OTRate"))
                        parameters[12] = new ReportParameter("Param_OTRate", "True");
                    else
                        parameters[12] = new ReportParameter("Param_OTRate", "False");

                    if (reportHeads.Contains("PresentDays"))
                        parameters[13] = new ReportParameter("Param_PresentDays", "True");
                    else
                        parameters[13] = new ReportParameter("Param_PresentDays", "False");

                    if (reportHeads.Contains("LateDays"))
                        parameters[14] = new ReportParameter("Param_LateDays", "True");
                    else
                        parameters[14] = new ReportParameter("Param_LateDays", "False");

                    if (reportHeads.Contains("OSDDays"))
                        parameters[15] = new ReportParameter("Param_OSDDays", "True");
                    else
                        parameters[15] = new ReportParameter("Param_OSDDays", "False");

                    if (reportHeads.Contains("AbsentDays"))
                        parameters[16] = new ReportParameter("Param_AbsentDays", "True");
                    else
                        parameters[16] = new ReportParameter("Param_AbsentDays", "False");

                    if (reportHeads.Contains("LeaveDays"))
                        parameters[17] = new ReportParameter("Param_LeaveDays", "True");
                    else
                        parameters[17] = new ReportParameter("Param_LeaveDays", "False");

                    if (reportHeads.Contains("Holidays"))
                        parameters[18] = new ReportParameter("Param_Holidays", "True");
                    else
                        parameters[18] = new ReportParameter("Param_Holidays", "False");

                    if (reportHeads.Contains("WeekendDays"))
                        parameters[19] = new ReportParameter("Param_WeekendDays", "True");
                    else
                        parameters[19] = new ReportParameter("Param_WeekendDays", "False");

                    if (reportHeads.Contains("LWP"))
                        parameters[20] = new ReportParameter("Param_LWP", "True");
                    else
                        parameters[20] = new ReportParameter("Param_LWP", "False");

                    if (reportHeads.Contains("TotalDays"))
                        parameters[21] = new ReportParameter("Param_TotalDays", "True");
                    else
                        parameters[21] = new ReportParameter("Param_TotalDays", "False");

                    if (reportHeads.Contains("WorkingDays"))
                        parameters[22] = new ReportParameter("Param_WorkingDays", "True");
                    else
                        parameters[22] = new ReportParameter("Param_WorkingDays", "False");

                    if (reportHeads.Contains("PayDays"))
                        parameters[23] = new ReportParameter("Param_PayDays", "True");
                    else
                        parameters[23] = new ReportParameter("Param_PayDays", "False");


                    if (reportHeads.Contains("OTHourLast"))
                        parameters[24] = new ReportParameter("Param_OTHourLast", "True");
                    else
                        parameters[24] = new ReportParameter("Param_OTHourLast", "False");


                    if (reportHeads.Contains("TotalExtraOTHours"))
                        parameters[25] = new ReportParameter("Param_TotalExtraOTHours", "True");
                    else
                        parameters[25] = new ReportParameter("Param_TotalExtraOTHours", "False");


                    if (reportHeads.Contains("TotalHolidayOTHours"))
                        parameters[26] = new ReportParameter("Param_TotalHolidayOTHours", "True");
                    else
                        parameters[26] = new ReportParameter("Param_TotalHolidayOTHours", "False");


                    if (reportHeads.Contains("TotalWeekendOTHours"))
                        parameters[27] = new ReportParameter("Param_TotalWeekendOTHours", "True");
                    else
                        parameters[27] = new ReportParameter("Param_TotalWeekendOTHours", "False");


                    if (reportHeads.Contains("Date"))
                        parameters[28] = new ReportParameter("Param_Date", "True");
                    else
                        parameters[28] = new ReportParameter("Param_Date", "False");


                    if (reportHeads.Contains("DayName"))
                        parameters[29] = new ReportParameter("Param_DayName", "True");
                    else
                        parameters[29] = new ReportParameter("Param_DayName", "False");


                    if (reportHeads.Contains("Shift"))
                        parameters[30] = new ReportParameter("Param_Shift", "True");
                    else
                        parameters[30] = new ReportParameter("Param_Shift", "False");


                    if (reportHeads.Contains("Status"))
                        parameters[31] = new ReportParameter("Param_Status", "True");
                    else
                        parameters[31] = new ReportParameter("Param_Status", "False");

                    if (reportHeads.Contains("InTime"))
                        parameters[32] = new ReportParameter("Param_InTime", "True");
                    else
                        parameters[32] = new ReportParameter("Param_InTime", "False");

                    if (reportHeads.Contains("OutTime"))
                        parameters[33] = new ReportParameter("Param_OutTime", "True");
                    else
                        parameters[33] = new ReportParameter("Param_OutTime", "False");

                    if (reportHeads.Contains("Delay"))
                        parameters[34] = new ReportParameter("Param_Delay", "True");
                    else
                        parameters[34] = new ReportParameter("Param_Delay", "False");

                    if (reportHeads.Contains("OTHours"))
                        parameters[35] = new ReportParameter("Param_OTHours", "True");
                    else
                        parameters[35] = new ReportParameter("Param_OTHours", "False");

                    if (reportHeads.Contains("ExtraOTHours"))
                        parameters[36] = new ReportParameter("Param_ExtraOTHours", "True");
                    else
                        parameters[36] = new ReportParameter("Param_ExtraOTHours", "False");

                    if (reportHeads.Contains("HolidayOTHours"))
                        parameters[37] = new ReportParameter("Param_HolidayOTHours", "True");
                    else
                        parameters[37] = new ReportParameter("Param_HolidayOTHours", "False");

                    if (reportHeads.Contains("WeekendOTHours"))
                        parameters[38] = new ReportParameter("Param_WeekendOTHours", "True");
                    else
                        parameters[38] = new ReportParameter("Param_WeekendOTHours", "False");

                    if (reportHeads.Contains("Remarks"))
                        parameters[39] = new ReportParameter("Param_Remarks", "True");
                    else
                        parameters[39] = new ReportParameter("Param_Remarks", "False");

                    if (reportHeads.Contains("TotalPenaltyOTHours"))
                        parameters[40] = new ReportParameter("Param_TotalPenaltyOTHours", "True");
                    else
                        parameters[40] = new ReportParameter("Param_TotalPenaltyOTHours", "False");

                    if (reportHeads.Contains("TotalPenaltyAttendanceDays"))
                        parameters[41] = new ReportParameter("Param_TotalPenaltyAttendanceDays", "True");
                    else
                        parameters[41] = new ReportParameter("Param_TotalPenaltyAttendanceDays", "False");

                    if (reportHeads.Contains("TotalPenaltyLeaveDays"))
                        parameters[42] = new ReportParameter("Param_TotalPenaltyLeaveDays", "True");
                    else
                        parameters[42] = new ReportParameter("Param_TotalPenaltyLeaveDays", "False");

                    if (reportHeads.Contains("TotalPenaltyFinancialAmount"))
                        parameters[43] = new ReportParameter("Param_TotalPenaltyFinancialAmount", "True");
                    else
                        parameters[43] = new ReportParameter("Param_TotalPenaltyFinancialAmount", "False");

                    if (reportHeads.Contains("LastIncrementDate"))
                        parameters[44] = new ReportParameter("Param_LastIncrementDate", "True");
                    else
                        parameters[44] = new ReportParameter("Param_LastIncrementDate", "False");

                    if (reportHeads.Contains("SkillType"))
                        parameters[45] = new ReportParameter("Param_SkillType", "True");
                    else
                        parameters[45] = new ReportParameter("Param_SkillType", "False");

                    DateTime paramFromDate = new DateTime(2017, 5, 26);
                    DateTime paramToDate = new DateTime(2017, 6, 25);

                    if (jobCardInfo[0].Unit == "Garments")
                    {
                        if (jobCardInfo[0].Date >= paramFromDate && jobCardInfo[0].Date <= paramToDate)
                            parameters[46] = new ReportParameter("Param_Note", "Lunch Break 30 Minutes");
                        else
                            parameters[46] = new ReportParameter("Param_Note", "Lunch Break 1 Hour");
                    }
                    else
                        parameters[46] = new ReportParameter("Param_Note", "10.30 to 11.00 Refreshment Break \n            04.30 to 05.00 Refreshment Break");

                    lr.SetParameters(parameters);

                    lr.DataSources.Add(rd);
                }

                string reportType = "pdf";
                if (model.SearchFieldModel.PrintFormatId == 2)
                    reportType = "Excel";

                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =
                    "<DeviceInfo>" +
                    "  <OutputFormat>" + 2 + "</OutputFormat>" +
                    "  <PageWidth>8.5in</PageWidth>" +
                    "  <PageHeight>11.7in</PageHeight>" +
                    "  <MarginTop>0.4in</MarginTop>" +
                    "  <MarginLeft>.4in</MarginLeft>" +
                    "  <MarginRight>.2in</MarginRight>" +
                    "  <MarginBottom>0.2in</MarginBottom>" +
                    "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;

                byte[] renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);


                //using (FileStream fs = new FileStream("D:\\QA_TempFile\\Jobcard.pdf", FileMode.Create))
                //{
                //    fs.Write(renderedBytes, 0, renderedBytes.Length);
                //}

                //EmailSender emailSender = new EmailSender("smtpout.secureserver.net", 80, "info@soft-code.net", "info123");

                //emailSender.From = "info@soft-code.net";
                //emailSender.To = "kallol39@gmail.com";
                //emailSender.Subject = "Test Mail - 1";
                //emailSender.Body = "mail with attachment";
                //emailSender.AddAttachment("D:\\QA_TempFile\\Jobcard.pdf");
                //emailSender.Send();


                return File(renderedBytes, mimeType);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return JobCardIndex(model);

        }

        public ActionResult JobCard10PMIndex(DynamicReportHeadViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [ActionName("Jobcardydi")]
        public ActionResult JobCard10PM(DynamicReportHeadViewModel model)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "Jobcardydi.rdlc");
                if (System.IO.File.Exists(path))
                    lr.ReportPath = path;
                else
                    return View("Index");

                int? companyId = model.SearchFieldModel.SearchByCompanyId;
                int? branchId = model.SearchFieldModel.SearchByBranchId;
                int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
                int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
                int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
                int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
                int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
                string employeeCardId = model.SearchFieldModel.SearchByEmployeeCardId;

                int year = Convert.ToInt32(model.SearchFieldModel.SelectedYear);
                int month = Convert.ToInt32(model.SearchFieldModel.SelectedMonth);

                DateTime? fromDate = model.SearchFieldModel.StartDate;
                DateTime? toDate = model.SearchFieldModel.EndDate;

                string userName = PortalContext.CurrentUser.Name;

                List<JobCardInfoModel> jobCardInfo = HrmReportManager.GetJobCardInfo10PM(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                    departmentLineId, employeeTypeId, employeeCardId, year, month,
                    fromDate, toDate, userName);

                ReportDataSource rd = new ReportDataSource("DataSetEmployeeJobCard", jobCardInfo);

                ReportParameter[] parameters = new ReportParameter[47];

                List<string> reportHeads = model.ReportHeads;

                if (reportHeads != null)
                {
                    if (reportHeads.Contains("Branch"))
                        parameters[0] = new ReportParameter("Param_Branch", "True");
                    else
                        parameters[0] = new ReportParameter("Param_Branch", "False");

                    if (reportHeads.Contains("Unit"))
                        parameters[1] = new ReportParameter("Param_Unit", "True");
                    else
                        parameters[1] = new ReportParameter("Param_Unit", "False");

                    if (reportHeads.Contains("Department"))
                        parameters[2] = new ReportParameter("Param_Department", "True");
                    else
                        parameters[2] = new ReportParameter("Param_Department", "False");

                    if (reportHeads.Contains("Section"))
                        parameters[3] = new ReportParameter("Param_Section", "True");
                    else
                        parameters[3] = new ReportParameter("Param_Section", "False");

                    if (reportHeads.Contains("Line"))
                        parameters[4] = new ReportParameter("Param_Line", "True");
                    else
                        parameters[4] = new ReportParameter("Param_Line", "False");

                    if (reportHeads.Contains("EmployeeType"))
                        parameters[5] = new ReportParameter("Param_EmployeeType", "True");
                    else
                        parameters[5] = new ReportParameter("Param_EmployeeType", "False");

                    if (reportHeads.Contains("EmployeeGrade"))
                        parameters[6] = new ReportParameter("Param_EmployeeGrade", "True");
                    else
                        parameters[6] = new ReportParameter("Param_EmployeeGrade", "False");

                    if (reportHeads.Contains("EmployeeDesignation"))
                        parameters[7] = new ReportParameter("Param_EmployeeDesignation", "True");
                    else
                        parameters[7] = new ReportParameter("Param_EmployeeDesignation", "False");

                    if (reportHeads.Contains("MobileNo"))
                        parameters[8] = new ReportParameter("Param_MobileNo", "True");
                    else
                        parameters[8] = new ReportParameter("Param_MobileNo", "False");

                    if (reportHeads.Contains("JoiningDate"))
                        parameters[9] = new ReportParameter("Param_JoiningDate", "True");
                    else
                        parameters[9] = new ReportParameter("Param_JoiningDate", "False");

                    if (reportHeads.Contains("QuitDate"))
                        parameters[10] = new ReportParameter("Param_QuitDate", "True");
                    else
                        parameters[10] = new ReportParameter("Param_QuitDate", "False");

                    if (reportHeads.Contains("GrossSalary"))
                        parameters[11] = new ReportParameter("Param_GrossSalary", "True");
                    else
                        parameters[11] = new ReportParameter("Param_GrossSalary", "False");

                    if (reportHeads.Contains("OTRate"))
                        parameters[12] = new ReportParameter("Param_OTRate", "True");
                    else
                        parameters[12] = new ReportParameter("Param_OTRate", "False");

                    if (reportHeads.Contains("PresentDays"))
                        parameters[13] = new ReportParameter("Param_PresentDays", "True");
                    else
                        parameters[13] = new ReportParameter("Param_PresentDays", "False");

                    if (reportHeads.Contains("LateDays"))
                        parameters[14] = new ReportParameter("Param_LateDays", "True");
                    else
                        parameters[14] = new ReportParameter("Param_LateDays", "False");

                    if (reportHeads.Contains("OSDDays"))
                        parameters[15] = new ReportParameter("Param_OSDDays", "True");
                    else
                        parameters[15] = new ReportParameter("Param_OSDDays", "False");

                    if (reportHeads.Contains("AbsentDays"))
                        parameters[16] = new ReportParameter("Param_AbsentDays", "True");
                    else
                        parameters[16] = new ReportParameter("Param_AbsentDays", "False");

                    if (reportHeads.Contains("LeaveDays"))
                        parameters[17] = new ReportParameter("Param_LeaveDays", "True");
                    else
                        parameters[17] = new ReportParameter("Param_LeaveDays", "False");

                    if (reportHeads.Contains("Holidays"))
                        parameters[18] = new ReportParameter("Param_Holidays", "True");
                    else
                        parameters[18] = new ReportParameter("Param_Holidays", "False");

                    if (reportHeads.Contains("WeekendDays"))
                        parameters[19] = new ReportParameter("Param_WeekendDays", "True");
                    else
                        parameters[19] = new ReportParameter("Param_WeekendDays", "False");

                    if (reportHeads.Contains("LWP"))
                        parameters[20] = new ReportParameter("Param_LWP", "True");
                    else
                        parameters[20] = new ReportParameter("Param_LWP", "False");

                    if (reportHeads.Contains("TotalDays"))
                        parameters[21] = new ReportParameter("Param_TotalDays", "True");
                    else
                        parameters[21] = new ReportParameter("Param_TotalDays", "False");

                    if (reportHeads.Contains("WorkingDays"))
                        parameters[22] = new ReportParameter("Param_WorkingDays", "True");
                    else
                        parameters[22] = new ReportParameter("Param_WorkingDays", "False");

                    if (reportHeads.Contains("PayDays"))
                        parameters[23] = new ReportParameter("Param_PayDays", "True");
                    else
                        parameters[23] = new ReportParameter("Param_PayDays", "False");


                    if (reportHeads.Contains("OTHourLast"))
                        parameters[24] = new ReportParameter("Param_OTHourLast", "True");
                    else
                        parameters[24] = new ReportParameter("Param_OTHourLast", "False");


                    if (reportHeads.Contains("TotalExtraOTHours"))
                        parameters[25] = new ReportParameter("Param_TotalExtraOTHours", "True");
                    else
                        parameters[25] = new ReportParameter("Param_TotalExtraOTHours", "False");


                    if (reportHeads.Contains("TotalHolidayOTHours"))
                        parameters[26] = new ReportParameter("Param_TotalHolidayOTHours", "True");
                    else
                        parameters[26] = new ReportParameter("Param_TotalHolidayOTHours", "False");


                    if (reportHeads.Contains("TotalWeekendOTHours"))
                        parameters[27] = new ReportParameter("Param_TotalWeekendOTHours", "True");
                    else
                        parameters[27] = new ReportParameter("Param_TotalWeekendOTHours", "False");


                    if (reportHeads.Contains("Date"))
                        parameters[28] = new ReportParameter("Param_Date", "True");
                    else
                        parameters[28] = new ReportParameter("Param_Date", "False");


                    if (reportHeads.Contains("DayName"))
                        parameters[29] = new ReportParameter("Param_DayName", "True");
                    else
                        parameters[29] = new ReportParameter("Param_DayName", "False");


                    if (reportHeads.Contains("Shift"))
                        parameters[30] = new ReportParameter("Param_Shift", "True");
                    else
                        parameters[30] = new ReportParameter("Param_Shift", "False");


                    if (reportHeads.Contains("Status"))
                        parameters[31] = new ReportParameter("Param_Status", "True");
                    else
                        parameters[31] = new ReportParameter("Param_Status", "False");

                    if (reportHeads.Contains("InTime"))
                        parameters[32] = new ReportParameter("Param_InTime", "True");
                    else
                        parameters[32] = new ReportParameter("Param_InTime", "False");

                    if (reportHeads.Contains("OutTime"))
                        parameters[33] = new ReportParameter("Param_OutTime", "True");
                    else
                        parameters[33] = new ReportParameter("Param_OutTime", "False");

                    if (reportHeads.Contains("Delay"))
                        parameters[34] = new ReportParameter("Param_Delay", "True");
                    else
                        parameters[34] = new ReportParameter("Param_Delay", "False");

                    if (reportHeads.Contains("OTHours"))
                        parameters[35] = new ReportParameter("Param_OTHours", "True");
                    else
                        parameters[35] = new ReportParameter("Param_OTHours", "False");

                    if (reportHeads.Contains("ExtraOTHours"))
                        parameters[36] = new ReportParameter("Param_ExtraOTHours", "True");
                    else
                        parameters[36] = new ReportParameter("Param_ExtraOTHours", "False");

                    if (reportHeads.Contains("HolidayOTHours"))
                        parameters[37] = new ReportParameter("Param_HolidayOTHours", "True");
                    else
                        parameters[37] = new ReportParameter("Param_HolidayOTHours", "False");

                    if (reportHeads.Contains("WeekendOTHours"))
                        parameters[38] = new ReportParameter("Param_WeekendOTHours", "True");
                    else
                        parameters[38] = new ReportParameter("Param_WeekendOTHours", "False");

                    if (reportHeads.Contains("Remarks"))
                        parameters[39] = new ReportParameter("Param_Remarks", "True");
                    else
                        parameters[39] = new ReportParameter("Param_Remarks", "False");

                    if (reportHeads.Contains("TotalPenaltyOTHours"))
                        parameters[40] = new ReportParameter("Param_TotalPenaltyOTHours", "True");
                    else
                        parameters[40] = new ReportParameter("Param_TotalPenaltyOTHours", "False");

                    if (reportHeads.Contains("TotalPenaltyAttendanceDays"))
                        parameters[41] = new ReportParameter("Param_TotalPenaltyAttendanceDays", "True");
                    else
                        parameters[41] = new ReportParameter("Param_TotalPenaltyAttendanceDays", "False");

                    if (reportHeads.Contains("TotalPenaltyLeaveDays"))
                        parameters[42] = new ReportParameter("Param_TotalPenaltyLeaveDays", "True");
                    else
                        parameters[42] = new ReportParameter("Param_TotalPenaltyLeaveDays", "False");

                    if (reportHeads.Contains("TotalPenaltyFinancialAmount"))
                        parameters[43] = new ReportParameter("Param_TotalPenaltyFinancialAmount", "True");
                    else
                        parameters[43] = new ReportParameter("Param_TotalPenaltyFinancialAmount", "False");

                    if (reportHeads.Contains("LastIncrementDate"))
                        parameters[44] = new ReportParameter("Param_LastIncrementDate", "True");
                    else
                        parameters[44] = new ReportParameter("Param_LastIncrementDate", "False");

                    if (reportHeads.Contains("SkillType"))
                        parameters[45] = new ReportParameter("Param_SkillType", "True");
                    else
                        parameters[45] = new ReportParameter("Param_SkillType", "False");

                    DateTime paramFromDate = new DateTime(2017, 5, 26);
                    DateTime paramToDate = new DateTime(2017, 6, 25);

                    if (jobCardInfo[0].Unit == "Garments")
                    {
                        if (jobCardInfo[0].Date >= paramFromDate && jobCardInfo[0].Date <= paramToDate)
                            parameters[46] = new ReportParameter("Param_Note", "Lunch Break 30 Minutes");
                        else
                            parameters[46] = new ReportParameter("Param_Note", "Lunch Break 1 Hour");
                    }
                    else
                        parameters[46] = new ReportParameter("Param_Note", "10.30 to 11.00 Refreshment Break \n            04.30 to 05.00 Refreshment Break");

                    lr.SetParameters(parameters);

                    lr.DataSources.Add(rd);
                }

                string reportType = "pdf";
                if (model.SearchFieldModel.PrintFormatId == 2)
                    reportType = "Excel";

                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =
                    "<DeviceInfo>" +
                    "  <OutputFormat>" + 2 + "</OutputFormat>" +
                    "  <PageWidth>8.5in</PageWidth>" +
                    "  <PageHeight>11.7in</PageHeight>" +
                    "  <MarginTop>0.4in</MarginTop>" +
                    "  <MarginLeft>.4in</MarginLeft>" +
                    "  <MarginRight>.2in</MarginRight>" +
                    "  <MarginBottom>0.2in</MarginBottom>" +
                    "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;

                byte[] renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                return File(renderedBytes, mimeType);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return JobCardIndex(model);
        }

        public ActionResult JobCard10PMNoWeekendIndex(DynamicReportHeadViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [ActionName("Jobcarddne")]
        public ActionResult JobCard10PMNoWeekend(DynamicReportHeadViewModel model)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "Jobcarddne.rdlc");
                if (System.IO.File.Exists(path))
                    lr.ReportPath = path;
                else
                    return View("Index");

                lr.DisplayName = "Sample Title";

                int? companyId = model.SearchFieldModel.SearchByCompanyId;
                int? branchId = model.SearchFieldModel.SearchByBranchId;
                int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
                int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
                int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
                int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
                int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
                string employeeCardId = model.SearchFieldModel.SearchByEmployeeCardId;

                int year = Convert.ToInt32(model.SearchFieldModel.SelectedYear);
                int month = Convert.ToInt32(model.SearchFieldModel.SelectedMonth);

                DateTime? fromDate = model.SearchFieldModel.StartDate;
                DateTime? toDate = model.SearchFieldModel.EndDate;

                string userName = PortalContext.CurrentUser.Name;

                List<JobCardInfoModel> jobCardInfo = HrmReportManager.GetJobCardInfo10PMNoWeekend(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, userName);

                ReportDataSource rd = new ReportDataSource("DataSetEmployeeJobCard", jobCardInfo);

                ReportParameter[] parameters = new ReportParameter[47];

                List<string> reportHeads = model.ReportHeads;

                if (reportHeads != null)
                {
                    if (reportHeads.Contains("Branch"))
                        parameters[0] = new ReportParameter("Param_Branch", "True");
                    else
                        parameters[0] = new ReportParameter("Param_Branch", "False");

                    if (reportHeads.Contains("Unit"))
                        parameters[1] = new ReportParameter("Param_Unit", "True");
                    else
                        parameters[1] = new ReportParameter("Param_Unit", "False");

                    if (reportHeads.Contains("Department"))
                        parameters[2] = new ReportParameter("Param_Department", "True");
                    else
                        parameters[2] = new ReportParameter("Param_Department", "False");

                    if (reportHeads.Contains("Section"))
                        parameters[3] = new ReportParameter("Param_Section", "True");
                    else
                        parameters[3] = new ReportParameter("Param_Section", "False");

                    if (reportHeads.Contains("Line"))
                        parameters[4] = new ReportParameter("Param_Line", "True");
                    else
                        parameters[4] = new ReportParameter("Param_Line", "False");

                    if (reportHeads.Contains("EmployeeType"))
                        parameters[5] = new ReportParameter("Param_EmployeeType", "True");
                    else
                        parameters[5] = new ReportParameter("Param_EmployeeType", "False");

                    if (reportHeads.Contains("EmployeeGrade"))
                        parameters[6] = new ReportParameter("Param_EmployeeGrade", "True");
                    else
                        parameters[6] = new ReportParameter("Param_EmployeeGrade", "False");

                    if (reportHeads.Contains("EmployeeDesignation"))
                        parameters[7] = new ReportParameter("Param_EmployeeDesignation", "True");
                    else
                        parameters[7] = new ReportParameter("Param_EmployeeDesignation", "False");

                    if (reportHeads.Contains("MobileNo"))
                        parameters[8] = new ReportParameter("Param_MobileNo", "True");
                    else
                        parameters[8] = new ReportParameter("Param_MobileNo", "False");

                    if (reportHeads.Contains("JoiningDate"))
                        parameters[9] = new ReportParameter("Param_JoiningDate", "True");
                    else
                        parameters[9] = new ReportParameter("Param_JoiningDate", "False");

                    if (reportHeads.Contains("QuitDate"))
                        parameters[10] = new ReportParameter("Param_QuitDate", "True");
                    else
                        parameters[10] = new ReportParameter("Param_QuitDate", "False");

                    if (reportHeads.Contains("GrossSalary"))
                        parameters[11] = new ReportParameter("Param_GrossSalary", "True");
                    else
                        parameters[11] = new ReportParameter("Param_GrossSalary", "False");

                    if (reportHeads.Contains("OTRate"))
                        parameters[12] = new ReportParameter("Param_OTRate", "True");
                    else
                        parameters[12] = new ReportParameter("Param_OTRate", "False");

                    if (reportHeads.Contains("PresentDays"))
                        parameters[13] = new ReportParameter("Param_PresentDays", "True");
                    else
                        parameters[13] = new ReportParameter("Param_PresentDays", "False");

                    if (reportHeads.Contains("LateDays"))
                        parameters[14] = new ReportParameter("Param_LateDays", "True");
                    else
                        parameters[14] = new ReportParameter("Param_LateDays", "False");

                    if (reportHeads.Contains("OSDDays"))
                        parameters[15] = new ReportParameter("Param_OSDDays", "True");
                    else
                        parameters[15] = new ReportParameter("Param_OSDDays", "False");

                    if (reportHeads.Contains("AbsentDays"))
                        parameters[16] = new ReportParameter("Param_AbsentDays", "True");
                    else
                        parameters[16] = new ReportParameter("Param_AbsentDays", "False");

                    if (reportHeads.Contains("LeaveDays"))
                        parameters[17] = new ReportParameter("Param_LeaveDays", "True");
                    else
                        parameters[17] = new ReportParameter("Param_LeaveDays", "False");

                    if (reportHeads.Contains("Holidays"))
                        parameters[18] = new ReportParameter("Param_Holidays", "True");
                    else
                        parameters[18] = new ReportParameter("Param_Holidays", "False");

                    if (reportHeads.Contains("WeekendDays"))
                        parameters[19] = new ReportParameter("Param_WeekendDays", "True");
                    else
                        parameters[19] = new ReportParameter("Param_WeekendDays", "False");

                    if (reportHeads.Contains("LWP"))
                        parameters[20] = new ReportParameter("Param_LWP", "True");
                    else
                        parameters[20] = new ReportParameter("Param_LWP", "False");

                    if (reportHeads.Contains("TotalDays"))
                        parameters[21] = new ReportParameter("Param_TotalDays", "True");
                    else
                        parameters[21] = new ReportParameter("Param_TotalDays", "False");

                    if (reportHeads.Contains("WorkingDays"))
                        parameters[22] = new ReportParameter("Param_WorkingDays", "True");
                    else
                        parameters[22] = new ReportParameter("Param_WorkingDays", "False");

                    if (reportHeads.Contains("PayDays"))
                        parameters[23] = new ReportParameter("Param_PayDays", "True");
                    else
                        parameters[23] = new ReportParameter("Param_PayDays", "False");


                    if (reportHeads.Contains("OTHourLast"))
                        parameters[24] = new ReportParameter("Param_OTHourLast", "True");
                    else
                        parameters[24] = new ReportParameter("Param_OTHourLast", "False");


                    if (reportHeads.Contains("TotalExtraOTHours"))
                        parameters[25] = new ReportParameter("Param_TotalExtraOTHours", "True");
                    else
                        parameters[25] = new ReportParameter("Param_TotalExtraOTHours", "False");


                    if (reportHeads.Contains("TotalHolidayOTHours"))
                        parameters[26] = new ReportParameter("Param_TotalHolidayOTHours", "True");
                    else
                        parameters[26] = new ReportParameter("Param_TotalHolidayOTHours", "False");


                    if (reportHeads.Contains("TotalWeekendOTHours"))
                        parameters[27] = new ReportParameter("Param_TotalWeekendOTHours", "True");
                    else
                        parameters[27] = new ReportParameter("Param_TotalWeekendOTHours", "False");


                    if (reportHeads.Contains("Date"))
                        parameters[28] = new ReportParameter("Param_Date", "True");
                    else
                        parameters[28] = new ReportParameter("Param_Date", "False");


                    if (reportHeads.Contains("DayName"))
                        parameters[29] = new ReportParameter("Param_DayName", "True");
                    else
                        parameters[29] = new ReportParameter("Param_DayName", "False");


                    if (reportHeads.Contains("Shift"))
                        parameters[30] = new ReportParameter("Param_Shift", "True");
                    else
                        parameters[30] = new ReportParameter("Param_Shift", "False");


                    if (reportHeads.Contains("Status"))
                        parameters[31] = new ReportParameter("Param_Status", "True");
                    else
                        parameters[31] = new ReportParameter("Param_Status", "False");

                    if (reportHeads.Contains("InTime"))
                        parameters[32] = new ReportParameter("Param_InTime", "True");
                    else
                        parameters[32] = new ReportParameter("Param_InTime", "False");

                    if (reportHeads.Contains("OutTime"))
                        parameters[33] = new ReportParameter("Param_OutTime", "True");
                    else
                        parameters[33] = new ReportParameter("Param_OutTime", "False");

                    if (reportHeads.Contains("Delay"))
                        parameters[34] = new ReportParameter("Param_Delay", "True");
                    else
                        parameters[34] = new ReportParameter("Param_Delay", "False");

                    if (reportHeads.Contains("OTHours"))
                        parameters[35] = new ReportParameter("Param_OTHours", "True");
                    else
                        parameters[35] = new ReportParameter("Param_OTHours", "False");

                    if (reportHeads.Contains("ExtraOTHours"))
                        parameters[36] = new ReportParameter("Param_ExtraOTHours", "True");
                    else
                        parameters[36] = new ReportParameter("Param_ExtraOTHours", "False");

                    if (reportHeads.Contains("HolidayOTHours"))
                        parameters[37] = new ReportParameter("Param_HolidayOTHours", "True");
                    else
                        parameters[37] = new ReportParameter("Param_HolidayOTHours", "False");

                    if (reportHeads.Contains("WeekendOTHours"))
                        parameters[38] = new ReportParameter("Param_WeekendOTHours", "True");
                    else
                        parameters[38] = new ReportParameter("Param_WeekendOTHours", "False");

                    if (reportHeads.Contains("Remarks"))
                        parameters[39] = new ReportParameter("Param_Remarks", "True");
                    else
                        parameters[39] = new ReportParameter("Param_Remarks", "False");

                    if (reportHeads.Contains("TotalPenaltyOTHours"))
                        parameters[40] = new ReportParameter("Param_TotalPenaltyOTHours", "True");
                    else
                        parameters[40] = new ReportParameter("Param_TotalPenaltyOTHours", "False");

                    if (reportHeads.Contains("TotalPenaltyAttendanceDays"))
                        parameters[41] = new ReportParameter("Param_TotalPenaltyAttendanceDays", "True");
                    else
                        parameters[41] = new ReportParameter("Param_TotalPenaltyAttendanceDays", "False");

                    if (reportHeads.Contains("TotalPenaltyLeaveDays"))
                        parameters[42] = new ReportParameter("Param_TotalPenaltyLeaveDays", "True");
                    else
                        parameters[42] = new ReportParameter("Param_TotalPenaltyLeaveDays", "False");

                    if (reportHeads.Contains("TotalPenaltyFinancialAmount"))
                        parameters[43] = new ReportParameter("Param_TotalPenaltyFinancialAmount", "True");
                    else
                        parameters[43] = new ReportParameter("Param_TotalPenaltyFinancialAmount", "False");

                    if (reportHeads.Contains("LastIncrementDate"))
                        parameters[44] = new ReportParameter("Param_LastIncrementDate", "True");
                    else
                        parameters[44] = new ReportParameter("Param_LastIncrementDate", "False");

                    if (reportHeads.Contains("SkillType"))
                        parameters[45] = new ReportParameter("Param_SkillType", "True");
                    else
                        parameters[45] = new ReportParameter("Param_SkillType", "False");

                    DateTime paramFromDate = new DateTime(2017, 5, 26);
                    DateTime paramToDate = new DateTime(2017, 6, 25);

                    if (jobCardInfo[0].Unit == "Garments")
                    {
                        if (jobCardInfo[0].Date >= paramFromDate && jobCardInfo[0].Date <= paramToDate)
                            parameters[46] = new ReportParameter("Param_Note", "Lunch Break 30 Minutes");
                        else
                            parameters[46] = new ReportParameter("Param_Note", "Lunch Break 1 Hour");
                    }
                    else
                        parameters[46] = new ReportParameter("Param_Note", "10.30 to 11.00 Refreshment Break \n            04.30 to 05.00 Refreshment Break");

                    lr.SetParameters(parameters);

                    lr.DataSources.Add(rd);
                }

                string reportType = "pdf";
                if (model.SearchFieldModel.PrintFormatId == 2)
                    reportType = "Excel";

                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =
                    "<DeviceInfo>" +
                    "  <OutputFormat>" + 2 + "</OutputFormat>" +
                    "  <PageWidth>8.5in</PageWidth>" +
                    "  <PageHeight>11.7in</PageHeight>" +
                    "  <MarginTop>0.4in</MarginTop>" +
                    "  <MarginLeft>.4in</MarginLeft>" +
                    "  <MarginRight>.2in</MarginRight>" +
                    "  <MarginBottom>0.2in</MarginBottom>" +
                    "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;

                byte[] renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                return File(renderedBytes, mimeType);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return JobCardIndex(model);
        }

        public ActionResult JobCardOriginalNoWeekendIndex(DynamicReportHeadViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [ActionName("Jobcarddnw")]
        public ActionResult JobCardOriginalNoWeekend(DynamicReportHeadViewModel model)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "Jobcarddnw.rdlc");
                if (System.IO.File.Exists(path))
                    lr.ReportPath = path;
                else
                    return View("Index");

                lr.DisplayName = "Sample Title";

                int? companyId = model.SearchFieldModel.SearchByCompanyId;
                int? branchId = model.SearchFieldModel.SearchByBranchId;
                int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
                int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
                int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
                int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
                int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
                string employeeCardId = model.SearchFieldModel.SearchByEmployeeCardId;

                int year = Convert.ToInt32(model.SearchFieldModel.SelectedYear);
                int month = Convert.ToInt32(model.SearchFieldModel.SelectedMonth);

                DateTime? fromDate = model.SearchFieldModel.StartDate;
                DateTime? toDate = model.SearchFieldModel.EndDate;

                string userName = PortalContext.CurrentUser.Name;

                List<JobCardInfoModel> jobCardInfo = HrmReportManager.GetJobCardOriginalNoWeekend(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, userName);

                ReportDataSource rd = new ReportDataSource("DataSetEmployeeJobCard", jobCardInfo);

                ReportParameter[] parameters = new ReportParameter[47];

                List<string> reportHeads = model.ReportHeads;

                if (reportHeads != null)
                {
                    if (reportHeads.Contains("Branch"))
                        parameters[0] = new ReportParameter("Param_Branch", "True");
                    else
                        parameters[0] = new ReportParameter("Param_Branch", "False");

                    if (reportHeads.Contains("Unit"))
                        parameters[1] = new ReportParameter("Param_Unit", "True");
                    else
                        parameters[1] = new ReportParameter("Param_Unit", "False");

                    if (reportHeads.Contains("Department"))
                        parameters[2] = new ReportParameter("Param_Department", "True");
                    else
                        parameters[2] = new ReportParameter("Param_Department", "False");

                    if (reportHeads.Contains("Section"))
                        parameters[3] = new ReportParameter("Param_Section", "True");
                    else
                        parameters[3] = new ReportParameter("Param_Section", "False");

                    if (reportHeads.Contains("Line"))
                        parameters[4] = new ReportParameter("Param_Line", "True");
                    else
                        parameters[4] = new ReportParameter("Param_Line", "False");

                    if (reportHeads.Contains("EmployeeType"))
                        parameters[5] = new ReportParameter("Param_EmployeeType", "True");
                    else
                        parameters[5] = new ReportParameter("Param_EmployeeType", "False");

                    if (reportHeads.Contains("EmployeeGrade"))
                        parameters[6] = new ReportParameter("Param_EmployeeGrade", "True");
                    else
                        parameters[6] = new ReportParameter("Param_EmployeeGrade", "False");

                    if (reportHeads.Contains("EmployeeDesignation"))
                        parameters[7] = new ReportParameter("Param_EmployeeDesignation", "True");
                    else
                        parameters[7] = new ReportParameter("Param_EmployeeDesignation", "False");

                    if (reportHeads.Contains("MobileNo"))
                        parameters[8] = new ReportParameter("Param_MobileNo", "True");
                    else
                        parameters[8] = new ReportParameter("Param_MobileNo", "False");

                    if (reportHeads.Contains("JoiningDate"))
                        parameters[9] = new ReportParameter("Param_JoiningDate", "True");
                    else
                        parameters[9] = new ReportParameter("Param_JoiningDate", "False");

                    if (reportHeads.Contains("QuitDate"))
                        parameters[10] = new ReportParameter("Param_QuitDate", "True");
                    else
                        parameters[10] = new ReportParameter("Param_QuitDate", "False");

                    if (reportHeads.Contains("GrossSalary"))
                        parameters[11] = new ReportParameter("Param_GrossSalary", "True");
                    else
                        parameters[11] = new ReportParameter("Param_GrossSalary", "False");

                    if (reportHeads.Contains("OTRate"))
                        parameters[12] = new ReportParameter("Param_OTRate", "True");
                    else
                        parameters[12] = new ReportParameter("Param_OTRate", "False");

                    if (reportHeads.Contains("PresentDays"))
                        parameters[13] = new ReportParameter("Param_PresentDays", "True");
                    else
                        parameters[13] = new ReportParameter("Param_PresentDays", "False");

                    if (reportHeads.Contains("LateDays"))
                        parameters[14] = new ReportParameter("Param_LateDays", "True");
                    else
                        parameters[14] = new ReportParameter("Param_LateDays", "False");

                    if (reportHeads.Contains("OSDDays"))
                        parameters[15] = new ReportParameter("Param_OSDDays", "True");
                    else
                        parameters[15] = new ReportParameter("Param_OSDDays", "False");

                    if (reportHeads.Contains("AbsentDays"))
                        parameters[16] = new ReportParameter("Param_AbsentDays", "True");
                    else
                        parameters[16] = new ReportParameter("Param_AbsentDays", "False");

                    if (reportHeads.Contains("LeaveDays"))
                        parameters[17] = new ReportParameter("Param_LeaveDays", "True");
                    else
                        parameters[17] = new ReportParameter("Param_LeaveDays", "False");

                    if (reportHeads.Contains("Holidays"))
                        parameters[18] = new ReportParameter("Param_Holidays", "True");
                    else
                        parameters[18] = new ReportParameter("Param_Holidays", "False");

                    if (reportHeads.Contains("WeekendDays"))
                        parameters[19] = new ReportParameter("Param_WeekendDays", "True");
                    else
                        parameters[19] = new ReportParameter("Param_WeekendDays", "False");

                    if (reportHeads.Contains("LWP"))
                        parameters[20] = new ReportParameter("Param_LWP", "True");
                    else
                        parameters[20] = new ReportParameter("Param_LWP", "False");

                    if (reportHeads.Contains("TotalDays"))
                        parameters[21] = new ReportParameter("Param_TotalDays", "True");
                    else
                        parameters[21] = new ReportParameter("Param_TotalDays", "False");

                    if (reportHeads.Contains("WorkingDays"))
                        parameters[22] = new ReportParameter("Param_WorkingDays", "True");
                    else
                        parameters[22] = new ReportParameter("Param_WorkingDays", "False");

                    if (reportHeads.Contains("PayDays"))
                        parameters[23] = new ReportParameter("Param_PayDays", "True");
                    else
                        parameters[23] = new ReportParameter("Param_PayDays", "False");


                    if (reportHeads.Contains("OTHourLast"))
                        parameters[24] = new ReportParameter("Param_OTHourLast", "True");
                    else
                        parameters[24] = new ReportParameter("Param_OTHourLast", "False");


                    if (reportHeads.Contains("TotalExtraOTHours"))
                        parameters[25] = new ReportParameter("Param_TotalExtraOTHours", "True");
                    else
                        parameters[25] = new ReportParameter("Param_TotalExtraOTHours", "False");


                    if (reportHeads.Contains("TotalHolidayOTHours"))
                        parameters[26] = new ReportParameter("Param_TotalHolidayOTHours", "True");
                    else
                        parameters[26] = new ReportParameter("Param_TotalHolidayOTHours", "False");


                    if (reportHeads.Contains("TotalWeekendOTHours"))
                        parameters[27] = new ReportParameter("Param_TotalWeekendOTHours", "True");
                    else
                        parameters[27] = new ReportParameter("Param_TotalWeekendOTHours", "False");


                    if (reportHeads.Contains("Date"))
                        parameters[28] = new ReportParameter("Param_Date", "True");
                    else
                        parameters[28] = new ReportParameter("Param_Date", "False");


                    if (reportHeads.Contains("DayName"))
                        parameters[29] = new ReportParameter("Param_DayName", "True");
                    else
                        parameters[29] = new ReportParameter("Param_DayName", "False");


                    if (reportHeads.Contains("Shift"))
                        parameters[30] = new ReportParameter("Param_Shift", "True");
                    else
                        parameters[30] = new ReportParameter("Param_Shift", "False");


                    if (reportHeads.Contains("Status"))
                        parameters[31] = new ReportParameter("Param_Status", "True");
                    else
                        parameters[31] = new ReportParameter("Param_Status", "False");

                    if (reportHeads.Contains("InTime"))
                        parameters[32] = new ReportParameter("Param_InTime", "True");
                    else
                        parameters[32] = new ReportParameter("Param_InTime", "False");

                    if (reportHeads.Contains("OutTime"))
                        parameters[33] = new ReportParameter("Param_OutTime", "True");
                    else
                        parameters[33] = new ReportParameter("Param_OutTime", "False");

                    if (reportHeads.Contains("Delay"))
                        parameters[34] = new ReportParameter("Param_Delay", "True");
                    else
                        parameters[34] = new ReportParameter("Param_Delay", "False");

                    if (reportHeads.Contains("OTHours"))
                        parameters[35] = new ReportParameter("Param_OTHours", "True");
                    else
                        parameters[35] = new ReportParameter("Param_OTHours", "False");

                    if (reportHeads.Contains("ExtraOTHours"))
                        parameters[36] = new ReportParameter("Param_ExtraOTHours", "True");
                    else
                        parameters[36] = new ReportParameter("Param_ExtraOTHours", "False");

                    if (reportHeads.Contains("HolidayOTHours"))
                        parameters[37] = new ReportParameter("Param_HolidayOTHours", "True");
                    else
                        parameters[37] = new ReportParameter("Param_HolidayOTHours", "False");

                    if (reportHeads.Contains("WeekendOTHours"))
                        parameters[38] = new ReportParameter("Param_WeekendOTHours", "True");
                    else
                        parameters[38] = new ReportParameter("Param_WeekendOTHours", "False");

                    if (reportHeads.Contains("Remarks"))
                        parameters[39] = new ReportParameter("Param_Remarks", "True");
                    else
                        parameters[39] = new ReportParameter("Param_Remarks", "False");

                    if (reportHeads.Contains("TotalPenaltyOTHours"))
                        parameters[40] = new ReportParameter("Param_TotalPenaltyOTHours", "True");
                    else
                        parameters[40] = new ReportParameter("Param_TotalPenaltyOTHours", "False");

                    if (reportHeads.Contains("TotalPenaltyAttendanceDays"))
                        parameters[41] = new ReportParameter("Param_TotalPenaltyAttendanceDays", "True");
                    else
                        parameters[41] = new ReportParameter("Param_TotalPenaltyAttendanceDays", "False");

                    if (reportHeads.Contains("TotalPenaltyLeaveDays"))
                        parameters[42] = new ReportParameter("Param_TotalPenaltyLeaveDays", "True");
                    else
                        parameters[42] = new ReportParameter("Param_TotalPenaltyLeaveDays", "False");

                    if (reportHeads.Contains("TotalPenaltyFinancialAmount"))
                        parameters[43] = new ReportParameter("Param_TotalPenaltyFinancialAmount", "True");
                    else
                        parameters[43] = new ReportParameter("Param_TotalPenaltyFinancialAmount", "False");

                    if (reportHeads.Contains("LastIncrementDate"))
                        parameters[44] = new ReportParameter("Param_LastIncrementDate", "True");
                    else
                        parameters[44] = new ReportParameter("Param_LastIncrementDate", "False");

                    if (reportHeads.Contains("SkillType"))
                        parameters[45] = new ReportParameter("Param_SkillType", "True");
                    else
                        parameters[45] = new ReportParameter("Param_SkillType", "False");

                    DateTime paramFromDate = new DateTime(2017, 5, 26);
                    DateTime paramToDate = new DateTime(2017, 6, 25);

                    if (jobCardInfo[0].Unit == "Garments")
                    {
                        if (jobCardInfo[0].Date >= paramFromDate && jobCardInfo[0].Date <= paramToDate)
                            parameters[46] = new ReportParameter("Param_Note", "Lunch Break 30 Minutes");
                        else
                            parameters[46] = new ReportParameter("Param_Note", "Lunch Break 1 Hour");
                    }
                    else
                        parameters[46] = new ReportParameter("Param_Note", "10.30 to 11.00 Refreshment Break \n            04.30 to 05.00 Refreshment Break");

                    lr.SetParameters(parameters);

                    lr.DataSources.Add(rd);
                }

                string reportType = "pdf";
                if (model.SearchFieldModel.PrintFormatId == 2)
                    reportType = "Excel";

                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =
                    "<DeviceInfo>" +
                    "  <OutputFormat>" + 2 + "</OutputFormat>" +
                    "  <PageWidth>8.5in</PageWidth>" +
                    "  <PageHeight>11.7in</PageHeight>" +
                    "  <MarginTop>0.4in</MarginTop>" +
                    "  <MarginLeft>.4in</MarginLeft>" +
                    "  <MarginRight>.2in</MarginRight>" +
                    "  <MarginBottom>0.2in</MarginBottom>" +
                    "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;

                byte[] renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                return File(renderedBytes, mimeType);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return JobCardIndex(model);
        }

        //[AjaxAuthorize(Roles = "jobcard(production)-1,jobcard(production)-2,jobcard(production)-3")]
        public ActionResult JobCardProductionIndex(DynamicReportHeadViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        //[AjaxAuthorize(Roles = "jobcard(production)-1,jobcard(production)-2,jobcard(production)-3")]
        public ActionResult JobCardProduction(DynamicReportHeadViewModel model)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "JobCardProduction.rdlc");
                if (System.IO.File.Exists(path))
                    lr.ReportPath = path;
                else
                    return View("Index");

                int? companyId = model.SearchFieldModel.SearchByCompanyId;
                int? branchId = model.SearchFieldModel.SearchByBranchId;
                int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
                int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
                int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
                int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
                int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
                string employeeCardId = model.SearchFieldModel.SearchByEmployeeCardId;

                int year = Convert.ToInt32(model.SearchFieldModel.SelectedYear);
                int month = Convert.ToInt32(model.SearchFieldModel.SelectedMonth);

                DateTime? fromDate = model.SearchFieldModel.StartDate;
                DateTime? toDate = model.SearchFieldModel.EndDate;

                string userName = PortalContext.CurrentUser.Name;


                List<JobCardInfoModel> jobCardInfo = HrmReportManager.GetJobCardInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                    departmentLineId, employeeTypeId, employeeCardId, year, month,
                    fromDate, toDate, userName);


                ReportDataSource rd = new ReportDataSource("DataSetEmployeeJobCard", jobCardInfo);

                ReportParameter[] parameters = new ReportParameter[37];

                List<string> reportHeads = model.ReportHeads;

                if (reportHeads != null)
                {
                    if (reportHeads.Contains("Branch"))
                        parameters[0] = new ReportParameter("Param_Branch", "True");
                    else
                        parameters[0] = new ReportParameter("Param_Branch", "False");

                    if (reportHeads.Contains("Unit"))
                        parameters[1] = new ReportParameter("Param_Unit", "True");
                    else
                        parameters[1] = new ReportParameter("Param_Unit", "False");

                    if (reportHeads.Contains("Department"))
                        parameters[2] = new ReportParameter("Param_Department", "True");
                    else
                        parameters[2] = new ReportParameter("Param_Department", "False");

                    if (reportHeads.Contains("Section"))
                        parameters[3] = new ReportParameter("Param_Section", "True");
                    else
                        parameters[3] = new ReportParameter("Param_Section", "False");

                    if (reportHeads.Contains("Line"))
                        parameters[4] = new ReportParameter("Param_Line", "True");
                    else
                        parameters[4] = new ReportParameter("Param_Line", "False");

                    if (reportHeads.Contains("EmployeeType"))
                        parameters[5] = new ReportParameter("Param_EmployeeType", "True");
                    else
                        parameters[5] = new ReportParameter("Param_EmployeeType", "False");

                    if (reportHeads.Contains("EmployeeGrade"))
                        parameters[6] = new ReportParameter("Param_EmployeeGrade", "True");
                    else
                        parameters[6] = new ReportParameter("Param_EmployeeGrade", "False");

                    if (reportHeads.Contains("EmployeeDesignation"))
                        parameters[7] = new ReportParameter("Param_EmployeeDesignation", "True");
                    else
                        parameters[7] = new ReportParameter("Param_EmployeeDesignation", "False");

                    if (reportHeads.Contains("MobileNo"))
                        parameters[8] = new ReportParameter("Param_MobileNo", "True");
                    else
                        parameters[8] = new ReportParameter("Param_MobileNo", "False");

                    if (reportHeads.Contains("JoiningDate"))
                        parameters[9] = new ReportParameter("Param_JoiningDate", "True");
                    else
                        parameters[9] = new ReportParameter("Param_JoiningDate", "False");

                    if (reportHeads.Contains("QuitDate"))
                        parameters[10] = new ReportParameter("Param_QuitDate", "True");
                    else
                        parameters[10] = new ReportParameter("Param_QuitDate", "False");

                    if (reportHeads.Contains("GrossSalary"))
                        parameters[11] = new ReportParameter("Param_GrossSalary", "True");
                    else
                        parameters[11] = new ReportParameter("Param_GrossSalary", "False");

                    if (reportHeads.Contains("PresentDays"))
                        parameters[12] = new ReportParameter("Param_PresentDays", "True");
                    else
                        parameters[12] = new ReportParameter("Param_PresentDays", "False");

                    if (reportHeads.Contains("LateDays"))
                        parameters[13] = new ReportParameter("Param_LateDays", "True");
                    else
                        parameters[13] = new ReportParameter("Param_LateDays", "False");

                    if (reportHeads.Contains("OSDDays"))
                        parameters[14] = new ReportParameter("Param_OSDDays", "True");
                    else
                        parameters[14] = new ReportParameter("Param_OSDDays", "False");

                    if (reportHeads.Contains("AbsentDays"))
                        parameters[15] = new ReportParameter("Param_AbsentDays", "True");
                    else
                        parameters[15] = new ReportParameter("Param_AbsentDays", "False");

                    if (reportHeads.Contains("LeaveDays"))
                        parameters[16] = new ReportParameter("Param_LeaveDays", "True");
                    else
                        parameters[16] = new ReportParameter("Param_LeaveDays", "False");

                    if (reportHeads.Contains("Holidays"))
                        parameters[17] = new ReportParameter("Param_Holidays", "True");
                    else
                        parameters[17] = new ReportParameter("Param_Holidays", "False");

                    if (reportHeads.Contains("WeekendDays"))
                        parameters[18] = new ReportParameter("Param_WeekendDays", "True");
                    else
                        parameters[18] = new ReportParameter("Param_WeekendDays", "False");

                    if (reportHeads.Contains("LWP"))
                        parameters[19] = new ReportParameter("Param_LWP", "True");
                    else
                        parameters[19] = new ReportParameter("Param_LWP", "False");

                    if (reportHeads.Contains("TotalDays"))
                        parameters[20] = new ReportParameter("Param_TotalDays", "True");
                    else
                        parameters[20] = new ReportParameter("Param_TotalDays", "False");

                    if (reportHeads.Contains("WorkingDays"))
                        parameters[21] = new ReportParameter("Param_WorkingDays", "True");
                    else
                        parameters[21] = new ReportParameter("Param_WorkingDays", "False");

                    if (reportHeads.Contains("PayDays"))
                        parameters[22] = new ReportParameter("Param_PayDays", "True");
                    else
                        parameters[22] = new ReportParameter("Param_PayDays", "False");


                    if (reportHeads.Contains("OTHourLast"))
                        parameters[23] = new ReportParameter("Param_OTHourLast", "True");
                    else
                        parameters[23] = new ReportParameter("Param_OTHourLast", "False");


                    if (reportHeads.Contains("TotalExtraOTHours"))
                        parameters[24] = new ReportParameter("Param_TotalExtraOTHours", "True");
                    else
                        parameters[24] = new ReportParameter("Param_TotalExtraOTHours", "False");


                    if (reportHeads.Contains("TotalWeekendOTHours"))
                        parameters[25] = new ReportParameter("Param_TotalWeekendOTHours", "True");
                    else
                        parameters[25] = new ReportParameter("Param_TotalWeekendOTHours", "False");


                    if (reportHeads.Contains("Date"))
                        parameters[26] = new ReportParameter("Param_Date", "True");
                    else
                        parameters[26] = new ReportParameter("Param_Date", "False");


                    if (reportHeads.Contains("DayName"))
                        parameters[27] = new ReportParameter("Param_DayName", "True");
                    else
                        parameters[27] = new ReportParameter("Param_DayName", "False");


                    if (reportHeads.Contains("Shift"))
                        parameters[28] = new ReportParameter("Param_Shift", "True");
                    else
                        parameters[28] = new ReportParameter("Param_Shift", "False");


                    if (reportHeads.Contains("Status"))
                        parameters[29] = new ReportParameter("Param_Status", "True");
                    else
                        parameters[29] = new ReportParameter("Param_Status", "False");

                    if (reportHeads.Contains("InTime"))
                        parameters[30] = new ReportParameter("Param_InTime", "True");
                    else
                        parameters[30] = new ReportParameter("Param_InTime", "False");

                    if (reportHeads.Contains("OutTime"))
                        parameters[31] = new ReportParameter("Param_OutTime", "True");
                    else
                        parameters[31] = new ReportParameter("Param_OutTime", "False");

                    if (reportHeads.Contains("Delay"))
                        parameters[32] = new ReportParameter("Param_Delay", "True");
                    else
                        parameters[32] = new ReportParameter("Param_Delay", "False");

                    if (reportHeads.Contains("OTHours"))
                        parameters[33] = new ReportParameter("Param_OTHours", "True");
                    else
                        parameters[33] = new ReportParameter("Param_OTHours", "False");

                    if (reportHeads.Contains("ExtraOTHours"))
                        parameters[34] = new ReportParameter("Param_ExtraOTHours", "True");
                    else
                        parameters[34] = new ReportParameter("Param_ExtraOTHours", "False");

                    if (reportHeads.Contains("WeekendOTHours"))
                        parameters[35] = new ReportParameter("Param_WeekendOTHours", "True");
                    else
                        parameters[35] = new ReportParameter("Param_WeekendOTHours", "False");

                    if (reportHeads.Contains("Remarks"))
                        parameters[36] = new ReportParameter("Param_Remarks", "True");
                    else
                        parameters[36] = new ReportParameter("Param_Remarks", "False");


                    lr.SetParameters(parameters);

                    lr.DataSources.Add(rd);
                }

                string reportType = "pdf";
                if (model.SearchFieldModel.PrintFormatId == 2)
                    reportType = "Excel";

                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =
                    "<DeviceInfo>" +
                    "  <OutputFormat>" + 2 + "</OutputFormat>" +
                    "  <PageWidth>8.5in</PageWidth>" +
                    "  <PageHeight>11.7in</PageHeight>" +
                    "  <MarginTop>0.4in</MarginTop>" +
                    "  <MarginLeft>.4in</MarginLeft>" +
                    "  <MarginRight>.2in</MarginRight>" +
                    "  <MarginBottom>0.2in</MarginBottom>" +
                    "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;

                byte[] renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                return File(renderedBytes, mimeType);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return JobCardIndex(model);

        }

        //[AjaxAuthorize(Roles = "jobcard-1,jobcard-2,jobcard-3")]
        public ActionResult JobCardModelIndex(DynamicReportHeadViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        //[AjaxAuthorize(Roles = "jobcard-1,jobcard-2,jobcard-3")]
        public ActionResult JobCardModel(DynamicReportHeadViewModel model)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "EmployeeJobCard.rdlc");
                if (System.IO.File.Exists(path))
                    lr.ReportPath = path;
                else
                    return View("Index");

                int? companyId = model.SearchFieldModel.SearchByCompanyId;
                int? branchId = model.SearchFieldModel.SearchByBranchId;
                int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
                int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
                int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
                int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
                int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
                string employeeCardId = model.SearchFieldModel.SearchByEmployeeCardId;

                int year = Convert.ToInt32(model.SearchFieldModel.SelectedYear);
                int month = Convert.ToInt32(model.SearchFieldModel.SelectedMonth);

                DateTime? fromDate = model.SearchFieldModel.StartDate;
                DateTime? toDate = model.SearchFieldModel.EndDate;

                string userName = PortalContext.CurrentUser.Name;


                List<JobCardInfoModel> jobCardModelInfo = HrmReportManager.GetJobCardModelInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                    departmentLineId, employeeTypeId, employeeCardId, year, month,
                    fromDate, toDate, userName);


                ReportDataSource rd = new ReportDataSource("DataSetEmployeeJobCardModel", jobCardModelInfo);
                lr.DataSources.Add(rd);

                ReportParameter[] parameters = new ReportParameter[36];

                List<string> reportHeads = model.ReportHeads;

                if (reportHeads != null)
                {

                    if (reportHeads.Contains("AbsentDays"))
                        parameters[0] = new ReportParameter("Param_AbsentDays", "True");
                    else
                        parameters[0] = new ReportParameter("Param_AbsentDays", "False");

                    if (reportHeads.Contains("Branch"))
                        parameters[1] = new ReportParameter("Param_Branch", "True");
                    else
                        parameters[1] = new ReportParameter("Param_Branch", "False");

                    if (reportHeads.Contains("Date"))
                        parameters[2] = new ReportParameter("Param_Date", "True");
                    else
                        parameters[2] = new ReportParameter("Param_Date", "False");

                    if (reportHeads.Contains("DayName"))
                        parameters[3] = new ReportParameter("Param_DayName", "True");
                    else
                        parameters[3] = new ReportParameter("Param_DayName", "False");

                    if (reportHeads.Contains("Delay"))
                        parameters[4] = new ReportParameter("Param_Delay", "True");
                    else
                        parameters[4] = new ReportParameter("Param_Delay", "False");

                    if (reportHeads.Contains("Department"))
                        parameters[5] = new ReportParameter("Param_Department", "True");
                    else
                        parameters[5] = new ReportParameter("Param_Department", "False");

                    if (reportHeads.Contains("EmployeeCardId"))
                        parameters[6] = new ReportParameter("Param_EmployeeCardId", "True");
                    else
                        parameters[6] = new ReportParameter("Param_EmployeeCardId", "False");

                    if (reportHeads.Contains("EmployeeDesignation"))
                        parameters[7] = new ReportParameter("Param_EmployeeDesignation", "True");
                    else
                        parameters[7] = new ReportParameter("Param_EmployeeDesignation", "False");

                    if (reportHeads.Contains("EmployeeGrade"))
                        parameters[8] = new ReportParameter("Param_EmployeeGrade", "True");
                    else
                        parameters[8] = new ReportParameter("Param_EmployeeGrade", "False");

                    if (reportHeads.Contains("EmployeeType"))
                        parameters[9] = new ReportParameter("Param_EmployeeType", "True");
                    else
                        parameters[9] = new ReportParameter("Param_EmployeeType", "False");

                    if (reportHeads.Contains("GrossSalary"))
                        parameters[10] = new ReportParameter("Param_GrossSalary", "True");
                    else
                        parameters[10] = new ReportParameter("Param_GrossSalary", "False");

                    if (reportHeads.Contains("Holidays"))
                        parameters[11] = new ReportParameter("Param_Holidays", "True");
                    else
                        parameters[11] = new ReportParameter("Param_Holidays", "False");

                    if (reportHeads.Contains("InTime"))
                        parameters[12] = new ReportParameter("Param_InTime", "True");
                    else
                        parameters[12] = new ReportParameter("Param_InTime", "False");

                    if (reportHeads.Contains("JoiningDate"))
                        parameters[13] = new ReportParameter("Param_JoiningDate", "True");
                    else
                        parameters[13] = new ReportParameter("Param_JoiningDate", "False");

                    if (reportHeads.Contains("LateDays"))
                        parameters[14] = new ReportParameter("Param_LateDays", "True");
                    else
                        parameters[14] = new ReportParameter("Param_LateDays", "False");

                    if (reportHeads.Contains("LeaveDays"))
                        parameters[15] = new ReportParameter("Param_LeaveDays", "True");
                    else
                        parameters[15] = new ReportParameter("Param_LeaveDays", "False");

                    if (reportHeads.Contains("Line"))
                        parameters[16] = new ReportParameter("Param_Line", "True");
                    else
                        parameters[16] = new ReportParameter("Param_Line", "False");

                    if (reportHeads.Contains("LWP"))
                        parameters[17] = new ReportParameter("Param_LWP", "True");
                    else
                        parameters[17] = new ReportParameter("Param_LWP", "False");

                    if (reportHeads.Contains("MobileNo"))
                        parameters[18] = new ReportParameter("Param_MobileNo", "True");
                    else
                        parameters[18] = new ReportParameter("Param_MobileNo", "False");

                    if (reportHeads.Contains("Name"))
                        parameters[19] = new ReportParameter("Param_Name", "True");
                    else
                        parameters[19] = new ReportParameter("Param_Name", "False");

                    if (reportHeads.Contains("OSDDays"))
                        parameters[20] = new ReportParameter("Param_OSDDays", "True");
                    else
                        parameters[20] = new ReportParameter("Param_OSDDays", "False");

                    if (reportHeads.Contains("OTHours"))
                        parameters[21] = new ReportParameter("Param_OTHours", "True");
                    else
                        parameters[21] = new ReportParameter("Param_OTHours", "False");

                    if (reportHeads.Contains("OTRate"))
                        parameters[22] = new ReportParameter("Param_OTRate", "True");
                    else
                        parameters[22] = new ReportParameter("Param_OTRate", "False");

                    if (reportHeads.Contains("OutTime"))
                        parameters[23] = new ReportParameter("Param_OutTime", "True");
                    else
                        parameters[23] = new ReportParameter("Param_OutTime", "False");

                    if (reportHeads.Contains("PayDays"))
                        parameters[24] = new ReportParameter("Param_PayDays", "True");
                    else
                        parameters[24] = new ReportParameter("Param_PayDays", "False");

                    if (reportHeads.Contains("PresentDays"))
                        parameters[25] = new ReportParameter("Param_PresentDays", "True");
                    else
                        parameters[25] = new ReportParameter("Param_PresentDays", "False");

                    if (reportHeads.Contains("QuitDate"))
                        parameters[26] = new ReportParameter("Param_QuitDate", "True");
                    else
                        parameters[26] = new ReportParameter("Param_QuitDate", "False");

                    if (reportHeads.Contains("Remarks"))
                        parameters[27] = new ReportParameter("Param_Remarks", "True");
                    else
                        parameters[27] = new ReportParameter("Param_Remarks", "False");

                    if (reportHeads.Contains("Section"))
                        parameters[28] = new ReportParameter("Param_Section", "True");
                    else
                        parameters[28] = new ReportParameter("Param_Section", "False");

                    if (reportHeads.Contains("Shift"))
                        parameters[29] = new ReportParameter("Param_Shift", "True");
                    else
                        parameters[29] = new ReportParameter("Param_Shift", "False");

                    if (reportHeads.Contains("Status"))
                        parameters[30] = new ReportParameter("Param_Status", "True");
                    else
                        parameters[30] = new ReportParameter("Param_Status", "False");

                    if (reportHeads.Contains("TotalDays"))
                        parameters[31] = new ReportParameter("Param_TotalDays", "True");
                    else
                        parameters[31] = new ReportParameter("Param_TotalDays", "False");

                    if (reportHeads.Contains("OTHourLast"))
                        parameters[32] = new ReportParameter("Param_OTHourLast", "True");
                    else
                        parameters[32] = new ReportParameter("Param_OTHourLast", "False");

                    if (reportHeads.Contains("Unit"))
                        parameters[33] = new ReportParameter("Param_Unit", "True");
                    else
                        parameters[33] = new ReportParameter("Param_Unit", "False");

                    if (reportHeads.Contains("WeekendDays"))
                        parameters[34] = new ReportParameter("Param_WeekendDays", "True");
                    else
                        parameters[34] = new ReportParameter("Param_WeekendDays", "False");

                    if (reportHeads.Contains("WorkingDays"))
                        parameters[35] = new ReportParameter("Param_WorkingDays", "True");
                    else
                        parameters[35] = new ReportParameter("Param_WorkingDays", "False");


                    lr.SetParameters(parameters);

                    lr.DataSources.Add(rd);
                }


                string reportType = "pdf";
                if (model.SearchFieldModel.PrintFormatId == 2)
                    reportType = "Excel";

                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =
                    "<DeviceInfo>" +
                    "  <OutputFormat>" + 2 + "</OutputFormat>" +
                    "  <PageWidth>8.5in</PageWidth>" +
                    "  <PageHeight>11.7in</PageHeight>" +
                    "  <MarginTop>0.4in</MarginTop>" +
                    "  <MarginLeft>.4in</MarginLeft>" +
                    "  <MarginRight>.2in</MarginRight>" +
                    "  <MarginBottom>0.2in</MarginBottom>" +
                    "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;

                byte[] renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                return File(renderedBytes, mimeType);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return JobCardModelIndex(model);

        }

        [AjaxAuthorize(Roles = "getpass(all)-1,getpass(all)-2,getpass(all)-3")]
        public ActionResult ShortLeaveSummaryIndex(ShortLeaveReportViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "getpass(all)-1,getpass(all)-2,getpass(all)-3")]
        public ActionResult ShortLeaveSummary(int branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, string employeeCardId, DateTime startDate, DateTime endDate)
        {
            List<ShortLeaveSummaryModel> shortLeaveSummary = new List<ShortLeaveSummaryModel>();
            shortLeaveSummary = HrmReportManager.GetShortLeaveSummary(branchUnitDepartmentId, departmentSectionId,
                departmentLineId, employeeCardId, startDate, endDate);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "ShortLeaveSummary.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("DataSet1", shortLeaveSummary);
            lr.DataSources.Add(rd);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>8.3in</PageWidth>" +
                "  <PageHeight>11.7in</PageHeight>" +
                "  <MarginTop>0..2in</MarginTop>" +
                "  <MarginLeft>.4in</MarginLeft>" +
                "  <MarginRight>.2in</MarginRight>" +
                "  <MarginBottom>0.2in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);

        }

        [AjaxAuthorize(Roles = "getpass(individual)-1,getpass(individual)-2,getpass(individual)-3")]
        public ActionResult ShortLeaveDetailIndex(ShortLeaveDetailReportViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchByBranchUnitDepartmentId);
                var reasonList = from ReasonType reasonType in Enum.GetValues(typeof(ReasonType))
                                 select new { Id = (int)reasonType, Name = reasonType.ToString() };

                model.ReasonTypes = reasonList;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "getpass(individual)-1,getpass(individual)-2,getpass(individual)-3")]
        public ActionResult ShortLeaveDetail(int? companyId, int? branchId,
            int? branchUnitId, int? branchUnitDepartmentId, int? departmentSectionId,
            int? departmentLineId, string employeeCardId, DateTime fromDate, DateTime toDate, byte? reasonType)
        {
            Guid employeeId = Guid.Empty;
            var employee = new Employee();

            if (!string.IsNullOrEmpty(employeeCardId))
                employee = EmployeeManager.GetEmployeeByCardId(employeeCardId);

            if (employee.Id > 0)
                employeeId = employee.EmployeeId;

            List<ShortLeaveDetailModel> shortLeaveDetailModel = new List<ShortLeaveDetailModel>();
            shortLeaveDetailModel = HrmReportManager.GetShortLeaveDetail(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                departmentLineId, employeeId, fromDate, toDate, reasonType);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "ShortLeaveDetail.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");
            ReportDataSource rd = new ReportDataSource("DataSet1", shortLeaveDetailModel);
            lr.DataSources.Add(rd);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>8.3in</PageWidth>" +
                "  <PageHeight>11.7in</PageHeight>" +
                "  <MarginTop>0..2in</MarginTop>" +
                "  <MarginLeft>.4in</MarginLeft>" +
                "  <MarginRight>.2in</MarginRight>" +
                "  <MarginBottom>0.2in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        [AjaxAuthorize(Roles = "attendance-1,attendance-2,attendance-3")]
        public ActionResult AttendanceIndex(AttendanceViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.WorkShifts = BranchUnitWorkShiftManager.GetBranchUnitWorkShiftByBrancUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.StartDate = DateTime.Now;
                model.EndDate = DateTime.Now;

                var attendanceStatus = from AttendanceStatus attStatus in Enum.GetValues(typeof(AttendanceStatus))
                                       select new { Id = (int)attStatus, Name = attStatus.ToString() };

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

                model.AttendanceStatuses = attendanceStatus;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "attendance-1,attendance-2,attendance-3")]
        public ActionResult Attendance(AttendanceViewModel model)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "Attendance.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            string employeeCardId = model.SearchByEmployeeCardId;
            int? branchUnitWorkShiftId = model.SearchFieldModel.SearchByBranchUnitWorkShiftId;
            DateTime? fromDate = model.StartDate;
            DateTime? toDate = model.EndDate;
            string attendanceStatus = Enum.GetName(typeof(AttendanceStatus), model.AttendanceStatus);
            int totalContinuousAbsentDays = Convert.ToInt16(model.TotalContinuousAbsentDays);
            bool otEnabled = model.SearchByOTEligibilty;
            bool extraOTEnabled = model.SearchByExtraOTEligibilty;
            bool weekendOTEnabled = model.SearchByWeekendOTEligibilty;


            List<AttendanceModel> attendance = HrmReportManager.GetEmployeeAttendanceInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId,
                employeeTypeId, employeeCardId, branchUnitWorkShiftId, fromDate, toDate, attendanceStatus,
                totalContinuousAbsentDays, otEnabled, extraOTEnabled, weekendOTEnabled);


            ReportDataSource rd = new ReportDataSource("DataSetEmployeeDailyAttendance", attendance);

            ReportParameter[] parameters = new ReportParameter[25];

            List<string> reportHeads = model.ReportHeads;

            if (reportHeads != null)
            {
                if (reportHeads.Contains("Date"))
                    parameters[0] = new ReportParameter("ReportParameter_Date", "True");
                else
                    parameters[0] = new ReportParameter("ReportParameter_Date", "False");

                if (reportHeads.Contains("EmployeeCardId"))
                    parameters[1] = new ReportParameter("ReportParameter_EmployeeCardId", "True");
                else
                    parameters[1] = new ReportParameter("ReportParameter_EmployeeCardId", "False");

                if (reportHeads.Contains("EmployeeName"))
                    parameters[2] = new ReportParameter("ReportParameter_EmployeeName", "True");
                else
                    parameters[2] = new ReportParameter("ReportParameter_EmployeeName", "False");

                if (reportHeads.Contains("MobileNo"))
                    parameters[3] = new ReportParameter("ReportParameter_MobileNo", "True");
                else
                    parameters[3] = new ReportParameter("ReportParameter_MobileNo", "False");

                if (reportHeads.Contains("JoiningDate"))
                    parameters[4] = new ReportParameter("ReportParameter_JoiningDate", "True");
                else
                    parameters[4] = new ReportParameter("ReportParameter_JoiningDate", "False");

                if (reportHeads.Contains("Section"))
                    parameters[5] = new ReportParameter("ReportParameter_Section", "True");
                else
                    parameters[5] = new ReportParameter("ReportParameter_Section", "False");

                if (reportHeads.Contains("Line"))
                    parameters[6] = new ReportParameter("ReportParameter_Line", "True");
                else
                    parameters[6] = new ReportParameter("ReportParameter_Line", "False");

                if (reportHeads.Contains("EmployeeType"))
                    parameters[7] = new ReportParameter("ReportParameter_EmployeeType", "True");
                else
                    parameters[7] = new ReportParameter("ReportParameter_EmployeeType", "False");

                if (reportHeads.Contains("EmployeeDesignation"))
                    parameters[8] = new ReportParameter("ReportParameter_EmployeeDesignation", "True");
                else
                    parameters[8] = new ReportParameter("ReportParameter_EmployeeDesignation", "False");

                if (reportHeads.Contains("WorkShiftName"))
                    parameters[9] = new ReportParameter("ReportParameter_WorkShiftName", "True");
                else
                    parameters[9] = new ReportParameter("ReportParameter_WorkShiftName", "False");

                if (reportHeads.Contains("InTime"))
                    parameters[10] = new ReportParameter("ReportParameter_InTime", "True");
                else
                    parameters[10] = new ReportParameter("ReportParameter_InTime", "False");

                if (reportHeads.Contains("OutTime"))
                    parameters[11] = new ReportParameter("ReportParameter_OutTime", "True");
                else
                    parameters[11] = new ReportParameter("ReportParameter_OutTime", "False");

                if (reportHeads.Contains("Status"))
                    parameters[12] = new ReportParameter("ReportParameter_Status", "True");
                else
                    parameters[12] = new ReportParameter("ReportParameter_Status", "False");

                if (reportHeads.Contains("LateTime"))
                    parameters[13] = new ReportParameter("ReportParameter_LateTime", "True");
                else
                    parameters[13] = new ReportParameter("ReportParameter_LateTime", "False");

                if (reportHeads.Contains("LastDayOutTime"))
                    parameters[14] = new ReportParameter("ReportParameter_LastDayOutTime", "True");
                else
                    parameters[14] = new ReportParameter("ReportParameter_LastDayOutTime", "False");

                if (reportHeads.Contains("TotalContinuousAbsentDays"))
                    parameters[15] = new ReportParameter("ReportParameter_TotalContinuousAbsentDays", "True");
                else
                    parameters[15] = new ReportParameter("ReportParameter_TotalContinuousAbsentDays", "False");

                if (reportHeads.Contains("OTHours"))
                    parameters[16] = new ReportParameter("ReportParameter_OTHours", "True");
                else
                    parameters[16] = new ReportParameter("ReportParameter_OTHours", "False");

                if (reportHeads.Contains("LastDayOTHours"))
                    parameters[17] = new ReportParameter("ReportParameter_LastDayOTHours", "True");
                else
                    parameters[17] = new ReportParameter("ReportParameter_LastDayOTHours", "False");

                if (reportHeads.Contains("ExtraOTHours"))
                    parameters[18] = new ReportParameter("ReportParameter_ExtraOTHours", "True");
                else
                    parameters[18] = new ReportParameter("ReportParameter_ExtraOTHours", "False");

                if (reportHeads.Contains("LastDayExtraOTHours"))
                    parameters[19] = new ReportParameter("ReportParameter_LastDayExtraOTHours", "True");
                else
                    parameters[19] = new ReportParameter("ReportParameter_LastDayExtraOTHours", "False");

                if (reportHeads.Contains("WeekendOTHours"))
                    parameters[20] = new ReportParameter("ReportParameter_WeekendOTHours", "True");
                else
                    parameters[20] = new ReportParameter("ReportParameter_WeekendOTHours", "False");

                if (reportHeads.Contains("HolidayOTHours"))
                    parameters[21] = new ReportParameter("ReportParameter_HolidayOTHours", "True");
                else
                    parameters[21] = new ReportParameter("ReportParameter_HolidayOTHours", "False");


                if (reportHeads.Contains("Remarks"))
                    parameters[22] = new ReportParameter("ReportParameter_Remarks", "True");
                else
                    parameters[22] = new ReportParameter("ReportParameter_Remarks", "False");


                if (reportHeads.Contains("SignatureOfEmployee"))
                    parameters[23] = new ReportParameter("ReportParameter_SignatureOfEmployee", "True");
                else
                    parameters[23] = new ReportParameter("ReportParameter_SignatureOfEmployee", "False");

                if (reportHeads.Contains("LastPresentDate"))
                    parameters[24] = new ReportParameter("ReportParameter_LastPresentDate", "True");
                else
                    parameters[24] = new ReportParameter("ReportParameter_LastPresentDate", "False");

                lr.SetParameters(parameters);

                lr.DataSources.Add(rd);
            }


            string reportType = "pdf";
            if (model.SearchFieldModel.PrintFormatId == 2)
                reportType = "Excel";

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0..0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        //[AjaxAuthorize(Roles = "attendance-1,attendance-2,attendance-3")]
        public ActionResult AttendanceModelIndex(AttendanceViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.WorkShifts = BranchUnitWorkShiftManager.GetBranchUnitWorkShiftByBrancUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.StartDate = DateTime.Now;
                model.EndDate = DateTime.Now;

                var attendanceStatus = from AttendanceStatus attStatus in Enum.GetValues(typeof(AttendanceStatus))
                                       select new { Id = (int)attStatus, Name = attStatus.ToString() };

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

                model.AttendanceStatuses = attendanceStatus;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        //[AjaxAuthorize(Roles = "attendance-1,attendance-2,attendance-3")]
        public ActionResult AttendanceModel(AttendanceViewModel model)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "EmployeeAttendance.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            string employeeCardId = model.SearchByEmployeeCardId;
            int? branchUnitWorkShiftId = model.SearchFieldModel.SearchByBranchUnitWorkShiftId;
            DateTime? fromDate = model.StartDate;
            DateTime? toDate = model.EndDate;
            string attendanceStatus = Enum.GetName(typeof(AttendanceStatus), model.AttendanceStatus);
            int totalContinuousAbsentDays = Convert.ToInt16(model.TotalContinuousAbsentDays);
            bool otEnabled = model.SearchByOTEligibilty;


            List<AttendanceModel> attendanceModelInfo = HrmReportManager.GetEmployeeAttendanceModelInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId,
                employeeTypeId, employeeCardId, branchUnitWorkShiftId, fromDate, toDate, attendanceStatus,
                totalContinuousAbsentDays, otEnabled);


            ReportDataSource rd = new ReportDataSource("DataSetEmployeeDailyAttendanceModelInfo", attendanceModelInfo);

            ReportParameter[] parameters = new ReportParameter[20];

            List<string> modelReportHeads = model.ModelReportHeads;

            if (modelReportHeads != null)
            {
                if (modelReportHeads.Contains("Date"))
                    parameters[0] = new ReportParameter("ReportParameter_Date", "True");
                else
                    parameters[0] = new ReportParameter("ReportParameter_Date", "False");

                if (modelReportHeads.Contains("EmployeeCardId"))
                    parameters[1] = new ReportParameter("ReportParameter_EmployeeCardId", "True");
                else
                    parameters[1] = new ReportParameter("ReportParameter_EmployeeCardId", "False");

                if (modelReportHeads.Contains("EmployeeName"))
                    parameters[2] = new ReportParameter("ReportParameter_EmployeeName", "True");
                else
                    parameters[2] = new ReportParameter("ReportParameter_EmployeeName", "False");

                if (modelReportHeads.Contains("MobileNo"))
                    parameters[3] = new ReportParameter("ReportParameter_MobileNo", "True");
                else
                    parameters[3] = new ReportParameter("ReportParameter_MobileNo", "False");

                if (modelReportHeads.Contains("JoiningDate"))
                    parameters[4] = new ReportParameter("ReportParameter_JoiningDate", "True");
                else
                    parameters[4] = new ReportParameter("ReportParameter_JoiningDate", "False");

                if (modelReportHeads.Contains("Section"))
                    parameters[5] = new ReportParameter("ReportParameter_Section", "True");
                else
                    parameters[5] = new ReportParameter("ReportParameter_Section", "False");

                if (modelReportHeads.Contains("Line"))
                    parameters[6] = new ReportParameter("ReportParameter_Line", "True");
                else
                    parameters[6] = new ReportParameter("ReportParameter_Line", "False");

                if (modelReportHeads.Contains("EmployeeType"))
                    parameters[7] = new ReportParameter("ReportParameter_EmployeeType", "True");
                else
                    parameters[7] = new ReportParameter("ReportParameter_EmployeeType", "False");

                if (modelReportHeads.Contains("EmployeeDesignation"))
                    parameters[8] = new ReportParameter("ReportParameter_EmployeeDesignation", "True");
                else
                    parameters[8] = new ReportParameter("ReportParameter_EmployeeDesignation", "False");

                if (modelReportHeads.Contains("WorkShiftName"))
                    parameters[9] = new ReportParameter("ReportParameter_WorkShiftName", "True");
                else
                    parameters[9] = new ReportParameter("ReportParameter_WorkShiftName", "False");

                if (modelReportHeads.Contains("InTime"))
                    parameters[10] = new ReportParameter("ReportParameter_InTime", "True");
                else
                    parameters[10] = new ReportParameter("ReportParameter_InTime", "False");

                if (modelReportHeads.Contains("OutTime"))
                    parameters[11] = new ReportParameter("ReportParameter_OutTime", "True");
                else
                    parameters[11] = new ReportParameter("ReportParameter_OutTime", "False");

                if (modelReportHeads.Contains("Status"))
                    parameters[12] = new ReportParameter("ReportParameter_Status", "True");
                else
                    parameters[12] = new ReportParameter("ReportParameter_Status", "False");

                if (modelReportHeads.Contains("LateTime"))
                    parameters[13] = new ReportParameter("ReportParameter_LateTime", "True");
                else
                    parameters[13] = new ReportParameter("ReportParameter_LateTime", "False");

                if (modelReportHeads.Contains("LastDayOutTime"))
                    parameters[14] = new ReportParameter("ReportParameter_LastDayOutTime", "True");
                else
                    parameters[14] = new ReportParameter("ReportParameter_LastDayOutTime", "False");

                if (modelReportHeads.Contains("TotalContinuousAbsentDays"))
                    parameters[15] = new ReportParameter("ReportParameter_TotalContinuousAbsentDays", "True");
                else
                    parameters[15] = new ReportParameter("ReportParameter_TotalContinuousAbsentDays", "False");

                if (modelReportHeads.Contains("OTHours"))
                    parameters[16] = new ReportParameter("ReportParameter_OTHours", "True");
                else
                    parameters[16] = new ReportParameter("ReportParameter_OTHours", "False");

                if (modelReportHeads.Contains("LastDayOTHours"))
                    parameters[17] = new ReportParameter("ReportParameter_LastDayOTHours", "True");
                else
                    parameters[17] = new ReportParameter("ReportParameter_LastDayOTHours", "False");


                if (modelReportHeads.Contains("Remarks"))
                    parameters[18] = new ReportParameter("ReportParameter_Remarks", "True");
                else
                    parameters[18] = new ReportParameter("ReportParameter_Remarks", "False");


                if (modelReportHeads.Contains("SignatureOfEmployee"))
                    parameters[19] = new ReportParameter("ReportParameter_SignatureOfEmployee", "True");
                else
                    parameters[19] = new ReportParameter("ReportParameter_SignatureOfEmployee", "False");

                lr.SetParameters(parameters);

                lr.DataSources.Add(rd);
            }


            string reportType = "pdf";
            if (model.SearchFieldModel.PrintFormatId == 2)
                reportType = "Excel";

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0..0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        //[AjaxAuthorize(Roles = "attendance-1,attendance-2,attendance-3")]
        public ActionResult AttendanceSummaryIndex(AttendanceViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.WorkShifts = BranchUnitWorkShiftManager.GetBranchUnitWorkShiftByBrancUnitId(model.SearchFieldModel.SearchByBranchUnitId);

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }


        //[AjaxAuthorize(Roles = "attendance-1,attendance-2,attendance-3")]
        public ActionResult AttendanceSummary(AttendanceViewModel model)
        {
            Guid employeeId = Guid.Empty;
            var employee = new Employee();

            if (!string.IsNullOrEmpty(model.SearchByEmployeeCardId))
                employee = EmployeeManager.GetEmployeeByCardId(model.SearchByEmployeeCardId);

            if (employee.Id > 0)
                employeeId = employee.EmployeeId;

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "AttendanceSummary.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            int companyId = model.SearchFieldModel.SearchByCompanyId;
            int branchId = model.SearchFieldModel.SearchByBranchId;
            int branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? branchUnitWorkShiftId = model.SearchByBranchUnitWorkShiftId;
            DateTime? transactionDate = model.StartDate;


            List<AttendanceSummaryModel> attendance = HrmReportManager.GetAttendanceSummaryInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, branchUnitWorkShiftId, transactionDate);


            ReportDataSource rd = new ReportDataSource("DataSetEmployeeDailyAttendanceSummary", attendance);
            lr.DataSources.Add(rd);


            string reportType = "pdf";
            if (model.SearchFieldModel.PrintFormatId == 2)
                reportType = "Excel";

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0..0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }


        //[AjaxAuthorize(Roles = "attendance-1,attendance-2,attendance-3")]
        public ActionResult AttendanceSummaryByDesignationIndex(AttendanceViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.WorkShifts = BranchUnitWorkShiftManager.GetBranchUnitWorkShiftByBrancUnitId(model.SearchFieldModel.SearchByBranchUnitId);

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }


        //[AjaxAuthorize(Roles = "attendance-1,attendance-2,attendance-3")]
        public ActionResult AttendanceSummaryByDesignation(AttendanceViewModel model)
        {
            Guid employeeId = Guid.Empty;
            var employee = new Employee();

            if (!string.IsNullOrEmpty(model.SearchByEmployeeCardId))
                employee = EmployeeManager.GetEmployeeByCardId(model.SearchByEmployeeCardId);

            if (employee.Id > 0)
                employeeId = employee.EmployeeId;

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "AttendanceSummaryByDesignation.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            int companyId = model.SearchFieldModel.SearchByCompanyId;
            int branchId = model.SearchFieldModel.SearchByBranchId;
            int branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? branchUnitWorkShiftId = model.SearchByBranchUnitWorkShiftId;
            DateTime? transactionDate = model.StartDate;


            List<AttendanceSummaryByDesignationModel> attendance = HrmReportManager.GetAttendanceSummaryByDesignationInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, branchUnitWorkShiftId, transactionDate);


            ReportDataSource rd = new ReportDataSource("DataSetEmployeeDailyAttendanceSummaryByDesignation", attendance);
            lr.DataSources.Add(rd);


            string reportType = "pdf";
            if (model.SearchFieldModel.PrintFormatId == 2)
                reportType = "Excel";

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0..0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        public ActionResult EmployeeAllInfoIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeGrades =
                    EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
                model.EmployeeDesignations =
                    EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(
                        model.SearchFieldModel.SearchByEmployeeGradeId);
                model.EmployeeBloodGroups = BloodGroupManager.GetAllBloodGroups();
                model.Genders = GenderManager.GetAllGenders();
                model.EmployeeReligions = ReligionManager.GetAllReligions();
                model.EmployeeMaritalStatuses = MaritalStatusManager.GetAllMaritalStatuses();
                model.EmployeeCountries = CountryManager.GetAllCountries();
                model.EmployeeDistricts = DistrictManager.GetDistrictsByCountry(model.SearchFieldModel.SearchByEmployeePermanentCountryId);
                model.EmployeeEducationLevels = EducationLevelManager.GetAllEducationLevels();


                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeStatuses = statusList;


                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult EmployeeAllInfo(ReportSearchViewModel model)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "EmployeeAllInfo.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("EmployeeAllInfoIndex");

            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? employeeGradeId = model.SearchFieldModel.SearchByEmployeeGradeId;
            int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;
            int? bloodGroupId = model.SearchFieldModel.SearchByEmployeeBloodGroupId;
            int? genderId = model.SearchFieldModel.SearchByEmployeeGenderId;
            int? religionId = model.SearchFieldModel.SearchByEmployeeReligionId;
            int? maritalStateId = model.SearchFieldModel.SearchByEmployeeMaritalStateId;

            DateTime? joiningDateBegin = model.SearchFieldModel.JoiningDateBegin;
            DateTime? joiningDateEnd = model.SearchFieldModel.JoiningDateEnd;
            DateTime? confirmationDateBegin = model.SearchFieldModel.ConfirmationDateBegin;
            DateTime? confirmationDateEnd = model.SearchFieldModel.ConfirmationDateEnd;
            DateTime? quitDateBegin = model.SearchFieldModel.QuitDateBegin;
            DateTime? quitDateEnd = model.SearchFieldModel.QuitDateEnd;
            int? birthDayMonth = Convert.ToInt32(model.SearchFieldModel.BirthDayMonth);
            DateTime? mariageAnniversaryDateBegin = model.SearchFieldModel.MariageAnniversaryDateBegin;
            DateTime? mariageAnniversaryDateEnd = model.SearchFieldModel.MariageAnniversaryDateEnd;

            int? permanentCountryId = model.SearchFieldModel.SearchByEmployeePermanentCountryId;
            int? employeePermanentDistrictId = model.SearchFieldModel.SearchByEmployeePermanentDistrictId;
            int? employeeEducationLevelId = model.SearchFieldModel.SearchByEmployeeEducationLevelId;

            string employeeCardId = model.SearchFieldModel.SearchByEmployeeCardId;
            string employeeName = model.SearchFieldModel.SearchByEmployeeName;
            string mobileNo = model.SearchFieldModel.SearchByEmployeeMobileNo;

            int? activeStatus = model.SearchFieldModel.SearchByEmployeeStatus;
            string userName = PortalContext.CurrentUser.Name;
            DateTime? fromDate = model.SearchFieldModel.StartDate;

            List<EmployeeAllInfoModel> employeeAllInfo = HrmReportManager.GetEmployeeAllInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId,
                bloodGroupId, genderId, religionId, maritalStateId, joiningDateBegin,
                joiningDateEnd, confirmationDateBegin, confirmationDateEnd, quitDateBegin,
                quitDateEnd, birthDayMonth, mariageAnniversaryDateBegin, mariageAnniversaryDateEnd,
                permanentCountryId, employeePermanentDistrictId, employeeEducationLevelId,
                employeeCardId, employeeName, mobileNo, activeStatus, userName, fromDate);


            ReportDataSource rd = new ReportDataSource("DataSetEmployeeAllInfo", employeeAllInfo);

            ReportParameter[] parameters = new ReportParameter[39];

            List<string> reportHeads = model.ReportHeads;

            if (reportHeads != null)
            {
                if (reportHeads.Contains("EmployeeCardId"))
                    parameters[0] = new ReportParameter("Param_EmployeeCardId", "True");
                else
                    parameters[0] = new ReportParameter("Param_EmployeeCardId", "False");

                if (reportHeads.Contains("PunchCardNo"))
                    parameters[1] = new ReportParameter("Param_PunchCardNo", "True");
                else
                    parameters[1] = new ReportParameter("Param_PunchCardNo", "False");

                if (reportHeads.Contains("EmployeeName"))
                    parameters[2] = new ReportParameter("Param_EmployeeName", "True");
                else
                    parameters[2] = new ReportParameter("Param_EmployeeName", "False");

                if (reportHeads.Contains("Company"))
                    parameters[3] = new ReportParameter("Param_Company", "True");
                else
                    parameters[3] = new ReportParameter("Param_Company", "False");

                if (reportHeads.Contains("Branch"))
                    parameters[4] = new ReportParameter("Param_Branch", "True");
                else
                    parameters[4] = new ReportParameter("Param_Branch", "False");

                if (reportHeads.Contains("Unit"))
                    parameters[5] = new ReportParameter("Param_Unit", "True");
                else
                    parameters[5] = new ReportParameter("Param_Unit", "False");

                if (reportHeads.Contains("Department"))
                    parameters[6] = new ReportParameter("Param_Department", "True");
                else
                    parameters[6] = new ReportParameter("Param_Department", "False");

                if (reportHeads.Contains("Section"))
                    parameters[7] = new ReportParameter("Param_Section", "True");
                else
                    parameters[7] = new ReportParameter("Param_Section", "False");

                if (reportHeads.Contains("Line"))
                    parameters[8] = new ReportParameter("Param_Line", "True");
                else
                    parameters[8] = new ReportParameter("Param_Line", "False");

                if (reportHeads.Contains("EmployeeType"))
                    parameters[9] = new ReportParameter("Param_EmployeeType", "True");
                else
                    parameters[9] = new ReportParameter("Param_EmployeeType", "False");

                if (reportHeads.Contains("EmployeeGrade"))
                    parameters[10] = new ReportParameter("Param_EmployeeGrade", "True");
                else
                    parameters[10] = new ReportParameter("Param_EmployeeGrade", "False");

                if (reportHeads.Contains("EmployeeDesignation"))
                    parameters[11] = new ReportParameter("Param_EmployeeDesignation", "True");
                else
                    parameters[11] = new ReportParameter("Param_EmployeeDesignation", "False");

                if (reportHeads.Contains("JoiningDate"))
                    parameters[12] = new ReportParameter("Param_JoiningDate", "True");
                else
                    parameters[12] = new ReportParameter("Param_JoiningDate", "False");


                if (reportHeads.Contains("ConfirmationDate"))
                    parameters[13] = new ReportParameter("Param_ConfirmationDate", "True");
                else
                    parameters[13] = new ReportParameter("Param_ConfirmationDate", "False");

                if (reportHeads.Contains("QuitDate"))
                    parameters[14] = new ReportParameter("Param_QuitDate", "True");
                else
                    parameters[14] = new ReportParameter("Param_QuitDate", "False");

                if (reportHeads.Contains("QuitType"))
                    parameters[15] = new ReportParameter("Param_QuitType", "True");
                else
                    parameters[15] = new ReportParameter("Param_QuitType", "False");

                if (reportHeads.Contains("BasicSalary"))
                    parameters[16] = new ReportParameter("Param_BasicSalary", "True");
                else
                    parameters[16] = new ReportParameter("Param_BasicSalary", "False");

                if (reportHeads.Contains("GrossSalary"))
                    parameters[17] = new ReportParameter("Param_GrossSalary", "True");
                else
                    parameters[17] = new ReportParameter("Param_GrossSalary", "False");

                if (reportHeads.Contains("ActiveStatus"))
                    parameters[18] = new ReportParameter("Param_ActiveStatus", "True");
                else
                    parameters[18] = new ReportParameter("Param_ActiveStatus", "False");

                if (reportHeads.Contains("MothersName"))
                    parameters[19] = new ReportParameter("Param_MothersName", "True");
                else
                    parameters[19] = new ReportParameter("Param_MothersName", "False");

                if (reportHeads.Contains("FathersName"))
                    parameters[20] = new ReportParameter("Param_FathersName", "True");
                else
                    parameters[20] = new ReportParameter("Param_FathersName", "False");

                if (reportHeads.Contains("GenderName"))
                    parameters[21] = new ReportParameter("Param_GenderName", "True");
                else
                    parameters[21] = new ReportParameter("Param_GenderName", "False");

                if (reportHeads.Contains("BirthDate"))
                    parameters[22] = new ReportParameter("Param_BirthDate", "True");
                else
                    parameters[22] = new ReportParameter("Param_BirthDate", "False");

                if (reportHeads.Contains("MobilePhone"))
                    parameters[23] = new ReportParameter("Param_MobilePhone", "True");
                else
                    parameters[23] = new ReportParameter("Param_MobilePhone", "False");

                if (reportHeads.Contains("BloodGroup"))
                    parameters[24] = new ReportParameter("Param_BloodGroup", "True");
                else
                    parameters[24] = new ReportParameter("Param_BloodGroup", "False");

                if (reportHeads.Contains("ReligionName"))
                    parameters[25] = new ReportParameter("Param_ReligionName", "True");
                else
                    parameters[25] = new ReportParameter("Param_ReligionName", "False");

                if (reportHeads.Contains("MaritalState"))
                    parameters[26] = new ReportParameter("Param_MaritalState", "True");
                else
                    parameters[26] = new ReportParameter("Param_MaritalState", "False");

                if (reportHeads.Contains("MarriageAnniversaryDate"))
                    parameters[27] = new ReportParameter("Param_MarriageAnniversaryDate", "True");
                else
                    parameters[27] = new ReportParameter("Param_MarriageAnniversaryDate", "False");

                if (reportHeads.Contains("CountryName"))
                    parameters[28] = new ReportParameter("Param_CountryName", "True");
                else
                    parameters[28] = new ReportParameter("Param_CountryName", "False");

                if (reportHeads.Contains("DistrictName"))
                    parameters[29] = new ReportParameter("Param_DistrictName", "True");
                else
                    parameters[29] = new ReportParameter("Param_DistrictName", "False");

                if (reportHeads.Contains("NationalIdNo"))
                    parameters[30] = new ReportParameter("Param_NationalIdNo", "True");
                else
                    parameters[30] = new ReportParameter("Param_NationalIdNo", "False");

                if (reportHeads.Contains("BirthRegistrationNo"))
                    parameters[31] = new ReportParameter("Param_BirthRegistrationNo", "True");
                else
                    parameters[31] = new ReportParameter("Param_BirthRegistrationNo", "False");

                if (reportHeads.Contains("TaxIdentificationNo"))
                    parameters[32] = new ReportParameter("Param_TaxIdentificationNo", "True");
                else
                    parameters[32] = new ReportParameter("Param_TaxIdentificationNo", "False");

                if (reportHeads.Contains("EducationLevel"))
                    parameters[33] = new ReportParameter("Param_EducationLevel", "True");
                else
                    parameters[33] = new ReportParameter("Param_EducationLevel", "False");

                if (reportHeads.Contains("LastIncrementDate"))
                    parameters[34] = new ReportParameter("Param_LastIncrementDate", "True");
                else
                    parameters[34] = new ReportParameter("Param_LastIncrementDate", "False");

                if (reportHeads.Contains("SkillType"))
                    parameters[35] = new ReportParameter("Param_SkillType", "True");
                else
                    parameters[35] = new ReportParameter("Param_SkillType", "False");

                if (reportHeads.Contains("LastIncrementAmount"))
                    parameters[36] = new ReportParameter("Param_LastIncrementAmount", "True");
                else
                    parameters[36] = new ReportParameter("Param_LastIncrementAmount", "False");

                if (reportHeads.Contains("PresentAddress"))
                    parameters[37] = new ReportParameter("Param_PresentAddress", "True");
                else
                    parameters[37] = new ReportParameter("Param_PresentAddress", "False");

                if (reportHeads.Contains("PermanentAddress"))
                    parameters[38] = new ReportParameter("Param_PermanentAddress", "True");
                else
                    parameters[38] = new ReportParameter("Param_PermanentAddress", "False");


                lr.SetParameters(parameters);

                lr.DataSources.Add(rd);
            }

            string reportType = "pdf";
            if (model.SearchFieldModel.PrintFormatId == 2)
                reportType = "Excel";

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0..0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        public ActionResult EmployeeAllInfoIndexNew(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeGrades =
                    EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
                model.EmployeeDesignations =
                    EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(
                        model.SearchFieldModel.SearchByEmployeeGradeId);
                model.EmployeeBloodGroups = BloodGroupManager.GetAllBloodGroups();
                model.Genders = GenderManager.GetAllGenders();
                model.EmployeeReligions = ReligionManager.GetAllReligions();
                model.EmployeeMaritalStatuses = MaritalStatusManager.GetAllMaritalStatuses();
                model.EmployeeCountries = CountryManager.GetAllCountries();
                model.EmployeeDistricts = DistrictManager.GetDistrictsByCountry(model.SearchFieldModel.SearchByEmployeePermanentCountryId);
                model.EmployeeEducationLevels = EducationLevelManager.GetAllEducationLevels();


                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeStatuses = statusList;


                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult EmployeeAllInfoNew(ReportSearchViewModel model)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "EmployeeAllInfoNew.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("EmployeeAllInfoIndexNew");

            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? employeeGradeId = model.SearchFieldModel.SearchByEmployeeGradeId;
            int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;
            int? bloodGroupId = model.SearchFieldModel.SearchByEmployeeBloodGroupId;
            int? genderId = model.SearchFieldModel.SearchByEmployeeGenderId;
            int? religionId = model.SearchFieldModel.SearchByEmployeeReligionId;
            int? maritalStateId = model.SearchFieldModel.SearchByEmployeeMaritalStateId;

            DateTime? joiningDateBegin = model.SearchFieldModel.JoiningDateBegin;
            DateTime? joiningDateEnd = model.SearchFieldModel.JoiningDateEnd;
            DateTime? confirmationDateBegin = model.SearchFieldModel.ConfirmationDateBegin;
            DateTime? confirmationDateEnd = model.SearchFieldModel.ConfirmationDateEnd;
            DateTime? quitDateBegin = model.SearchFieldModel.QuitDateBegin;
            DateTime? quitDateEnd = model.SearchFieldModel.QuitDateEnd;
            int? birthDayMonth = Convert.ToInt32(model.SearchFieldModel.BirthDayMonth);
            DateTime? mariageAnniversaryDateBegin = model.SearchFieldModel.MariageAnniversaryDateBegin;
            DateTime? mariageAnniversaryDateEnd = model.SearchFieldModel.MariageAnniversaryDateEnd;

            int? permanentCountryId = model.SearchFieldModel.SearchByEmployeePermanentCountryId;
            int? employeePermanentDistrictId = model.SearchFieldModel.SearchByEmployeePermanentDistrictId;
            int? employeeEducationLevelId = model.SearchFieldModel.SearchByEmployeeEducationLevelId;

            string employeeCardId = model.SearchFieldModel.SearchByEmployeeCardId;
            string employeeName = model.SearchFieldModel.SearchByEmployeeName;
            string mobileNo = model.SearchFieldModel.SearchByEmployeeMobileNo;

            int? activeStatus = model.SearchFieldModel.SearchByEmployeeStatus;
            string userName = PortalContext.CurrentUser.Name;
            DateTime? fromDate = model.SearchFieldModel.StartDate;

            List<EmployeeAllInfoNewModel> employeeAllInfo = HrmReportManager.GetEmployeeAllInfoNew(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId,
                bloodGroupId, genderId, religionId, maritalStateId, joiningDateBegin,
                joiningDateEnd, confirmationDateBegin, confirmationDateEnd, quitDateBegin,
                quitDateEnd, birthDayMonth, mariageAnniversaryDateBegin, mariageAnniversaryDateEnd,
                permanentCountryId, employeePermanentDistrictId, employeeEducationLevelId,
                employeeCardId, employeeName, mobileNo, activeStatus, userName, fromDate);

            foreach (var t in employeeAllInfo)
            {
                t.RowId = BanglaConversion.ConvertToBanglaNumber(t.RowId);
                t.EmployeeCardId = BanglaConversion.ConvertToBanglaNumber(t.EmployeeCardId);
                t.NationalIdNo = BanglaConversion.ConvertToBanglaNumber(t.NationalIdNo);
                t.JoiningDate = BanglaConversion.ConvertToBanglaNumber(t.JoiningDate);
                t.BirthDate = BanglaConversion.ConvertToBanglaNumber(t.BirthDate);
                t.AgeInYear = BanglaConversion.ConvertToBanglaNumber(t.AgeInYear);
                t.EarnLeave = BanglaConversion.ConvertToBanglaNumber(t.EarnLeave);
                t.Weekend = BanglaConversion.ConvertEnglishDaytoBanglaDay(t.Weekend);
            }

            ReportDataSource rd = new ReportDataSource("DataSet1", employeeAllInfo);

            lr.DataSources.Add(rd);

            string reportType = "pdf";
            if (model.SearchFieldModel.PrintFormatId == 2)
                reportType = "Excel";

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0..0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        public ActionResult EmployeeInfo()
        {
            ModelState.Clear();

            var urlBuilder = new StringBuilder();
            urlBuilder.Append("http://");
            urlBuilder.Append(AppConfig.ReportServerAddress); // "Server name or IP addrress " 
            urlBuilder.Append("/reportserver/Pages/ReportViewer.aspx?");
            urlBuilder.Append("/SCERPREPORT.HR/"); // "Report Module Name"
            urlBuilder.Append("EmployeeInfo");
            urlBuilder.Append("&rs:Command=Render");

            urlBuilder.Append("&UserName=" + PortalContext.CurrentUser.Name);
            urlBuilder.Append("&CompanyId=" + "-1");
            urlBuilder.Append("&BranchId=" + "-1");
            urlBuilder.Append("&UnitId=" + "-1");
            urlBuilder.Append("&DepartmentId=" + "-1");
            urlBuilder.Append("&LineId=" + "-1");
            urlBuilder.Append("&SectionId=" + "-1");
            urlBuilder.Append("&EmployeeTypeId=" + "-1");
            urlBuilder.Append("&EmployeeGradeId=" + "-1");
            urlBuilder.Append("&EmployeeDesignationId=" + "-1");

            ViewBag.ReportUrl = urlBuilder;

            return PartialView("_SSRSReportContorl");
        }

        public ActionResult EmployeeFamilyInfoIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            var urlBuilder = new StringBuilder();
            urlBuilder.Append("http://");
            urlBuilder.Append(AppConfig.ReportServerAddress); // "Server name or IP addrress " 
            urlBuilder.Append("/reportserver/Pages/ReportViewer.aspx?");
            urlBuilder.Append("/SCERPREPORT.HR/"); // "Report Module Name"
            urlBuilder.Append("EmployeeFamilyInfo");
            urlBuilder.Append("&rs:Command=Render");

            urlBuilder.Append("&UserName=" + PortalContext.CurrentUser.Name);
            urlBuilder.Append("&CompanyId=" + "-1");
            urlBuilder.Append("&BranchId=" + "-1");
            urlBuilder.Append("&UnitId=" + "-1");

            urlBuilder.Append("&DepartmentId=" + "-1");
            urlBuilder.Append("&LineId=" + "-1");
            urlBuilder.Append("&SectionId=" + "-1");
            urlBuilder.Append("&ChildGenderId=" + "-1");

            ViewBag.ReportUrl = urlBuilder;

            return PartialView("_SSRSReportContorl");
        }

        public ActionResult EmployeeDepartmentInfoIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            var urlBuilder = new StringBuilder();
            urlBuilder.Append("http://");
            urlBuilder.Append(AppConfig.ReportServerAddress); // "Server name or IP addrress " 
            urlBuilder.Append("/reportserver/Pages/ReportViewer.aspx?");
            urlBuilder.Append("/SCERPREPORT.HR/"); // "Report Module Name"
            urlBuilder.Append("EmployeeDepartmentInfo2");
            urlBuilder.Append("&rs:Command=Render");

            urlBuilder.Append("&UserName=" + PortalContext.CurrentUser.Name);
            urlBuilder.Append("&CompanyId=" + "-1");
            urlBuilder.Append("&BranchId=" + "-1");
            urlBuilder.Append("&UnitId=" + "-1");
            urlBuilder.Append("&DepartmentId=" + "-1");
            urlBuilder.Append("&LineId=" + "-1");
            urlBuilder.Append("&SectionId=" + "-1");

            ViewBag.ReportUrl = urlBuilder;

            return PartialView("_SSRSReportContorl");
        }

        public ActionResult AllEmployeeJobCardIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                var attendanceStatus = from AttendanceStatus attStatus in Enum.GetValues(typeof(AttendanceStatus))
                                       select new { Id = (int)attStatus, Name = attStatus.ToString() };
                model.AttendanceStatuses = attendanceStatus;


                var activeStatusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                       select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeActiveStatuses = activeStatusList;


                var employeeCategoryValueList = from EmployeeCategoryValue status in Enum.GetValues(typeof(EmployeeCategoryValue))
                                                select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeCategories = employeeCategoryValueList;


                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };
                model.PrintFormatStatuses = printFormat;


            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult AllEmployeeJobCard(ReportSearchViewModel model)
        {
            LocalReport lr = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "AllEmployeeJobCard.rdlc");

            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int year = Convert.ToInt32(model.SelectedYear);
            int month = Convert.ToInt32(model.SelectedMonth);
            DateTime? fromDate = model.SearchFieldModel.StartDate;
            DateTime? toDate = model.SearchFieldModel.EndDate;
            int employeeActiveStatusId = model.EmployeeActiveStatus;
            int employeeCategoryId = model.EmployeeCategory;
            string employeeCardId = model.EmployeeCardId;

            string attendanceStatus = Enum.GetName(typeof(AttendanceStatus), model.AttendanceStatus);
            bool otEnabled = model.SearchByOTEligibilty;
            bool extraOTEnabled = model.SearchByExtraOTEligibilty;
            bool weekendOTEnabled = model.SearchByWeekendOTEligibilty;


            List<AllEmployeeJobCardView> allEmployeeJobCard = HRMReportManager.GetAllEmployeeJobCardInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId,
                departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year,
                month, fromDate, toDate, attendanceStatus, employeeActiveStatusId, employeeCategoryId,
                otEnabled, extraOTEnabled, weekendOTEnabled);

            ReportDataSource rd = new ReportDataSource("DataSetAllEmployeeJobCard", allEmployeeJobCard);
            lr.DataSources.Add(rd);


            string reportType = "pdf";
            if (model.SearchFieldModel.PrintFormatId == 2)
                reportType = "Excel";


            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0..2in</MarginTop>" +
                "  <MarginLeft>.4in</MarginLeft>" +
                "  <MarginRight>.2in</MarginRight>" +
                "  <MarginBottom>0.2in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        public ActionResult EmployeeLeaveDetailInfoIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeGrades =
                    EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
                model.EmployeeDesignations =
                    EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(
                        model.SearchFieldModel.SearchByEmployeeGradeId);
                model.Genders = GenderManager.GetAllGenders();
                model.LeaveTypes = LeaveTypeManager.GetAllLeaveTypes();


                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeStatuses = statusList;


                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult EmployeeLeaveDetailInfo(ReportSearchViewModel model)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "EmployeeLeaveDetail.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("EmployeeLeaveDetailInfoIndex");

            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? employeeGradeId = model.SearchFieldModel.SearchByEmployeeGradeId;
            int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;
            int? genderId = model.SearchFieldModel.SearchByEmployeeGenderId;

            DateTime? joiningDateBegin = model.SearchFieldModel.JoiningDateBegin;
            DateTime? joiningDateEnd = model.SearchFieldModel.JoiningDateEnd;
            DateTime? quitDateBegin = model.SearchFieldModel.QuitDateBegin;
            DateTime? quitDateEnd = model.SearchFieldModel.QuitDateEnd;

            string employeeCardId = model.SearchFieldModel.SearchByEmployeeCardId;
            string employeeName = model.SearchFieldModel.SearchByEmployeeName;
            int? leaveTypeId = model.SearchFieldModel.SearchByLeaveTypeId;

            DateTime? consumedDateBegin = model.SearchFieldModel.ConsumedDateBegin;
            DateTime? consumedDateEnd = model.SearchFieldModel.ConsumedDateEnd;

            int? activeStatus = model.SearchFieldModel.SearchByEmployeeStatus;
            DateTime? fromDate = model.SearchFieldModel.StartDate;
            string userName = PortalContext.CurrentUser.Name;


            List<EmployeeLeaveDetailModel> employeeLeaveDetailInfo =
                HrmReportManager.GetEmployeeLeaveDetailInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                    departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId,
                    genderId, joiningDateBegin, joiningDateEnd, quitDateBegin, quitDateEnd, employeeCardId,
                    employeeName, leaveTypeId, consumedDateBegin, consumedDateEnd, activeStatus, fromDate,
                    userName);

            ReportDataSource rd = new ReportDataSource("DataSet1", employeeLeaveDetailInfo);
            lr.DataSources.Add(rd);

            string reportType = "pdf";
            if (model.SearchFieldModel.PrintFormatId == 2)
                reportType = "Excel";

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0..0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        public ActionResult ManpowerSummaryIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeDesignations =
                    EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(
                        model.SearchFieldModel.SearchByEmployeeGradeId);
                model.Genders = GenderManager.GetAllGenders();

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult ManpowerSummaryInfo(ReportSearchViewModel model)
        {
            LocalReport lr = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "ManpowerSummary.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("EmployeeAllInfoIndex");

            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;
            int? genderId = model.SearchFieldModel.SearchByEmployeeGenderId;


            DateTime? joiningDateBegin = model.SearchFieldModel.JoiningDateBegin;
            DateTime? joiningDateEnd = model.SearchFieldModel.JoiningDateEnd;
            DateTime? confirmationDateBegin = model.SearchFieldModel.ConfirmationDateBegin;
            DateTime? confirmationDateEnd = model.SearchFieldModel.ConfirmationDateEnd;
            DateTime? quitDateBegin = model.SearchFieldModel.QuitDateBegin;
            DateTime? quitDateEnd = model.SearchFieldModel.QuitDateEnd;

            string userName = PortalContext.CurrentUser.Name;

            List<ManpowerSummaryModel> manpowerSummary = HrmReportManager.GetManpowerSummaryInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                departmentLineId, employeeTypeId, employeeDesignationId,
                genderId, joiningDateBegin,
                joiningDateEnd, confirmationDateBegin, confirmationDateEnd, quitDateBegin,
                quitDateEnd, userName);


            ReportDataSource rd = new ReportDataSource("DataSet1", manpowerSummary);

            lr.DataSources.Add(rd);

            string reportType = "pdf";
            if (model.SearchFieldModel.PrintFormatId == 2)
                reportType = "Excel";

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0..0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        public ActionResult ManPowerSummaryShortIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeDesignations =
                    EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(
                        model.SearchFieldModel.SearchByEmployeeGradeId);
                model.Genders = GenderManager.GetAllGenders();

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult ManPowerSummaryShortInfo(ReportSearchViewModel model)
        {
            LocalReport lr = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "ManpowerSummaryShort.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("EmployeeAllInfoIndex");

            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;
            int? genderId = model.SearchFieldModel.SearchByEmployeeGenderId;


            DateTime? joiningDateBegin = model.SearchFieldModel.JoiningDateBegin;
            DateTime? joiningDateEnd = model.SearchFieldModel.JoiningDateEnd;
            DateTime? confirmationDateBegin = model.SearchFieldModel.ConfirmationDateBegin;
            DateTime? confirmationDateEnd = model.SearchFieldModel.ConfirmationDateEnd;
            DateTime? quitDateBegin = model.SearchFieldModel.QuitDateBegin;
            DateTime? quitDateEnd = model.SearchFieldModel.QuitDateEnd;

            string userName = PortalContext.CurrentUser.Name;

            List<ManpowerSummaryModel> manpowerSummary = HrmReportManager.GetManpowerSummaryInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                departmentLineId, employeeTypeId, employeeDesignationId,
                genderId, joiningDateBegin,
                joiningDateEnd, confirmationDateBegin, confirmationDateEnd, quitDateBegin,
                quitDateEnd, userName);


            ReportDataSource rd = new ReportDataSource("DataSet1", manpowerSummary);

            lr.DataSources.Add(rd);

            string reportType = "pdf";
            if (model.SearchFieldModel.PrintFormatId == 2)
                reportType = "Excel";

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0..0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        public ActionResult ManPowerSummarySkillIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeDesignations =
                    EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(
                        model.SearchFieldModel.SearchByEmployeeGradeId);
                model.Genders = GenderManager.GetAllGenders();

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult ManPowerSummarySkillInfo(ReportSearchViewModel model)
        {
            LocalReport lr = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "ManpowerSummarySkill.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("EmployeeAllInfoIndex");

            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;
            int? genderId = model.SearchFieldModel.SearchByEmployeeGenderId;


            DateTime? joiningDateBegin = model.SearchFieldModel.JoiningDateBegin;
            DateTime? joiningDateEnd = model.SearchFieldModel.JoiningDateEnd;
            DateTime? confirmationDateBegin = model.SearchFieldModel.ConfirmationDateBegin;
            DateTime? confirmationDateEnd = model.SearchFieldModel.ConfirmationDateEnd;
            DateTime? quitDateBegin = model.SearchFieldModel.QuitDateBegin;
            DateTime? quitDateEnd = model.SearchFieldModel.QuitDateEnd;

            string userName = PortalContext.CurrentUser.Name;

            List<ManpowerSummaryModel> manpowerSummary = HrmReportManager.GetManpowerSummarySkillInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                departmentLineId, employeeTypeId, employeeDesignationId,
                genderId, joiningDateBegin,
                joiningDateEnd, confirmationDateBegin, confirmationDateEnd, quitDateBegin,
                quitDateEnd, userName);

            ReportDataSource rd = new ReportDataSource("DataSet1", manpowerSummary);

            lr.DataSources.Add(rd);

            string reportType = "pdf";
            if (model.SearchFieldModel.PrintFormatId == 2)
                reportType = "Excel";

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0..0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        public ActionResult EmployeeLeaveHistoryInfoIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeGrades =
                    EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
                model.EmployeeDesignations =
                    EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(
                        model.SearchFieldModel.SearchByEmployeeGradeId);
                model.Genders = GenderManager.GetAllGenders();
                model.LeaveTypes = LeaveTypeManager.GetAllLeaveTypes();


                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeStatuses = statusList;


                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult EmployeeLeaveHistoryInfo(ReportSearchViewModel model)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "EmployeeLeaveHistory.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("EmployeeLeaveHistoryInfoIndex");

            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? employeeGradeId = model.SearchFieldModel.SearchByEmployeeGradeId;
            int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;
            int? genderId = model.SearchFieldModel.SearchByEmployeeGenderId;

            DateTime? joiningDateBegin = model.SearchFieldModel.JoiningDateBegin;
            DateTime? joiningDateEnd = model.SearchFieldModel.JoiningDateEnd;
            DateTime? quitDateBegin = model.SearchFieldModel.QuitDateBegin;
            DateTime? quitDateEnd = model.SearchFieldModel.QuitDateEnd;

            string employeeCardId = model.SearchFieldModel.SearchByEmployeeCardId;
            string employeeName = model.SearchFieldModel.SearchByEmployeeName;
            int? leaveTypeId = model.SearchFieldModel.SearchByLeaveTypeId;

            int? activeStatus = model.SearchFieldModel.SearchByEmployeeStatus;
            int year = Convert.ToInt32(model.SearchFieldModel.SelectedYear);
            DateTime? fromDate = model.SearchFieldModel.StartDate;
            string userName = PortalContext.CurrentUser.Name;


            List<EmployeeLeaveHistoryModel> employeeLeaveHistoryInfo =
                HrmReportManager.GetEmployeeLeaveHistoryInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId,
                    departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, genderId,
                    joiningDateBegin, joiningDateEnd, quitDateBegin, quitDateEnd, employeeCardId, employeeName, leaveTypeId,
                    activeStatus, year, userName, fromDate);

            ReportDataSource rd = new ReportDataSource("DataSet1", employeeLeaveHistoryInfo);
            lr.DataSources.Add(rd);

            string reportType = "pdf";
            if (model.SearchFieldModel.PrintFormatId == 2)
                reportType = "Excel";

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0..0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        public JsonResult CheckValidUser(AttendanceViewModel model)
        {
            var employee = new Employee();
            Guid? employeeId = Guid.Empty;
            bool valid = true;

            if (!string.IsNullOrEmpty(model.SearchByEmployeeCardId))
            {
                employee.EmployeeCardId = model.SearchByEmployeeCardId;
                valid = EmployeeManager.CheckExistingEmployeeCardNumber(employee);
            }
            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllBranchesByCompanyId(int companyId)
        {
            var branches = BranchManager.GetAllPermittedBranchesByCompanyId(companyId);
            return Json(new { Success = true, Branches = branches }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllBranchUnitByBranchId(int branchId)
        {
            var brancheUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(branchId);
            return Json(new { Success = true, BrancheUnits = brancheUnits }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllBranchUnitDepartmentByBranchUnitId(int branchUnitId)
        {
            var branchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(branchUnitId);
            return Json(new { Success = true, BranchUnitDepartments = branchUnitDepartments }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartmentSectionAndLineByBranchUnitDepartmentId(int branchUnitDepartmentId)
        {
            var departmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(branchUnitDepartmentId);
            var departmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(branchUnitDepartmentId);
            return Json(new { Success = true, DepartmentSections = departmentSections, DepartmentLines = departmentLines, }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllBranchUnitDepartmentAndWorkShiftByBranchUnitId(int branchUnitId)
        {
            var branchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(branchUnitId);
            var branchUnitWorkShifts = BranchUnitWorkShiftManager.GetBranchUnitWorkShiftByBrancUnitId(branchUnitId);
            return Json(new { Success = true, BranchUnitDepartments = branchUnitDepartments, DataSources = branchUnitWorkShifts }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllEmployeeGradesByEmployeeTypeId(int employeeTypeId)
        {
            var employeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(employeeTypeId);
            return Json(new { Success = true, EmployeeGrades = employeeGrades }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllEmployeeDesignationsByEmployeeGradeId(int employeeGradeId)
        {
            var employeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(employeeGradeId);
            return Json(new { Success = true, EmployeeDesignations = employeeDesignations }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllDistrictsByCountryId(int countryId)
        {
            var districts = DistrictManager.GetDistrictsByCountry(countryId);
            return Json(new { Success = true, Districts = districts }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckExistingEmployeeCardNumber(string employeeCardId)
        {
            var isExist = false;
            try
            {
                var employee = new Employee { EmployeeCardId = employeeCardId };
                isExist = EmployeeManager.CheckExistingEmployeeCardNumber(employee);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return Json(new { Checked = isExist }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReportView(string reportName, string userName)
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append("http://");
            urlBuilder.Append(AppConfig.ReportServerAddress);
            urlBuilder.Append("/reportserver/Pages/ReportViewer.aspx?");
            urlBuilder.Append("/SCERPREPORT.HR/");
            urlBuilder.Append(reportName);
            ViewBag.ReportUrl = urlBuilder;
            return PartialView("_SSRSReportContorl");
        }

        public ActionResult GetEmployeeBonusInfo()
        {
            List<SPGetEmployeesForBonus> reportdata = HrmReportManager.GetEmployeeBonusInfo();

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "EmployeeBonus.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("DataSet1", reportdata);

            lr.DataSources.Add(rd);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>8.3in</PageWidth>" +
                "  <PageHeight>11.7in</PageHeight>" +
                "  <MarginTop>0..2in</MarginTop>" +
                "  <MarginLeft>.3in</MarginLeft>" +
                "  <MarginRight>.2in</MarginRight>" +
                "  <MarginBottom>0.2in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        public ActionResult CuttingSectionAbsentIndex(DynamicReportHeadViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };
                model.SearchFieldModel.StartDate = DateTime.Now;
                model.PrintFormatStatuses = printFormat;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult CuttingSectionAbsentPrint(DynamicReportHeadViewModel model)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "CuttingAbsent.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            DateTime? fromDate = model.SearchFieldModel.StartDate;

            List<SpHrmCuttingSectionAbsent> employee = HrmReportManager.GetCuttingAbsentInfo(fromDate);

            string companyId = PortalContext.CurrentUser.CompId;
            string companyName = HrmReportManager.GetCompanyNameByCompanyId(companyId);

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_CompanySector", companyName);


            ReportDataSource rd = new ReportDataSource("DataSet1", employee);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);

            string reportType = "pdf";
            if (model.SearchFieldModel.PrintFormatId == 2)
                reportType = "Excel";

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11.7in</PageHeight>" +
                "  <MarginTop>0.4in</MarginTop>" +
                "  <MarginLeft>.4in</MarginLeft>" +
                "  <MarginRight>.2in</MarginRight>" +
                "  <MarginBottom>0.2in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }


        //Maternity

        public ActionResult MaternityLeaveIndex(DynamicReportHeadViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult MaternityLeavePrint(DynamicReportHeadViewModel model)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "MaternityLeave.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            DateTime? fromDate = model.SearchFieldModel.StartDate;
            decimal totalNetAmount = 0;
            int totalWorkingDays = 0;
            decimal averageWages = 0;
            decimal totalMaternityAmount = 0;

            model.ReportDataTable = HrmReportManager.GetMaternityInfo(model.SearchByEmployeeCardId, fromDate);

            for (int i = 0; i < model.ReportDataTable.Rows.Count; i++)
            {
                totalNetAmount = totalNetAmount + Convert.ToDecimal(model.ReportDataTable.Rows[i]["NetAmount"].ToString());
                totalWorkingDays = totalWorkingDays + Convert.ToInt32(model.ReportDataTable.Rows[i]["WorkingDays"].ToString());

                model.ReportDataTable.Rows[i]["SerialId"] = BanglaConversion.ConvertToBanglaNumber(model.ReportDataTable.Rows[i]["SerialId"].ToString());
                model.ReportDataTable.Rows[i]["EmployeeCardId"] = BanglaConversion.ConvertToBanglaNumber(model.ReportDataTable.Rows[i]["EmployeeCardId"].ToString());
                model.ReportDataTable.Rows[i]["JoiningDate"] = BanglaConversion.ConvertToBanglaNumber(model.ReportDataTable.Rows[i]["JoiningDate"].ToString());
                model.ReportDataTable.Rows[i]["LeaveEndDate"] = BanglaConversion.ConvertToBanglaNumber(model.ReportDataTable.Rows[i]["LeaveEndDate"].ToString());
                model.ReportDataTable.Rows[i]["GrossSalary"] = BanglaConversion.ConvertToBanglaNumber(model.ReportDataTable.Rows[i]["GrossSalary"].ToString());
                model.ReportDataTable.Rows[i]["WorkingDays"] = BanglaConversion.ConvertToBanglaNumber(model.ReportDataTable.Rows[i]["WorkingDays"].ToString());
                model.ReportDataTable.Rows[i]["Bonus"] = BanglaConversion.ConvertToBanglaNumber(model.ReportDataTable.Rows[i]["Bonus"].ToString());
                model.ReportDataTable.Rows[i]["NetAmount"] = BanglaConversion.ConvertToBanglaNumber(model.ReportDataTable.Rows[i]["NetAmount"].ToString());
                model.ReportDataTable.Rows[i]["LeaveStartDate"] = BanglaConversion.ConvertToBanglaNumber(model.ReportDataTable.Rows[i]["LeaveStartDate"].ToString());
                model.ReportDataTable.Rows[i]["MonthName"] = BanglaConversion.ConvertToBanglaMonth(model.ReportDataTable.Rows[i]["Month"].ToString()) + "-" + BanglaConversion.ConvertToBanglaNumber(model.ReportDataTable.Rows[i]["Year"].ToString());

                model.ReportDataTable.Rows[i]["FirstPaymentDate"] = BanglaConversion.ConvertToBanglaNumber(model.ReportDataTable.Rows[i]["FirstPaymentDate"].ToString());
                model.ReportDataTable.Rows[i]["FirstPaymentAmount"] = BanglaConversion.ConvertToBanglaNumber(model.ReportDataTable.Rows[i]["FirstPaymentAmount"].ToString());
                model.ReportDataTable.Rows[i]["SecondPaymentDate"] = BanglaConversion.ConvertToBanglaNumber(model.ReportDataTable.Rows[i]["SecondPaymentDate"].ToString());
                model.ReportDataTable.Rows[i]["SecondPaymentAmount"] = BanglaConversion.ConvertToBanglaNumber(model.ReportDataTable.Rows[i]["SecondPaymentAmount"].ToString());
            }

            averageWages = totalNetAmount / totalWorkingDays;
            totalMaternityAmount = 112 * averageWages;

            for (int i = 0; i < model.ReportDataTable.Rows.Count; i++)
            {
                model.ReportDataTable.Rows[i]["TotalNetAmount"] = BanglaConversion.ConvertToBanglaNumber(totalNetAmount.ToString());
                model.ReportDataTable.Rows[i]["TotalWorkingDays"] = BanglaConversion.ConvertToBanglaNumber(totalWorkingDays.ToString());
                model.ReportDataTable.Rows[i]["AverageWages"] = BanglaConversion.ConvertToBanglaNumber(averageWages.ToString());
                model.ReportDataTable.Rows[i]["TotalMaternityAmount"] = BanglaConversion.ConvertToBanglaNumber(totalMaternityAmount.ToString());
            }

            string companyId = PortalContext.CurrentUser.CompId;
            string companyName = HrmReportManager.GetCompanyNameByCompanyId(companyId);

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_CompanySector", companyName);

            ReportDataSource rd = new ReportDataSource("DataSet1", model.ReportDataTable);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);

            string reportType = "pdf";
            if (model.SearchFieldModel.PrintFormatId == 2)
                reportType = "Excel";

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>11.7in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0.4in</MarginTop>" +
                "  <MarginLeft>.4in</MarginLeft>" +
                "  <MarginRight>.2in</MarginRight>" +
                "  <MarginBottom>0.2in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        public ActionResult CMInfoIndex(AttendanceViewModel model)
        {
            //AttendanceIndex for more detail

            ModelState.Clear();

            try
            {
                model.StartDate = DateTime.Now;
                model.EndDate = DateTime.Now;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult CMInfo(AttendanceViewModel model)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "CMCalculation.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            string companyId = PortalContext.CurrentUser.CompId;
            string companyName = HrmReportManager.GetCompanyNameByCompanyId(companyId);

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_CompanySector", companyName);

            DateTime? fromDate = model.StartDate;
            DateTime? toDate = model.EndDate;

            List<SPCommCMInfo> cmInfo = HrmReportManager.GetCMInfo(fromDate, toDate);

            ReportDataSource rd = new ReportDataSource("DataSet1", cmInfo);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);

            string reportType = "pdf";
            if (model.SearchFieldModel.PrintFormatId == 2)
                reportType = "Excel";

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>14.0in</PageHeight>" +
                "  <MarginTop>0..0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        public ActionResult DailyOtIndex(AttendanceViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.StartDate = DateTime.Now;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult DailyOt(AttendanceViewModel model)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "DailyOverTime.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            string companyId = PortalContext.CurrentUser.CompId;
            string companyName = HrmReportManager.GetCompanyNameByCompanyId(companyId);

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_CompanySector", companyName);

            DateTime? fromDate = model.StartDate;

            List<HrmDailyOTReport> dailyOt = HrmReportManager.GetDailyOtReport(fromDate);

            ReportDataSource rd = new ReportDataSource("DataSet1", dailyOt);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);

            string reportType = "pdf";
            if (model.SearchFieldModel.PrintFormatId == 2)
                reportType = "Excel";

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0..0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        public ActionResult MonthlyOtSummaryIndex(AttendanceViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.StartDate = DateTime.Now;
                model.EndDate = DateTime.Now;

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult MonthlyOtSummary(AttendanceViewModel model)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "MonthlyOTSummary.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            string companyId = PortalContext.CurrentUser.CompId;
            string companyName = HrmReportManager.GetCompanyNameByCompanyId(companyId);

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_CompanySector", companyName);

            DateTime? fromDate = model.StartDate;
            DateTime? toDate = model.EndDate;

            List<HrmOTSummaryReport> monthlyOverTime = HrmReportManager.GetMonthlyOtSummaryReport(fromDate, toDate);

            ReportDataSource rd = new ReportDataSource("DataSet1", monthlyOverTime);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);

            string reportType = "pdf";
            if (model.SearchFieldModel.PrintFormatId == 2)
                reportType = "Excel";

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>14.0in</PageHeight>" +
                "  <MarginTop>0..0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        public ActionResult OverTimeAttendanceByTimeIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
                model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(model.SearchFieldModel.SearchByEmployeeGradeId);

                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.EmployeeStatuses = statusList;
                model.PrintFormatStatuses = printFormat;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult OverTimeAttendanceByTime(ReportSearchViewModel model)
        {
            ModelState.Clear();

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "AttendanceSearchByTime.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? employeeGradeId = model.SearchFieldModel.SearchByEmployeeGradeId;
            int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;

            string userName = PortalContext.CurrentUser.Name;
            DateTime? fromDate = model.FromDate;

            DateTime dateTime = DateTime.ParseExact(model.InTime, "hh:mm tt", CultureInfo.InvariantCulture);
            TimeSpan fromTime = dateTime.TimeOfDay;

            dateTime = DateTime.ParseExact(model.OutTime, "hh:mm tt", CultureInfo.InvariantCulture);
            TimeSpan toTime = dateTime.TimeOfDay;

            List<AttendanceSearchByTimeModel> attendanceInfo = HrmReportManager.GetAttendanceSearchByTime(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, userName, fromDate, fromTime, toTime);

            ReportDataSource rd = new ReportDataSource("DataSet1", attendanceInfo);
            lr.DataSources.Add(rd);

            string reportType = "pdf";
            if (model.SearchFieldModel.PrintFormatId == 2)
                reportType = "Excel";

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0..0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        public ActionResult EmployeeEarnLeaveIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
                model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(model.SearchFieldModel.SearchByEmployeeGradeId);

                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.EmployeeStatuses = statusList;
                model.PrintFormatStatuses = printFormat;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult EmployeeEarnLeave(ReportSearchViewModel model)
        {
            ModelState.Clear();
            var reportType = new ReportType();
            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }
            string employeeCardId = model.EmployeeCardId ?? "";
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? employeeGradeId = model.SearchFieldModel.SearchByEmployeeGradeId;
            int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;
            int? activeStatus = model.SearchFieldModel.SearchByEmployeeStatus;

            string userName = PortalContext.CurrentUser.Name;
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "EmployeeEarnLeaveNew.rdlc");
            model.BuyerOrderMasterDataTable = HrmReportManager.GetEmployeeEarnLeave(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId, userName, fromDate, toDate, activeStatus);

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var selectedDate = toDate.Value.ToString("MMMM dd, yyyy");

            var parameters = new List<ReportParameter>() { new ReportParameter("selectedDate", selectedDate) };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.BuyerOrderMasterDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, parameters);

        }

        public ActionResult EmployeeMonthwiseAttendenceIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
                model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(model.SearchFieldModel.SearchByEmployeeGradeId);

                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.EmployeeStatuses = statusList;
                model.PrintFormatStatuses = printFormat;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult EmployeeMonthwiseAttendence(ReportSearchViewModel model)
        {
            ModelState.Clear();
            var reportType = new ReportType();
            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }
            string employeeCardId = model.EmployeeCardId ?? "";
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? employeeGradeId = model.SearchFieldModel.SearchByEmployeeGradeId;
            int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;

            string userName = PortalContext.CurrentUser.Name;
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "YearlyAttendence.rdlc");
            model.BuyerOrderMasterDataTable = HrmReportManager.GetEmployeeMonthwiseAttendence(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId, userName, fromDate, toDate);

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var selectedYear = DateTime.Parse(fromDate.ToString()).Year.ToString();

            //var parameters = new List<ReportParameter>() {new ReportParameter("SelectedYear", selectedYear)};
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.BuyerOrderMasterDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);

        }

        public ActionResult EmployeeDailyAbsentReportIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
                model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(model.SearchFieldModel.SearchByEmployeeGradeId);

                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.EmployeeStatuses = statusList;
                model.PrintFormatStatuses = printFormat;
                model.FromDate = DateTime.Today;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult EmployeeDailyAbsentReport(ReportSearchViewModel model)
        {
            ModelState.Clear();
            var reportType = new ReportType();
            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }
            string employeeCardId = model.EmployeeCardId ?? "";
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? employeeGradeId = model.SearchFieldModel.SearchByEmployeeGradeId;
            int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;

            string userName = PortalContext.CurrentUser.Name;
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "EmployeeDailyAbsent.rdlc");
            model.BuyerOrderMasterDataTable = HrmReportManager.GetEmployeeDailyAbsent(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId.Trim(), userName, fromDate, toDate);

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var selectedYear = DateTime.Parse(fromDate.ToString()).Year.ToString();

            //var parameters = new List<ReportParameter>() { new ReportParameter("SelectedYear", selectedYear) };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.BuyerOrderMasterDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);

        }

        public ActionResult EmployeeDailyPresentReportIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
                model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(model.SearchFieldModel.SearchByEmployeeGradeId);

                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.EmployeeStatuses = statusList;
                model.PrintFormatStatuses = printFormat;
                model.FromDate = DateTime.Today;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult EmployeeDailyPresentReport(ReportSearchViewModel model)
        {
            ModelState.Clear();
            var reportType = new ReportType();

            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }
            string employeeCardId = model.EmployeeCardId ?? "";
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? employeeGradeId = model.SearchFieldModel.SearchByEmployeeGradeId;
            int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;

            string userName = PortalContext.CurrentUser.Name;
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "EmployeeDailyPresent.rdlc");
            model.BuyerOrderMasterDataTable = HrmReportManager.GetEmployeeDailyAttendance(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId.Trim(), userName, fromDate, toDate);

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var selectedYear = DateTime.Parse(fromDate.ToString()).Year.ToString();

            //var parameters = new List<ReportParameter>() { new ReportParameter("SelectedYear", selectedYear) };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.BuyerOrderMasterDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult EmployeeDailyPresentByDesignationReportIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
                model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(model.SearchFieldModel.SearchByEmployeeGradeId);

                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.EmployeeStatuses = statusList;
                model.PrintFormatStatuses = printFormat;
                model.FromDate = DateTime.Today;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult EmployeeDailyPresentByDesignationReport(ReportSearchViewModel model)
        {
            ModelState.Clear();
            var reportType = new ReportType();

            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }
            string employeeCardId = model.EmployeeCardId ?? "";
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? employeeGradeId = model.SearchFieldModel.SearchByEmployeeGradeId;
            int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;

            string userName = PortalContext.CurrentUser.Name;
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "EmployeeDailyPresentByDesignation.rdlc");
            model.BuyerOrderMasterDataTable = HrmReportManager.GetEmployeeDailyAttendanceByDesignation(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId.Trim(), userName, fromDate, toDate);

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var selectedYear = DateTime.Parse(fromDate.ToString()).Year.ToString();

            //var parameters = new List<ReportParameter>() { new ReportParameter("SelectedYear", selectedYear) };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.BuyerOrderMasterDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult EmployeePreviousAbsentTodayPresentReportIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
                model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(model.SearchFieldModel.SearchByEmployeeGradeId);

                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.EmployeeStatuses = statusList;
                model.PrintFormatStatuses = printFormat;
                model.FromDate = DateTime.Today;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult EmployeePreviousAbsentTodayPresentReport(ReportSearchViewModel model)
        {
            ModelState.Clear();
            var reportType = new ReportType();

            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }
            string employeeCardId = model.EmployeeCardId ?? "";
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? employeeGradeId = model.SearchFieldModel.SearchByEmployeeGradeId;
            int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;

            string userName = PortalContext.CurrentUser.Name;
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "PreviousDayAbsentTodayPresentReport.rdlc");
            model.BuyerOrderMasterDataTable = HrmReportManager.GetEmployeeDailyAttendanceButPreviousDayAbsent(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId.Trim(), userName, fromDate, toDate);
            var results = from myRow in model.BuyerOrderMasterDataTable.AsEnumerable()
                          where myRow.Field<string>("PastRemarks") == "Absent" && (myRow.Field<string>("Remarks") == "Present" || myRow.Field<string>("Remarks") == "Late")
                          select myRow;
            DataTable boundTable = results.CopyToDataTable<DataRow>();

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            //var selectedYear = DateTime.Parse(fromDate.ToString()).Year.ToString();

            //var parameters = new List<ReportParameter>() { new ReportParameter("SelectedDate", fromDate.ToString()) };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", boundTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult EmployeeWeeklyWorkingHoursDetailsIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
                model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(model.SearchFieldModel.SearchByEmployeeGradeId);

                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.EmployeeStatuses = statusList;
                model.PrintFormatStatuses = printFormat;
                model.FromDate = DateTime.Today;
                model.ToDate = DateTime.Today;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult EmployeeWeeklyWorkingHoursDetailsReport(ReportSearchViewModel model)
        {
            ModelState.Clear();
            var reportType = new ReportType();

            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }
            string employeeCardId = model.EmployeeCardId ?? "";
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            //int? employeeGradeId = model.SearchFieldModel.SearchByEmployeeGradeId;
            //int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;

            string userName = PortalContext.CurrentUser.Name;
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "WeeklyEmployeeWorkingHoursDetails.rdlc");
            model.BuyerOrderMasterDataTable = HrmReportManager.GetEmployeeWorkingHoursDetails(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId.Trim(), userName, fromDate, toDate);



            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            //var selectedYear = DateTime.Parse(fromDate.ToString()).Year.ToString();

            //var parameters = new List<ReportParameter>() { new ReportParameter("SelectedDate", fromDate.ToString()) };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.BuyerOrderMasterDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult SkillMatrixPoint(string employeeCardId)
        {

            LocalReport lr = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "SkillMatrixPoint.rdlc");

            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportSearchViewModel model = new ReportSearchViewModel();
            model.ReportDataSource = HRMReportManager.GetSkillMatrixPoint(employeeCardId);
            model.ReportDataSourceTwo = HRMReportManager.GetSkillMatrixPointSecondPart(employeeCardId);

            ReportDataSource rd1 = new ReportDataSource("DataSet1", model.ReportDataSource);
            ReportDataSource rd2 = new ReportDataSource("DataSet2", model.ReportDataSourceTwo);

            lr.DataSources.Add(rd1);
            lr.DataSources.Add(rd2);

            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>11.7in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0..2in</MarginTop>" +
                "  <MarginLeft>.4in</MarginLeft>" +
                "  <MarginRight>.2in</MarginRight>" +
                "  <MarginBottom>0.2in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        public ActionResult SkillMatrixAllReportIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
                model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(model.SearchFieldModel.SearchByEmployeeGradeId);

                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.EmployeeStatuses = statusList;
                model.PrintFormatStatuses = printFormat;
                model.FromDate = DateTime.Today;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult SkillMatrixAllReport(ReportSearchViewModel model)
        {

            ModelState.Clear();

            var reportType = new ReportType();

            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }

            string employeeCardId = model.EmployeeCardId ?? "";
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "SkillMatrixAll.rdlc");

            model.BuyerOrderMasterDataTable = HrmReportManager.GetSkillMatrixAll(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId.Trim());

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.BuyerOrderMasterDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };

            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult ManPowerwithApprovedEmployeeIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                // model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                IEnumerable employeeType = from EmployeeTypeReport empType in Enum.GetValues(typeof(EmployeeTypeReport))
                                           select new { Id = (int)empType, Title = empType.ToString() };

                model.EmployeeTypes = employeeType;

                model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
                model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(model.SearchFieldModel.SearchByEmployeeGradeId);

                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.EmployeeStatuses = statusList;
                model.PrintFormatStatuses = printFormat;
                model.FromDate = DateTime.Today;
                model.ToDate = DateTime.Today;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult ManPowerwithApprovedEmployee(ReportSearchViewModel model)
        {
            ModelState.Clear();

            var reportType = new ReportType();

            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }

            string employeeCardId = model.EmployeeCardId ?? "";
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            DateTime? effectiveDate = model.ToDate;
            DateTime? fromDate = model.FromDate;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "EmployeeManpowerApproved.rdlc");

            model.BuyerOrderMasterDataTable = HrmReportManager.GetManpowerApprovedEmployee(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId.Trim(), effectiveDate, fromDate);

            var reportTitle = "";
            if (employeeTypeId == 2)
                reportTitle = "Staff";
            else
                reportTitle = "Team Member";

            var parameters = new List<ReportParameter>() { new ReportParameter("Param_ReportTitle", reportTitle) };

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.BuyerOrderMasterDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };

            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, parameters);
        }

        public ActionResult EmployeeNewJoinAndQuitSummaryIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult EmployeeNewJoinAndQuitSummary(ReportSearchViewModel model)
        {
            ModelState.Clear();

            var reportType = new ReportType();

            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }

            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int fromYear = Convert.ToInt32(model.SelectedYear);
            int toYear = Convert.ToInt32(model.SelectedToYear);
            int fromMonth = Convert.ToInt32(model.SelectedMonth);
            int toMonth = Convert.ToInt32(model.SelectedToMonth);
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "NewJoinAndQuitSummary.rdlc");

            DataTable data = HrmReportManager.EmployeeNewJoinAndQuitSummary(companyId, branchId, fromYear, toYear, fromMonth, toMonth, fromDate, toDate);

            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", data) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult DailyAdvanceOTAmountIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;
                model.FromDate = DateTime.Today;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult DailyAdvanceOTAmount(ReportSearchViewModel model)
        {
            ModelState.Clear();

            var reportType = new ReportType();

            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }

            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;

            DateTime? date = model.FromDate;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "AdvanceOTAmount.rdlc");

            var parameters = new List<ReportParameter>() { new ReportParameter("Param_ReportDate", model.FromDate.Value.ToString("dd/MMM/yyyy")) };

            model.BuyerOrderMasterDataTable = HrmReportManager.GetAdvanceOTAmount(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, date);

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.BuyerOrderMasterDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };

            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, parameters);
        }

        public ActionResult EmployeeDailyAbsentRootCauseReportIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
                model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(model.SearchFieldModel.SearchByEmployeeGradeId);

                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.EmployeeStatuses = statusList;
                model.PrintFormatStatuses = printFormat;
                model.FromDate = DateTime.Today;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult EmployeeDailyAbsentRootCauseReport(ReportSearchViewModel model)
        {

            ModelState.Clear();

            var reportType = new ReportType();

            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }

            string employeeCardId = model.EmployeeCardId ?? "";
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? employeeGradeId = model.SearchFieldModel.SearchByEmployeeGradeId;
            int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;

            string userName = PortalContext.CurrentUser.Name;
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "DailyAbsentRootCause.rdlc");
            model.BuyerOrderMasterDataTable = HrmReportManager.GetEmployeeDailyAbsentRootCause(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId.Trim(), userName, fromDate, toDate);

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            var selectedYear = DateTime.Parse(fromDate.ToString()).Year.ToString();

            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.BuyerOrderMasterDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult TiffinBillIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult TiffinBill(ReportSearchViewModel model)
        {
            ModelState.Clear();

            var reportType = new ReportType();

            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }

            string employeeCardId = model.EmployeeCardId ?? "";
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            DateTime? fromDate = model.FromDate;

            string path = string.Empty;

            if (branchUnitId == 3)
            {
                path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "TiffinBillDyeing.rdlc");
                model.BuyerOrderMasterDataTable = HrmReportManager.GetTiffinBillDyeing(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, "", "SuperAdmin", fromDate, model.All, model.Management, model.MiddleManagement, model.TeamMemberA, model.TeamMemberB);
            }
            else
            {
                path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "TiffinBill.rdlc");
                model.BuyerOrderMasterDataTable = HrmReportManager.GetTiffinBill(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, "", "SuperAdmin", fromDate, model.All, model.Management, model.MiddleManagement, model.TeamMemberA, model.TeamMemberB);
            }

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.BuyerOrderMasterDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };

            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult JobCardSummaryIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                var attendanceStatus = from AttendanceStatus attStatus in Enum.GetValues(typeof(AttendanceStatus))
                                       select new { Id = (int)attStatus, Name = attStatus.ToString() };
                model.AttendanceStatuses = attendanceStatus;


                var activeStatusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                       select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeActiveStatuses = activeStatusList;


                var employeeCategoryValueList = from EmployeeCategoryValue status in Enum.GetValues(typeof(EmployeeCategoryValue))
                                                select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeCategories = employeeCategoryValueList;


                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };
                model.PrintFormatStatuses = printFormat;


            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult JobCardSummary(ReportSearchViewModel model)
        {
            ModelState.Clear();

            var reportType = new ReportType();

            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }

            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? employeeGradeId = model.SearchFieldModel.SearchByEmployeeGradeId;
            int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;
            string employeeCardId = model.EmployeeCardId ?? "";

            string userName = PortalContext.CurrentUser.Name;
            DateTime? fromDate = model.SearchFieldModel.StartDate;
            DateTime? toDate = model.SearchFieldModel.EndDate;

            model.BuyerOrderMasterDataTable = HrmReportManager.GetJobCardSummary(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId.Trim(), userName, fromDate, toDate);

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "JobCardSummary.rdlc");

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            var selectedYear = DateTime.Parse(fromDate.ToString()).Year.ToString();

            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.BuyerOrderMasterDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult JobCardNoPenaltyIndex(DynamicReportHeadViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        //[AjaxAuthorize(Roles = "jobcard-1,jobcard-2,jobcard-3")]
        [ActionName("Jobcardnpy")]
        public ActionResult JobCardNoPenalty(DynamicReportHeadViewModel model)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "JobCardnpy.rdlc");
                if (System.IO.File.Exists(path))
                    lr.ReportPath = path;
                else
                    return View("Index");

                int? companyId = model.SearchFieldModel.SearchByCompanyId;
                int? branchId = model.SearchFieldModel.SearchByBranchId;
                int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
                int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
                int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
                int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
                int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
                string employeeCardId = model.SearchFieldModel.SearchByEmployeeCardId;

                int year = Convert.ToInt32(model.SearchFieldModel.SelectedYear);
                int month = Convert.ToInt32(model.SearchFieldModel.SelectedMonth);

                DateTime? fromDate = model.SearchFieldModel.StartDate;
                DateTime? toDate = model.SearchFieldModel.EndDate;

                string userName = PortalContext.CurrentUser.Name;


                List<JobCardInfoModel> jobCardInfo = HrmReportManager.GetJobCardInfoNoPenalty(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                    departmentLineId, employeeTypeId, employeeCardId, year, month,
                    fromDate, toDate, userName);


                ReportDataSource rd = new ReportDataSource("DataSetEmployeeJobCard", jobCardInfo);

                ReportParameter[] parameters = new ReportParameter[47];

                List<string> reportHeads = model.ReportHeads;

                if (reportHeads != null)
                {
                    if (reportHeads.Contains("Branch"))
                        parameters[0] = new ReportParameter("Param_Branch", "True");
                    else
                        parameters[0] = new ReportParameter("Param_Branch", "False");

                    if (reportHeads.Contains("Unit"))
                        parameters[1] = new ReportParameter("Param_Unit", "True");
                    else
                        parameters[1] = new ReportParameter("Param_Unit", "False");

                    if (reportHeads.Contains("Department"))
                        parameters[2] = new ReportParameter("Param_Department", "True");
                    else
                        parameters[2] = new ReportParameter("Param_Department", "False");

                    if (reportHeads.Contains("Section"))
                        parameters[3] = new ReportParameter("Param_Section", "True");
                    else
                        parameters[3] = new ReportParameter("Param_Section", "False");

                    if (reportHeads.Contains("Line"))
                        parameters[4] = new ReportParameter("Param_Line", "True");
                    else
                        parameters[4] = new ReportParameter("Param_Line", "False");

                    if (reportHeads.Contains("EmployeeType"))
                        parameters[5] = new ReportParameter("Param_EmployeeType", "True");
                    else
                        parameters[5] = new ReportParameter("Param_EmployeeType", "False");

                    if (reportHeads.Contains("EmployeeGrade"))
                        parameters[6] = new ReportParameter("Param_EmployeeGrade", "True");
                    else
                        parameters[6] = new ReportParameter("Param_EmployeeGrade", "False");

                    if (reportHeads.Contains("EmployeeDesignation"))
                        parameters[7] = new ReportParameter("Param_EmployeeDesignation", "True");
                    else
                        parameters[7] = new ReportParameter("Param_EmployeeDesignation", "False");

                    if (reportHeads.Contains("MobileNo"))
                        parameters[8] = new ReportParameter("Param_MobileNo", "True");
                    else
                        parameters[8] = new ReportParameter("Param_MobileNo", "False");

                    if (reportHeads.Contains("JoiningDate"))
                        parameters[9] = new ReportParameter("Param_JoiningDate", "True");
                    else
                        parameters[9] = new ReportParameter("Param_JoiningDate", "False");

                    if (reportHeads.Contains("QuitDate"))
                        parameters[10] = new ReportParameter("Param_QuitDate", "True");
                    else
                        parameters[10] = new ReportParameter("Param_QuitDate", "False");

                    if (reportHeads.Contains("GrossSalary"))
                        parameters[11] = new ReportParameter("Param_GrossSalary", "True");
                    else
                        parameters[11] = new ReportParameter("Param_GrossSalary", "False");

                    if (reportHeads.Contains("OTRate"))
                        parameters[12] = new ReportParameter("Param_OTRate", "True");
                    else
                        parameters[12] = new ReportParameter("Param_OTRate", "False");

                    if (reportHeads.Contains("PresentDays"))
                        parameters[13] = new ReportParameter("Param_PresentDays", "True");
                    else
                        parameters[13] = new ReportParameter("Param_PresentDays", "False");

                    if (reportHeads.Contains("LateDays"))
                        parameters[14] = new ReportParameter("Param_LateDays", "True");
                    else
                        parameters[14] = new ReportParameter("Param_LateDays", "False");

                    if (reportHeads.Contains("OSDDays"))
                        parameters[15] = new ReportParameter("Param_OSDDays", "True");
                    else
                        parameters[15] = new ReportParameter("Param_OSDDays", "False");

                    if (reportHeads.Contains("AbsentDays"))
                        parameters[16] = new ReportParameter("Param_AbsentDays", "True");
                    else
                        parameters[16] = new ReportParameter("Param_AbsentDays", "False");

                    if (reportHeads.Contains("LeaveDays"))
                        parameters[17] = new ReportParameter("Param_LeaveDays", "True");
                    else
                        parameters[17] = new ReportParameter("Param_LeaveDays", "False");

                    if (reportHeads.Contains("Holidays"))
                        parameters[18] = new ReportParameter("Param_Holidays", "True");
                    else
                        parameters[18] = new ReportParameter("Param_Holidays", "False");

                    if (reportHeads.Contains("WeekendDays"))
                        parameters[19] = new ReportParameter("Param_WeekendDays", "True");
                    else
                        parameters[19] = new ReportParameter("Param_WeekendDays", "False");

                    if (reportHeads.Contains("LWP"))
                        parameters[20] = new ReportParameter("Param_LWP", "True");
                    else
                        parameters[20] = new ReportParameter("Param_LWP", "False");

                    if (reportHeads.Contains("TotalDays"))
                        parameters[21] = new ReportParameter("Param_TotalDays", "True");
                    else
                        parameters[21] = new ReportParameter("Param_TotalDays", "False");

                    if (reportHeads.Contains("WorkingDays"))
                        parameters[22] = new ReportParameter("Param_WorkingDays", "True");
                    else
                        parameters[22] = new ReportParameter("Param_WorkingDays", "False");

                    if (reportHeads.Contains("PayDays"))
                        parameters[23] = new ReportParameter("Param_PayDays", "True");
                    else
                        parameters[23] = new ReportParameter("Param_PayDays", "False");


                    if (reportHeads.Contains("OTHourLast"))
                        parameters[24] = new ReportParameter("Param_OTHourLast", "True");
                    else
                        parameters[24] = new ReportParameter("Param_OTHourLast", "False");


                    if (reportHeads.Contains("TotalExtraOTHours"))
                        parameters[25] = new ReportParameter("Param_TotalExtraOTHours", "True");
                    else
                        parameters[25] = new ReportParameter("Param_TotalExtraOTHours", "False");


                    if (reportHeads.Contains("TotalHolidayOTHours"))
                        parameters[26] = new ReportParameter("Param_TotalHolidayOTHours", "True");
                    else
                        parameters[26] = new ReportParameter("Param_TotalHolidayOTHours", "False");


                    if (reportHeads.Contains("TotalWeekendOTHours"))
                        parameters[27] = new ReportParameter("Param_TotalWeekendOTHours", "True");
                    else
                        parameters[27] = new ReportParameter("Param_TotalWeekendOTHours", "False");


                    if (reportHeads.Contains("Date"))
                        parameters[28] = new ReportParameter("Param_Date", "True");
                    else
                        parameters[28] = new ReportParameter("Param_Date", "False");


                    if (reportHeads.Contains("DayName"))
                        parameters[29] = new ReportParameter("Param_DayName", "True");
                    else
                        parameters[29] = new ReportParameter("Param_DayName", "False");


                    if (reportHeads.Contains("Shift"))
                        parameters[30] = new ReportParameter("Param_Shift", "True");
                    else
                        parameters[30] = new ReportParameter("Param_Shift", "False");


                    if (reportHeads.Contains("Status"))
                        parameters[31] = new ReportParameter("Param_Status", "True");
                    else
                        parameters[31] = new ReportParameter("Param_Status", "False");

                    if (reportHeads.Contains("InTime"))
                        parameters[32] = new ReportParameter("Param_InTime", "True");
                    else
                        parameters[32] = new ReportParameter("Param_InTime", "False");

                    if (reportHeads.Contains("OutTime"))
                        parameters[33] = new ReportParameter("Param_OutTime", "True");
                    else
                        parameters[33] = new ReportParameter("Param_OutTime", "False");

                    if (reportHeads.Contains("Delay"))
                        parameters[34] = new ReportParameter("Param_Delay", "True");
                    else
                        parameters[34] = new ReportParameter("Param_Delay", "False");

                    if (reportHeads.Contains("OTHours"))
                        parameters[35] = new ReportParameter("Param_OTHours", "True");
                    else
                        parameters[35] = new ReportParameter("Param_OTHours", "False");

                    if (reportHeads.Contains("ExtraOTHours"))
                        parameters[36] = new ReportParameter("Param_ExtraOTHours", "True");
                    else
                        parameters[36] = new ReportParameter("Param_ExtraOTHours", "False");

                    if (reportHeads.Contains("HolidayOTHours"))
                        parameters[37] = new ReportParameter("Param_HolidayOTHours", "True");
                    else
                        parameters[37] = new ReportParameter("Param_HolidayOTHours", "False");

                    if (reportHeads.Contains("WeekendOTHours"))
                        parameters[38] = new ReportParameter("Param_WeekendOTHours", "True");
                    else
                        parameters[38] = new ReportParameter("Param_WeekendOTHours", "False");

                    if (reportHeads.Contains("Remarks"))
                        parameters[39] = new ReportParameter("Param_Remarks", "True");
                    else
                        parameters[39] = new ReportParameter("Param_Remarks", "False");

                    if (reportHeads.Contains("TotalPenaltyOTHours"))
                        parameters[40] = new ReportParameter("Param_TotalPenaltyOTHours", "True");
                    else
                        parameters[40] = new ReportParameter("Param_TotalPenaltyOTHours", "False");

                    if (reportHeads.Contains("TotalPenaltyAttendanceDays"))
                        parameters[41] = new ReportParameter("Param_TotalPenaltyAttendanceDays", "True");
                    else
                        parameters[41] = new ReportParameter("Param_TotalPenaltyAttendanceDays", "False");

                    if (reportHeads.Contains("TotalPenaltyLeaveDays"))
                        parameters[42] = new ReportParameter("Param_TotalPenaltyLeaveDays", "True");
                    else
                        parameters[42] = new ReportParameter("Param_TotalPenaltyLeaveDays", "False");

                    if (reportHeads.Contains("TotalPenaltyFinancialAmount"))
                        parameters[43] = new ReportParameter("Param_TotalPenaltyFinancialAmount", "True");
                    else
                        parameters[43] = new ReportParameter("Param_TotalPenaltyFinancialAmount", "False");

                    if (reportHeads.Contains("LastIncrementDate"))
                        parameters[44] = new ReportParameter("Param_LastIncrementDate", "True");
                    else
                        parameters[44] = new ReportParameter("Param_LastIncrementDate", "False");

                    if (reportHeads.Contains("SkillType"))
                        parameters[45] = new ReportParameter("Param_SkillType", "True");
                    else
                        parameters[45] = new ReportParameter("Param_SkillType", "False");

                    DateTime paramFromDate = new DateTime(2017, 5, 26);
                    DateTime paramToDate = new DateTime(2017, 6, 25);

                    if (jobCardInfo[0].Unit == "Garments")
                    {
                        if (jobCardInfo[0].Date >= paramFromDate && jobCardInfo[0].Date <= paramToDate)
                            parameters[46] = new ReportParameter("Param_Note", "Lunch Break 30 Minutes");
                        else
                            parameters[46] = new ReportParameter("Param_Note", "Lunch Break 1 Hour");
                    }
                    else
                        parameters[46] = new ReportParameter("Param_Note", "10.30 to 11.00 Refreshment Break \n            04.30 to 05.00 Refreshment Break");

                    lr.SetParameters(parameters);

                    lr.DataSources.Add(rd);
                }

                string reportType = "pdf";
                if (model.SearchFieldModel.PrintFormatId == 2)
                    reportType = "Excel";

                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =
                    "<DeviceInfo>" +
                    "  <OutputFormat>" + 2 + "</OutputFormat>" +
                    "  <PageWidth>8.5in</PageWidth>" +
                    "  <PageHeight>11.7in</PageHeight>" +
                    "  <MarginTop>0.4in</MarginTop>" +
                    "  <MarginLeft>.4in</MarginLeft>" +
                    "  <MarginRight>.2in</MarginRight>" +
                    "  <MarginBottom>0.2in</MarginBottom>" +
                    "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;

                byte[] renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                return File(renderedBytes, mimeType);
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return JobCardIndex(model);
        }

        public ActionResult AgeAndFitnessCertificateIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();
            model.Date = DateTime.Now;
            try
            {

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult AgeAndFitnessCertificateDate(ReportSearchViewModel searchModel)
        {
            ModelState.Clear();

            Guid EmployeeId = new Guid();
            Employee employee = new Employee();
            EmployeeId = employee.EmployeeId;
            var model = new EmployeeAppointmentViewModel();
            string userName = PortalContext.CurrentUser.Name;

            if (!string.IsNullOrEmpty(searchModel.SearchByEmployeeCardId))
                employee = EmployeeManager.GetEmployeeByCardId(searchModel.SearchByEmployeeCardId);

            if (employee == null)
                return ErrorResult("Invalid Employee Id or Access Denied!");

            if (searchModel.Date != null)
            {
                DateTime prepareDate = searchModel.Date.Value;
                model.AppointmentLetterInfo = HrmReportManager.GetAgeAndFitnessCertificateInfo(employee.EmployeeId, userName, prepareDate);
            }
            return PartialView("_AgeAndFitnessCertificate", model);
        }

        public ActionResult JobApplicationIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult JobApplication(ReportSearchViewModel searchModel)
        {
            ModelState.Clear();

            Guid EmployeeId = new Guid();
            Employee employee = new Employee();
            EmployeeId = employee.EmployeeId;
            var model = new EmployeeAppointmentViewModel();
            string userName = PortalContext.CurrentUser.Name;

            if (!string.IsNullOrEmpty(searchModel.SearchByEmployeeCardId))
                employee = EmployeeManager.GetEmployeeByCardId(searchModel.SearchByEmployeeCardId);

            if (employee == null)
                return ErrorResult("Invalid Employee Id or Access denied!");

            if (searchModel.Date != null)
            {
                DateTime prepareDate = searchModel.Date.Value;
                model.AppointmentLetterInfo = HrmReportManager.GetJobApplicationInfo(employee.EmployeeId, userName, prepareDate);
            }
            return PartialView("_JobApplication", model);
        }

        public ActionResult JobVerificationIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult JobVerification(ReportSearchViewModel searchModel)
        {
            ModelState.Clear();

            Guid EmployeeId = new Guid();
            Employee employee = new Employee();
            EmployeeId = employee.EmployeeId;
            var model = new EmployeeAppointmentViewModel();
            Guid? UserId = PortalContext.CurrentUser.UserId;

            if (!string.IsNullOrEmpty(searchModel.SearchByEmployeeCardId))
                employee = EmployeeManager.GetEmployeeByCardId(searchModel.SearchByEmployeeCardId);

            if (employee == null)
                return ErrorResult("Invalid Employee Id or Access denied!");

            if (searchModel.Date != null)
            {
                DateTime prepareDate = searchModel.Date.Value;
                model.AppointmentLetterInfo = HrmReportManager.GetJobVerificationInfo(employee.EmployeeId, UserId, prepareDate);
            }
            return PartialView("_JobVerification", model);
        }

        public ActionResult LeaveApplicationIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult LeaveApplication(ReportSearchViewModel searchModel)
        {
            ModelState.Clear();

            Guid EmployeeId = new Guid();
            Employee employee = new Employee();
            EmployeeId = employee.EmployeeId;
            var model = new EmployeeAppointmentViewModel();
            string userName = PortalContext.CurrentUser.Name;

            if (!string.IsNullOrEmpty(searchModel.SearchByEmployeeCardId))
                employee = EmployeeManager.GetEmployeeByCardId(searchModel.SearchByEmployeeCardId);

            if (employee == null)
                return ErrorResult("Invalid Employee Id or Access denied!");

            if (searchModel.Date != null)
            {
                DateTime prepareDate = searchModel.Date.Value;
                model.AppointmentLetterInfo = HrmReportManager.GetLeaveApplicationWorkerInfo(employee.EmployeeId, employee.EmployeeCardId, userName, prepareDate);
            }
            return PartialView("_LeaveApplicationWorker", model);
        }

        public ActionResult EmployeeLeaveSummaryIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult EmployeeLeaveSummary(ReportSearchViewModel model)
        {

            ModelState.Clear();

            var reportType = new ReportType();

            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }

            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            string employeeCardId = model.EmployeeCardId ?? "";

            DateTime? fromDate = model.SearchFieldModel.StartDate;
            DateTime? toDate = model.SearchFieldModel.EndDate;

            model.BuyerOrderMasterDataTable = HrmReportManager.GetEmployeeLeaveSummary(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, employeeTypeId, employeeCardId.Trim(), fromDate, toDate);

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "EmployeeLeaveSummary.rdlc");

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            var selectedYear = DateTime.Parse(fromDate.ToString()).Year.ToString();

            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.BuyerOrderMasterDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult LeaveApplicationStaffIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult LeaveApplicationStaff(ReportSearchViewModel searchModel)
        {
            ModelState.Clear();

            Guid EmployeeId = new Guid();
            Employee employee = new Employee();
            EmployeeId = employee.EmployeeId;
            var model = new EmployeeAppointmentViewModel();
            string userName = PortalContext.CurrentUser.Name;

            if (!string.IsNullOrEmpty(searchModel.SearchByEmployeeCardId))
                employee = EmployeeManager.GetEmployeeByCardId(searchModel.SearchByEmployeeCardId);

            if (employee == null)
                return ErrorResult("Invalid Employee Id or Access denied!");

            if (searchModel.Date != null)
            {
                DateTime prepareDate = searchModel.Date.Value;
                model.AppointmentLetterInfo = HrmReportManager.GetLeaveApplicationStaffInfo(employee.EmployeeId, employee.EmployeeCardId, userName, prepareDate);
            }

            return PartialView("_LeaveApplicationStaff", model);
        }

        public ActionResult NominationFormIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult NominationForm(ReportSearchViewModel searchModel)
        {
            ModelState.Clear();

            Guid EmployeeId = new Guid();
            Employee employee = new Employee();
            EmployeeId = employee.EmployeeId;
            var model = new EmployeeAppointmentViewModel();
            string userName = PortalContext.CurrentUser.Name;

            if (!string.IsNullOrEmpty(searchModel.SearchByEmployeeCardId))
                employee = EmployeeManager.GetEmployeeByCardId(searchModel.SearchByEmployeeCardId);

            if (employee == null)
                return ErrorResult("Invalid Employee Id or Access Denied!");

            if (searchModel.Date != null)
            {
                DateTime prepareDate = searchModel.Date.Value;
                model.AppointmentLetterInfo = HrmReportManager.GetNominationFormInfo(employee.EmployeeId, userName, prepareDate);
            }
            return PartialView("_NominationForm", model);
        }

        public ActionResult ManPowerBudgetIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
                model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(model.SearchFieldModel.SearchByEmployeeGradeId);

                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.EmployeeStatuses = statusList;
                model.PrintFormatStatuses = printFormat;
                model.FromDate = DateTime.Today;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult ManPowerBudget(ReportSearchViewModel model)
        {
            ModelState.Clear();

            var reportType = new ReportType();

            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }

            string employeeCardId = model.EmployeeCardId ?? "";
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? employeeGradeId = model.SearchFieldModel.SearchByEmployeeGradeId;
            int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;

            string userName = PortalContext.CurrentUser.Name;
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "ManPowerBudget.rdlc");

            model.BuyerOrderMasterDataTable = HrmReportManager.GetManPowerBudget(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId.Trim(), userName, fromDate, toDate);

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            var selectedYear = DateTime.Parse(fromDate.ToString()).Year.ToString();
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.BuyerOrderMasterDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };

            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult DailyOTDetailWithAmountIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
                model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(model.SearchFieldModel.SearchByEmployeeGradeId);

                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.EmployeeStatuses = statusList;
                model.PrintFormatStatuses = printFormat;
                model.FromDate = DateTime.Today;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult DailyOTDetailWithAmount(ReportSearchViewModel model)
        {
            ModelState.Clear();

            var reportType = new ReportType();

            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }

            string employeeCardId = model.EmployeeCardId ?? "";
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? employeeGradeId = model.SearchFieldModel.SearchByEmployeeGradeId;
            int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;

            string userName = PortalContext.CurrentUser.Name;
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "DailyOTDetail.rdlc");

            model.BuyerOrderMasterDataTable = HrmReportManager.GetDailyOTDetailWithAmount(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId.Trim(), userName, fromDate, toDate);

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            var selectedYear = DateTime.Parse(fromDate.ToString()).Year.ToString();
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.BuyerOrderMasterDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };

            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult EmployeeEarnLeaveQuitEmployeeIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
                model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
                model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(model.SearchFieldModel.SearchByEmployeeGradeId);

                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.EmployeeStatuses = statusList;
                model.PrintFormatStatuses = printFormat;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult EmployeeEarnLeaveQuitEmployee(ReportSearchViewModel model)
        {
            ModelState.Clear();
            var reportType = new ReportType();
            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }
            string employeeCardId = model.EmployeeCardId ?? "";
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            int? employeeGradeId = model.SearchFieldModel.SearchByEmployeeGradeId;
            int? employeeDesignationId = model.SearchFieldModel.SearchByEmployeeDesignationId;

            string userName = PortalContext.CurrentUser.Name;
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "EmployeeEarnLeaveQuitEmployee.rdlc");
            model.BuyerOrderMasterDataTable = HrmReportManager.GetEmployeeEarnLeaveQuit(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId, employeeCardId, userName, fromDate, toDate);

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var selectedDate = toDate.Value.ToString("MMMM dd, yyyy");

            var parameters = new List<ReportParameter>() { new ReportParameter("selectedDate", selectedDate) };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.BuyerOrderMasterDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, parameters);

        }

        public ActionResult FemaleConsentLetterIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.SearchFieldModel.EffectiveFromDate = DateTime.Now;
                model.SearchFieldModel.DisagreeDate = DateTime.Now;


                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult FemaleConsentLetter(ReportSearchViewModel model)
        {
            ModelState.Clear();

            var reportType = new ReportType();

            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }

            string employeeCardId = model.SearchFieldModel.SearchByEmployeeCardId ?? "";
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;

            DateTime? disagreeDate = model.SearchFieldModel.DisagreeDate;
            DateTime? effectiveDate = model.SearchFieldModel.EffectiveFromDate;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "FemaleConsentLetter.rdlc");
            model.ReportDataSource = HrmReportManager.GetFemaleConsentLetterInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, employeeCardId, disagreeDate, effectiveDate);

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.ReportDataSource) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);

        }

        public ActionResult JobCardModelKnittingDyeingIndex(DynamicReportHeadViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult JobCardModelKnittingDyeing(DynamicReportHeadViewModel model)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "EmployeeJobCardModelKnittingDyeing.rdlc");
                if (System.IO.File.Exists(path))
                    lr.ReportPath = path;
                else
                    return View("Index");

                int? companyId = model.SearchFieldModel.SearchByCompanyId;
                int? branchId = model.SearchFieldModel.SearchByBranchId;
                int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
                int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
                int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
                int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
                int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
                string employeeCardId = model.SearchFieldModel.SearchByEmployeeCardId;

                int year = Convert.ToInt32(model.SearchFieldModel.SelectedYear);
                int month = Convert.ToInt32(model.SearchFieldModel.SelectedMonth);

                DateTime? fromDate = model.SearchFieldModel.StartDate;
                DateTime? toDate = model.SearchFieldModel.EndDate;

                string userName = PortalContext.CurrentUser.Name;


                List<JobCardInfoModel> jobCardModelInfo = HrmReportManager.GetJobCardModelKnittingDyeingInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                    departmentLineId, employeeTypeId, employeeCardId, year, month,
                    fromDate, toDate, userName);


                ReportDataSource rd = new ReportDataSource("DataSetEmployeeJobCardModel", jobCardModelInfo);
                lr.DataSources.Add(rd);

                ReportParameter[] parameters = new ReportParameter[36];

                List<string> reportHeads = model.ReportHeads;

                if (reportHeads != null)
                {

                    if (reportHeads.Contains("AbsentDays"))
                        parameters[0] = new ReportParameter("Param_AbsentDays", "True");
                    else
                        parameters[0] = new ReportParameter("Param_AbsentDays", "False");

                    if (reportHeads.Contains("Branch"))
                        parameters[1] = new ReportParameter("Param_Branch", "True");
                    else
                        parameters[1] = new ReportParameter("Param_Branch", "False");

                    if (reportHeads.Contains("Date"))
                        parameters[2] = new ReportParameter("Param_Date", "True");
                    else
                        parameters[2] = new ReportParameter("Param_Date", "False");

                    if (reportHeads.Contains("DayName"))
                        parameters[3] = new ReportParameter("Param_DayName", "True");
                    else
                        parameters[3] = new ReportParameter("Param_DayName", "False");

                    if (reportHeads.Contains("Delay"))
                        parameters[4] = new ReportParameter("Param_Delay", "True");
                    else
                        parameters[4] = new ReportParameter("Param_Delay", "False");

                    if (reportHeads.Contains("Department"))
                        parameters[5] = new ReportParameter("Param_Department", "True");
                    else
                        parameters[5] = new ReportParameter("Param_Department", "False");

                    if (reportHeads.Contains("EmployeeCardId"))
                        parameters[6] = new ReportParameter("Param_EmployeeCardId", "True");
                    else
                        parameters[6] = new ReportParameter("Param_EmployeeCardId", "False");

                    if (reportHeads.Contains("EmployeeDesignation"))
                        parameters[7] = new ReportParameter("Param_EmployeeDesignation", "True");
                    else
                        parameters[7] = new ReportParameter("Param_EmployeeDesignation", "False");

                    if (reportHeads.Contains("EmployeeGrade"))
                        parameters[8] = new ReportParameter("Param_EmployeeGrade", "True");
                    else
                        parameters[8] = new ReportParameter("Param_EmployeeGrade", "False");

                    if (reportHeads.Contains("EmployeeType"))
                        parameters[9] = new ReportParameter("Param_EmployeeType", "True");
                    else
                        parameters[9] = new ReportParameter("Param_EmployeeType", "False");

                    if (reportHeads.Contains("GrossSalary"))
                        parameters[10] = new ReportParameter("Param_GrossSalary", "True");
                    else
                        parameters[10] = new ReportParameter("Param_GrossSalary", "False");

                    if (reportHeads.Contains("Holidays"))
                        parameters[11] = new ReportParameter("Param_Holidays", "True");
                    else
                        parameters[11] = new ReportParameter("Param_Holidays", "False");

                    if (reportHeads.Contains("InTime"))
                        parameters[12] = new ReportParameter("Param_InTime", "True");
                    else
                        parameters[12] = new ReportParameter("Param_InTime", "False");

                    if (reportHeads.Contains("JoiningDate"))
                        parameters[13] = new ReportParameter("Param_JoiningDate", "True");
                    else
                        parameters[13] = new ReportParameter("Param_JoiningDate", "False");

                    if (reportHeads.Contains("LateDays"))
                        parameters[14] = new ReportParameter("Param_LateDays", "True");
                    else
                        parameters[14] = new ReportParameter("Param_LateDays", "False");

                    if (reportHeads.Contains("LeaveDays"))
                        parameters[15] = new ReportParameter("Param_LeaveDays", "True");
                    else
                        parameters[15] = new ReportParameter("Param_LeaveDays", "False");

                    if (reportHeads.Contains("Line"))
                        parameters[16] = new ReportParameter("Param_Line", "True");
                    else
                        parameters[16] = new ReportParameter("Param_Line", "False");

                    if (reportHeads.Contains("LWP"))
                        parameters[17] = new ReportParameter("Param_LWP", "True");
                    else
                        parameters[17] = new ReportParameter("Param_LWP", "False");

                    if (reportHeads.Contains("MobileNo"))
                        parameters[18] = new ReportParameter("Param_MobileNo", "True");
                    else
                        parameters[18] = new ReportParameter("Param_MobileNo", "False");

                    if (reportHeads.Contains("Name"))
                        parameters[19] = new ReportParameter("Param_Name", "True");
                    else
                        parameters[19] = new ReportParameter("Param_Name", "False");

                    if (reportHeads.Contains("OSDDays"))
                        parameters[20] = new ReportParameter("Param_OSDDays", "True");
                    else
                        parameters[20] = new ReportParameter("Param_OSDDays", "False");

                    if (reportHeads.Contains("OTHours"))
                        parameters[21] = new ReportParameter("Param_OTHours", "True");
                    else
                        parameters[21] = new ReportParameter("Param_OTHours", "False");

                    if (reportHeads.Contains("OTRate"))
                        parameters[22] = new ReportParameter("Param_OTRate", "True");
                    else
                        parameters[22] = new ReportParameter("Param_OTRate", "False");

                    if (reportHeads.Contains("OutTime"))
                        parameters[23] = new ReportParameter("Param_OutTime", "True");
                    else
                        parameters[23] = new ReportParameter("Param_OutTime", "False");

                    if (reportHeads.Contains("PayDays"))
                        parameters[24] = new ReportParameter("Param_PayDays", "True");
                    else
                        parameters[24] = new ReportParameter("Param_PayDays", "False");

                    if (reportHeads.Contains("PresentDays"))
                        parameters[25] = new ReportParameter("Param_PresentDays", "True");
                    else
                        parameters[25] = new ReportParameter("Param_PresentDays", "False");

                    if (reportHeads.Contains("QuitDate"))
                        parameters[26] = new ReportParameter("Param_QuitDate", "True");
                    else
                        parameters[26] = new ReportParameter("Param_QuitDate", "False");

                    if (reportHeads.Contains("Remarks"))
                        parameters[27] = new ReportParameter("Param_Remarks", "True");
                    else
                        parameters[27] = new ReportParameter("Param_Remarks", "False");

                    if (reportHeads.Contains("Section"))
                        parameters[28] = new ReportParameter("Param_Section", "True");
                    else
                        parameters[28] = new ReportParameter("Param_Section", "False");

                    if (reportHeads.Contains("Shift"))
                        parameters[29] = new ReportParameter("Param_Shift", "True");
                    else
                        parameters[29] = new ReportParameter("Param_Shift", "False");

                    if (reportHeads.Contains("Status"))
                        parameters[30] = new ReportParameter("Param_Status", "True");
                    else
                        parameters[30] = new ReportParameter("Param_Status", "False");

                    if (reportHeads.Contains("TotalDays"))
                        parameters[31] = new ReportParameter("Param_TotalDays", "True");
                    else
                        parameters[31] = new ReportParameter("Param_TotalDays", "False");

                    if (reportHeads.Contains("OTHourLast"))
                        parameters[32] = new ReportParameter("Param_OTHourLast", "True");
                    else
                        parameters[32] = new ReportParameter("Param_OTHourLast", "False");

                    if (reportHeads.Contains("Unit"))
                        parameters[33] = new ReportParameter("Param_Unit", "True");
                    else
                        parameters[33] = new ReportParameter("Param_Unit", "False");

                    if (reportHeads.Contains("WeekendDays"))
                        parameters[34] = new ReportParameter("Param_WeekendDays", "True");
                    else
                        parameters[34] = new ReportParameter("Param_WeekendDays", "False");

                    if (reportHeads.Contains("WorkingDays"))
                        parameters[35] = new ReportParameter("Param_WorkingDays", "True");
                    else
                        parameters[35] = new ReportParameter("Param_WorkingDays", "False");


                    lr.SetParameters(parameters);

                    lr.DataSources.Add(rd);
                }


                string reportType = "pdf";
                if (model.SearchFieldModel.PrintFormatId == 2)
                    reportType = "Excel";

                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =
                    "<DeviceInfo>" +
                    "  <OutputFormat>" + 2 + "</OutputFormat>" +
                    "  <PageWidth>8.5in</PageWidth>" +
                    "  <PageHeight>11.7in</PageHeight>" +
                    "  <MarginTop>0.4in</MarginTop>" +
                    "  <MarginLeft>.4in</MarginLeft>" +
                    "  <MarginRight>.2in</MarginRight>" +
                    "  <MarginBottom>0.2in</MarginBottom>" +
                    "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;

                byte[] renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                return File(renderedBytes, mimeType);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return JobCardModelIndex(model);
        }

        public ActionResult EmployeeLeaveRegisterIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult EmployeeLeaveRegister(ReportSearchViewModel model)
        {
            ModelState.Clear();

            var reportType = new ReportType();

            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 0:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }

            string employeeCardId = model.SearchByEmployeeCardId ?? "";
            DateTime effectiveDate = model.SearchFieldModel.EffectiveFromDate.Value;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "LeaveRegister.rdlc");
            model.ReportDataSource = HrmReportManager.GetEmployeeLeaveRegister(employeeCardId, effectiveDate);

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            var parameters = new List<ReportParameter>() { new ReportParameter("Param_Year", effectiveDate.Year.ToString()) };

            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.ReportDataSource) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, parameters);
        }

        public ActionResult DyeingShiftCountIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult DyeingShiftCount(ReportSearchViewModel model)
        {
            ModelState.Clear();

            var reportType = new ReportType();

            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }       
       
            DateTime fromDate = model.FromDate.Value;
            DateTime toDate = model.ToDate.Value;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "DyeingShiftCount.rdlc");

            model.ReportDataSource = HrmReportManager.GetDyeingShiftCount(fromDate, toDate);

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.ReportDataSource) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult LeaveButPresentIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult LeaveButPresent(ReportSearchViewModel model)
        {
            ModelState.Clear();

            var reportType = new ReportType();

            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }

            DateTime fromDate = model.FromDate.Value;
            DateTime toDate = model.ToDate.Value;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "LeaveButPresent.rdlc");

            model.ReportDataSource = HrmReportManager.GetLeaveButPresent(fromDate, toDate);

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.ReportDataSource) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult AbsentOnJoiningDateIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult AbsentOnJoiningDate(ReportSearchViewModel model)
        {
            ModelState.Clear();

            var reportType = new ReportType();

            switch (model.SearchFieldModel.PrintFormatId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 2:
                    reportType = ReportType.Excel;
                    break;
            }

            DateTime fromDate = model.FromDate.Value;
            DateTime toDate = model.ToDate.Value;

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "AbsentOnJoiningDate.rdlc");

            model.ReportDataSource = HrmReportManager.GetAbsentOnJoiningDate(fromDate, toDate);

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", model.ReportDataSource) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }
    }
}