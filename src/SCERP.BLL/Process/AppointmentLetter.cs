using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SCERP.BLL.Process
{
    public class AppointmentLetter
    {
        public string EmployeeSpecificInfo { get; set; }

        public static AppointmentLetter Create(string path)
        {
            var email = XElement.Load(path).Deserialize<AppointmentLetter>();
            return email;
        }
    }
}
