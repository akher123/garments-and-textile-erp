using System;
using System.Collections.Generic;
using System.Transactions;
using System.Web.Mvc;
using SCERP.BLL.Manager.PayrollManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Payroll.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Payroll.Controllers
{
    public class OvertimeSettingsController : BasePayrollController
    {

        public const int PageSize = 10;

        [AjaxAuthorize(Roles = "overtime-1,overtime-2,overtime-3")]
        public ActionResult Index(int? page, string sort, OvertimeSettingsViewModel model)
        {
            var startPage = 0;
            int totalRecords = 0;
            if (page.HasValue && page.Value > 0)
            {
                startPage = page.Value - 1;
            }
            model.OvertimeSettings = OvertimeSettingsManager.GetAllOvertimeSettings(startPage, PageSize, out totalRecords, model) ?? new List<OvertimeSettings>();
            model.TotalRecords = totalRecords;
            return View(model);
        }

        [AjaxAuthorize(Roles = "overtime-2,overtime-3")]
        public ActionResult Edit(OvertimeSettingsViewModel model)
        {
            ModelState.Clear();
            if (model.Id > 0)
            {
                var overtimeSetting = OvertimeSettingsManager.GetAllOvertimeById(model.Id) ?? new OvertimeSettings();
                model.OvertimeHours = overtimeSetting.OvertimeHours;
                model.OvertimeRate = overtimeSetting.OvertimeRate;
                model.ToDate = overtimeSetting.ToDate;
                model.FromDate = overtimeSetting.FromDate;
                model.CreatedBy = overtimeSetting.CreatedBy;
                model.CreatedDate = overtimeSetting.CreatedDate;
                model.Id = overtimeSetting.Id;
                model.IsActive = overtimeSetting.IsActive;
               
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "overtime-2,overtime-3")]
        public ActionResult Save(OvertimeSettings model)
        {

            var overtimeSetting = OvertimeSettingsManager.GetAllOvertimeById(model.Id) ?? new OvertimeSettings();
            overtimeSetting.OvertimeHours = model.OvertimeHours;
            overtimeSetting.OvertimeRate = model.OvertimeRate;
            overtimeSetting.FromDate = model.FromDate;
            overtimeSetting.ToDate = model.ToDate;

            var saveIndex = 0;

            var isExist = OvertimeSettingsManager.IsExistOvertimeSettings(model);

            if (isExist)
            {
                return ErrorResult("Ovetime settings already exist from this date.");
            }


            if (model.Id > 0)
            {
                saveIndex = OvertimeSettingsManager.EditOvertime(overtimeSetting);
            }
            else
            {
                using (var transactionScope = new TransactionScope())
                {
                    var latestOvertimeSettingsInfo = OvertimeSettingsManager.GetLatestOvertimeSettingInfo();
                    if (latestOvertimeSettingsInfo != null)
                    {
                        if (latestOvertimeSettingsInfo.FromDate > overtimeSetting.FromDate)
                            return ErrorResult("Invalid date!");

                        if (overtimeSetting.FromDate != null)
                            latestOvertimeSettingsInfo.ToDate = overtimeSetting.FromDate.Value.AddDays(-1);

                        OvertimeSettingsManager.UpdateLatestOvertimeSettingInfoDate(latestOvertimeSettingsInfo);

                    }

                    saveIndex = OvertimeSettingsManager.SaveOvertime(overtimeSetting);
                    transactionScope.Complete();
                }
            }

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "overtime-3")]
        public ActionResult Delete(int id)
        {
            int deleteOvertimeSettings = 0;
            deleteOvertimeSettings = OvertimeSettingsManager.DeleteOvertimeSettings(id);
            return deleteOvertimeSettings > 0 ? Reload() : ErrorResult("Failed to delete data");
        }
    }
}