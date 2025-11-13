using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Message.Service.Model
{
    [DataContract]
    public class EmailMessage
    {
        [DataMember]
        public string From { get; set; }
        [DataMember]
        public List<string> To { get; set; }
        [DataMember]
        public List<string> CC { get; set; }
        [DataMember]
        public List<string> BCC { get; set; }
        [DataMember]
        public string Body { get; set; }
        [DataMember]
        public string Subject { get; set; }
    }
}
