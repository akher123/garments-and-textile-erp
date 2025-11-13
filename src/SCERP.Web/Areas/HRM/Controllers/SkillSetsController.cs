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
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class SkillSetsController : BaseHrmController
    {

        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "skillset-1,skillset-2,skillset-3")]
        public ActionResult Index(SkillSetViewModel model)
        {
            ModelState.Clear();
            SkillSet skillSet = model;
            skillSet.Title = model.SearchKey;

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

            var totalRecords = 0;
            model.SkillSets = SkillSetManager.GetAllSkillSetsByPaging(startPage, _pageSize, out totalRecords, skillSet) ?? new List<SkillSet>();
            model.TotalRecords = totalRecords;

            return View(model);
        }


        [AjaxAuthorize(Roles = "skillset-2,skillset-3")]
        public ActionResult Edit(SkillSetViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.Id > 0)
                {
                    var skillSet = SkillSetManager.GetSkillSetById(model.Id);
                    model.Title = skillSet.Title;
                    model.TitleInBengali = skillSet.TitleInBengali;
                    model.Description = skillSet.Description;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }


        [AjaxAuthorize(Roles = "skillset-2,skillset-3")]
        public ActionResult Save(SkillSetViewModel model)
        {

            var isExist = SkillSetManager.IsExistSkillSets(model);
            var saveIndex = 0;
            switch (isExist)
            {
                case false:
                    var skillSet = SkillSetManager.GetSkillSetById(model.Id) ?? new SkillSet();
                    skillSet.Title = model.Title;
                    skillSet.TitleInBengali = model.TitleInBengali;
                    skillSet.Description = model.Description;
                    saveIndex = (model.Id > 0) ? SkillSetManager.EditSkillSet(skillSet) : SkillSetManager.SaveSkillSet(skillSet);
                    break;
                case true:
                    return ErrorResult(model.Title + " " + "Skill Set already exist");
            }


            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }


        [AjaxAuthorize(Roles = "skillset-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var skillSet = SkillSetManager.GetSkillSetById(id) ?? new SkillSet();
            deleted = SkillSetManager.DeleteSkillSet(skillSet);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        [AjaxAuthorize(Roles = "skillset-1,skillset-2,skillset-3")]
        public void GetExcel(SkillSetViewModel model)
        {

            try
            {

                model.SkillSets = SkillSetManager.GetSkillSetBySearchKey(model.SearchKey);
                const string fileName = "SkillSet";
                var boundFields = new List<BoundField>
            {
                new BoundField(){HeaderText = "Skill set",DataField = "Title"},
                 new BoundField(){HeaderText = "Description",DataField = "Description"},
            };

                ReportConverter.CustomGridView(boundFields, model.SkillSets, fileName);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

        }

        [AjaxAuthorize(Roles = "skillset-1,skillset-2,skillset-3")]
        public ActionResult Print(SkillSetViewModel model)
        {
            try
            {
                List<SkillSet> skillSets = SkillSetManager.GetSkillSetBySearchKey(model.SearchKey);
                model.SkillSets = skillSets;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View("_SkillSetsPdfReportViewer", model);
        }

    }
}
