using System.Collections.Generic;
using SCERP.Model.CommonModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Common.Models.ViewModel
{
    public class EmailUserViewModel : ProSearchModel<EmailUserViewModel>
    {
        public List<EmailUser> EmailUsers { get; set; }
        public EmailUser EmailUser { get; set; }

        public EmailUserViewModel()
        {
            this.EmailUser=new EmailUser();
            this.EmailUsers=new List<EmailUser>();
        }
    }
}