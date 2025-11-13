using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommonModel
{
    public class MessagingNotification
    {
        public long MessageId { get; set; }
        public string MessageText { get; set; }
        public string Sender{ get; set; }
        public string ReceiverId { get; set; }
        public long? IsViewed { get; set; }
        public string PhotographPath { get; set; }
    }
}
