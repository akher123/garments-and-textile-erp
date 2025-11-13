using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCERP.Web.Models
{
    public class Chart
    {
        public dynamic[] labels { get; set; }
        public List<Datasets> datasets { get; set; }
    }
    public class Datasets
    {
        public object label { get; set; }
        public object backgroundColor { get; set; }
        public object borderColor { get; set; }
        public string borderWidth { get; set; }
        public object data { get; set; }
        public bool fill { get; set; }
    } 
}