using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class MachineViewModel:Production_Machine
    {
        public List<VMachine> Machines { get; set; }
        public List<PROD_Processor> Processors { get; set; }
        public MachineViewModel()
        {
            Processors=new List<PROD_Processor>();
            Machines = new List<VMachine>();
        }
        public IEnumerable<SelectListItem> ProcessorsSeasonsSelectListItem
        {
            get
            {
                return new SelectList(Processors, "ProcessorRefId", "ProcessorName");
            }
        }
    }
}