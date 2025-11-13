using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.EnterpriseServices;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Microsoft.Reporting.WebForms;
using System.IO;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Web.Areas.HRM.Controllers;
using SCERP.Web.Areas.Commercial.Controllers;
using SCERP.Web.Areas.Payroll.Models.ViewModels;
using System.Text;
using SCERP.Model.PayrollModel;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Payroll.Controllers
{
    public class ReportController : BaseHrmController
    {
        public ActionResult Index()
        {
            return View();
        }

        [AjaxAuthorize(Roles = "payslip-2,payslip-3")]
        public ActionResult PaySlipIndex(ReportSearchViewModel model)
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

                var employeeCategoryValueList = from EmployeeCategoryValue status in Enum.GetValues(typeof(EmployeeCategoryValue))
                                                select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeCategories = employeeCategoryValueList;
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "payslip-2,payslip-3")]
        public ActionResult PaySlip(ReportSearchViewModel model)
        {
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            string employeeCardId = model.EmployeeCardId;
            int year = Convert.ToInt32(model.SelectedYear);
            int month = Convert.ToInt32(model.SelectedMonth);
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;
            int employeeCategoryId = model.EmployeeCategory;
            LocalReport lr = new LocalReport();

            string path = string.Empty;

            if (employeeTypeId == (int)EmployeeTypeId.TeamMemberA || employeeTypeId == (int)EmployeeTypeId.TeamMemberB)
            {

                path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "PaySlip.rdlc");
            }
            else
            {
                path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "PaySlipManager.rdlc");
            }

            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            var paySlip = new List<PaySlipView>();

            paySlip = PayrollReportManager.GetPaySlipInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId);

            var paySlipInBenglai = new List<PaySlipView>();

            var serialNo = 0;

            foreach (var t in paySlip)
            {
                //DateTime dt = DateTime.Now;
                //DateTime dt2 = DateTime.Now.AddDays(30);

                t.Month = BanglaConversion.ConvertToBanglaMonth(t.Month);
                t.Year = BanglaConversion.ConvertToBanglaNumber(t.Year);
                t.JoiningDate = BanglaConversion.ConvertToBanglaNumber(t.JoiningDate);
                t.BasicSalary = BanglaConversion.ConvertToBanglaNumber(t.BasicSalary);
                t.HouseRent = BanglaConversion.ConvertToBanglaNumber(t.HouseRent);
                t.MedicalAllowance = BanglaConversion.ConvertToBanglaNumber(t.MedicalAllowance);
                t.Conveyance = BanglaConversion.ConvertToBanglaNumber(t.Conveyance);
                t.FoodAllowance = BanglaConversion.ConvertToBanglaNumber(t.FoodAllowance);
                t.EntertainmentAllowance = BanglaConversion.ConvertToBanglaNumber(t.EntertainmentAllowance);
                t.GrossSalary = BanglaConversion.ConvertToBanglaNumber(t.GrossSalary);

                t.WorkingDays = BanglaConversion.ConvertToBanglaNumber(t.WorkingDays);
                //t.WeekendDays = BanglaConversion.ConvertToBanglaNumber(PayrollReportManager.GetWeekendDays(DateTime.Now, dt2).ToString());
                t.WeekendDays = BanglaConversion.ConvertToBanglaNumber(t.WeekendDays);
                t.HolidayDays = BanglaConversion.ConvertToBanglaNumber(t.HolidayDays);
                t.TotalDays = BanglaConversion.ConvertToBanglaNumber(t.TotalDays);
                t.PresentDays = BanglaConversion.ConvertToBanglaNumber(t.PresentDays);
                t.AbsentDays = BanglaConversion.ConvertToBanglaNumber(t.AbsentDays);
                t.LateDays = BanglaConversion.ConvertToBanglaNumber(t.LateDays);
                t.LeaveDays = BanglaConversion.ConvertToBanglaNumber(t.LeaveDays);
                t.AttendanceBonus = BanglaConversion.ConvertToBanglaNumber(t.AttendanceBonus);
                t.ShiftingBonus = BanglaConversion.ConvertToBanglaNumber(t.ShiftingBonus);
                t.TotalBonus = BanglaConversion.ConvertToBanglaNumber(t.TotalBonus);
                t.OTHours = BanglaConversion.ConvertToBanglaNumber(t.OTHours);
                t.OTRate = BanglaConversion.ConvertToBanglaNumber(t.OTRate);
                t.TotalOTAmount = BanglaConversion.ConvertToBanglaNumber(t.TotalOTAmount);
                t.TotalPaid = BanglaConversion.ConvertToBanglaNumber(t.TotalPaid);
                t.Stamp = BanglaConversion.ConvertToBanglaNumber(t.Stamp);
                t.AbsentFee = BanglaConversion.ConvertToBanglaNumber(t.AbsentFee);
                t.Advance = BanglaConversion.ConvertToBanglaNumber(t.Advance);
                t.TotalDeduction = BanglaConversion.ConvertToBanglaNumber(t.TotalDeduction);
                t.NetAmount = BanglaConversion.ConvertToBanglaNumber(t.NetAmount);
                t.FromDate = BanglaConversion.ConvertToBanglaNumber(t.FromDate);
                t.ToDate = BanglaConversion.ConvertToBanglaNumber(t.ToDate);
                t.EmployeeCardId = BanglaConversion.ConvertToBanglaNumber(t.EmployeeCardId);
                t.Rate = BanglaConversion.ConvertToBanglaNumber(t.Rate);
                t.EmployeeTypeId = t.EmployeeTypeId;

                serialNo += 1;
                t.SerialNo = t.SerialNo = serialNo;
                t.SerialId = serialNo.ToString();

                paySlipInBenglai.Add(t);
            }

            //paySlipInBenglai = paySlipInBenglai.OrderBy(x => x.EmployeeCardId).ToList();

            var paySlipInBengaliLeft = new List<PaySlipView>();
            paySlipInBengaliLeft = paySlipInBenglai.OrderBy(x => x.SerialNo).ToList();

            var paySlipInBengaliRight = new List<PaySlipView>();
            paySlipInBengaliRight = paySlipInBenglai.OrderBy(x => x.SerialNo).ToList();

            ReportDataSource rd1 = new ReportDataSource("DataSet1", paySlipInBengaliLeft);
            ReportDataSource rd2 = new ReportDataSource("DataSet2", paySlipInBengaliRight);

            lr.DataSources.Add(rd1);
            lr.DataSources.Add(rd2);

            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
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

        [AjaxAuthorize(Roles = "salarysheet-2,salarysheet-3")]
        public ActionResult SalarysheetIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(PortalContext.CurrentUser.CompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                var employeeCategoryValueList = from EmployeeCategoryValue status in Enum.GetValues(typeof(EmployeeCategoryValue))
                                                select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeCategories = employeeCategoryValueList;
                model.PrintedDate = DateTime.Now;
                var firstDayOfMonth =
                model.FromDate = DateTime.Now.ToLastMonthDate(26);
                model.ToDate = DateTime.Now.ToThisMonthDate(25);
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "salarysheet-2,salarysheet-3")]
        public ActionResult SalarySheet(ReportSearchViewModel model)
        {
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            string employeeCardId = model.EmployeeCardId;
            int year = Convert.ToInt32(model.SelectedYear);
            int month = Convert.ToInt32(model.SelectedMonth);
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;
            int employeeCategoryId = model.EmployeeCategory;
            LocalReport lr = new LocalReport();

            string path = string.Empty;
            var reportParams = CreateReportParams(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId,model.PrintedDate);
            string reportName = "SalarySheet2";
            if (employeeTypeId == (int)EmployeeTypeId.TeamMemberA || employeeTypeId == (int)EmployeeTypeId.TeamMemberB)
            {
                if (employeeCategoryId == 1 || employeeCategoryId == 3)
                {
                     reportName = "SalarySheet2";
                }
                else
                {
                     reportName = "SalarySheetForQuitEmployee";
                }
            }
            else
            {
                if (employeeCategoryId == 1 || employeeCategoryId == 3)
                {
                     reportName = "SalarySheetManager";

                }
                else
                {
                     reportName = "SalarySheetManagerForQuitEmployee";
                }
            }

            //var reportType = model.ReportType.ToString();
            //if (reportType == null)
            //{
            //    reportType = ReportType.PDF.ToString();
            //}
            return ReportExtension.ToSsrsFile(model.ReportType, reportName, reportParams);

        }

       
        private List<ReportParameter> CreateReportParams(int? companyId, int? branchId, int? branchUnitId,
         int? branchUnitDepartmentId,
         int? departmentSectionId, int? departmentLineId, int? employeeTypeId, string employeeCardId,
         int year, int month, DateTime? fromDate, DateTime? toDate, int? employeeCategoryId,DateTime? printedDate)
        {

            try
            {

                if (companyId == 0)
                    companyId = -1;

                var companyIdParam = new ReportParameter("CompanyId", companyId.ToString());
                if (branchId == 0)
                    branchId = -1;

                var branchIdParam = new ReportParameter("BranchId", branchId.ToString());
                if (branchUnitId == 0)
                    branchUnitId = -1;

                var branchUnitIdParam = new ReportParameter("BranchUnitId", branchUnitId.ToString());
                if (branchUnitDepartmentId == 0)
                    branchUnitDepartmentId = -1;

                var branchUnitDepartmentIdParam = new ReportParameter("BranchUnitDepartmentId", branchUnitDepartmentId.ToString());
                if (departmentSectionId == 0)
                    departmentSectionId = -1;
                var departmentSectionIdParam = new ReportParameter("DepartmentSectionId", departmentSectionId.ToString());

                if (departmentLineId == 0)
                    departmentLineId = -1;
                var departmentLineIdParam = new ReportParameter("DepartmentLineId", departmentLineId.ToString());

                if (employeeTypeId == 0)
                    employeeTypeId = -1;
                var employeeTypeIdParam = new ReportParameter("EmployeeTypeId", employeeTypeId.ToString());

                if (String.IsNullOrEmpty(employeeCardId))
                    employeeCardId = string.Empty;
                var employeeCardIdParam = new ReportParameter("EmployeeCardId", employeeCardId.ToString());

                var yearParam = new ReportParameter("Year", year.ToString());

                var monthParam = new ReportParameter("Month", month.ToString());

                var fromDateParam = new ReportParameter("FromDate", fromDate.ToString());

                var toDateParam = new ReportParameter("ToDate", toDate.ToString());

                if (employeeCategoryId == 0)
                    employeeCategoryId = -1;
                var employeeCategoryIdParam = new ReportParameter("EmployeeCategoryId", employeeCategoryId.ToString());

                var userNameParam = new ReportParameter("UserName", PortalContext.CurrentUser.Name.ToString());
                printedDate = printedDate ?? DateTime.Now;
                var PrintedDateParam = new ReportParameter("PrintedDate", printedDate.ToString());

                var reportParams = new List<ReportParameter>
                { companyIdParam , branchIdParam ,branchUnitIdParam
                ,branchUnitDepartmentIdParam,departmentSectionIdParam
                ,departmentLineIdParam,employeeTypeIdParam,employeeCardIdParam
                ,yearParam,monthParam,fromDateParam,toDateParam
                ,employeeCategoryIdParam,userNameParam,PrintedDateParam};

                return reportParams;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public ActionResult SalarySheetGrossDeductionIndex(ReportSearchViewModel model)
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

                var employeeCategoryValueList = from EmployeeCategoryValue status in Enum.GetValues(typeof(EmployeeCategoryValue))
                                                select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeCategories = employeeCategoryValueList;
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult SalarySheetGrossDeduction(ReportSearchViewModel model)
        {
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            string employeeCardId = model.EmployeeCardId;
            int year = Convert.ToInt32(model.SelectedYear);
            int month = Convert.ToInt32(model.SelectedMonth);
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;
            int employeeCategoryId = model.EmployeeCategory;
            LocalReport lr = new LocalReport();

            string path = string.Empty;

            if (employeeTypeId == (int)EmployeeTypeId.TeamMemberA || employeeTypeId == (int)EmployeeTypeId.TeamMemberB)
            {

                if (employeeCategoryId == 1 || employeeCategoryId == 3)
                    path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "SalarySheet.rdlc");
                else
                {
                    path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "SalarySheetForQuitEmployee.rdlc");
                }
            }
            else
            {
                if (employeeCategoryId == 1 || employeeCategoryId == 3)
                    path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "SalarySheetManager.rdlc");
                else
                {
                    path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "SalarySheetManagerForQuitEmployee.rdlc");
                }
            }

            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            var salarySheet = new List<SalarySheetView>();

            salarySheet = PayrollReportManager.GetSalarySheetGrossDeductionInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId);

            ReportDataSource rd = new ReportDataSource("DataSet1", salarySheet);
            lr.DataSources.Add(rd);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0.0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
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

        public ActionResult SalarysheetBankIndex(ReportSearchViewModel model)
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

                var employeeCategoryValueList = from EmployeeCategoryValue status in Enum.GetValues(typeof(EmployeeCategoryValue))
                                                select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeCategories = employeeCategoryValueList;

                model.SearchFieldModel.SearchByEmployeeTypeId = 1;
                model.EmployeeCategory = 1;
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult SalarySheetBank(ReportSearchViewModel model)
        {
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = -1;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = -1;
            string employeeCardId = model.EmployeeCardId;
            int year = Convert.ToInt32(model.SelectedYear);
            int month = Convert.ToInt32(model.SelectedMonth);
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;
            int employeeCategoryId = -1;
            LocalReport lr = new LocalReport();

            string path = string.Empty;

            path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "SalarySheetBank.rdlc");

            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            var salarySheet = new List<SalarySheetView>();

            salarySheet = PayrollReportManager.GetSalarySheetBankInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId);

            ReportDataSource rd = new ReportDataSource("DataSet1", salarySheet);
            lr.DataSources.Add(rd);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0.0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
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

        [AjaxAuthorize(Roles = "salarysheet(all)-2,salarysheet(all)-3")]
        public ActionResult SalarySheetAllIndex(ReportSearchViewModel model)
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

                var employeeCategoryValueList = from EmployeeCategoryValue status in Enum.GetValues(typeof(EmployeeCategoryValue))
                                                select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeCategories = employeeCategoryValueList;
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "salarysheet(all)-2,salarysheet(all)-3")]
        public ActionResult SalarySheetAll(ReportSearchViewModel model)
        {
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            string employeeCardId = model.EmployeeCardId;
            int year = Convert.ToInt32(model.SelectedYear);
            int month = Convert.ToInt32(model.SelectedMonth);
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;
            int employeeCategoryId = model.EmployeeCategory;
            LocalReport lr = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "SalarySheetForAllEmployees.rdlc");

            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("SalarySheetAllIndex");

            var salarySheet = new List<SalarySheetView>();

            salarySheet = PayrollReportManager.GetSalarySheetInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId);

            string compId = PortalContext.CurrentUser.CompId;
            string companyName = PayrollReportManager.GetCompanyByCompanyId(compId).Name;
            string companyAddress = PayrollReportManager.GetCompanyByCompanyId(compId).FullAddress;
            EmployeeCategoryValue category = (EmployeeCategoryValue)employeeCategoryId;

            ReportParameter[] parameters = new ReportParameter[3];
            parameters[0] = new ReportParameter("param_CompanySector", companyName);
            parameters[1] = new ReportParameter("param_CompanyAddress", companyAddress);
            parameters[2] = new ReportParameter("param_EmployeeCategory", category.ToString());

            ReportDataSource rd = new ReportDataSource("DataSetSalarySheetAllInfo", salarySheet);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0.0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
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

        [AjaxAuthorize(Roles = "monthlypayment(all)-2,monthlypayment(all)-3")]
        public ActionResult EmployeeAllPaymentSheetIndex(ReportSearchViewModel model)
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
                model.EmployeeTypes = EmployeeTypeManager.GetAllEmployeeTypes();

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

        [AjaxAuthorize(Roles = "monthlypayment(all)-2,monthlypayment(all)-3")]
        public ActionResult EmployeeAllPaymentSheet(ReportSearchViewModel model)
        {
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            string employeeCardId = model.EmployeeCardId;
            int year = Convert.ToInt32(model.SelectedYear);
            int month = Convert.ToInt32(model.SelectedMonth);
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;
            int employeeCategoryId = model.EmployeeCategory;

            LocalReport lr = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "EmployeeAllPaymentSheet.rdlc");

            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            var employeeAllPaymentSheet = new List<EmployeeAllPaymentSheetView>();

            employeeAllPaymentSheet = PayrollReportManager.GetEmployeeAllPaymentSheetInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId);

            EmployeeCategoryValue category = (EmployeeCategoryValue)employeeCategoryId;

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_EmployeeCategory", category.ToString());

            ReportDataSource rd = new ReportDataSource("DataSetEmployeeAllPaymentInfo", employeeAllPaymentSheet);
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
                "  <MarginTop>0.0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
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

        public ActionResult EmployeeAllPaymentSheetGrossDeductionIndex(ReportSearchViewModel model)
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
                model.EmployeeTypes = EmployeeTypeManager.GetAllEmployeeTypes();

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

        public ActionResult EmployeeAllPaymentSheetGrossDeduction(ReportSearchViewModel model)
        {
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            string employeeCardId = model.EmployeeCardId;
            int year = Convert.ToInt32(model.SelectedYear);
            int month = Convert.ToInt32(model.SelectedMonth);
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;
            int employeeCategoryId = model.EmployeeCategory;

            LocalReport lr = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "EmployeeAllPaymentSheetGrossDeduction.rdlc");

            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            var employeeAllPaymentSheet = new List<EmployeeAllPaymentSheetView>();

            employeeAllPaymentSheet = PayrollReportManager.GetEmployeeAllPaymentSheetInfoGrossDeduction(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId);

            EmployeeCategoryValue category = (EmployeeCategoryValue)employeeCategoryId;

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_EmployeeCategory", category.ToString());

            ReportDataSource rd = new ReportDataSource("DataSetEmployeeAllPaymentInfo", employeeAllPaymentSheet);
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
                "  <MarginTop>0.0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
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

        [AjaxAuthorize(Roles = "salary(all)-2,salary(all)-3")]
        public ActionResult EmployeeSalaryInfoIndex(ReportSearchViewModel model)
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

        [AjaxAuthorize(Roles = "salary(all)-2,salary(all)-3")]
        public ActionResult EmployeeSalaryInfo(ReportSearchViewModel model)
        {
            var lr = new LocalReport();
            var path = Path.Combine(Server.MapPath("~/Areas/PAYROLL/Reports"), "EmployeeSalaryInfo.rdlc");
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
            int? genderId = model.SearchFieldModel.SearchByEmployeeGenderId;

            DateTime? joiningDateBegin = model.SearchFieldModel.JoiningDateBegin;
            DateTime? joiningDateEnd = model.SearchFieldModel.JoiningDateEnd;
            DateTime? quitDateBegin = model.SearchFieldModel.QuitDateBegin;
            DateTime? quitDateEnd = model.SearchFieldModel.QuitDateEnd;

            string employeeCardId = model.SearchFieldModel.SearchByEmployeeCardId;
            string employeeName = model.SearchFieldModel.SearchByEmployeeName;

            int? activeStatus = model.SearchFieldModel.SearchByEmployeeStatus;
            string userName = PortalContext.CurrentUser.Name;
            DateTime? upToDate = model.SearchFieldModel.StartDate;

            List<EmployeeSalaryInfoModel> employeeSalaryInfo = PayrollReportManager.GetEmployeeSalaryReport(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId,
                departmentLineId, employeeTypeId, employeeGradeId, employeeDesignationId,
                genderId, joiningDateBegin, joiningDateEnd, quitDateBegin, quitDateEnd,
                employeeCardId, employeeName, activeStatus, userName, upToDate);

            string compId = PortalContext.CurrentUser.CompId;
            string companyName = PayrollReportManager.GetCompanyByCompanyId(compId).Name;
            string companyAddress = PayrollReportManager.GetCompanyByCompanyId(compId).FullAddress;

            var rd = new ReportDataSource("DataSetEmployeeSalaryInfo", employeeSalaryInfo);

            var parameters = new ReportParameter[25];

            List<string> reportHeads = model.ReportHeads;

            if (reportHeads != null)
            {
                if (reportHeads.Contains("EmployeeCardId"))
                    parameters[0] = new ReportParameter("Param_EmployeeCardId", "True");
                else
                    parameters[0] = new ReportParameter("Param_EmployeeCardId", "False");


                if (reportHeads.Contains("EmployeeName"))
                    parameters[1] = new ReportParameter("Param_EmployeeName", "True");
                else
                    parameters[1] = new ReportParameter("Param_EmployeeName", "False");

                if (reportHeads.Contains("Company"))
                    parameters[2] = new ReportParameter("Param_Company", "True");
                else
                    parameters[2] = new ReportParameter("Param_Company", "False");

                if (reportHeads.Contains("Branch"))
                    parameters[3] = new ReportParameter("Param_Branch", "True");
                else
                    parameters[3] = new ReportParameter("Param_Branch", "False");

                if (reportHeads.Contains("Unit"))
                    parameters[4] = new ReportParameter("Param_Unit", "True");
                else
                    parameters[4] = new ReportParameter("Param_Unit", "False");

                if (reportHeads.Contains("Department"))
                    parameters[5] = new ReportParameter("Param_Department", "True");
                else
                    parameters[5] = new ReportParameter("Param_Department", "False");

                if (reportHeads.Contains("Section"))
                    parameters[6] = new ReportParameter("Param_Section", "True");
                else
                    parameters[6] = new ReportParameter("Param_Section", "False");

                if (reportHeads.Contains("Line"))
                    parameters[7] = new ReportParameter("Param_Line", "True");
                else
                    parameters[7] = new ReportParameter("Param_Line", "False");

                if (reportHeads.Contains("EmployeeType"))
                    parameters[8] = new ReportParameter("Param_EmployeeType", "True");
                else
                    parameters[8] = new ReportParameter("Param_EmployeeType", "False");

                if (reportHeads.Contains("EmployeeGrade"))
                    parameters[9] = new ReportParameter("Param_EmployeeGrade", "True");
                else
                    parameters[9] = new ReportParameter("Param_EmployeeGrade", "False");

                if (reportHeads.Contains("EmployeeDesignation"))
                    parameters[10] = new ReportParameter("Param_EmployeeDesignation", "True");
                else
                    parameters[10] = new ReportParameter("Param_EmployeeDesignation", "False");

                if (reportHeads.Contains("JoiningDate"))
                    parameters[11] = new ReportParameter("Param_JoiningDate", "True");
                else
                    parameters[11] = new ReportParameter("Param_JoiningDate", "False");

                if (reportHeads.Contains("QuitDate"))
                    parameters[12] = new ReportParameter("Param_QuitDate", "True");
                else
                    parameters[12] = new ReportParameter("Param_QuitDate", "False");


                if (reportHeads.Contains("GrossSalary"))
                    parameters[13] = new ReportParameter("Param_GrossSalary", "True");
                else
                    parameters[13] = new ReportParameter("Param_GrossSalary", "False");

                if (reportHeads.Contains("BasicSalary"))
                    parameters[14] = new ReportParameter("Param_BasicSalary", "True");
                else
                    parameters[14] = new ReportParameter("Param_BasicSalary", "False");

                if (reportHeads.Contains("HouseRent"))
                    parameters[15] = new ReportParameter("Param_HouseRent", "True");
                else
                    parameters[15] = new ReportParameter("Param_HouseRent", "False");

                if (reportHeads.Contains("MedicalAllowance"))
                    parameters[16] = new ReportParameter("Param_MedicalAllowance", "True");
                else
                    parameters[16] = new ReportParameter("Param_MedicalAllowance", "False");

                if (reportHeads.Contains("FoodAllowance"))
                    parameters[17] = new ReportParameter("Param_FoodAllowance", "True");
                else
                    parameters[17] = new ReportParameter("Param_FoodAllowance", "False");

                if (reportHeads.Contains("Conveyance"))
                    parameters[18] = new ReportParameter("Param_Conveyance", "True");
                else
                    parameters[18] = new ReportParameter("Param_Conveyance", "False");

                if (reportHeads.Contains("EntertainmentAllowance"))
                    parameters[19] = new ReportParameter("Param_EntertainmentAllowance", "True");
                else
                    parameters[19] = new ReportParameter("Param_EntertainmentAllowance", "False");

                if (reportHeads.Contains("EffectiveFromDate"))
                    parameters[20] = new ReportParameter("Param_EffectiveFromDate", "True");
                else
                    parameters[20] = new ReportParameter("Param_EffectiveFromDate", "False");

                if (reportHeads.Contains("GenderName"))
                    parameters[21] = new ReportParameter("Param_GenderName", "True");
                else
                    parameters[21] = new ReportParameter("Param_GenderName", "False");

                if (reportHeads.Contains("ActiveStatus"))
                    parameters[22] = new ReportParameter("Param_ActiveStatus", "True");
                else
                    parameters[22] = new ReportParameter("Param_ActiveStatus", "False");

                parameters[23] = new ReportParameter("param_CompanySector", companyName);
                parameters[24] = new ReportParameter("param_CompanyAddress", companyAddress);

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

        public ActionResult EmployeeCurrentSalaryIndex(ReportSearchViewModel model)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append("http://");
            urlBuilder.Append(AppConfig.ReportServerAddress); // "Server name or IP addrress " 
            urlBuilder.Append("/reportserver/Pages/ReportViewer.aspx?");
            urlBuilder.Append("/SCERPREPORT.Payroll/"); // "Report Module Name"
            urlBuilder.Append("EmployeeCurrentSalary");
            urlBuilder.Append("&rs:Command=Render");

            urlBuilder.Append("&UserName=" + PortalContext.CurrentUser.Name);
            urlBuilder.Append("&CompanyId=" + "-1");
            urlBuilder.Append("&BranchId=" + "-1");
            urlBuilder.Append("&UnitId=" + "-1");

            urlBuilder.Append("&DepartmentId=" + "-1");
            urlBuilder.Append("&SectionId=" + "-1");
            urlBuilder.Append("&LineId=" + "-1");

            urlBuilder.Append("&EmployeeTypeId=" + "-1");
            urlBuilder.Append("&EmployeeGradeId=" + "-1");
            urlBuilder.Append("&DesignationId=" + "-1");

            ViewBag.ReportUrl = urlBuilder;

            return PartialView("_SSRSReportContorl");
        }

        public ActionResult EmployeeSalaryHistoryIndex(ReportSearchViewModel model)
        {
            ModelState.Clear();

            var urlBuilder = new StringBuilder();
            urlBuilder.Append("http://");
            urlBuilder.Append(AppConfig.ReportServerAddress); // "Server name or IP addrress " 
            urlBuilder.Append("/reportserver/Pages/ReportViewer.aspx?");
            urlBuilder.Append("/SCERPREPORT.Payroll/"); // "Report Module Name"
            urlBuilder.Append("EmployeeIndividualSalaryHistory");
            urlBuilder.Append("&rs:Command=Render");

            ViewBag.ReportUrl = urlBuilder;

            return PartialView("_SSRSReportContorl");
        }

        [AjaxAuthorize(Roles = "salarysheet(advance)-2,salarysheet(advance)-3")]
        public ActionResult AdvanceSalarySheetIndex(ReportSearchViewModel model)
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
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "salarysheet(advance)-2,salarysheet(advance)-3")]
        public ActionResult AdvanceSalarySheet(string cardId, int companyId, int branchId, int branchUnitId, int? branchUnitDepartmentId, int? sectionId, int? lineId, DateTime fromDate, DateTime toDate, int employeeTypeId)
        {
            LocalReport lr = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "AdvanceSalarySheet.rdlc");

            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            var salarySheet = new List<AdvanceSalarySheetView>();

            salarySheet = PayrollReportManager.GetAdvanceSalarySheetInfo(cardId, companyId, branchId, branchUnitId, branchUnitDepartmentId, sectionId, lineId, fromDate, toDate, employeeTypeId);

            string compId = PortalContext.CurrentUser.CompId;
            string companyAddress = PayrollReportManager.GetCompanyByCompanyId(compId).FullAddress;

            ReportDataSource rd = new ReportDataSource("DataSet1", salarySheet);
            lr.DataSources.Add(rd);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0.2in</MarginTop>" +
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

        [AjaxAuthorize(Roles = "bonussheet-2,bonussheet-3")]
        public ActionResult EmployeeBonusSheetIndex(ReportSearchViewModel model)
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
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "bonussheet-2,bonussheet-3")]
        public ActionResult EmployeeBonusSheet(string cardId, int companyId, int branchId, int branchUnitId, int? branchUnitDepartmentId, int? sectionId, int? lineId, int employeeTypeId, DateTime effectiveDate)
        {
            LocalReport lr = new LocalReport();

            string path = path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "EmployeeBonusSheet.rdlc");

            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            var bonusSheet = new List<EmployeeBonusSheetView>();

            bonusSheet = PayrollReportManager.GetEmployeeBonusSheetInfo(cardId, companyId, branchId, branchUnitId, branchUnitDepartmentId, sectionId, lineId, employeeTypeId, effectiveDate);

            ReportDataSource rd = new ReportDataSource("DataSetEmployeeBonus", bonusSheet);
            lr.DataSources.Add(rd);
            string reportType = ReportType.PDF.ToString();
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

        [AjaxAuthorize(Roles = "extraotsheet-2,extraotsheet-3")]
        public ActionResult ExtraOTSheetIndex(ReportSearchViewModel model)
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

                var employeeCategoryValueList = from EmployeeCategoryValue status in Enum.GetValues(typeof(EmployeeCategoryValue))
                                                select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeCategories = employeeCategoryValueList;
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "extraotsheet-2,extraotsheet-3")]
        public ActionResult ExtraOTSheet(ReportSearchViewModel model)
        {
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            string employeeCardId = model.EmployeeCardId;
            int year = Convert.ToInt32(model.SelectedYear);
            int month = Convert.ToInt32(model.SelectedMonth);
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;
            int employeeCategoryId = model.EmployeeCategory;

            LocalReport lr = new LocalReport();

            string path = path = Path.Combine(Server.MapPath("~/Areas/PayRoll/Reports"), "ExtraOTSheet.rdlc");

            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            var extraOtSheetInfo = new List<ExtraOTSheetView>();

            extraOtSheetInfo = PayrollReportManager.GetExtraOTSheetInfo(companyId, branchId, branchUnitId,
                branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year,
                month, fromDate, toDate, employeeCategoryId);

            EmployeeCategoryValue category = (EmployeeCategoryValue)employeeCategoryId;

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_EmployeeCategory", category.ToString());

            ReportDataSource rd = new ReportDataSource("DataSetExtraOTSheetInfo", extraOtSheetInfo);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0.2in</MarginTop>" +
                "  <MarginLeft>0.2in</MarginLeft>" +
                "  <MarginRight>0.2in</MarginRight>" +
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

        public ActionResult ExtraOTSheetModelIndex(ReportSearchViewModel model)
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

                var employeeCategoryValueList = from EmployeeCategoryValue status in Enum.GetValues(typeof(EmployeeCategoryValue))
                                                select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeCategories = employeeCategoryValueList;
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult ExtraOTSheetModel(ReportSearchViewModel model)
        {
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            string employeeCardId = model.EmployeeCardId;
            int year = Convert.ToInt32(model.SelectedYear);
            int month = Convert.ToInt32(model.SelectedMonth);
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;
            int employeeCategoryId = model.EmployeeCategory;

            LocalReport lr = new LocalReport();

            string path = path = Path.Combine(Server.MapPath("~/Areas/PayRoll/Reports"), "ExtraOTSheetModel.rdlc");

            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            var extraOtSheetInfo = new List<ExtraOTSheetView>();

            extraOtSheetInfo = PayrollReportManager.GetExtraOTSheetModelInfo(companyId, branchId, branchUnitId,
                branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year,
                month, fromDate, toDate, employeeCategoryId);

            EmployeeCategoryValue category = (EmployeeCategoryValue)employeeCategoryId;

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_EmployeeCategory", category.ToString());

            ReportDataSource rd = new ReportDataSource("DataSetExtraOTSheetInfo", extraOtSheetInfo);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0.2in</MarginTop>" +
                "  <MarginLeft>0.2in</MarginLeft>" +
                "  <MarginRight>0.2in</MarginRight>" +
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

        public ActionResult ExtraOTSheet10PMNoWeekendIndex(ReportSearchViewModel model)
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

                var employeeCategoryValueList = from EmployeeCategoryValue status in Enum.GetValues(typeof(EmployeeCategoryValue))
                                                select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeCategories = employeeCategoryValueList;
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [ActionName("ExtraOTSheetJobcarddne")]
        public ActionResult ExtraOTSheet10PMNoWeekend(ReportSearchViewModel model)
        {
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            string employeeCardId = model.EmployeeCardId;
            int year = Convert.ToInt32(model.SelectedYear);
            int month = Convert.ToInt32(model.SelectedMonth);
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;
            int employeeCategoryId = model.EmployeeCategory;

            LocalReport lr = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/Areas/PayRoll/Reports"), "ExtraOTSheetJobcarddne.rdlc");

            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            var extraOtSheetInfo = new List<ExtraOTSheetView>();

            extraOtSheetInfo = PayrollReportManager.GetExtraOTSheet10PMNoWeekendInfo(companyId, branchId, branchUnitId,
                branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year,
                month, fromDate, toDate, employeeCategoryId);

            EmployeeCategoryValue category = (EmployeeCategoryValue)employeeCategoryId;

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_EmployeeCategory", category.ToString());

            ReportDataSource rd = new ReportDataSource("DataSetExtraOTSheetInfo", extraOtSheetInfo);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);

            var reportType = model.ReportType.ToString();
            if (reportType == null)
            {
                reportType = ReportType.PDF.ToString();
            }
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0.2in</MarginTop>" +
                "  <MarginLeft>0.2in</MarginLeft>" +
                "  <MarginRight>0.2in</MarginRight>" +
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

        public ActionResult ExtraOTSheetAfter10withHolidayIndex(ReportSearchViewModel model)
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

                var employeeCategoryValueList = from EmployeeCategoryValue status in Enum.GetValues(typeof(EmployeeCategoryValue))
                                                select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeCategories = employeeCategoryValueList;
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [ActionName("ExtraOTSheetydi")]
        public ActionResult ExtraOTSheetAfter10withHoliday(ReportSearchViewModel model)
        {
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            string employeeCardId = model.EmployeeCardId;
            int year = Convert.ToInt32(model.SelectedYear);
            int month = Convert.ToInt32(model.SelectedMonth);
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;
            int employeeCategoryId = model.EmployeeCategory;

            LocalReport lr = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/Areas/PayRoll/Reports"), "ExtraOTSheetydi.rdlc");

            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            var extraOtSheetInfo = new List<ExtraOTSheetView>();

            extraOtSheetInfo = PayrollReportManager.GetExtraOTSheetAfter10PMWithHolidayInfo(companyId, branchId, branchUnitId,
                branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year,
                month, fromDate, toDate, employeeCategoryId);

            EmployeeCategoryValue category = (EmployeeCategoryValue)employeeCategoryId;

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_EmployeeCategory", category.ToString());

            ReportDataSource rd = new ReportDataSource("DataSetExtraOTSheetInfo", extraOtSheetInfo);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);

            var reportType = model.ReportType.ToString();
            if (reportType == null)
            {
                reportType = ReportType.PDF.ToString();
            }
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0.2in</MarginTop>" +
                "  <MarginLeft>0.2in</MarginLeft>" +
                "  <MarginRight>0.2in</MarginRight>" +
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

        [AjaxAuthorize(Roles = "weekendotsheet-2,weekendotsheet-3")]
        public ActionResult WeekendOTSheetIndex(ReportSearchViewModel model)
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


                var employeeCategoryValueList = from EmployeeCategoryValue status in Enum.GetValues(typeof(EmployeeCategoryValue))
                                                select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeCategories = employeeCategoryValueList;

            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        //[AjaxAuthorize(Roles = "extraotsheet-2,extraotsheet-3")]
        [AjaxAuthorize(Roles = "weekendotsheet-2,weekendotsheet-3")]
        public ActionResult WeekendOTSheet(ReportSearchViewModel model)
        {

            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            string employeeCardId = model.EmployeeCardId;
            int year = Convert.ToInt32(model.SelectedYear);
            int month = Convert.ToInt32(model.SelectedMonth);
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;
            int employeeCategoryId = model.EmployeeCategory;



            LocalReport lr = new LocalReport();

            string path = path = Path.Combine(Server.MapPath("~/Areas/PayRoll/Reports"), "WeekendOTSheet.rdlc");

            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            var extraOtSheetInfo = new List<WeekendOTSheetView>();

            extraOtSheetInfo = PayrollReportManager.GetWeekendOTSheetInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId,
                employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId);

            EmployeeCategoryValue category = (EmployeeCategoryValue)employeeCategoryId;

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_EmployeeCategory", category.ToString());

            ReportDataSource rd = new ReportDataSource("DataSetWeekendOTSheetInfo", extraOtSheetInfo);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);

            var reportType = model.ReportType.ToString();
            if (reportType == null)
            {
                reportType = ReportType.PDF.ToString();
            }
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

        [AjaxAuthorize(Roles = "holidayotsheet-2,holidayotsheet-3")]
        public ActionResult HolidayOTSheetIndex(ReportSearchViewModel model)
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


                var employeeCategoryValueList = from EmployeeCategoryValue status in Enum.GetValues(typeof(EmployeeCategoryValue))
                                                select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeCategories = employeeCategoryValueList;

            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "holidayotsheet-2,holidayotsheet-3")]
        public ActionResult HolidayOTSheet(ReportSearchViewModel model)
        {

            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            string employeeCardId = model.EmployeeCardId;
            int year = Convert.ToInt32(model.SelectedYear);
            int month = Convert.ToInt32(model.SelectedMonth);
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;
            int employeeCategoryId = model.EmployeeCategory;



            LocalReport lr = new LocalReport();

            string path = path = Path.Combine(Server.MapPath("~/Areas/PayRoll/Reports"), "HolidayOTSheet.rdlc");

            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            var holidayOTSheetInfo = new List<HolidayOTSheetView>();

            holidayOTSheetInfo = PayrollReportManager.GetHolidayOTSheetInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId,
                employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId);

            EmployeeCategoryValue category = (EmployeeCategoryValue)employeeCategoryId;

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_EmployeeCategory", category.ToString());

            ReportDataSource rd = new ReportDataSource("DataSet1", holidayOTSheetInfo);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);
            string reportType = "pdf";
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

        [AjaxAuthorize(Roles = "otsheet(others)-2,otsheet(others)-3")]
        public ActionResult ExtraOTWeekendOTAndHolidayOTSheetIndex(ReportSearchViewModel model)
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

        [AjaxAuthorize(Roles = "otsheet(others)-2,otsheet(others)-3")]
        public ActionResult ExtraOTWeekendOTAndHolidayOTSheet(ReportSearchViewModel model)
        {

            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            string employeeCardId = model.EmployeeCardId;
            int year = Convert.ToInt32(model.SelectedYear);
            int month = Convert.ToInt32(model.SelectedMonth);
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;
            int employeeCategoryId = model.EmployeeCategory;

            var lr = new LocalReport();

            string path = string.Empty;
            if (employeeCategoryId == 1 || employeeCategoryId == 3)
                path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "ExtraOTWeekendOTAndHolidayOTSheet.rdlc");
            else
            {
                path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "ExtraOTWeekendOTAndHolidayOTSheetForQuitEmployee.rdlc");
            }

            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            var extraOTWeekendOTAndHolidayOTSheetInfo = PayrollReportManager.GetExtraOTWeekendOTAndHolidayOTSheetInfo(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, employeeCardId, year, month, fromDate, toDate, employeeCategoryId);


            EmployeeCategoryValue category = (EmployeeCategoryValue)employeeCategoryId;

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_EmployeeCategory", category.ToString());

            var rd = new ReportDataSource("DataSet1", extraOTWeekendOTAndHolidayOTSheetInfo);
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

        [AjaxAuthorize(Roles = "salarysummary-2,salarysummary-3")]
        public ActionResult SalarySummarySheetIndex(ReportSearchViewModel model)
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

        [AjaxAuthorize(Roles = "salarysummary-2,salarysummary-3")]
        public ActionResult SalarySummarySheet(ReportSearchViewModel model)
        {
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int year = Convert.ToInt32(model.SelectedYear);
            int month = Convert.ToInt32(model.SelectedMonth);
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;

            var branchUnitIdList = BranchUnitManager.GetAllPermittedBranchUnitIdByBranchId((int)branchId);
            var employeeTypeIdList = EmployeeTypeManager.GetAllPermittedEmployeeTypeId();

            List<int> branchUnitIds;
            List<int> employeeTypeIds;
            int employeeCategoryId = 0;

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_MonthName", toDate.Value.ToString("MMMM") + "' " + toDate.Value.Year.ToString());

            LocalReport lr = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "SalarySummaryInfo.rdlc");


            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            //Regular Team Member
            branchUnitIds = new List<int>();
            branchUnitIds = (from branchUnitId in branchUnitIdList where branchUnitId.BranchUnitId == 1 || branchUnitId.BranchUnitId == 2 select branchUnitId.BranchUnitId).ToList();

            employeeTypeIds = new List<int>();
            employeeTypeIds = (from userEmployeeType in employeeTypeIdList where userEmployeeType.Id == 4 || userEmployeeType.Id == 5 select userEmployeeType.Id).ToList();

            employeeCategoryId = (int)SCERP.Common.EmployeeCategory.REGULAR;

            List<SalarySummaryView> salarySummaryViewForRegularTeamMember = PayrollReportManager.GetEmployeeSalarySummary(companyId, branchId, branchUnitIds, employeeTypeIds, employeeCategoryId, year, month, fromDate, toDate);
            var reportDataSourceForRegularTeamMember = new ReportDataSource("DataSet1", salarySummaryViewForRegularTeamMember);

            //Regular Management
            employeeTypeIds = new List<int>();
            employeeTypeIds = (from userEmployeeType in employeeTypeIdList where userEmployeeType.Id == 2 || userEmployeeType.Id == 3 select userEmployeeType.Id).ToList();

            List<SalarySummaryView> salarySummaryViewForRegularManagementEmployee = PayrollReportManager.GetEmployeeSalarySummary(companyId, branchId, branchUnitIds, employeeTypeIds, employeeCategoryId, year, month, fromDate, toDate);
            var reportDataSourceForRegularManagementEmployee = new ReportDataSource("DataSet2", salarySummaryViewForRegularManagementEmployee);

            //Quit Employee
            employeeTypeIds = new List<int>();
            employeeTypeIds = (from userEmployeeType in employeeTypeIdList where userEmployeeType.Id != 1 select userEmployeeType.Id).ToList();

            employeeCategoryId = (int)SCERP.Common.EmployeeCategory.QUIT;

            List<SalarySummaryView> salarySummaryViewForQuitEmployee = PayrollReportManager.GetEmployeeSalarySummary(companyId, branchId, branchUnitIds, employeeTypeIds, employeeCategoryId, year, month, fromDate, toDate);
            var reportDataSourceForQuitEmployee = new ReportDataSource("DataSet3", salarySummaryViewForQuitEmployee);


            //New Joining
            employeeTypeIds = new List<int>();
            employeeTypeIds = (from userEmployeeType in employeeTypeIdList where userEmployeeType.Id != 1 select userEmployeeType.Id).ToList();

            employeeCategoryId = (int)SCERP.Common.EmployeeCategory.NEW_JOINING;

            List<SalarySummaryView> salarySummaryViewForNewJoiningEmployee = PayrollReportManager.GetEmployeeSalarySummary(companyId, branchId, branchUnitIds, employeeTypeIds, employeeCategoryId, year, month, fromDate, toDate);
            var reportDataSourceForNewJoiningEmployee = new ReportDataSource("DataSet4", salarySummaryViewForNewJoiningEmployee);


            //New Joining and Quit
            employeeTypeIds = new List<int>();
            employeeTypeIds = (from userEmployeeType in employeeTypeIdList where userEmployeeType.Id != 1 select userEmployeeType.Id).ToList();

            employeeCategoryId = (int)SCERP.Common.EmployeeCategory.New_JOINING_AND_QUIT;

            List<SalarySummaryView> salarySummaryViewForNewJoiningAndQuitEmployee = PayrollReportManager.GetEmployeeSalarySummary(companyId, branchId, branchUnitIds, employeeTypeIds, employeeCategoryId, year, month, fromDate, toDate);
            var reportDataSourceForNewJoiningAndQuitEmployee = new ReportDataSource("DataSet5", salarySummaryViewForNewJoiningAndQuitEmployee);


            //Salary Summary (All)
            List<SalarySummaryView> salarySummaryViewAll = PayrollReportManager.GetEmployeeSalarySummaryAll(companyId, branchId, branchUnitIds, employeeTypeIds, year, month, fromDate, toDate);
            var reportDataSourceSalarySummaryViewAll = new ReportDataSource("DataSet6", salarySummaryViewAll);
            lr.SetParameters(parameters);
            lr.DataSources.Add(reportDataSourceForRegularTeamMember);
            lr.DataSources.Add(reportDataSourceForRegularManagementEmployee);
            lr.DataSources.Add(reportDataSourceForQuitEmployee);
            lr.DataSources.Add(reportDataSourceForNewJoiningEmployee);
            lr.DataSources.Add(reportDataSourceForNewJoiningAndQuitEmployee);
            lr.DataSources.Add(reportDataSourceSalarySummaryViewAll);

            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0.0in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
                "  <MarginRight>.0in</MarginRight>" +
                "  <MarginBottom>0.0in</MarginBottom>" +
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

        public ActionResult WeekendBillIndex(ReportSearchViewModel model)
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

        public ActionResult WeekendBill(ReportSearchViewModel model)
        {
            List<WeekendBillModel> reportdata = PayrollReportManager.GetWeekendBill(model.FromDate.Value);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "WeekendBill.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            string companyId = PortalContext.CurrentUser.CompId;
            string companyName = PayrollReportManager.GetCompanyNameByCompanyId(companyId);

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_CompanySector", companyName);

            ReportDataSource rd = new ReportDataSource("DataSet1", reportdata);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>11.7in</PageWidth>" +
                "  <PageHeight>8.3in</PageHeight>" +
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

        public ActionResult WeekendBillProcess(ReportSearchViewModel model)
        {
            int result = PayrollReportManager.ProcessWeekendBill(model.FromDate.Value);

            if (result > 0)
                return RedirectToAction("WeekendBillIndex");
            else
                return ErrorResult("No data found !");
        }

        public ActionResult SalarySummaryShortIndex(ReportSearchViewModel model)
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

        public ActionResult SalarySummaryShort(ReportSearchViewModel model)
        {

            return null;
        }

        public ActionResult SalaryIncrementForWorkerIndex(ReportSearchViewModel model)
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

                model.EmployeeDesignations =
                    EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(
                        model.SearchFieldModel.SearchByEmployeeGradeId);
                model.Genders = GenderManager.GetAllGenders();

                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeStatuses = statusList;


                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;
                model.SearchFieldModel.IncrementPercent = 5;
                model.SearchFieldModel.IncrementAmount = 0;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult SalaryIncrementForWorker(ReportSearchViewModel model)
        {
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            string employeeCardId = model.EmployeeCardId;
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;
            float? incrementPercent = model.SearchFieldModel.IncrementPercent;
            decimal? incrementAmount = model.SearchFieldModel.IncrementAmount;

            string userName = PortalContext.CurrentUser.Name;

            List<SPSalaryIncrementReport> reportdata = PayrollReportManager.GetSalaryIncrementReport(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, fromDate, toDate, employeeCardId, incrementPercent, incrementAmount, userName);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "SalaryIncrement.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportParameter[] parameters = new ReportParameter[5];

            string monthName = toDate.Value.ToString("MMMM") + ", " + toDate.Value.Year.ToString();
            parameters[0] = new ReportParameter("param_Month", monthName);

            Branch branch = BranchManager.GetBranchById(branchId);
            Unit unit = UnitManager.GetUnitById(branchUnitId.Value);

            BranchUnitDepartment bud = BranchUnitDepartmentManager.GetBranchUnitDepartmentById(branchUnitDepartmentId.Value);
            UnitDepartment ud = new UnitDepartment();
            Department department = new Department();
            if (bud != null)
            {
                ud = UnitDepartmentManager.GetUnitDepartmentById(bud.UnitDepartmentId);
            }
            if (ud != null)
            {
                department = DepartmentManager.GetDepartmentById(ud.DepartmentId);
            }

            Section section = new Section();
            DepartmentSection ds = DepartmentSectionManager.GetDepartmentSectionById(departmentSectionId.Value);
            if (ds != null)
            {
                section = SectionManager.GetSectionById(ds.SectionId);
            }
            else
            {
                section = null;
            }

            if (branch != null)
                parameters[1] = new ReportParameter("param_Branch", branch.Name);
            else
                parameters[1] = new ReportParameter("param_Branch", "All");


            if (unit != null)
                parameters[2] = new ReportParameter("param_Unit", unit.Name);
            else
                parameters[2] = new ReportParameter("param_Unit", "All");


            if (department != null)
                parameters[3] = new ReportParameter("param_Department", department.Name);
            else
                parameters[3] = new ReportParameter("param_Department", "All");


            if (section != null)
                parameters[4] = new ReportParameter("param_Section", section.Name);
            else
                parameters[4] = new ReportParameter("param_Section", "All");


            ReportDataSource rd = new ReportDataSource("DataSet1", reportdata);
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
                "  <PageHeight>8.3in</PageHeight>" +
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

        public ActionResult SalaryIncrementSummaryForWorkerIndex(ReportSearchViewModel model)
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

                model.EmployeeDesignations =
                    EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(
                        model.SearchFieldModel.SearchByEmployeeGradeId);
                model.Genders = GenderManager.GetAllGenders();

                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };
                model.EmployeeStatuses = statusList;


                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                model.PrintFormatStatuses = printFormat;
                model.SearchFieldModel.IncrementPercent = 5;
                model.SearchFieldModel.IncrementAmount = 0;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult SalaryIncrementSummaryForWorker(ReportSearchViewModel model)
        {
            int? companyId = model.SearchFieldModel.SearchByCompanyId;
            int? branchId = model.SearchFieldModel.SearchByBranchId;
            int? branchUnitId = model.SearchFieldModel.SearchByBranchUnitId;
            int? branchUnitDepartmentId = model.SearchFieldModel.SearchByBranchUnitDepartmentId;
            int? departmentSectionId = model.SearchFieldModel.SearchByDepartmentSectionId;
            int? departmentLineId = model.SearchFieldModel.SearchByDepartmentLineId;
            int? employeeTypeId = model.SearchFieldModel.SearchByEmployeeTypeId;
            string employeeCardId = model.EmployeeCardId;
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;
            float? incrementPercent = model.SearchFieldModel.IncrementPercent;
            decimal? incrementAmount = model.SearchFieldModel.IncrementAmount;

            string userName = PortalContext.CurrentUser.Name;

            List<SPSalaryIncrementReport> reportdata = PayrollReportManager.GetSalaryIncrementReport(companyId, branchId, branchUnitId, branchUnitDepartmentId, departmentSectionId, departmentLineId, employeeTypeId, fromDate, toDate, employeeCardId, incrementPercent, incrementAmount, userName);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "SalaryIncrementSummary.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            string monthName = toDate.Value.ToString("MMMM") + ", " + toDate.Value.Year.ToString();
            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_Month", monthName);

            ReportDataSource rd = new ReportDataSource("DataSet1", reportdata);
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
                "  <PageHeight>8.3in</PageHeight>" +
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

        public ActionResult SalaryBankStatementIndex(ReportSearchViewModel model)
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

        public ActionResult SalaryBankStatement(ReportSearchViewModel model)
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

            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;

            string path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "SalaryBankStatement.rdlc");

            DataTable dt = PayrollReportManager.GetSalaryBankStatement(fromDate, toDate);

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", dt) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
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

        public JsonResult GetDepartmentLineByBranchUnitDepartmentId(int branchUnitDepartmentId)
        {
            var departmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(branchUnitDepartmentId);
            return Json(new { Success = true, DataSources = departmentLines }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartmentSectionAndLineByBranchUnitDepartmentId(int branchUnitDepartmentId)
        {
            var departmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(branchUnitDepartmentId);
            var departmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(branchUnitDepartmentId);
            return Json(new { Success = true, DepartmentSections = departmentSections, DepartmentLines = departmentLines, }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllGradeByTypeId(int employeeTypeId)
        {
            var grades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(employeeTypeId);
            return Json(new { Success = true, EmployeeGrades = grades }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllDesignationsByEmployeeGradeId(int employeeGradeId)
        {
            var employeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(employeeGradeId);
            return Json(new { Success = true, EmployeeDesignations = employeeDesignations }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmployeeSalarySummaryIndex(ReportSearchViewModel model)
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

        public ActionResult EmployeeSalarySummary(ReportSearchViewModel model)
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

            string path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "SalarySummaryMultiMonth.rdlc");
            DataTable data = PayrollReportManager.SalarySummaryMultipleMonth(companyId, branchId, fromYear, toYear, fromMonth, toMonth, fromDate, toDate);

            int countMonth = data.AsEnumerable().Select(r => r.Field<DateTime>("FromDate")).Distinct().Count();

            var parameters = new List<ReportParameter>() { new ReportParameter("Param_CountMonth", countMonth.ToString()) };

            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", data) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, parameters);
        }

        public ActionResult EmployeeSalarySummaryTopSheetIndex(ReportSearchViewModel model)
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

        public ActionResult EmployeeSalarySummaryTopSheet(ReportSearchViewModel model)
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
            int year = Convert.ToInt32(model.SelectedYear);
            int month = Convert.ToInt32(model.SelectedMonth);
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;

            string path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "SalarySummaryTopSheet.rdlc");
            DataTable data = PayrollReportManager.SalarySummaryTopSheet(companyId, branchId, year, month, fromDate, toDate);

            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", data) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult EmployeeSalaryXLIndex(ReportSearchViewModel model)
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

        public ActionResult EmployeeSalaryXL(ReportSearchViewModel model)
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
            int year = Convert.ToInt32(model.SelectedYear);
            int month = Convert.ToInt32(model.SelectedMonth);
            DateTime? fromDate = model.FromDate;
            DateTime? toDate = model.ToDate;

            string path = Path.Combine(Server.MapPath("~/Areas/Payroll/Reports"), "SalaryXL.rdlc");
            DataTable data = PayrollReportManager.SalaryXL(companyId, branchId, year, month, fromDate, toDate);

            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", data) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }
    }
}