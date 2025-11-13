using System;
using System.Collections.Generic;
using System.Transactions;
using System.Web.Helpers;
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
    public class StampAmountsController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "stampamount-1,stampamount-2,stampamount-3")]
        public ActionResult Index(StampAmountViewModel model)
        {
            ModelState.Clear();

            //if (model.IsSearch)
            //{
            //    model.IsSearch = false;
            //    return View(model);
            //}

            StampAmount stampAmount = model;
            if (model.SearchByFromDate != null)
                stampAmount.FromDate = model.SearchByFromDate.Value;

            stampAmount.ToDate = model.SearchByToDate;

            var startPage = 0;
            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }

            var totalRecords = 0;
            model.StampAmounts = StampAmountManager.GetAllStampAmountsByPaging(startPage, _pageSize, out totalRecords, stampAmount) ?? new List<StampAmount>();
            model.TotalRecords = totalRecords;

            return View(model);
        }

        [AjaxAuthorize(Roles = "stampamount-2,stampamount-3")]
        public ActionResult Edit(StampAmountViewModel model)
        {
            ModelState.Clear();

            try
            {
                if (model.StampAmountId > 0)
                {
                    var stampAmount = StampAmountManager.GetStampAmountById(model.StampAmountId);

                    model.Amount = stampAmount.Amount;
                    model.FromDate = stampAmount.FromDate;
                    model.ToDate = stampAmount.ToDate;
                    ViewBag.Title = "Edit Stamp Amount";
                }
                else
                {
                    ViewBag.Title = "Add Stamp Amount";
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "stampamount-2,stampamount-3")]
        public ActionResult Save(StampAmountViewModel model)
        {
            
            var stampAmount = StampAmountManager.GetStampAmountById(model.StampAmountId) ?? new StampAmount();

            stampAmount.Amount = model.Amount;
            stampAmount.FromDate = model.FromDate;
            stampAmount.ToDate = model.ToDate;

            var saveIndex = 0;

            if (model.StampAmountId > 0)
            {
                saveIndex = StampAmountManager.EditStampAmount(stampAmount);
            }
            else
            {
                var isExist = StampAmountManager.CheckExistingStampAmount(model);

                if (isExist)
                {
                    return ErrorResult("Stamp amount already exist from this date.");
                }


                using (var transactionScope = new TransactionScope())
                {
                    var latestStampAmountInfo = StampAmountManager.GetLatestStampAmountInfo();
                    if (latestStampAmountInfo != null)
                    {
                        if (latestStampAmountInfo.FromDate > stampAmount.FromDate)
                            return ErrorResult("Invalid date!");

                        if (stampAmount.FromDate != null)
                            latestStampAmountInfo.ToDate = stampAmount.FromDate.Value.AddDays(-1);

                        StampAmountManager.UpdateLatestStampInfoDate(latestStampAmountInfo);

                    }

                    saveIndex = StampAmountManager.SaveStampAmount(stampAmount);
                    transactionScope.Complete();
                }
            }

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "stampamount-3")]
        public ActionResult Delete(int StampAmountId)
        {
            var deleted = 0;
            var stampAmount = StampAmountManager.GetStampAmountById(StampAmountId) ?? new StampAmount();
            deleted = StampAmountManager.DeleteStampAmount(stampAmount);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");

        }
    }
}