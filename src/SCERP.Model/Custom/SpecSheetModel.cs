using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class SpecSheetModel
    {
        public int SpecificationSheetId { get; set; }
        public string BuyerName { get; set; }
        public string Name { get; set; }
        public string StyleNo { get; set; }
        public string JobNo { get; set; }
        public string ReferenceNo { get; set; }
        public string Department { get; set; }
        public string Season { get; set; }
        public string Brand { get; set; }
        public string StyleDescription { get; set; }
        public string FabricationDescription { get; set; }
        public string Material { get; set; }
        public string Finishing { get; set; }
        public string ItemGroup { get; set; }
        public string SizeRange { get; set; }
        public string ApproximateQuantity { get; set; }
        public string LeadTimeInDays { get; set; }
        public string EntryDate { get; set; }
        public string ShipmentDate { get; set; }
        public string Remarks { get; set; }
    }
}
