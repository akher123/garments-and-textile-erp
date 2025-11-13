using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Common;


namespace SCERP.Web.Areas.Commercial.Controllers
{
    public class LcOrderController : BaseController
    {
        private readonly IBuyerOrderManager _buyerOrderManager;
        private readonly ILcManager _lcManager;
        

        public LcOrderController(IBuyerOrderManager buyerOrderManager, ILcManager lcManager)
        {
            this._buyerOrderManager = buyerOrderManager;
            this._lcManager = lcManager;
        }

        public ActionResult Index(BuyerOrderViewModel model)
        {
            ModelState.Clear();

            var totalRecords = 0;
            var totalLcRecords = 0;

            model.BuyerOrders = _buyerOrderManager.GetBuyerWithoutLcOrderPaging(model, out totalRecords);
            model.BuyerOrdersLc = _buyerOrderManager.GetBuyerLcOrderPaging(model, out totalLcRecords);

            model.TotalRecords = totalRecords;
            model.TotalLcRecords = totalLcRecords;

            return View("~/Areas/Commercial/Views/LcOrder/Index.cshtml", model);
        }

        public ActionResult OrderDetailsByOrderNo(string OrderNo)
        {
            //OrderNo = "PFL/ORD00820";
            BuyerOrderViewModel model = new BuyerOrderViewModel();
            model.BuyerOrderStyle = _buyerOrderManager.GetOrderDetailsByOrderNo(OrderNo);
            return View(model);
        }

        public ActionResult Save(List<string> values)
        {
            ModelState.Clear();

            OM_BuyerOrder buyerOrder;
            int count = 0;
            int saved = 0;

            try
            {
                foreach (var t in values)
                {
                    if (count == 0)
                    {
                        count++;
                        continue;
                    }

                    string orderRefNo = t.Split('^').ElementAt(0).Trim();
                    string lcNo = t.Split('^').ElementAt(1).Trim();

                    if (!string.IsNullOrEmpty(lcNo))
                    {
                        buyerOrder = _buyerOrderManager.GetBuyerLcOrderByOrderNo(orderRefNo);

                        if (buyerOrder.BuyerOrderId > 0)
                        {
                            buyerOrder.LcRefId = _lcManager.GetLcInfoByLcNo(lcNo).LcId;
                            saved = _buyerOrderManager.EditLcOrder(buyerOrder);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            if (saved > 0)
                return RedirectToAction("Index");
            else
                return ErrorResult("Failed to delete data");
        }

        public ActionResult Delete(string orderNo)
        {
            int deleted = 0;

            try
            {
                OM_BuyerOrder buyerOrder = _buyerOrderManager.GetBuyerLcOrderByOrderNo(orderNo);
                buyerOrder.LcRefId = null;
                deleted = _buyerOrderManager.EditLcOrder(buyerOrder);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }
    }
}