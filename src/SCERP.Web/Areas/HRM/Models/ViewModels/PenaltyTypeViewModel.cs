
using System.Collections.Generic;
using iTextSharp.text;
using SCERP.Model.HRMModel;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class PenaltyTypeViewModel:HrmPenaltyType
    {
        public List<HrmPenaltyType> PenaltyTypes { get; set; }
        public PenaltyTypeViewModel()
        {
            PenaltyTypes=new List<HrmPenaltyType>();
        }
    }
}