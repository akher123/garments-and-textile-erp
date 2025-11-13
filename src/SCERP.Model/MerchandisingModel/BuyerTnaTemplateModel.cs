using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MerchandisingModel
{
    public class BuyerTnaTemplateModel
    {
      
        public int TemplateId { get; set; }
        public string CompId { get; set; }
        public string BuyerRefId { get; set; }
        public int TemplateTypeId { get; set; }
        public int ActivityId { get; set; }
        public double? Duration { get; set; }
        public string Remarks { get; set; }
        public string ShortName { get; set; }
        public string BuyerName { get; set; }
        public string Activity { get; set; }
        public string Responsible { get; set; }
        
        public int SerialNo { get; set; }
        public int? RSerialNo { get; set; }
        public double? FDuration { get; set; }
        public string RType { get; set; }

    }
}
