using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Model;
using SCERP.Model.Planning;
using SCERP.Web.Areas.Planning.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Common;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.Process;
using System.Net.Mail;

namespace SCERP.Web.Areas.Planning.Controllers
{
    public class TnaController : BaseController
    {
      

        private readonly ITNAManager _tnaManager;
        private readonly ITNAHorizontalManager _tnaHorizontalManager;
        private readonly IOmBuyOrdStyleManager _omBuyOrdStyleManager;
        private readonly IOmBuyerManager _omBuyerManager;
        private readonly IEmailSendManager _emailSendManager;

        public TnaController(IOmBuyerManager omBuyerManager,ITNAManager tnaManager, ITNAHorizontalManager tnaHorizontalManager, IOmBuyOrdStyleManager omBuyOrdStyleManager, IEmailSendManager emailSendManager)
        {
            _omBuyerManager = omBuyerManager;
            this._tnaManager = tnaManager;
            this._tnaHorizontalManager = tnaHorizontalManager;
            this._omBuyOrdStyleManager = omBuyOrdStyleManager;
            this._emailSendManager = emailSendManager;
        }

        public ActionResult Index(TnaViewModel model)
        {
            try
            {
                ModelState.Clear();
                if (model.IsSearch)
                {
                    model.Activities = _tnaManager.GetAllActivities();
                    model.ResponsiblePersons = _tnaManager.GetAllResponsiblePersons();
                    model.Buyers = _omBuyerManager.GetAllBuyers();
                    model.BuyerRefId = model.BuyerRefId ?? "";
                    model.OrderNo = model.OrderNo ?? "";
                    model.OrderStyleRefId = model.OrderStyleRefId ?? "";
                    model.OrderList = _omBuyOrdStyleManager.GetOrderByBuyer(model.BuyerRefId);
                    model.BuyerOrderStyles = _omBuyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.OrderNo);
                    //PLAN_TNA planTna = model;
                    //planTna.OrderStyleRefId = model.OrderStyleRefId;

                    //if (string.IsNullOrEmpty(model.OrderStyleRefId))
                    //    planTna.OrderStyleRefId = "";
                    // planTna.or=
                    //planTna.ActivityId = model.ActivityId;
                    //planTna.FromDate = model.FromDate;
                    //planTna.ToDate = model.ToDate;
                    //planTna.ResponsiblePerson = model.ResponsiblePerson;
                    //planTna.CompId = PortalContext.CurrentUser.CompId;

                    var startPage = 0;
                    if (model.page.HasValue && model.page.Value > 0)
                    {
                        startPage = model.page.Value - 1;
                    }

                    var totalRecords = 0;
                    model.TnaReports = _tnaManager.GetAllTnaByPaging(model.PageIndex, AppConfig.PageSize, out totalRecords, model.BuyerRefId, model.OrderNo, model.OrderStyleRefId) ?? new List<PLAN_TNAReport>();
                    model.TotalRecords = totalRecords;
                }
                else
                {
                    model.IsSearch = true;
                }
              
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
                    model.OrderStyleRefId = tna.OrderStyleRefId;

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
                tna.CompId = PortalContext.CurrentUser.CompId;

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
            List<string> tags = _tnaManager.GetStyleNames();
            return this.Json(tags.Where(t => t.Substring(0, t.Length).Trim().ToLower().StartsWith(term.Trim().ToLower())), JsonRequestBehavior.AllowGet);
        }

        public bool SendMail()
        {
            try
            {
                string to = "";
                string body = "";
                string subject = "";
                bool? isMail = false;
                bool? isMailValid = false;
                bool result = false;

                var bodyBuilder = new StringBuilder();
                var subjectBuilder = new StringBuilder();

                var email = Email.Create(HostingEnvironment.MapPath("~/Content/Mail.xml"));
                List<PLAN_TNAReport> tna = _tnaManager.GetTnaMailData();

                foreach (var t in tna)
                {
                    subjectBuilder.AppendLine(email.Subject);
                    subjectBuilder.Replace("{ACTIVITY_NAME}", t.ActivityName);
                    subjectBuilder.Replace("{STYLE_NO}", t.StyleName);

                    bodyBuilder.AppendLine(email.Body);
                    bodyBuilder.Replace("{EMP_NAME}", t.ResponsiblePersonName);
                    bodyBuilder.Replace("{ACTIVITY_NAME}", t.ActivityName);
                    bodyBuilder.Replace("{STYLE_NO}", t.StyleName);

                    if (t.StartStatus == 0)
                    {
                        bodyBuilder.Replace("{STATUS}", "start");
                        bodyBuilder.Replace("{PLANNED_DATE}", t.PlannedStartDate);
                    }

                    if (t.EndStatus == 0)
                    {
                        bodyBuilder.Replace("{STATUS}", "end");
                        bodyBuilder.Replace("{PLANNED_DATE}", t.PlannedEndDate);
                    }

                    subject = subjectBuilder.Replace("\n", "").Replace("\r", "").ToString().Trim();
                    body = bodyBuilder.ToString();

                    to = t.EmailAddress;
                    isMail = true;

                    if (to.Contains(";"))
                    {
                        string[] toArray = to.Split(';');

                        foreach (string To in toArray)
                        {
                            isMailValid = IsValidEmail(To);

                            if (isMail.Value && isMailValid.Value)
                            {
                                result = _emailSendManager.SendEmail(To, subject, body, "", "");
                            }
                        }
                    }
                    else if (IsValidEmail(to))
                    {
                        result = _emailSendManager.SendEmail(to, subject, body, "", "");
                    }
                }
                return result;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return false;
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public ActionResult GetActivityById(int activityId)
        {
            string activityMode = _tnaManager.GetActivityById(activityId).ActivityMode;
            return Json(new {Success = true, activityMode = activityMode});
        }
    }
}