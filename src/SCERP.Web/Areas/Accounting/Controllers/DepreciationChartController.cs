using System;
using System.Web.Mvc;
using SCERP.BLL.Manager.AccountingManager;
using SCERP.Model;
using SCERP.Web.Areas.Accounting.Models.ViewModels;

namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class DepreciationChartController : BaseAccountingController
    {
        public const int PageSize = 10;
        private Guid employeeGuidId = Guid.Parse("7bca454d-187a-4885-9c65-07e4ef23ee8c");
                
        public ActionResult Index(int? page, string sort, DepreciationChartViewModel model)
        {
            var startPage = 0;

            if (page.HasValue && page.Value > 0)
            {
                startPage = page.Value - 1;
            }

            model.DepreciationCharts = DepreciationChartManager.GetAllDepreciationCharts(startPage, PageSize, sort);
            model.TotalRecords = model.DepreciationCharts.Count;
            return View(model);
        }
   
        public ActionResult Edit(Acc_DepreciationChart model)
        {
            ModelState.Clear();

            if (model.Id != 0)
            {
                var sectorManager = DepreciationChartManager.GetDepreciationChartById(model.Id);

                model.ControlCode = sectorManager.ControlCode;
                model.ControlName = sectorManager.ControlName;
                model.DepreciationRate = sectorManager.DepreciationRate;               
            }

            ViewBag.ControlCode = new SelectList(DepreciationChartManager.GetAllControlAccounts(), "ControlCode", "ControlName", model.ControlCode);

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }

        public ActionResult Save(DepreciationChartViewModel model)
        {
            if (Session["EmployeeGuid"] != null)
            {
                employeeGuidId = (Guid)Session["EmployeeGuid"];
            }

            var DepreciationChart = DepreciationChartManager.GetDepreciationChartById(model.Id) ?? new Acc_DepreciationChart();

            DepreciationChart.ControlCode = model.ControlCode;
            DepreciationChart.ControlName = DepreciationChartManager.GetControlName(model.ControlCode.Value);
            DepreciationChart.DepreciationRate = model.DepreciationRate;          

            if (model.Id > 0)
            {
                DepreciationChart.EDT = DateTime.Now;
                DepreciationChart.EditedBy = employeeGuidId;
            }
            else
            {
                DepreciationChart.CDT = DateTime.Now;
                DepreciationChart.CreatedBy = employeeGuidId;
            }

            string message = "";

            var i = DepreciationChartManager.SaveDepreciationChart(DepreciationChart);

            if (i == 0)
                message = "Database error has occured !";

            if (i == 2)
                message = "Duplicate Control Code Exists!";
       
            if (i == 1)
            {
                return Reload();
            }
            return CreateJsonResult(new { Success = false, Reload = true, Message = message });
        }

        public ActionResult Delete(int id)
        {
            var DepreciationChart = DepreciationChartManager.GetDepreciationChartById(id) ?? new Acc_DepreciationChart();
            DepreciationChartManager.DeleteDepreciationChart(DepreciationChart);
            return Reload();
        }
    }
}
