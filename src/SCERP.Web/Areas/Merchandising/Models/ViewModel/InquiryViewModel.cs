using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.BLL.Manager.MerchandisingManager;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class InquiryViewModel : ProSearchModel<InquiryViewModel>
    {
        public List<OM_Inquiry> Inquiries { get; set; }
        public OM_Inquiry Inquiry { get; set; }

        public InquiryViewModel()
        {
            this.Inquiries=new List<OM_Inquiry>();
            this.Inquiry=new OM_Inquiry();
        }


    }
}