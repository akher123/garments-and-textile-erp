using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class ProcessorViewModel:PROD_Processor
    {
        public List<PLAN_Process> Processes { get; set; }
        public List<VProcessor> Processors { get; set; }
        public ProcessorViewModel()
        {
            Processes=new List<PLAN_Process>();
            Processors = new List<VProcessor>();
        }
        public IEnumerable<SelectListItem> ProcessesSeasonsSelectListItem
        {
            get
            {
                return new SelectList(Processes, "ProcessRefId", "ProcessName");
            }
        }
    }
}