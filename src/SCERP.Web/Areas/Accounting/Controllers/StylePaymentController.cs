using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IMisManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.AccountingModel;
using SCERP.Web.Areas.Accounting.Models.ViewModels;
using SCERP.Web.Areas.MIS.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class StylePaymentController : BaseController
    {
        private readonly IMisDashboardManager _misDashboardManager;
        private readonly ISewingOutPutProcessManager _sewingOutPutProcessManager;
        private IOmBuyerManager _buyerManager;
        private readonly IOmBuyOrdStyleManager _omBuyOrdStyleManager;
        private readonly IStylePaymentManager _stylePaymentManager;

        public StylePaymentController(IMisDashboardManager misDashboardManager, ISewingOutPutProcessManager sewingOutPutProcessManager, IOmBuyerManager buyerManager, IOmBuyOrdStyleManager omBuyOrdStyleManager, IStylePaymentManager stylePaymentManager)
        {
            _misDashboardManager = misDashboardManager;
            _sewingOutPutProcessManager = sewingOutPutProcessManager;
            _buyerManager = buyerManager;
            this._omBuyOrdStyleManager = omBuyOrdStyleManager;
            _stylePaymentManager = stylePaymentManager;
        }

        // GET: Accounting/StylePayment
        [AjaxAuthorize(Roles = "stylepayment-1,stylepayment-2,stylepayment-3")]
        public ActionResult Index(StylePaymentViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.BuyerRefId = model.BuyerRefId ?? "";
            model.OrderNo = model.OrderNo ?? "";
            model.OrderStyleRefId = model.OrderStyleRefId ?? "";
            model.BuyerOrderMasterDataTable =
                _misDashboardManager.GetSpBuyerOrderMasterDashBoard(model.BuyerRefId, model.OrderNo, model.OrderStyleRefId);
            model.Buyers = _buyerManager.GetAllBuyers();

            model.OrderList = _omBuyOrdStyleManager.GetOrderByBuyer(model.BuyerRefId);
            model.BuyerOrderStyles = _omBuyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.OrderNo);
            model.VwStylePayments = _stylePaymentManager.GetAllStylePaymentByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "stylepayment-2,stylepayment-3")]
        public ActionResult Edit(StylePaymentViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.StylePaymnetId > 0)
                {
                    model.BuyerRefId = model.BuyerRefId ?? "";
                    model.OrderNo = model.OrderNo ?? "";
                    model.OrderStyleRefId = model.OrderStyleRefId ?? "";
                    
                    Acc_StylePayment stylePayment = _stylePaymentManager.GetStylePaymentsByStylePaymentsId(model.StylePaymnetId);
                    model.StylePayment = stylePayment;
                    model.Buyers = _buyerManager.GetAllBuyers();
                    model.OrderList = _omBuyOrdStyleManager.GetOrderByBuyer(model.StylePayment.BuyerRefId);
                    model.BuyerOrderStyles = _omBuyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.StylePayment.OrderNo);


                }
                else
                {
                    model.BuyerRefId = model.BuyerRefId ?? "";
                    model.OrderNo = model.OrderNo ?? "";
                    model.OrderStyleRefId = model.OrderStyleRefId ?? "";
                    model.Buyers = _buyerManager.GetAllBuyers();
                    model.OrderList = _omBuyOrdStyleManager.GetOrderByBuyer(model.BuyerRefId);
                    model.BuyerOrderStyles = _omBuyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.OrderNo);
                    model.StylePayment.StylePaymentRefId = _stylePaymentManager.GetNewStylePaymentRefId(ReferencePrefix.StylePayment);
                    model.StylePayment.PayDate=DateTime.Now;

                }
                

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "stylepayment-2,stylepayment-3")]
        public ActionResult Save(StylePaymentViewModel model)
        {
            var index = 0;
            
            try
            {
                bool exist = _stylePaymentManager.IsStylePaymentExist(model.StylePayment);
                if (!exist)
                {
                    if (model.StylePayment.StylePaymnetId > 0)
                    {
                        index = _stylePaymentManager.EditStylePayment(model.StylePayment);
                    }
                    else
                    {

                        index = _stylePaymentManager.SaveStylePayment(model.StylePayment);

                    }
                }
                else
                {
                    return ErrorResult("Same Information Already Exist ! Please Entry Another One.");
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Save Task");
        }

        [AjaxAuthorize(Roles = "stylepayment-3")]
        public ActionResult Delete(int stylePaymnetId)
        {
            var index = 0;
            try
            {
                
                index = _stylePaymentManager.DeleteStylePayment(stylePaymnetId);
                

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Delete Subject");
        }

    }
}