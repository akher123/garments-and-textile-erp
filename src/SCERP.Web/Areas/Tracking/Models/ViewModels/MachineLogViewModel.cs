using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.TrackingModel;

namespace SCERP.Web.Areas.Tracking.Models.ViewModels
{
    public class MachineLogViewModel : TrackMachineLog
    {
        public List<TrackMachineLog> MachineLogs { get; set; }
        public TrackMachineLog MachineLog { get; set; }
        public List<Production_Machine> ProductionMachines { get; set; }
        public string EmployeeName { get; set; }
        public string MachineName { get; set; }
        public List<TrackMachineAction> MachineActionList { get; set; }
        public MachineLogViewModel()
        {
            MachineLogs = new List<TrackMachineLog>();
            MachineLog = new TrackMachineLog();
            ProductionMachines = new List<Production_Machine>();
            MachineActionList=new List<TrackMachineAction>();
        }
        public IEnumerable<SelectListItem> ProductionMachineSelectListItem
        {
            get
            {
                return new SelectList(ProductionMachines, "MachineId", "Name");
            }
        }
        public IEnumerable<SelectListItem> MachineActionListSelectListItem
        {
            get
            {
                return new SelectList(MachineActionList, "MachineActionId", "MachineActionName");
            }
        }
    }
}