using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class FollowUpViewModel : OM_BuyOrdStyle
    {
        public bool IsShowReport { get; set; }
        public string BuyerRefId { get; set; }
        public string MerchandiserId { get; set; }
        public List<OM_Season> Seasons { get; set; }
        public DataTable DatatTable { get; set; }
        public List<MaterialStatus> MaterialStatusList { get; set; }
        public List<VOMBuyOrdStyle>OmBuyOrdStyles { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public IEnumerable<OM_Merchandiser> Merchandisers { get; set; }
        public List<VwStyleFollowupStatus> VwStyleFollowupStatuses { get; set; }
        public FollowUpViewModel()
        {
            VwStyleFollowupStatuses=new List<VwStyleFollowupStatus>();
            Merchandisers=new List<OM_Merchandiser>();
            Buyers=new List<OM_Buyer>();
            OmBuyOrdStyles=new List<VOMBuyOrdStyle>();
            DatatTable=new DataTable();
            MaterialStatusList=new List<MaterialStatus>();
            Seasons=new List<OM_Season>();
        }
            public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(Buyers, "BuyerRefId", "BuyerName");
            }
        }

            public IEnumerable<SelectListItem> MerchandisersSelectListItem
            {
                get
                {
                    return new SelectList(Merchandisers, "EmpId", "EmpName");
                }
            }
            public IEnumerable<SelectListItem> SeasonSelectListItem
            {
                get
                {
                    return new SelectList(Seasons, "SeasonRefId", "SeasonName");
                }
            }

    }
}