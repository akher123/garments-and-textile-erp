using System.Xml.Linq;

namespace SCERP.BLL.Process
{
    public class Email
    {
        public string Subject { get; set; }
        public string Body { get; set; }
  
        public static Email Create(string path)
        {
            var email = XElement.Load(path).Deserialize<Email>();
            return email;
        }
    }
}
