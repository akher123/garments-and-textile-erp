using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.BLL.Manager.PlanningManager;
using SCERP.Model.Planning;
using SCERP.Web.Areas.Planning.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Common;
using SCERP.BLL.IManager.IPlanningManager;

namespace SCERP.Web.Areas.Planning.Controllers
{
    public class ProcessTemplateController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        private readonly IProcessTemplateManager ProcessTemplateManager;
        public ProcessTemplateController(IProcessTemplateManager processTemplateManager)
        {
            this.ProcessTemplateManager = processTemplateManager;
        }

        public ActionResult Index(ProcessTemplateViewModel model)
        {
            ModelState.Clear();
        
            PLAN_ProcessTemplate processTemplate = model;
            processTemplate.StylerefId = model.StylerefId;

            var startPage = 0;
            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }

            var totalRecords = 0;
            model.ProcessTemplate = ProcessTemplateManager.GetAllProcessTemplateByPaging(startPage, _pageSize, out totalRecords, processTemplate) ?? new List<PLAN_ProcessTemplate>();
            model.TotalRecords = totalRecords;

            return View(model);
        }

        public ActionResult Edit(ProcessTemplateViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Styles = ProcessTemplateManager.GetAllStyles();
                model.Processes = ProcessTemplateManager.GetAllProcesses();
                model.ResponsiblePersons = ProcessTemplateManager.GetAllResponsiblePersons();

                if (model.Id > 0)
                {
                    var processTemplate = ProcessTemplateManager.GetProcessTemplateById(model.Id);

                    model.LeadTime = processTemplate.LeadTime;
                    model.PlannedStartDate = processTemplate.PlannedStartDate;
                    model.PlannedEndDate = processTemplate.PlannedEndDate;
                    model.ActualStartDate = processTemplate.ActualStartDate;
                    model.ActrualEndDate = processTemplate.ActrualEndDate;
                    model.NotifyBeforeDays = processTemplate.NotifyBeforeDays;
                    model.Remarks = processTemplate.Remarks;
                    model.ProcessId = processTemplate.ProcessId;
                    model.ResponsiblePerson = processTemplate.ResponsiblePerson;

                    ViewBag.Title = "Edit ProcessTemplate";
                }
                else
                {
                    ViewBag.Title = "Add ProcessTemplate";
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult Save(ProcessTemplateViewModel model)
        {
            var processTemplate = ProcessTemplateManager.GetProcessTemplateById(model.Id) ?? new PLAN_ProcessTemplate();

            processTemplate.StylerefId = model.StylerefId;
            processTemplate.ProcessId = model.ProcessId;
            processTemplate.LeadTime = model.LeadTime;
            processTemplate.ActualStartDate = model.ActualStartDate;
            processTemplate.ActrualEndDate = model.ActrualEndDate;
            processTemplate.PlannedStartDate = model.PlannedStartDate;
            processTemplate.PlannedEndDate = model.PlannedEndDate;
            processTemplate.NotifyBeforeDays = model.NotifyBeforeDays;
            processTemplate.Remarks = model.Remarks;
            processTemplate.ResponsiblePerson = model.ResponsiblePerson;

            var saveIndex = (model.Id > 0) ? ProcessTemplateManager.EditProcessTemplate(processTemplate) : ProcessTemplateManager.SaveProcessTemplate(processTemplate);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var plan = ProcessTemplateManager.GetProcessTemplateById(id) ?? new PLAN_ProcessTemplate();
            deleted = ProcessTemplateManager.DeleteProcessTemplate(plan);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }
    }
}