using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Planning.Models.ViewModels
{
    public class PlanningReportViewModel
    {
        public List<PLAN_Process> Processes { get; set; }
        public PlanningReportViewModel()
        {
            DataTable=new DataTable();
            Processes = new List<PLAN_Process>();
           
        }

        public string ProcessRefId { get; set; }
        public DataTable DataTable { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }
        public bool IsShowReport { get; set; }

        public IEnumerable<SelectListItem> ProcessSeasonsSelectListItem
        {
            get
            {
                return new SelectList(Processes, "ProcessRefId", "ProcessName");
            }
        }
    }
}