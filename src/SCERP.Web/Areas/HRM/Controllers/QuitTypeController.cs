using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class QuitTypeController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "quittype-1,quittype-2,quittype-3")]
        public ActionResult Index(QuitTypeViewModel model)
        {
            try
            {
                ModelState.Clear();
                var totalRecords = 0;
                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }

                //if (!model.IsSearch)
                //{
                //    model.IsSearch = true;
                //    return View(model);
                //}
                model.Type = model.SearchByQuitType;
                model.QuitTypes = QuitTypeManager.GetAllQuitTypesByPaging(startPage, _pageSize, model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "quittype-2,quittype-3")]
        public ActionResult Edit(QuitTypeViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.QuitTypeId > 0)
                {
                    var quitType = QuitTypeManager.GetQuitTypeById(model.QuitTypeId);
                    model.QuitTypeId = quitType.QuitTypeId;
                    model.Type = quitType.Type;
                    model.TypeInBengali = quitType.TypeInBengali;
                    model.Description = quitType.Description;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "quittype-2,quittype-3")]
        public ActionResult Save(QuitType model)
        {
            var saveIndex = 0;

            try
            {
                saveIndex = model.QuitTypeId > 0 ? QuitTypeManager.EditQuitType(model) : QuitTypeManager.SaveQuitType(model);

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "quittype-3")]
        public ActionResult Delete(QuitType model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = QuitTypeManager.DeleteQuitType(model.QuitTypeId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");

        }

        public JsonResult CheckQuitTypeExist(QuitType model)
        {
            bool isExist = !QuitTypeManager.IsExistQuitType(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "quittype-1,quittype-2,quittype-3")]
        public void GetExcel(QuitTypeViewModel model)
        {
            try
            {

                model.QuitTypes = QuitTypeManager.GetAllQuitTypesBySearchKey(model);
                const string fileName = @"Unit List";
                var boundFields = new List<BoundField>
            {
               new BoundField(){HeaderText = @"QuitType",DataField = "Type"},
            
              new BoundField(){HeaderText = @"Description",DataField = "Description"},
   
            };
                ReportConverter.CustomGridView(boundFields, model.QuitTypes, fileName);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

        }

        [AjaxAuthorize(Roles = "quittype-1,quittype-2,quittype-3")]
        public ActionResult Print(QuitTypeViewModel model)
        {
            try
            {

                model.QuitTypes = QuitTypeManager.GetAllQuitTypesBySearchKey(model);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return PartialView("_QuitTypeReport", model);
        }
    }
}