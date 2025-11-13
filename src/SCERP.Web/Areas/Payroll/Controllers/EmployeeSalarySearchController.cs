using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Payroll.Models.ViewModels;
using SCERP.Web.Controllers;
using System.Data.SqlClient;

namespace SCERP.Web.Areas.Payroll.Controllers
{
    public class EmployeeSalarySearchController : BasePayrollController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "salarysearch-1,salarysearch-2,salarysearch-3")]
        public ActionResult Index(EmployeeSalarySearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);

                if (model.IsSearch)
                {
                    model.IsSearch = false;
                    return View(model);
                }

                var startPage = 0;

                DateTime? fromDate = model.FromDateCustom;
                DateTime? toDate = model.ToDateCustom;

                SqlParameter fromD = new SqlParameter("@FromDate", fromDate);
                SqlParameter toD = new SqlParameter("@ToDate", toDate);

                object[] objs = new object[] {fromD, toD};

                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                int totalRecords = 0;
                //model.EmployeeSalaries = EmployeeSalaryProcessManager.GetSalaryByPaging(startPage, _pageSize, out totalRecords, model, model.SearchFieldModel, objs);
                model.EmployeeSalaries = null;
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }
    }
}
