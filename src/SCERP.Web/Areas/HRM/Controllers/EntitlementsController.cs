using System;
using System.Collections.Generic;

using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf.qrcode;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Model;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EntitlementsController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "benefit-1,benefit-2,benefit-3")]
        public ActionResult Index(EntitlementViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Title = model.SearchKey;
                var startPage = 0;

                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                //if (model.IsSearch)
                //{
                //    model.IsSearch = false;
                //    return View(model);
                //}
                var totalRecords = 0;
                model.Entitlements = EntitlementManager.GetAllEducationLevelsByPaging(startPage, _pageSize, out totalRecords, model) ?? new List<Entitlement>();
                model.TotalRecords = totalRecords;

            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return View(model);
        }


        [AjaxAuthorize(Roles = "benefit-2,benefit-3")]
        public ActionResult Edit(EntitlementViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.Id > 0)
                {
                    var entitlement = EntitlementManager.GetEntitlementById(model.Id);
                    model.Title = entitlement.Title;
                    model.TitleInBengali = entitlement.TitleInBengali;
                    model.Description = entitlement.Description;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }


        [AjaxAuthorize(Roles = "benefit-2,benefit-3")]
        public ActionResult Save(EntitlementViewModel model)
        {
            var saveIndex = 0;
            try
            {
                var isExist = EntitlementManager.IsEntitlementExist(model);
                switch (isExist)
                {
                    case false:
                        var entitlement = EntitlementManager.GetEntitlementById(model.Id) ?? new Entitlement();
                        entitlement.Title = model.Title;
                        entitlement.TitleInBengali = model.TitleInBengali;
                        entitlement.Description = model.Description;
                        saveIndex = (model.Id > 0) ? EntitlementManager.EditEntitlement(entitlement) : EntitlementManager.SaveEntitlement(entitlement);
                        break;
                    case true:
                        return ErrorResult(model.Title + " " + "entitlement already exist");

                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");

        }


        [AjaxAuthorize(Roles = "benefit-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;
            try
            {
                var entitlement = EntitlementManager.GetEntitlementById(id) ?? new Entitlement();
                deleted = EntitlementManager.DeleteEntitlement(entitlement);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }


        [AjaxAuthorize(Roles = "benefit-1,benefit-2,benefit-3")]
        public void GetExcel(EntitlementViewModel model)
        {
            try
            {
                model.Title = model.SearchKey;
                model.Entitlements = EntitlementManager.GetEntitlementBySearchKey(model);
                const string fileName = "Entitlement";
                var boundFields = new List<BoundField>
            {
                new BoundField(){HeaderText = @"Entitlement Name",DataField = "Title"},
                   new BoundField(){HeaderText = @"Description",DataField = "Description"},
          
            };
                ReportConverter.CustomGridView(boundFields, model.Entitlements, fileName);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

        }

        [AjaxAuthorize(Roles = "benefit-1,benefit-2,benefit-3")]
        public ActionResult Print(EntitlementViewModel model)
        {
            try
            {
                model.Title = model.SearchKey;
                model.Entitlements = EntitlementManager.GetEntitlementBySearchKey(model);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return PartialView("_EntitlementPdfReport", model);
        }
    }
}
