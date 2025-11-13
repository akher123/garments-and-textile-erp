using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SCERP.BLL.Process
{
    public static class Serializer
    {
        public static T Deserialize<T>(this XElement element) where T : class
        {
            var reader = element.CreateReader();
            var serializer = new XmlSerializer(typeof(T));
            var obj = (T)serializer.Deserialize(reader);
            return obj;
        }
    }
}
