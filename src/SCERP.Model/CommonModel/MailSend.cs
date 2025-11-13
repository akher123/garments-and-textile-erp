using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommonModel
{
    public class MailSend
    {
        public int MailSendId { get; set; }
        [Required]
        public Nullable<int> ModuleId { get; set; }
        [Required]
        public string ReportName { get; set; }
        [Required]
        public string FileName { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        [Required]
        public string MailAddress { get; set; }
        public string PersonName { get; set; }
        public string MailType { get; set; }
        public string Profile { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string CompId { get; set; }
    }
}
