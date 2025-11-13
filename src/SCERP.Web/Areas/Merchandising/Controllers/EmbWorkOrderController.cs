using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Production;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using Microsoft.Reporting.WebForms;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class EmbWorkOrderController : BaseController
    {
        private readonly IEmbWorkOrderManager _embWorkOrderManager;
        private readonly IMerchandiserManager _merchandiserManager;
        private readonly IPartyManager _partyManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IComponentManager _componentManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyleManager;
        public EmbWorkOrderController(IEmbWorkOrderManager embWorkOrderManager, IMerchandiserManager merchandiserManager, IPartyManager partyManager, IOmBuyerManager buyerManager, IComponentManager componentManager, IOmBuyOrdStyleManager buyOrdStyleManager)
        {
            _embWorkOrderManager = embWorkOrderManager;
            _merchandiserManager = merchandiserManager;
            _partyManager = partyManager;
            _buyerManager = buyerManager;
            _componentManager = componentManager;
            _buyOrdStyleManager = buyOrdStyleManager;
        }

        public ActionResult Index(EmbWorkOrderViewModel model)
        {
            ModelState.Clear();
            return View(model);
        }

        public PartialViewResult EmbWorkOrderList(EmbWorkOrderViewModel model)
        {
            int totalRecords = 0;
             ModelState.Clear();
            model.EmbWorkOrders = _embWorkOrderManager.GetEmbWorkOrders(model.PageIndex, model.sort, model.sortdir, out totalRecords, model.SearchString,model.EmbWorkOrder.PartyId);
            model.Parties = _partyManager.GetAllParties(PortalContext.CurrentUser.CompId);
            return PartialView("~/Areas/Merchandising/Views/EmbWorkOrder/_EmbWorkOrderList.cshtml", model);
        }
        public PartialViewResult EmbWorkOrderDetailList(int embWorkOrderId)
        {
            EmbWorkOrderViewModel model = new EmbWorkOrderViewModel
            {
                EmbWorkOrderDetails = _embWorkOrderManager.GetEmbWorkOrderDetails(embWorkOrderId)
            };
            model.EmbWorkOrderDetail.EmbWorkOrderId = embWorkOrderId;
            return PartialView("~/Areas/Merchandising/Views/EmbWorkOrder/_EmbWorkOrderDetailList.cshtml", model);
        }
        public ActionResult Edit(EmbWorkOrderViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.EmbWorkOrder.EmbWorkOrderId > 0)
                {
                    model.EmbWorkOrder = _embWorkOrderManager.GetEmbWorkOrderById(model.EmbWorkOrder.EmbWorkOrderId);
                }
                else
                {
                    model.EmbWorkOrder.RefId = _embWorkOrderManager.GetNewRefId(PortalContext.CurrentUser.CompId);
                }
                model.Merchandisers = _merchandiserManager.GetPermitedMerchandisers();
                model.Parties = _partyManager.GetAllParties(PortalContext.CurrentUser.CompId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }

            return View(model);

        }

        public ActionResult EditDetail(int embWorkOrderDetailId, int embWorkOrderId)
        {
            EmbWorkOrderViewModel model = new EmbWorkOrderViewModel();
            try
            {
               
                if (embWorkOrderDetailId > 0)
                {
                    model.EmbWorkOrderDetail = _embWorkOrderManager.GetEmbWorkOrderDetailById(embWorkOrderDetailId);
                    model.Buyers = _buyerManager.GetAllBuyers();
                    model.OrderList = _buyOrdStyleManager.GetOrderByBuyer(model.EmbWorkOrderDetail.BuyerRefId);
                    model.Styles = _buyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.EmbWorkOrderDetail.OrderNo);
                    model.Colors = _buyOrdStyleManager.GetColorsByOrderStyleRefId(model.EmbWorkOrderDetail.OrderStyleRefId);
                    model.Sizes = _buyOrdStyleManager.GetSizeByOrderStyleRefId(model.EmbWorkOrderDetail.OrderStyleRefId);
                }
                else
                {
                    model.EmbWorkOrderDetail.EmbWorkOrderId = embWorkOrderId;
                }
                model.Buyers = _buyerManager.GetAllBuyers();
                model.Components = _componentManager.GetComponents();

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }

            return View(model);

        }
        public ActionResult Save(EmbWorkOrderViewModel model)
        {
            try
            {
                OM_EmbWorkOrder embWorkOrder;
                if (model.EmbWorkOrder.EmbWorkOrderId > 0)
                {
                    embWorkOrder = _embWorkOrderManager.GetEmbWorkOrderById(model.EmbWorkOrder.EmbWorkOrderId) ?? new OM_EmbWorkOrder();
                    embWorkOrder.PartyId = model.EmbWorkOrder.PartyId;
                    embWorkOrder.MerchandiserRefId = model.EmbWorkOrder.MerchandiserRefId;
                    embWorkOrder.ProcessRefId = model.EmbWorkOrder.ProcessRefId;
                    embWorkOrder.Remarks = model.EmbWorkOrder.Remarks;
                    embWorkOrder.Attention = model.EmbWorkOrder.Attention;
                    embWorkOrder.BookingDate = model.EmbWorkOrder.BookingDate;
                    embWorkOrder.ExpectedDate = model.EmbWorkOrder.ExpectedDate;
                    embWorkOrder.EditedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
                    embWorkOrder.EditedDate = DateTime.Now;
                    embWorkOrder.IsApproved = false;
                }
                else
                {
                    embWorkOrder = model.EmbWorkOrder;
                    embWorkOrder.CompId = PortalContext.CurrentUser.CompId;
                    embWorkOrder.CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
                    embWorkOrder.CreatedDate = DateTime.Now;
                    embWorkOrder.IsApproved = false;
                }
                int saved = _embWorkOrderManager.SaveEmbWorkOrder(embWorkOrder);
                return saved > 0 ? Reload() : ErrorResult("Work Order Save Failed");

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("System Error :" + exception.Message);
            }

        }

        public ActionResult SaveDetails(EmbWorkOrderViewModel model)
        {
            try
            {
                OM_EmbWorkOrderDetail embWorkOrderDetail;
                if (model.EmbWorkOrderDetail.EmbWorkOrderDetailId > 0)
                {
                    embWorkOrderDetail = _embWorkOrderManager.GetEmbWorkOrderDetailById(model.EmbWorkOrderDetail.EmbWorkOrderDetailId) ?? new OM_EmbWorkOrderDetail();
                    embWorkOrderDetail.BuyerRefId = model.EmbWorkOrderDetail.BuyerRefId;
                    embWorkOrderDetail.OrderNo = model.EmbWorkOrderDetail.OrderNo;
                    embWorkOrderDetail.OrderStyleRefId = model.EmbWorkOrderDetail.OrderStyleRefId;
                    embWorkOrderDetail.ItemName = model.EmbWorkOrderDetail.ItemName;
                    embWorkOrderDetail.FabricType = model.EmbWorkOrderDetail.FabricType;
                    embWorkOrderDetail.EmbellishmentType = model.EmbWorkOrderDetail.EmbellishmentType;
                    embWorkOrderDetail.ComponentRefId = model.EmbWorkOrderDetail.ComponentRefId;
                    embWorkOrderDetail.GColorRefId = model.EmbWorkOrderDetail.GColorRefId ?? "0000";
                    embWorkOrderDetail.GSizeRefId = model.EmbWorkOrderDetail.GSizeRefId??"0000";
                    embWorkOrderDetail.Qty = model.EmbWorkOrderDetail.Qty;
                    embWorkOrderDetail.Rate = model.EmbWorkOrderDetail.Rate;
                    embWorkOrderDetail.Remarks = model.EmbWorkOrderDetail.Remarks;
                }
                else
                {
                    embWorkOrderDetail = model.EmbWorkOrderDetail;
                    embWorkOrderDetail.CompId = PortalContext.CurrentUser.CompId;
                   // embWorkOrderDetail.GSizeRefId = "0000";

                }
            
                int saved = _embWorkOrderManager.SaveEmbWorkOrderDetail(embWorkOrderDetail);
                if (saved > 0)
                {

                    model.EmbWorkOrderDetails = _embWorkOrderManager.GetEmbWorkOrderDetails(model.EmbWorkOrderDetail.EmbWorkOrderId);
                    return PartialView("~/Areas/Merchandising/Views/EmbWorkOrder/_EmbWorkOrderDetailList.cshtml", model);
                }
                else
                {
                    return ErrorResult("Save Failed");
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("System Error :" + exception.Message);
            }

        }
        public ActionResult Delete(int embWorkOrderId)
        {
            try
            {
                int deleted = _embWorkOrderManager.DeleteEmbWorkOrder(embWorkOrderId);
                return deleted > 0 ? Reload() : ErrorResult("Work Order Delete Failed");
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("System Error :" + exception.Message);
            }
        }

        public ActionResult DeleteDetail(int embWorkOrderDetailId, int embWorkOrderId)
        {
            try
            {
                int deleted = _embWorkOrderManager.DeleteEmbWorkOrderDetail(embWorkOrderDetailId);
                if (deleted > 0)
                {

                    EmbWorkOrderViewModel model = new EmbWorkOrderViewModel
                    {
                        EmbWorkOrderDetails = _embWorkOrderManager.GetEmbWorkOrderDetails(embWorkOrderId)
                    };
                    model.EmbWorkOrderDetail.EmbWorkOrderId = embWorkOrderId;
                    return PartialView("~/Areas/Merchandising/Views/EmbWorkOrder/_EmbWorkOrderDetailList.cshtml", model);
                }
                else
                {
                    return ErrorResult("Save Failed");
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("System Error :" + exception.Message);
            }


        }

        public ActionResult PrintEmbWorkOrderReport(string embWorkOrderId)
        {
            string reportName = "PrintEmbWorkOrder";
            var reportParams = new List<ReportParameter> { new ReportParameter("EmbWorkOrderId",embWorkOrderId),
             new ReportParameter("CompId", PortalContext.CurrentUser.CompId),
             new ReportParameter("HostingServerAddress", AppConfig.HostingServerAddress)};
            return ReportExtension.ToSsrsFile(ReportType.PDF, reportName, reportParams);
        }


        public ActionResult GetColorSizeByOrderStyleRefId(string orderStyleRefId)
        {
            IEnumerable colors = _buyOrdStyleManager.GetColorsByOrderStyleRefId(orderStyleRefId);
            IEnumerable sizes = _buyOrdStyleManager.GetSizeByOrderStyleRefId(orderStyleRefId);
            return Json(new { colors, sizes }, JsonRequestBehavior.AllowGet);

        }

    }
}