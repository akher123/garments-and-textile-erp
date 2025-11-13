using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Planning;

namespace SCERP.Web.Areas.Planning.Models.ViewModels
{
    public class TNAHorizontalViewModel : PLAN_TNAHorizontal
    {
        public TNAHorizontalViewModel()
        {
            tnaHorizon = new List<PLAN_TNAHorizontal>();
            IsSearch = true;
        }

        public List<PLAN_TNAHorizontal> tnaHorizon { get; set; }

        public List<OM_Buyer> Buyers { get; set; }

        public List<SelectListItem> BuyerSelectListItem
        {
            get { return new SelectList(Buyers, "BuyerRefId", "BuyerName").ToList(); }
        }

        public List<OM_BuyerOrder> BuyerOrders { get; set; }

        public List<SelectListItem> BuyerOrderSelectListItem
        {
            get { return new SelectList(BuyerOrders, "OrderNo", "OrderNo").ToList(); }
        }

        public List<OM_Season> Seasons { get; set; }

        public List<SelectListItem> SeasonsSelectListItem
        {
            get { return new SelectList(Seasons, "SeasonRefId", "SeasonName").ToList(); }
        }

        public List<PLAN_StyleUF> Particulars { get; set; }

        public List<SelectListItem> ParticularsSelectListItem
        {
            get { return new SelectList(Particulars, "FLD", "FDes").ToList(); }
        }


        public string SearchBySeasonId
        {
            get;
            set;
        }

        public int SearchByBuyerId
        {
            get;
            set;
        }

        public int SearchByMerchandiserId
        {
            get;
            set;
        }

        public string SearchByStyleUfId
        {
            get; 
            set;
        }
    }
}