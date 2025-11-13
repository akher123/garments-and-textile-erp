using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model.Planning;
using SCERP.Web.Controllers;
using SCERP.Common;
using SCERP.Web.Areas.Planning.Models.ViewModels;
using SCERP.BLL.IManager.IPlanningManager;

namespace SCERP.Web.Areas.Planning.Controllers
{
    public class TnaGroupUpdateController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        private readonly ITNAManager TnaManager;
        private readonly ITNAHorizontalManager TnaHorizontalManager;
        public TnaGroupUpdateController(ITNAManager TnaManager, ITNAHorizontalManager TnaHorizontalManager)
        {
            this.TnaManager = TnaManager;
            this.TnaHorizontalManager = TnaHorizontalManager;
        }
        public ActionResult Index(TNAHorizontalViewModel model)
        {
            try
            {
                ModelState.Clear();

                var seasonNameList = TnaManager.GetAllSeasons();
                var buyerList = TnaManager.GetAllBuyers();
                var merchandiserList = TnaManager.GetAllMerchandiser();

                ViewBag.SearchBySeasonId = new SelectList(seasonNameList, "SeasonRefId", "SeasonName");
                ViewBag.SearchByBuyerId = new SelectList(buyerList, "BuyerRefId", "BuyerName");
                ViewBag.SearchByMerchandiserId = new SelectList(merchandiserList, "EmpId", "EmpName");
                model.Particulars = TnaManager.GetStyleUf();

                if (model.IsSearch)
                {
                    model.FromDate = DateTime.Now;
                    model.ToDate = DateTime.Now.AddDays(7);
                    model.IsSearch = false;
                    return View(model);
                }

                PLAN_TNAHorizontal tna = model;

                tna.SeasonRefId = model.SearchBySeasonId;
                tna.BuyerName = model.SearchByBuyerId.ToString(CultureInfo.InvariantCulture);
                tna.MerchandiserName = model.SearchByMerchandiserId.ToString(CultureInfo.InvariantCulture);
                tna.PI = model.SearchByStyleUfId.Trim();
                tna.FromDate = model.FromDate;
                tna.ToDate = model.ToDate;

                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }

                var totalRecords = 0;
                model.tnaHorizon = TnaHorizontalManager.GetAllTnaUpdateHorizontalByPaging(startPage, _pageSize, out totalRecords, tna) ?? new List<PLAN_TNAHorizontal>();
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }
    }
}