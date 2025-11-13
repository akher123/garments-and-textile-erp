using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SCERP.Web.Models
{
    public class DeviceInformation
    {
        public DeviceInformation()
        {
            // Use the DefaultValue propety of each property to actually set it, via reflection.
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
            {
                DefaultValueAttribute attr = (DefaultValueAttribute)prop.Attributes[typeof(DefaultValueAttribute)];
                if (attr != null)
                {
                    prop.SetValue(this, attr.Value);
                }
            }
        }

        [DefaultValue(2)]
        public int OutputFormat { get; set; }
        [DefaultValue(8.3)]
        public double PageWidth { get; set; }
        [DefaultValue(11.7)]
        public double PageHeight { get; set; }
        [DefaultValue(0.2)]
        public double MarginTop { get; set; }
        [DefaultValue(0.2)]
        public double MarginLeft { get; set; }
        [DefaultValue(0.2)]
        public double MarginRight { get; set; }
        [DefaultValue(0.2)]
        public double MarginBottom { get; set; }

    }
}