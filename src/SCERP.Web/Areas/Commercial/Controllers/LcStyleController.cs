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
    public class LcStyleController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;
        private readonly ILcStyleManager _lcStyleManager;
        private readonly ITNAManager _tnaManager;
        private readonly ILcManager _lcManager;

        public LcStyleController(ILcStyleManager lcStyleManager, ITNAManager tnaManager, ILcManager lcManager)
        {
            this._lcStyleManager = lcStyleManager;
            this._tnaManager = tnaManager;
            this._lcManager = lcManager;
        }

        public ActionResult Index(LcStyleViewModel model)
        {
            try
            {
                ModelState.Clear();
                COMMLcStyle lcStyle = model;

                lcStyle.OrderNo = model.OrderNo;
                lcStyle.LcRefId = _lcStyleManager.GetLcIdByLcNo(model.LcNo);

                int startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }

                int totalRecords = 0;
                model.vWLcStyles = _lcStyleManager.GetAllLcStylesByPaging(startPage, _pageSize, out totalRecords, lcStyle) ?? new List<VwCommLcStyle>();
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
            COMMLcStyle lcStyle = new COMMLcStyle();
            LcStyleViewModel model = new LcStyleViewModel();

            lcStyle.LcRefId = id;

            try
            {
                if (id > 0)
                {
                    VwCommLcStyle lc = _lcStyleManager.GetLcStyleEditByLcId(lcStyle).SingleOrDefault();

                    if (lc != null)
                    {
                        model.LcNo = lc.LcNo;
                        model.OrderNo = lc.OrderNo;
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

        public ActionResult Save(LcStyleViewModel model)
        {
            int saveIndex = 0;

            try
            {
                COMMLcStyle lcStyle;

                var orderNo = _lcStyleManager.GetOrderNoByOrderRefNo(model.OrderNo);
                List<OM_BuyOrdStyle> styles = _lcStyleManager.GetStylesByOrderNo(orderNo);

                foreach (var t in styles)
                {
                    lcStyle = new COMMLcStyle();
                    lcStyle.LcStyleId = model.LcStyleId;
                    lcStyle.LcRefId = _lcStyleManager.GetLcIdByLcNo(model.LcNo);
                    lcStyle.OrderNo = _lcStyleManager.GetOrderNoByOrderRefNo(model.OrderNo);
                    lcStyle.OrderStyleRefId = t.OrderStyleRefId;
                    lcStyle.StyleQuantity = t.Quantity;
                    saveIndex = (model.LcStyleId > 0) ? _lcStyleManager.EditLcStyle(lcStyle) : _lcStyleManager.SaveLcStyle(lcStyle);
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
                List<COMMLcStyle> lcStyle = _lcStyleManager.GetLcStyleByLcId(id);

                foreach (var t in lcStyle)
                {
                    deleted = _lcStyleManager.DeleteLcStyle(t);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        public ActionResult TagLcSearch(string term)
        {
            List<string> tags = _lcManager.GetAllLcInfos().Select(p => p.LcNo).ToList();
            return this.Json(tags.Where(t => t.Substring(0, t.Length).Trim().ToLower().StartsWith(term.Trim().ToLower())), JsonRequestBehavior.AllowGet);
        }

        public ActionResult TagOrderSearch(string term)
        {
            List<string> tags = _tnaManager.GetAllOrders().Select(p => p.RefNo).ToList();
            return this.Json(tags.Where(t => t.Substring(0, t.Length).Trim().ToLower().StartsWith(term.Trim().ToLower())), JsonRequestBehavior.AllowGet);
        }
    }
}