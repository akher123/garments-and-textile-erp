using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class CosumptionCostViewModel:OM_BuyOrdStyle
    {
        public double BalanceQty { get; set; }
        public List<VConsumption> Consumptions { get; set; }
        public List<VOMBuyOrdStyle> OmBuyOrdStyles { get; set; }
        public List<Mrc_SupplierCompany> SupplierCompanies { get; set; }
        public OM_ConsumptionSupplier ConsumptionSupplier { get; set; }
        public List<OM_ConsumptionSupplier> ConsumptionSuppliers { get; set; }
        public long ConsumptionId { get; set; }
        public double ConsRate { get; set; }
        public double ConsQty { get; set; }
        public double AssignedQty { get; set; }
        public CosumptionCostViewModel()
        {
       
            Consumptions = new List<VConsumption>();
            OmBuyOrdStyles=new List<VOMBuyOrdStyle>();
            SupplierCompanies=new List<Mrc_SupplierCompany>();
            ConsumptionSupplier=new OM_ConsumptionSupplier();
            ConsumptionSuppliers=new List<OM_ConsumptionSupplier>();
        }

      

    }
}