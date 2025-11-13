using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.Planning;

namespace SCERP.Web.Areas.Planning.Models.ViewModels
{
    public class ProcessViewModel:PLAN_Process
    {
        public List<PLAN_Process> Processes { get; set; } 
        public ProcessViewModel()
        {
            Processes=new List<PLAN_Process>();
        }
    }
}