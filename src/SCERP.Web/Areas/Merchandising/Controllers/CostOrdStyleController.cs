using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class CostOrdStyleController : BaseController
    {
        private readonly IOmBuyOrdStyleManager _omBuyOrdStyleManager;
        private readonly ICostDefinationManager _costDefinationManager;
        private readonly ICostOrdStyleManager _costOrdStyleManager;
        private readonly IOmBuyerManager _buyerManager;

        public CostOrdStyleController(IOmBuyOrdStyleManager omBuyOrdStyleManager, ICostDefinationManager costDefinationManager, ICostOrdStyleManager costOrdStyleManager, IOmBuyerManager buyerManager)
        {
            this._omBuyOrdStyleManager = omBuyOrdStyleManager;
            this._costDefinationManager = costDefinationManager;
            this._costOrdStyleManager = costOrdStyleManager;
            _buyerManager = buyerManager;
        }

        [AjaxAuthorize(Roles = "precost -1,precost-2,precost-3")]

        public ActionResult Index(CostOrdStyleViewModel model)
        {
            ModelState.Clear();
            model.Buyers = _buyerManager.GetAllBuyers();
            var totalRecords = 0;
            model.CostDate = DateTime.Now;
            model.OrderList = _omBuyOrdStyleManager.GetOrderByBuyer(model.BuyerRefId);
            model.Styles = _omBuyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.OrderNo);
            var omBuyOrdStyle = new OM_BuyOrdStyle { page = model.page, sort = model.sort, sortdir = model.sortdir, FromDate = model.FromDate, ToDate = model.ToDate, SearchString = model.OrderStyleRefId };
            model.OmBuyOrdStyles = _omBuyOrdStyleManager.GetBuyerOrderStyles(omBuyOrdStyle, out totalRecords);
            model.TotalRecords = totalRecords;
            model.ButtinName = "Add";
            return View(model);
        }
        [AjaxAuthorize(Roles = "precost -1,precost-2,precost-3")]
        public ActionResult CostOrdStyleList(CostOrdStyleViewModel model)
        {
            ModelState.Clear();

            model.CostDate = DateTime.Now;
            model.CostDefinations = _costDefinationManager.GetCostDefination();
            model.ButtinName = "Add";
            model.VCostOrderStyles = _costOrdStyleManager.GetCostCostOrdStyle(model.OrderStyleRefId);
            return PartialView("~/Areas/Merchandising/Views/CostOrdStyle/_CostOrdStyleList.cshtml", model);
        }
        [AjaxAuthorize(Roles = "precost-2,precost-3")]
        public ActionResult Edit(CostOrdStyleViewModel model)
        {
            ModelState.Clear();
            var costOrderStyle = _costOrdStyleManager.GetCostCostOrderById(model.CostOrderStyleId);
            if (model.CostOrderStyleId > 0)
            {
                model.CostDefinations = _costDefinationManager.GetCostDefinationByCostGroup(costOrderStyle.CostGroup, PortalContext.CurrentUser.CompId);
            }
            model.CostOrderStyleId = costOrderStyle.CostOrderStyleId;
            model.CostDate = costOrderStyle.CostDate;
            model.CostRate = costOrderStyle.CostRate;
            model.CostRefId = costOrderStyle.CostRefId;
            model.CostGroup = costOrderStyle.CostGroup;
            model.OrderStyleRefId = costOrderStyle.OrderStyleRefId;
            model.Unit = costOrderStyle.Unit;
            model.Qty = costOrderStyle.Qty;
            model.ButtinName = "Edit";
            return PartialView("~/Areas/Merchandising/Views/CostOrdStyle/_Edit.cshtml", model);
        }
        [AjaxAuthorize(Roles = "precost-2,precost-3")]
        public ActionResult Save(OM_CostOrdStyle model)
        {
            try
            {
                model.CostDate = DateTime.Now;
                var saveIndex = model.CostOrderStyleId > 0 ? _costOrdStyleManager.EditCostOrdStyle(model) : _costOrdStyleManager.SaveCostOrdStyle(model);
                if (saveIndex > 0)
                {

                    return RedirectToAction("CostOrdStyleList", new { model.OrderStyleRefId });
                }
                else
                {
                    return ErrorResult("Save Fail !!");
                }
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }

        }
        [AjaxAuthorize(Roles = "precost-3")]
        public ActionResult Delete(CostOrdStyleViewModel model)
        {
            try
            {
                var deleteIndex = _costOrdStyleManager.DeleteCostOrdStyle(model.CostOrderStyleId);
                if (deleteIndex > 0)
                {

                    return RedirectToAction("CostOrdStyleList", new { model.OrderStyleRefId });
                }
                else
                {
                    return ErrorResult("Delete Fail !!");
                }
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
        }
        [AjaxAuthorize(Roles = "precost-2,precost-3")]
        public JsonResult GetCostDefinationByCostGroup(string costGroupId)
        {
            List<OM_CostDefination> costDefinations = _costDefinationManager.GetCostDefinationByCostGroup(costGroupId, PortalContext.CurrentUser.CompId);
            return Json(costDefinations, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CopyCostOrdStyleList(CostOrdStyleViewModel model)
        {
            ModelState.Clear();
            model.OrderList = _omBuyOrdStyleManager.GetOrderByBuyer(model.BuyerRefId);
            model.Styles = _omBuyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.OrderNo);
            model.VCostOrderStyles = _costOrdStyleManager.GetCostCostOrdStyle(model.OrderStyleRefId);
            return View(model);
        }

        public ActionResult SaveCostOrdStyleList(CostOrdStyleViewModel model)
        {
            
            bool exist = _costOrdStyleManager.IsPreCostExit(model.OrderStyleRefId);
            if (!exist)
            {
                List<OM_CostOrdStyle> costOrdStyles = model.VCostOrderStyles.Select(x => new OM_CostOrdStyle()
                {
                    CompId = PortalContext.CurrentUser.CompId,
                    OrderStyleRefId = model.OrderStyleRefId,
                    CostRate = x.CostRate,
                    CostRefId = x.CostRefId,
                    Unit = x.Unit,
                    CostDate = DateTime.Now,
                    Qty = x.Qty
                }).ToList();
                int saved = _costOrdStyleManager.SaveCostOrdStyleList(costOrdStyles);
                return Reload();
            }
            else
            {
                return ErrorResult("Pre Costing Exist");
            }



        }
    }
}