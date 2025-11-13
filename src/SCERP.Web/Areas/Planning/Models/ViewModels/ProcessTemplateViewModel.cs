using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Planning;

namespace SCERP.Web.Areas.Planning.Models.ViewModels
{
    public class ProcessTemplateViewModel : PLAN_ProcessTemplate
    {
        public ProcessTemplateViewModel()
        {
            ProcessTemplate = new List<PLAN_ProcessTemplate>();
            IsSearch = true;
        }

        public List<PLAN_ProcessTemplate> ProcessTemplate { get; set; }

        public List<OM_Buyer> Buyers { get; set; }

        public List<SelectListItem> BuyerSelectListItem
        {
            get { return new SelectList(Buyers, "BuyerRefId", "BuyerName").ToList(); }
        }

        public List<OM_BuyerOrder> BuyerOrders { get; set; }

        public List<SelectListItem> BuyerOrderSelectListItem
        {
            get { return new SelectList(BuyerOrders, "OrderNo", "OrderNo").ToList(); }
        }


        public List<OM_Style> Styles { get; set; }

        public List<SelectListItem> StyleSelectListItem
        {
            get { return new SelectList(Styles, "Id", "ProcessName").ToList(); }
        }

        public List<PLAN_Process> Processes { get; set; }

        public List<SelectListItem> ProcessSelectListItem
        {
            get { return new SelectList(Processes, "ProcessId", "ProcessName").ToList(); }
        }

        public List<PLAN_ResponsiblePerson> ResponsiblePersons { get; set; }

        public List<SelectListItem> ResponsiblePersonSelectListItem
        {
            get { return new SelectList(ResponsiblePersons, "ResponsiblePersonId", "ResponsiblePersonName").ToList(); }
        }

        public int SearchByBuyerId
        {
            get;
            set;
        }

        public string SearchByBuyerOrderId
        {
            get;
            set;
        }

        public int SearchByProcessId
        {
            get;
            set;
        }

        public string SearchByStyleId
        {
            get;
            set;
        }
    }
}