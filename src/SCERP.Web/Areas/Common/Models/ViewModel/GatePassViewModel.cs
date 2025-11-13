using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model.CommonModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Common.Models.ViewModel
{
    public class GatePassViewModel : ProSearchModel<GatePassViewModel>
    {
        public List<GatePass> GatePasses { get; set; }
        public GatePass GatePass { get; set; }
        public string TypeId { get; set; }
        public GatePassDetail GatePassDetail { get; set; }
        public string Key { get; set; }
        public Dictionary<string,GatePassDetail> GatePassDetails { get; set; }

        public GatePassViewModel()
        {
            GatePasses=new List<GatePass>();
            GatePassDetails=new Dictionary<string, GatePassDetail>();
            GatePass=new GatePass();
            GatePassDetail=new GatePassDetail();
        }
        public IEnumerable<SelectListItem> YarnTypeReceivesSelectListItem
        {
            get
            {
                
                return new SelectList(new List<Dropdown>() { new Dropdown { Id = "Y", Value = "GYD" }, new Dropdown { Id = "1", Value = "GYR" } }, "Id", "Value");
            }
        }
    }
}