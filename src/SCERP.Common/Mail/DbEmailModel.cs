using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Common.Mail
{
    public class DbEmailModel
    {
        public string Recipients { get; set; }
        public string CopyRecipients { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string FileAttachments { get; set; }
    }
}
