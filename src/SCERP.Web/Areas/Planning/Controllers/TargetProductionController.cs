using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Planning;
using SCERP.Web.Areas.Planning.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;

namespace SCERP.Web.Areas.Planning.Controllers
{
    public class TargetProductionController : BaseController
    {
        private readonly ITargetProductionManager _targetProductionManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IMachineManager _machineManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyleManager;
        private List<VwTargetProduction> _targetProductions;

        public TargetProductionController(IOmBuyOrdStyleManager buyOrdStyleManager, IOmBuyerManager buyerManager, ITargetProductionManager targetProductionManager, IMachineManager machineManager)
        {
            _buyOrdStyleManager = buyOrdStyleManager;
            _targetProductionManager = targetProductionManager;
            _machineManager = machineManager;
            this._buyerManager = buyerManager;
        }
        [AjaxAuthorize(Roles = "sewingplan-1,sewingplan-2,sewingplan-3")]
        public ActionResult Index(TargetProductionViewModel model)
        {
            ModelState.Clear();
            if (model.YearId == 0)
            {
                model.YearId = DateTime.Now.Year;
                model.MonthId = (MonthEnum)DateTime.Now.Month;
            }
            _targetProductions = _targetProductionManager.GetMontylyActiveTargetProductionList((int)model.MonthId, model.YearId, PortalContext.CurrentUser.CompId);
        
            model.TargetProductions = _targetProductions;
            return View(model);
        }


           [AjaxAuthorize(Roles = "sewingplan-2,sewingplan-3")]
        public ActionResult Edit(TargetProductionViewModel model)
        {
            ModelState.Clear();
            model.Machines = _machineManager.GetLines();
            model.Buyers = _buyerManager.GetAllBuyers();
            if (model.PlanTargetProduction.TargetProductionId > 0)
            {

                model.PlanTargetProduction = _targetProductionManager.GetTargetProductionById(model.PlanTargetProduction.TargetProductionId);
                model.OrderList = _buyOrdStyleManager.GetOrderByBuyer(model.PlanTargetProduction.BuyerRefId);
                model.BuyerOrderStyles = _buyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.PlanTargetProduction.OrderNo);
            }
            else
            {
                model.PlanTargetProduction.TargetProductionRefId = _targetProductionManager.GetnewTargetProductionRefId(PortalContext.CurrentUser.CompId);
            }
            return View(model);
        }
          [AjaxAuthorize(Roles = "sewingplan-2,sewingplan-3")]
        public ActionResult Save(TargetProductionViewModel model)
        {
            int saveIndex = 0;
            try
            {
                PLAN_TargetProduction targetProduction = _targetProductionManager.GetTargetProductionById(model.PlanTargetProduction.TargetProductionId);

                targetProduction.BuyerRefId = model.PlanTargetProduction.BuyerRefId;
                targetProduction.OrderNo = model.PlanTargetProduction.OrderNo;
                targetProduction.OrderStyleRefId = model.PlanTargetProduction.OrderStyleRefId;
                targetProduction.LineId = model.PlanTargetProduction.LineId;
                targetProduction.TotalTargetQty = model.PlanTargetProduction.TotalTargetQty;
                targetProduction.Remarks = model.PlanTargetProduction.Remarks;
                targetProduction.StartDate = model.PlanTargetProduction.StartDate;
                targetProduction.EndDate = model.PlanTargetProduction.EndDate;
                if (model.PlanTargetProduction.TargetProductionId > 0)
                {

                    targetProduction.EditedBy = PortalContext.CurrentUser.UserId;
                    targetProduction.EditedDate = DateTime.Now;
                }
                else
                {
                    targetProduction.CreatedBy = PortalContext.CurrentUser.UserId;
                    targetProduction.CreatedDate = DateTime.Now;
                    targetProduction.CompId = PortalContext.CurrentUser.CompId;
                    targetProduction.TargetProductionRefId = _targetProductionManager.GetnewTargetProductionRefId(targetProduction.CompId);
                }

                saveIndex = _targetProductionManager.SaveTargetProduction(targetProduction);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Internale System Error " + exception.Message);
            }
            return model.IsPartial ? (ActionResult)RedirectToAction("PlanningBorad") : Reload();
        }
          [AjaxAuthorize(Roles = "sewingplan-3")]
        public ActionResult Delete(long targetProductionId)
        {
            var deleteIndex = _targetProductionManager.DeleteTargetProduction(targetProductionId);
            return deleteIndex > 0 ? Reload() : ErrorResult("Delete Failed !!");
        }


        [AjaxAuthorize(Roles = "sewingplanningboard-1,sewingplanningboard-2,sewingplanningboard-3")]
        public ActionResult PlanningBorad(TargetProductionViewModel model)
        {

            int total = 0;
            if (model.YearId == 0)
            {
                model.YearId = DateTime.Now.Year;
                model.MonthId = (MonthEnum)DateTime.Now.Month;
            }
            model.Machines = _machineManager.GetLines();
            List<GenttValue> genttValues = new List<GenttValue>();
            model.GnettSources.Add(new GnettSource { name = "<b style=' color: red'>LINE NAME</b>", desc = "&nbsp;&nbsp;" + "<b  style=' color: red'>QUANTITY</b>", values = new List<GenttValue>() });
            _targetProductions = _targetProductionManager.GetMontylyActiveTargetProductionList((int)model.MonthId, model.YearId, PortalContext.CurrentUser.CompId);
            foreach (var line in model.Machines)
            {
                int qty = 0;
                genttValues = GetGnettValue(line.MachineId, out qty);
                var gnett = new GnettSource
                {
                    name = line.Name,
                    desc = "&nbsp;&nbsp;&nbsp;" + "<b>" + qty + "</b>",
                    id = line.MachineId
                };
                total += qty;
                model.GnettSources.Add(gnett);
                if (genttValues.Any())
                {
                    gnett.values.AddRange(genttValues);

                }
            }
            model.GnettSources.Add(new GnettSource
            {
                name = "<b style=' color: black'>TOTAL</b>",
                desc = "&nbsp;&nbsp;" + "<b  style=' color: black'>" + total + "</b>",
                values = new List<GenttValue>()
            });
            return View(model);
        }

        private List<GenttValue> GetGnettValue(int lineId, out int qty)
        {
            List<VwTargetProduction> targetProductions = _targetProductions.Where(x => x.LineId == lineId).ToList();
            qty = targetProductions.Sum(x => x.TotalTargetQty);
            return targetProductions.Select(x => new GenttValue()
            {
                id = x.TargetProductionId.ToString(),
                to = x.EndDate.GetValueOrDefault(),
                from = x.StartDate.GetValueOrDefault(),
                label = x.BuyerName + "&nbsp;" + ":" + x.StyleName,
                customClass = "ganttRed",
                desc = "Buyer :" + x.BuyerName + "</br>Order :" + x.OrderName + "</br>Style :" + x.StyleName + "</br>Plan Qty :" + x.TotalTargetQty+ "</br>Achieved Qty :" + x.AcheivedQty + "</br>From :" + x.StartDate.GetValueOrDefault().ToString("dd/MM/yyyy") + "<br/>To :" + x.EndDate.GetValueOrDefault().ToString("dd/MM/yyyy"),
                dataObj = x.TargetProductionId.ToString()
            }).ToList();

        }


    }
}