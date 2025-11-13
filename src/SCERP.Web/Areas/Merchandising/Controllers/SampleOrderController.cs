using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model.MerchandisingModel;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using Microsoft.Reporting.WebForms;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class SampleOrderController : BaseController
    {
        private IOmBuyerManager _buyerManager;
        private readonly IMerchandiserManager _merchandiserManager; 
        private readonly ISampleOrderManager _sampleOrderManager;

        public SampleOrderController(ISampleOrderManager sampleOrderManager, IOmBuyerManager buyerManager, IMerchandiserManager merchandiserManager)
        {
            _sampleOrderManager = sampleOrderManager;
            _buyerManager = buyerManager;
            _merchandiserManager = merchandiserManager;
        }

        //
        // GET: /Merchandising/SampleOrder/
        public ActionResult Index(SampleOrderViewModel model)
        {
            ModelState.Clear();
            return View(model);
        }

        public PartialViewResult SampleOrderList(SampleOrderViewModel model)
        {
            int totalRecords = 0;
            model.SampleOrders = _sampleOrderManager.GetSampleOrder(model.PageIndex, model.sort, model.sortdir,
                model.SearchString, out totalRecords);
            model.TotalRecords = totalRecords;
            return PartialView("~/Areas/Merchandising/Views/SampleOrder/_SampleOrderList.cshtml", model);
        }
        public ActionResult Edit(SampleOrderViewModel model)
        {
            ModelState.Clear();
            if (model.SampleOrder.SampleOrderId > 0)
            {
                model.SampleOrder = _sampleOrderManager.GetSampleOrderById(model.SampleOrder.SampleOrderId);
            }
            else
            {
                model.SampleOrder.RefId = _sampleOrderManager.GetNewRefId(PortalContext.CurrentUser.CompId);
            }
            model.Merchandisers = _merchandiserManager.GetPermitedMerchandisers();
            model.Buyers = _buyerManager.GetAllBuyers();
            return View(model);
        }

        public ActionResult Save(SampleOrderViewModel model)
        {
            try
            {
                var saved = model.SampleOrder.SampleOrderId > 0
                    ? _sampleOrderManager.EditSampleOrder(model.SampleOrder)
                    : _sampleOrderManager.SaveSampleOrder(model.SampleOrder);
                return saved > 0 ? Reload() : ErrorResult("Sample Order  not saved successfully!!");

            }
            catch (Exception e)
            {
                Errorlog.WriteLog(e);
                return ErrorResult(e.Message);
            }

        }

     

        public ActionResult Delete(int sampleOrderId)
        {
            var deleted = 0;
            var sampleOrder = _sampleOrderManager.GetSampleOrderById(sampleOrderId) ?? new OM_SampleOrder();
            deleted = _sampleOrderManager.DeleteSampleOrder(sampleOrder);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }


        public ActionResult SampleOrderReport(string sampleOrderId)
        {
            
            string reportName = "SampleOrderReport";
            var reportParams = new List<ReportParameter> { new ReportParameter("SampleOrderId", sampleOrderId), new ReportParameter("CompId", PortalContext.CurrentUser.CompId), new ReportParameter("HostingServerAddress", AppConfig.HostingServerAddress) };
            return ReportExtension.ToSsrsFile(ReportType.PDF, reportName, reportParams);
        }
    }
}