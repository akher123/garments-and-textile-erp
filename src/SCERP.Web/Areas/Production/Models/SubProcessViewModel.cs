using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class SubProcessViewModel : PROD_SubProcess
    {
        public List<PLAN_Process> Processes { get; set; }
      //  public List<PROD_SubProcess> SubProcessors { get; set; }
        public List<VSubProcess> SubProcesses { get; set; }
        public SubProcessViewModel()
        {
            Processes = new List<PLAN_Process>();
            SubProcesses = new List<VSubProcess>();
            
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