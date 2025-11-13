using System.Linq;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.Process.Salary;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using System;
using System.Collections.Generic;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeGradeSalaryPercentageManager : IEmployeeGradeSalaryPercentageManager
    {
        private readonly IEmployeeGradeSalaryPercentageRepository _employeeGradeSalaryPercentageRepository;
        public EmployeeGradeSalaryPercentageManager(SCERPDBContext context)
        {
            this._employeeGradeSalaryPercentageRepository=new EmployeeGradeSalaryPercentageRepository(context);
        }

        public List<EmployeeGradeSalaryPercentage> GetEmployeeGradeSalaryPercentages(int startPage, int pageSize, EmployeeGradeSalaryPercentage model,SearchFieldModel searchFieldModel,
            out int totalRecords)
        {
            List<EmployeeGradeSalaryPercentage> employeeGradeSalaryPercentages;
            try
            {
              employeeGradeSalaryPercentages=  _employeeGradeSalaryPercentageRepository.GetEmployeeTypeSalaryPercentages(startPage, pageSize, model,searchFieldModel, out totalRecords);
            }
            catch (Exception)
            {
                throw;
            }
            return employeeGradeSalaryPercentages;
        }

        public EmployeeGradeSalaryPercentage GetEmployeeGradeSalaryPercentageById(int employeeGradeSalaryPercentageId)
        {
            EmployeeGradeSalaryPercentage employeeGradeSalaryPercentage;
            try
            {
                employeeGradeSalaryPercentage =
                    _employeeGradeSalaryPercentageRepository.GetEmployeeGradeSalaryPercentageById(employeeGradeSalaryPercentageId);
            }
            catch (Exception)
            {
                
                throw;
            }
            return employeeGradeSalaryPercentage;
        }

        public int EditEmployeeGradeSalaryPercentage(EmployeeGradeSalaryPercentage model)
        {
            var editIndex = 0;
            try
            {
                var employeeGradeSalaryPercentage =
                    _employeeGradeSalaryPercentageRepository.FindOne(
                        x => x.EmployeeGradeSalaryPercentageId == model.EmployeeGradeSalaryPercentageId);
                employeeGradeSalaryPercentage.EmployeeGradeId = model.EmployeeGradeId;
                employeeGradeSalaryPercentage.Medical = model.Medical;
                employeeGradeSalaryPercentage.Conveyance = model.Conveyance;
                employeeGradeSalaryPercentage.Food = model.Food;
                employeeGradeSalaryPercentage.HouseRentPercentage = model.HouseRentPercentage;
                employeeGradeSalaryPercentage.BasicPercentageRate = model.BasicPercentageRate;
                employeeGradeSalaryPercentage.EditedBy = PortalContext.CurrentUser.UserId;
                employeeGradeSalaryPercentage.EditedDate = DateTime.Now;
                employeeGradeSalaryPercentage.Status = (Int16)StatusValue.Active;

                editIndex = _employeeGradeSalaryPercentageRepository.Edit(employeeGradeSalaryPercentage);
            }
            catch (Exception)
            {
                
                throw;
            }
            return editIndex;
        }

        public int SaveEmployeeGradeSalaryPercentage(EmployeeGradeSalaryPercentage model)
        {
            int saveIndex = 0;
            try
            {
                model.CreatedBy = PortalContext.CurrentUser.UserId;
                model.CreatedDate = DateTime.Now;
                model.Status = (Int16) StatusValue.Active;
                model.IsActive = true;
                saveIndex = _employeeGradeSalaryPercentageRepository.Save(model);
            }
            catch (Exception)
            {

                throw;
            }
            return saveIndex;
        }

        public bool IsEmployeeGradeExist(EmployeeGradeSalaryPercentage model)
        {
            bool isExist =
                _employeeGradeSalaryPercentageRepository.Exists(
                    x =>
                        x.EmployeeGradeId == model.EmployeeGradeId && x.IsActive && x.Status == (Int16)StatusValue.Active && x.EmployeeGradeSalaryPercentageId != model.EmployeeGradeSalaryPercentageId);
            return isExist;
        }

        public List<EmployeeGradeSalaryPercentage> GetEmployeeGradeSalaryPercentageBySearchKey(SearchFieldModel searchField)
        {
            List<EmployeeGradeSalaryPercentage> employeeGradeSalaryPercentages;
            try
            {
                employeeGradeSalaryPercentages = _employeeGradeSalaryPercentageRepository.GetEmployeeTypeSalaryPercentageBySearchKey(searchField);
            }
            catch (Exception)
            {
                throw;
            }
            return employeeGradeSalaryPercentages;
        }

        public int DeleteEmployeeGradeSalaryPercentage(int employeeGradeSalaryPercentageId)
        {
            var deleteIndex = 0;
            try
            {
                var employeeGradeSalaryPercentageObj = _employeeGradeSalaryPercentageRepository.FindOne(x => x.IsActive && x.EmployeeGradeSalaryPercentageId == employeeGradeSalaryPercentageId);
                employeeGradeSalaryPercentageObj.IsActive = false;
                employeeGradeSalaryPercentageObj.EditedDate = DateTime.Now;
                employeeGradeSalaryPercentageObj.EditedBy = PortalContext.CurrentUser.UserId;
                deleteIndex = _employeeGradeSalaryPercentageRepository.Edit(employeeGradeSalaryPercentageObj);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return deleteIndex;
        }

        public EmployeeGradeSalaryPercentage GetEmployeeGradeSalaryPercentangeByEmployeeGradeAndTypeId(int employeeGradeId, int employeeTypeId)
        {
            var employeeGradeSalaryPercentage=new EmployeeGradeSalaryPercentage();
            try
            {
                var employeeGradeSalaryPercentageObj =_employeeGradeSalaryPercentageRepository.GetEmployeeGradeSalaryPercentangeByEmployeeGradeAndTypeId(employeeGradeId, employeeTypeId)??new EmployeeGradeSalaryPercentage();
                   employeeGradeSalaryPercentage.EmployeeGradeId = employeeGradeSalaryPercentageObj.EmployeeGradeId;
                    employeeGradeSalaryPercentage.Medical = employeeGradeSalaryPercentageObj.Medical;
                    employeeGradeSalaryPercentage.Conveyance = employeeGradeSalaryPercentageObj.Conveyance;
                    employeeGradeSalaryPercentage.Food = employeeGradeSalaryPercentageObj.Food;
                
            }
            catch (Exception)
            {

                throw;
            }
            return employeeGradeSalaryPercentage;
        }

        public object GetEmployeeGradeSalaryPercentange(int employeeGradeId, int employeeTypeId, decimal grossSalary)
        {
            var employeeGradeSalaryPercentage=new object();
            try
            {
                var employeeGradeSalaryPercentageObj = _employeeGradeSalaryPercentageRepository.GetEmployeeGradeSalaryPercentangeByEmployeeGradeAndTypeId(employeeGradeId, employeeTypeId) ?? new EmployeeGradeSalaryPercentage();
                if (employeeGradeSalaryPercentageObj.EmployeeGradeSalaryPercentageId>0)
                {
                    var salaryPercentage = new SalaryPercentage(grossSalary, employeeGradeSalaryPercentageObj);
                    employeeGradeSalaryPercentage = new
                    {
                        BasicSalary = salaryPercentage.BasicSalary,
                        HouseRent = salaryPercentage.HouseRent,
                        TotalSalary = salaryPercentage.GetTotalSalary(),
                        Status = true
                    };
                }
                else
                {
                    employeeGradeSalaryPercentage = new
                    {
                        Status = false
                    };
                }

            }
            catch (Exception)
            {

                throw;
            }
            return employeeGradeSalaryPercentage;
        }
    }
}
