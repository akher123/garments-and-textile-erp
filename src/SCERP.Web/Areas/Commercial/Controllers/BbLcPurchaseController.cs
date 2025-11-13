using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Model;
using SCERP.Model.CommercialModel;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Common;
using System.Collections;

namespace SCERP.Web.Areas.Commercial.Controllers
{
    public class BbLcPurchaseController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;
        private readonly IBbLcPurchaseManager _bbLcPurchaseManager;
        private readonly IPurchaseOrderManager _purchaseOrderManager;
        private readonly IBbLcManager _bbLcManager;

        public BbLcPurchaseController(IBbLcPurchaseManager bbLcPurchaseManager, IPurchaseOrderManager purchaseOrderManager, IBbLcManager bbLcManager)
        {
            this._bbLcManager = bbLcManager;
            this._bbLcPurchaseManager = bbLcPurchaseManager;
            this._purchaseOrderManager = purchaseOrderManager;
        }

        public ActionResult Index(BbLcPurchaseViewModel model)
        {
            try
            {
                ModelState.Clear();
                CommBbLcPurchaseCommon bbLcPurchase = model;

                bbLcPurchase.BbLcNo = model.BbLcNo;
                bbLcPurchase.PurchaseOrderNo = model.PurchaseOrderNo;

                int startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                    startPage = model.page.Value - 1;

                int totalRecords = 0;
                model.VwBbLcPurchaseCommon = _bbLcPurchaseManager.GetAllBbLcPurchasesByPaging(startPage, _pageSize, out totalRecords, bbLcPurchase) ?? new List<VwBbLcPurchaseCommon>();
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            ModelState.Clear();
            BbLcPurchaseViewModel model = new BbLcPurchaseViewModel();

            try
            {
                if (id > 0)
                {
                    CommBbLcPurchaseCommon lc = _bbLcPurchaseManager.GetBbLcPurchaseByBbLcId(id).SingleOrDefault();

                    if (lc != null)
                    {
                        model.BbLcNo = lc.BbLcNo;
                        model.PurchaseOrderNo = lc.PurchaseOrderNo;
                        model.BbLcPurchaseId = lc.BbLcPurchaseId;
                    }
                    ViewBag.Title = "Edit LC";
                }
                else
                {
                    ViewBag.Title = "Add LC";
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult Save(BbLcPurchaseViewModel model)
        {
            ModelState.Clear();
            int saveIndex = 0;

            try
            {
                List<CommPurchaseOrderDetail> details = new List<CommPurchaseOrderDetail>();

                details = _purchaseOrderManager.GetPurchaseOrderDetails(model.PurchaseOrderNo);

                foreach (var t in details)
                {
                    CommBbLcPurchaseCommon bbLcPurchase = new CommBbLcPurchaseCommon();
                    bbLcPurchase.BbLcPurchaseId = model.BbLcPurchaseId;
                    bbLcPurchase.BbLcRefId = _bbLcManager.GetBbLcIdByBbLcNo(model.BbLcNo).BbLcId;
                    bbLcPurchase.BbLcNo = model.BbLcNo;
                    bbLcPurchase.PurchaseOrderRefId = _purchaseOrderManager.GetPurchaseOrderByPurchaseOrderNo(model.PurchaseOrderNo).PurchaseOrderId;
                    bbLcPurchase.PurchaseOrderNo = model.PurchaseOrderNo;
                    bbLcPurchase.PurchaseDate = _purchaseOrderManager.GetPurchaseOrderByPurchaseOrderNo(model.PurchaseOrderNo).PurchaseOrderDate;
                    bbLcPurchase.CompId = PortalContext.CurrentUser.CompId;
                    bbLcPurchase.PurchaseType = "O";
                    bbLcPurchase.PType = _purchaseOrderManager.GetPurchaseOrderByPurchaseOrderNo(model.PurchaseOrderNo).PType;
                    bbLcPurchase.IsActive = true;

                    bbLcPurchase.ItemCode = t.ItemCode;
                    bbLcPurchase.ColorRefId = t.ColorRefId;
                    bbLcPurchase.SizeRefId = t.SizeRefId;
                    bbLcPurchase.Quantity = t.Quantity;
                    bbLcPurchase.xRate = t.xRate;

                    saveIndex = (model.BbLcPurchaseId > 0) ? _bbLcPurchaseManager.EditBbLcPurchase(bbLcPurchase) : _bbLcPurchaseManager.SaveBbLcPurchase(bbLcPurchase);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        public ActionResult Delete(int id)
        {
            int deleted = 0;
            try
            {
                List<CommBbLcPurchaseCommon> lcStyle = _bbLcPurchaseManager.GetBbLcPurchaseByBbLcId(id);

                foreach (var t in lcStyle)
                {
                    deleted = _bbLcPurchaseManager.DeleteBbLcPurchase(t);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        public ActionResult TagBbLcSearch(string term)
        {
            List<string> tags = _bbLcManager.GetAllBbLcInfos().Select(p => p.BbLcNo).ToList();
            return this.Json(tags.Where(t => t.Substring(0, t.Length).Trim().ToLower().StartsWith(term.Trim().ToLower())), JsonRequestBehavior.AllowGet);
        }

        public ActionResult TagPurchaseOrderSearch(string term)
        {
            List<string> tags = _purchaseOrderManager.GetAllPurchaseOrders().Select(p => p.PurchaseOrderNo).ToList();
            return this.Json(tags.Where(t => t.Substring(0, t.Length).Trim().ToLower().StartsWith(term.Trim().ToLower())), JsonRequestBehavior.AllowGet);
        }
    }
}