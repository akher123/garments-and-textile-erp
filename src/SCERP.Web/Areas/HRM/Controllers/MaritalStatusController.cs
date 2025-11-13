using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Model;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class MaritalStatusController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "maritalstatus-1,maritalstatus-2,maritalstatus-3")]
        public ActionResult Index(MaritalStatusViewModel model)
        {
            try
            {
                ModelState.Clear();
                MaritalState maritalStatus = model;
                maritalStatus.Title = model.SearchKey;

                //if (model.IsSearch)
                //{
                //    model.IsSearch = false;
                //    return View(model);
                //}

                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }

                int totalRecords = 0;
                model.MaritalStatuses = MaritalStatusManager.GetAllMaritalStatusesByPaging(startPage, _pageSize, out totalRecords, maritalStatus) ?? new List<MaritalState>();
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }


            return View(model);
        }

        [AjaxAuthorize(Roles = "maritalstatus-2,maritalstatus-3")]
        public ActionResult Edit(MaritalStatusViewModel model)
        {
            ModelState.Clear();

            try
            {
                if (model.MaritalStateId > 0)
                {
                    var maritalStatus = MaritalStatusManager.GetMaritalStatusById(model.MaritalStateId);
                    model.Title = maritalStatus.Title;
                    model.TitleInBengali = maritalStatus.TitleInBengali;
                    ViewBag.Title = "Edit";
                }
                else
                {
                    ViewBag.Title = "Add";
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }


        [AjaxAuthorize(Roles = "maritalstatus-2,maritalstatus-3")]
        public ActionResult Save(MaritalStatusViewModel model)
        {
            var isExist = MaritalStatusManager.CheckExistingMaritalStatus(model);

            if (isExist)
            {
                return ErrorResult(model.Title + " " + "Marital Status already exist");
            }
            var religion = MaritalStatusManager.GetMaritalStatusById(model.MaritalStateId) ?? new MaritalState();
            religion.Title = model.Title;
            religion.TitleInBengali = model.TitleInBengali;


            var saveIndex = (model.MaritalStateId > 0) ? MaritalStatusManager.EditMaritalStatus(religion) : MaritalStatusManager.SaveMaritalStatus(religion);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");

        }


        [AjaxAuthorize(Roles = "maritalstatus-3")]
        public ActionResult Delete(int maritalStateId)
        {
            var deleted = 0;
            var maritalStatus = MaritalStatusManager.GetMaritalStatusById(maritalStateId) ?? new MaritalState();
            deleted = MaritalStatusManager.DeleteMaritalStatus(maritalStatus);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }


        [AjaxAuthorize(Roles = "maritalstatus-1,maritalstatus-2,maritalstatus-3")]
        public void GetExcel(MaritalStatusViewModel model)
        {
            var maritalStatuses = MaritalStatusManager.GetMaritalStatusBySearchKey(model.SearchKey);
            model.MaritalStatuses = maritalStatuses;

            const string fileName = "MaritalStatuses";
            var boundFields = new List<BoundField>
            {
                new BoundField(){HeaderText = @"Title", DataField = "Title"}
            };
            ReportConverter.CustomGridView(boundFields, model.MaritalStatuses, fileName);
        }


        [AjaxAuthorize(Roles = "maritalstatus-1,maritalstatus-2,maritalstatus-3")]
        public ActionResult Print(MaritalStatusViewModel model)
        {
            var maritalStatuses = MaritalStatusManager.GetMaritalStatusBySearchKey(model.SearchKey);
            model.MaritalStatuses = maritalStatuses;
            return View("_MaritalStatusPdfReport", model);
        }

    }

}
