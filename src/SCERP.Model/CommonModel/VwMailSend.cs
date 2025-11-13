using System;
    

namespace SCERP.Model.CommonModel
{
    
    public partial class VwMailSend
    {
        public int MailSendId { get; set; }
        public Nullable<int> ModuleId { get; set; }
        public string ReportName { get; set; }
        public string FileName { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public string MailAddress { get; set; }
        public string PersonName { get; set; }
        public string MailType { get; set; }
        public string Profile { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string CompId { get; set; }
        public string ModuleName { get; set; }
    }
}
