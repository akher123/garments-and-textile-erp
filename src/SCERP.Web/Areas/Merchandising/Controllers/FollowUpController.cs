using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model.MerchandisingModel;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class FollowUpController : BaseController
    {
        private readonly IOmBuyOrdStyleManager _omBuyOrdStyleManager;
        private readonly IPurchaseOrderManager _purchaseOrderManager;
        private readonly IMarchandisingReportManager _marchandisingReportManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IMerchandiserManager _merchandiserManager;
        private readonly ISeasonManager _seasonManager;
        public FollowUpController(ISeasonManager seasonManager, IOmBuyerManager buyerManager,IMerchandiserManager merchandiserManager,IPurchaseOrderManager purchaseOrderManager, IMarchandisingReportManager marchandisingReportManager, IOmBuyOrdStyleManager omBuyOrdStyleManager)
        {
            _seasonManager = seasonManager;
            _merchandiserManager = merchandiserManager;
            _buyerManager = buyerManager;
            _purchaseOrderManager = purchaseOrderManager;
            _marchandisingReportManager = marchandisingReportManager;
            _omBuyOrdStyleManager = omBuyOrdStyleManager;
        }
    
        public ActionResult MaterialOrderStyleList(FollowUpViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            List<VOMBuyOrdStyle> vomBuyOrdStyles = _omBuyOrdStyleManager.GetBuyerOrderStyles(model, out totalRecords);
            model.OmBuyOrdStyles = vomBuyOrdStyles;
            model.TotalRecords = totalRecords;
            return View(model);
        }
        public ActionResult ShowMaterialStatus(FollowUpViewModel model)
        {
            ModelState.Clear();
            model.MaterialStatusList = _purchaseOrderManager.GetSyleWiseMaterialStatus(model.OrderStyleRefId);
            if (model.IsShowReport)
            {
                string path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report/RDLC"), "MaterialStatusReport.rdlc");
                if (!System.IO.File.Exists(path))
                {
                    return View(model);
                }
                var reportDataSources = new List<ReportDataSource>()
                {
                    new ReportDataSource("MaterialStatusDataSet", model.MaterialStatusList)
                };
                return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources);
            }
            return View(model);
        }
        public ActionResult ProductionStatus(FollowUpViewModel model)
        {
            ModelState.Clear();
            if (!model.IsShowReport)
            {
                model.IsShowReport = true;
                return View(model);
            }
            else
            {
                model.DatatTable = _marchandisingReportManager.GetProductionStatus(model.FromDate,model.ToDate);
                return View(model);
            }
        }
        public ActionResult ShipmentStatus(FollowUpViewModel model)
        {

            ModelState.Clear();
            if (!model.IsShowReport)
            {
                model.IsShowReport = true;
                model.Merchandisers = _merchandiserManager.GetMerchandisers();
                model.Buyers = _buyerManager.GetAllBuyers();
                model.Seasons = _seasonManager.GetSeasons();
                return View(model);
            }
            else
            {
                model.DatatTable = _marchandisingReportManager.GetShipmentStatus(model.SeasonRefId, model.MerchandiserId, model.BuyerRefId);
                var reportDataSource = new ReportDataSourceModel()
                {
                    DataSource = model.DatatTable,
                    Path = "~/Areas/Merchandising/Report/RDLC/ShipmentStatusReport.rdlc",
                    DataSetName = "ShipmentStatusDataSet",
                   
                };
                return PartialView("~/Views/Shared/ReportViwerAPX.aspx", reportDataSource);
            }
          
        }

        public ActionResult Index(FollowUpViewModel model)
        {
             ModelState.Clear();
             var totalRecords = 0;
            if (!model.IsSearch)
            {
                model.IsSearch = true;
            }
            else
            {
                model.VwStyleFollowupStatuses = _omBuyOrdStyleManager.GetStyleFollowupStatusesByPaging(model.PageIndex, model.FromDate, model.ToDate, model.MerchandiserId, model.BuyerRefId, model.SearchString, out totalRecords);
            }
            model.Buyers = _buyerManager.GetAllBuyers();
            model.Merchandisers = _merchandiserManager.GetMerchandisers();
            model.TotalRecords = totalRecords; 

            return View(model);
        }

        public ActionResult SendMail()
        {
            int id=_marchandisingReportManager.SendMailExecut();
            return ErrorResult("Mail Send Successfully");
        }
    }
}