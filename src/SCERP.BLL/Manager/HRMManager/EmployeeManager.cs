using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeManager : BaseManager, IEmployeeManager
    {
        private readonly IEmployeeRepository _employeeRepository = null;
        private readonly IDepartmentRepository _departmentRepository = null;
        private readonly IEmployeeCompanyInfoRepository _employeeCompanyInfoRepository = null;
        private readonly IEmployeePresentAddressRepository _employeePresentAddressRepository = null;
        private readonly IEmployeePermanentAddressRepository _employeePermanentAddressRepository = null;
        private readonly IEmployeeDesignationRepository _employeeDesignationRepository = null;
        private readonly IEmployeeGradeRepository _employeeGradeRepository = null;
        private readonly ISalarySetupRepository _salarySetupRepository = null;
        private readonly IEmployeeSalaryRepository _employeeSalaryRepository = null;
        private readonly string _compId;

        public EmployeeManager(SCERPDBContext context)
        {
            this._employeeRepository = new EmployeeRepository(context);
            this._departmentRepository = new DepartmentRepository(context);
            this._employeeCompanyInfoRepository = new EmployeeCompanyInfoRepository(context);
            this._employeePresentAddressRepository = new EmployeePresentAddressRepository(context);
            this._employeePermanentAddressRepository = new EmployeePermanentAddressRepository(context);
            this._employeeDesignationRepository = new EmployeeDesignationRepository(context);
            this._employeeGradeRepository = new EmployeeGradeRepository(context);
            this._salarySetupRepository = new SalarySetupRepository(context);
            this._employeeSalaryRepository = new EmployeeSalaryRepository(context);
            _compId = PortalContext.CurrentUser.CompId;
        }

        public IQueryable<Department> GetAllDepartments()
        {
            IQueryable<Department> department;

            try
            {
                department = _departmentRepository.Filter(x => x.IsActive);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return department;
        }

        public Guid SaveEmployeeInfo(EmployeeViewModel employeeViewModel)
        {
            var savedEmployee = Guid.Parse("00000000-0000-0000-0000-000000000000");

            try
            {
                //Employee Extra Info
                Guid employeeIGuid = Guid.NewGuid();
                employeeViewModel.Employee.EmployeeId = employeeIGuid;
                employeeViewModel.Employee.CreatedDate = DateTime.Now;
                employeeViewModel.Employee.CreatedBy = PortalContext.CurrentUser.UserId;
                employeeViewModel.Employee.IsActive = true;

                //Employee Extra Info
                employeeViewModel.EmployeeCompanyInfo.EmployeeId = employeeIGuid;
                employeeViewModel.EmployeeCompanyInfo.CreatedDate = DateTime.Now;
                employeeViewModel.EmployeeCompanyInfo.CreatedBy = PortalContext.CurrentUser.UserId;
                employeeViewModel.EmployeeCompanyInfo.IsActive = true;

                //Employee Present Address Info
                employeeViewModel.EmployeePresentAddress.EmployeeId = employeeIGuid;
                employeeViewModel.EmployeePresentAddress.CreatedDate = DateTime.Now;
                employeeViewModel.EmployeePresentAddress.CreatedBy = PortalContext.CurrentUser.UserId;
                employeeViewModel.EmployeePresentAddress.IsActive = true;

                //Employee Permanent Address Info
                employeeViewModel.EmployeePermanentAddress.EmployeeId = employeeIGuid;
                employeeViewModel.EmployeePermanentAddress.CreatedDate = DateTime.Now;
                employeeViewModel.EmployeePermanentAddress.CreatedBy = PortalContext.CurrentUser.UserId;
                employeeViewModel.EmployeePermanentAddress.IsActive = true;

                if (_employeeRepository.SaveEmployeeInfo(employeeViewModel) > 0)
                    savedEmployee = employeeIGuid;
            }
            catch (Exception)
            {
                savedEmployee = Guid.Parse("00000000-0000-0000-0000-000000000000");
            }
            return savedEmployee;
        }

        public Employee GetEmployeeById(Guid employeeIdGuid)
        {

            Employee employee;

            try
            {
                employee = _employeeRepository.GetEmployeeById(employeeIdGuid);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return employee;
        }

        public EmployeePresentAddress GetEmployeePresentAddressById(Guid employeeIdGuid)
        {

            EmployeePresentAddress employee = null;

            try
            {
                employee = _employeePresentAddressRepository.GetEmployeePresentAddressById(employeeIdGuid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return employee;
        }

        public EmployeePermanentAddress GetEmployeePermanentAddressById(Guid employeeIdGuid)
        {

            EmployeePermanentAddress employee = null;

            try
            {
                employee = _employeePermanentAddressRepository.GetEmployeePermanentAddressById(employeeIdGuid);
            }
            catch (Exception ex)
            {
                employee = null;
            }

            return employee;
        }

        public Guid SaveEmployeeMandatoryInfo(EmployeeMandatoryInfoCustomModel employeeMandatoryInfoCustomModel)
        {
            var savedEmployee = Guid.Parse("00000000-0000-0000-0000-000000000000");

            try
            {
                Guid employeeIdGuid = employeeMandatoryInfoCustomModel.Employee.EmployeeId;

                employeeMandatoryInfoCustomModel.Employee.EmployeeId = employeeIdGuid;
                employeeMandatoryInfoCustomModel.Employee.CreatedDate = DateTime.Now;
                employeeMandatoryInfoCustomModel.Employee.CreatedBy = PortalContext.CurrentUser.UserId;
                employeeMandatoryInfoCustomModel.Employee.Status = (int) StatusValue.Active;
                employeeMandatoryInfoCustomModel.Employee.IsActive = true;

                employeeMandatoryInfoCustomModel.EmployeeCompanyInfo.EmployeeId = employeeIdGuid;
                employeeMandatoryInfoCustomModel.EmployeeCompanyInfo.BranchUnitDepartmentId = employeeMandatoryInfoCustomModel.EmployeeBranchUnitDepartmentId;
                employeeMandatoryInfoCustomModel.EmployeeCompanyInfo.DesignationId = employeeMandatoryInfoCustomModel.EmployeeDesignationId;
                employeeMandatoryInfoCustomModel.IsEligibleForOvertime = employeeMandatoryInfoCustomModel.IsEligibleForOvertime;
                employeeMandatoryInfoCustomModel.EmployeeCompanyInfo.FromDate = employeeMandatoryInfoCustomModel.Employee.JoiningDate;
                employeeMandatoryInfoCustomModel.EmployeeCompanyInfo.CreatedDate = DateTime.Now;
                employeeMandatoryInfoCustomModel.EmployeeCompanyInfo.CreatedBy = PortalContext.CurrentUser.UserId;
                employeeMandatoryInfoCustomModel.EmployeeCompanyInfo.IsActive = true;

                employeeMandatoryInfoCustomModel.EmployeePresentAddress.EmployeeId = employeeIdGuid;
                employeeMandatoryInfoCustomModel.EmployeePresentAddress.CreatedDate = DateTime.Now;
                employeeMandatoryInfoCustomModel.EmployeePresentAddress.CreatedBy = PortalContext.CurrentUser.UserId;
                employeeMandatoryInfoCustomModel.EmployeePresentAddress.Status = (int) StatusValue.Active;
                employeeMandatoryInfoCustomModel.EmployeePresentAddress.IsActive = true;


                if (_employeeRepository.SaveEmployeeMandatoryInfo(employeeMandatoryInfoCustomModel) > 0)
                    savedEmployee = employeeIdGuid;
            }
            catch (Exception exception)
            {
                savedEmployee = Guid.Parse("00000000-0000-0000-0000-000000000000");
            }
            return savedEmployee;
        }

        public int EditEmployeeMandatoryInfo(EmployeeMandatoryInfoCustomModel employeeMandatoryInfoCustomModel)
        {

            var editedEmployeeMandatoryInfo = 0;

            try
            {
                using (var transaction = new TransactionScope())
                {
                    var employee = _employeeRepository.GetEmployeeById(employeeMandatoryInfoCustomModel.Employee.EmployeeId);

                    if (employeeMandatoryInfoCustomModel.Employee.PhotographPath != null)
                        employee.PhotographPath = employeeMandatoryInfoCustomModel.Employee.PhotographPath;

                    employee.EmployeeId = employeeMandatoryInfoCustomModel.Employee.EmployeeId;
                    employee.EmployeeCardId = employeeMandatoryInfoCustomModel.Employee.EmployeeCardId.Trim().Replace(" ", "");
                    employee.Name = employeeMandatoryInfoCustomModel.Employee.Name;
                    employee.NameInBengali = employeeMandatoryInfoCustomModel.Employee.NameInBengali;
                    employee.MothersName = employeeMandatoryInfoCustomModel.Employee.MothersName;
                    employee.MothersNameInBengali = employeeMandatoryInfoCustomModel.Employee.MothersNameInBengali;
                    employee.FathersName = employeeMandatoryInfoCustomModel.Employee.FathersName;
                    employee.FathersNameInBengali = employeeMandatoryInfoCustomModel.Employee.FathersNameInBengali;
                    employee.ReligionId = employeeMandatoryInfoCustomModel.Employee.ReligionId;
                    employee.DateOfBirth = employeeMandatoryInfoCustomModel.Employee.DateOfBirth;
                    employee.GenderId = employeeMandatoryInfoCustomModel.Employee.GenderId;
                    employee.JoiningDate = employeeMandatoryInfoCustomModel.Employee.JoiningDate;
                    employee.ConfirmationDate = employeeMandatoryInfoCustomModel.Employee.ConfirmationDate;
                    employee.EditedDate = DateTime.Now;
                    employee.EditedBy = PortalContext.CurrentUser.UserId;
                    employee.IsActive = true;
                    var editedEmployee = _employeeRepository.Edit(employee);

                    var employeePresentAddress = _employeePresentAddressRepository.GetEmployeePresentAddressById(employeeMandatoryInfoCustomModel.EmployeePresentAddress.EmployeeId);
                    employeePresentAddress.EmployeeId = employeeMandatoryInfoCustomModel.EmployeePresentAddress.EmployeeId;
                    employeePresentAddress.MobilePhone = employeeMandatoryInfoCustomModel.EmployeePresentAddress.MobilePhone;
                    employeePresentAddress.MailingAddress = employeeMandatoryInfoCustomModel.EmployeePresentAddress.MailingAddress;
                    employeePresentAddress.MailingAddressInBengali = employeeMandatoryInfoCustomModel.EmployeePresentAddress.MailingAddressInBengali;
                    employeePresentAddress.EditedDate = DateTime.Now;
                    employeePresentAddress.EditedBy = PortalContext.CurrentUser.UserId;
                    employeePresentAddress.Status = (int) StatusValue.Active;
                    employeePresentAddress.IsActive = true;
                    var editedEmployeePresentAddress = _employeePresentAddressRepository.Edit(employeePresentAddress);

                    if (editedEmployee > 0 && editedEmployeePresentAddress > 0)
                        editedEmployeeMandatoryInfo = 1;

                    transaction.Complete();
                }
            }
            catch (Exception)
            {
                editedEmployeeMandatoryInfo = 0;
            }

            return editedEmployeeMandatoryInfo;
        }

        public int EditEmployeePhoto(EmployeeMandatoryInfoViewModel employeeMandatoryInfoViewModel)
        {
            var editedEmployee = 0;
            try
            {
                var employee = _employeeRepository.GetEmployeeById(employeeMandatoryInfoViewModel.Employee.EmployeeId);
                employee.EmployeeId = employeeMandatoryInfoViewModel.Employee.EmployeeId;
                employee.PhotographPath = employeeMandatoryInfoViewModel.Employee.PhotographPath;
                editedEmployee = _employeeRepository.Edit(employee);
            }
            catch (Exception)
            {
                editedEmployee = 0;
            }

            return editedEmployee;
        }

        public List<EmployeeDesignation> GetDesignationByGradeId(int id)
        {
            List<EmployeeDesignation> employeeDesignations = null;
            try
            {
                employeeDesignations = _employeeDesignationRepository.Filter(x => x.GradeId == id).ToList();
            }
            catch (Exception)
            {

                employeeDesignations = null;
            }
            return employeeDesignations;
        }

        public List<EmployeeGrade> GetGetGradeByGradeId(int id)
        {
            List<EmployeeGrade> employeeGrades = null;
            try
            {
                employeeGrades = _employeeGradeRepository.Filter(x => x.EmployeeTypeId == id).ToList();
            }
            catch (Exception)
            {

                employeeGrades = null;
            }
            return employeeGrades;
        }

        public int EditEmployeePersonalInfo(Employee employee)
        {
            var editedPersonalInfo = 0;
            try
            {
                var employeeInfo = _employeeRepository.GetEmployeeById(employee.EmployeeId);
                employeeInfo.SpousesName = employee.SpousesName;
                employeeInfo.SpousesNameInBengali = employee.SpousesNameInBengali;
                employeeInfo.MaritalStateId = employee.MaritalStateId;
                employeeInfo.MariageAnniversary = employee.MariageAnniversary;
                employeeInfo.Nationality = employee.Nationality;
                employeeInfo.NationalityInBengali = employee.NationalityInBengali;
                employeeInfo.NationalIdNo = employee.NationalIdNo;
                employeeInfo.BirthRegistrationNo = employee.BirthRegistrationNo;
                employeeInfo.TaxIdentificationNo = employee.TaxIdentificationNo;
                employeeInfo.DrivingLicenseNo = employee.DrivingLicenseNo;
                employeeInfo.PassportNo = employee.PassportNo;
                employeeInfo.BloodGroupId = employee.BloodGroupId;
                employeeInfo.EditedDate = DateTime.Now;
                employeeInfo.EditedBy = PortalContext.CurrentUser.UserId;
                editedPersonalInfo = _employeeRepository.Edit(employeeInfo);
            }
            catch (Exception ex)
            {
                editedPersonalInfo = 0;
            }

            return editedPersonalInfo;
        }

        public int EditEmployeeQuitInfo(Employee employee)
        {
            var editedQuitInfo = 0;
            try
            {
                var employeeInfo = _employeeRepository.GetEmployeeById(employee.EmployeeId);

                if (employee.QuitTypeId == null)
                {
                    employeeInfo.Status = (int) StatusValue.Active;
                    employeeInfo.QuitDate = null;
                }
                else
                {
                    employeeInfo.Status = (int) StatusValue.InActive;
                    employeeInfo.QuitDate = employee.QuitDate;
                    employeeInfo.QuitTypeId = employee.QuitTypeId;
                }

                employeeInfo.EditedDate = DateTime.Now;
                employeeInfo.EditedBy = PortalContext.CurrentUser.UserId;
                editedQuitInfo = _employeeRepository.Edit(employeeInfo);
            }
            catch (Exception ex)
            {
                editedQuitInfo = 0;
            }

            return editedQuitInfo;
        }

        public int EditEmployeeDepartmentalInfo(EmployeeCompanyInfo employeeCompanyInfo)
        {
            var editedDepartmentalInfo = 0;
            try
            {
                editedDepartmentalInfo = _employeeCompanyInfoRepository.Edit(employeeCompanyInfo);
            }
            catch (Exception ex)
            {
                editedDepartmentalInfo = 0;
            }

            return editedDepartmentalInfo;
        }

        public int EditEmployeeAddressInfo(EmployeeAddressViewModel employeeAddressViewModel)
        {
            var editedEmployeeAddressInfo = 0;

            using (var transaction = new TransactionScope())
            {
                try
                {
                    var editedPresentAddress = 0;

                    var employeePresentAddress = _employeePresentAddressRepository.GetEmployeePresentAddressById(employeeAddressViewModel.EmployeePresentAddress.EmployeeId);

                    if (employeePresentAddress != null)
                    {
                        if (employeePresentAddress.EmployeeId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                        {
                            employeeAddressViewModel.EmployeePresentAddress.Id = employeePresentAddress.Id;
                            employeeAddressViewModel.EmployeePresentAddress.EmployeeId =
                                employeePresentAddress.EmployeeId;
                            employeeAddressViewModel.EmployeePresentAddress.MailingAddress =
                                employeePresentAddress.MailingAddress;
                            employeeAddressViewModel.EmployeePresentAddress.DistrictId =
                                employeePresentAddress.DistrictId;
                            employeeAddressViewModel.EmployeePresentAddress.PoliceStationId =
                                employeePresentAddress.PoliceStationId;
                            employeeAddressViewModel.EmployeePresentAddress.PostOffice =
                                employeePresentAddress.PostOffice;
                            employeeAddressViewModel.EmployeePresentAddress.PostCode =
                                employeePresentAddress.PostCode;
                            employeeAddressViewModel.EmployeePresentAddress.MobilePhone =
                                employeePresentAddress.MobilePhone;
                            employeeAddressViewModel.EmployeePresentAddress.CreatedDate = employeePresentAddress.CreatedDate;
                            employeeAddressViewModel.EmployeePresentAddress.CreatedBy = employeePresentAddress.CreatedBy;
                            employeeAddressViewModel.EmployeePresentAddress.IsActive = employeePresentAddress.IsActive;
                            employeeAddressViewModel.EmployeePresentAddress.EditedDate = DateTime.Now;
                            employeeAddressViewModel.EmployeePresentAddress.EditedBy = PortalContext.CurrentUser.UserId;

                            editedPresentAddress = _employeePresentAddressRepository.Edit(employeeAddressViewModel.EmployeePresentAddress);
                        }
                    }
                    else
                    {
                        editedPresentAddress = 0;
                    }

                    var employeePermanentAddress = _employeePermanentAddressRepository.GetEmployeePermanentAddressById(employeeAddressViewModel.EmployeePermanentAddress.EmployeeId);

                    if (employeePermanentAddress != null)
                    {
                        employeeAddressViewModel.EmployeePermanentAddress.Id = employeePermanentAddress.Id;
                        employeeAddressViewModel.EmployeePermanentAddress.EmployeeId = employeePermanentAddress.EmployeeId;
                        employeeAddressViewModel.EmployeePermanentAddress.EditedDate = DateTime.Now;
                        employeeAddressViewModel.EmployeePermanentAddress.EditedBy = PortalContext.CurrentUser.UserId;
                    }
                    else
                    {
                        employeeAddressViewModel.EmployeePermanentAddress.CreatedDate = DateTime.Now;
                        employeeAddressViewModel.EmployeePermanentAddress.CreatedBy = PortalContext.CurrentUser.UserId;
                    }


                    employeeAddressViewModel.EmployeePermanentAddress.IsActive = true;

                    var savedPermanentAddress = 0;

                    savedPermanentAddress = employeePermanentAddress == null ? _employeePermanentAddressRepository.SaveEmployeePermanentAddressInfo(employeeAddressViewModel.EmployeePermanentAddress) : _employeePermanentAddressRepository.Edit(employeeAddressViewModel.EmployeePermanentAddress);


                    if (editedPresentAddress > 0 && savedPermanentAddress > 0)
                        editedEmployeeAddressInfo = 1;

                    transaction.Complete();

                }
                catch (Exception ex)
                {
                    editedEmployeeAddressInfo = 0;
                }
            }
            return editedEmployeeAddressInfo;
        }

        public EmployeeMandatoryInfoCustomModel GetEmployeeMandatoryInfoByEmployeeId(Guid? employeeId, DateTime? effectiveDate)
        {
            EmployeeMandatoryInfoCustomModel employee = null;

            try
            {
                employee = _employeeRepository.GetEmployeeMandatoryInfoByEmployeeId(employeeId, effectiveDate);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return employee;
        }

        public int DeleteEmployee(Guid employeeId)
        {
            var deleted = 0;
            try
            {
                var employee = _employeeRepository.GetEmployeeById(employeeId);
                employee.Status = (int) StatusValue.InActive;
                employee.IsActive = false;
                employee.EditedDate = DateTime.Now;
                employee.EditedBy = PortalContext.CurrentUser.UserId;
                deleted = _employeeRepository.Edit(employee);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return deleted;
        }

        public IQueryable<EmployeeViewModel> GetEmployees(int? employeeDepartment, int? employeeTypeId, int? employeeDesignationId, string searchByEmployeeCardId,
            string searchByName, bool searchAll)
        {

            IQueryable<EmployeeViewModel> employeeList = null;

            try
            {
                employeeList = _employeeRepository.GetEmployees(employeeDepartment, employeeTypeId, employeeDesignationId,
                    searchByEmployeeCardId, searchByName, searchAll);
            }
            catch (Exception ex)
            {
                employeeList = null;
            }

            return employeeList;
        }

        public Employee GetEmployeeByCardId(string employeeCardId)
        {
            var employee = new Employee();

            try
            {
                employee = _employeeRepository.GetEmployeeByCardId(employeeCardId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);

            }
            return employee;
        }

        public Employee GetAnyEmployeeByCardId(string employeeCardId)
        {
            var employee = new Employee();

            try
            {
                employee = _employeeRepository.GetAnyEmployeeByCardId(employeeCardId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);

            }
            return employee;
        }

        public List<Employee> GetAallEmployeeByDepartmentId(int departmentId)
        {
            var employeeDetails = new List<Employee>();
            try
            {

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return employeeDetails;
        }

        public bool CheckExistingEmployeeCardNumber(Employee employee)
        {
            var isExist = false;
            try
            {

                isExist = _employeeRepository.CheckExistingEmployeeCardNumber(employee);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return isExist;
        }

        public List<EmployeeInfoCustomModel> GetAllEmployeeInfoByPaging(int startPage, int pageSize, SearchFieldModel searchFieldModel, EmployeeInfoCustomModel model, out int totalRecords)
        {
            List<EmployeeInfoCustomModel> employeeInfos = null;

            try
            {
                employeeInfos = _employeeRepository.GetAllEmployeeInfoByPaging(startPage, pageSize, searchFieldModel, model, out totalRecords);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return employeeInfos;
        }

        public int SaveEmployeeSalary(EmployeeSalary aEmployeeSalary)
        {
            var savedEmployeeSalary = 0;
            try
            {
                aEmployeeSalary.CreatedBy = PortalContext.CurrentUser.UserId;
                aEmployeeSalary.CreatedDate = DateTime.Now;
                aEmployeeSalary.IsActive = true;
                savedEmployeeSalary = _employeeSalaryRepository.Save(aEmployeeSalary);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return savedEmployeeSalary;
        }

        public object GetEmployeesByCardIdAndName(string serachString)
        {
            return _employeeRepository.Filter(x => x.IsActive == true && (x.Name.Trim().Replace(" ", String.Empty)
                .ToLower().Contains(serachString.Trim().Replace(" ", String.Empty).ToLower()))
                                                   || (x.EmployeeCardId.Trim().Replace(" ", String.Empty)
                                                       .ToLower().Contains(serachString.Trim().Replace(" ", String.Empty).ToLower()))).Take(10);
        }

        public string GetEmployeeNameByEmployeeId(Guid employeeId)
        {
            Employee employee = _employeeRepository.FindOne(x => x.EmployeeId == employeeId && x.IsActive == true);
            if (employee != null)
            {
                return employee.Name;
            }
            else
            {
                throw new ArgumentNullException("Employee not found by EmployeeId");
            }
        }

        public Guid GetEmployeeIdByEmployeeCardId(string employeeCardId)
        {
            Employee employee = _employeeRepository.FindOne(x => x.EmployeeCardId == employeeCardId && x.IsActive == true);
            return employee.EmployeeId;
        }

        public object GetEmployeesBySearchCharacter(string searchCharacter)
        {
            return _employeeRepository.Filter(x => x.IsActive == true && (x.Name.Trim().Replace(" ", String.Empty)
                .ToLower().Contains(searchCharacter.Trim().Replace(" ", String.Empty).ToLower()))).Take(10);
        }

        public List<SkillMatrixMachineType> GetAllMachineTypes()
        {
            return _employeeRepository.GetAllMachineTypes();
        }

        public List<SkillMatrixDetailProcess> GetSkillMatrixDetailByMachineTypeId(int? machineTypeId, Guid employeeId)
        {
            return _employeeRepository.GetSkillMatrixDetailByMachineTypeId(machineTypeId, employeeId);
        }

        public SkillMatrixPointTable GetAllSkillPoint(Guid employeeId)
        {
            return _employeeRepository.GetAllSkillPoint(employeeId);
        }

        public string SaveSkillMatrix(List<string> values, Guid employeeId, string employeeCardId)
        {
            return _employeeRepository.SaveSkillMatrix(values, employeeId, employeeCardId);
        }

        public int SaveSkillMatrixPoint(SkillMatrixPointTable model)
        {
            return _employeeRepository.SaveSkillMatrixPoint(model);
        }

        public string GetEmployeeImageUrlById(Guid? userId)
        {
            string imagePath= _employeeRepository.FindOne(x => x.EmployeeId == userId).PhotographPath;
            if (string.IsNullOrEmpty(imagePath))
            {
                imagePath = "/Content/images/employee.png";
            }
            return "~"+imagePath;
        }
    }
}
