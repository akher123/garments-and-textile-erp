using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Planning;
using SCERP.Model.Production;
using SCERP.Web.Helpers;

namespace SCERP.Web.Areas.Planning.Models.ViewModels
{
    public class TargetProductionViewModel : ProSearchModel<TargetProductionViewModel>
    {

        public List<GnettSource> GnettSources { get; set; }
        public PLAN_TargetProduction PlanTargetProduction { get; set; }
        public List<VwAssignedProgram> VwPrograms { get; set; }
        public List<VwTargetProduction> TargetProductions { get; set; }
        public List<PLAN_TargetProductionDetail> TargetProductionDetails { get; set; }
        public List<Production_Machine> Machines { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public bool  IsPartial { get; set; }

        public TargetProductionViewModel()
        {
            VwPrograms = new List<VwAssignedProgram>();
            TargetProductions = new List<VwTargetProduction>();
            TargetProductionDetails=new List<PLAN_TargetProductionDetail>();
            PlanTargetProduction=new PLAN_TargetProduction();
            Machines=new List<Production_Machine>();
            Buyers=new List<OM_Buyer>();
            OrderList = new List<OM_BuyerOrder>();
            BuyerOrderStyles = new List<OM_BuyOrdStyle>();
            GnettSources=new List<GnettSource>();
        }
        public MonthEnum MonthId { get; set; }
        public int YearId { get; set; }
        public IEnumerable<SelectListItem> YearSelectListItem
        {
            get
            {
                return new SelectList(Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 10));
            }
        }
        public IEnumerable OrderList { get; set; }
        public IEnumerable BuyerOrderStyles { get; set; }
        public List<SelectListItem> BuyerSelectListItem
        {
            get { return new SelectList(Buyers, "BuyerRefId", "BuyerName").ToList(); }
        }

        public List<OM_BuyerOrder> BuyerOrders { get; set; }

        public IEnumerable<SelectListItem> OrderSelectListItem
        {
            get
            {
                return new SelectList(OrderList, "OrderNo", "RefNo");
            }
        }
        public IEnumerable<SelectListItem> StylesSelectListItem
        {
            get
            {
                return new SelectList(BuyerOrderStyles, "OrderStyleRefId", "StyleName");
            }
        }
    
     
        public IEnumerable<SelectListItem>LineSeasonsSelectListItem
        {
            get
            {
                return new SelectList(Machines, "MachineId", "Name", PlanTargetProduction.LineId);
            }
        }
    }
}