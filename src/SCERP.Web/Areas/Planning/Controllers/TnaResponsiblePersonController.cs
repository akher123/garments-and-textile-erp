using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Model.Planning;
using SCERP.Web.Areas.Planning.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Common;
using SCERP.BLL.IManager.IPlanningManager;

namespace SCERP.Web.Areas.Planning.Controllers
{
    public class TnaResponsiblePersonController : BaseController
    {
        private readonly int _pageSize = 50;
        private readonly ITNAManager _tnaManager;
        private readonly ITNAHorizontalManager _tnaHorizontalManager;
        private IOmBuyOrdStyleManager _omBuyOrdStyleManager;
        public TnaResponsiblePersonController(ITNAManager tnaManager, ITNAHorizontalManager tnaHorizontalManager, IOmBuyOrdStyleManager omBuyOrdStyleManager)
        {
            this._tnaManager = tnaManager;
            this._tnaHorizontalManager = tnaHorizontalManager;
            this._omBuyOrdStyleManager = omBuyOrdStyleManager;
        }

        public ActionResult Index(TnaViewModel model)
        {
            try
            {
                ModelState.Clear();

                model.Activities = _tnaManager.GetAllActivities();
                model.ResponsiblePersons = _tnaManager.GetAllResponsiblePersons();

                PLAN_TNA planTna = model;

                planTna.OrderStyleRefId = model.OrderStyleRefId;                
                planTna.ActivityId = model.ActivityId;
                planTna.FromDate = model.FromDate;
                planTna.ToDate = model.ToDate;
                planTna.ResponsiblePerson = model.ResponsiblePerson;
                if (string.IsNullOrEmpty(model.OrderStyleRefId))
                    planTna.OrderStyleRefId = "";

                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }

                var totalRecords = 0;
                model.TnaReports = _tnaManager.GetAllTnaResponsibleByPaging(startPage, _pageSize, out totalRecords, planTna) ?? new List<PLAN_TNAReport>();
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult Edit(TnaViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Styles = _tnaManager.GetAllStyles();
                model.Activities = _tnaManager.GetAllActivities();
                model.ResponsiblePersons = _tnaManager.GetAllResponsiblePersons();

                if (model.Id > 0)
                {
                    var tna = _tnaManager.GetTnaById(model.Id);

                    model.SearchString = _tnaManager.GetStyleNameByOrderStyleRefId(tna.OrderStyleRefId);
                    model.LeadTime = tna.LeadTime;
                    model.PlannedStartDate = tna.PlannedStartDate;
                    model.PlannedEndDate = tna.PlannedEndDate;
                    model.ActualStartDate = tna.ActualStartDate;
                    model.ActrualEndDate = tna.ActrualEndDate;
                    model.NotifyBeforeDays = tna.NotifyBeforeDays;
                    model.Remarks = tna.Remarks;
                    model.ActivityId = tna.ActivityId;
                    model.ResponsiblePerson = tna.ResponsiblePerson;

                    ViewBag.Title = "Edit TNA";
                }
                else
                {
                    ViewBag.Title = "Add TNA";
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult Save(TnaViewModel model)
        {
            int saveIndex = 0;

            try
            {
                PLAN_TNA tna;
                tna = _tnaManager.GetTnaById(model.Id) ?? new PLAN_TNA();

                if (tna.Id == 0)
                {
                    var refId = _tnaManager.GetOrderStyleRefIdByStyleName(model.SearchString);

                    if (model.ActivityId != null)
                    {
                        tna = _tnaManager.GetTnaBySearchKey(refId, model.ActivityId.Value).FirstOrDefault();
                    }
                }

                if (tna == null)
                    tna = new PLAN_TNA();

                tna.OrderStyleRefId = _tnaManager.GetOrderStyleRefIdByStyleName(model.SearchString);
                tna.ActivityId = model.ActivityId;
                tna.LeadTime = model.LeadTime;
                tna.NotifyBeforeDays = model.NotifyBeforeDays;
                tna.Remarks = model.Remarks;
                tna.ResponsiblePerson = model.ResponsiblePerson;

                var activity = new PLAN_Activity();

                if (model.ActivityId != null)
                {
                    activity = _tnaManager.GetActivityById(model.ActivityId.Value);
                }

                if (activity.StartField.Trim().Length > 0 && activity.EndField.Trim().Length > 0)
                {
                    tna.ActualStartDate = model.ActualStartDate;
                    tna.ActrualEndDate = model.ActrualEndDate;
                    tna.PlannedStartDate = model.PlannedStartDate;
                    tna.PlannedEndDate = model.PlannedEndDate;
                }
                else if (activity.StartField.Trim().Length > 0)
                {
                    tna.ActualStartDate = model.ActualStartDate;
                    tna.ActrualEndDate = null;
                    tna.PlannedStartDate = model.PlannedStartDate;
                    tna.PlannedEndDate = null;
                }
                else if (activity.EndField.Trim().Length > 0)
                {
                    tna.ActualStartDate = model.ActualStartDate;
                    tna.ActrualEndDate = null;
                    tna.PlannedStartDate = model.PlannedStartDate;
                    tna.PlannedEndDate = null;
                }

                saveIndex = (model.Id > 0) ? _tnaManager.EditTna(tna) : _tnaManager.SaveTna(tna);

                if (saveIndex > 0)
                    saveIndex = _tnaHorizontalManager.SaveTnaHorizontal(tna);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        public ActionResult Delete(int id)
        {
            var deleted = 0;
            try
            {
                var tna = _tnaManager.GetTnaById(id) ?? new PLAN_TNA();
                deleted = _tnaManager.DeleteTna(tna);
                if (deleted > 0)
                    deleted = _tnaHorizontalManager.DeleteTnaHorizontal(tna);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        public JsonResult StyleAutocomplite(string searchString)
        {
            var vOrdStyleList = _omBuyOrdStyleManager.StyleAutocomplite(searchString);
            return Json(vOrdStyleList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TagSearch(string term)
        {
            List<string> tags = _tnaManager.GetResponsibles();
            return this.Json(tags.Where(t => t.Substring(0, t.Length).Trim().ToLower().Contains(term.Trim().ToLower())), JsonRequestBehavior.AllowGet);
        }
    }
}