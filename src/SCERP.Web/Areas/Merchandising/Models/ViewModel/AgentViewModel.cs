using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class AgentViewModel:OM_Agent
    {
        public IEnumerable Cities { get; set; }
        public List<Country> Countries { get; set; }
        public List<OM_Agent> Agents { get; set; }

        public AgentViewModel()
        {
            Countries=new List<Country>();
            Cities=new List<object>();
            Agents=new List<OM_Agent>();
        }
        public IEnumerable<Dropdown> AgentTypes
        {
            get
            {
                return new[]
                {
                      new Dropdown {Id = "B", Value = "Buyer Agent"},
                      new Dropdown {Id = "S", Value = "Shipping Agent"}
                };
            }
        }

        public IEnumerable<SelectListItem> AgentTypesSelectListItem
        {
            get
            {

                return new SelectList(AgentTypes, "Id", "Value");
            }
        }

        public string GetAgentYepeName(string typeId)
        {
            var agentType= AgentTypes.FirstOrDefault(x => x.Id == typeId);
            return agentType!=null ? agentType.Value : "";
        }

        public List<SelectListItem> CountrySelectListItem
        {
            get { return new SelectList(Countries, "Id", "CountryName").ToList(); }

        }
        public List<SelectListItem> CitySelectListItem
        {
            get { return new SelectList(Cities, "CityId", "CityName").ToList(); }

        }
        
    }
}