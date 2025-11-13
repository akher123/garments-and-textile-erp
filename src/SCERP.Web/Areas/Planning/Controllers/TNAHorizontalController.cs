using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using SCERP.Model.Planning;
using SCERP.Web.Areas.Planning.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Common;
using SCERP.BLL.IManager.IPlanningManager;

namespace SCERP.Web.Areas.Planning.Controllers
{
    public class TNAHorizontalController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        private readonly ITNAManager TnaManager;
        private readonly ITNAHorizontalManager TnaHorizontalManager;

        public TNAHorizontalController(ITNAManager TnaManager, ITNAHorizontalManager TnaHorizontalManager)
        {
            this.TnaManager = TnaManager;
            this.TnaHorizontalManager = TnaHorizontalManager;
        }

        public ActionResult Index(TNAHorizontalViewModel model)
        {

            try
            {
                ModelState.Clear();

                var seasonNameList = TnaManager.GetAllSeasons();
                var buyerList = TnaManager.GetAllBuyers();
                var merchandiserList = TnaManager.GetAllMerchandiser();

                ViewBag.SearchBySeasonId = new SelectList(seasonNameList, "SeasonRefId", "SeasonName");
                ViewBag.SearchByBuyerId = new SelectList(buyerList, "BuyerRefId", "BuyerName");
                ViewBag.SearchByMerchandiserId = new SelectList(merchandiserList, "EmpId", "EmpName");

                PLAN_TNAHorizontal tna = model;

                tna.SeasonRefId = model.SearchBySeasonId;
                tna.BuyerName = model.SearchByBuyerId.ToString(CultureInfo.InvariantCulture);
                tna.MerchandiserName = model.SearchByMerchandiserId.ToString(CultureInfo.InvariantCulture);
                tna.CompId = PortalContext.CurrentUser.CompId;
                if (model.StyleName != null) tna.OrderStyleRefId = TnaManager.GetOrderStyleRefIdByStyleName(model.StyleName);

                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }

                var totalRecords = 0;
                model.tnaHorizon = TnaHorizontalManager.GetAllTnaHorizontalByPaging(startPage, _pageSize, out totalRecords, tna) ?? new List<PLAN_TNAHorizontal>();

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
            try
            {
                ModelState.Clear();

                model.Styles = TnaManager.GetAllStyles();
                model.Activities = TnaManager.GetAllActivities();
                model.ResponsiblePersons = TnaManager.GetAllResponsiblePersons();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult Save(TnaViewModel model)
        {
            var saveIndex = 0;

            try
            {
                var tna = new PLAN_TNA();

                tna.ActivityId = model.ActivityId;
                tna.LeadTime = model.LeadTime;
                tna.ActualStartDate = model.ActualStartDate;
                tna.ActrualEndDate = model.ActrualEndDate;
                tna.PlannedStartDate = model.PlannedStartDate;
                tna.PlannedEndDate = model.PlannedEndDate;
                tna.NotifyBeforeDays = model.NotifyBeforeDays;
                tna.Remarks = model.Remarks;
                tna.ResponsiblePerson = model.ResponsiblePerson;
                tna.OrderStyleRefId = model.OrderStyleRefId;

                tna.CompId = PortalContext.CurrentUser.CompId;
                tna.CreatedBy = PortalContext.CurrentUser.UserId;
                tna.EditedBy = PortalContext.CurrentUser.UserId;
                tna.CreatedDate = DateTime.Now;
                tna.EditedDate = DateTime.Now;
                tna.IsActive = true;

                PLAN_TNA test = TnaManager.GetTnaByRefId(model.OrderStyleRefId, model.ActivityId);

                if (test != null)
                {
                    tna.Id = test.Id;
                }

                saveIndex = TnaHorizontalManager.SaveTnaHorizontal(tna);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        public ActionResult GetActivityById(int? Id, string RefId)
        {
            PLAN_TNA plan = TnaManager.GetTnaByRefId(RefId, Id);
            PLAN_Activity activity = TnaManager.GetActivityById(Id.Value);

            int? leadTime = null;
            var planStartDate = "";
            var planEndDate = "";
            var actualStartDate = "";
            var actualEndDate = "";
            Guid? responsiblePerson = null;
            int? notifyBeforeDays = null;
            var remarks = "";
            var activityMode = "";

            if (plan != null)
            {
                if (plan.LeadTime != null)
                    leadTime = plan.LeadTime;

                if (plan.PlannedStartDate != null)
                    planStartDate = plan.PlannedStartDate.Value.ToString("dd/MM/yyyy");

                if (plan.PlannedEndDate != null)
                    planEndDate = plan.PlannedEndDate.Value.ToString("dd/MM/yyyy");

                if (plan.ActualStartDate != null)
                    actualStartDate = plan.ActualStartDate.Value.ToString("dd/MM/yyyy");

                if (plan.ActrualEndDate != null)
                    actualEndDate = plan.ActrualEndDate.Value.ToString("dd/MM/yyyy");

                if (plan.ResponsiblePerson != null)
                    responsiblePerson = plan.ResponsiblePerson;

                if (plan.NotifyBeforeDays != null)
                    notifyBeforeDays = plan.NotifyBeforeDays;

                if (plan.Remarks != null)
                    remarks = plan.Remarks;
            }

            activityMode = activity.ActivityMode;

            return Json(new { Success = true, leadTime = leadTime, planStartDate = planStartDate, planEndDate = planEndDate, actualStartDate = actualStartDate, actualEndDate = actualEndDate, responsiblePerson = responsiblePerson, notifyBeforeDays = notifyBeforeDays, remarks = remarks, activityMode = activityMode });
        }
    }
}