using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.Common.Models.ViewModel
{
    public class MeasurementUnitViewModel:MeasurementUnit
    {
        public List<MeasurementUnit> MeasurementUnits { get; set; }

        public MeasurementUnitViewModel()
        {
            this.MeasurementUnits=new List<MeasurementUnit>();
        }

    }
}