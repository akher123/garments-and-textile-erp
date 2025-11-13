using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Model;
using SCERP.Model.HRMModel;
using SCERP.Web.Controllers;
using SCERP.Common;
using SCERP.Web.Areas.HRM.Models.ViewModels;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class MaternityLeaveController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;
        private readonly IMaternityLeaveManager _maternityLeaveManager;

        public MaternityLeaveController(IMaternityLeaveManager lcManager)
        {
            this._maternityLeaveManager = lcManager;
        }

        public ActionResult Index(MaternityLeaveViewModel model)
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

                var statusList = from StatusValue status in Enum.GetValues(typeof(StatusValue))
                                 select new { Id = (int)status, Name = status.ToString() };
                ViewBag.EmployeeStatus = new SelectList(statusList, "Id", "Name");

                string userName = PortalContext.CurrentUser.Name;
                model.SearchFieldModel.SearchByUserName = userName;

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
                int totalRecords = 0;

                model.maternityLeaveInfos = _maternityLeaveManager.GetMaternityLeaveInfoByPaging(startPage, _pageSize, out totalRecords, model, model.SearchFieldModel);

                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult Edit(MaternityLeaveViewModel model)
        {
            ModelState.Clear();

            HrmMaternityPayment maternity = _maternityLeaveManager.GetMaternityPaymentById(model.MaternityPaymentId);

            if (maternity != null)
            {
                Employee employee = _maternityLeaveManager.GetEmployeeCardIdByEmployeeId(maternity.EmployeeId);

                if (employee.Id > 0)
                    model.EmployeeCardId = employee.EmployeeCardId;

                model.LeaveDayStart = maternity.LeaveDayStart;
                model.LeaveDayEnd = maternity.LeaveDayEnd;
                model.FirstPaymentDate = maternity.FirstPaymentDate;
                model.FirstPaymentAmount = maternity.FirstPaymentAmount;
                model.SecondPaymentDate = maternity.SecondPaymentDate;
                model.SecondPaymentAmount = maternity.SecondPaymentAmount;
            }

            return View(model);
        }

        public ActionResult Save(MaternityLeaveViewModel model)
        {
            HrmMaternityPayment maternity = _maternityLeaveManager.GetMaternityPaymentById(model.MaternityPaymentId) ?? new HrmMaternityPayment();

            Employee employee = _maternityLeaveManager.GetEmployeeIdByCardId(model.EmployeeCardId);

            if (employee.Id > 0)
            {
                maternity.EmployeeId = employee.EmployeeId;
                maternity.LeaveDayStart = model.LeaveDayStart;
                maternity.LeaveDayEnd = model.LeaveDayEnd;
                maternity.FirstPaymentDate = model.FirstPaymentDate;
                maternity.FirstPaymentAmount = model.FirstPaymentAmount;
                maternity.SecondPaymentDate = model.SecondPaymentDate;
                maternity.SecondPaymentAmount = model.SecondPaymentAmount;
            }
            else
                return ErrorResult("Please Insert a valid Employee Id");

            var saveIndex = (model.MaternityPaymentId > 0) ? _maternityLeaveManager.EditMaternityPayment(maternity) : _maternityLeaveManager.SaveMaternityPayment(maternity);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }
    }
}