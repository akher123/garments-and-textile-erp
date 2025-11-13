using System;
namespace SCERP.Model.Production
{
   public  class VwStanderdMinValDetail
    {
        public long StanderdMinValDetailId { get; set; }
        public long StanderdMinValueId { get; set; }
        public string CompId { get; set; }
        public string StanderdMinValueRefId { get; set; }
        public string SubProcessRefId { get; set; }
        public Nullable<decimal> StMvD { get; set; }
        public string SubProcessName { get; set; }
    }
}
