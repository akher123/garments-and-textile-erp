using System.Collections.Generic;
using SCERP.Model;
using SCERP.Model.CommonModel;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class YarnConsumptionCostViewModel : OM_BuyOrdStyle
    {
        public YarnConsumptionCostViewModel()
        {
            YarnConsumptions = new List<VYarnConsumption>();
            OmBuyOrdStyles = new List<VwFabricOrderDetail>();
            SupplierCompanies = new List<Mrc_SupplierCompany>();
            PiInvoices = new List<ProFormaInvoice>();
        }
        public List<VYarnConsumption> YarnConsumptions { get; set; }
        public List<VwFabricOrderDetail> OmBuyOrdStyles { get; set; }
        public List<Mrc_SupplierCompany> SupplierCompanies { get; set; }

        public List<ProFormaInvoice> PiInvoices { get; set; }
    }
}