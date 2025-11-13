using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class ProductionViewModel : PROD_Production
    {
        public List<VwProgram> VwPrograms { get; set; }
        public PROD_Production Production { get; set; }
        public List<VwProduction> InProdProductions { get; set; }
        public List<VwProduction> OutProdProductions { get; set; }
        public List<VProductionDetail> ProductionDetails { get; set; }
        public List<PLAN_Program> Programs { get; set; }
        public List<PROD_Processor> Processors { get; set; }
        public List<Production_Machine> Machines { get; set; }
        public long ProgramDetailId { get; set; }
        public List<VProgramDetail> VProgramDetails { get; set; }
        public ProductionViewModel()
        {
            VwPrograms=new List<VwProgram>();
            InProdProductions = new List<VwProduction>();
            OutProdProductions = new List<VwProduction>();
            Programs=new List<PLAN_Program>();
            Production=new PROD_Production();
            Machines = new List<Production_Machine>();
            Processors = new List<PROD_Processor>();
            ProductionDetails = new List<VProductionDetail>();
            VProgramDetails=new List<VProgramDetail>();
        
        }
        public IEnumerable<SelectListItem> ProgramSelectListItem
        {
            get
            {
                return new SelectList(Programs, "ProgramRefId", "ProgramRefId");
            }
        }
        public IEnumerable<SelectListItem> ProcessorsSelectListItem
        {
            get
            {
                return new SelectList(Processors, "ProcessorRefId", "ProcessorName");
            }
        }
        public IEnumerable<SelectListItem> MachinesSelectListItem
        {
            get
            {
                return new SelectList(Machines, "MachineRefId", "Name");
            }
        }

        public IEnumerable<SelectListItem> ProgramDetailsSelectListItem
        {
            get
            {
                return new SelectList(VProgramDetails, "ProgramDetailId", "CompositName");
            }
        }
    }
}