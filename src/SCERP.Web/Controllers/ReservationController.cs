using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Planning;
using SCERP.Web.Areas.Planning.Models.ViewModels;
using SCERP.Web.Models;

namespace SCERP.Web.Controllers
{

    public class ReservationController : BaseController
    {
        private readonly ITargetProductionManager _targetProductionManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IMachineManager _machineManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyleManager;
        private List<VwTargetProduction> _targetProductions;

        public ReservationController(IOmBuyOrdStyleManager buyOrdStyleManager, IOmBuyerManager buyerManager, ITargetProductionManager targetProductionManager, IMachineManager machineManager)
        {
            _buyOrdStyleManager = buyOrdStyleManager;
            _targetProductionManager = targetProductionManager;
            _machineManager = machineManager;
            this._buyerManager = buyerManager;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(string id)
        {
            DataRow dr = Db.GetReservation(id);
            if (dr == null)
            {
                throw new Exception("The task was not found");
            }

            return View(new
            {
                Id = id,
                Text = dr["ReservationName"],
                Start = Convert.ToDateTime(dr["ReservationStart"]).ToShortDateString(),
                End = Convert.ToDateTime(dr["ReservationEnd"]).ToShortDateString(),
                Status = new SelectList(new SelectListItem[]
                {
                new SelectListItem { Text = "New", Value = "0"},
                new SelectListItem { Text = "Confirmed", Value = "1"},
                new SelectListItem { Text = "Arrived", Value = "2"},
                new SelectListItem { Text = "Checked out", Value = "3"}
                }, "Value", "Text", dr["ReservationStatus"]),
                Paid = new SelectList(new SelectListItem[]
                {
                new SelectListItem { Text = "0%", Value = "0"},
                new SelectListItem { Text = "50%", Value = "50"},
                new SelectListItem { Text = "100%", Value = "100"},
                }, "Value", "Text", dr["ReservationPaid"]),
                Resource = new SelectList(Db.GetRoomSelectList(), "Value", "Text", dr["RoomId"])
            });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveEdit(TargetProductionViewModel model)
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
            return Reload();
        }

        public ActionResult Create(TargetProductionViewModel model)
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveCreate(TargetProductionViewModel model)
        {
            //DateTime start = Convert.ToDateTime(form["Start"]).Date.AddHours(12);
            //DateTime end = Convert.ToDateTime(form["End"]).Date.AddHours(12);
            //string text = form["Text"];
            //string resource = form["Resource"];

            //Db.CreateReservation(start, end, resource, text);
            //return JavaScript(SimpleJsonSerializer.Serialize("OK"));

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
            return Reload();
        }


    }

}