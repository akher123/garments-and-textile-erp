using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Org.BouncyCastle.Asn1.Cms;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Web.Controllers;


namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeesController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        Guid _employeeGuidId = Guid.Parse("00000000-0000-0000-0000-000000000000");

        [AjaxAuthorize(Roles = "employee-1,employee-2,employee-3")]
        public ActionResult Index(EmployeeInfoCustomModel model)
        {
            Session["EmployeeGuid"] = null;
           
            ModelState.Clear();
            model.Companies = CompanyManager.GetAllPermittedCompanies();
            model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
            model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
            model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
            model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
            model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
            model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
            model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
            model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(model.SearchFieldModel.SearchByEmployeeGradeId);
            model.EmployeeBloodGroups = BloodGroupManager.GetAllBloodGroups();
            model.Genders = GenderManager.GetAllGenders();
            var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                             select new { Id = (int)status, Name = status.ToString() };
            ViewBag.EmployeeStatus = new SelectList(statusList, "Id", "Name");

            if (model.IsSearch)
            {
                model.IsSearch = false;
                return View(model);
            }

            var startPage = 0;
            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }

            var totalRecords = 0;
            model.Employees = EmployeeManager.GetAllEmployeeInfoByPaging(startPage, _pageSize, model.SearchFieldModel, model, out totalRecords) ?? new List<EmployeeInfoCustomModel>();
            model.TotalRecords = totalRecords;

            return View(model);

        }

        [AjaxAuthorize(Roles = "employeemandatoryinfo-1,employeemandatoryinfo-2,employeemandatoryinfo-3")]
        public ActionResult CreateNewEmployee(EmployeeMandatoryInfoCustomModel model)
        {
            model.EmployeeId = _employeeGuidId;
            model.Genders = GenderManager.GetAllGenders();
            model.Religions = ReligionManager.GetAllReligions();
            model.Companies = CompanyManager.GetAllPermittedCompanies();
            model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
            if (Session["EmployeeGuid"] != null)
            {
                var employeeMandatoryInfo = EmployeeManager.GetEmployeeMandatoryInfoByEmployeeId(model.EmployeeId, model.FromDate);

                if (employeeMandatoryInfo != null)
                {
                    model.Employee.EmployeeCardId = employeeMandatoryInfo.Employee.EmployeeCardId.Trim()
                        .Replace(" ", "");
                    model.Employee.Name = employeeMandatoryInfo.Employee.Name;
                    model.Employee.NameInBengali = employeeMandatoryInfo.Employee.NameInBengali;
                    model.Employee.MothersName = employeeMandatoryInfo.Employee.MothersName;
                    model.Employee.MothersNameInBengali = employeeMandatoryInfo.Employee.MothersNameInBengali;
                    model.Employee.FathersName = employeeMandatoryInfo.Employee.FathersName;
                    model.Employee.FathersNameInBengali = employeeMandatoryInfo.Employee.FathersNameInBengali;
                    model.Employee.ReligionId = employeeMandatoryInfo.Employee.ReligionId;
                    model.Employee.DateOfBirth = employeeMandatoryInfo.Employee.DateOfBirth;
                    model.Employee.GenderId = employeeMandatoryInfo.Employee.GenderId;
                    model.Employee.JoiningDate = employeeMandatoryInfo.Employee.JoiningDate;
                    model.Employee.ConfirmationDate = employeeMandatoryInfo.Employee.ConfirmationDate;

                    model.EmployeeCompanyId = employeeMandatoryInfo.Company.Id;
                    model.EmployeeBranchId = employeeMandatoryInfo.Branch.Id;
                    model.EmployeeBranchUnitId = employeeMandatoryInfo.BranchUnit.BranchUnitId;
                    model.EmployeeBranchUnitDepartmentId =
                        employeeMandatoryInfo.BranchUnitDepartment.BranchUnitDepartmentId;
                    model.EmployeeDepartmentSectionId = employeeMandatoryInfo.DepartmentSectionId;
                    model.EmployeeDepartmentLineId = employeeMandatoryInfo.DepartmentLineId;
                    model.EmployeeTypeId = employeeMandatoryInfo.EmployeeType.Id;
                    model.EmployeeGradeId = employeeMandatoryInfo.EmployeeGrade.Id;
                    model.EmployeeDesignationId = employeeMandatoryInfo.EmployeeDesignation.Id;
                    model.IsEligibleForOvertime = employeeMandatoryInfo.IsEligibleForOvertime;

                    model.EmployeePresentAddress.MobilePhone = employeeMandatoryInfo.EmployeePresentAddress.MobilePhone;
                    model.EmployeePresentAddress.MailingAddress = employeeMandatoryInfo.EmployeePresentAddress.MailingAddress;
                    model.EmployeePresentAddress.MailingAddressInBengali = employeeMandatoryInfo.EmployeePresentAddress.MailingAddressInBengali;
                    model.EmployeePresentAddress.LandlordName = employeeMandatoryInfo.EmployeePresentAddress.LandlordPhone;
                    model.EmployeePresentAddress.LandlordNameInBengali = employeeMandatoryInfo.EmployeePresentAddress.LandlordNameInBengali;
                    model.EmployeePresentAddress.LandlordPhone = employeeMandatoryInfo.EmployeePresentAddress.LandlordPhone;
                }
            }
            model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.EmployeeCompanyId);
            model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.EmployeeBranchId);
            model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.EmployeeBranchUnitId);
            model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.EmployeeBranchUnitDepartmentId);
            model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.EmployeeBranchUnitDepartmentId);
            model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.EmployeeTypeId);
            model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(model.EmployeeGradeId);
            return View(model);

        }

        [AjaxAuthorize(Roles = "employee-1,employee-2,employee-3")]
        public ActionResult EmployeeDetail(Guid? employeeId, DateTime? effectiveDate)
        {
            ModelState.Clear();

            Session["EmployeeGuid"] = employeeId;

            var model = EmployeeManager.GetEmployeeMandatoryInfoByEmployeeId(employeeId, effectiveDate);

            model.Genders = GenderManager.GetAllGenders();
            model.Religions = ReligionManager.GetAllReligions();
            model.Companies = CompanyManager.GetAllPermittedCompanies();
            model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.EmployeeCompanyId);
            model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.EmployeeBranchId);
            model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.EmployeeBranchUnitId);
            model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.EmployeeBranchUnitDepartmentId);
            model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.EmployeeBranchUnitDepartmentId);
            model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();
            model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.EmployeeTypeId);
            model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(model.EmployeeGradeId);
            return View(model);
        }

        [AjaxAuthorize(Roles = "employeepersonalinfo-1,employeepersonalinfo-2,employeepersonalinfo-3")]
        public ActionResult EditPersonalInformation(Models.ViewModels.EmployeePersonalInfoViewModel model)
        {
            ModelState.Clear();

            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];
            else
            {
                return ErrorMessageResult();
            }
            model.MaritalStates = MaritalStatusManager.GetAllMaritalStatuses();
            model.BloodGroups = BloodGroupManager.GetAllBloodGroups();
            var employeePersonalInfo = EmployeeManager.GetEmployeeById(_employeeGuidId);
            if (employeePersonalInfo != null)
            {
                model.SpousesName = employeePersonalInfo.SpousesName;
                model.SpousesNameInBengali = employeePersonalInfo.SpousesNameInBengali;
                model.MaritalStateId = employeePersonalInfo.MaritalStateId;
                model.MariageAnniversary = employeePersonalInfo.MariageAnniversary;
                model.Nationality = employeePersonalInfo.Nationality;
                model.NationalityInBengali = employeePersonalInfo.NationalityInBengali;
                model.BirthRegistrationNo = employeePersonalInfo.BirthRegistrationNo;
                model.NationalIdNo = employeePersonalInfo.NationalIdNo;
                model.TaxIdentificationNo = employeePersonalInfo.TaxIdentificationNo;
                model.DrivingLicenseNo = employeePersonalInfo.DrivingLicenseNo;
                model.PassportNo = employeePersonalInfo.PassportNo;
                model.BloodGroupId = employeePersonalInfo.BloodGroupId;
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeequitinfo-1,employeequitinfo-2,employeequitinfo-3")]
        public ActionResult EditQuitInformation(Models.ViewModels.EmployeeQuitInfoViewModel model)
        {
            ModelState.Clear();
            string serviceDuration = "";

            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid) Session["EmployeeGuid"];
            else
            {
                return ErrorMessageResult();
            }

            var employeeInfo = EmployeeManager.GetEmployeeById(_employeeGuidId);

            model.QuitTypes = QuitTypeManager.GetAllQuitTypes();

            TimeSpan duration = DateTime.Now - employeeInfo.JoiningDate.Value;
            int days = duration.Days;

            if (days >= 365)
            {
                serviceDuration = (days - days%365)/365 + " Year ";
                days = days%365;
            }

            if (days >= 30)
            {
                serviceDuration = serviceDuration + (days - days%30)/30 + " Month ";
                days = days%30;
            }
            serviceDuration = serviceDuration + days + " Day ";

            model.ServiceDuration = serviceDuration;

            if (employeeInfo != null && employeeInfo.Status == (int) StatusValue.InActive)
            {
                model.QuitTypeId = employeeInfo.QuitTypeId;
                model.QuitDate = employeeInfo.QuitDate;
            }
            else
            {
                model.QuitTypeId = null;
                model.QuitDate = null;
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeequitinfo-1,employeequitinfo-2,employeequitinfo-3")]
        public ActionResult SkillMatrix(Models.ViewModels.SkillMatrixNew model)
        {
            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid) Session["EmployeeGuid"];

            if (model.MachineTypeId == 0)
                model.MachineTypeId = 1;

            model.SkillMatrixMachineTypes = EmployeeManager.GetAllMachineTypes();
            model.SkillMatrixDetails = EmployeeManager.GetSkillMatrixDetailByMachineTypeId(model.MachineTypeId, _employeeGuidId).ToList();
            model.SkillMatrixPointTable = EmployeeManager.GetAllSkillPoint(_employeeGuidId);

            return View(model);
        }

        [HttpPost]
        public JsonResult SaveSkillMatrix(List<string> values)
        {
            ModelState.Clear();

            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];

            string employeeCardId = EmployeeManager.GetEmployeeById(_employeeGuidId).EmployeeCardId;

            var result = EmployeeManager.SaveSkillMatrix(values, _employeeGuidId, employeeCardId);
            return Json(new { Success = true, Result = result });
        }

        [HttpPost]
        public JsonResult SaveSkillMatrixPoint(Models.ViewModels.SkillMatrixNew model)
        {
            ModelState.Clear();

            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];

            model.SkillMatrixPointTable.EmployeeId = _employeeGuidId;
            model.SkillMatrixPointTable.EmployeeCardId = EmployeeManager.GetEmployeeById(_employeeGuidId).EmployeeCardId;        

            var result = EmployeeManager.SaveSkillMatrixPoint(model.SkillMatrixPointTable);
            return Json(new { Success = true, Result = result });
        }

        [AjaxAuthorize(Roles = "employee-3")]
        public ActionResult DeleteEmployee(Guid employeeId)
        {
            try
            {
                EmployeeManager.DeleteEmployee(employeeId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return Reload();
        }

        [AjaxAuthorize(Roles = "employeemandatoryinfo-2,employeemandatoryinfo-3")]
        public ActionResult SaveEmployeeMandatoryInfo(EmployeeMandatoryInfoCustomModel employeeMandatoryInfoCustomModel)
        {
            int saveEmployeeSalary = 0;
            try
            {
                using (var transactionScope = new TransactionScope())
                {
                    if (String.IsNullOrEmpty(employeeMandatoryInfoCustomModel.Employee.PhotographPath))
                        return ErrorResult("Employee photo was not uploaded!");

                    var employeeIdGuid = Guid.NewGuid();
                    employeeMandatoryInfoCustomModel.Employee.EmployeeId = employeeIdGuid;

                    var isExist = EmployeeManager.CheckExistingEmployeeCardNumber(employeeMandatoryInfoCustomModel.Employee);
                    if (isExist)
                    {
                        return ErrorResult(string.Format("{0} Card Number already exists!", employeeMandatoryInfoCustomModel.Employee.EmployeeCardId));
                    }

                    if (employeeMandatoryInfoCustomModel.Employee.DateOfBirth > employeeMandatoryInfoCustomModel.Employee.JoiningDate)
                        return ErrorResult("Date of birth can not be greater than joining date!");

                    if (employeeMandatoryInfoCustomModel.Employee.DateOfBirth > employeeMandatoryInfoCustomModel.Employee.ConfirmationDate)
                        return ErrorResult("Date of birth can not be greater than confirmation date!");

                    if (employeeMandatoryInfoCustomModel.Employee.JoiningDate > employeeMandatoryInfoCustomModel.Employee.ConfirmationDate)
                        return ErrorResult("Joining date can not be greater than confirmation date!");

                    var salarySetup =
                        SalarySetupManager.GetSalarySetupByEmployeeGrade(employeeMandatoryInfoCustomModel.EmployeeGradeId, employeeMandatoryInfoCustomModel.Employee.JoiningDate);
                    if (salarySetup == null)
                        return ErrorResult("Salary setup for this employee's type and grade doesn't exist!");

                    employeeMandatoryInfoCustomModel.Employee.EmployeeCardId =
                        employeeMandatoryInfoCustomModel.Employee.EmployeeCardId.Trim().Replace(" ", "");
                    _employeeGuidId = EmployeeManager.SaveEmployeeMandatoryInfo(employeeMandatoryInfoCustomModel);
                    if (_employeeGuidId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                        return ErrorResult("Failed to save mandatory information!");

                    Session["EmployeeGuid"] = _employeeGuidId;

                    employeeMandatoryInfoCustomModel.EmployeeCompanyInfo.EmployeeId = _employeeGuidId;
                    employeeMandatoryInfoCustomModel.EmployeeCompanyInfo.FromDate = employeeMandatoryInfoCustomModel.Employee.JoiningDate;

                    var employeeCompanyInfo = EmployeeCompanyInfoManager.GetEmployeeLatestCompanyInfoByEmployeeGuidId(employeeMandatoryInfoCustomModel.EmployeeCompanyInfo);
                    if (employeeCompanyInfo == null) return ErrorResult("Departmental information was not saved!");

                    var employeeSalary = new EmployeeSalary
                    {
                        EmployeeId = _employeeGuidId,
                        GrossSalary = salarySetup.GrossSalary,
                        BasicSalary = salarySetup.BasicSalary,
                        HouseRent = salarySetup.HouseRent,
                        MedicalAllowance = salarySetup.MedicalAllowance,
                        FoodAllowance = salarySetup.FoodAllowance,
                        Conveyance = salarySetup.Conveyance,
                        EntertainmentAllowance = salarySetup.EntertainmentAllowance,
                        FromDate = employeeMandatoryInfoCustomModel.Employee.JoiningDate
                    };
                    saveEmployeeSalary = EmployeeSalaryManager.SaveEmployeeSalary(employeeSalary);

                    string employeeCardId = employeeMandatoryInfoCustomModel.Employee.EmployeeCardId;

                    int year = 0;
                    if (employeeMandatoryInfoCustomModel.Employee.JoiningDate != null)
                    {
                        year = employeeMandatoryInfoCustomModel.Employee.JoiningDate.Value.Year;
                    }

                    int branchUnitId = employeeMandatoryInfoCustomModel.EmployeeBranchUnitId;
                    int employeeTypeId = employeeMandatoryInfoCustomModel.EmployeeTypeId;

                    int saveEmployeeLeaveHistory = 0;
                    saveEmployeeLeaveHistory = EmployeeLeaveManager.SaveIndividualLeaveHistoryForSpecificYear(_employeeGuidId, employeeCardId, year, branchUnitId, employeeTypeId);
                    if (saveEmployeeLeaveHistory <= 0) return ErrorResult("Leave information can not be saved!");

                    int saveEmployeeWorkShift = 0;
                    saveEmployeeWorkShift = EmployeeWorkShiftManager.SaveNewJoiningEmployeeWorkShift(_employeeGuidId, employeeMandatoryInfoCustomModel.Employee.JoiningDate, branchUnitId);
                    if (saveEmployeeWorkShift <= 0) return ErrorResult("Work shift information can not be saved!");

                    if (employeeMandatoryInfoCustomModel.Employee.JoiningDate != null)
                        OvertimeEligibleEmployeeManager.SaveNewEmployeeOverTime(_employeeGuidId, employeeMandatoryInfoCustomModel.Employee.JoiningDate.Value);

                    transactionScope.Complete();
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return CreateJsonResult(saveEmployeeSalary > 0 ? new {Success = true} : new {Success = false});
        }

        [AjaxAuthorize(Roles = "employeemandatoryinfo-2,employeemandatoryinfo-3")]
        public ActionResult EditEmployeeMandatoryInfo(EmployeeMandatoryInfoCustomModel employeeMandatoryInfoCustomModel)
        {

            int edited = 0;

            try
            {
                if (Session["EmployeeGuid"] != null)
                {
                    _employeeGuidId = (Guid)Session["EmployeeGuid"];

                    if (employeeMandatoryInfoCustomModel.Employee.DateOfBirth > employeeMandatoryInfoCustomModel.Employee.JoiningDate)
                        return ErrorResult("Date of birth can not be greater than joining date!");

                    if (employeeMandatoryInfoCustomModel.Employee.DateOfBirth > employeeMandatoryInfoCustomModel.Employee.ConfirmationDate)
                        return ErrorResult("Date of birth can not be greater than confirmation date!");

                    if (employeeMandatoryInfoCustomModel.Employee.JoiningDate > employeeMandatoryInfoCustomModel.Employee.ConfirmationDate)
                        return ErrorResult("Confirmation date can not be greater than joining date!");

                    employeeMandatoryInfoCustomModel.EmployeeCompanyInfo.EmployeeId = _employeeGuidId;

                    using (var transactionScope = new TransactionScope())
                    {
                        employeeMandatoryInfoCustomModel.Employee.EmployeeId = _employeeGuidId;
                        employeeMandatoryInfoCustomModel.EmployeeCompanyInfo.EmployeeId = _employeeGuidId;
                        employeeMandatoryInfoCustomModel.EmployeePresentAddress.EmployeeId = _employeeGuidId;

                        edited = EmployeeManager.EditEmployeeMandatoryInfo(employeeMandatoryInfoCustomModel);

                        transactionScope.Complete();
                    }
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return edited > 0 ? CreateJsonResult(new { Success = true }) : ErrorResult("Failed to update mandatory information!");
        }

        [AjaxAuthorize(Roles = "employeepersonalinfo-2,employeepersonalinfo-3")]
        public ActionResult EditEmployeePersonalInfo(Models.ViewModels.EmployeePersonalInfoViewModel employee)
        {
            if (Session["EmployeeGuid"] != null)
            {
                _employeeGuidId = (Guid)Session["EmployeeGuid"];
            }
            employee.EmployeeId = _employeeGuidId;
            var editedEmployee = 0;
            editedEmployee = EmployeeManager.EditEmployeePersonalInfo(employee);
            return CreateJsonResult(editedEmployee > 0 ? new { Success = true } : new { Success = false });
        }

        [AjaxAuthorize(Roles = "employeequitinfo-2,employeequitinfo-3")]
        public ActionResult EditEmployeeQuitInfo(Models.ViewModels.EmployeeQuitInfoViewModel employeeQuitInfo)
        {
            var editedEmployee = 0;
            if (Session["EmployeeGuid"] != null)
            {
                _employeeGuidId = (Guid)Session["EmployeeGuid"];

                var employeeInfo = EmployeeManager.GetEmployeeById(_employeeGuidId);

                if (employeeInfo != null)
                {
                    if (employeeInfo.JoiningDate > employeeQuitInfo.QuitDate)
                        return ErrorResult("Joining date can not be greater than quit date!");
                }
                employeeQuitInfo.EmployeeId = _employeeGuidId;

                editedEmployee = EmployeeManager.EditEmployeeQuitInfo(employeeQuitInfo);
            }

            return CreateJsonResult(editedEmployee > 0 ? new { Success = true } : new { Success = false });
        }

        public ActionResult Upload(HttpPostedFileBase photoimg)
        {
            if (Session["EmployeeGuid"] != null)
            {
                _employeeGuidId = (Guid)Session["EmployeeGuid"];
            }
            else
            {
                return Json(new { Status = false, Message = "Please fill up basic information first" },
                    JsonRequestBehavior.AllowGet);
            }
            if (photoimg == null || photoimg.ContentLength <= 0)
                return Json(new { Sucess = false, Message = "Please select image first" }, JsonRequestBehavior.AllowGet);
            const int maxContentLength = 1024 * 124; //1 MB
            var allowedFileExtensions = new string[] { ".jpg", ".gif", ".png" };
            if (!allowedFileExtensions.Contains(photoimg.FileName.Substring(photoimg.FileName.LastIndexOf('.'))))
            {
                return Json(
                    new
                    {
                        Success = false,
                        Message = "Upload image of type:" + string.Join(",", allowedFileExtensions)
                    },
                    JsonRequestBehavior.AllowGet);

            }
            if (photoimg.ContentLength < maxContentLength)
            {
                return Json(
                    new
                    {
                        Success = false,
                        Message =
                            " Your image is too large, maximum allowed size is: " + 1 + " MB"
                    }, JsonRequestBehavior.AllowGet);
            }
            var img = new WebImage(photoimg.InputStream);
            img.Resize(200, 200, true, true);
            var imgname = _employeeGuidId + "-" + Path.GetFileName(photoimg.FileName);
            var path = Path.Combine(Server.MapPath(@"~/Areas/HRM/Photoes"), imgname);
            img.Save(path);
            var relativePath = "~/Areas/HRM/Photoes/" + imgname;
            var employeeMandatoryInfo = new EmployeeMandatoryInfoViewModel { Employee = { EmployeeId = _employeeGuidId, PhotographPath = relativePath } };
            var info = EmployeeManager.EditEmployeePhoto(employeeMandatoryInfo);
            return Json(new { Success = true, url = Url.Content(@"~\Areas\HRM\Photoes\" + imgname) },
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult UploadImage(HttpPostedFileBase file, Employee employee)
        {
            if (!Directory.Exists((Server.MapPath(@"~/Areas/HRM/EmployeePhotograph"))))
            {
                Directory.CreateDirectory((Server.MapPath(@"~/Areas/HRM/EmployeePhotograph")));
            }

            Guid flder = Guid.NewGuid();
            var img = new WebImage(file.InputStream);
            var imgname = flder + "-" + Path.GetFileName(file.FileName);
            employee.PhotographPath = Path.Combine(Server.MapPath(@"~/Areas/HRM/EmployeePhotograph"), imgname);
            img.Save(employee.PhotographPath);

            img.Resize(200, 200, true, true);
            var thumbnailimgname = flder + "-" + "thumbnail" + "-" + Path.GetFileName(file.FileName);
            var thumbnailpath = Path.Combine(Server.MapPath(@"~/Areas/HRM/EmployeePhotograph"), thumbnailimgname);
            img.Save(thumbnailpath);

            return Json(new { Success = true, thumbnailpath = @"/Areas/HRM/EmployeePhotograph/" + thumbnailimgname, fullpath = @"/Areas/HRM/EmployeePhotograph/" + imgname }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetAllBranchesByCompanyId(int companyId)
        {
            var branches = BranchManager.GetAllPermittedBranchesByCompanyId(companyId);
            return Json(new { Success = true, Branches = branches }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllBranchUnitsByBranchId(int branchId)
        {
            var brancheUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(branchId);
            return Json(new { Success = true, BrancheUnits = brancheUnits }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBranchUnitDepartmentsByBranchUnitId(int branchUnitId)
        {
            var branchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(branchUnitId);
            return Json(new { Success = true, BranchUnitDepartments = branchUnitDepartments }, JsonRequestBehavior.AllowGet);
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

        public JsonResult GetDepartmentSectionAndLineByBranchUnitDepartmentId(int branchUnitDepartmentId)
        {
            var departmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(branchUnitDepartmentId);
            var departmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(branchUnitDepartmentId);
            return Json(new { Success = true, DepartmentSections = departmentSections, DepartmentLines = departmentLines, }, JsonRequestBehavior.AllowGet);
        }
    }
}
