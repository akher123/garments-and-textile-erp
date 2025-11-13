using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.CommonModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Common.Models.ViewModel
{
    public class MailSendViewModel : ProSearchModel<MailSend>
    {
        public MailSendViewModel()
        {
            MailSend=new MailSend();
            MailSendList = new List<VwMailSend>();
            Modules=new List<Module>();
            
        }
        public string MailAddress2 { get; set; }
        public string MailAddress3 { get; set; }
        public string PersonName2 { get; set; }
        public string PersonName3 { get; set; }
        public MailSend MailSend { get; set; }
        public List<VwMailSend> MailSendList { get; set; }
        public List<Module> Modules { get; set; }
        public IEnumerable<SelectListItem> ModuleSelectListItems
        {
            get
            {
                return new SelectList(Modules, "Id", "ModuleName");
            }
        }
    }
}