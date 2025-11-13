using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCERP.Web.Helpers
{
    public class GnettSource
    {
        public GnettSource()
        {
            values = new List<GenttValue>();
        }
        public string name;
        public int id;
        public string desc;
        public List<GenttValue> values;

    }
}