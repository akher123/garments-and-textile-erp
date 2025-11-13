using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Optimization;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.CommonModel;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Planning.Models.ViewModels
{
    public class ProgramViewModel : PLAN_Program
    {
     
        public List<VProcessSequence> ProcessSequences { get; set; }
        public List<PROD_Processor> Processors { get; set; }
        public List<Party> Parties { get; set; }
        public List<VwProgram> VPrograms { get; set; }
        public List<PLAN_Program> Programs { get; set; }
        public List<VProgramDetail> InPutProgramDetails { get; set; }
        public List<VProgramDetail> OutPutProgramDetails { get; set; }
        public PLAN_Program Program { get; set; }
        public IEnumerable<VBuyerOrder> BuyerOrders { get; set; }
        public List<VOMBuyOrdStyle> BuyOrdStyles { get; set; }
        public VProgramDetail InputProgramDetail { get; set; }
        public VProgramDetail OutputProgramDetail { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable Styles { get; set; }
        public string GroupCode { get; set; }
        public string SampleItemCode { get; set; }
        public string SampleItemName{ get; set; }
        public string SampleColorRefId{ get; set; }
        public string SampleColorName { get; set; }
        public ProgramViewModel()
        {
            Processors=new List<PROD_Processor>();
            Parties=new List<Party>();
            InputProgramDetail = new VProgramDetail();
            OutputProgramDetail = new VProgramDetail();
            BuyOrdStyles=new List<VOMBuyOrdStyle>();
            Programs=new List<PLAN_Program>();
            VPrograms = new List<VwProgram>();
            ProcessSequences=new List<VProcessSequence>();
            InPutProgramDetails = new List<VProgramDetail>();
            OutPutProgramDetails = new List<VProgramDetail>();
            Program=new PLAN_Program();
            Buyers = new List<OM_Buyer>();
            OrderList = new List<object>();
            Styles = new List<object>();
            InputDataTable=new Dictionary<string, VProgramDetail>();
            OutputDataTable=new Dictionary<string, VProgramDetail>();
            GroupCode = "10";
            Dropdowns = new List<Dropdown>();
        }

        public Dictionary<string, VProgramDetail> InputDataTable { get; set; }
        public Dictionary<string, VProgramDetail> OutputDataTable { get; set; }
        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(Buyers, "BuyerRefId", "BuyerName");
            }
        }
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
                return new SelectList(Styles, "OrderStyleRefId", "StyleName");
            }
        }
        public IEnumerable<SelectListItem> ProcessesSeasonsSelectListItem
        {
            get
            {
                return new SelectList(ProcessSequences, "ProcessRefId", "ProcessName");
            }
        }

        public IEnumerable<SelectListItem> ProcessorSelectListItem
        {
            get
            {
                return new SelectList(Processors, "ProcessorRefId", "ProcessorName");
            }
        }
        public List<Dropdown> Dropdowns { get; set; }
        public IEnumerable<SelectListItem> FabricsSelectListItem
        {
            get
            {
                return new SelectList(Dropdowns, "Id", "Value");
            }
        }
        public IEnumerable<SelectListItem> ProgramTypeSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { Value = "BP", Text = "BULK PROGRAM" }, new { Value = "SP", Text = "SAMPLE PROGRAM" } }, "Value", "Text");
            }
        }
        public IEnumerable<SelectListItem> PartyTypeSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { Value = "IN", Text = "INTERNAL" }, new { Value = "EX", Text = "EXTERNAL" } }, "Value", "Text");
            }
        }
        public IEnumerable<SelectListItem> PartySelectListItem
        {
            get
            {
                return new SelectList(Parties, "PartyId", "Name");
            }
        }
        public IEnumerable<SelectListItem> ApprovedKnittingProgramSelectListItem
        {
            get 
            {
                return new SelectList(new[] { new { Text = "Approved", Value = true }, new { Text = "Pending", Value = false}}, "Value", "Text");
            }
        }
        public IEnumerable<SelectListItem> LockedProgramSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { Text = "Loked", Value = true }, new { Text = "UnLocked", Value = false } }, "Value", "Text");
            }
        }
        public IEnumerable<SelectListItem> ProcessSelectListItem
        {
            get
            {
                return new SelectList(new List<PLAN_Process>()
                {
                     new PLAN_Process(){ProcessRefId = ProcessType.KNITTING,ProcessName="KNITTING"},
                    new PLAN_Process(){ProcessRefId = ProcessType.COLLARCUFF,ProcessName="COLLARCUFF"},
                   
                }, "ProcessRefId", "ProcessName");
            }

        }
    }
}