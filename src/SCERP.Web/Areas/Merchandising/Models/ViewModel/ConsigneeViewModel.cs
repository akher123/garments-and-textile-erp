using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class ConsigneeViewModel:OM_Consignee
    {
        public List<OM_Buyer> OmBuyers { get; set; }
        public List<OM_Consignee> Consignees { get; set; }
        public IEnumerable Cities { get; set; }
        public List<Country> Countries { get; set; }
        public ConsigneeViewModel()
        {
            OmBuyers=new List<OM_Buyer>();
            Consignees=new List<OM_Consignee>();
            Cities = new List<Object>();
        }
        public List<SelectListItem> CountrySelectListItem
        {
            get { return new SelectList(Countries, "Id", "CountryName").ToList(); }

        }
        public List<SelectListItem> CitySelectListItem
        {
            get { return new SelectList(Cities, "CityId", "CityName").ToList(); }

        }
        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(OmBuyers, "BuyerRefId", "BuyerName");
            }
        }

    }
}