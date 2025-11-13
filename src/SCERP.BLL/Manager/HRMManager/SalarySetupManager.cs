using System;
using System.Transactions;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using System.Linq;
using SCERP.Model;
using System.Collections.Generic;
using SCERP.Common;


namespace SCERP.BLL.Manager.HRMManager
{
    public class SalarySetupManager : BaseManager, ISalarySetupManager
    {

        private readonly ISalarySetupRepository _salarySetupRepository = null;

        public SalarySetupManager(SCERPDBContext context)
        {
            _salarySetupRepository = new SalarySetupRepository(context);
        }

        public List<SalarySetup> GetAllSalarySetup(int page, int records, string sort, int? GradeId)
        {
            return _salarySetupRepository.GetAllSalarySetup(page, records, sort, GradeId);
        }

        public IQueryable<EmployeeType> GetEmployeeTypes()
        {
            return _salarySetupRepository.GetEmployeeTypes();
        }

        public List<EmployeeGrade> GetEmpGradeByEmpType(int employeeTypeId)
        {
            return _salarySetupRepository.GetEmpGradeByEmpType(employeeTypeId).ToList();
        }

        public SalarySetup GetSalarySetupById(int? id)
        {
            return _salarySetupRepository.GetSalarySetupById(id);
        }

        public int SaveSalarySetup(SalarySetup salary)
        {
            int saveIndex;

            try
            {
                using (var transactionScope = new TransactionScope())
                {

                    var salarySetup = new SalarySetup
                    {
                        EmployeeGradeId = salary.EmployeeGradeId,
                        GrossSalary = salary.GrossSalary,
                        BasicSalary = salary.BasicSalary,
                        HouseRent = salary.HouseRent,
                        MedicalAllowance = salary.MedicalAllowance,
                        Conveyance = salary.Conveyance,
                        EntertainmentAllowance = salary.EntertainmentAllowance,
                        FromDate = salary.FromDate,
                        ToDate = salary.ToDate,
                        Description = salary.Description,
                        FoodAllowance = salary.FoodAllowance,
                        CreatedDate = DateTime.Now,
                        CreatedBy = PortalContext.CurrentUser.UserId,
                        IsActive = true
                    };
                    saveIndex = _salarySetupRepository.Save(salarySetup);
                    transactionScope.Complete();
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return saveIndex;
        }

        public int DeleteSalarySetup(SalarySetup salary)
        {
            var deleteIndex = 0;
            try
            {
                var salarySetupObj = _salarySetupRepository.FindOne(x => x.IsActive == true && x.Id == salary.Id);
                salarySetupObj.IsActive = false;
                salarySetupObj.EditedDate = DateTime.Now;
                salarySetupObj.EditedBy = PortalContext.CurrentUser.UserId;
                deleteIndex = _salarySetupRepository.Edit(salarySetupObj);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return deleteIndex;

        }

        public List<SalarySetup> GetAllSalarySetupesByPaging(int startPage, int pageSize, out int totalRecords, SalarySetup SalarySetup)
        {
            var salarySetups = new List<SalarySetup>();
            try
            {
                salarySetups = _salarySetupRepository.GetAllSalarySetupByPaging(startPage, pageSize, SalarySetup, out totalRecords);
            }
            catch (Exception exception)
            {
                totalRecords = 0;

                Errorlog.WriteLog(exception);
            }

            return salarySetups;
        }


        public int EditSalarySetup(SalarySetup model)
        {
            int edit;
            try
            {
                var salary =
                    _salarySetupRepository.FindOne(
                        x => x.Id == model.Id
                            && x.IsActive);
                salary.EmployeeGradeId = model.EmployeeGradeId;
                salary.GrossSalary = model.GrossSalary;
                salary.BasicSalary = model.BasicSalary;
                salary.HouseRent = model.HouseRent;
                salary.MedicalAllowance = model.MedicalAllowance;
                salary.Conveyance = model.Conveyance;
                salary.EntertainmentAllowance = model.EntertainmentAllowance;
                salary.FromDate = model.FromDate;
                salary.ToDate = model.ToDate;
                salary.Description = model.Description;
                salary.FoodAllowance = model.FoodAllowance;
                salary.CreatedDate = DateTime.Now;
                salary.CreatedBy = PortalContext.CurrentUser.UserId;
                salary.IsActive = true;
                edit = _salarySetupRepository.Edit(salary);
            }
            catch (Exception)
            {

                throw;
            }
            return edit;
        }

        public List<SalarySetup> GetAllSalarySetupNewBySearchKey(SalarySetup salarySetup)
        {
            var salarySetups = new List<SalarySetup>();
            try
            {
                salarySetups = _salarySetupRepository.GetAllSalarySetupBySearchKey(salarySetup);
            }
            catch (Exception exception)
            {


                Errorlog.WriteLog(exception);
            }

            return salarySetups;
        }

        public SalarySetup GetLatestSalarySetupInfoByGrade(SalarySetup salarySetup)
        {
            SalarySetup _salarySetup = null;

            try
            {
                _salarySetup = _salarySetupRepository.GetLatestSalarySetupInfoByGrade(salarySetup);

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return _salarySetup;
        }

        public int UpdateSalarySetupInfoDate(SalarySetup salarySetup)
        {
            var updated = 0;
            try
            {
                salarySetup.EditedBy = PortalContext.CurrentUser.UserId;
                salarySetup.EditedDate = DateTime.Now;
                updated = _salarySetupRepository.Edit(salarySetup);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return updated;
        }

        public bool CheckMinimumSalary(EmployeeSalary employeeSalary, SalarySetup salarySetup)
        {
            if (employeeSalary.GrossSalary < salarySetup.GrossSalary)
                return false;

            if (employeeSalary.BasicSalary < salarySetup.BasicSalary)
                return false;

            if (employeeSalary.HouseRent < salarySetup.HouseRent)
                return false;

            if (employeeSalary.MedicalAllowance < salarySetup.MedicalAllowance)
                return false;

            if (employeeSalary.FoodAllowance < salarySetup.FoodAllowance)
                return false;

            if (employeeSalary.Conveyance < salarySetup.Conveyance)
                return false;

            if (employeeSalary.EntertainmentAllowance < salarySetup.EntertainmentAllowance)
                return false;

            return true;

        }

        public bool CheckSumOfAllSalary(EmployeeSalary employeeSalary)
        {
            try
            {
                var sumOfAllSalary = employeeSalary.BasicSalary +
                                     employeeSalary.HouseRent +
                                     employeeSalary.MedicalAllowance +
                                     (employeeSalary.FoodAllowance ?? 0.0M) +
                                     (employeeSalary.EntertainmentAllowance ?? 0.0M) +
                                     employeeSalary.Conveyance;
                return employeeSalary.GrossSalary == sumOfAllSalary;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

        }

        public SalarySetup GetSalarySetupByEmployeeGrade(int employeeGradeId, DateTime? effectiveDate)
        {
            SalarySetup salarySetup = null;

            try
            {
                salarySetup = _salarySetupRepository.GetSalarySetupByEmployeeGrade(employeeGradeId, effectiveDate);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return salarySetup;
        }


    }
}
